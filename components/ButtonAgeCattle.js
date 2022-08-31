import * as React from "react";
import { View, TouchableOpacity, Text, StyleSheet } from "react-native";

const ButtonAgeCattle = ({gettingValue}) => {

    const [isPressedOne, setisPressedOne] = React.useState(false);
    const [isPressedTwo, setisPressedTwo] = React.useState(false);
    const [isPressedThree, setisPressedThree] = React.useState(false);
    const [isPressedFour, setisPressedFour] = React.useState(false);
    const [isPressedFive, setisPressedFive] = React.useState(false);
    let gettingAllStates = []

    gettingAllStates.push(isPressedOne, isPressedTwo, isPressedThree, isPressedFour, isPressedFive)

    const verifyPressedButtons = () => {
        /*
            On each iteration, we check if the current value is already 
            present as a key in the result object. 
            If it is present, we increment it by 1.
            If the value is not present, we initialize it to 1.
        */
        //pega a key como item do foreach
        let result = {}
        gettingAllStates.forEach((item, index) => {
            result[item] = (result[item] || 0) + 1;
        })
        
        if(result?.true > 1){
            return true
        }else{
            return false
        }
    }

    // verifyPressedButtons()

    const dataToSend = [
        {
            id: 1,
            value: "0 a 8 meses",
        },
        {
            id: 2,
            value: "9 a 12 meses",
        },
        {
            id: 3,
            value: "13 a 24 meses",
        },
        {
            id: 4,
            value: "24 a 36 meses",
        },
        {
            id: 5,
            value: ">36 meses",
        }
    ]

    React.useEffect(()=>{
        if(verifyPressedButtons()){
            alert("Só pode escolher umas das opções")
            setisPressedOne(false)
            setisPressedTwo(false)
            setisPressedThree(false)
            setisPressedFour(false)
            setisPressedFive(false)
        }

    }, [isPressedOne, isPressedTwo, isPressedThree, isPressedFour, isPressedFive])
    return(
        <>
            <TouchableOpacity style={isPressedOne ? style.containerButtonPressed : style.containerButton} onPress={()=>{setisPressedOne(!isPressedOne);gettingValue(dataToSend[0].id)}}>
                <Text style={isPressedOne ? style.textPressed : style.textColor}>{dataToSend[0].value}</Text>
            </TouchableOpacity>

            <TouchableOpacity style={isPressedTwo ? style.containerButtonPressed : style.containerButton} onPress={()=>{setisPressedTwo(!isPressedTwo);gettingValue(dataToSend[1].id)}}>
                <Text style={isPressedTwo ? style.textPressed : style.textColor}>{dataToSend[1].value}</Text>
            </TouchableOpacity>

            <TouchableOpacity style={isPressedThree ? style.containerButtonPressed : style.containerButton} onPress={()=>{setisPressedThree(!isPressedThree);gettingValue(dataToSend[2].id)}}>
                <Text style={isPressedThree ? style.textPressed : style.textColor}>{dataToSend[2].value}</Text>
            </TouchableOpacity>

            <TouchableOpacity style={isPressedFour ? style.containerButtonPressed : style.containerButton} onPress={()=>{setisPressedFour(!isPressedFour);gettingValue(dataToSend[3].id)}}>
                <Text style={isPressedFour ? style.textPressed : style.textColor}>{dataToSend[3].value}</Text>
            </TouchableOpacity>

            <TouchableOpacity style={isPressedFive ? style.containerButtonPressed : style.containerButton} onPress={()=>{setisPressedFive(!isPressedFive);gettingValue(dataToSend[4].id)}}>
                <Text style={isPressedFive ? style.textPressed : style.textColor}>{dataToSend[4].value}</Text>
            </TouchableOpacity>
        </>
    )
}

export default ButtonAgeCattle;

const style = StyleSheet.create({
    containerButton: {
        marginRight: 5,
        padding: 8,
        borderRadius: 999,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#424242'
    },
    containerButtonPressed: {
        marginRight: 5,
        padding: 8,
        borderRadius: 999,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#E9FFF9',
        color: "#424242",
    },
    textColor: {
        color: '#E9FFF9',
    },
    textPressed: {
        color: "#424242",
    }
})