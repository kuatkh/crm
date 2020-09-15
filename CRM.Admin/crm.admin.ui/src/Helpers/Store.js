import {createStore, applyMiddleware} from 'redux'
import thunkMiddleware from 'redux-thunk'
// import { createLogger } from 'redux-logger';
import rootReducer from '../Reducers'

// const loggerMiddleware = createLogger();

const store = createStore(
	rootReducer,
	applyMiddleware(
		thunkMiddleware,
		// process.env.NODE_ENV !== 'production' && loggerMiddleware
	)
)

export default store
