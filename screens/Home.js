import React, { useEffect, useState } from 'react';
import {
  Button,
  SafeAreaView,
  StyleSheet,
  Text,
  View,
  PermissionsAndroid,
  Alert,
  TouchableOpacity,
  ScrollView,
  TouchableHighlight,
  KeyboardAvoidingView
} from 'react-native';

// icons
import Icon from 'react-native-vector-icons/FontAwesome';

import { FloatingAction } from 'react-native-floating-action';

const Home = ({navigation}) => {

  // pedir duas permisasões: BLUETOOTH_CONNECT, ACCESS_FINE_LOCATION

  const action = [
    {
      text: "Home",
      icon: <Icon name="user" size={30} color={'#E9FFF9'}/>,
      name: "bt_accessibility",
      position: 1
    },
    {
      text: "Cadastro",
      icon: <Icon name="user" size={30} color={'#E9FFF9'}/>,
      name: "bt_accessibility",
      position: 2
    },
    {
      text: "instruções",
      icon: <Icon name="user" size={30} color={'#E9FFF9'}/>,
      name: "bt_accessibility",
      position: 3
    }
    
  ]
  return (
      <SafeAreaView style={styles.container}>
        
        <View style={styles.mainHeader}>

          <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
 
              <Text style={{marginRight: 10, fontWeight: '800', color: '#E9FFF9', fontSize: 20, letterSpacing: 3}}>SYMMETRY</Text>
              <TouchableOpacity onPress={()=>navigation.push('Cadastro')}>
                <Icon name="user" size={30} color={'#E9FFF9'}/>
              </TouchableOpacity>

          </View>

        </View>

        <ScrollView showsVerticalScrollIndicator={false}>
          <Text>I AM HOME</Text>
        </ScrollView>

        <FloatingAction 
          actions={action}
          onPressItem={(item)=>{
            console.log(item)
          }}
        />
      </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'flex-start',
    alignItems: 'center',
    backgroundColor: '#ffff'
  },
  line: {
    borderColor: 'grey',
    borderWidth: 2,
    width: '100%',
    elevation: 10
  },
  mainHeader: {
    width: '100%',
    padding: 12,
    backgroundColor: "#3F51B5",
    borderBottomColor: 'rgba(0, 0, 0, 0.3)',
    borderBottomWidth: 3
  },
  buttonsForActions: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 15,
    borderRadius: 5,
    backgroundColor: "#424242",
    width: 100,
  },
  containerButtonsForAction: {
    flexDirection: 'row',
    justifyContent: 'space-evenly',
    alignItems: 'center',
    marginTop: 10,
    marginBottom: 10,
  },
  saveButton: {
    borderRadius: 999,
    width: 70,
    alignItems: 'center',
    padding: 15,
    backgroundColor: "#424242"
  },
  instructions: {
    elevation: 10,
    alignItems: 'center', 
    justifyContent: 'center',
    padding: 20,
    borderRadius: 10,
    backgroundColor: '#ea7070',
  },
  containerInstructions: {
    width: 300,
    height: 550,
    alignItems: 'center',
    justifyContent: 'center',
  },
  containerButtonToInstructions: {
    flexDirection: 'row',
    width: 150,
    height: 50,
    elevation: 10,
    alignItems: 'center', 
    justifyContent: 'center',
    padding: 5,
    borderRadius: 10,
    backgroundColor: '#A2E8AE',
    marginTop: 8
  }
});

export default Home;
