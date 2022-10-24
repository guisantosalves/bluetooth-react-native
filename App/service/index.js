import AsyncStorage from '@react-native-async-storage/async-storage'
import LogDAO from '../db/LogDao'
import { PesoService } from './peso'

export const sincronizacaoGeral = async () => {
  try {

    // Pedidos
      await PesoService.Sincronizar()

    // Logs
    // await LogService.Sincronizar()
  } catch (error) {
    LogDAO.GravarLog(error.message)
  }
}
