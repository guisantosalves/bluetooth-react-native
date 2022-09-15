import * as React from "react";

const DisplayMoney = (value) => {
    console.log(value.toString().replace("/^[$£€]\d+(?:\.\d\d)*$/g", ''))
    return value.toString().replace("^(\$?\d{1,3}(?:,?\d{3})*(?:\.\d{2})?|\.\d{2})?$", '');
}

export default DisplayMoney;