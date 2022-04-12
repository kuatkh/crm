import {createContext} from 'react'
import empty from 'empty'

const defaultValue = {
	forceHide: empty.func,
	hide: empty.func,
	show: empty.func,
}

const LoadingContext = createContext(defaultValue)

export default LoadingContext
