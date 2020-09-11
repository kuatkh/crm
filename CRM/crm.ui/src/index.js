import 'babel-polyfill'
import React from 'react'
import ReactDOM from 'react-dom'
// import './index.css'
import App from './Components/App'
import store from './Helpers/Store'
import {Provider} from 'react-redux'
require('./index.css')

ReactDOM.render(
	<Provider store={store}>
		<App />
	</Provider>,
	document.getElementById('root')
)
