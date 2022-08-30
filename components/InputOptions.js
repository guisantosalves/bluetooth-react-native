import * as React from "react";
import { View, TextInput, Text } from "react-native";

const InputOptions = ({title, value, Pholder, isEnable, isNumeric}) => {
    return(
        <>
            {
                isEnable === true ? (
                    <View>
                        <Text>{title}</Text>
                        <View>
                            <TextInput onChange={(e)=>value(e.target.value)} placeholder={Pholder}/>
                        </View>
                    </View>
                ) : isEnable === false ? (
                    <View>
                        <Text>{title}</Text>
                        <View>
                            <Text>{value}</Text>
                        </View>
                    </View>
                ) : isNumeric === true ?(
                    <View>
                        <Text>{title}</Text>
                        <View>
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