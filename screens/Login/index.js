import AsyncStorage from '@react-native-async-storage/async-storage';
import { useFocusEffect } from '@react-navigation/native';
import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Modal, StyleSheet, Text, View } from 'react-native';
import LinearGradient from 'react-native-linear-gradient';
import uuid from "react-native-uuid";
import { useSelector,  useDispatch} from 'react-redux';
import {requestFazenda} from "../../features/appSlice";

import Splash from '../Splash';
import Login from './Login';

export default function ({ navigation }) {
    const [fazendas, setFazendas] = useState([{ nome: 'Taboca', key: 56 }, { nome: 'Sao Jose', key: 57 }])
    const [fazendaLogada, setFazendaLogada] = useState(null)
    const [openSplashScreen, setOpenSplashScreen] = useState(true)
    const [openModal, setOpenModal] = useState(false)
    const [width, setWidth] = useState(null)
    const [widthTotal, setWidthTotal] = useState(100)
    
    // redux
    const dispatch = useDispatch();

    const buscarFazendaLogada = async () => {
        const fazenda = await AsyncStorage.getItem('fazenda')

        if (fazenda) {
            setFazendaLogada(fazenda)
        }
    }

    const carregando = async (id) => {
        try {
            setOpenModal(true)
            setWidth(0)

            dispatch(requestFazenda({
                requestFazenda: id,
            }))

        } catch (error) {
            alert(error)
        } finally {
            setTimeout(() => setOpenModal(false), 5000)
        }
    }

    useFocusEffect(useCallback(() => {
        const buscar = async () => {
            await buscarFazendaLogada()
        }
        buscar()
    }, []))

    useEffect(() => {
        if (width < widthTotal && openModal) {
            setTimeout(() => {
                if (width == 90) {
                    setWidth(width + 12)
                } else {
                    setWidth(width + 10)
                }
            }, 1000)
        }
    }, [width, openModal])

    const onSubmit = async (id) => {
        if (!id){
            return alert('Você precisa selecionar uma fazenda!')
        } else {
            setFazendaLogada(fazendas.find((q)=>q.key = id))
        }

        await carregando(id)
    }
    if (openModal) return (
        <View style={styles.container}>
            <Modal
                animationType='fade'
                transparent={false}
                visible={openModal}
                onRequestClose={() => setOpenModal(false)}>
                <View style={styles.modal}>
                    <View style={{ alignItems: 'flex-start', justifyContent: 'flex-start', color: '#000000' }}>
                        <Text style={{ color: '#000000' }}>Validando fazenda</Text>
                    </View>
                    <View style={{ borderRadius: 10, width: '80%', justifyContent: 'center', alignItems: 'flex-start', borderWidth: 2, borderColor: '#1D50A8', height: 30 }}>
                        {/* <View style={{ margin: 2, borderRadius: 10, width: `${width}%`, backgroundColor: '#424242', height: '100%' }}></View> */}
                        <LinearGradient start={{ x: 0, y: 5 }} end={{ x: 1, y: 0.5 }} colors={['#1D50A8', '#00CEB0']} style={{ marginLeft: -2, borderRadius: 10, width: `${width}%`, height: 30 }}></LinearGradient>
                    </View>
                </View>
            </Modal>
        </View>)
    if (openSplashScreen) return <Splash navigation={navigation} setOpen={setOpenSplashScreen} />

    if (!fazendaLogada) return <Login onSubmit={onSubmit} fazendas={fazendas} />
    if (fazendaLogada) return navigation.push('Home')

}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        align: 'center',
        justifyContent: 'center',
        backgroundColor: '#FFFFFF',
    },
    modal: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: "#FFFFFF",
        borderRadius: 10,
        borderWidth: 1,
        borderColor: '#fff',

    },
    text: {
        color: '#3f2949',
        marginTop: 10
    }
})