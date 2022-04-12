import React from 'react'
import useLoading from './useLoading'

export const loading = Component => {
	const WrappedComponent = props => {
		const loadingContext = useLoading()

		return (
			<Component {...props} loadingScreen={loadingContext} />
		)
	}

	return WrappedComponent
}

export default loading
