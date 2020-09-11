import {currentUser} from './CurrentUser.reducer.js'
import {token} from './Token.reducer.js'
import {combineReducers} from 'redux'

const rootReducer = combineReducers({
	currentUser,
	token,
})

export default rootReducer
