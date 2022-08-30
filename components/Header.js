import * as React from 'react';
import { View, Text } from 'react-native';

const Header = ({theader}) => {
    return(
        <View>
            <Text>{theader}</Text>
            <View style={{borderColor: 'white', borderBottomWidth: 1}}/>
        </View>
    )
}

export default Header;