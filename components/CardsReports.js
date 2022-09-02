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
    return(
        <>
          <View style={{borderWidth: 1, borderColor: 'blue', height: 150, borderRadius: 5, backgroundColor: '#E9FFF9', marginTop: 5, marginBottom: 15}}>
            <View style={{width: '100%', height: 45, borderTopRightRadius: 3, borderTopLeftRadius: 3, backgroundColor: '#3F51B5',
              flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between'}}>
              <Text style={{color: '#E9FFF9', fontSize: 16, marginLeft: 5}}>Brinco: {"12455851315"}</Text>
              <Text style={{color: '#E9FFF9', fontSize: 13, marginBottom: 20, marginRight: 5}}>{"01/01/2001 14:25"}</Text>
            </View>
            <View style={{flexDirection: 'row', height: '100%'}}>
              <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-start', padding: 5}}>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>Peso(KG): {"30"}</Text>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>Raça: {"Nelore"}</Text>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>Sexo: {"Fêmea"}</Text>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>Idade: {">33 meses"}</Text>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>valor médio: {"250,00"}</Text>
              </View>
              <View style={{width: '50%', justifyContent: 'space-between', height: 108, alignItems: 'flex-end', padding: 5}}>
                <Text style={{fontWeight: '700', letterSpacing: 1}}>Fazenda: {"Test test"}</Text>
                <TouchableOpacity>
                  <Icon name="remove" size={38} color={'#ea7070'}/>
                </TouchableOpacity>
              </View>
            </View>
          </View>
        </>
    )
}

export default CardsReports