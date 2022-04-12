import {useContext} from 'react'
import SnackbarContext from './SnackbarContext'

const useSnackbar = () => {
	const snackbarContext = useContext(SnackbarContext)
	return snackbarContext
}

export default useSnackbar
