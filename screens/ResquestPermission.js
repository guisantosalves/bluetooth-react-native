import * as React from "react";
import { View, Text, StyleSheet, TouchableOpacity, PermissionsAndroid } from "react-native";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { useNavigation } from "@react-navigation/native";

// icons
import Icon from 'react-native-vector-icons/FontAwesome';

//redux
import { useDispatch } from "react-redux";
import { requestSender } from "../features/appSlice";

const RequestPermissionUser = ({navigation}) => {

    const [isRequestAccepted, setisRequestAccepted] = React.useState();
    const dispatch = useDispatch()

    const [ACCESS_FINE_LOCATIONPerm, setACCESS_FINE_LOCATIONPerm] = React.useState();
    const [bluetoothRequest, setbluetoothRequest] = React.useState();
    const [bluetoothScan, setbluetoothScan] = React.useState();
    const [advertiseBluetooth, setadvertiseBluetooth] = React.useState();
    const [isOkay, setisOkay] = React.useState(false);


    // ver a request a ser feita
    const gettingPermissions = () => {

      try{
        requestLocationPermission();
        requestBLUETOOTH_ADMINPermission()
        BLUETOOTH_SCANrequestPermission()
        BLUETOOTH_ADVERTISErequestPermission()
        
        settingInRequestPermissionDB(true)
        
      }catch(e){
        alert(e)
      }
    }

    const requestLocationPermission = async () => {
        try {
          const granted = await PermissionsAndroid.request(
            PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION, {
              title: 'permissão de localização para o bluetooth scanning',
              message: 'Nós precisamos dessa permissão para rodar o bluetooth',
              buttonNeutral: 'Perguntar depois',
              buttonNegative: 'Cancelar',
              buttonPositive: 'OK',
            },
          ); 
          if (granted === PermissionsAndroid.RESULTS.GRANTED) {
            console.log('Location permission for bluetooth scanning granted');
            setACCESS_FINE_LOCATIONPerm(true);
          } else {
            console.log('Lowcation permission for bluetooth scanning revoked');
            setACCESS_FINE_LOCATIONPerm(false);
          }
        } catch (err) {
          console.log(err);
          return false;
        }
    }

    const requestBLUETOOTH_ADMINPermission = async () => {
        try {
          const granted = await PermissionsAndroid.request(
            PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT, {
              title: 'Location permission for bluetooth scanning',
              message: 'Nós precisamos dessa permissão para rodar o bluetooth',
              buttonNeutral: 'Perguntar depois',
              buttonNegative: 'Cancelar',
              buttonPositive: 'OK',
            },
          ); 
          if (granted === PermissionsAndroid.RESULTS.GRANTED) {
            console.log('Location permission for bluetooth scanning granted');
            setbluetoothScan(true);
          } else {
            console.log('Location permission for bluetooth scanning revoked');
            setbluetoothScan(false);
          }
        } catch (err) {
          console.log(err);
          return false;
        }
    }

    const BLUETOOTH_SCANrequestPermission = async () => {
      try {
        const granted = await PermissionsAndroid.request(
          PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN, {
            title: 'Location permission for bluetooth scanning',
            message: 'Nós precisamos dessa permissão para rodar o bluetooth',
            buttonNeutral: 'Perguntar depois',
            buttonNegative: 'Cancelar',
            buttonPositive: 'OK',
          },
        ); 
        if (granted === PermissionsAndroid.RESULTS.GRANTED) {
          console.log('Location permission for bluetooth scanning granted');
          setbluetoothRequest(true);
        } else {
          console.log('Location permission for bluetooth scanning revoked');
          setbluetoothRequest(false);
        }
      } catch (err) {
        console.log(err);
        return false;
      }
    }

    const BLUETOOTH_ADVERTISErequestPermission = async () => {
      try {
        const granted = await PermissionsAndroid.request(
          PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN, {
            title: 'Location permission for bluetooth scanning',
            message: 'Nós precisamos dessa permissão para rodar o bluetooth',
            buttonNeutral: 'Perguntar depois',
            buttonNegative: 'Cancelar',
            buttonPositive: 'OK',
          },
        ); 
        if (granted === PermissionsAndroid.RESULTS.GRANTED) {
          console.log('Location permission for bluetooth scanning granted');
          setadvertiseBluetooth(true);
        } else {
          console.log('Location permission for bluetooth scanning revoked');
          setadvertiseBluetooth(false);
        }
      } catch (err) {
        console.log(err);
        return false;
      }
    }
    
    const settingInRequestPermissionDB = async (response) => {
      const value = JSON.stringify(response)
      await AsyncStorage.setItem("@permissions", value)
      if(response){
        verifyIfRequestPermissionExist()
      }
    }

    const verifyIfRequestPermissionExist = async () => {
      const value = await AsyncStorage.getItem("@permissions")
      const valueParsed = JSON.parse(value)

      if(valueParsed == true){
        setisOkay(true)
        navigation.navigate("Home")
      }
    }
    // React.useEffect(()=>{
    //   verifyIfRequestPermissionExist()
    // }, [isOkay])
    return(
        <View style={style.container}>

            <View style={style.containerHead}>
                <View style={{flexDirection: 'row', justifyContent: 'flex-start', alignItems: 'center'}}>
                    <Text style={{marginRight: 10, fontWeight: '800', color: '#E9FFF9', fontSize: 20, letterSpacing: 3, padding: 2}}>FORBOV</Text>
                </View>
            </View>

            <View style={style.containerBody}>
                <View style={{height: "60%",flexDirection: "column", alignItems: 'center', justifyContent: 'flex-end'}}>
                    
                    {/* icon */}
                    <View>
                        <Icon name="heart" size={60} color={"#424242"}/>
                    </View>

                    {/* phrase */}
                    <Text style={style.textWarning}>
                        Nós precisamos de sua permissão para executarmos algumas funções nesse APP.
                    </Text>

                    {/* button */}
                    <TouchableOpacity style={style.buttonRequest} onPress={gettingPermissions}>
                        <Text style={style.textButton}>PERMITIR</Text>
                    </TouchableOpacity>

                </View>
            </View>

        </View>
    )
}

export default RequestPermissionUser;

const style = StyleSheet.create({
    container:{
        flex: 1,
        width: "100%",
    },
    containerHead: {
        width: '100%',
        padding: 12,
        backgroundColor: "#3F51B5",
        borderBottomColor: 'rgba(0, 0, 0, 0.3)',
        borderBottomWidth: 3
    },
    containerBody: {
        flex: 1,
    },
    buttonRequest: {
        paddingTop: 20,
        paddingBottom: 20,
        paddingLeft: 100,
        paddingRight: 100,
        borderRadius: 5,
        marginTop: 18,
        backgroundColor: "#3F51B5",
        elevation: 10
    },
    textButton: {
        color: "#e9fff9",
        fontSize: 18,
        letterSpacing: 3
    },
    textWarning: {
        textAlign: "center",
        fontSize: 18,
        marginTop: 18,
        color: 'black',
    }
})