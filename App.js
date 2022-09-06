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
import RequestPermissionUser from './screens/ResquestPermission';
import User from './screens/User';

//redux
import { Provider } from 'react-redux';
import { store } from './App/Store';

const App = () => {

  const Stack = createNativeStackNavigator();

  return (
    <Provider store={store}>
      <NavigationContainer>
        
        <Stack.Navigator initialRouteName={'Home'} screenOptions={{
          headerShown: false,
        }}>
          <Stack.Screen name="Request" component={RequestPermissionUser}/>
          <Stack.Screen name="Home" component={Home} />
          <Stack.Screen name="Instructions" component={Instructions}/>
          <Stack.Screen name="Login" component={Login}/>
          <Stack.Screen name="Cadastro" component={Cadastro}/>
          <Stack.Screen name="User" component={User}/>
        </Stack.Navigator>
      </NavigationContainer>
    </Provider>
  );
};

export default App;
