import React, {Component} from 'react'
import {withStyles} from '@material-ui/core/styles'
import Typography from '@material-ui/core/Typography'
import Button from '@material-ui/core/Button'
import Autocomplete from '@material-ui/lab/Autocomplete'
import Tooltip from '@material-ui/core/Tooltip'
import IconButton from '@material-ui/core/IconButton'
import TextField from '@material-ui/core/TextField'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import CardMedia from '@material-ui/core/CardMedia'
import CloudUploadIcon from '@material-ui/icons/CloudUpload'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import EditIcon from '@material-ui/icons/Edit'
import red from '@material-ui/core/colors/red'
import {connect} from 'react-redux'
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
		gridTemplateColumns: 'repeat(12, 1fr)',
		margin: 0,
		padding: 20,
	},
	input: {
		// margin: theme.spacing.unit,
		margin: 0,
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
		fontWeight: 'normal',
	},
	backdrop: {
		zIndex: theme.zIndex.drawer + 3,
		color: '#fff',
	},
	typography: {
		padding: '10px',
		wordWrap: 'break-word',
		whiteSpace: 'break-spaces',
		display: 'inline-block',
	},
	gridPaddingRight: {
		paddingRight: '10px',
	},
	media: {
		margin: 'auto',
		width: 'inherit',
		height: 'inherit',
		maxWidth: '100%',
		maxHeight: '50vh',
		borderRadius: theme.spacing(10),
	},
	iconButton: {
		float: 'right',
		zIndex: 1200,
		bottom: theme.spacing(2),
		right: theme.spacing(2),
		marginTop: -theme.spacing(3),
		backgroundColor: 'white',
		'&:hover': {
			backgroundColor: red[200],
		},
	},
	photoGrid: {
		flexWrap: 'wrap',
		gridTemplateColumns: 'repeat(12, 1fr)',
		margin: 0,
		padding: 20,
		marginRight: theme.spacing(4),
	},
})

class Profile extends Component {

	constructor(props) {
		super(props)
		this.state = {
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			isEditProfile: false,
			oldData: null,
			isPhotoChanged: false,
			id: 0,
			crmEmployeesId: null,
			crmPatientsId: null,
			userName: '',
			nameRu: '',
			nameKz: '',
			nameEn: '',
			surnameRu: '',
			surnameKz: '',
			surnameEn: '',
			middlenameRu: '',
			middlenameKz: '',
			middlenameEn: '',
			department: null,
			position: null,
			birthDate: '',
			aboutMe: '',
			address: '',
			phoneNumber: '',
			photoB64: '',
		}
	}

	componentDidMount() {
		this.getUserProfile()
		this.getCurrentUserPhoto()
	}

	getUserProfile = () => {
		const {token} = this.props

		getRequest(`${allConstants.serverUrl}/api/Users/GetProfile`, token, result => {
			if (result && result.isSuccess && result.data) {
				this.setState({...result.data})
			} else {
				this.handleSnackbarOpen(`Во время получения данных профиля произошла ошибка${result && result.msg ? `: ${result.msg}` : ''}`, 'error')
			}
		},
		error => {
			console.log(error)
			this.handleSnackbarOpen(`Во время получения данных профиля произошла ошибка: ${error}`, 'error')
		})
	}

	getCurrentUserPhoto = () => {
		const {token} = this.props

		getRequest(`${allConstants.serverUrl}/api/Users/GetCurrentUserPhoto`, token, result => {
			if (result && result.isSuccess) {
				this.setState({
					photoB64: result.data,
				})
			} else {
				this.handleSnackbarOpen(`Во время получения фото профиля произошла ошибка${result && result.msg ? `: ${result.msg}` : ''}`, 'error')
			}
		},
		error => {
			console.log(error)
			this.handleSnackbarOpen(`Во время получения фото профиля произошла ошибка: ${error}`, 'error')
		})
	}

handleSnackbarOpen = (msg, severity) => {
	this.setState({
		openSnackbar: true,
		snackbarMsg: msg || '',
		snackbarSeverity: severity || 'success',
	})
}

handleSnackbarClose = () => {
	this.setState({
		openSnackbar: false,
	})
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
	})
}

