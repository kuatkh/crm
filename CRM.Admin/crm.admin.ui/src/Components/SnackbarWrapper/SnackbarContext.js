import {createContext} from 'react'
import empty from 'empty'

const defaultValue = {
	hide: empty.func,
	showSuccess: empty.func,
	showError: empty.func,
	showWarning: empty.func,
}

const SnackbarContext = createContext(defaultValue)

export default SnackbarContext
