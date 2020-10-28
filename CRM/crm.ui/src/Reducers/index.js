import {combineReducers} from 'redux'
import {currentUser} from './CurrentUser.reducer.js'
import {token} from './Token.reducer.js'

const rootReducer = combineReducers({
	currentUser,
	token,
})

export default rootReducer
