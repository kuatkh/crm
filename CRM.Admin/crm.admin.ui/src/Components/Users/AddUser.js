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
		marginTop:50,
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

class AddUser extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			id: 0,
			userName: '',
			userSecret: '',
			userSecretConfirmation: '',
			showUserSecret: false,
			showUserSecretConfirmation: false,
			rolesOptions: [],
			selectedRole: null,
			email: '',
			iin: '',
			birthDate: new Date(),
			phoneNumber: '',
			nameRu: '',
			surnameRu: '',
			middlenameRu: '',
			enterprisesId: 0,
			departmentsId: 0,
			positionsId: 0,
			roleId: 0,
			enterprisesOptions: [],
			selectedEnterprise: null,
			departmentsOptions: [],
			selectedDepartment: null,
			positionsOptions: [],
			selectedPosition: null,
			newDepartmentStr: '',
			newPositionStr: '',
		}

		this.regexStr = '^[0-9]*$'
		this.userSecretRegexStr = '^(?:[A-Za-z]+|\d+)$'
	}

	componentDidMount() {
		if (this.props.editUserData) {
			this.setState({...this.props.editUserData})
		}

		this.getEnterprises()
		this.getDepartments()
		this.getPositions()
		this.getRoles()
	}

	getEnterprises = () => {
		const {token, isLoaded, handleSnackbarOpen} = this.props
		if (isLoaded) {
			isLoaded(false)
		}

		getRequest(`${allConstants.serverUrl}/api/Admin/GetEnterprises`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (Array.isArray(result)) {
				var enterprise = null
				if (this.state.enterprisesId && this.state.enterprisesId > 0 && result.length > 0 && result.some(r => r.id == this.state.enterprisesId)) {
					enterprise = result.filter(r => r.id == this.state.enterprisesId)[0]
				} else if (result.length == 1) {
					enterprise = result[0]
				}
				this.setState({
					enterprisesOptions: result,
					selectedEnterprise: enterprise,
				})
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время получения списка компаний/филиалов произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время получения списка компаний/филиалов произошла ошибка: ${error}`, 'error')
			}
		})
	}

	getDepartments = () => {
		const {token, isLoaded, handleSnackbarOpen} = this.props
		if (isLoaded) {
			isLoaded(false)
		}

		getRequest(`${allConstants.serverUrl}/api/Admin/GetDepartments`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (Array.isArray(result)) {
				var department = null
				if (this.state.departmentsId && this.state.departmentsId > 0 && result.length > 0 && result.some(r => r.id == this.state.departmentsId)) {
					department = result.filter(r => r.id == this.state.departmentsId)[0]
				} else if (result.length == 1) {
					department = result[0]
				}
				this.setState({
					departmentsOptions: result,
					selectedDepartment: department,
				})
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время получения списка подразделений произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время получения списка подразделений произошла ошибка: ${error}`, 'error')
			}
		})
	}

	getPositions = () => {
		const {token, isLoaded, handleSnackbarOpen} = this.props

		if (isLoaded) {
			isLoaded(false)
		}

		getRequest(`${allConstants.serverUrl}/api/Admin/GetPositions`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (Array.isArray(result)) {
				var position = null
				if (this.state.positionsId && this.state.positionsId > 0 && result.length > 0 && result.some(r => r.id == this.state.positionsId)) {
					position = result.filter(r => r.id == this.state.positionsId)[0]
				} else if (result.length == 1) {
					position = result[0]
				}
				this.setState({
					positionsOptions: result,
					selectedPosition: position,
				})
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время получения списка должностей произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время получения списка должностей произошла ошибка: ${error}`, 'error')
			}
		})
	}

	getRoles = () => {
		const {token, isLoaded, handleSnackbarOpen} = this.props

		if (isLoaded) {
			isLoaded(false)
		}

		getRequest(`${allConstants.serverUrl}/api/Admin/GetRoles`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (Array.isArray(result)) {
				var role = null
				if (this.state.roleId && this.state.roleId > 0 && result.length > 0 && result.some(r => r.id == this.state.roleId)) {
					role = result.filter(r => r.id == this.state.roleId)[0]
				} else if (result.length > 0 && result.some(r => r.code == 'user')) {
					role = result.filter(r => r.code == 'user')[0]
				}
				this.setState({
					rolesOptions: result,
					selectedRole: role,
				})
				this.setState({rolesOptions: result})
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время получения списка ролей произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время получения списка ролей произошла ошибка: ${error}`, 'error')
			}
		})
	}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleIinKeydown = event => {
	if ([46, 8, 9, 27, 13, 110, 190].indexOf(event.keyCode) !== -1
		// Allow: Ctrl+A
		|| event.keyCode === 65 && event.ctrlKey === true
		// Allow: Ctrl+C
		|| event.keyCode === 67 && event.ctrlKey === true
		// Allow: Ctrl+V
		|| event.keyCode === 86 && event.ctrlKey === true
		// Allow: Ctrl+X
		|| event.keyCode === 88 && event.ctrlKey === true
		// Allow: home, end, left, right
		|| event.keyCode >= 35 && event.keyCode <= 39) {
		// let it happen, don't do anything
		return
	}
	const ch = String.fromCharCode(event.keyCode)
	const regEx = new RegExp(this.regexStr)
	/* eslint-disable */
	if (regEx.test(ch) || event.keyCode > 95 && event.keyCode < 106) {
		return
	} else {
		event.preventDefault()
	}
	/* eslint-enable */
}

