import * as React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';

const RadioButton = ({ array, onSelected }) => {
    const [optSelected, setOptSelected] = React.useState(null);
    const selectHandler = (value) => {
        setOptSelected(value)
        onSelected(value)
    }

    return (
        <View style={{ display: 'flex', flexDirection: 'row' }}>
            {array.map((opt) => {
                return (
                    <View style={{ padding: 5 }}>
                        <TouchableOpacity
                            style={optSelected === opt.id ? style.pressedButton : style.defaulButton}
                            onPress={() => selectHandler(opt.id)}>
                            <Text style={optSelected === opt.id ? style.textFromPressedButton : style.textDefault}>{opt.value}</Text>
                        </TouchableOpacity>
                    </View>
                )
            })}
        </View >
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