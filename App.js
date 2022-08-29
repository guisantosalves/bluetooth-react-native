import React, { useEffect, useState } from 'react';
import {
  Button,
  SafeAreaView,
  StyleSheet,
  Text,
  View,
  PermissionsAndroid,
  Alert,
} from 'react-native';


import DevicesLists from './components/DevicesLists';

import 'text-encoding-polyfill'

import { stringToBytes } from "convert-string";

import EscPosEncoder from 'esc-pos-encoder';

import { BleManager } from 'react-native-ble-plx';
import { Service } from 'react-native-ble-plx';
import { NativeDevice } from 'react-native-ble-plx';
import base64 from 'react-native-base64'
import { Buffer } from 'buffer';
import { Console } from 'console';

const App = () => {
  
  const manager = new BleManager();
  const serv = new Service(NativeDevice, manager)

  const [turnOn, setTurnOn] = useState(false)
  const [devices, setDevices] = useState([])
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
        
        // erro resolvido fazendo a request do ACCESS_FINE_LOCATION
        alert(error)

        return
      }

      if(listOfDevices){
        
        setDevices(oldArray=>[...oldArray, listOfDevices])

      }
      
      
    });

    // espera terminar todo o scan e espera 1 sec pra dar o stop
    setTimeout(()=>{
      manager.stopDeviceScan()
      setIsScanFinished(true)
      console.log("finished scanning")
    }, 1000)
  }

  // liga scan
  const turnOnScan = () => {

    if(turnOn){

      theScan()
      
    }else{
      Alert.alert("Bluetooth desligado", "Você precisa ligar o bluetooth", [
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

  }


  // pega características de algo ja pareado
  const getData = async (theBalance) => {

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

        result.monitorCharacteristicForService("6e400001-b5a3-f393-e0a9-e50e24dcca9e", "6e400003-b5a3-f393-e0a9-e50e24dcca9e", (err, cha)=>{
            
          console.log(cha)
            
            if(cha.value){
              let weight_transformed = base64.decode(cha.value)
              setgettingWeight(weight_transformed)
              alert("Peso", `Seu peso é: ${weight_transformed}`)
              return
            }else{
              alert(err)
            }
              
          })

      }).catch((errr)=>{
        console.log(errr)
      });

    })
    .catch(err=>{
      console.log(err)
    })

    // console.log("CAINDO EM CONNECT")
    
  }

  React.useEffect(() => {

    const subscription = manager.onStateChange((state) => {

        if(state === 'PoweredOn'){
          setTurnOn(true)
        }else{
          setTurnOn(false)
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


  // pedir duas permissões: BLUETOOTH_CONNECT, ACCESS_FINE_LOCATION
  return (
    <SafeAreaView style={styles.container}>
      <View style={[styles.form, {flexDirection: 'row'}]}>

      <View style={{marginRight: 20}}>
          <Text style={styles.text}>Encontrar balança</Text>
          <Button onPress={turnOnScan} title={"Clique aqui"}/>
        </View>

        <View>
          <Text style={styles.text}>Limpar busca</Text>
          <Button onPress={()=>setDevices([])} title={"Clique aqui"}/>
        </View>
      </View>

      <View style={[styles.form, {flexDirection: 'row'}]}>
        <View style={styles.form}>
          <Text style={styles.text}>Request permission</Text>
          <Button onPress={requestLocationPermission} title={"Clique aqui"}/>
        </View>

        <View style={styles.form}>
          <Text style={styles.text}>request weight</Text>
          <Button onPress={gettingFromChild} title={"Clique aqui"}/>
        </View>
      </View>

      <View style={styles.line}/>

      <SafeAreaView style={{height: 400}}>

        {isScanFinished && (
          <DevicesLists devices={devices} valFunc={gettingFromChild}/>
        )}

      </SafeAreaView>

      {onlyOneWeightIsGot ? (
      <View style={{height: 100, borderColor: 'red', borderWidth: 1}}>
        <Text>{gettingWeight}</Text>
      </View>) : (<></>)}

    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'flex-start',
    alignItems: 'center',
    backgroundColor: '#c9c3c3'
  },
  form: {
    padding: 10
  },
  text: {
    marginBottom: 10
  },
  line: {
    borderColor: 'black',
    borderWidth: 1,
    width: '95%'
  }
});

export default App;
