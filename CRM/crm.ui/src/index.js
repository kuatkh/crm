import 'babel-polyfill'
import React from 'react'
import ReactDOM from 'react-dom'
import {createMuiTheme, MuiThemeProvider} from '@material-ui/core/styles'
import App from './Components/App'
import store from './Helpers/Store'
import {Provider} from 'react-redux'
require('./index.css')

const newTheme = createMuiTheme({
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
		<MuiThemeProvider theme={newTheme}>
			<App />
		</MuiThemeProvider>
	</Provider>,
	document.getElementById('root')
)
