import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Autocomplete from '@material-ui/lab/Autocomplete'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import PersonAddIcon from '@material-ui/icons/PersonAdd'
import DateFnsUtils from '@date-io/date-fns'
import ruLocale from 'date-fns/locale/ru'
import {MuiPickersUtilsProvider, KeyboardDatePicker, KeyboardTimePicker} from '@material-ui/pickers'
import AccessTimeIcon from '@material-ui/icons/AccessTime'
import Typography from '@material-ui/core/Typography'
import _ from 'lodash'
import AddVisitor from '../AddVisitor'
import CrmTable from '../CrmTable'
import {allConstants} from '../../Constants/AllConstants.js'
import {addCardColumns} from '../../Constants/TableColumns.js'
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
		// gridGap: theme.spacing(1),
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
		fontWeight: 'normal',
	},
	backdrop: {
		zIndex: theme.zIndex.drawer + 1,
		color: '#fff',
	},
})

class AddCard extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			comment:'',
			justification:'',
			beginDate: new Date(),
			beginTime: new Date(),
			endDate: new Date(),
			endTime: new Date(),
			inviter: null,
			cardTypeOptions: [
				{id: 1, nameRu: 'Разовый'},
				// { id: 2, nameRu: 'Временный'},
				// { id: 3, nameRu: 'На иностранцев'}
			],
			cardType: {id: 1, nameRu: 'Разовый'},
			usersOptions: [],
			openVisitorDialog: false,
			tableRows: [],
			editVisitorData: null,
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			cardId: 0,
		}
	}

	componentDidMount() {
		this.getUsersByDepartment()
	}

	componentDidUpdate(prevProps) {
		if (!_.isEqual(prevProps.currentUser, this.props.currentUser)) {
			this.getUsersByDepartment()
		}
	}

	getUsersByDepartment = () => {
		const {currentUser, token} = this.props

		if (currentUser) {
			if (currentUser && currentUser.departmentsId) {
				getRequest(`${allConstants.serverUrl}/api/Users/GetUsersByDepartmentId?departmentId=${currentUser.departmentsId}`, token, result => {
					this.isLoaded(true)
					if (Array.isArray(result)) {
						this.setState({usersOptions: result})
					} else {
						this.handleSnackbarOpen('Во время получения списка пользователей произошла ошибка', 'error')
					}
				},
				error => {
					this.isLoaded(true)
					this.handleSnackbarOpen(`Во время получения списка пользователей произошла ошибка: ${error}`, 'error')
				})
			}
		}
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

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleAutocompleteChange = (e, v) => {
	this.setState({...this.state, [e]: v})
}

handleBeginDateChange = date => {
	if (!date) {
		this.setState({beginDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/'))})
	} else {
		const newDate = new Date(date.getFullYear() + '/' + (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '/' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()))
		if (this.state.endDate && newDate > this.state.endDate) {
			this.setState({
				beginDate: newDate,
				endDate: newDate,
			})
		} else {
			this.setState({beginDate: newDate})
		}
	}
}

handleEndDateChange = date => {
	if (!date) {
		if (this.state.beginDate) {
			this.setState({endDate: this.state.beginDate})
		} else {
			this.setState({endDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/'))})
		}
	} else {
		let newDate = new Date(date.getFullYear() + '/' + (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '/' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()))
		if (this.state.beginDate && newDate < this.state.beginDate) {
			newDate = this.state.beginDate
		}
		this.setState({endDate: newDate})
	}
}

handleBeginTimeChange = time => {
	if (!time) {
		this.setState({beginTime: new Date()})
	} else {
		this.setState({beginTime: time})
		if (time && this.state.endTime && this.state.endTime < time) {
			this.setState({endTime: time})
		}
	}
}

handleEndTimeChange = time => {
	if (!time) {
		this.setState({endTime: new Date()})
	} else if (time && this.state.beginTime && this.state.beginTime > time) {
		this.setState({endTime: this.state.beginTime})
	} else {
		this.setState({endTime: time})
	}
}

handleSelectChange = event => {
	const name = event.target.name
	this.setState({
		...this.state,
		[name]: event.target.value,
	})
}

handleVisitorDialogOpen = () => {
	this.setState({
		openVisitorDialog: true,
	})
}

handleVisitorDialogClose = visitor => {
	if (visitor) {
		let rows = [...this.state.tableRows]
		let lastId = 0
		if (rows.sort((a, b) => b.id - a.id).length > 0) {
			lastId = rows.sort((a, b) => b.id - a.id)[0].id
		}
		if (rows.some(row => row.id == visitor.id)) {
			let ix = -1
			rows.forEach((v, i) => {
				if (v.id == visitor.id) {
					ix = i
				}
			})
			if (ix > -1) {
				rows.splice(ix, 1)
				rows = [...rows, {...visitor, id: lastId + 1}]
			}
		} else {
			rows = [...rows, {...visitor, id: lastId + 1}]
		}
		this.setState({
			openVisitorDialog: false,
			tableRows: rows,
			editVisitorData: null,
		})
	} else {
		this.setState({
			openVisitorDialog: false,
			editVisitorData: null,
		})
	}
}

handleEditClick = id => {
	if (this.state.tableRows.length > 0) {
		const editData = this.state.tableRows.filter(row => row.id == id)
		if (Array.isArray(editData) && editData.length > 0) {
			this.setState({
				openVisitorDialog: true,
				editVisitorData: editData[0],
			})
		}
	}
}

handleLaunchClick = id => {
	console.log('handleLaunchClick', id)
}

handleDeleteClick = id => {
	let rows = [...this.state.tableRows]
	if (rows.some(row => row.id == id)) {
		let ix = -1
		rows.forEach((v, i) => {
			if (v.id == id) {
				ix = i
			}
		})
		if (ix > -1) {
			rows.splice(ix, 1)
		}
	}
	this.setState({
		openVisitorDialog: false,
		tableRows: rows,
		editVisitorData: null,
	})
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
	})
}

handleSaveClick = () => {
	const {cardType, inviter, beginDate, endDate, beginTime, endTime, justification, tableRows, cardId} = this.state
	const {token} = this.props
	if (!Array.isArray(tableRows) || Array.isArray(tableRows) && tableRows.length == 0) {
		this.handleSnackbarOpen('Вы не добавили посетителей!', 'error')
		return
	}
	if (!justification) {
		this.handleSnackbarOpen('Вы не заполнили обоснование!', 'error')
		return
	}
	if (!inviter) {
		this.handleSnackbarOpen('Вы выбрали приглашающего!', 'error')
		return
	}
	if (beginDate == 'Invalid date format' || beginDate == 'Неверный формат даты' || endDate == 'Invalid date format' || endDate == 'Неверный формат даты') {
		this.handleSnackbarOpen('Неверный формат даты', 'error')
		return
	}
	if (beginDate > endDate) {
		this.handleSnackbarOpen(`Дата по не может быть раньше ${((beginDate.getDate() < 10 ? '0' + beginDate.getDate() : beginDate.getDate()) + '.' + (beginDate.getMonth() + 1 < 10 ? '0' + (beginDate.getMonth() + 1) : beginDate.getMonth() + 1) + '.' + beginDate.getFullYear())}`, 'error')
		return
	}
	if (beginTime == 'Invalid date format' || beginTime == 'Неверный формат времени' || endTime == 'Invalid date format' || endTime == 'Неверный формат времени') {
		this.handleSnackbarOpen('Неверный формат времени', 'error')
		return
	}
	if (beginTime > endTime) {
		this.handleSnackbarOpen(`Время по не может быть раньше ${beginTime.getHours() < 10 ? `0${beginTime.getHours()}` : beginTime.getHours()}:${beginTime.getMinutes() < 10 ? `0${beginTime.getMinutes()}` : beginTime.getMinutes()}`, 'error')
		return
	}
	
	const newCard = {
		id: cardId,
		cardsTypeId: cardType.id,
		inviterId: inviter.id,
		beginDate,
		endDate,
		beginTime: `${beginTime.getHours() < 10 ? `0${beginTime.getHours()}` : beginTime.getHours()}:${beginTime.getMinutes() < 10 ? `0${beginTime.getMinutes()}` : beginTime.getMinutes()}`,
		endTime: `${endTime.getHours() < 10 ? `0${endTime.getHours()}` : endTime.getHours()}:${endTime.getMinutes() < 10 ? `0${endTime.getMinutes()}` : endTime.getMinutes()}`,
		justification,
		visitors: tableRows.map(r => ({
			iin: r.iin || null,
			documentNumber: r.documentNumber || null,
			additionalDocumentNumber: r.additionalDocumentNumber || null,
			citizenship: r.citizenship || null,
			nameRu: r.nameRu || null,
			surnameRu: r.surnameRu || null,
			middlenameRu: r.middlenameRu || null,
			jobPlaceName: r.jobPlaceName || null,
			positionName: r.positionName || null,
		})),
	}
	this.isLoaded(false)
	postRequest(`${allConstants.serverUrl}/api/Cards/SaveCard`, token, newCard, result => {
		this.isLoaded(true)
		if (result && result.isSuccess) {
			window.location.href = '/cards-list'
		} else {
			this.handleSnackbarOpen(`Во время сохранения пропуска произошла ошибка: ${result.msg}`, 'error')
		}
	},
	error => {
		this.isLoaded(true)
		this.handleSnackbarOpen(`Во время сохранения пропуска произошла ошибка: ${error}`, 'error')
	})
}

render() {
	const {classes, isDesktop, token} = this.props
	const {
		cardType,
		cardTypeOptions,
		inviter,
		usersOptions,
		justification,
		beginDate,
		beginTime,
		endDate,
		endTime,
	} = this.state

	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>Создание нового пропуска</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={isDesktop ? 6 : 12}>
					<Paper className={classes.paper}>
						<Autocomplete
							id='cardType'
							name='cardType'
							disableClearable
							value={cardType}
							options={cardTypeOptions}
							fullWidth
							onChange={(e, v) => { this.handleAutocompleteChange('cardType', v) }}
							getOptionLabel={option => option.nameRu}
							renderInput={params => <TextField {...params} label='Тип пропуска' variant='outlined' />}
						/>
					</Paper>
					<Paper className={classes.paper}>
						<Autocomplete
							id='inviter'
							name='inviter'
							disableClearable
							value={inviter}
							options={usersOptions}
							fullWidth
							onChange={(e, v) => { this.handleAutocompleteChange('inviter', v) }}
							getOptionLabel={option => option.nameRu}
							renderInput={params => <TextField {...params} label='Приглашающий' variant='outlined' />}
						/>
					</Paper>
				</Grid>
				<Grid item xs={isDesktop ? 6 : 12}>
					<Paper className={classes.paper}>
						<TextField
							name='justification'
							multiline
							rows={5}
							fullWidth
							size='small'
							value={justification}
							label='Обоснование'
							variant='outlined'
							className={classes.input}
							inputProps={{'aria-label': 'Description'}}
							onChange={this.handleChange}/>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' noWrap>
							Разрешенное время для входа:
						</Typography>
					</Paper>
				</Grid>
				<MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardDatePicker
								margin='normal'
								inputVariant='outlined'
								variant='dialog'
								cancelLabel='Отменить'
								okLabel='Выбрать'
								fullWidth
								label='Дата с'
								format='dd.MM.yyyy'
								value={beginDate}
								minDate={new Date()}
								onChange={this.handleBeginDateChange}
								invalidDateMessage='Неверный формат даты'
								minDateMessage={`Дата не может быть раньше ${((new Date().getDate() < 10 ? '0' + new Date().getDate() : new Date().getDate()) + '.' + (new Date().getMonth() + 1 < 10 ? '0' + (new Date().getMonth() + 1) : new Date().getMonth() + 1) + '.' + new Date().getFullYear())}`}
								KeyboardButtonProps={{'aria-label': 'change date'}}/>
						</Paper>
					</Grid>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardDatePicker
								margin='normal'
								inputVariant='outlined'
								variant='dialog'
								cancelLabel='Отменить'
								okLabel='Выбрать'
								fullWidth
								label='по'
								format='dd.MM.yyyy'
								value={endDate}
								minDate={beginDate}
								onChange={this.handleEndDateChange}
								invalidDateMessage='Неверный формат даты'
								minDateMessage={`Дата не может быть раньше ${((beginDate.getDate() < 10 ? '0' + beginDate.getDate() : beginDate.getDate()) + '.' + (beginDate.getMonth() + 1 < 10 ? '0' + (beginDate.getMonth() + 1) : beginDate.getMonth() + 1) + '.' + beginDate.getFullYear())}`}
								KeyboardButtonProps={{'aria-label': 'change date'}}/>
						</Paper>
					</Grid>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardTimePicker
								margin='normal'
								inputVariant='outlined'
								variant='dialog'
								cancelLabel='Отменить'
								okLabel='Выбрать'
								fullWidth
								ampm={false}
								label='Время с'
								value={beginTime}
								onChange={this.handleBeginTimeChange}
								keyboardIcon={<AccessTimeIcon />}
								views={['hours', 'minutes']}
								invalidDateMessage='Неверный формат времени'
								KeyboardButtonProps={{'aria-label': 'change date'}}/>
						</Paper>
					</Grid>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardTimePicker
								margin='normal'
								inputVariant='outlined'
								variant='dialog'
								cancelLabel='Отменить'
								okLabel='Выбрать'
								fullWidth
								ampm={false}
								label='по'
								value={endTime}
								minDate={beginTime}
								onChange={this.handleEndTimeChange}
								keyboardIcon={<AccessTimeIcon />}
								views={['hours', 'minutes']}
								invalidDateMessage='Неверный формат времени'
								KeyboardButtonProps={{'aria-label': 'change date'}}/>
						</Paper>
					</Grid>
				</MuiPickersUtilsProvider>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<CrmTable
							rows={this.state.tableRows}
							columns={addCardColumns}
							isLoaded={this.isLoaded}
							token={token}
							tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
							handleSnackbarOpen={this.handleSnackbarOpen}
							handleEditClick={this.handleEditClick}
							// handleLaunchClick={this.handleLaunchClick}
							handleDeleteClick={this.handleDeleteClick}
							// canOpen={false}
							canEdit={true}
							canDelete={true} />
					</Paper>
				</Grid>
			</Grid>
			<Grid container className={classes.container}>
				<Grid item xs={isDesktop ? 6 : 12}>
					<Paper className={classes.paper}>
						<Button startIcon={<PersonAddIcon />} variant='outlined' color='primary' className={classes.button} style={{float: 'left', width: '50%'}} onClick={this.handleVisitorDialogOpen}>
							Добавить посетителя
						</Button>
					</Paper>
				</Grid>
				<Grid item xs={isDesktop ? 2 : 12}>
					<Paper className={classes.paper}>
						<Button
							fullWidth
							startIcon={<CancelIcon />}
							variant='outlined'
							color='secondary'
							className={classes.button}
							style={!isDesktop ? {float: 'left', width: '50%'} : null}
							onClick={() => window.history.back()} >
							Отменить
						</Button>
					</Paper>
				</Grid>
				<Grid item xs={isDesktop ? 2 : 12}>
					<Paper className={classes.paper}>
						<Button
							fullWidth
							startIcon={<SaveIcon />}
							variant='outlined'
							color='primary'
							className={classes.button}
							style={!isDesktop ? {float: 'left', width: '50%'} : null}
							onClick={() => this.handleSaveClick()} >
							Сохранить
						</Button>
					</Paper>
				</Grid>
			</Grid>
			<Snackbar open={this.state.openSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
				<Alert onClose={this.handleSnackbarClose} severity={this.state.snackbarSeverity}>
					{this.state.snackbarMsg}
				</Alert>
			</Snackbar>
			<Backdrop className={classes.backdrop} open={this.state.loading}>
				<CircularProgress color='inherit' />
			</Backdrop>
			<div>
				<Dialog
					open={this.state.openVisitorDialog}
					onClose={() => this.handleVisitorDialogClose()}
					scroll={'paper'}
					fullWidth
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<AddVisitor
						handleVisitorDialogClose={this.handleVisitorDialogClose}
						isLoaded={this.isLoaded}
						editVisitorData={this.state.editVisitorData}
						handleSnackbarOpen={this.handleSnackbarOpen} />
				</Dialog>
			</div>
		</div>
	)
}
}

export default withStyles(styles, {withTheme: true})(AddCard)
