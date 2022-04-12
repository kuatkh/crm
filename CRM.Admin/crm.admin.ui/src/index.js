import 'babel-polyfill'
import React from 'react'
import ReactDOM from 'react-dom'
import {createTheme, ThemeProvider} from '@mui/material/styles'
import App from 'Components/App'
import store from 'Helpers/Store'
import {Provider} from 'react-redux'
import LoadingWrapper from 'components/LoadingWrapper'
import SnackbarWrapper from 'components/SnackbarWrapper'
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
