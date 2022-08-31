import * as React from "react";
import { View, TextInput, Text, StyleSheet } from "react-native";

const InputOptions = ({title, value, Pholder, isEnable, isNumeric}) => {
    return(
        <>
            {
                isEnable === true ? (
                    <View style={style.container}>
                        <Text style={style.textColor}>{title}</Text>
                        <View style={style.inputStyle}>
                            <TextInput onChange={(e)=>value(e.target.value)} placeholder={Pholder}/>
                        </View>
                    </View>
                ) : isEnable === false ? (
                    <View style={style.container}>
                        <Text style={style.textColor}>{title}</Text>
                        <View style={style.inputStyle}>
                            <TextInput value={value} placeholder={Pholder} keyboardType={"none"} editable={false}/>
                        </View>
                    </View>
                ) : isNumeric === true ?(
                    <View style={style.container}>
                        <Text style={style.textColor}>{title}</Text>
                        <View style={style.inputStyle}>
                            <TextInput onChange={(e)=>value(e.target.value)} placeholder={Pholder} keyboardType={"numeric"}/>
                        </View>
                    </View>
                ) : (
                    <></>
                )
            }
        </>
    )
}

export default InputOptions;

const style = StyleSheet.create({
    container: {
        padding: 3,
        marginTop: 2
    },
    textColor: {
        color: '#424242',
        marginLeft: 8,
        marginBottom: 1
    },
    inputStyle: {
        elevation: 1,
        borderRadius: 999,
        paddingLeft: 8,
        backgroundColor: "#E9FFF9"
    }

})