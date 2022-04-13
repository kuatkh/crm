import React from 'react'
import {withStyles} from '@mui/styles'
import {
	Grid,
	DialogActions,
	DialogContent,
	DialogTitle,
	Button,
	TextField,
	Divider,
	Paper,
} from '@mui/material'
import SaveIcon from '@mui/icons-material/Save'
import CancelIcon from '@mui/icons-material/Cancel'
import {appConstants} from 'constants/app.constants.js'
import {postRequest} from 'services/requests.services.js'

const styles = theme => ({
	formControl: {
		margin: theme.spacing(1),
		minWidth: 120,
		maxWidth: 300,
	},
	chips: {
		display: 'flex',
		flexWrap: 'wrap',
	},
	chip: {
		margin: 2,
	},
	noLabel: {
		marginTop: theme.spacing(3),
	},
	selectEmpty: {
		marginTop: theme.spacing(2),
	},
	container: {
		flexWrap: 'wrap',
		display: 'grid',
		gridTemplateColumns: 'repeat(12, 1fr)',
		gridGap: theme.spacing(1),
	},
	input: {
		margin: theme.spacing.unit,
	},
	button: {
		margin: theme.spacing.unit,
	},
	gapSmall: {
		marginTop: 50,
	},
	paper: {
		paddingRight: theme.spacing(1),
		// textAlign: 'center',
		color: theme.palette.text.secondary,
		whiteSpace: 'nowrap',
		marginBottom: theme.spacing(1),
		boxShadow: 'none',
	},
	divider: {
		margin: 0,
	},
	modalRoot: {
		flexGrow: 1,
	},
	actionButtons: {
		marginRight: theme.spacing(2),
	},
	headerStyle: {
		color: '#fff !important',
		backgroundColor: '#3f51b5 !important',
	},
})

class AddDictionaryData extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			id: 0,
			code: '',
			name: '',
			description: '',
			parentId: null,
			parentName: '',
			positionCategory: '',
			phoneNumber: '',
			amount: '',
			address: '',
		}
	}

	componentDidMount() {
		if (this.props.editDictionaryData) {
			this.setState({...this.props.editDictionaryData})
		}

		// this.getDepartments()
		// this.getPositions()
		// this.getRoles()
	}

	// getDepartments = () => {
	// 	const {isLoaded, handleSnackbarOpen} = this.props
	// 	if (isLoaded) {
	// 		isLoaded(false)
	// 	}

	// 	getRequest(`${appConstants.serverUrl}/api/Admin/GetDepartments`, result => {
	// 		if (isLoaded) {
	// 			isLoaded(true)
	// 		}
	// 		if (Array.isArray(result)) {
	// 			var department = null
	// 			if (this.state.departmentsId && this.state.departmentsId > 0 && result.length > 0 && result.some(r => r.id == this.state.departmentsId)) {
	// 				department = result.filter(r => r.id == this.state.departmentsId)[0]
	// 			}
	// 			this.setState({
	// 				departmentsOptions: result,
	// 				selectedDepartment: department,
	// 			})
	// 		} else if (handleSnackbarOpen) {
	// 			handleSnackbarOpen('Во время получения списка подразделений произошла ошибка', 'error')
	// 		}
	// 	},
	// 	error => {
	// 		if (isLoaded) {
	// 			isLoaded(true)
	// 		}
	// 		if (handleSnackbarOpen) {
	// 			handleSnackbarOpen(`Во время получения списка подразделений произошла ошибка: ${error}`, 'error')
	// 		}
	// 	})
	// }

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleAutocompleteChange = (e, v) => {
	this.setState({...this.state, [e]: v})
}

handleAutocompleteInputChange = (e, v) => {
	this.setState({...this.state, [e]: v})
}

handleSaveClick = () => {
	const {
		id,
		code,
		name,
		description,
		parentId,
		parentName,
		positionCategory,
		phoneNumber,
		amount,
		address,
	} = this.state
	const {handleSnackbarOpen, isLoaded, handleEditDictionaryDialogClose, dictionaryName} = this.props

	if (!name) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Название"', 'error')
		}
		return
	}

	if (isLoaded) {
		isLoaded(false)
	}

	const dictionaryData = {
		id,
		name,
		description,
	}

	postRequest(`${appConstants.serverUrl}/api/Dictionaries/Save${dictionaryName}`, dictionaryData, result => {
		if (isLoaded) {
			isLoaded(true)
		}
		if (result && result.isSuccess) {
			if (handleEditDictionaryDialogClose) {
				handleEditDictionaryDialogClose(true)
			}
		} else if (handleSnackbarOpen) {
			handleSnackbarOpen(`Во время сохранения произошла ошибка. ${result && !result.isSuccess && result.msg ? result.msg : ''}`, 'error')
		}
	},
	error => {
		if (isLoaded) {
			isLoaded(true)
		}
		if (handleSnackbarOpen) {
			handleSnackbarOpen(`Во время сохранения произошла ошибка: ${error}`, 'error')
		}
	})
}

handleCancelClick = () => {
	this.setState({
		id: 0,
		code: '',
		name: '',
		description: '',
		parentId: null,
		parentName: '',
		positionCategory: '',
		phoneNumber: '',
		amount: '',
		address: '',
	})
	if (this.props.handleEditDictionaryDialogClose) {
		this.props.handleEditDictionaryDialogClose()
	}
}

render() {
	const {classes, showDescriptionsFields, pageTitle} = this.props
	return (
		<React.Fragment>
			<DialogTitle className={classes.headerStyle} id='add-Dictionary-dialog-title'>{pageTitle ? pageTitle + '. ' : ''}Редактирование</DialogTitle>
			<DialogContent dividers={true}>
				<div className={classes.modalRoot}>
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									name='name'
									error={(!this.state.name)}
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.name}
									label='Название'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						{
							showDescriptionsFields && (
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											required
											name='description'
											error={(!this.state.description)}
											fullWidth
											multiline
											rows={3}
											size='small'
											autoComplete='off'
											value={this.state.description}
											label='Описание'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
								</Grid>
							)
						}
					</Grid>
					<Divider className={classes.divider} />
				</div>
			</DialogContent>
			<DialogActions>
				<Button onClick={this.handleCancelClick} startIcon={<CancelIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='secondary'>
					Отменить
				</Button>
				<Button onClick={this.handleSaveClick} startIcon={<SaveIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='primary'>
					Сохранить
				</Button>
			</DialogActions>
		</React.Fragment>
	)
}
}

export default withStyles(styles, {withTheme: true})(AddDictionaryData)
