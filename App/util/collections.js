export const removeDuplicatesFromList = (collection, uniqueIdentifier) => {
  const props = [...new Set(collection.map(item => item[uniqueIdentifier]))]
  const listReturn = props.map(item => collection.find(q => item === q[uniqueIdentifier]))

  return listReturn
}
