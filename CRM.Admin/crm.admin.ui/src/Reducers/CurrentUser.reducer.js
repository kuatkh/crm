import {allConstants} from '../Constants/AllConstants'

const initialState = localStorage.getItem('currentUser') ? JSON.parse(localStorage.getItem('currentUser')) : null

export function currentUser(state = initialState, action) {

	switch (action.type) {
		case allConstants.ADD_CURRENT_USER_REQUEST:
			return null
		case allConstants.ADD_CURRENT_USER_SUCCESS:
			return action.data
		case allConstants.ADD_CURRENT_USER_FAILURE:
			return null
		default:
			return state
	}
}
