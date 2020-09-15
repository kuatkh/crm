import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import Autocomplete from '@material-ui/lab/Autocomplete'
import TextField from '@material-ui/core/TextField'
import InputAdornment from '@material-ui/core/InputAdornment'
import IconButton from '@material-ui/core/IconButton'
import Visibility from '@material-ui/icons/Visibility'
import VisibilityOff from '@material-ui/icons/VisibilityOff'
import Button from '@material-ui/core/Button'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import DateFnsUtils from '@date-io/date-fns'
import ruLocale from 'date-fns/locale/ru'
import {MuiPickersUtilsProvider, KeyboardDatePicker} from '@material-ui/pickers'
import {allConstants} from '../../Constants/AllConstants.js'
import {getRequest, postRequest} from '../../Services/RequestsServices.js'

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
			nameRu: '',
			nameKz: '',
			nameEn: '',
			descriptionRu: '',
			descriptionKz: '',
			descriptionEn: '',
			parentId: null,
			parentNameRu: '',
			parentNameKz: '',
			parentNameEn: '',
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
	// 	const {token, isLoaded, handleSnackbarOpen} = this.props
	// 	if (isLoaded) {
	// 		isLoaded(false)
	// 	}

	// 	getRequest(`${allConstants.serverUrl}/api/Admin/GetDepartments`, token, result => {
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
		nameRu,
		nameKz,
		nameEn,
		descriptionRu,
		descriptionKz,
		descriptionEn,
		parentId,
		parentNameRu,
		parentNameKz,
		parentNameEn,
		positionCategory,
		phoneNumber,
		amount,
		address,
	} = this.state
	const {token, handleSnackbarOpen, isLoaded, handleEditDictionaryDialogClose, dictionaryName} = this.props

	if (!nameRu) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Название (рус.)"', 'error')
		}
		return
	}

	if (isLoaded) {
		isLoaded(false)
	}

	const dictionaryData = {
		id,
		nameRu,
		nameKz,
		nameEn,
		descriptionRu,
		descriptionKz,
		descriptionEn,
	}

	postRequest(`${allConstants.serverUrl}/api/Dictionaries/Save${dictionaryName}`, token, dictionaryData, result => {
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
		nameRu: '',
		nameKz: '',
		nameEn: '',
		descriptionRu: '',
		descriptionKz: '',
		descriptionEn: '',
		parentId: null,
		parentNameRu: '',
		parentNameKz: '',
		parentNameEn: '',
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
									name='nameRu'
									error={(!this.state.nameRu)}
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.nameRu}
									label='Название (рус.)'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
							<Paper className={classes.paper}>
								<TextField
									required
									name='nameKz'
									error={(!this.state.nameKz)}
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.nameKz}
									label='Название (каз.)'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
							<Paper className={classes.paper}>
								<TextField
									required
									name='nameEn'
									error={(!this.state.nameEn)}
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.nameEn}
									label='Название (англ.)'
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
											name='descriptionRu'
											error={(!this.state.descriptionRu)}
											fullWidth
											multiline
											rows={3}
											size='small'
											autoComplete='off'
											value={this.state.descriptionRu}
											label='Описание (рус.)'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
									<Paper className={classes.paper}>
										<TextField
											required
											name='descriptionKz'
											error={(!this.state.descriptionKz)}
											fullWidth
											multiline
											rows={3}
											size='small'
											autoComplete='off'
											value={this.state.descriptionKz}
											label='Описание (каз.)'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
									<Paper className={classes.paper}>
										<TextField
											required
											name='descriptionEn'
											error={(!this.state.descriptionEn)}
											fullWidth
											multiline
											rows={3}
											size='small'
											autoComplete='off'
											value={this.state.descriptionEn}
											label='Описание (англ.)'
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
