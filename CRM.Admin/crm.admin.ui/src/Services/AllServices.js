export const allServices = {
	addCurrentUser,
}

function addCurrentUser(userData) {
	if (userData) {
		localStorage.setItem('currentUser', JSON.stringify(userData))
		return userData
	}
	return null
}
