import * as React from "react";
import { View, Text, StyleSheet, Pressable, TouchableOpacity } from "react-native";
import Icon from "react-native-vector-icons/Entypo";
import SQLite from 'react-native-sqlite-storage';

SQLite.enablePromise(true)

const CardScale = ({nameOfScale}) => {
    
    const [scaleNames, setScaleNames] = React.useState([]);

    const callingTheData = async () => {
        let db = await SQLite.openDatabase({name: 'fazenda.db'})
        
        await db.transaction(tx=>{
            tx.executeSql(`
                SELECT * FROM balancas
            `,
            [],
            (tx, results)=>{
                for(let i = 0; i < results.rows.length; i++){
                    setScaleNames(oldvalue=>[...oldvalue, results.rows.item(i)])
                }
            })
        })

    }

    React.useLayoutEffect(()=>{
        callingTheData()
    }, [])

    
    return(
        <View>
            {scaleNames.map((item, index)=>{
                return(
                    <TouchableOpacity style={style.container} activeOpacity={0.8} onPress={()=>nameOfScale(item.nome)}>
                        <Text key={index} style={style.text}>{item.nome}</Text>
                        <Icon name="upload-to-cloud" size={35} color={"#424242"}/>
                    </TouchableOpacity>
                )
            })}
            
        </View>
    )
}

export default CardScale;

const style = StyleSheet.create({
    container: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: 13,
        borderRadius: 8,
        marginBottom: 5,
        backgroundColor: "#A2E8AE",
        elevation: 10
    },
    text: {
        color: "#424242",
        fontWeight: 'bold',
        fontSize: 20
    }
})