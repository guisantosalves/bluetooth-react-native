import React from "react";
import { 
    View, 
    StyleSheet, 
    ScrollView, 
    Text, 
    TouchableOpacity 
} from "react-native";

const DevicesLists = ({devices, valFunc}) => {
    
    const convertingArrays = Object.entries(devices)
    
    return(
        <ScrollView style={style.container}>

            {convertingArrays && (
            convertingArrays?.map((item, index)=>(
            <TouchableOpacity 
                onPress={()=>valFunc(item)} 
                style={{width: 320, margin: 10, borderRadius: 10, backgroundColor: "#88B7B5"}}
                key={index}
                >
                <View style={{flexDirection: 'row', flexWrap: 'wrap', padding: 20}}>
                    <Text style={{marginRight: 5}}>name: {item[1].name}</Text>
                    <Text style={{marginRight: 5}}>localName: {item[1].localName}</Text>
                    <Text style={{marginRight: 5}}>id: {item[1].id}</Text>
                    <Text style={{marginRight: 5}}>rssi: {item[1].rssi}</Text>
                    <Text style={{marginRight: 5}}>isConnectable: {String(item[1].isConnectable)}</Text>
                </View>
            </TouchableOpacity>
                ))
            )}

        </ScrollView>
    )
}

export default DevicesLists;

const style = StyleSheet.create({
    container: {
        height: 500
    }
})