import * as React from 'react';
import {
  View,
  TextInput,
  Text,
  StyleSheet,
  ScrollView,
  KeyboardAvoidingView,
  TouchableOpacity,
  Alert,
} from 'react-native';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

// components
import ButtonAgeCattle from './ButtonAgeCattle';
import RadioButton from './RadioButton';
import AsyncStorage from '@react-native-async-storage/async-storage';
import uuid from 'react-native-uuid';
import ButtonRaceCattle from './ButtonRaceCattle';
import * as Yup from 'yup';
import {Formik} from 'formik';
import {useSelector} from 'react-redux';
import SQLite from 'react-native-sqlite-storage';

SQLite.enablePromise(true);

const InputOptions = ({peso, devices, functionGetWeight}) => {
  const [pesoManual, setPesoManual] = React.useState('');
  const weightRef = React.useRef(null);
  const earingRef = React.useRef(null);

  // redux
  const fazendaSelector = useSelector(state => state.counter.requestFazenda);

  // db -> transaction -> tx
  const creatingTableAndInsertingIt = async () => {
    let db = await SQLite.openDatabase({name: 'fazenda.db'});

    await db.executeSql(`
        'CREATE TABLE IF NOT EXISTS Pesagens (ID INTEGER PRIMARY KEY AUTOINCREMENT, tipoMovimentacao INTEGER, peso REAL, brinco TEXT, brincoEL TEXT, idade INTEGER, raca INTEGER, sexo INTEGER, custoMedio REAL, observacao TEXT)
    `);

    // db.transaction(tx => {
    //   tx.executeSql(
    //     'CREATE TABLE IF NOT EXISTS ' +
    //       'Pesagens ' +
    //       '(ID INTEGER PRIMARY KEY AUTOINCREMENT, peso REAL, brinco TEXT, brincoEL TEXT, idade INTEGER, raca INTEGER, sexo INTEGER, custoMedio REAL, observacao TEXT)',
    //   );
    // });
  };

  // variables for tipo de movimentação field
  const movimentacaoArray = [
    {
      id: 0,
      value: 'Entrada',
    },
    {
      id: 1,
      value: 'Saída',
    },
  ];
  // variables for sexo field
  const sexoArray = [
    {
      id: 1,
      value: 'Macho',
    },
    {
      id: 2,
      value: 'Femea',
    },
  ];

  const getFocusInputWeight = () => {
    weightRef.current.focus();
  };

  const getFocusInputEaring = () => {
    earingRef.current.focus();
  };

  const insertingInRealmDB = async values => {
    try {
      // peso: peso ? peso : 0,
      //   brinco: '',
      //   brincoEletronico: '',
      //   idade: '',
      //   raca: '',
      //   sexo: '',
      //   valorMedio: '',
      //   observacao: '',
      //   dataCriacao: new Date(),
      //   pesoManual: !(devices.length > 0),
      //   tipoMovimentacao: 0,

      let db = await SQLite.openDatabase({name: 'fazenda.db'});
      await db.executeSql(`
      CREATE TABLE IF NOT EXISTS Pesagem (ID INTEGER PRIMARY KEY AUTOINCREMENT, tipoMovimentacao INTEGER, peso REAL, brinco TEXT, brincoEletronico TEXT, idade INTEGER, raca INTEGER, sexo INTEGER, valorMedio REAL, observacao TEXT, dataCriacao TEXT, pesoManual TEXT, fazenda TEXT)
      `);

      if (db) {
        db.executeSql(`
        INSERT INTO Pesagem (tipoMovimentacao, peso, brinco, brincoEletronico, idade, raca, sexo, valorMedio, observacao, dataCriacao, pesoManual, fazenda)
        VALUES (${values.tipoMovimentacao}, ${values.peso}, '${values.brinco}', '${values.brincoEletronico}', ${values.idade}, ${values.raca}, ${values.sexo}, ${values.valorMedio}, '${values.observacao}', '${values.dataCriacao}', '${values.pesoManual}', '${fazendaSelector}')
        `);
      }

      alert('Cadastrado com sucesso');
    } catch (e) {
      console.log(e);
    }
  };

  const ConfirmVerification = (handleSubmit, resetForm) => {
    Alert.alert(
      'Deseja Confirmar o cadastro',
      'Os dados que irão ser cadastrados não poderão ser editados nessa versão do app',
      [
        {
          text: 'Cancelar',
          onPress: () => {
            alert('preencha novamente');
            resetForm();
          },
          style: 'cancel',
        },
        {
          text: 'Confirmar',
          onPress: () => handleSubmit(),
          style: 'default',
        },
      ],
    );
  };

  const RegistrationSchema = Yup.object().shape({
    // farm: Yup.string().max(50, 'Nome de empresa muito grande, máximo 50 caracteres').required('Required'),
    peso: Yup.number().required('Required'),
    brinco: Yup.string().max(22, 'Valor maior do que 22').required('Required'),
    brincoEletronico: Yup.string()
      .max(22, 'Valor maior do que 22')
      .required('Required'),
    idade: Yup.string().required('Required'),
    raca: Yup.string().required('Required'),
    sexo: Yup.string().required('Required'),
    valorMedio: Yup.number().required('Required'),
    observacao: Yup.string().max(
      60,
      'Observação só é permitido até 60 caracteres',
    ),
    dataCriacao: Yup.date(),
    pesoManual: Yup.bool().required('Required'),
    tipoMovimentacao: Yup.number().required('Required'),
  });

  React.useMemo(() => {
    if (weightRef.current != null) {
      getFocusInputWeight();
    }
  }, [peso]);

  return (
    <Formik
      initialValues={{
        peso: peso ? peso : 0,
        brinco: '',
        brincoEletronico: '',
        idade: '',
        raca: '',
        sexo: '',
        valorMedio: '',
        observacao: '',
        dataCriacao: new Date(),
        pesoManual: !(devices?.length > 0),
        tipoMovimentacao: 0,
      }}
      onSubmit={(values, {resetForm}) => {
        insertingInRealmDB(values);
        resetForm();
      }}
      validationSchema={RegistrationSchema}>
      {({
        handleChange,
        handleSubmit,
        values,
        errors,
        touched,
        setFieldValue,
        resetForm,
      }) => (
        <KeyboardAvoidingView>
          <View style={style.container}>
            {/* Operation Type */}
            <Text style={style.textColor}>Tipo da Movimentação</Text>
            <View
              style={{
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'space-around',
                padding: 3,
              }}>
              <RadioButton
                array={movimentacaoArray}
                onSelected={value => {
                  setFieldValue('tipoMovimentacao', value);
                }}
              />
              <TouchableOpacity 
                onPress={functionGetWeight}
                style={style.buttonActionGetPeso}
                >
                <Text style={{fontSize: 18, fontWeight: 'bold', letterSpacing: 1, color: 'white', marginRight: 5}}>PESO</Text>
                <Icon name='cow' size={24} color={'white'}/>
              </TouchableOpacity>
            </View>
            {errors.tipoMovimentacao && touched.tipoMovimentacao ? (
              <Text style={style.textRequired}>{errors.tipoMovimentacao}</Text>
            ) : null}

            {/* weight */}
            <View style={style.viewRow}>
              <Text style={style.textColor}>Peso</Text>
            </View>
            <View>
                <View
                  style={{ 
                    elevation: 1,
                    borderRadius: 999,
                    paddingLeft: 8,
                    backgroundColor: '#E9FFF9',
                  }}>
                  {peso ? (
                    <TextInput
                      ref={weightRef}
                      value={values.peso}
                      onFocus={() => {
                        setFieldValue('peso', peso), getFocusInputEaring();
                      }}
                      onChangeText={handleChange('peso')}
                      numberOfLines={2}
                      keyboardType="none"
                    />
                  ) : (
                    <TextInput
                      ref={weightRef}
                      value={values.peso}
                      onChangeText={p => {
                        setFieldValue('peso', p);
                      }}
                      numberOfLines={2}
                      keyboardType="numeric"
                    />
                  )}
                </View>
                {errors.peso && touched.peso ? (
                  <Text style={style.textRequired}>{errors.peso}</Text>
                ) : null}
              </View>

            </View>
          {/* earings */}
          <View style={style.container}>
            <Text style={style.textColor}>Brinco</Text>
            <View style={style.inputStyle}>
              <TextInput
                ref={earingRef}
                t
                value={values.brinco?.trim()}
                onChangeText={handleChange('brinco')}
                multiline={false}
                numberOfLines={2}
              />
            </View>
            {errors.brinco && touched.brinco ? (
              <Text style={style.textRequired}>{errors.brinco}</Text>
            ) : null}
          </View>

          {/* eletronic earings */}
          <View style={style.container}>
            <Text style={style.textColor}>Brinco eletrônico</Text>
            <View style={style.inputStyle}>
              <TextInput
                value={values.brincoEletronico?.trim()}
                onChangeText={handleChange('brincoEletronico')}
                multiline={false}
                numberOfLines={2}
              />
            </View>
            {errors.brincoEletronico && touched.brincoEletronico ? (
              <Text style={style.textRequired}>{errors.brincoEletronico}</Text>
            ) : null}
          </View>

          {/* age of cattle */}
          <Text style={style.textColor}>Idade</Text>
          <ScrollView
            showsHorizontalScrollIndicator={false}
            horizontal={true}
            style={{paddingLeft: 4, paddingRight: 4}}>
            <ButtonAgeCattle
              gettingValue={value => setFieldValue('idade', value)}
            />
          </ScrollView>
          {errors.idade && touched.idade ? (
            <Text style={style.textRequired}>{errors.idade}</Text>
          ) : null}

          {/* race */}
          <Text style={style.textColor}>Raça</Text>
          <ScrollView
            horizontal={true}
            showsHorizontalScrollIndicator={false}
            style={{paddingLeft: 4, paddingRight: 4}}>
            <ButtonRaceCattle
              gettingValue={value => setFieldValue('raca', value)}
            />
          </ScrollView>
          {errors.raca && touched.raca ? (
            <Text style={style.textRequired}>{errors.raca}</Text>
          ) : null}

          {/*  radio button sex */}
          <Text style={style.textColor}>Sexo</Text>
          <View
            style={{
              flexDirection: 'row',
              alignItems: 'center',
              justifyContent: 'space-around',
              padding: 3,
            }}>
            <RadioButton
              array={sexoArray}
              onSelected={value => {
                setFieldValue('sexo', value);
              }}
            />
          </View>
          {errors.sexo && touched.sexo ? (
            <Text style={style.textRequired}>{errors.sexo}</Text>
          ) : null}

          {/* avarange coast */}
          <View style={style.container}>
            <Text style={style.textColor}>Custo médio</Text>
            <View style={style.inputStyle}>
              <TextInput
                value={values.valorMedio}
                onChangeText={handleChange('valorMedio')}
                numberOfLines={2}
                keyboardType="numeric"
              />
            </View>
            {errors.valorMedio && touched.valorMedio ? (
              <Text style={style.textRequired}>{errors.valorMedio}</Text>
            ) : null}
          </View>
          <View style={style.container}>
            <Text style={style.textColor}>Observação</Text>
            <View style={style.inputStyle}>
              <TextInput
                value={values.observacao}
                onChangeText={handleChange('observacao')}
                multiline={false}
                numberOfLines={2}
              />
            </View>
            {errors.observacao && touched.observacao ? (
              <Text style={style.textRequired}>{errors.observacao}</Text>
            ) : null}
          </View>
          <View style={{padding: 2, alignItems: 'flex-start', marginTop: 3}}>
            <TouchableOpacity
              style={style.saveButton}
              onPress={() => ConfirmVerification(handleSubmit, resetForm)}>
              <Icon name="cloud-upload" size={40} color={'#E9FFF9'} />
            </TouchableOpacity>
          </View>
        </KeyboardAvoidingView>
      )}
    </Formik>
  );
};

export default InputOptions;

const style = StyleSheet.create({
  container: {
    padding: 3,
    marginTop: 2,
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
    backgroundColor: '#E9FFF9',
  },
  saveButton: {
    borderRadius: 999,
    width: 70,
    alignItems: 'center',
    padding: 15,
    backgroundColor: '#424242',
  },
  textRequired: {
    marginLeft: 8,
    fontWeight: 'bold',
    color: 'red',
  },
  viewRow: {
    flexDirection: 'row',
  },
  buttonActionGetPeso: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 24,
    paddingVertical: 10,
    borderRadius: 8,
    backgroundColor: '#424242',
    elevation: 10
  }
});
