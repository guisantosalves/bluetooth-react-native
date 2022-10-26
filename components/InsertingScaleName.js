/* eslint-disable prettier/prettier */
import {Button} from '@react-native-material/core';
import React from 'react';
import {
  Alert,
  Pressable,
  StyleSheet,
  Text,
  TextInput,
  View,
  ScrollView
} from 'react-native';
import {useDispatch, useSelector} from 'react-redux';
import {requestDevice} from '../features/appSlice';
import SQLite from 'react-native-sqlite-storage';
import CardScale from './CardScale';
import { useNavigation } from '@react-navigation/native';

SQLite.enablePromise(true)

const InsertingScaleName = () => {
  const [nameOfScale, setNameOfScale] = React.useState('');
  const navigation = useNavigation();

  const dispatch = useDispatch();

  const handleSubmit = () => {
    if (!nameOfScale) {
      alert('insira algum nome');
    } else {
      Alert.alert(
        'Confirmação',
        `Deseja mesmo confirmar o nome: ${nameOfScale}`,
        [
          {
            text: 'Cancelar',
            onPress: () => false,
            style: 'cancel',
          },
          {
            text: 'Salvar',
            onPress: saving,
            style: 'default',
          },
        ],
      );
    }
  };

  const saving = async () => {
    dispatch(
      requestDevice({
        requestDevice: nameOfScale,
      }),
    );

    let db = await SQLite.openDatabase({name: 'fazenda.db'});

    await db.executeSql(`
      CREATE TABLE IF NOT EXISTS balancas (ID INTEGER PRIMARY KEY AUTOINCREMENT, nome TEXT, createdAt TEXT)
    `)
    
    await db.executeSql(`
      INSERT INTO balancas (nome, createdAt) VALUES ('${nameOfScale}', '${new Date()}')
    `)
    
    navigation.push("Cadastro")
  };

  return (
    <View style={style.container}>
      <View style={style.containerButton}>
        <Text style={style.text}>Nome da balança</Text>
        <TextInput
          style={style.containerInput}
          keyboardType="default"
          placeholder="Ex: S3 123456"
          value={nameOfScale}
          onChangeText={setNameOfScale}
        />
      </View>
      <Pressable style={style.button} onPress={handleSubmit}>
        <Text style={style.text}>Salvar</Text>
      </Pressable>
      <View style={{marginTop: 16}}>
        <CardScale nameOfScale={setNameOfScale}/>
      </View>
    </View>
  );
};

export default InsertingScaleName;

const style = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 10,
    backgroundColor: '#e8e8e8',
  },
  containerButton: {
    padding: 5,
  },
  text: {
    color: '#424242',
    fontWeight: 'bold',
    fontSize: 18,
  },
  containerInput: {
    borderBottomWidth: 1,
    borderBottomColor: 'gray',
  },
  button: {
    alignItems: 'center',
    padding: 10,
    backgroundColor: '#A2E8AE',
    borderRadius: 8,
    elevation: 10,
    marginTop: 16,
  },
});
