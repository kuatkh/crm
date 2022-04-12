export const allConstants = {

	/* eslint-disable*/
	serverUrl: process.env.NODE_ENV == 'development' ? 'https://localhost:5001' : '',
	/* eslint-enable*/
	requestHeaders: {
		'Content-Type': 'application/json',
		'Cache-Control': 'no-cache, no-store, must-revalidate',
		'Pragma': 'no-cache',
		'Expires': 0,
	},

	ADD_CURRENT_USER_REQUEST: 'ADD_CURRENT_USER_REQUEST',
	ADD_CURRENT_USER_SUCCESS: 'ADD_CURRENT_USER_SUCCESS',
	ADD_CURRENT_USER_FAILURE: 'ADD_CURRENT_USER_FAILURE',

	ADD_TOKEN_SUCCESS: 'ADD_TOKEN_SUCCESS',
	ADD_TOKEN_FAILURE: 'ADD_TOKEN_FAILURE',

}
