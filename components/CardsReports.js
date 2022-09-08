import * as React from "react";
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
  import Icon from 'react-native-vector-icons/FontAwesome';

  // colocar os props
const CardsReports = () => {

    const [gettingFromDB, setgettingFromDB] = React.useState([])

    // React.useEffect(()=>{
    // }, [])

    const gettingTheDataFromRealm = () => {
      try{
      // abro conexao no meu model e fecho quando executo ele
        
      
      }catch(e){
        alert(e)
      }

    }

    return(
        <>
        {gettingFromDB.map((item, index)=>(
          <View key={index} style={{borderWidth: 1, borderColor: 'blue', height: 150, borderRadius: 5, backgroundColor: '#E9FFF9', marginTop: 5, marginBottom: 15}}>
          <View style={{width: '100%', height: 45, borderTopRightRadius: 3, borderTopLeftRadius: 3, backgroundColor: '#3F51B5',
            flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between'}}>
            <Text style={{color: '#E9FFF9', fontSize: 16, marginLeft: 5}}>Brinco: {item.brinco}</Text>
            <Text style={{color: '#E9FFF9', fontSize: 13, marginBottom: 20, marginRight: 5}}>{item.date}</Text>
          </View>
          <View style={{flexDirection: 'row', height: '100%'}}>
            <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-start', padding: 5}}>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>Peso(KG): {item.peso}</Text>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>Raça: {item.raca}</Text>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>Sexo: {item.sexo}</Text>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>Idade: {item.idade}</Text>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>valor médio: {item.valorMedia}</Text>
            </View>
            <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-end', padding: 5}}>
              <Text style={{fontWeight: '700', letterSpacing: 1}}>Fazenda: {item.fazenda}</Text>
              <TouchableOpacity onPress={()=>console.log(item.brinco)}>
                <Icon name="remove" size={38} color={'#ea7070'}/>
              </TouchableOpacity>
            </View>
          </View>
        </View>
        ))}
        </>
    )
}

export default CardsReports