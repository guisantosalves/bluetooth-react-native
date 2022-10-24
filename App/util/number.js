export class NumberUtil {
  static toLocaleStringSupportsOptions () {
    return !!(typeof Intl === 'object' &&
      Intl &&
      typeof Intl.NumberFormat === 'function')
  }

  // Soma um array de numeros
  static mapSum (numbers) {
    if (numbers.length < 1) { return 0 }
    return numbers.reduce((prev, curr) => {
      if (curr === null || curr === undefined || isNaN(curr)) { return 0 }
      return prev + curr
    }, 0)
  }

  // Retorna um valor numérico
  static mapNumber (numero) {
    if (numero === undefined || numero === null) {
      return 0
    }
    if (typeof numero === 'string') {
      return parseFloat(numero)
    }
    return numero
  }

  /**
   * Formata um valor numérico para número decimal com 2 casas
   * @param param valor numérico
   */
  static toDisplayNumber (valor = 0, unidade = '', prefix = false, casasDecimais = 2) {
    let result = valor.toFixed(casasDecimais) // casas decimais
      .replace('.', ',')
      .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')

    if (unidade) {
      if (prefix) {
        result = `${unidade} ${result}`
      } else {
        result = `${result} ${unidade}`
      }
    }

    return result
  }

  /**
   * Formata um valor numérico para número decimal com 2 casas
   * @param param valor numérico
   */
  static toDisplayNumberLeft (valor, unidade = '') {
    const result = valor.toFixed(2) // casas decimais
      .replace('.', ',')
      .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')

    if (!unidade) {
      return result
    }

    return `${unidade} ${result}`
  }

  static appendTextInputToNumber (inputValue, decimalPlaces = 2) {
    inputValue = !inputValue || inputValue == null ? '0' : inputValue
    inputValue = inputValue.replace(/[^\d]/g, '')
    if (inputValue === '') { return 0 }
    while (inputValue.length < decimalPlaces + 1) {
      inputValue = `0${inputValue}`
    }
    inputValue = `${inputValue.substring(0, inputValue.length - decimalPlaces)}.${inputValue.substring(inputValue.length - decimalPlaces)}`
    return Number(inputValue)
  }
}
