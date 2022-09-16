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
import AsyncStorage from '@react-native-async-storage/async-storage';


// icons
import Icon from 'react-native-vector-icons/FontAwesome';

import { FloatingAction } from 'react-native-floating-action';

import CardsReports from '../components/CardsReports';
import RequestPermissionUser from './ResquestPermission';

// redux
import { useSelector } from 'react-redux';

const Home = ({navigation}) => {

  const requestsFromUser = useSelector((state)=>state.counter.requestResult)

  const [dataToDisplayFromAS, setdataToDisplayFromAS] = React.useState([])
  
  const [requestVerification, setrequestVerification] = React.useState()
  const action = [
    {
      text: "Home",
      icon: <Icon name="home" size={30} color={'#E9FFF9'}/>,
      name: "home",
      position: 1
    },
    {
      text: "Cadastro",
      icon: <Icon name="archive" size={25} color={'#E9FFF9'}/>,
      name: "cadastro",
      position: 2
    },
    {
      text: "instruções",
      icon: <Icon name="comments-o" size={25} color={'#E9FFF9'}/>,
      name: "instrucao",
      position: 3
    }
    
  ]

  const gettingItem = async () => {
    try{
      const allkeys = await AsyncStorage.getAllKeys()
      const dataFromAS = await AsyncStorage.multiGet(allkeys)

      if(dataFromAS !== null){
          dataFromAS.map((item, index)=>{
            if(item[0] != "@permissions"){
              setdataToDisplayFromAS(oldValue => [...oldValue, JSON.parse(item[1])])
            }
          })
      }else{
        alert("nao deu")
      }
    }catch(e){
      alert(e)
    }
  }

  React.useEffect(()=>{
    gettingItem()
  }, [])

  const verifyRace = (id) => {
    switch(id){
      case 2:
        return "NELORE"
      case 5:
        return "SENEPOL"
      case 20:
        return "REDIANO"
      case 19:
        return "MURRAY GREY"
      case 18:
        return "CRUZADA"
      case 7:
        return "CARACU"
      case 17:
        return "ABERDEEN"
    }
  }

  const verifySex = (id) => {
    if(id === 1){
      return "Macho"
    }else{
      return "Femea"
    }
  }

  const verifyAge = (id) => {
    switch(id){
      case 1:
        return "0 a 8 meses"
      case 2:
        return "9 a 12 meses"
      case 3:
        return "13 a 24 meses"
      case 4:
        return "24 a 36 meses"
      case 5:
        return ">36 meses"
    }
  }

  const DisplayMoney = (value) => {
    const newVa = value?.toString().replace('.',',').replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')
    return newVa
  }

  return (
      <SafeAreaView style={styles.container}>
          <>
            <View style={styles.mainHeader}>
              <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>

                  <Text style={{marginRight: 10, fontWeight: '800', color: '#E9FFF9', fontSize: 20, letterSpacing: 3}}>FORBOV</Text>
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

              {/* the latest insertions */}
              <ScrollView showsVerticalScrollIndicator={false} style={{width: '100%', padding: 10}}>
                {dataToDisplayFromAS.map((item, index)=>(
                <View key={index} style={{borderWidth: 2, borderColor: 'blue', height: 158, borderRadius: 5, backgroundColor: '#E9FFF9', marginTop: 5, marginBottom: 15}}>
                <View style={{width: '100%', height: 45, borderTopRightRadius: 3, borderTopLeftRadius: 3, backgroundColor: '#3F51B5',
                  flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between'}}>
                  <Text style={{color: '#E9FFF9', fontSize: 16, marginLeft: 5}}>Brinco: {item.brinco}</Text>
                  <Text style={{color: '#E9FFF9', fontSize: 13, marginBottom: 20, marginRight: 5}}>{item.date}</Text>
                </View>
                <View style={{flexDirection: 'row', height: '100%'}}>
                  <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-start', padding: 5}}>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>Peso(KG): {item.peso}</Text>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>Raça: {verifyRace(item.raca)}</Text>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>Sexo: {verifySex(item.sexo)}</Text>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>Idade: {verifyAge(item.idade)}</Text>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>valor médio: {DisplayMoney(item?.valorMedio)}</Text>
                  </View>
                  <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-end', padding: 5}}>
                    <Text style={{fontWeight: '700', letterSpacing: 1}}>Brinco EL: {item.brincoEletronico}</Text>
                  </View>
                </View>
                </View>
              ))}
              </ScrollView>

              <FloatingAction 
              actions={action}
              onPressItem={(item)=>{
                if(item === "home"){
                  navigation.push("Home")
                }else if(item === "cadastro"){
                  navigation.push("Cadastro")
                }else if(item === "instrucao"){
                  navigation.push("Instructions")
                }
              }}
              />
          </>
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
