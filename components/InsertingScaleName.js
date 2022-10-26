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
} from 'react-native';
import {useDispatch, useSelector} from 'react-redux';
import {requestDevice} from '../features/appSlice';
import { useNavigation } from '@react-navigation/native';

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

  const saving = () => {
    dispatch(
      requestDevice({
        requestDevice: nameOfScale,
      }),
    );
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
