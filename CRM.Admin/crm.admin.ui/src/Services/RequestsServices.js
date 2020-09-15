import {allConstants} from '../Constants/AllConstants.js'

const getRequest = (url, token, successCallback, errorCallback) => {
	fetch(url, {
		method: 'GET',
		headers: {
			...allConstants.requestHeaders,
			Authorization: `Bearer ${(token || localStorage.getItem('abToken'))}`,
		},
	})
		.then(res => {
			if (res.status === 401 || res.status === 403 || res.status === 302) {
				localStorage.clear()
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

const postRequest = (url, token, body, successCallback, errorCallback) => {
	let requestHeaders = {
		...allConstants.requestHeaders,
		Authorization: `Bearer ${(token || localStorage.getItem('abToken'))}`,
	}
	if (token == 'init') {
		delete requestHeaders.Authorization
	}
	fetch(url, {
		method: 'POST',
		headers: {...requestHeaders},
		body: JSON.stringify(body),
	})
		.then(res => {
			if (res.status === 401 || res.status === 403 || res.status === 302) {
				localStorage.clear()
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
