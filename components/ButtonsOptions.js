import * as React from "react";
import { TouchableHighlight } from "react-native";

const ButtonsOptions = ({title, color, effect}) => {
    return(
        <TouchableHighlight style={{backgroundColor: color}} onPress={effect}>
            <Text>{title}</Text>
        </TouchableHighlight>
    )
}

export default ButtonsOptions;