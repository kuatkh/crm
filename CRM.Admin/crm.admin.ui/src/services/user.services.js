export const userServices = {
	addCurrentUser,
	getCurrentUser,
}

function addCurrentUser(userData) {
	if (userData) {
		localStorage.setItem('currentUser', JSON.stringify(userData))
		return userData
	}
	return null
}

function getCurrentUser() {
	const userData = localStorage.getItem('currentUser')
	if (userData) {
		return JSON.parse(userData)
	}
	return null
}
