import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import Radio from '@material-ui/core/Radio'
import RadioGroup from '@material-ui/core/RadioGroup'
import FormControlLabel from '@material-ui/core/FormControlLabel'
import Typography from '@material-ui/core/FormControl'
import {Calendar, momentLocalizer} from 'react-big-calendar'
import moment from 'moment'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import Tooltip from '@material-ui/core/Tooltip'
import Button from '@material-ui/core/Button'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import DeleteIcon from '@material-ui/icons/Delete'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Paper from '@material-ui/core/Paper'
import Autocomplete from '@material-ui/lab/Autocomplete'
import TextField from '@material-ui/core/TextField'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import {MuiPickersUtilsProvider, KeyboardDatePicker, KeyboardTimePicker} from '@material-ui/pickers'
import AccessTimeIcon from '@material-ui/icons/AccessTime'
import DateFnsUtils from '@date-io/date-fns'
import ruLocale from 'date-fns/locale/ru'
import {
	pink, purple, teal, amber, deepOrange,
} from '@material-ui/core/colors'
import _ from 'lodash'
import {allConstants} from '../../Constants/AllConstants.js'
import {getRequest, postRequest} from '../../Services/RequestsServices.js'
require('react-big-calendar/lib/css/react-big-calendar.css')

const localizer = momentLocalizer(moment)
const minTime = new Date()
minTime.setHours(7, 0, 0)
const maxTime = new Date()
maxTime.setHours(22, 0, 0)

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
		gridTemplateColumns: 'repeat(12, 1fr)',
		gridGap: theme.spacing(1),
	},
	gridItem: {
		height: '100%',
		minHeight: '60vh',
	},
	input: {
		margin: theme.spacing(1),
	},
	button: {
		margin: theme.spacing(1),
		overflow: 'hidden',
	},
	paper: {
		paddingRight: theme.spacing(1),
		// textAlign: 'center',
		color: theme.palette.text.secondary,
		whiteSpace: 'nowrap',
		marginBottom: theme.spacing(1),
		boxShadow: 'none',
		height: '100%',
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
	backdrop: {
		zIndex: theme.zIndex.drawer + 3,
		color: '#fff',
	},
})

const Event = ({event}) => (
	<span>
		<p><b>{event.surnameRu} {event.nameRu} {event.middlenameRu}</b></p>
		<p>{event.toEmployee ? event.toEmployee.nameRu : ''}</p>
		{event.complain && <p style={{wordBreak: 'break-all', whiteSpace: 'normal'}}>Симптомы: <i>{event.complain}</i></p>}
	</span>
)

