import * as React from 'react'
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native'

// icon
import Icon from 'react-native-vector-icons/FontAwesome';

const User = () => {
  return (
    <View style={style.container}>

      <View style={style.containerHead}>
        <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
          <Text style={{marginRight: 10, fontWeight: '800', color: '#E9FFF9', fontSize: 20, letterSpacing: 3}}>SYMMETRY</Text>
          <View style={{flexDirection: 'row'}}>
            <TouchableOpacity onPress={()=>navigation.push('Home')} style={{marginRight: 30}}>
              <Icon name="file-text" size={30} color={'#E9FFF9'}/>
            </TouchableOpacity>
            <TouchableOpacity onPress={()=>navigation.push('User')}>
              <Icon name="user" size={30} color={'#E9FFF9'}/>
            </TouchableOpacity>
          </View>
        </View>
      </View>

    </View>
  )
}

export default User;

const style = StyleSheet.create({
  container: {
    flex: 1,
    width: "100%",
  }, 
  containerHead: {
      width: '100%',
      padding: 12,
      backgroundColor: "#3F51B5",
      borderBottomColor: 'rgba(0, 0, 0, 0.3)',
      borderBottomWidth: 3
    }
})