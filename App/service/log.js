import LogDAO from '../db/LogDao'
import { getApiUrl, getRequestOptions } from '../util/fetch'

export class LogService {
  static async Sincronizar () {
    // Insere na API registros novos :)
    const registrosAdicionados = await LogDAO.GetList()

    if (registrosAdicionados.length) {
      // Chama a API e ela retornará um array com os ids que não existem lá (logo foram removidos)
      const responseAdicionados = await fetch(`${await getApiUrl()}/Logs/SalvarEmLote`, await getRequestOptions('POST', true, registrosAdicionados))

      // Trata a response de inserção
      if (!responseAdicionados.ok) {
        await LogDAO.GravarLog(await responseAdicionados.text())
        throw new Error(`Ocorreu um erro ${responseAdicionados.status} ao sincronizar os logs no passo 1`)
      }

      // Remove os registros inseridos em lote para obter mais a frente o registro com id
      await LogDAO.RemoveAll()
    }
  }

  static async RemoverTodos () {
    await LogDAO.RemoveAll()
  }
}