handleUserSecretKeydown = event => {
	if ([46, 8, 9, 27, 13, 110, 190].indexOf(event.keyCode) !== -1
		// Allow: Ctrl+A
		|| event.keyCode === 65 && event.ctrlKey === true
		// Allow: Ctrl+C
		|| event.keyCode === 67 && event.ctrlKey === true
		// Allow: Ctrl+V
		|| event.keyCode === 86 && event.ctrlKey === true
		// Allow: Ctrl+X
		|| event.keyCode === 88 && event.ctrlKey === true
		// Allow: home, end, left, right
		|| event.keyCode >= 35 && event.keyCode <= 39) {
		// let it happen, don't do anything
		return
	}
	const userSecretCh = String.fromCharCode(event.keyCode)
	const userSecretKey = event.key
	const userSecretNumberRegEx = new RegExp(this.regexStr)
	/* eslint-disable */
	console.log('userSecretCh', userSecretCh)
	const replaced = userSecretKey.replace(/[^A-Za-z]/gi, "")
	if (replaced || userSecretNumberRegEx.test(userSecretCh) || event.keyCode > 95 && event.keyCode < 106) {
		return
	} else {
		event.preventDefault()
	}
	/* eslint-enable */
}

handleAutocompleteChange = (e, v) => {
	this.setState({...this.state, [e]: v})
}

// handleAutocompleteInputChange = (e, v) => {
// 	this.setState({...this.state, [e]: v})
// }

// handleAutocompleteClose = (text, obj, arr) => {
// 	if (this.state[text]) {
// 		const newVal = {
// 			id: 0,
// 			nameRu: this.state[text],
// 		}
// 		this.setState({...this.state, [arr]: [...this.state[arr], {...newVal}]}, () => {
// 			this.setState({...this.state, [text]: '', [obj]: newVal})
// 		})
// 	}
// }

handleClickShowUserSecret = isConfirmation => {
	if (isConfirmation) {
		this.setState({
			showUserSecretConfirmation: !this.state.showUserSecretConfirmation,
		})
	} else {
		this.setState({
			showUserSecret: !this.state.showUserSecret,
		})
	}
}

handleMouseDownUserSecret = isConfirmation => {
	if (isConfirmation) {
		this.setState({
			showUserSecretConfirmation: !this.state.showUserSecretConfirmation,
		})
	} else {
		this.setState({
			showUserSecret: !this.state.showUserSecret,
		})
	}
}

