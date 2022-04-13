import React from 'react'
import ReactDOM from 'react-dom'
import {createTheme, ThemeProvider} from '@mui/material/styles'
import App from 'Components/App'
import {Provider} from 'react-redux'
import {applyMiddleware, createStore} from 'redux'
import thunk from 'redux-thunk'
import * as allReducers from 'Reducers'
import LoadingWrapper from 'Components/LoadingWrapper'
import SnackbarWrapper from 'Components/SnackbarWrapper'
require('./index.css')

const theme = createTheme({
	typography: {
		// 'fontFamily': '"Roboto", "Helvetica", "Arial", sans-serif',
		// 'fontFamily': '"Playfair Display", serif',
		// 'fontFamily': '"Lora", serif',
		'fontFamily': '"PT Serif", serif',
	},
	zIndex: {
		mobileStepper: 1000,
		appBar: 1202,
		drawer: 1200,
		modal: 1201,
		snackbar: 1400,
		tooltip: 1500,
	},
})

const store = createStore(
	allReducers,
	applyMiddleware(thunk),
)

ReactDOM.render(
	<Provider store={store}>
		<ThemeProvider theme={theme}>
			<LoadingWrapper>
				<SnackbarWrapper>
					<App />
				</SnackbarWrapper>
			</LoadingWrapper>
		</ThemeProvider>
	</Provider>,
	document.getElementById('root')
)
