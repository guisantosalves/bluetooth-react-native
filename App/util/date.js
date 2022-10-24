import { endOfDay, endOfMonth, endOfWeek, endOfYear, startOfDay, startOfMonth, startOfWeek, startOfYear, sub } from 'date-fns'

export const PeriodList = [
  {
    label: 'Hoje',
    value: 0
  },
  {
    label: 'Ontem',
    value: 1
  },
  {
    label: 'Semana atual',
    value: 2
  },
  {
    label: 'Semana passada',
    value: 3
  },
  {
    label: 'Mês atual',
    value: 4
  },
  {
    label: 'Mês passado',
    value: 5
  },
  {
    label: 'Ano atual',
    value: 6
  },
  {
    label: 'Ano passado',
    value: 7
  }
]

export const getPeriodLabel = value => {
  switch (value) {
    case PeriodList[0].value:
      return 'de hoje'
    case PeriodList[1].value:
      return 'de ontem'
    case PeriodList[2].value:
      return 'nesta semana'
    case PeriodList[3].value:
      return 'na semana passada'
    case PeriodList[4].value:
      return 'neste mês'
    case PeriodList[5].value:
      return 'no mês passado'
    case PeriodList[6].value:
      return 'neste ano'
    case PeriodList[7].value:
      return 'no ano passado'
    default:
      return null
  }
}

export const getPeriodValues = value => {
  const data = new Date()

  switch (value) {
    case PeriodList[0].value:
      return [startOfDay(data), endOfDay(data)]
    case PeriodList[1].value:
      return [startOfDay(sub(data, { days: 1 })), endOfDay(sub(data, { days: 1 }))]
    case PeriodList[2].value:
      return [startOfWeek(data), endOfWeek(data)]
    case PeriodList[3].value:
      return [startOfWeek(sub(data, { weeks: 1 })), endOfWeek(sub(data, { weeks: 1 }))]
    case PeriodList[4].value:
      return [startOfMonth(data), endOfMonth(data)]
    case PeriodList[5].value:
      return [startOfMonth(sub(data, { months: 1 })), endOfMonth(sub(data, { months: 1 }))]
    case PeriodList[6].value:
      return [startOfYear(data), endOfYear(data)]
    case PeriodList[7].value:
      return [startOfYear(sub(data, { years: 1 })), endOfYear(sub(data, { years: 1 }))]
    default:
      return null
  }
}

export const formatTimeByOffset = (dateString, offset) => {
  // Params:
  // How the backend sends me a timestamp
  // dateString: on the form yyyy-mm-dd hh:mm:ss
  // offset: the amount of hours to add.

  // If we pass anything falsy return empty string
  if (!dateString) return ''
  if (dateString.length === 0) return ''

  // Step 1: Parse the backend date string

  // Get Parameters needed to create a new date object
  const year = dateString.slice(0, 4)
  const month = dateString.slice(5, 7)
  const day = dateString.slice(8, 10)
  const hour = dateString.slice(11, 13)
  const minute = dateString.slice(14, 16)
  const second = dateString.slice(17, 19)

  // Step: 2 Make a JS date object with the data
  const dateObject = new Date(`${year}-${month}-${day}T${hour}:${minute}:${second}`)

  // Step 3: Get the current hours from the object
  const currentHours = dateObject.getHours()

  // Step 4: Add the offset to the date object
  dateObject.setHours(currentHours + offset)

  return dateObject
}
