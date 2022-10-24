import { Picker } from '@react-native-picker/picker';
import React, { useEffect, useMemo, useRef, useState } from 'react';
import {
    Image,
    Keyboard,
    KeyboardAvoidingView,
    Platform,
    ScrollView,
    StyleSheet,
    Text,
    TouchableOpacity,
    TouchableWithoutFeedback,
    View,
} from 'react-native';

export default function Login({ onSubmit, fazendas }) {
    const [id, setId] = useState()

    return (
        <KeyboardAvoidingView
            style={styles.inner}
            behavior={Platform.OS === 'ios' ? 'position' : 'height'}
        >
            <TouchableWithoutFeedback onPress={Keyboard.dismiss}>
                <ScrollView contentContainerStyle={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
                    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', width: '100%' }}>
                        <View style={styles.image}>
                            <Image style={styles.image} source={require('../../assets/ForBov.png')} />
                        </View>
                        <View style={{ width: '100%', justifyContent: 'flex-start', alignItems: 'flex-start', paddingLeft: 5, marginBottom: 5, marginTop: 10 }}>
                            <Text style={{ color: 'black', textAlign: 'center' }}>Fazenda</Text>
                        </View>
                        <View style={styles.container}>
                            <Picker
                                placeholderTextColor='black'
                                style={{ color: '#6c757d' }}
                                itemStyle={{ height: 120, color: 'wight' }}
                                selectedValue={id}
                                onValueChange={(value, _) => setId(value)}>
                                <Picker.Item color='wigth' key={'fazenda-selecione'} value="" label="Selecione" />
                                {(fazendas || []).map((item, key) => <Picker.Item color='wight' key={`fazenda-${key}`} value={item.key} label={item.nome} />)}
                            </Picker>
                        </View>
                        <View style={{ width: '100%', marginTop: 10 }}>
                            <TouchableOpacity
                                style={[styles.marginVertical10, styles.marginBottom30, { color: '#FFFFFF', backgroundColor: '#00CEB0', padding: 10, borderRadius: 10, justifyContent: 'center', alignContent: 'center' }]}
                                onPress={() => onSubmit(id)}
                            >
                                <Text style={{ color: '#FFFFFF', textAlign: 'center', fontWeight: 'bold' }}>ENTRAR</Text>
                            </TouchableOpacity>
                        </View>
                    </View>
                </ScrollView>
            </TouchableWithoutFeedback>
        </KeyboardAvoidingView>
    )
}

const styles = StyleSheet.create({
    container: {
        justifyContent: 'center',
        height: 40,
        width: '100%',
        paddingLeft: 2,
        paddingRight: 2,
        borderColor: 'black',
        borderWidth: 0.7,
        borderRadius: 5
    },
    inner: {
        padding: 24,
        flex: 1,
        justifyContent: 'center'
    },
    marginVertical10: {
        padding: 0
    },
    marginBottom30: {
        marginBottom: 30
    },
    image: {
        width: 180,
        height: 180
    },
})