export const tokenServices = {
	getToken,
	setToken,
}

function getToken() {
	let token = localStorage.getItem('authToken')
	if (!token) {
		token = sessionStorage.getItem('authToken')
	}
	return token
}

function setToken(token, isRemember) {
	if (isRemember) {
		localStorage.setItem('authToken', setToken)
	} else {
		sessionStorage.setItem('authToken', setToken)
	}
	return token
}