class Home extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			dateRange: this.getDateRange(new Date(), 'week'),
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			toEmployee: null,
			selectedProcedures: [],
			proceduresOptions: [],
			events: [],
			id: 0,
			code: '',
			complain: '',
			doctorsAppointment: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			start: new Date(),
			end: new Date(),
			openSlot: false,
			openEvent: false,
			clickedEvent: {},
		}

		this.searchTimeout = null
		this.regexStr = '^[0-9]*$'
	}

	componentDidMount() {
		this.getAppointments()
	}

	componentDidUpdate(prevProps) {
		if (!_.isEqual(prevProps.currentUser, this.props.currentUser)) {
			this.getAppointments()
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

	isLoaded = loading => {
		this.setState({
			loading: !loading,
		})
	}

	getDateRange = (date, view) => {
		if (!date) {
			date = new Date()
		}

		if (view == 'month') {
			var monthStart = new Date(date.getFullYear(), date.getMonth(), 1),
				monthEnd = new Date(date.getFullYear(), date.getMonth() + 1, 0)
			return {rangeStart: monthStart, rangeEnd: monthEnd}
		} else if (view == 'week' || !view) {
			var day = date.getDay() , diffToMonday = date.getDate() - day + (day === 0 ? -6 : 1)
				, date2 = new Date(date)
				, weekMonday = new Date(date.setDate(diffToMonday))
				, weekSunday = new Date(date2.setDate(diffToMonday + 6))
			return {rangeStart: weekMonday, rangeEnd: weekSunday}
		} else {
			return {rangeStart: new Date(), rangeEnd: new Date()}
		}
	}

	getAppointments = () => {
		const {token, currentUser} = this.props
		const {dateRange} = this.state

		if (currentUser && currentUser.crmEmployeesId && dateRange) {
			this.isLoaded(false)

			let filter = {
				start: moment(dateRange.rangeStart).format(),
				end: moment(dateRange.rangeEnd).format(),
				toEmployee: {id: currentUser.crmEmployeesId},
			}

			postRequest(`${allConstants.serverUrl}/api/Appointments/GetAppointments`, token, filter, result => {
				this.isLoaded(true)
				if (result && result.isSuccess && Array.isArray(result.data)) {
					this.setState({events: result.data.map(d => {
						d.start = new Date(d.start)
						d.end = new Date(d.end)
						return d
					})})
				} else {
					this.handleSnackbarOpen(`Во время получения данных произошла ошибка ${(result && !result.isSuccess && result.msg ? result.msg : '')}`, 'error')
				}
			},
			error => {
				this.isLoaded(true)
				this.handleSnackbarOpen(`Во время получения данных произошла ошибка: ${error}`, 'error')
			})
		}
	}

	onCurrentViewChange = view => {
		this.setState({
			selectedProcedures: [],
			id: 0,
			code: '',
			complain: '',
			doctorsAppointment: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			start: new Date(),
			end: new Date(),
			dateRange: this.getDateRange(this.state.dateRange.rangeStart, view),
		}, () => {
			this.getAppointments()
		})
	}

	onCalendarRangeChange = (dates, view) => {
		if (Array.isArray(dates)) {
			if ((view == 'week' || !view) && dates.length == 7) {
				console.log('dddd', dates.sort((a, b) => new Date(b) - new Date(a))[6])
				this.setState({
					dateRange: this.getDateRange(dates.sort((a, b) => new Date(b) - new Date(a))[6], 'week'),
				}, () => {
					this.getAppointments()
				})
			} else if (view == 'day' && dates.length == 1) {
				this.setState({
					dateRange: {rangeStart: dates[0], rangeEnd: dates[0]},
				}, () => {
					this.getAppointments()
				})
			}
		} else if (dates.start && dates.end) {
			this.setState({
				dateRange: this.getDateRange(new Date(dates.start.setDate(dates.start.getDate() + 15)), 'month'),
			}, () => {
				this.getAppointments()
			})
		}
	}

	handleClose = () => {
		this.setState({
			openEvent: false,
			openSlot: false,
			selectedProcedures: [],
			searchData: '',
			id: 0,
			code: '',
			complain: '',
			doctorsAppointment: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			start: new Date(),
			end: new Date(),
		})
	}

	handleEventSelected = event => {
		this.setState({
			openEvent: true,
			clickedEvent: event,
			id: event.id,
			code: event.code,
			start: event.start,
			end: event.end,
			complain: event.complain || '',
			doctorsAppointment: event.doctorsAppointment || '',
			nameRu: event.nameRu || '',
			nameEn: event.nameEn || '',
			nameKz: event.nameKz || '',
			surnameRu: event.surnameRu || '',
			surnameEn: event.surnameEn || '',
			surnameKz: event.surnameKz || '',
			middlenameRu: event.middlenameRu || '',
			middlenameEn: event.middlenameEn || '',
			middlenameKz: event.middlenameKz || '',
			toEmployee: event.toEmployee,
			selectedProcedures: event.selectedProcedures,
		})
	}

	saveDoctorsAppointment = () => {
		const {token, currentUser} = this.props

		if (currentUser) {
			this.isLoaded(false)

			const {id, code, doctorsAppointment} = this.state

			const appointment = {
				id,
				code,
				doctorsAppointment,
			}

			postRequest(`${allConstants.serverUrl}/api/Appointments/SaveDoctorsAppointment`, token, appointment, result => {
				// this.isLoaded(true)
				this.handleClose()
				if (result && !result.isSuccess) {
					this.handleSnackbarOpen(`Во время сохранения произошла ошибка: ${result.msg}`, 'error')
				} else if (!result) {
					this.handleSnackbarOpen('Во время сохранения ошибка', 'error')
				}
				this.getAppointments()
			},
			error => {
				this.isLoaded(true)
				this.handleClose()
				this.handleSnackbarOpen(`Во время сохранения произошла ошибка: ${error}`, 'error')
			})
		}
	}

	handleChange = e => {
		this.setState({...this.state, [e.target.name]: e.target.value})
	}

	render() {
		const {classes} = this.props
		const {
			selectedProcedures,
			proceduresOptions,
			events,
			openSlot,
			openEvent,
			complain,
			doctorsAppointment,
			start,
			end,
			openSnackbar,
			snackbarMsg,
			snackbarSeverity,
			loading,
			nameRu,
			nameEn,
			nameKz,
			surnameRu,
			surnameEn,
			surnameKz,
			middlenameRu,
			middlenameEn,
			middlenameKz,
		} = this.state

		return (
			<div>
				<Grid container spacing={1} className={classes.container}>
					<Grid item container xs={12} className={classes.gridItem}>
						<Grid item xs={12}>
							<Paper className={classes.paper}>
								<Calendar
									localizer={localizer}
									onView={this.onCurrentViewChange}
									resizable={false}
									selectable={false}
									events={events}
									views={['month', 'week', 'day']}
									timeslots={2}
									step={15}
									defaultView='week'
									defaultDate={new Date()}
									min={minTime}
									max={maxTime}
									onRangeChange={this.onCalendarRangeChange}
									onSelectEvent={event => this.handleEventSelected(event)}
									culture='ru-RU'
									components={{event: Event}}
									messages={{
										allDay: 'Весь день',
										previous: 'Пред.',
										next: 'След.',
										today: 'Сегодня',
										month: 'Месяц',
										week: 'Неделя',
										day: 'День',
										date: 'Дата',
										time: 'Время',
									}}
								/>
							</Paper>
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

				<Dialog
					open={openSlot || openEvent}
					onClose={() => this.handleClose()}
					scroll={'paper'}
					fullWidth
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<DialogTitle className={classes.headerStyle} id='add-event-dialog-title'>Детали</DialogTitle>
					<DialogContent dividers={true}>
						<div className={classes.modalRoot}>
							<MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
								<Grid container spacing={1}>
									<Grid container item xs={6}>
										<Grid item xs={12}>
											<Paper className={classes.paper}>
												<Autocomplete
													name='selectedProcedures'
													disabled={true}
													multiple
													fullWidth
													filterSelectedOptions
													size='small'
													value={selectedProcedures}
													options={proceduresOptions}
													className={classes.input}
													getOptionLabel={option => option.nameRu}
													renderInput={params => <TextField {...params} autoComplete='off' label='Процедуры' variant='outlined' />}
												/>
											</Paper>
										</Grid>
									</Grid>
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<TextField
												name='complain'
												fullWidth
												multiline
												disabled={true}
												rows={4}
												size='small'
												autoComplete='off'
												value={complain}
												className={classes.input}
												label='Симптомы'
												variant='outlined'
												inputProps={{'aria-label': 'Description'}}/>
										</Paper>
									</Grid>
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<KeyboardTimePicker
												margin='normal'
												inputVariant='outlined'
												variant='dialog'
												cancelLabel='Отменить'
												okLabel='Выбрать'
												fullWidth
												disabled={true}
												size='small'
												className={classes.input}
												ampm={false}
												label='Время начала'
												value={start}
												keyboardIcon={<AccessTimeIcon />}
												views={['hours', 'minutes']}
												invalidDateMessage='Неверный формат времени'
												KeyboardButtonProps={{'aria-label': 'change date'}}/>
										</Paper>
									</Grid>
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<KeyboardTimePicker
												margin='normal'
												inputVariant='outlined'
												variant='dialog'
												cancelLabel='Отменить'
												okLabel='Выбрать'
												fullWidth
												disabled={true}
												size='small'
												className={classes.input}
												ampm={false}
												label='Время окончания'
												value={end}
												keyboardIcon={<AccessTimeIcon />}
												views={['hours', 'minutes']}
												invalidDateMessage='Неверный формат времени'
												KeyboardButtonProps={{'aria-label': 'change date'}}/>
										</Paper>
									</Grid>
								</Grid>
							</MuiPickersUtilsProvider>
							<Divider className={classes.divider} />
							<Grid container spacing={1}>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											name='surnameRu'
											fullWidth
											disabled={true}
											size='small'
											autoComplete='off'
											value={surnameRu}
											label='Фамилия'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}/>
									</Paper>
								</Grid>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											name='nameRu'
											fullWidth
											disabled={true}
											size='small'
											autoComplete='off'
											value={nameRu}
											label='Имя'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}/>
									</Paper>
								</Grid>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											name='middlenameRu'
											fullWidth
											disabled={true}
											size='small'
											autoComplete='off'
											value={middlenameRu}
											label='Отчество'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}/>
									</Paper>
								</Grid>
							</Grid>
							<Divider className={classes.divider} />
							<Grid container spacing={1}>
								<Grid item xs={12}>
									<Paper className={classes.paper}>
										<TextField
											name='doctorsAppointment'
											fullWidth
											multiline
											rows={4}
											size='small'
											autoComplete='off'
											value={doctorsAppointment}
											className={classes.input}
											label='Результат'
											variant='outlined'
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
								</Grid>
							</Grid>
						</div>
					</DialogContent>
					<DialogActions>
						<Button
							onClick={() => {
								this.saveDoctorsAppointment()
							}}
							disabled={!doctorsAppointment}
							startIcon={<SaveIcon />}
							className={classes.actionButtons}
							variant='outlined'
							size='medium'
							color='primary'>
								Сохранить изменения
						</Button>
						<Button
							onClick={this.handleClose}
							startIcon={<CancelIcon />}
							className={classes.actionButtons}
							variant='outlined'
							size='medium'
							color='secondary'>
							Закрыть
						</Button>
					</DialogActions>
				</Dialog>
			</div>
		)
	}
}

export default withStyles(styles, {withTheme: true})(Home)
