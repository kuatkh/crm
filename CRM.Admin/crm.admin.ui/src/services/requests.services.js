import {appConstants} from 'constants/app.constants.js'
import {tokenServices} from 'services/token.services'

const getRequest = (url, successCallback, errorCallback) => {
	fetch(url, {
		method: 'GET',
		headers: {
			...appConstants.requestHeaders,
			Authorization: `Bearer ${(tokenServices.getToken() || '')}`,
		},
	})
		.then(res => {
			if (res.status === 401 || res.status === 403 || res.status === 302) {
				localStorage.clear()
				sessionStorage.clear()
				location.reload()
			} else {
				return res.json()
			}
		})
		.then(
			result => {
				if (successCallback) {
					successCallback(result)
				}
			},
			error => {
				console.log(error)
				if (errorCallback) {
					errorCallback(error)
				}
			}
		)
}

const postRequest = (url, body, successCallback, errorCallback) => {
	fetch(url, {
		method: 'POST',
		headers: {
			...appConstants.requestHeaders,
			Authorization: `Bearer ${(tokenServices.getToken() || '')}`,
		},
		body: JSON.stringify(body),
	})
		.then(res => {
			if (res.status === 401 || res.status === 403 || res.status === 302) {
				localStorage.clear()
				sessionStorage.clear()
				location.reload()
			} else {
				return res.json()
			}
		})
		.then(
			result => {
				if (successCallback) {
					successCallback(result)
				}
			},
			error => {
				console.log(error)
				if (errorCallback) {
					errorCallback(error)
				}
			}
		)
}

export {getRequest, postRequest}