handleEditClick = () => {
	const {
		surnameRu,
		nameRu,
		middlenameRu,
		address,
		phoneNumber,
		aboutMe,
	} = this.state
	const oldData = {
		surnameRu,
		nameRu,
		middlenameRu,
		address,
		phoneNumber,
		aboutMe,
	}
	this.setState({
		isEditProfile: true,
		oldData: {...oldData},
	})
}

handleCancelClick = () => {
	const {oldData} = this.state
	this.setState({...this.state, ...oldData, isEditProfile: false, oldData: null})
}

handleSaveClick = () => {
	const {
		id,
		isPhotoChanged,
		crmEmployeesId,
		crmPatientsId,
		surnameRu,
		nameRu,
		middlenameRu,
		address,
		phoneNumber,
		aboutMe,
	} = this.state
	const {token} = this.props

	if (!surnameRu) {
		this.handleSnackbarOpen('Вы не заполнили поле "Фамилия"', 'error')
		return
	}
	if (!nameRu) {
		this.handleSnackbarOpen('Вы не заполнили поле "Имя"', 'error')
		return
	}

	this.isLoaded(false)

	const userData = {
		id,
		crmEmployeesId,
		crmPatientsId,
		surnameRu,
		nameRu,
		middlenameRu,
		address,
		phoneNumber,
		aboutMe,
	}

	postRequest(`${allConstants.serverUrl}/api/Users/SaveProfile`, token, userData, result => {
		this.isLoaded(true)

		if (result && result.isSuccess) {
			this.handleSnackbarOpen('Профиль успешно сохранен!', 'success')
		} else if (result && !result.isSuccess && result.msg == 'empty_current_user_or_profile_data') {
			this.handleSnackbarOpen('Во время сохранения профиля произошла ошибка. Данные текущего пользователя или отправленные данные пустые', 'error')
		} else if (result && !result.isSuccess && result.msg == 'empty_employee') {
			this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка. Профиль пользователя с идентификатором ${userData.crmEmployeesId} не найден`, 'error')
		} else if (result && !result.isSuccess && result.msg == 'empty_patient') {
			this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка. Профиль пациента с идентификатором ${userData.crmPatientsId} не найден`, 'error')
		} else if (result && !result.isSuccess && result.msg == 'empty_employee_and_patient') {
			this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка. Профиль пользователя и пациента с идентификатором ${userData.id} не найден`, 'error')
		} else if (result && !result.isSuccess && (result.msg == 'empty_id' || result.msg == 'empty_user')) {
			this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка. Профиль пользователя с идентификатором ${userData.id} не найден`, 'error')
		} else {
			this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка${result && result.msg ? `: ${result.msg}` : ''}`, 'error')
		}

		if (isPhotoChanged) {
			this.handleSavePhoto()
		} else {
			this.setState({
				isEditProfile: false,
				oldData: null,
			})
		}
	},
	error => {
		this.isLoaded(true)
		this.handleSnackbarOpen(`Во время сохранения профиля произошла ошибка: ${error}`, 'error')
	})
}

handleSavePhoto = () => {
	const {token} = this.props
	const {
		id,
		crmEmployeesId,
		crmPatientsId,
		photoB64,
	} = this.state

	this.isLoaded(false)

	const photoData = {
		id,
		crmEmployeesId,
		crmPatientsId,
		photoB64,
	}
	postRequest(`${allConstants.serverUrl}/api/Users/SaveProfilePhoto`, token, photoData, photoResult => {
		this.isLoaded(true)
		if (photoResult && photoResult.isSuccess) {
			this.handleSnackbarOpen('Фото профиля успешно сохранено!', 'success')
		} else if (photoResult && !photoResult.isSuccess && photoResult.msg == 'empty_current_user_or_profile_data') {
			this.handleSnackbarOpen('Во время сохранения фото профиля произошла ошибка. Данные текущего пользователя или отправленные данные пустые', 'error')
		} else if (photoResult && !photoResult.isSuccess && photoResult.msg == 'empty_photo') {
			this.handleSnackbarOpen('Во время сохранения фото профиля произошла ошибка. Данные фото пустые', 'error')
		} else if (photoResult && !photoResult.isSuccess && photoResult.msg == 'empty_employee_and_patient') {
			this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка. Профиль пользователя и пациента с идентификатором ${photoData.id} не найден`, 'error')
		} else if (photoResult && !photoResult.isSuccess && (photoResult.msg == 'empty_id' || photoResult.msg == 'empty_user')) {
			this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка. Профиль пользователя с идентификатором ${photoData.id} не найден`, 'error')
		} else if (photoResult && !photoResult.isSuccess && photoResult.msg == 'empty_employee') {
			this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка. Профиль пользователя с идентификатором ${photoData.crmEmployeesId} не найден`, 'error')
		} else if (photoResult && !photoResult.isSuccess && photoResult.msg == 'empty_patient') {
			this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка. Профиль пациента с идентификатором ${photoData.crmPatientsId} не найден`, 'error')
		} else {
			this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка${photoResult && photoResult.msg ? `: ${photoResult.msg}` : ''}`, 'error')
		}

		this.setState({
			isEditProfile: false,
			oldData: null,
		})
	},
	error => {
		this.isLoaded(true)
		this.handleSnackbarOpen(`Во время сохранения фото профиля произошла ошибка: ${error}`, 'error')

		this.setState({
			isEditProfile: false,
			oldData: null,
		})
	})
}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleBirthDateChange = date => {
	this.setState({birthDate: date})
}

