import { subDays } from 'date-fns'
import * as FileSystem from 'expo-file-system'
import * as Sharing from 'expo-sharing'
import moment from 'moment'
import { Alert } from 'react-native'

export const BACKUPS_DIR = FileSystem.documentDirectory + '/backups_de_pedidos'

export class BackupControl {
  static async VerificarExistenciaDiretorio () {
    const exists = await FileSystem.getInfoAsync(BACKUPS_DIR)

    if (!exists.exists) {
      await FileSystem.makeDirectoryAsync(BACKUPS_DIR, { intermediates: true })
    }
  }

  static async SetPedido (obj) {
    await this.VerificarExistenciaDiretorio()

    const nomeArquivo = `${((obj.cliente.apelido || obj.cliente.nomeRazao).normalize('NFD').replace(/[\u0300-\u036f/'"´`]/g, '')).replace(/\s/g, '_')}_${moment(obj.dataEHora).format('DD-MM-YYYY')}_${moment(obj.dataEHora).format('HH-mm')}`
    await FileSystem.writeAsStringAsync(BACKUPS_DIR + `/${nomeArquivo}_Pedido.json`, JSON.stringify(obj), { encoding: FileSystem.EncodingType.UTF8 })
  }

  static async SetCompra (obj) {
    await this.VerificarExistenciaDiretorio()

    const nomeArquivo = `${((obj.fornecedor.apelido || obj.fornecedor.nomeRazao).normalize('NFD').replace(/[\u0300-\u036f/'"´`]/g, '')).replace(/\s/g, '_')}_${moment(obj.dataEHora).format('DD-MM-YYYY')}_${moment(obj.dataEHora).format('HH-mm')}`
    await FileSystem.writeAsStringAsync(BACKUPS_DIR + `/${nomeArquivo}_Compra.json`, JSON.stringify(obj), { encoding: FileSystem.EncodingType.UTF8 })
  }

  static async GetAllBackupNames () {
    await this.VerificarExistenciaDiretorio()

    return await FileSystem.readDirectoryAsync(BACKUPS_DIR)
  }

  /**
   * Obtém os backups
   * @param keys lista de chaves para retornar os pedidos, caso seja vazia vem todos
   */
  static async GetBackups (listNames) {
    await this.VerificarExistenciaDiretorio()

    const arquivosPromise = listNames.map(async item => {
      const json = await FileSystem.readAsStringAsync(`${BACKUPS_DIR}/${item}`)
      return JSON.parse(json)
    })

    return await Promise.all(arquivosPromise)
  }

  static async RemoveOldBackups () {
    await this.VerificarExistenciaDiretorio()

    try {
      // Pega as chaves
      const allKeys = await this.GetAllBackupNames()

      // Busca a informação de cada arquivo
      const promises = allKeys.map(async item => {
        const { modificationTime } = await FileSystem.getInfoAsync(`${BACKUPS_DIR}/${item}`)

        const dataModificacao = new Date(modificationTime * 1000)
        const dataComparacao = subDays(new Date(), 60)

        if (dataModificacao < dataComparacao) {
          await FileSystem.deleteAsync(`${BACKUPS_DIR}/${item}`)
        }
      })

      await Promise.all(promises)
    } catch (error) {
      Alert.alert('Erro', `Ocorreu um erro na rotina de exclusão de backups antigos! ${error}`)
    }
  }

  static async RemoveAllBackups () {
    await this.VerificarExistenciaDiretorio()

    // Pega as chaves
    const allKeys = await this.GetAllBackupNames()

    // Remove tudo
    const deletePromises = allKeys.map(async key => await FileSystem.deleteAsync(`${BACKUPS_DIR}/${key}`))

    await Promise.all(deletePromises)
  }

  static async ShareAllBackups () {
    await this.VerificarExistenciaDiretorio()

    if (!Sharing.isAvailableAsync()) {
      throw new Error('Seu dispositivo não suporta o compartilhamento dos backups!')
    }

    // Define caminho do backup a gerar
    const backupPath = `${BACKUPS_DIR}/backup.json`

    // Obtém uma lista com TODOS os IDs de backup
    const contents = await BackupControl.GetBackups(await BackupControl.GetAllBackupNames())

    // Gera um único arquivo .json com os IDs
    await FileSystem.writeAsStringAsync(backupPath, JSON.stringify(contents), { encoding: FileSystem.EncodingType.UTF8 })

    // Usa a função share para compartilhar para qualquer lugar
    await Sharing.shareAsync(backupPath, { dialogTitle: 'Compartilhe seu backup', mimeType: 'application/json', UTI: 'application/json' })

    // Deleta o arquivo de backup em seguida
    await FileSystem.deleteAsync(backupPath)
  }
}