handleBirthDateChange = date => {
	this.setState({birthDate: date})
}

handleSaveClick = () => {
	const {
		id,
		iin,
		userName,
		userSecret,
		userSecretConfirmation,
		surnameRu,
		nameRu,
		middlenameRu,
		email,
		birthDate,
		phoneNumber,
		departmentsId,
		positionsId,
		selectedRole,
		selectedEnterprise,
		selectedDepartment,
		selectedPosition,
	} = this.state
	const {token, handleSnackbarOpen, isLoaded, handleEditUserDialogClose} = this.props

	if (iin && iin.length < 12) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('ИИН должен состоять из 12 цифр!', 'error')
		}
		return
	}
	if (!userName) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Логин"', 'error')
		}
		return
	}
	if (!email) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Email"', 'error')
		}
		return
	}
	if (!userSecret) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Пароль"', 'error')
		}
		return
	} else if (userSecret && userSecret.length < 6) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Пароль должен состоять минимум из 6 символов и должен содержать заглавные и прописные латинские буквы!', 'error')
		}
		return
	}
	if (!userSecretConfirmation) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Подтверждение пароля"', 'error')
		}
		return
	} else if (userSecretConfirmation && userSecretConfirmation.length < 6) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Подтверждение пароля должен состоять минимум из 6 символов и должен содержать заглавные и прописные латинские буквы!', 'error')
		}
		return
	}
	if (userSecret && userSecretConfirmation && userSecret != userSecretConfirmation) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Пароли не совпадают!', 'error')
		}
		return
	}
	if (!selectedRole) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не выбрали роль', 'error')
		}
		return
	}
	if (!surnameRu) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Фамилия"', 'error')
		}
		return
	}
	if (!nameRu) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не заполнили поле "Имя"', 'error')
		}
		return
	}
	if (selectedRole && selectedRole != 1 && !selectedEnterprise) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не выбрали компанию/филиал', 'error')
		}
		return
	}
	if (selectedRole && selectedRole != 1 && selectedRole != 2 && !selectedDepartment) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не выбрали структурное подразделение', 'error')
		}
		return
	}
	if (selectedRole && selectedRole != 1 && selectedRole != 2 && !selectedPosition) {
		if (handleSnackbarOpen) {
			handleSnackbarOpen('Вы не выбрали должность', 'error')
		}
		return
	}

	if (isLoaded) {
		isLoaded(false)
	}

	const userData = {
		id,
		iin: iin ? iin : null,
		userName,
		userSecret: userSecret ? userSecret : null,
		userSecretConfirmation: userSecretConfirmation ? userSecretConfirmation : null,
		surnameRu,
		nameRu,
		middlenameRu: middlenameRu ? middlenameRu : null,
		email,
		birthDate: birthDate ? birthDate : null,
		phoneNumber: phoneNumber ? phoneNumber : null,
		enterprise: {
			id: selectedEnterprise.id,
			nameRu: selectedEnterprise.nameRu,
			nameEn: selectedEnterprise.nameEn,
			nameKz: selectedEnterprise.nameKz,
		},
		department: {
			id: selectedDepartment.id,
			nameRu: selectedDepartment.nameRu,
			nameEn: selectedDepartment.nameEn,
			nameKz: selectedDepartment.nameKz,
		},
		position: {
			id: selectedPosition.id,
			nameRu: selectedPosition.nameRu,
			nameEn: selectedPosition.nameEn,
			nameKz: selectedPosition.nameKz,
		},
		roleId: selectedRole ? selectedRole.id : 0,
	}

	postRequest(`${allConstants.serverUrl}/api/Admin/SaveUser`, token, userData, result => {
		if (isLoaded) {
			isLoaded(true)
		}
		if (result && result.isSuccess) {
			if ((result.msg == 'role_err' || result.msg == 'role_not_found' || result.msg == 'without_role') && handleSnackbarOpen) {
				handleSnackbarOpen('Пользователь успешно добавлен!', 'success')
			} else if (result.msg && handleSnackbarOpen) {
				handleSnackbarOpen(`Пользователь с ролью ${result.msg} успешно добавлен!`, 'success')
			}

			if (handleEditUserDialogClose) {
				handleEditUserDialogClose(true)
			}
		} else if (result && !result.isSuccess) {
			if (result.msg == 'empty_department' && handleSnackbarOpen) {
				handleSnackbarOpen('Отправлено пустое структурное подразделение', 'error')
			} else if (result.msg == 'empty_position' && handleSnackbarOpen) {
				handleSnackbarOpen('Отправлено пустая должность', 'error')
			} else if (result.msg == 'empty_secret' && handleSnackbarOpen) {
				handleSnackbarOpen('Отправлено пустой пароль', 'error')
			} else if (result.msg == 'empty_data' && handleSnackbarOpen) {
				handleSnackbarOpen('Отправленные данные пустые', 'error')
			} else if (result.msg && handleSnackbarOpen) {
				handleSnackbarOpen(`Во время сохранения пользователя произошла ошибка: ${result.msg}`, 'error')
			}
		} else if (handleSnackbarOpen) {
			handleSnackbarOpen('Во время сохранения пользователя произошла ошибка', 'error')
		}
	},
	error => {
		if (isLoaded) {
			isLoaded(true)
		}
		if (handleSnackbarOpen) {
			handleSnackbarOpen(`Во время сохранения пользователя произошла ошибка: ${error}`, 'error')
		}
	})
}

