import React, { useEffect, useState } from 'react';
import {
  StyleSheet,
} from 'react-native';

import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import Home from './screens/Home';
import Instructions from './screens/Instructions';
import Login from './screens/Login';
import Cadastro from './screens/Cadastro';

  //I can use this to delete the file
  // Realm.deleteFile(Pesagem)

const App = () => {

  const Stack = createNativeStackNavigator();

  return (
      <NavigationContainer>
        <Stack.Navigator initialRouteName='Home' screenOptions={{
          headerShown: false,
        }}>

          <Stack.Screen name="Home" component={Home} />
          <Stack.Screen name="Instructions" component={Instructions}/>
          <Stack.Screen name="Login" component={Login}/>
          <Stack.Screen name="Cadastro" component={Cadastro}/>
        </Stack.Navigator>
      </NavigationContainer>
  );
};

export default App;
