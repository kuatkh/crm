import React, {useReducer, useRef} from 'react'
import {Backdrop, CircularProgress} from '@mui/material'
import LoadingContext from './LoadingContext'

const initialState = {loadingElements: 0}

const reducer = ({loadingElements}, action) => {
	switch (action.type) {
		case 'INCREMENT':
			return {loadingElements: loadingElements + 1}
		case 'DECREMENT':
			return {loadingElements: loadingElements > 0 ? loadingElements - 1 : 0}
		case 'RESET':
			return {loadingElements: 0}
		default:
			return {loadingElements}
	}
}

const LoadingWrapper = props => {
	const {
		children,
		className,
	} = props

	const [state, dispatch] = useReducer(reducer, initialState)

	const contextValue = useRef({
		forceHide: () => dispatch({type: 'RESET'}),
		hide: () => dispatch({type: 'DECREMENT'}),
		show: () => dispatch({type: 'INCREMENT'}),
	})

	return (
		<>
			{
				Boolean(state.loadingElements) && <Backdrop
					sx={{color: '#fff', zIndex: theme => theme.zIndex.drawer + 1000}}
					open={true}
				>
					<CircularProgress color='inherit' />
				</Backdrop>
			}
			<LoadingContext.Provider value={contextValue.current}>
				{children}
			</LoadingContext.Provider>
		</>
	)
}

export default LoadingWrapper
