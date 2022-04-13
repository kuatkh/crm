import React from 'react'
import PropTypes from 'prop-types'
import {withStyles} from '@mui/styles'
import {
	Grid,
	Typography,
	FormGroup,
	FormControlLabel,
	TextField,
	Dialog,
	DialogActions,
	Button,
	Autocomplete,
	Divider,
	Paper,
	Switch,
} from '@mui/material'
import PersonAddIcon from '@mui/icons-material/PersonAdd'
import {connect} from 'react-redux'
import ruLocale from 'date-fns/locale/ru'
import AdapterDateFns from '@mui/lab/AdapterDateFns'
import DatePicker from '@mui/lab/DatePicker'
import LocalizationProvider from '@mui/lab/LocalizationProvider'
import AddUser from './AddUser'
import AbTable from 'Components/AbTable'
import {allConstants} from 'Constants/AllConstants.js'
import {usersColumns} from 'Constants/TableColumns.js'
import {getRequest} from 'Services/RequestsServices.js'

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
		gridGap: theme.spacing(1),
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
})

const defaultCurrentDate = new Date()

class Users extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			beginDate: new Date(),
			endDate: new Date(),
			filterByCreatedDate: false,
			departmentsOptions: [{id: 0, nameRu: 'Все'}],
			selectedDepartment: {id: 0, nameRu: 'Все'},
			selectedUser: null,
			openEditUserDialog: false,
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
		}

		this._abTableGetData = null
	}

	componentDidMount() {
		this.getDepartments()
	}

	getDepartments = () => {
		const {token} = this.props
		this.isLoaded(false)

		getRequest(`${allConstants.serverUrl}/api/Admin/GetDepartments`, token, result => {
			this.isLoaded(true)
			if (Array.isArray(result)) {
				this.setState({
					departmentsOptions: [...this.state.departmentsOptions, ...result],
				})
			} else {
				this.handleSnackbarOpen('Во время получения списка подразделений произошла ошибка', 'error')
			}
		},
		error => {
			this.isLoaded(true)
			this.handleSnackbarOpen(`Во время получения списка подразделений произошла ошибка: ${error}`, 'error')
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

handleSwitchChange = e => {
	this.setState({filterByCreatedDate: e.target.checked})
}

handleEditUserDialogOpen = () => {
	this.setState({
		openEditUserDialog: true,
	})
}

handleEditUserDialogClose = isSuccess => {
	if (isSuccess) {
		this.setState({
			openEditUserDialog: false,
			beginDate: new Date(),
			endDate: new Date(),
			filterByCreatedDate: false,
			selectedDepartment: {id: 0, nameRu: 'Все'},
		}, () => {
			if (this._abTableGetData) {
				this._abTableGetData()
			}
		})
	} else {
		this.setState({
			openEditUserDialog: false,
		})
	}
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
	})
}

render() {
	const {classes, token} = this.props
	const {
		openSnackbar,
		snackbarMsg,
		snackbarSeverity,
		loading,
		openEditUserDialog,
		selectedDepartment,
		departmentsOptions,
		beginDate,
		endDate,
		filterByCreatedDate,
	} = this.state

	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>Пользователи системы</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container spacing={1} className={classes.container}>
				<Grid item container xs={12}>
					<Grid item xs={8}>
						<Paper className={classes.paper}>
							<Autocomplete
								id='selectedDepartment'
								name='selectedDepartment'
								disableClearable
								fullWidth
								value={selectedDepartment}
								options={departmentsOptions}
								onChange={(e, v) => { this.handleAutocompleteChange('selectedDepartment', v) }}
								getOptionLabel={option => option.nameRu}
								renderInput={params => <TextField {...params} label='Структурное подразделение' variant='outlined' />}
							/>
						</Paper>
					</Grid>
					<Grid item xs={8}>
						<Paper className={classes.paper}>
							<FormGroup>
								<FormControlLabel
									control={<Switch name='withDeadline' checked={filterByCreatedDate} onChange={this.handleSwitchChange} aria-label='deadline switch' />}
									label={'Фильтровать по дате создания'}
								/>
							</FormGroup>
							{
								filterByCreatedDate && (
									<Grid item container xs={12}>
											<Grid item xs={6}>
												<Paper className={classes.paper}>
												<LocalizationProvider dateAdapter={AdapterDateFns} locale={ruLocale}>
													<DatePicker
														margin='normal'
														inputVariant='outlined'
														variant='dialog'
														cancelLabel='Отменить'
														okLabel='Выбрать'
														fullWidth
														label='Дата создания пользователя с'
														format='dd.MM.yyyy'
														value={beginDate}
														onChange={this.handleBeginDateChange}
														invalidDateMessage='Неверный формат даты'
														KeyboardButtonProps={{'aria-label': 'change date'}}/>
												</LocalizationProvider>
												</Paper>
											</Grid>
											<Grid item xs={6}>
												<Paper className={classes.paper}>
												<LocalizationProvider dateAdapter={AdapterDateFns} locale={ruLocale}>
													<DatePicker
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
												</LocalizationProvider>
												</Paper>
											</Grid>
									</Grid>
								)
							}
						</Paper>
					</Grid>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<br/>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={6}>
					<Paper className={classes.paper}>
						<Button startIcon={<PersonAddIcon />} variant='outlined' color='secondary' className={classes.button} style={{float: 'left', width: '50%'}} onClick={this.handleEditUserDialogOpen}>
							Добавить пользователя
						</Button>
					</Paper>
				</Grid>
			</Grid>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<AbTable
							url={`${allConstants.serverUrl}/api/Admin/GetUsers`}
							filterData={{
								departmentId: selectedDepartment ? selectedDepartment.id : 0,
								filterByCreatedDate,
								beginDate: filterByCreatedDate ? beginDate : defaultCurrentDate,
								endDate: filterByCreatedDate ? endDate : defaultCurrentDate,
							}}
							setGetDataFunc={func => this._abTableGetData = func}
							tableStyle={{tableLayout: 'fixed'}}
							defaultOrderByColumn='surnameRu'
							columns={usersColumns}
							isLoaded={this.isLoaded}
							tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
							handleSnackbarOpen={this.handleSnackbarOpen}
						/>
					</Paper>
				</Grid>
			</Grid>
			<div>
				<Dialog
					open={openEditUserDialog}
					onClose={() => this.handleEditUserDialogClose(false)}
					scroll={'paper'}
					fullWidth={true}
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<AddUser
						handleEditUserDialogClose={this.handleEditUserDialogClose}
						isLoaded={this.isLoaded}
						token={token}
						handleSnackbarOpen={this.handleSnackbarOpen} />
				</Dialog>
			</div>
		</div>
	)
}
}

Users.propTypes = {
	classes: PropTypes.object.isRequired,
}

function mapStateToProps(state) {
	const {currentUser, token} = state
	return {
		currentUser,
		token,
	}
}

export default connect(mapStateToProps)(withStyles(styles, {withTheme: true})(Users))
