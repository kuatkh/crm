import React, {useReducer, useRef} from 'react'
import {Snackbar, Alert} from '@mui/material'
import SnackbarContext from './SnackbarContext'

const initialState = {openSnackbar: false, message: '', snackbarSeverity: 'info'}

const reducer = ({openSnackbar, message, snackbarSeverity}, action) => {
	switch (action.type) {
		case 'SHOW_SUCCESS':
			return {openSnackbar: true, message: action.message, snackbarSeverity: 'success'}
		case 'SHOW_ERROR':
			return {openSnackbar: true, message: action.message, snackbarSeverity: 'error'}
		case 'SHOW_WARNING':
			return {openSnackbar: true, message: action.message, snackbarSeverity: 'warning'}
		case 'HIDE':
			return {openSnackbar: false, message, snackbarSeverity}
		case 'RESET':
			return {openSnackbar: false, message: '', snackbarSeverity: 'info'}
		default:
			return {openSnackbar, message, snackbarSeverity}
	}
}

const SnackbarWrapper = props => {
	const {
		children,
		className,
	} = props

	const [state, dispatch] = useReducer(reducer, initialState)

	const contextValue = useRef({
		hide: () => dispatch({type: 'HIDE'}),
		showSuccess: message => dispatch({type: 'SHOW_SUCCESS', message}),
		showError: message => dispatch({type: 'SHOW_ERROR', message}),
		showWarning: message => dispatch({type: 'SHOW_WARNING', message}),
	})

	return (
		<>
			<Snackbar
				anchorOrigin={{vertical: 'bottom', horizontal: 'center'}}
				autoHideDuration={6000}
				open={state.openSnackbar}
				onClose={() => dispatch({type: 'HIDE'})}
			>
				<Alert onClose={() => dispatch({type: 'HIDE'})} severity={state.snackbarSeverity}>{state.message}</Alert>
			</Snackbar>
			<SnackbarContext.Provider value={contextValue.current}>
				{children}
			</SnackbarContext.Provider>
		</>
	)
}

export default SnackbarWrapper
