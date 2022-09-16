import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    requestResult: null,
    requestDevice: [],
}

export const appSlice = createSlice({
    name: "app",

    initialState,

    reducers: {
        requestSender: (state, action) => {
            state.requestResult = action.payload.requestResult;
        },
        requestDevice: (state, action) => {
            state.requestDevice = action.payload.requestDevice;
        }
    },
})

export const {requestSender} = appSlice.actions;

export const {requestDevice} = appSlice.actions;

export const selectRequestResult = (state) => state.app.requestResult;

export const selectRequestDevice = (state) => state.app.requestDevice;

export default appSlice.reducer;