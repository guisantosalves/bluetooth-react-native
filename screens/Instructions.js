import * as React from "react";
import { View, StyleSheet, Text, TouchableOpacity, SafeAreaView } from "react-native";

import Icon from 'react-native-vector-icons/MaterialIcons';

const Instructions = ({navigation}) => {
    return(
      <SafeAreaView style={styles.container}>
        <View style={styles.mainHeader}>

            <View style={{flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
            
            <View style={{flexDirection: 'row'}}>
                <TouchableOpacity onPress={()=>navigation.pop()}>
                <Icon name="arrow-back-ios" size={30} color={'#E9FFF9'}/>
                </TouchableOpacity>
            </View>

            </View>

        </View>
        
        <View style={styles.geralContainer}>
            <View style={styles.boxInstructions}>
                <Text style={styles.textPrimary}>Para usar o aplicativo deverá ter o bastão e a balança em mãos, podendo um ser usado sem o outro.</Text>
                <Text style={styles.textPrimary}>1 - conecte-se ao bastão.</Text>
                <Text style={styles.textPrimary}>2 - ligue a balança.</Text>
                <Text style={styles.textPrimary}>3 - Fique em uma distância até 2 metros da balança e do bastão.</Text>

                <View style={styles.subBoxDoubt}>
                    <Text style={styles.textSecondary}>Qualquer dúvida entrar em contato com o suporte</Text>
                    <Text style={styles.textSecondary}>(69) 3316-9350</Text>
                </View>
            </View>
        </View>
      </SafeAreaView>
    )
}

export default Instructions;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'flex-start',
        alignItems: 'center',
        backgroundColor: '#ffff'
    },
    mainHeader: {
        width: '100%',
        padding: 12,
        backgroundColor: "#3F51B5",
        borderBottomColor: 'rgba(0, 0, 0, 0.3)',
        borderBottomWidth: 3
    },
    geralContainer: {
        width: '100%',
        height: '80%',
        justifyContent: 'center',
        alignItems: "center"
    },
    boxInstructions: {
        width: '80%',
        height: 300,
        padding: 10,
        borderRadius: 10,
        backgroundColor: '#ea7070',
        elevation: 10,
    },
    textPrimary: {
        color: '#424242',
        fontWeight: 'bold',
        fontSize: 18,
        textAlign: 'auto'
    },
    textSecondary: {
        color: '#424242',
        fontWeight: '500',
        fontSize: 18,
        textAlign: 'center'
    },
    subBoxDoubt: {
        width: '100%',
        height: 90,
        justifyContent: "center",
        alignItems: 'center'
    }
    
    
})