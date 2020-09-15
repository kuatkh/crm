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
	// function request(data) { return {type: allConstants.ADD_CURRENT_USER_REQUEST, data} }
	function success(data) { return {type: allConstants.ADD_CURRENT_USER_SUCCESS, data} }
	// function failure(error) { return {type: allConstants.ADD_CURRENT_USER_FAILURE, error} }
}

function addToken(token) {
	return dispatch => {
		dispatch(success(token))
	}
	function success(data) { return {type: allConstants.ADD_TOKEN_SUCCESS, data} }
	// function failure(error) { return {type: allConstants.ADD_TOKEN_FAILURE, error} }
}
