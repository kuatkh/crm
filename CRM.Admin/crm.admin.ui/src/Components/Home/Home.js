import React from 'react'
import {compose} from 'react-recompose'
import {withStyles} from '@mui/styles'
import {Calendar, momentLocalizer} from 'react-big-calendar'
import moment from 'moment'
import withDragAndDrop from 'react-big-calendar/lib/addons/dragAndDrop'
import {
	Grid,
	Dialog,
	DialogTitle,
	DialogContent,
	TextField,
	DialogActions,
	Button,
	Autocomplete,
	Divider,
	Paper,
	Tooltip,
} from '@mui/material'
import SaveIcon from '@mui/icons-material/Save'
import CancelIcon from '@mui/icons-material/Cancel'
import DeleteIcon from '@mui/icons-material/Delete'
import AccessTimeIcon from '@mui/icons-material/AccessTime'
import AdapterDateFns from '@mui/lab/AdapterDateFns'
import DatePicker from '@mui/lab/DatePicker'
import TimePicker from '@mui/lab/TimePicker'
import LocalizationProvider from '@mui/lab/LocalizationProvider'
import ruLocale from 'date-fns/locale/ru'
import _ from 'lodash'
import {appConstants} from 'constants/app.constants.js'
import {getRequest, postRequest} from 'services/requests.services'
import {userServices} from 'services/user.services'
import {withSnackbar} from 'components/SnackbarWrapper'
import {loading} from 'components/LoadingWrapper'
import 'moment/locale/ru'
require('react-big-calendar/lib/addons/dragAndDrop/styles.css')
require('react-big-calendar/lib/css/react-big-calendar.css')

const localizer = momentLocalizer(moment)
const DnDCalendar = withDragAndDrop(Calendar)
const minTime = new Date()
minTime.setHours(7, 0, 0)
const maxTime = new Date()
maxTime.setHours(22, 0, 0)

const minPickerTime = moment('06:59', 'HH:mm')
const maxPickerTime = moment('22:01', 'HH:mm')

