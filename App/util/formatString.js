export const formatarTelefone = (v) => {
  if (!v) {
    return ''
  }
  let r = v.replace(/\D/g, '')
  r = r.replace(/^0/, '')

  if (r.length >= 11) {
    r = r.replace(/^(\d{2})(\d)(\d{4})(\d{4}).*/, '($1) $2 $3-$4')
  }
  return r
}

export function formatarCpfOuCnpj (doc) {
  if (!doc) {
    return ''
  }
  if (doc.length === 11) {
    return formatarCpfPessoa(doc)
  } else if (doc.length === 14) {
    return formatarCnpjPessoa(doc)
  }

  return doc
}
export function formatarCpfPessoa (cpf) {
  // Cria pattern para o seguinte formato: 111.111.111-11
  const regex = new RegExp('([0-9]{3}).([0-9]{3}).([0-9]{3})-([0-9]{2})')
  if (!cpf) {
    return '--'
  }
  // Verifica se o valor já não está com a máscara
  if (!regex.test(cpf)) {
    return cpf.replace(/([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})/, '$1.$2.$3-$4')
  }
  return cpf
}
export function testarCpfOuCnpj (texto) {
  if (!texto) {
    return false
  }
  if (texto.length === 11) {
    return testarCpfPessoa(texto)
  } else if (texto.length === 14) {
    return testarCnpjPessoa(texto)
  } else {
    return false
  }
}
export function testarCpfPessoa (strCPF) {
  let soma
  let resto
  soma = 0
  if (strCPF === '00000000000') { return false }
  for (let i = 1; i <= 9; i++) { soma = soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i) }
  resto = (soma * 10) % 11
  if ((resto === 10) || (resto === 11)) { resto = 0 }
  if (resto !== parseInt(strCPF.substring(9, 10))) { return false }
  soma = 0
  for (let i = 1; i <= 10; i++) { soma = soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i) }
  resto = (soma * 10) % 11
  if ((resto === 10) || (resto === 11)) { resto = 0 }
  if (resto !== parseInt(strCPF.substring(10, 11))) { return false }
  return true
}
function testarCnpjPessoa (cnpj) {
  cnpj = cnpj.replace(/[^\d]+/g, '')
  if (cnpj === '') { return false }
  if (cnpj.length !== 14) {
    return false
  }
  // Elimina CNPJs invalidos conhecidos
  if (cnpj === '00000000000000' ||
    cnpj === '11111111111111' ||
    cnpj === '22222222222222' ||
    cnpj === '33333333333333' ||
    cnpj === '44444444444444' ||
    cnpj === '55555555555555' ||
    cnpj === '66666666666666' ||
    cnpj === '77777777777777' ||
    cnpj === '88888888888888' ||
    cnpj === '99999999999999') {
    return false
  }
  // Valida DVs
  let tamanho = cnpj.length - 2
  let numeros = cnpj.substring(0, tamanho)
  const digitos = cnpj.substring(tamanho)
  let soma = 0
  let pos = tamanho - 7
  for (let i = tamanho; i >= 1; i--) {
    soma += parseInt(numeros.charAt(tamanho - i)) * pos--
    if (pos < 2) {
      pos = 9
    }
  }
  let resultado = soma % 11 < 2 ? 0 : 11 - soma % 11
  if (resultado !== parseInt(digitos.charAt(0))) {
    return false
  }
  tamanho = tamanho + 1
  numeros = cnpj.substring(0, tamanho)
  soma = 0
  pos = tamanho - 7
  for (let i = tamanho; i >= 1; i--) {
    soma += parseInt(numeros.charAt(tamanho - i)) * pos--
    if (pos < 2) {
      pos = 9
    }
  }
  resultado = soma % 11 < 2 ? 0 : 11 - soma % 11
  if (resultado !== parseInt(digitos.charAt(1))) {
    return false
  }
  return true
}
export function formatarCnpjPessoa (cnpj) {
  // Cria pattern para o seguinte formato: 11.111.111\1111-11
  const regex = new RegExp('([0-9]{2}).([0-9]{3}).([0-9]{3})V([0-9]{4})-([0-9]{2})')
  if (!cnpj) {
    return '--'
  }
  // Verifica se o valor já não está com a máscara
  if (!regex.test(cnpj)) {
    return cnpj.replace(/([0-9]{2})([0-9]{3})([0-9]{3})([0-9]{4})([0-9{2}])/, '$1.$2.$3/$4-$5')
  }
  return cnpj
}
export function formatarCepEndereco (cep) {
  // Cria pattern para o seguinte formato: 11111-111
  const regex = new RegExp('([0-9]{5})-([0-9]{3})')
  // Verifica se o valor já não está com a máscara
  if (!regex.test(cep)) {
    return cep.replace(/([0-9]{5})([0-9]{3})/, '$1-$2')
  }
  return cep
}
export function formatCurrency (num) {
  return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')
}
