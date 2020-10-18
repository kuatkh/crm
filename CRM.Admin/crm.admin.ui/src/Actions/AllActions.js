import {allConstants} from '../Constants/AllConstants.js'
import {allServices} from '../Services/AllServices.js'

export const allActions = {
	addCurrentUser,
	addToken,
}

function addCurrentUser(userData) {
	return dispatch => {
		allServices.addCurrentUser(userData)
		dispatch(success(userData))
	}
	function success(data) { return {type: allConstants.ADD_CURRENT_USER_SUCCESS, data} }
}

function addToken(token) {
	return dispatch => {
		dispatch(success(token))
	}
	function success(data) { return {type: allConstants.ADD_TOKEN_SUCCESS, data} }
}