const styles = theme => ({
	formControl: {
		m: theme.spacing(1),
		minWidth: 120,
		maxWidth: 300,
	},
	chips: {
		display: 'flex',
		flexWrap: 'wrap',
	},
	chip: {
		m: 2,
	},
	noLabel: {
		mt: theme.spacing(3),
	},
	selectEmpty: {
		mt: theme.spacing(2),
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
		m: theme.spacing.unit,
	},
	button: {
		m: theme.spacing.unit,
		overflow: 'hidden',
	},
	paper: {
		pr: theme.spacing(1),
		// textAlign: 'center',
		color: theme.palette.text.secondary,
		whiteSpace: 'nowrap',
		mb: theme.spacing(1),
		boxShadow: 'none',
		height: '100%',
	},
	divider: {
		m: 0,
	},
	modalRoot: {
		flexGrow: 1,
	},
	actionButtons: {
		mr: theme.spacing(2),
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
		{
			event.complain && <p style={{wordBreak: 'break-all', whiteSpace: 'normal'}}>Симптомы: <i>{event.complain}</i></p>
		}
		{
			Array.isArray(event.selectedProcedures) && event.selectedProcedures.length > 0
				? <span style={{wordBreak: 'break-all', whiteSpace: 'normal'}}>Процедуры: {
					event.selectedProcedures.map(p => <p key={'proc-' + p.id} style={{wordBreak: 'break-all', whiteSpace: 'normal', paddingLeft: '5px'}}><i><b> * {p.nameRu}</b></i></p>)
				}</span>
				: null
		}
	</span>
)

class Home extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isCalendarResizable: true,
			dateRange: this.getDateRange(new Date(), 'week'),
			searchData: '',
			proceduresSearchData: '',
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
			name: '',
			surname: '',
			middlename: '',
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
		this.proceduresSearchTimeout = null
		this.regexStr = '^[0-9]*$'
	}

	componentDidMount() {
		this.getAppointments()
		this.filterUsersData()
		this.filterProceduresData()
	}

	componentDidUpdate(prevProps, prevState) {
		if (prevState.searchData != this.state.searchData) {
			window.clearTimeout(this.searchTimeout)
			this.searchTimeout = window.setTimeout(() => {
				this.filterUsersData()
			}, 800)
		}
		if (prevState.proceduresSearchData != this.state.proceduresSearchData) {
			window.clearTimeout(this.proceduresSearchTimeout)
			this.proceduresSearchTimeout = window.setTimeout(() => {
				this.filterProceduresData()
			}, 800)
		}
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
		const currentUser = userServices.getCurrentUser()
		const {mainSelectedEmployee, dateRange} = this.state

		console.log('dateRange', dateRange)

		if ((mainSelectedEmployee && mainSelectedEmployee.id || currentUser && currentUser.crmEmployeesId) && dateRange) {
			this.props.loadingScreen.show()

			let filter = {
				start: moment(dateRange.rangeStart).format(),
				end: moment(dateRange.rangeEnd).format(),
			}

			if (mainSelectedEmployee) {
				filter.toEmployee = mainSelectedEmployee
			} else {
				filter.toEmployee = {id: currentUser.crmEmployeesId}
			}

			postRequest(`${appConstants.serverUrl}/api/Appointments/GetAppointments`, filter, result => {
				this.props.loadingScreen.hide()
				console.log('result.data', result)
				if (result && result.isSuccess && Array.isArray(result.data)) {
					this.setState({events: result.data.map(d => {
						d.start = new Date(d.start)
						d.end = new Date(d.end)
						return d
					})})
				} else {
					this.props.snackbar.showError(`Во время получения данных произошла ошибка ${(result && !result.isSuccess && result.msg ? result.msg : '')}`)
				}
			},
			error => {
				this.props.loadingScreen.hide()
				this.props.snackbar.showError(`Во время получения данных произошла ошибка: ${error}`)
			})
		}
	}

	onCurrentViewChange = view => {
		this.setState({
			isCalendarResizable: view !== 'month',
			toEmployee: null,
			selectedProcedures: [],
			searchData: '',
			proceduresSearchData: '',
			id: 0,
			code: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			name: '',
			surname: '',
			middlename: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
			start: new Date(),
			end: new Date(),
			dateRange: this.getDateRange(this.state.dateRange.rangeStart, view),
			clickedEvent: {},
		}, () => {
			this.getAppointments()
		})
	}

	onCalendarRangeChange = (dates, view) => {
		if (Array.isArray(dates)) {
			if ((view == 'week' || !view) && dates.length == 7) {
				console.log('dddd', dates.sort((a, b) => new Date(b) - new Date(a))[0])
				this.setState({
					dateRange: this.getDateRange(dates.sort((a, b) => new Date(b) - new Date(a))[0], 'week'),
				}, () => {
					this.getAppointments()
				})
			} else if ((view == 'day' || !view) && dates.length == 1) {
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
			proceduresSearchData: '',
			id: 0,
			code: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			name: '',
			surname: '',
			middlename: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
			start: new Date(),
			end: new Date(),
			clickedEvent: {},
		})
	}

	handleSlotSelected = slotInfo => {
		this.setState({
			id: 0,
			code: '',
			toEmployee: this.state.mainSelectedEmployee,
			selectedProcedures: [],
			searchData: '',
			proceduresSearchData: '',
			title: '',
			complain: '',
			iin: '',
			documentNumber: '',
			name: '',
			surname: '',
			middlename: '',
			phoneNumber: '',
			startDate: new Date(),
			endDate: new Date(),
			start: slotInfo.start,
			end: slotInfo.end,
			openSlot: true,
			clickedEvent: {},
		})
	}

	handleEventSelected = event => {
		this.setState({
			openEvent: true,
			clickedEvent: event,
			searchData: '',
			proceduresSearchData: '',
			id: event.id,
			code: event.code,
			start: new Date(event.start),
			end: new Date(event.end),
			title: event.title || '',
			complain: event.complain || '',
			iin: event.iin || '',
			documentNumber: event.documentNumber || '',
			name: event.name || '',
			surname: event.surname || '',
			middlename: event.middlename || '',
			phoneNumber: event.phoneNumber || '',
			toEmployee: event.toEmployee,
			selectedProcedures: event.selectedProcedures || [],
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
			name,
			surname,
			middlename,
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
					name,
					surname,
					middlename,
					phoneNumber,
				}
				this.saveAppointment(newItem)
			}
		} else {

			const newStart = new Date(start.getFullYear(), start.getMonth(), start.getDate(), start.getHours(), start.getMinutes(), start.getSeconds())
			const newEnd = new Date(start.getFullYear(), start.getMonth(), start.getDate(), end.getHours(), end.getMinutes(), end.getSeconds())

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
				name,
				surname,
				middlename,
				phoneNumber,
			}
			this.saveAppointment(newItem)
		}
	}

	saveAppointment = appointment => {
		const currentUser = userServices.getCurrentUser()

		if (currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) && !appointment.iin) {
			this.props.snackbar.showWarning('Вы не заполнили поле "ИИН"')
			return
		}

		if (!appointment.toEmployee) {
			this.props.snackbar.showWarning('Вы не заполнили поле "Доктор"')
			return
		}

		if (!appointment.name) {
			this.props.snackbar.showWarning('Вы не заполнили поле "Имя"')
			return
		}

		if (!appointment.surname) {
			this.props.snackbar.showWarning('Вы не заполнили поле "Фамилия"')
			return
		}
		console.log('appointment', appointment)

		this.props.loadingScreen.show()

		postRequest(`${appConstants.serverUrl}/api/Appointments/SaveAppointment`, appointment, result => {
			// this.props.loadingScreen.hide()
			this.handleClose()
			if (result && !result.isSuccess) {
				this.props.snackbar.showError(`Во время сохранения произошла ошибка: ${result.msg}`)
			} else if (!result) {
				this.props.snackbar.showError('Во время сохранения ошибка')
			}
			this.getAppointments()
		},
		error => {
			this.props.loadingScreen.hide()
			this.handleClose()
			this.props.snackbar.showError(`Во время сохранения произошла ошибка: ${error}`)
		})
	}

	saveAppointmentStartEnd = (id, start, end) => {
		this.props.loadingScreen.show()

		const newStart = new Date(start.getFullYear(), start.getMonth(), start.getDate(), start.getHours(), start.getMinutes(), start.getSeconds())
		const newEnd = new Date(start.getFullYear(), start.getMonth(), start.getDate(), end.getHours(), end.getMinutes(), end.getSeconds())

		postRequest(`${appConstants.serverUrl}/api/Appointments/SetAppointmentStartEnd`, {id, start: moment(newStart).format(), end: moment(newEnd).format()}, result => {
			// this.props.loadingScreen.hide()
			this.handleClose()
			if (result && !result.isSuccess) {
				this.props.snackbar.showError(`Во время сохранения произошла ошибка: ${result.msg}`)
			} else if (!result) {
				this.props.snackbar.showError('Во время сохранения ошибка')
			}
			this.getAppointments()
		},
		error => {
			this.props.loadingScreen.hide()
			this.handleClose()
			this.props.snackbar.showError(`Во время сохранения произошла ошибка: ${error}`)
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
			name,
			surname,
			middlename,
			phoneNumber,
		} = this.state
		console.log('updateEvent clickedEvent', clickedEvent)
		console.log('updateEvent events', events)
		const index = events.findIndex(event => event === clickedEvent)
		console.log('updateEvent index', index)
		const updatedEvent = events.slice()
		console.log('updateEvent updatedEvent', updatedEvent)
		updatedEvent[index].id = id
		updatedEvent[index].code = code || ''
		updatedEvent[index].toEmployee = toEmployee || mainSelectedEmployee
		updatedEvent[index].selectedProcedures = selectedProcedures || []
		updatedEvent[index].title = title || ''
		updatedEvent[index].complain = complain || ''
		updatedEvent[index].start = start
		updatedEvent[index].end = end
		updatedEvent[index].iin = iin || ''
		updatedEvent[index].documentNumber = documentNumber || ''
		updatedEvent[index].name = name || ''
		updatedEvent[index].surname = surname || ''
		updatedEvent[index].middlename = middlename || ''
		updatedEvent[index].phoneNumber = phoneNumber || ''

		const newStart = new Date(start.getFullYear(), start.getMonth(), start.getDate(), start.getHours(), start.getMinutes(), start.getSeconds())
		const newEnd = new Date(start.getFullYear(), start.getMonth(), start.getDate(), end.getHours(), end.getMinutes(), end.getSeconds())

		const newItem = {
			id: updatedEvent[index].id,
			code: updatedEvent[index].code,
			toEmployee: toEmployee || mainSelectedEmployee,
			selectedProcedures,
			title,
			complain,
			start: moment(newStart).format(),
			end: moment(newEnd).format(),
			iin,
			documentNumber,
			name,
			surname,
			middlename,
			phoneNumber,
		}
		this.setState({
			events: updatedEvent,
		})
		this.saveAppointment(newItem)
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

		this.props.loadingScreen.show()

		getRequest(`${appConstants.serverUrl}/api/Appointments/DeleteAppointment?id=${(this.state.id || 0)}`, result => {
			// this.props.loadingScreen.hide()
			this.handleClose()
			if (result && !result.isSuccess) {
				this.props.snackbar.showError(`Во время удаления произошла ошибка: ${result.msg}`)
			} else if (!result) {
				this.props.snackbar.showError('Во время удаления ошибка')
			}
			this.getAppointments()
		},
		error => {
			this.props.loadingScreen.hide()
			this.handleClose()
			this.props.snackbar.showError(`Во время удаления произошла ошибка: ${error}`)
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
		const {start, end} = this.state
		console.log('time', time)
		let check = moment(new Date().getHours() + ':' + new Date().getMinutes(), 'HH:mm')
		if (time) {
			check = moment(time.getHours() + ':' + time.getMinutes(), 'HH:mm')
			// check = new Date(start.getFullYear(), start.getMonth(), start.getDate(), time.getHours(), time.getMinutes(), time.getSeconds())
		}
		let newTime = null
		if (check.isBetween(minPickerTime, maxPickerTime)) {
			// newTime = time
			newTime = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), check.hours(), check.minutes())
			if (start) {
				newTime = new Date(start.getFullYear(), start.getMonth(), start.getDate(), check.hours(), check.minutes())
			}
		} else {
			newTime = new Date()
			if (start) {
				newTime = new Date(start.getFullYear(), start.getMonth(), start.getDate(), newTime.getHours(), newTime.getMinutes())
			}
		}
		console.log('newTime', newTime)

		this.setState({start: newTime})
		if (end && end < newTime) {
			this.setState({end: newTime})
		}
	}

	handleEndTime = time => {
		const {start} = this.state

		let check = moment(new Date().getHours() + ':' + new Date().getMinutes(), 'HH:mm')
		if (time) {
			check = moment(time.getHours() + ':' + time.getMinutes(), 'HH:mm')
			// check = new Date(start.getFullYear(), start.getMonth(), start.getDate(), time.getHours(), time.getMinutes(), time.getSeconds())
		}
		let newTime = null
		if (check && start && moment(start.getHours() + ':' + start.getMinutes(), 'HH:mm') > check) {
			newTime = start
			console.log('end time 1', newTime)
		} else if (check.isBetween(minPickerTime, maxPickerTime)) {
			// newTime = check //.setDate(time.getDate() + 1)
			newTime = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate(), check.hours(), check.minutes())
			if (start) {
				newTime = new Date(start.getFullYear(), start.getMonth(), start.getDate(), check.hours(), check.minutes())
			}
			console.log('end time 2', newTime)
		} else {
			newTime = new Date()
			if (start) {
				newTime = new Date(start.getFullYear(), start.getMonth(), start.getDate(), newTime.getHours(), newTime.getMinutes())
			}
			console.log('end time 3', newTime)
		}
		console.log('end newTime', newTime)
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
		window.clearTimeout(this.searchTimeout)
		this.setState({
			...this.state,
			[e]: v,
			// searchData: '',
		}, () => {
			this.getAppointments()
		})
	}

	handleDoctorAutocompleteChange = (e, v) => {
		window.clearTimeout(this.searchTimeout)
		this.setState({
			...this.state,
			[e]: v,
		})
	}

	handleAutocompleteInputChange = v => {
		window.clearTimeout(this.searchTimeout)
		this.setState({
			searchData: v,
		})
	}

	handleProceduresAutocompleteChange = (e, v) => {
		window.clearTimeout(this.proceduresSearchTimeout)
		this.setState({
			...this.state,
			[e]: v,
		})
	}

	handleProceduresAutocompleteInputChange = v => {
		window.clearTimeout(this.proceduresSearchTimeout)
		this.setState({
			proceduresSearchData: v,
		})
	}

	getPatientByDocumentNumber = docNum => {
		this.props.loadingScreen.show()

		getRequest(`${appConstants.serverUrl}/api/Patients/GetPatientByDocumentNumber?docNum=${docNum}`, result => {
			this.props.loadingScreen.hide()
			if (result && result.isSuccess && result.data) {
				this.setState({
					documentNumber: result.data.documentNumber,
					name: result.data.name,
					surname: result.data.surname,
					middlename: result.data.middlename,
					phoneNumber: result.data.phoneNumber,
				})
			} else {
				this.props.snackbar.showError('Во время получения данных произошла ошибка')
			}
		},
		error => {
			this.props.loadingScreen.hide()
			this.props.snackbar.showError(`Во время получения данных произошла ошибка: ${error}`)
		})
	}

	filterProceduresData = () => {
		const {proceduresSearchData} = this.state
		this.props.loadingScreen.show()

		getRequest(`${appConstants.serverUrl}/api/Dictionaries/GetDictServicesData?searchData=${proceduresSearchData}`, result => {
			this.props.loadingScreen.hide()
			if (result.isSuccess && Array.isArray(result.data)) {
				this.setState({
					proceduresOptions: [...result.data],
				})
			} else {
				this.props.snackbar.showError(`Во время получения списка произошла ошибка ${result.msg || ''}`)
			}
			window.clearTimeout(this.proceduresSearchTimeout)
		},
		error => {
			this.props.loadingScreen.hide()
			this.props.snackbar.showError(`Во время получения списка произошла ошибка: ${error}`)
			window.clearTimeout(this.proceduresSearchTimeout)
		})
	}

	filterUsersData = () => {
		const {searchData} = this.state
		this.props.loadingScreen.show()

		getRequest(`${appConstants.serverUrl}/api/Users/GetUsersBySearch?searchData=${searchData}&toAppointment=true`, result => {
			this.props.loadingScreen.hide()
			if (Array.isArray(result)) {
				this.setState({
					employeesOptions: [...result],
				})
			} else {
				this.props.snackbar.showError('Во время получения списка произошла ошибка')
			}
			window.clearTimeout(this.searchTimeout)
		},
		error => {
			this.props.loadingScreen.hide()
			this.props.snackbar.showError(`Во время получения списка произошла ошибка: ${error}`)
			window.clearTimeout(this.searchTimeout)
		})
	}

	render() {
		const currentUser = userServices.getCurrentUser()
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
			proceduresSearchData,
			events,
			openSlot,
			openEvent,
			title,
			complain,
			start,
			end,
			startDate,
			endDate,
			iin,
			documentNumber,
			name,
			surname,
			middlename,
			phoneNumber,
		} = this.state

		return (
			<div>
				<Grid container spacing={1} className={classes.container}>
					{
						currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
							? <Grid item xs={6}>
								<Paper className={classes.paper}>
									<Autocomplete
										name='mainSelectedEmployee'
										fullWidth
										size='small'
										value={mainSelectedEmployee}
										options={employeesOptions}
										onChange={(e, v) => { this.handleAutocompleteChange('mainSelectedEmployee', v) }}
										onInputChange={(e, v) => { this.handleAutocompleteInputChange(v) }}
										getOptionLabel={option => option.name}
										renderInput={params => <TextField {...params} autoComplete='off' value={searchData} label='Доктор' variant='outlined' />}
									/>
								</Paper>
							</Grid>
							: null
					}
					<Grid item container xs={12} className={classes.gridItem}>
						<Grid item xs={12}>
							<Paper className={classes.paper}>
								{
									currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
										? <DnDCalendar
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
										: <Calendar
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
								}

							</Paper>
						</Grid>
					</Grid>
				</Grid>

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
							{
								currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
									? <React.Fragment>
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
									</React.Fragment>
									: null
							}
							<Grid container spacing={1}>
								<Grid container item xs={6}>
									{
										currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
											? <Grid item xs={12}>
												<Paper className={classes.paper}>
													<Autocomplete
														name='toEmployee'
														fullWidth
														size='small'
														value={toEmployee}
														options={employeesOptions}
														groupBy={option => option.positionId}
														groupOptionLabel={option => option.positionName}
														className={classes.input}
														onChange={(e, v) => { this.handleDoctorAutocompleteChange('toEmployee', v) }}
														onInputChange={(e, v) => { this.handleAutocompleteInputChange(v) }}
														getOptionLabel={option => option.name}
														renderOption={option => <span>{option.name} ({option.positionName})</span>}
														renderInput={params => <TextField {...params} autoComplete='off' value={searchData} label='Доктор' variant='outlined' />}
													/>
												</Paper>
											</Grid>
											: null
									}
									<Grid item xs={12}>
										<Paper className={classes.paper}>
											<Autocomplete
												name='selectedProcedures'
												disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
												multiple
												fullWidth
												filterSelectedOptions
												size='small'
												value={selectedProcedures}
												options={proceduresOptions}
												className={classes.input}
												onChange={(e, v) => { this.handleProceduresAutocompleteChange('selectedProcedures', v) }}
												onInputChange={(e, v) => { this.handleProceduresAutocompleteInputChange(v) }}
												getOptionLabel={option => option.name}
												renderInput={params => <TextField {...params} autoComplete='off' value={proceduresSearchData} label='Процедуры' variant='outlined' />}
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
											disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
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
									(id <= 0 || !id) && currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) && <React.Fragment>
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
												</LocalizationProvider>
											</Paper>
										</Grid>
									</React.Fragment>
								}
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<LocalizationProvider dateAdapter={AdapterDateFns} locale={ruLocale}>
											<TimePicker
												margin='normal'
												inputVariant='outlined'
												variant='dialog'
												cancelLabel='Отменить'
												okLabel='Выбрать'
												fullWidth
												disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
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
										</LocalizationProvider>
									</Paper>
								</Grid>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<LocalizationProvider dateAdapter={AdapterDateFns} locale={ruLocale}>
											<TimePicker
												margin='normal'
												inputVariant='outlined'
												variant='dialog'
												cancelLabel='Отменить'
												okLabel='Выбрать'
												fullWidth
												disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
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
										</LocalizationProvider>
									</Paper>
								</Grid>
							</Grid>
							<Divider className={classes.divider} />
							<Grid container spacing={1}>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<TextField
											required
											error={!surname}
											name='surname'
											fullWidth
											disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
											size='small'
											autoComplete='off'
											value={surname}
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
											error={!name}
											name='name'
											fullWidth
											disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
											size='small'
											autoComplete='off'
											value={name}
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
											name='middlename'
											fullWidth
											disabled={!(currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2))}
											size='small'
											autoComplete='off'
											value={middlename}
											label='Отчество'
											variant='outlined'
											className={classes.input}
											inputProps={{'aria-label': 'Description'}}
											onChange={this.handleChange}/>
									</Paper>
								</Grid>
								{
									currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
										? <React.Fragment>
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
										</React.Fragment>
										: null
								}
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
							{currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) ? 'Отменить' : 'Закрыть'}
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
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
								? <Button
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
								: null
						}
					</DialogActions>
				</Dialog>
			</div>
		)
	}
}

export default compose(withSnackbar, loading, withStyles(styles))(Home)