handleImageChange = event => {
	if (event.target.files && event.target.files[0]) {
		let img = event.target.files[0]
		this.getBase64(img, b64 => {
			if (b64) {
				if (b64.indexOf('base64,') > -1) {
					this.setState({
						isPhotoChanged: true,
						photoB64: b64.split('base64,')[1],
					})
				} else {
					this.setState({
						isPhotoChanged: true,
						photoB64: b64,
					})
				}
			}
		})
	}
}

getBase64 = (file, callback) => {
	let reader = new FileReader()
	reader.readAsDataURL(file)
	reader.onload = () => {
		callback(reader.result)
	}
	reader.onerror = error => {
		console.log('Error: ', error)
		this.handleSnackbarOpen(`Во время загрузки файла произошла ошибка: ${error}`, 'error')
	}
}

render() {
	const {classes} = this.props
	const {
		openSnackbar,
		snackbarMsg,
		snackbarSeverity,
		loading,
		isEditProfile,
		photoB64,
		surnameRu,
		nameRu,
		middlenameRu,
		address,
		phoneNumber,
		aboutMe,
		department,
		position,
	} = this.state

	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>Профиль</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid container item xs={4} className={classes.photoGrid}>
					<Grid item xs={12}>
						<Paper className={classes.paper}>
							{
								photoB64
									? <CardMedia component='img' className={classes.media} src={`data:image/jpeg;base64,${photoB64}`} title='Фото пользователя' />
									: <CardMedia component='img' className={classes.media} src={require('../../Static/important-person.jpg')} title='Фото пользователя' />
							}
							{
								isEditProfile
									? (
										<React.Fragment>
											<input
												accept='image/x-png,image/jpeg'
												className={classes.input}
												style={{display: 'none'}}
												id='profile-photo-file'
												ref={ref => this._fileInput = ref}
												type='file'
												onChange={this.handleImageChange}
											/>
											<label htmlFor='profile-photo-file'>
												<IconButton color='secondary' aria-label='upload-photo' onClick={() => { if (this._fileInput) this._fileInput.click() }} className={classes.iconButton}>
													<CloudUploadIcon />
												</IconButton>
											</label>
										</React.Fragment>
									)
									: null
							}
						</Paper>
					</Grid>
					{
						isEditProfile
							? (
								<React.Fragment>
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<Button
												startIcon={<SaveIcon />}
												variant='outlined'
												color='secondary'
												size='medium'
												fullWidth
												className={classes.button}
												onClick={this.handleSaveClick}>
												<Tooltip title='Сохранить изменения'>
													<Typography noWrap>Сохранить изменения</Typography>
												</Tooltip>
											</Button>
										</Paper>
									</Grid>
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<Button
												startIcon={<CancelIcon />}
												variant='outlined'
												color='primary'
												size='medium'
												fullWidth
												className={classes.button}
												onClick={this.handleCancelClick}>
												<Tooltip title='Отменить'>
													<Typography noWrap>Отменить</Typography>
												</Tooltip>
											</Button>
										</Paper>
									</Grid>
								</React.Fragment>
							)
							: (
								<Grid item xs={12}>
									<Paper className={classes.paper}>
										<Button
											startIcon={<EditIcon />}
											variant='outlined'
											color='secondary'
											size='medium'
											fullWidth
											className={classes.button}
											onClick={this.handleEditClick}>
											<Tooltip title='Редактировать профиль'>
												<Typography noWrap>Редактировать профиль</Typography>
											</Tooltip>
										</Button>
									</Paper>
								</Grid>
							)
					}
				</Grid>
				<Grid item xs={7}>
					<Grid container className={classes.container} item>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									error={(isEditProfile && !surnameRu)}
									name='surnameRu'
									fullWidth
									disabled={!isEditProfile}
									size='small'
									autoComplete='off'
									value={surnameRu}
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
									error={(isEditProfile && !nameRu)}
									name='nameRu'
									fullWidth
									disabled={!isEditProfile}
									size='small'
									autoComplete='off'
									value={nameRu}
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
									disabled={!isEditProfile}
									size='small'
									autoComplete='off'
									value={middlenameRu}
									label='Отчество'
									variant='outlined'
									className={classes.middlenameRu}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
					<Divider className={classes.divider} />
					<Grid container className={classes.container} item xs={12}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									name='department'
									size='small'
									value={department}
									options={department ? [{...department}] : []}
									fullWidth
									disabled
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Структурное подразделение' variant='outlined' />}
								/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<Autocomplete
									name='position'
									size='small'
									value={position}
									options={position ? [{...position}] : []}
									fullWidth
									disabled
									getOptionLabel={option => option.nameRu}
									renderInput={params => <TextField {...params} label='Должность' variant='outlined' />}
								/>
							</Paper>
						</Grid>
					</Grid>
					<Divider className={classes.divider} />
					<Grid container className={classes.container} item xs={12}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='address'
									fullWidth
									disabled={!isEditProfile}
									size='small'
									autoComplete='off'
									value={address}
									label='Адрес'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='phoneNumber'
									fullWidth
									disabled={!isEditProfile}
									size='small'
									autoComplete='off'
									value={phoneNumber}
									label='Номер телефона'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='aboutMe'
									fullWidth
									multiline
									disabled={!isEditProfile}
									rows={3}
									size='small'
									autoComplete='off'
									value={aboutMe}
									label='Био'
									variant='outlined'
									className={classes.aboutMe}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
				</Grid>
			</Grid>
			<Snackbar open={openSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
				<Alert onClose={this.handleSnackbarClose} severity={snackbarSeverity}>
					{snackbarMsg}
				</Alert>
			</Snackbar>
			<Backdrop className={classes.backdrop} open={loading}>
				<CircularProgress color='inherit' />
			</Backdrop>
		</div>
	)
}
}

function mapStateToProps(state) {
	const {currentUser, token} = state
	return {
		currentUser,
		token,
	}
}

export default connect(mapStateToProps)(withStyles(styles, {withTheme: true})(Profile))
