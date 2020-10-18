import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import Radio from '@material-ui/core/Radio'
import RadioGroup from '@material-ui/core/RadioGroup'
import FormControlLabel from '@material-ui/core/FormControlLabel'
import Typography from '@material-ui/core/FormControl'
import {Calendar, momentLocalizer} from 'react-big-calendar'
import moment from 'moment'
import withDragAndDrop from 'react-big-calendar/lib/addons/dragAndDrop'
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
require('react-big-calendar/lib/addons/dragAndDrop/styles.css')
require('react-big-calendar/lib/css/react-big-calendar.css')

const localizer = momentLocalizer(moment)
const DnDCalendar = withDragAndDrop(Calendar)
const minTime = new Date()
minTime.setHours(7, 0, 0)
const maxTime = new Date()
maxTime.setHours(22, 0, 0)

const minPickerTime = moment('6:59', 'HH:mm')
const maxPickerTime = moment('22:01', 'HH:mm')

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
		margin: theme.spacing.unit,
	},
	button: {
		margin: theme.spacing.unit,
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
		<p style={{wordBreak: 'break-all', whiteSpace: 'normal'}}>Симптомы: <i>{event.complain}</i></p>
	</span>
)

class Home extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isCalendarResizable: true,
			dateRange: this.getDateRange(new Date(), 'week'),
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			searchData: '',
			employeesOptions: [],
			mainSelectedEmployee: null,
			toEmployee: null,
			selectedProcedures: [],
			proceduresOptions: [],
			events: [],
			id: 0,
			code: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
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
		// var range = this.getDateRange(new Date(), 'week')
		this.getAppointments()
	}

	componentDidUpdate(prevProps) {
		if (!_.isEqual(prevProps.currentUser, this.props.currentUser)) {
			// var range = this.getDateRange(new Date(), 'week')
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

		console.log('fff', date)

		if (view == 'month') {
			var monthStart = new Date(date.getFullYear(), date.getMonth(), 1),
				monthEnd = new Date(date.getFullYear(), date.getMonth() + 1, 0)
			return {rangeStart: monthStart, rangeEnd: monthEnd}
		} else if (view == 'week' || !view) {
			var day = date.getDay() , diffToMonday = date.getDate() - day + (day === 0 ? -6 : 1)
				, date2 = new Date(date)
				, weekMonday = new Date(date.setDate(diffToMonday))
				, weekSunday = new Date(date2.setDate(diffToMonday + 6))
			console.log('ssss', date, day, diffToMonday, weekSunday)
			return {rangeStart: weekMonday, rangeEnd: weekSunday}
		} else {
			return {rangeStart: new Date(), rangeEnd: new Date()}
		}
	}

	getAppointments = () => {
		const {token, currentUser} = this.props
		const {mainSelectedEmployee, dateRange} = this.state

		console.log('dateRange', dateRange)

		if ((mainSelectedEmployee && mainSelectedEmployee.id || currentUser && currentUser.crmEmployeesId) && dateRange) {
			this.isLoaded(false)

			let filter = {
				start: moment(dateRange.rangeStart).format(),
				end: moment(dateRange.rangeEnd).format(),
			}

			if (mainSelectedEmployee) {
				filter.toEmployee = mainSelectedEmployee
			} else {
				filter.toEmployee = {id: currentUser.crmEmployeesId}
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
			isCalendarResizable: view !== 'month',
			toEmployee: null,
			selectedProcedures: [],
			searchData: '',
			id: 0,
			code: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
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
		console.log('dates, view', dates, view)
	}

	handleClose = () => {
		this.setState({
			openEvent: false,
			openSlot: false,
			toEmployee: null,
			selectedProcedures: [],
			searchData: '',
			id: 0,
			code: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
			start: new Date(),
			end: new Date(),
		})
	}

	handleSlotSelected = slotInfo => {
		this.setState({
			id: 0,
			code: '',
			toEmployee: this.state.mainSelectedEmployee,
			selectedProcedures: [],
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			nameRu: '',
			nameEn: '',
			nameKz: '',
			surnameRu: '',
			surnameEn: '',
			surnameKz: '',
			middlenameRu: '',
			middlenameEn: '',
			middlenameKz: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
			start: slotInfo.start,
			end: slotInfo.end,
			openSlot: true,
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
			title: event.title || '',
			complain: event.complain || '',
			iin: event.iin || '',
			documentNumber: event.documentNumber || '',
			nameRu: event.nameRu || '',
			nameEn: event.nameEn || '',
			nameKz: event.nameKz || '',
			surnameRu: event.surnameRu || '',
			surnameEn: event.surnameEn || '',
			surnameKz: event.surnameKz || '',
			middlenameRu: event.middlenameRu || '',
			middlenameEn: event.middlenameEn || '',
			middlenameKz: event.middlenameKz || '',
			phoneNumber: event.phoneNumber || '',
			toEmployee: event.toEmployee,
			selectedProcedures: event.selectedProcedures,
		})
	}

	setNewAppointment = () => {
		const {
			title,
			complain,
			mainSelectedEmployee,
			toEmployee,
			selectedProcedures,
			startDate,
			endDate,
			iin,
			documentNumber,
			nameRu,
			nameEn,
			nameKz,
			surnameRu,
			surnameEn,
			surnameKz,
			middlenameRu,
			middlenameEn,
			middlenameKz,
			phoneNumber,
		} = this.state
		let {start, end} = this.state

		if (startDate < endDate) {
			let newDate = new Date(startDate.getFullYear() + '/'
				+ (startDate.getMonth() + 1 < 10 ? '0' + (startDate.getMonth() + 1) : startDate.getMonth() + 1) + '/'
				+ (startDate.getDate() < 10 ? '0' + startDate.getDate() : startDate.getDate()))
			// eslint-disable-next-line no-unmodified-loop-condition
			for (var d = newDate; d <= endDate; d.setDate(d.getDate() + 1)) {
				const newStart = new Date(d.getFullYear(), d.getMonth(), d.getDate(), start.getHours(), start.getMinutes(), start.getSeconds())
				const newEnd = new Date(d.getFullYear(), d.getMonth(), d.getDate(), end.getHours(), end.getMinutes(), end.getSeconds())
				const newItem = {
					title,
					toEmployee: toEmployee || mainSelectedEmployee,
					selectedProcedures,
					start: moment(newStart).format(),
					end: moment(newEnd).format(),
					complain,
					code: this.uuidv4(),
					iin,
					documentNumber,
					nameRu,
					nameEn,
					nameKz,
					surnameRu,
					surnameEn,
					surnameKz,
					middlenameRu,
					middlenameEn,
					middlenameKz,
					phoneNumber,
				}
				this.saveAppointment(newItem)
			}
		} else {
			const newItem = {
				title,
				toEmployee: toEmployee || mainSelectedEmployee,
				selectedProcedures,
				start: moment(start).format(),
				end: moment(end).format(),
				complain,
				code: this.uuidv4(),
				iin,
				documentNumber,
				nameRu,
				nameEn,
				nameKz,
				surnameRu,
				surnameEn,
				surnameKz,
				middlenameRu,
				middlenameEn,
				middlenameKz,
				phoneNumber,
			}
			this.saveAppointment(newItem)
		}
	}

	saveAppointment = appointment => {
		const {token} = this.props

		this.isLoaded(false)

		postRequest(`${allConstants.serverUrl}/api/Appointments/SaveAppointment`, token, appointment, result => {
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

	saveAppointmentStartEnd = (id, start, end) => {
		const {token} = this.props

		this.isLoaded(false)

		postRequest(`${allConstants.serverUrl}/api/Appointments/SetAppointmentStartEnd`, token, {id, start: moment(start).format(), end: moment(end).format()}, result => {
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

	uuidv4 = () => 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
		// eslint-disable-next-line no-bitwise
		var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8
		return v.toString(16)
	})

	//Updates Existing Appointments Title and/or Description
	updateEvent = () => {
		const {
			id,
			code,
			title,
			complain,
			mainSelectedEmployee,
			toEmployee,
			selectedProcedures,
			start,
			end,
			events,
			clickedEvent,
			iin,
			documentNumber,
			nameRu,
			nameEn,
			nameKz,
			surnameRu,
			surnameEn,
			surnameKz,
			middlenameRu,
			middlenameEn,
			middlenameKz,
			phoneNumber,
		} = this.state
		const index = events.findIndex(event => event === clickedEvent)
		const updatedEvent = events.slice()
		updatedEvent[index].id = id
		updatedEvent[index].code = code || ''
		updatedEvent[index].toEmployee = toEmployee || mainSelectedEmployee
		updatedEvent[index].selectedProcedures = selectedProcedures
		updatedEvent[index].title = title || ''
		updatedEvent[index].complain = complain || ''
		updatedEvent[index].start = start
		updatedEvent[index].end = end
		updatedEvent[index].iin = iin || ''
		updatedEvent[index].documentNumber = documentNumber || ''
		updatedEvent[index].nameRu = nameRu || ''
		updatedEvent[index].nameEn = nameEn || ''
		updatedEvent[index].nameKz = nameKz || ''
		updatedEvent[index].surnameRu = surnameRu || ''
		updatedEvent[index].surnameEn = surnameEn || ''
		updatedEvent[index].surnameKz = surnameKz || ''
		updatedEvent[index].middlenameRu = middlenameRu || ''
		updatedEvent[index].middlenameEn = middlenameEn || ''
		updatedEvent[index].middlenameKz = middlenameKz || ''
		updatedEvent[index].phoneNumber = phoneNumber || ''
		const newItem = {
			id: updatedEvent[index].id,
			code: updatedEvent[index].code,
			toEmployee: toEmployee || mainSelectedEmployee,
			selectedProcedures,
			title,
			complain,
			start,
			end,
			iin,
			documentNumber,
			nameRu,
			nameEn,
			nameKz,
			surnameRu,
			surnameEn,
			surnameKz,
			middlenameRu,
			middlenameEn,
			middlenameKz,
			phoneNumber,
		}
		this.saveAppointment(newItem)
		// this.setState({
		// 	events: updatedEvent,
		// })
	}

	//filters out specific event that is to be deleted and set that variable to state
	deleteEvent = () => {
		// let updatedEvents = this.state.events.filter(
		// 	event => event.code !== this.state.code
		// )
		// this.setState({
		// 	events: updatedEvents,
		// 	toEmployee: null,
		// 	searchData: '',
		// 	id: 0,
		// 	code: '',
		// 	title: '',
		// 	complain: '',
		// })

		const {token} = this.props

		this.isLoaded(false)

		getRequest(`${allConstants.serverUrl}/api/Appointments/DeleteAppointment?id=${(this.state.id || 0)}`, token, result => {
			// this.isLoaded(true)
			this.handleClose()
			if (result && !result.isSuccess) {
				this.handleSnackbarOpen(`Во время удаления произошла ошибка: ${result.msg}`, 'error')
			} else if (!result) {
				this.handleSnackbarOpen('Во время удаления ошибка', 'error')
			}
			this.getAppointments()
		},
		error => {
			this.isLoaded(true)
			this.handleClose()
			this.handleSnackbarOpen(`Во время удаления произошла ошибка: ${error}`, 'error')
		})
	}

	onEventResize = data => {
		if (this.state.isCalendarResizable) {
			const {start, end} = data

			let updatedEvents = this.state.events.map(e => {
				if (e.code == data.event.code) {
					e.start = start
					e.end = end
				}
				return e
			})
			this.setState({events: updatedEvents})

			this.saveAppointmentStartEnd(data.event.id, start, end)
		}
	}

	onEventDrop = data => {
		const {start, end} = data

		let updatedEvents = this.state.events.map(e => {
			if (e.code == data.event.code) {
				e.start = start
				e.end = end
			}
			return e
		})
		this.setState({events: updatedEvents})

		this.saveAppointmentStartEnd(data.event.id, start, end)
	}

	handleStartDateChange = date => {
		if (!date) {
			this.setState({startDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/'))})
		} else {
			const newDate = new Date(date.getFullYear() + '/' + (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '/' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()))
			if (this.state.endDate && newDate > this.state.endDate) {
				this.setState({
					startDate: newDate,
					endDate: newDate,
				})
			} else {
				this.setState({startDate: newDate})
			}
		}
	}

	handleEndDateChange = date => {
		if (!date) {
			if (this.state.startDate) {
				this.setState({endDate: this.state.startDate})
			} else {
				this.setState({endDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/'))})
			}
		} else {
			let newDate = new Date(date.getFullYear() + '/' + (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '/' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()))
			if (this.state.startDate && newDate < this.state.startDate) {
				newDate = this.state.startDate
			}
			this.setState({endDate: newDate})
		}
	}

	handleStartTime = time => {
		let newTime = null
		if (minPickerTime.isBefore(time) && maxPickerTime.isAfter(time)) {
			this.setState({start: time})
			newTime = time
		} else {
			newTime = new Date()
		}

		this.setState({start: newTime})
		if (this.state.end && this.state.end < newTime) {
			this.setState({end: newTime})
		}
	}

	handleEndTime = time => {
		let newTime = null
		if (time && this.state.start && this.state.start > time) {
			newTime = this.state.start
		} else if (minPickerTime.isBefore(time) && maxPickerTime.isAfter(time)) {
			newTime = time //.setDate(time.getDate() + 1)
		} else {
			newTime = new Date()
		}
		this.setState({end: newTime})
	}

	handleChange = e => {
		this.setState({...this.state, [e.target.name]: e.target.value})
		if (e.target.name == 'iin' && e.target.value && e.target.value.length == 12) {
			this.getPatientByDocumentNumber(e.target.value)
		}
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

	handleDocumentNumberKeydown = event => {
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
		const docNumCh = String.fromCharCode(event.keyCode)
		const docNumKey = event.key
		const docNumRegEx = new RegExp(this.regexStr)
		/* eslint-disable */
		const replaced = docNumKey.replace(/[^A-Za-z]/gi, "")
		if (replaced || docNumRegEx.test(docNumCh) || event.keyCode > 95 && event.keyCode < 106) {
			return
		} else {
			event.preventDefault()
		}
		/* eslint-enable */
	}

	handleAutocompleteChange = (e, v) => {
		this.setState({
			...this.state,
			[e]: v,
			searchData: '',
		}, () => {
			this.getAppointments()
		})
	}

	handleAutocompleteInputChange = v => {
		window.clearTimeout(this.searchTimeout)
		this.setState({
			searchData: v,
		})
		this.searchTimeout = setTimeout(() => {
			this.filterData()
		}, 1000)
	}

	getPatientByDocumentNumber = docNum => {
		const {token} = this.props
		this.isLoaded(false)

		getRequest(`${allConstants.serverUrl}/api/Patients/GetPatientByDocumentNumber?docNum=${docNum}`, token, result => {
			this.isLoaded(true)
			if (result && result.isSuccess && result.data) {
				this.setState({
					documentNumber: result.data.documentNumber,
					nameRu: result.data.nameRu,
					nameEn: result.data.nameEn,
					nameKz: result.data.nameKz,
					surnameRu: result.data.surnameRu,
					surnameEn: result.data.surnameEn,
					surnameKz: result.data.surnameKz,
					middlenameRu: result.data.middlenameRu,
					middlenameEn: result.data.middlenameEn,
					middlenameKz: result.data.middlenameKz,
					phoneNumber: result.data.phoneNumber,
				})
			} else {
				this.handleSnackbarOpen('Во время получения данных произошла ошибка', 'error')
			}
		},
		error => {
			this.isLoaded(true)
			this.handleSnackbarOpen(`Во время получения данных произошла ошибка: ${error}`, 'error')
		})
	}

	filterData = () => {
		const {token} = this.props
		const {searchData} = this.state
		this.isLoaded(false)

		getRequest(`${allConstants.serverUrl}/api/Users/GetUsersBySearch?searchData=${searchData}&toAppointment=true`, token, result => {
			this.isLoaded(true)
			if (Array.isArray(result)) {
				this.setState({
					employeesOptions: [...result],
				})
			} else {
				this.handleSnackbarOpen('Во время получения списка произошла ошибка', 'error')
			}
			window.clearTimeout(this.searchTimeout)
		},
		error => {
			this.isLoaded(true)
			this.handleSnackbarOpen(`Во время получения списка произошла ошибка: ${error}`, 'error')
			window.clearTimeout(this.searchTimeout)
		})
	}

	render() {
		const {classes} = this.props
		const {
			id,
			isCalendarResizable,
			employeesOptions,
			mainSelectedEmployee,
			toEmployee,
			selectedProcedures,
			proceduresOptions,
			searchData,
			events,
			openSlot,
			openEvent,
			title,
			complain,
			start,
			end,
			startDate,
			endDate,
			openSnackbar,
			snackbarMsg,
			snackbarSeverity,
			loading,
			iin,
			documentNumber,
			nameRu,
			nameEn,
			nameKz,
			surnameRu,
			surnameEn,
			surnameKz,
			middlenameRu,
			middlenameEn,
			middlenameKz,
			phoneNumber,
		} = this.state

		return (
			<div>
				<Grid container spacing={1} className={classes.container}>
					<Grid item xs={6}>
						<Paper className={classes.paper}>
							<Autocomplete
								name='mainSelectedEmployee'
								fullWidth
								size='small'
								value={mainSelectedEmployee}
								options={employeesOptions}
								onChange={(e, v) => { this.handleAutocompleteChange('mainSelectedEmployee', v) }}
								onInputChange={(e, v) => { this.handleAutocompleteInputChange(v) }}
								getOptionLabel={option => option.nameRu}
								renderInput={params => <TextField {...params} autoComplete='off' value={searchData} label='Доктор' variant='outlined' />}
							/>
						</Paper>
					</Grid>
					<Grid item container xs={12} className={classes.gridItem}>
						<Grid item xs={12}>
							<Paper className={classes.paper}>
								<DnDCalendar
									localizer={localizer}
									onEventDrop={this.onEventDrop}
									onEventResize={this.onEventResize}
									onView={this.onCurrentViewChange}
									resizable={isCalendarResizable}
									events={events}
									views={['month', 'week', 'day']}
									timeslots={2}
									step={15}
									defaultView='week'
									defaultDate={new Date()}
									selectable={isCalendarResizable}
									onRangeChange={this.onCalendarRangeChange}
									// onNavigate={this.onCalendarNavigate}
									onSelectEvent={event => this.handleEventSelected(event)}
									onSelectSlot={slotInfo => this.handleSlotSelected(slotInfo)}
									culture='ru-RU'
									min={minTime}
									max={maxTime}
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
					<DialogTitle className={classes.headerStyle} id='add-event-dialog-title'>{openEvent ? 'Редактирование' : openSlot ? 'Добавление' : ''}</DialogTitle>
					<DialogContent dividers={true}>
						<div className={classes.modalRoot}>
							<Grid container spacing={1}>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											name='iin'
											error={(iin && iin.length < 12 || !iin && !documentNumber)}
											fullWidth={true}
											size='small'
											autoComplete='off'
											value={iin}
											label='ИИН'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description', maxLength: 12, onKeyDown: this.handleIinKeydown}}
											onChange={this.handleChange}/>
									</Paper>
								</Grid>
								<Grid container item xs={6}>
									<Grid item xs={8}>
										<Paper className={classes.paper}>
											<TextField
												error={(documentNumber && documentNumber.length < 4 || !iin && !documentNumber || iin && iin.length < 12)}
												name='documentNumber'
												fullWidth={true}
												size='small'
												autoComplete='off'
												value={documentNumber}
												label='Номер документа'
												variant='outlined'
												className={classes.input}
												inputProps={{'aria-label': 'Description', onKeyDown: this.handleDocumentNumberKeydown}}
												onChange={this.handleChange}/>
										</Paper>
									</Grid>
									<Grid item xs={4}>
										<Paper className={classes.paper}>
											<Tooltip title='Поиск по номеру документа'>
												<Button
													fullWidth
													variant='outlined'
													color='primary'
													disabled={(!documentNumber)}
													className={classes.button}
													onClick={() => this.getPatientByDocumentNumber(documentNumber)} >
													Найти
												</Button>
											</Tooltip>
										</Paper>
									</Grid>
								</Grid>
							</Grid>
							<Divider className={classes.divider} />
							<MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
								<Grid container spacing={1}>
									<Grid container item xs={6}>
										<Grid item xs={12}>
											<Paper className={classes.paper}>
												<Autocomplete
													name='toEmployee'
													fullWidth
													size='small'
													value={toEmployee}
													options={employeesOptions}
													groupBy={option => option.positionId}
													getOptionLabel={option => option.positionNameRu}
													className={classes.input}
													onChange={(e, v) => { this.handleAutocompleteChange('toEmployee', v) }}
													onInputChange={(e, v) => { this.handleAutocompleteInputChange(v) }}
													getOptionLabel={option => option.nameRu}
													renderOption={option => <span>{option.nameRu} ({option.positionNameRu})</span>}
													renderInput={params => <TextField {...params} autoComplete='off' value={searchData} label='Доктор' variant='outlined' />}
												/>
											</Paper>
										</Grid>
										<Grid item xs={12}>
											<Paper className={classes.paper}>
												<Autocomplete
													name='selectedProcedures'
													multiple
													fullWidth
													filterSelectedOptions
													size='small'
													value={selectedProcedures}
													options={proceduresOptions}
													className={classes.input}
													onChange={(e, v) => { this.handleAutocompleteChange('selectedProcedures', v) }}
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
												rows={4}
												size='small'
												autoComplete='off'
												value={complain}
												className={classes.input}
												label='Симптомы'
												variant='outlined'
												inputProps={{'aria-label': 'Description'}}
												onChange={this.handleChange}/>
										</Paper>
									</Grid>
									{
										(id <= 0 || !id) && <React.Fragment>
											<Grid item xs={6}>
												<Paper className={classes.paper}>
													<KeyboardDatePicker
														margin='normal'
														inputVariant='outlined'
														variant='dialog'
														cancelLabel='Отменить'
														okLabel='Выбрать'
														fullWidth
														size='small'
														className={classes.input}
														label='Дата начала'
														format='dd.MM.yyyy'
														value={startDate}
														minDate={new Date()}
														onChange={this.handleStartDateChange}
														minDateMessage={`Дата не может быть раньше ${((new Date().getDate() < 10 ? '0' + new Date().getDate() : new Date().getDate()) + '.' + (new Date().getMonth() + 1 < 10 ? '0' + (new Date().getMonth() + 1) : new Date().getMonth() + 1) + '.' + new Date().getFullYear())}`}
														invalidDateMessage='Неверный формат даты'
														KeyboardButtonProps={{'aria-label': 'change date'}}/>
												</Paper>
											</Grid>
											<Grid item xs={6}>
												<Paper className={classes.paper}>
													<KeyboardDatePicker
														margin='normal'
														inputVariant='outlined'
														variant='dialog'
														cancelLabel='Отменить'
														okLabel='Выбрать'
														fullWidth
														size='small'
														className={classes.input}
														label='Дата окончания'
														format='dd.MM.yyyy'
														value={endDate}
														minDate={startDate}
														onChange={this.handleEndDateChange}
														minDateMessage={`Дата не может быть раньше ${((startDate.getDate() < 10 ? '0' + startDate.getDate() : startDate.getDate()) + '.' + (startDate.getMonth() + 1 < 10 ? '0' + (startDate.getMonth() + 1) : startDate.getMonth() + 1) + '.' + startDate.getFullYear())}`}
														invalidDateMessage='Неверный формат даты'
														KeyboardButtonProps={{'aria-label': 'change date'}}/>
												</Paper>
											</Grid>
										</React.Fragment>
									}
									<Grid item xs={6}>
										<Paper className={classes.paper}>
											<KeyboardTimePicker
												margin='normal'
												inputVariant='outlined'
												variant='dialog'
												cancelLabel='Отменить'
												okLabel='Выбрать'
												fullWidth
												size='small'
												className={classes.input}
												ampm={false}
												label='Время начала'
												value={start}
												onChange={this.handleStartTime}
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
												size='small'
												className={classes.input}
												ampm={false}
												label='Время окончания'
												value={end}
												onChange={this.handleEndTime}
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
											required
											error={!surnameRu}
											name='surnameRu'
											fullWidth
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
											error={!nameRu}
											name='nameRu'
											fullWidth
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
											size='small'
											autoComplete='off'
											value={middlenameRu}
											label='Отчество'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
								</Grid>
								<Divider className={classes.divider} />
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											name='phoneNumber'
											fullWidth
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
							</Grid>
						</div>
					</DialogContent>
					<DialogActions>
						<Button
							onClick={this.handleClose}
							startIcon={<CancelIcon />}
							className={classes.actionButtons}
							variant='outlined'
							size='medium'
							color='secondary'>
								Отменить
						</Button>
						{
							openEvent && <Button
								onClick={() => { this.deleteEvent() }}
								startIcon={<DeleteIcon />}
								className={classes.actionButtons}
								variant='outlined'
								size='medium'
								color='primary'>
								Удалить
							</Button>
						}
						<Button
							onClick={() => {
								if (openEvent) {
									this.updateEvent()
								} else if (openSlot) {
									this.setNewAppointment()
								}
							}}
							startIcon={<SaveIcon />}
							className={classes.actionButtons}
							variant='outlined'
							size='medium'
							color='primary'>
								Сохранить{openEvent && ' изменения'}
						</Button>
					</DialogActions>
				</Dialog>
			</div>
		)
	}
}

export default withStyles(styles, {withTheme: true})(Home)
