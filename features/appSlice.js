import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    requestResult: null,
}

export const appSlice = createSlice({
    name: "app",

    initialState,

    reducers: {
        requestSender: (state, action) => {
            state.requestResult = action.payload.requestResult;
        }
    },
})

export const {requestSender} = appSlice.actions;

export const selectRequestResult = (state) => state.app.requestResult;

export default appSlice.reducer;