handleCancelClick = () => {
	this.setState({
		id: 0,
		userName: '',
		userSecret: '',
		userSecretConfirmation: '',
		showUserSecret: false,
		showUserSecretConfirmation: false,
		rolesOptions: [],
		selectedRole: null,
		email: '',
		iin: '',
		birthDate: '',
		phoneNumber: '',
		nameRu: '',
		surnameRu: '',
		middlenameRu: '',
		departmentsId: 0,
		positionsId: 0,
		roleId: 0,
		enterprisesOptions: [],
		selectedEnterprise: null,
		departmentsOptions: [],
		selectedDepartment: null,
		positionsOptions: [],
		selectedPosition: null,
		newDepartmentStr: '',
		newPositionStr: '',
	})
	if (this.props.handleEditUserDialogClose) {
		this.props.handleEditUserDialogClose()
	}
}

render() {
	const {classes} = this.props
	return (
		<React.Fragment>
			<DialogTitle className={classes.headerStyle} id='add-user-dialog-title'>Данные пользователя</DialogTitle>
			<DialogContent dividers={true}>
				<div className={classes.modalRoot}>
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									name='userName'
									error={(!this.state.userName)}
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.userName}
									label='Логин'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									error={(!this.state.email)}
									name='email'
									type='email'
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.email}
									label='Email'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='userSecret'
									type={this.state.showUserSecret ? 'text' : 'password'}
									fullWidth
									error={Boolean(!this.state.userSecret || this.state.userSecret && this.state.userSecret.length < 6)}
									size='small'
									autoComplete='off'
									value={this.state.userSecret}
									label='Пароль'
									variant='outlined'
									className={classes.input}
									InputProps={{
										'aria-label': 'Description',
										onKeyDown: this.handleUserSecretKeydown,
										endAdornment: <InputAdornment position='end'>
											<IconButton
												aria-label='Показать пароль'
												onClick={() => this.handleClickShowUserSecret(false)}
												onMouseDown={() => this.handleMouseDownUserSecret(false)}
											>{this.state.showUserSecret ? <Visibility /> : <VisibilityOff />}</IconButton>
										</InputAdornment>,
									}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='userSecretConfirmation'
									type={this.state.showUserSecretConfirmation ? 'text' : 'password'}
									fullWidth
									error={Boolean(!this.state.userSecretConfirmation || this.state.userSecretConfirmation && this.state.userSecretConfirmation.length < 6 || this.state.userSecret && this.state.userSecretConfirmation && this.state.userSecret != this.state.userSecretConfirmation)}
									size='small'
									autoComplete='off'
									value={this.state.userSecretConfirmation}
									label='Подтверждение пароля'
									variant='outlined'
									className={classes.input}
									InputProps={{
										'aria-label': 'Description',
										onKeyDown: this.handleUserSecretKeydown,
										endAdornment: <InputAdornment position='end'>
											<IconButton
												aria-label='Показать потверждение пароля'
												onClick={() => this.handleClickShowUserSecret(true)}
												onMouseDown={() => this.handleMouseDownUserSecret(true)}
											>{this.state.showUserSecretConfirmation ? <Visibility /> : <VisibilityOff />}</IconButton>
										</InputAdornment>,
									}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									name='selectedRole'
									size='small'
									value={this.state.selectedRole}
									options={this.state.rolesOptions}
									fullWidth
									onChange={(e, v) => { this.handleAutocompleteChange('selectedRole', v) }}
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Роль' variant='outlined' />}
								/>
							</Paper>
						</Grid>
					</Grid>
					<Divider className={classes.divider} />
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									name='surnameRu'
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.surnameRu}
									label='Фамилия'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									name='nameRu'
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.nameRu}
									label='Имя'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='middlenameRu'
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.middlenameRu}
									label='Отчество'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
					<Divider className={classes.divider} />
					<br/>
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									required
									name='selectedEnterprise'
									size='small'
									disabled={Array.isArray(this.state.enterprisesOptions) && this.state.enterprisesOptions.length == 1}
									value={this.state.selectedEnterprise}
									options={this.state.enterprisesOptions}
									groupBy={option => option.parentId}
									getOptionLabel={option => option.parentNameRu}
									fullWidth
									onChange={(e, v) => { this.handleAutocompleteChange('selectedEnterprise', v) }}
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Компания/филиал' variant='outlined' />}
								/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									required
									name='selectedDepartment'
									size='small'
									value={this.state.selectedDepartment}
									options={this.state.departmentsOptions}
									fullWidth
									onChange={(e, v) => { this.handleAutocompleteChange('selectedDepartment', v) }}
									// onInputChange={(e, v) => { this.handleAutocompleteInputChange('newDepartmentStr', v) }}
									// onClose={(e, v) => { this.handleAutocompleteClose('newDepartmentStr', 'selectedDepartment', 'departmentsOptions') }}
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Подразделение' variant='outlined' />}
								/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									required
									name='selectedPosition'
									size='small'
									value={this.state.selectedPosition}
									options={this.state.positionsOptions}
									fullWidth
									onChange={(e, v) => { this.handleAutocompleteChange('selectedPosition', v) }}
									// onInputChange={(e, v) => { this.handleAutocompleteInputChange('newPositionStr', v) }}
									// onClose={(e, v) => { this.handleAutocompleteClose('newPositionStr', 'selectedPosition', 'positionsOptions') }}
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Должность' variant='outlined' />}
								/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='iin'
									error={Boolean(this.state.iin && this.state.iin.length < 12)}
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.iin}
									label='ИИН'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description', maxLength: 12, onKeyDown: this.handleIinKeydown}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
									<KeyboardDatePicker
										margin='normal'
										inputVariant='outlined'
										variant='dialog'
										cancelLabel='Отменить'
										okLabel='Выбрать'
										fullWidth
										size='small'
										className={classes.input}
										label='День рождения'
										format='dd.MM.yyyy'
										value={this.state.birthDate}
										onChange={this.handleBirthDateChange}
										invalidDateMessage='Неверный формат даты'
										KeyboardButtonProps={{'aria-label': 'change date'}}/>
								</MuiPickersUtilsProvider>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='phoneNumber'
									fullWidth
									size='small'
									autoComplete='off'
									value={this.state.phoneNumber}
									label='Номер телефона'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
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

export default withStyles(styles, {withTheme: true})(AddUser)
