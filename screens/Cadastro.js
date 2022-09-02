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
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

// components
import ButtonsOptions from '../components/ButtonsOptions';
import InputOptions from "../components/InputOptions";
import Header from "../components/Header";
import RadioButton from '../components/RadioButton';
import ButtonAgeCattle from '../components/ButtonAgeCattle';

import { BleManager } from 'react-native-ble-plx';
import { Service } from 'react-native-ble-plx';
import { NativeDevice } from 'react-native-ble-plx';
import base64 from 'react-native-base64'
import { Buffer, INSPECT_MAX_BYTES } from 'buffer';
import { deepStrictEqual } from 'assert';
import { FloatingAction } from 'react-native-floating-action';

const Cadastro = ({navigation}) => {
  
  const manager = new BleManager();
  const serv = new Service(NativeDevice, manager)

  // States Of The Application
  const [Weightt, setWeightt] = useState()

  const [stopScan, setstopScan] = useState(false)
  const [devices, setDevices] = useState([])
  const [deviceStick, setdeviceStick] = useState([])
  const [isScanFinished, setIsScanFinished] = useState(false)
  const [onlyOneWeightIsGot, setonlyOneWeightIsGot] = useState(false)
  const [balance, setBalance] = useState([])
  const [serviceUUID, setServiceUUID] = useState("")
  const [UUID, setUUID] = useState("")
  const [gettingWeight, setgettingWeight] = useState("")
  const [isChoose, setisChoose] = useState(false)
  const [objectFromChildBalance, setobjectFromChildBalance] = useState()

  // setting data to the weight works
  const [services, setServices] = useState()
  const [characteristc, setcharacteristc] = useState()

  const requestLocationPermission = async () => {
    try {
      const granted = await PermissionsAndroid.request(
        PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION, {
          title: 'Location permission for bluetooth scanning',
          message: 'wahtever',
          buttonNeutral: 'Ask Me Later',
          buttonNegative: 'Cancel',
          buttonPositive: 'OK',
        },
      ); 
      if (granted === PermissionsAndroid.RESULTS.GRANTED) {
        console.log('Location permission for bluetooth scanning granted');
        return true;
      } else {
        console.log('Location permission for bluetooth scanning revoked');
        return false;
      }
    } catch (err) {
      console.warn(err);
      return false;
    }
  }

  const requestBluetoothPermission = async () => {
    try {

      const granted = await PermissionsAndroid.request(
        PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,
        {
          title: "bluetooth Permission",
          message:
            "O aplicativo precisa acessar o seu bluetooth ",
          buttonNeutral: "Ask Me Later",
          buttonNegative: "Cancel",
          buttonPositive: "OK"
        }
      )

    } catch (err) {
      console.warn(err);
    }
  }

  // array of bytes
  function toUTF8Array(str) {
    let utf8 = [];
    for (let i = 0; i < str.length; i++) {
        let charcode = str.charCodeAt(i);
        if (charcode < 0x80) utf8.push(charcode);
        else if (charcode < 0x800) {
            utf8.push(0xc0 | (charcode >> 6),
                      0x80 | (charcode & 0x3f));
        }
        else if (charcode < 0xd800 || charcode >= 0xe000) {
            utf8.push(0xe0 | (charcode >> 12),
                      0x80 | ((charcode>>6) & 0x3f),
                      0x80 | (charcode & 0x3f));
        }
        // surrogate pair
        else {
            i++;
            // UTF-16 encodes 0x10000-0x10FFFF by
            // subtracting 0x10000 and splitting the
            // 20 bits of 0x0-0xFFFFF into two halves
            charcode = 0x10000 + (((charcode & 0x3ff)<<10)
                      | (str.charCodeAt(i) & 0x3ff));
            utf8.push(0xf0 | (charcode >>18),
                      0x80 | ((charcode>>12) & 0x3f),
                      0x80 | ((charcode>>6) & 0x3f),
                      0x80 | (charcode & 0x3f));
        }
    }
    return utf8;
  }

  // hex string to byte
  function hexToBytes(hex) {
    for (var bytes = [], c = 0; c < hex.length; c += 2)
        bytes.push(parseInt(hex.substr(c, 2), 16));
    return bytes;
  }

  // o scan
  const theScan = () => {
    manager.startDeviceScan(null, {allowDuplicates: false}, (error, listOfDevices) => {
      
      if(error){
        
        console.log(error)
        // erro resolvido fazendo a request do ACCESS_FINE_LOCATION
        alert(error)

        return
      }

      if(listOfDevices){
        
        setDevices(oldArray=>[...oldArray, listOfDevices])
        

      }
      
      // espera terminar todo o scan e espera 1 sec pra dar o stop
      setTimeout(()=>{
        manager.stopDeviceScan()
        setIsScanFinished(true)
        console.log("finished scanning")
      }, 1000)
    });


  }

  // pega características de algo ja pareado
  const getData = (theBalance) => {

    theBalance[1].connect().then((balance)=>{
      
      return balance.discoverAllServicesAndCharacteristics();
      
    })
    .then((data)=>{
      return data
    })
    .then((dataComplet)=>{
      
      let base64Stringg = Buffer.from("{RW}").toString('base64')
      dataComplet.writeCharacteristicWithResponseForService("6e400001-b5a3-f393-e0a9-e50e24dcca9e", "6e400002-b5a3-f393-e0a9-e50e24dcca9e", base64Stringg)
      .then((result)=>{

        return result

      }).then(()=>{

        dataComplet.monitorCharacteristicForService("6e400001-b5a3-f393-e0a9-e50e24dcca9e", "6e400003-b5a3-f393-e0a9-e50e24dcca9e", (err, cha)=>{
            console.log(err)
            try{
              let weight_transformed = base64.decode(cha.value)
              setgettingWeight(weight_transformed)
              return
            }catch(err){
              console.log("error", err)
            }
              
          })

      }).catch((errr)=>{

        console.log("error", errr)

      });

    })
    .catch(err=>{

      console.log("error", err)

    }).finally(()=>{

      alert(gettingWeight)

    })
    
  }

  const ScanAndConnect = () => {
    manager.startDeviceScan(null, {allowDuplicates: false}, (error, listOfDevices) => {
      
      if(error){
        
        // erro resolvido fazendo a request do ACCESS_FINE_LOCATION
        alert(error)

        return
      }

      if(listOfDevices.id == "C7:C6:8B:C9:9F:2D"){
        
        setDevices(oldArray=>[...oldArray, listOfDevices])
        manager.stopDeviceScan()
      }
      
      // espera terminar todo o scan e espera 1 sec pra dar o stop
    //  setTimeout(()=>{
    //   manager.stopDeviceScan()
    //   setIsScanFinished(true)
    //   console.log("finished scanning")
    // }, 1000)
    });
  }

  React.useEffect(() => {

    const subscription = manager.onStateChange((state) => {

        if(state === 'PoweredOn'){

          // ScanAndConnect()
          console.log("Bluetooth ligado")

        }else{
          
          Alert.alert("Bluetooth desligado", "Você precisa ligar o bluetooth e conectar na balança", [
            {
              text: "Cancelar",
              onPress: () => console.log("apertou cancelar"),
              style: "cancel"
            },
            {
              text: "ok",
              onPress: () => console.log("apertou ok"),
            }
          ])
          
        }

    }, true);

    return () => {
      subscription.remove()
    };

  }, [manager]);

  const gettingFromChild = (value) => {
    // get value from child and pass to the getData
    getData(value)

  }

  const gettingMacID = () => {
    alert(`your mac adress is: ${devices[0].id}`)
  }
  

  const getFileOfEarings = () => {
    manager.startDeviceScan(null, {allowDuplicates: false}, (err, listOfDevice) => {
      
      if(err){
        alert(err)
        return
      }

      if(listOfDevice){
        console.log(listOfDevice)
        
      }

    })
    setTimeout(()=>{
      manager.stopDeviceScan()
      setIsScanFinished(true)
      console.log("finished scanning")
    }, 1000)
  }

  
  const getTheCurrentWeight = () => {

    devices[0]
    .connect()
    .then((deviceConnected)=>{

      return deviceConnected.discoverAllServicesAndCharacteristics()

    })
    .then((data)=>{

      let base64Stringg = Buffer.from("{RW}").toString('base64')

      data.writeCharacteristicWithoutResponseForService("6e400001-b5a3-f393-e0a9-e50e24dcca9e", "6e400002-b5a3-f393-e0a9-e50e24dcca9e", base64Stringg)
      .then((oldCharacterist)=>{

        data.monitorCharacteristicForService("6e400001-b5a3-f393-e0a9-e50e24dcca9e", "6e400003-b5a3-f393-e0a9-e50e24dcca9e", (err, cha)=>{
          
          try{

            let weight_transformed = base64.decode(cha.value)

            console.log(weight_transformed)
            Alert.alert("Peso", `Importar o peso: ${weight_transformed}`, [
              {
                text: "Errado",
                onPress: () => console.log("apertou cancelar"),
                style: "cancel"
              },
              {
                text: "Correto",
                onPress: () => setWeightt(weight_transformed),
              }
            ])

          }catch(err){
            
            if(err){
              console.log("error", err)
              getTheCurrentWeight()
            }
            
          }
            
        })

      })

        
    })
    .catch((err)=>{

      alert(err)

    })
    .finally(()=>{})
    
    devices[0].onDisconnected((err, dev)=>{
      console.log(dev)
    })
  }

  // pedir duas permisasões: BLUETOOTH_CONNECT, ACCESS_FINE_LOCATION
  return (
      <SafeAreaView style={styles.container}>
        
        <View style={styles.mainHeader}>

          <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
            
            <View style={{flexDirection: 'row'}}>
              <TouchableOpacity onPress={()=>navigation.push('Instructions')}>
                <Icon name="tooltip-text" size={30} color={'#E9FFF9'}/>
              </TouchableOpacity>
            </View>

            
            <View style={{flexDirection: 'row'}}>
              <Text style={{marginRight: 10, fontWeight: '800', color: '#E9FFF9', fontSize: 20}}>Scan & connect</Text>
              <TouchableOpacity onPress={ScanAndConnect}>
                <Icon name="clipboard-search" size={30} color={'#E9FFF9'}/>
              </TouchableOpacity>
            </View>

          </View>

        </View>

        <ScrollView showsVerticalScrollIndicator={false}>

          {devices.length > 0 ? (
          <View>
            <View style={styles.containerButtonsForAction}>
              <TouchableOpacity onPress={getTheCurrentWeight} style={styles.buttonsForActions}>
                <Text style={{fontSize: 20,color: '#E9FFF9', marginRight: 5}}>Peso</Text>
                <Icon name='cow' size={24} color={'#E9FFF9'}/>
              </TouchableOpacity>
            </View>

            {/* Form */}
            <View>
              <Header theader={"Cadastro"}/>
              <InputOptions weight={Weightt}/>
            </View>
          </View>
          ) : (
            <View style={styles.containerInstructions}>
              <View style={styles.instructions}>
                  <Text style={{color: '#424242', fontWeight: 'bold', fontSize: 18, textAlign: 'center'}}>Leia as instruções de como usar o App</Text>
              </View>
              <TouchableOpacity 
              style={styles.containerButtonToInstructions}
              onPress={()=>navigation.push('Instructions')}>
                  <Text style={{color: '#424242', fontWeight: 'bold', fontSize: 18, textAlign: 'center', marginRight: 5}}>Leia aqui</Text>
                  <Icon name="tooltip-text" size={30} color={'#E9FFF9'}/>
              </TouchableOpacity>
            </View>
          )}
          
        </ScrollView>
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

export default Cadastro;
