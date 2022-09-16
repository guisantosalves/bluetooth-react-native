import * as React from "react";
import {
    View,
    TextInput,
    Text,
    StyleSheet,
    ScrollView,
    KeyboardAvoidingView,
    TouchableOpacity,
    Alert
} from "react-native";
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

// components
import ButtonAgeCattle from "./ButtonAgeCattle";
import RadioButton from "./RadioButton";
import AsyncStorage from '@react-native-async-storage/async-storage';
import uuid from "react-native-uuid";
import ButtonRaceCattle from "./ButtonRaceCattle";
import { Formik } from 'formik';

const InputOptions = ({ weight }) => {
    const weightRef = React.useRef(null)
    const earingRef = React.useRef(null)

    const [farm, setFarm] = React.useState("Fazenda feliz")
    const [earing, setearing] = React.useState()
    const [eletronicEaring, seteletronicEaring] = React.useState()
    const [age, setAge] = React.useState()
    const [race, setRace] = React.useState()
    const [value, setValue] = React.useState()
    const [sex, setSex] = React.useState();
    const [completeData, setCompleteData] = React.useState([])
    const [isPressed, setIsPressed] = React.useState(false)

    const getFocusInputWeight = () => {
        weightRef.current.focus()
    }

    const getFocusInputEaring = () => {
        earingRef.current.focus()
    }

    const insertingInRealmDB = async (values) => {
        const newString = weight.replace('[', '')
        const correctString = newString.replace(']', '')
        const chagingCommom = value.replace(",", ".")
        try {
            // const objetToInsert = {
            //     fazenda: null,
            //     brinco: earing,
            //     brincoEletronico: eletronicEaring,
            //     peso: parseFloat(correctString),
            //     idade: parseInt(age),
            //     raca: parseInt(race),
            //     valorMedio: parseFloat(chagingCommom),
            //     sexo: parseInt(sex),
            // }

            const jsonValue = JSON.stringify(values)


            await AsyncStorage.setItem(`${uuid.v4()}`, jsonValue)

            const allk = await AsyncStorage.getAllKeys()
            const ArrayOfJsonStr = await AsyncStorage.multiGet(allk)

            ArrayOfJsonStr.map((item, index) => {
                console.log(JSON.parse(item[1]))
            })
            // await AsyncStorage.getItem()
            alert("Cadastrado com sucesso")
        } catch (e) {
            alert(e)
        }
    }

    const cancel = () => {
        setAge()
        setRace()
        setValue()
        setSex()
    }

    const ConfirmVerification = (handleSubmit) => {
        Alert.alert(
            "Deseja Confirmar o cadastro",
            "Os dados que irão ser cadastrados não poderão ser editados nessa versão do app",
            [
                {
                    text: "Cancelar",
                    onPress: () => { alert("preencha novamente"); cancel() },
                    style: "cancel"
                },
                {
                    text: "Confirmar",
                    onPress: () => handleSubmit(),
                    style: "default"
                }
            ]
        )
    }

    const RegistrationSchema = Yup.object().shape({
        farm: Yup.string().max(50, 'Nome de empresa muito grande, máximo 50 caracteres').required('Required'),
        weight: Yup.number().required('Required'),
        earing: Yup.string().max(22, 'Valor maior do que 22').required('Required'),
        eletronicEaring: Yup.string().max(22, 'Valor maior do que 22').required('Required'),
        age: Yup.number().required('Required'),
        race: Yup.string().max(15).required('Required'),
        sex: Yup.string().required('Required'),
        chagingCommom: Yup.number().required('Required'),
        observation: Yup.number().max(60, 'Observação só é permitido até 50 caracteres'),
    });

    React.useMemo(() => {
        setEditable(true)
        if (weightRef.current != null) {
            getFocusInputWeight()
        }
    }, [weight])

    return (
        <Formik
            initialValues={{ weight: weight, earing: '', eletronicEaring: '', age: '', race: '', sex: '', chagingCommom: '', observation: '' }}
            onSubmit={values => insertingInRealmDB(values)}
            validationSchema={RegistrationSchema}
        >
            {({ handleChange, handleSubmit, values, errors, touched, setFieldValue }) => (
                <KeyboardAvoidingView>
                    {/* <View style={style.container}>
                            <Text style={style.textColor}>Fazenda</Text>
                            <View style={style.inputStyle}>
                            <TextInput value={farm} placeholder={"Ex: Fazendinha"} keyboardType={"none"} editable={false}/>
                            </View>
                        </View> */}
                    {/* weight */}
                    <View style={style.container}>
                        <View style={style.viewRow}>
                            <Text style={style.textColor}>Peso</Text>
                        </View>
                        <View style={style.inputStyle}>
                            <TextInput ref={weightRef} value={values.weight} onFocus={() => { setFieldValue('weight', weight), getFocusInputEaring(), setEditable(false) }} onChangeText={handleChange('weight')} numberOfLines={2} keyboardType="none" />
                        </View>
                        {errors.weight && touched.weight ? (
                            <Text style={style.textRequired}>{errors.weight}</Text>
                        ) : null}
                    </View>

                    {/* earings */}
                    <View style={style.container}>
                        <Text style={style.textColor}>Brinco</Text>
                        <View style={style.inputStyle}>
                            <TextInpu ref={earingRef} t value={values.earing?.trim()} onChangeText={handleChange('earing')} multiline={false} numberOfLines={2} />
                        </View>
                        {errors.earing && touched.earing ? (
                            <Text style={style.textRequired}>{errors.earing}</Text>
                        ) : null}
                    </View>

                    {/* eletronic earings */}
                    <View style={style.container}>
                        <Text style={style.textColor}>Brinco eletrônico</Text>
                        <View style={style.inputStyle}>
                            <TextInput value={values.eletronicEaring?.trim()} onChangeText={handleChange('eletronicEaring')} multiline={false} numberOfLines={2} />
                        </View>
                        {errors.eletronicEaring && touched.eletronicEaring ? (
                            <Text style={style.textRequired}>{errors.eletronicEaring}</Text>
                        ) : null}
                    </View>

                    {/* age of cattle */}
                    <Text style={style.textColor}>Idade</Text>
                    <ScrollView showsHorizontalScrollIndicator={false} horizontal={true} style={{ paddingLeft: 4, paddingRight: 4 }}>
                        <ButtonAgeCattle gettingValue={(value) => setFieldValue('age', value)} />
                    </ScrollView>
                    {errors.age && touched.age ? (
                        <Text style={style.textRequired}>{errors.age}</Text>
                    ) : null}

                    {/* race */}
                    <Text style={style.textColor}>Raça</Text>
                    <ScrollView horizontal={true} showsHorizontalScrollIndicator={false} style={{ paddingLeft: 4, paddingRight: 4 }}>
                        <ButtonRaceCattle gettingValue={(value) => setFieldValue('race', value)} />
                    </ScrollView>
                    {errors.race && touched.race ? (
                        <Text style={style.textRequired}>{errors.race}</Text>
                    ) : null}

                    {/*  radio button sex */}
                    <Text style={style.textColor}>Sexo</Text>
                    <View style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-around', padding: 3 }}>
                        <RadioButton getValue={(value) => setFieldValue('sex', value)} />
                    </View>
                    {errors.sex && touched.sex ? (
                        <Text style={style.textRequired}>{errors.sex}</Text>
                    ) : null}

                    {/* avarange coast */}
                    <View style={style.container}>
                        <Text style={style.textColor}>Custo médio</Text>
                        <View style={style.inputStyle}>
                            <TextInput value={values.chagingCommom} onChangeText={handleChange('chagingCommom')} numberOfLines={2} keyboardType="numeric" />
                        </View>
                        {errors.chagingCommom && touched.chagingCommom ? (
                            <Text style={style.textRequired}>{errors.chagingCommom}</Text>
                        ) : null}
                    </View>
                    <View style={style.container}>
                        <Text style={style.textColor}>Observação</Text>
                        <View style={style.inputStyle}>
                            <TextInput value={values.observation} onChangeText={handleChange('observation')} multiline={false} numberOfLines={2} />
                        </View>
                        {errors.observation && touched.observation ? (
                            <Text style={style.textRequired}>{errors.observation}</Text>
                        ) : null}
                    </View>
                    <View style={{ padding: 2, alignItems: 'flex-start', marginTop: 3 }}>
                        <TouchableOpacity style={style.saveButton} onPress={() => ConfirmVerification(handleSubmit)}>
                            <Icon name="cloud-upload" size={40} color={'#E9FFF9'} />
                        </TouchableOpacity>
                    </View>
                </KeyboardAvoidingView>
            )
            }
        </Formik >
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
        marginRight: 0,
        marginBottom: 1,
        fontWeight: 'bold',
    },
    inputStyle: {
        elevation: 1,
        borderRadius: 999,
        paddingLeft: 8,
        backgroundColor: "#E9FFF9"
    },
    saveButton: {
        borderRadius: 999,
        width: 70,
        alignItems: 'center',
        padding: 15,
        backgroundColor: "#424242"
    },
    textRequired: {
        fontWeight: 'bold',
        color: 'red'
    },
    viewRow: {
        flexDirection: "row"
    }
})