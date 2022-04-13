import {allConstants} from 'Constants/AllConstants'

const initialState = localStorage.getItem('crmToken') || null

export function token(state = initialState, action) {

	switch (action.type) {
		case allConstants.ADD_TOKEN_SUCCESS:
			return action.data
		case allConstants.ADD_TOKEN_FAILURE:
			return null
		default:
			return state
	}

}
