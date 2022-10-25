import AsyncStorage from '@react-native-async-storage/async-storage'
import { Alert } from 'react-native'

export const getRequestOptions = async (method = 'GET', authorized = true, body = null) => {
  let headers = { 'Content-Type': 'application/json' }

  if (authorized) {
    // Pega a data em que o token foi incluso
    const dataToken = new Date(await AsyncStorage.getItem('DataToken'))
    // N° de segundos de 9 dias atrás
    const segundosHa9Dias = (((60 * 60) * 24) * 9) * 1000
    // Cria a data de 9 dias atrás para comparação de datas
    const dataHa9Dias = new Date(Date.now() - segundosHa9Dias)

    // Se a data do token for menor ou igual a essa faremos o refresh Token
    if (dataToken <= dataHa9Dias) {
      await refreshToken()
    }

    const token = await AsyncStorage.getItem('Authorization')

    headers = { ...headers, Authorization: `Bearer ${token}` }
  } else {
    headers = { ...headers, Alpha: '0EACD71FD4BE0008C5658DACC419F5386ABDF5D34A3B2468B1906AAEC2B14F9D' }
  }

  const options = {
    method,
    headers
  }

  if (body) {
    options.body = JSON.stringify(body)
  }

  return options
}

export const refreshToken = async () => {
  const expiredToken = await AsyncStorage.getItem('Authorization')
  const refreshToken = await AsyncStorage.getItem('RefreshToken')

  if (!expiredToken) {
    return
  }

  const response = await fetch(`${await getApiUrl()}/Auth/Token/Refresh`, await getRequestOptions('POST', false, { expiredToken, refreshToken }))

  if (!response.ok) {
    Alert.alert('Erro', 'Ocorreu um erro ao renovar as credenciais, faça login novamente!')
    return
  }

  const body = await response.json()

  await AsyncStorage.setItem('DataToken', new Date().toJSON())
  await AsyncStorage.setItem('Authorization', body.token)
  await AsyncStorage.setItem('RefreshToken', body.refreshToken)
  throw new Error('Credenciais renovadas', 'Suas credenciais foram renovadas!')
}

export const removerCredenciais = async () => {
  await AsyncStorage.removeItem('DataToken')
  await AsyncStorage.removeItem('Authorization')
  await AsyncStorage.removeItem('RefreshToken')
  await AsyncStorage.removeItem('UserInfo')
  await AsyncStorage.removeItem('SubEmpresaId')
  await AsyncStorage.removeItem('Sincronizar')
  await AsyncStorage.removeItem('DispositivoLiberado')
  await AsyncStorage.removeItem('Configuracoes')
}

export const incluirCredenciais = async (token, refreshToken, userInfo, subEmpresaId, sincronizar, dispositivoLiberado) => {
  await AsyncStorage.setItem('DataToken', new Date().toJSON())
  await AsyncStorage.setItem('Authorization', token)
  await AsyncStorage.setItem('RefreshToken', refreshToken)
  await AsyncStorage.setItem('UserInfo', userInfo)
  await AsyncStorage.setItem('SubEmpresaId', subEmpresaId)
  await AsyncStorage.setItem('Sincronizar', sincronizar.toString())
  await AsyncStorage.setItem('DispositivoLiberado', dispositivoLiberado.toString())
}

export const defaultFetcher = async (url, method, body) => fetch(await getApiUrl() + url, await getRequestOptions(method, true, body)).then(r => r.json())

export const getApiUrl = async () => {
  const homologacao = await AsyncStorage.getItem('Homologacao')

  if (homologacao === 'true') {
    return 'https://vendas-mobile-api.alphasoftware.com.br/api'
  }

  return 'https://venum-api-gb7pu.ondigitalocean.app/api'
}

export const extractApiErrors = error => {
  const keys = Object.keys(error)

  return keys.map(key => error[key]).join(' | ')
}
