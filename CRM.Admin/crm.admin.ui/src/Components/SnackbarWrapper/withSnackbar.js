import React from 'react'
import useSnackbar from './useSnackbar'

export const withSnackbar = Component => {
	const WrappedComponent = props => {
		const snackbarContext = useSnackbar()

		return (
			<Component {...props} snackbar={snackbarContext} />
		)
	}

	return WrappedComponent
}

export default withSnackbar
