import * as React from 'react';
import { View, Text, StyleSheet } from 'react-native';

const Header = ({theader}) => {
    return(
        <View style={style.container}>
            <Text style={{fontSize: 30}}>{theader}</Text>
            <View style={style.line}/>
        </View>
    )
}

export default Header;

const style = StyleSheet.create({
    container: {
        justifyContent: 'center',
        alignItems: 'center',
        marginBottom: 10
    },
    line: {
        borderColor: 'black',
        borderBottomWidth: 1,
        width: '100%'
    }

})