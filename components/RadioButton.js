import * as React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';

const RadioButton = ({getValue}) => {

    const [isPressedOne, setisPressedOne] = React.useState(false)
    const [isPressedTwo, setisPressedTwo] = React.useState(false)
    

    const dataToSend = [
        {
            id: 1,
            value: 'Macho'
        },
        {
            id: 2,
            value: 'Femea'
        }
    ]

    // setting using setState (//oldValues => tratingOldValueAndSettingn)
    return(
        <>
            <TouchableOpacity 
            style={ isPressedOne ? style.pressedButton : style.defaulButton } 
            onPress={() => {
                    setisPressedTwo(oldValueFromState => oldValueFromState = false)
                    setisPressedOne(oldValueFromState => oldValueFromState = true)
                    getValue(dataToSend[0].id)
                }}>
                <Text style={isPressedOne ? style.textFromPressedButton : style.textDefault}>Macho</Text>
                <Icon name='male' size={20} color={ '#424242'}/>
            </TouchableOpacity>
            <TouchableOpacity 
            style={isPressedTwo ? style.pressedButton : style.defaulButton} 
            onPress={()=>{
                    setisPressedTwo(oldValueFromState => oldValueFromState = true)
                    setisPressedOne(oldValueFromState => oldValueFromState = false)
                    getValue(dataToSend[1].id)
                }}>
                <Text style={isPressedTwo ? style.textFromPressedButton : style.textDefault}>Fêmea</Text>
                <Icon name='female' size={20} color={'#424242'}/>
            </TouchableOpacity>
        </>
    )
}

const style = StyleSheet.create({
    defaulButton: {
        backgroundColor: "#cccccc",
        width: 120,
        flexDirection: 'row',
        padding: 10,
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: 999
    },
    pressedButton: {
        backgroundColor: "#A2E8AE",
        width: 120,
        flexDirection: 'row',
        padding: 10,
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: 999  
    },
    textFromPressedButton: {
        color: '#424242',
        marginRight: 3
    },
    textDefault: {
        color: '#424242',
        marginRight: 3
    }
})

export default RadioButton;