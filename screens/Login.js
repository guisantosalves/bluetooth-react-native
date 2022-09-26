import { Picker } from "@react-native-picker/picker";
import React, { useState } from "react";
import { View, Text, Image, ScrollView, StyleSheet, Platform, KeyboardAvoidingView, TouchableWithoutFeedback, Keyboard, TouchableOpacity } from "react-native";

export default function Login() {

    const [fazendas, setFazendas] = useState([{ item: { nome: 'Witillan' }, key: 1 }, { item: { nome: 'Langa' }, key: 2 }])
    const [id, setId] = useState([])
    return (
        <KeyboardAvoidingView
            style={styles.inner}
            behavior={Platform.OS === 'ios' ? 'position' : 'height'}
        >
            <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
                <View>
                    <ScrollView>
                        <View style={styles.image}>
                            <Image style={styles.image} source={require('../assets/ForBov.png')} />
                        </View>
                        <Text style={styles.marginVertical10}>Usuário</Text>
                        <View>
                            <Picker
                                placeholderTextColor='black'
                                style={{ color: '#6c757d' }}
                                itemStyle={{ height: 120, color: 'black' }}
                                selectedValue={id}
                                onValueChange={(value, _) => setId(value)}>
                                <Picker.Item color='black' key={'fazenda-selecione'} value="" label="Selecione" />
                                {(fazendas || []).map((item, key) => <Picker.Item color='black' key={`fazenda-${key}`} value={item?.id} label={item?.nome} />)}
                            </Picker>
                        </View>
                        <View>
                            <TouchableOpacity
                                style={[styles.marginVertical10, styles.marginBottom30, { backgroundColor: '#3BB54A' }]}
                            >
                                <Text style={{ color: 'black' }}>ENTRAR</Text>
                            </TouchableOpacity>
                        </View>
                    </ScrollView>
                </View>
            </TouchableWithoutFeedback>
        </KeyboardAvoidingView>
    )
}

const styles = StyleSheet.create({
    container: {
        justifyContent: 'center',
        height: `${Platform.OS === 'ios' ? '120px' : '35px'}`,
        paddingLeft: 2,
        paddingRight: 2,
        borderColor: `${props => props.theme.colors.color4.background}`,
        borderWidth: 0.7,
        borderRadius: 5,
    },
    inner: {
        paddingTop: 0,
        paddingBottom: 24,
        paddingLeft: 24,
        paddingRight: 24,
        flex: 1,
        justifyContent: 'center'
    },
    marginVertical10: {
        marginVertical: 10
    },
    marginBottom30: {
        marginBottom: 30
    },
    image: {
        width: 180,
        height: 180,
        marginTop: 50
    },
})