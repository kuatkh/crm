import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import TextField from '@material-ui/core/TextField'
import Dialog from '@material-ui/core/Dialog'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Autocomplete from '@material-ui/lab/Autocomplete'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Typography from '@material-ui/core/Typography'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import DateFnsUtils from '@date-io/date-fns'
import ruLocale from 'date-fns/locale/ru'
import {MuiPickersUtilsProvider, KeyboardDatePicker} from '@material-ui/pickers'
import CardVisitors from './CardVisitors'
import CrmTable from '../CrmTable'
import {cardsTableColumns} from '../../Constants/TableColumns.js'
import {allConstants} from '../../Constants/AllConstants.js'
import {getRequest} from '../../Services/RequestsServices.js'

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

class CardsList extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			beginDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/')),
			endDate: new Date(new Date().toJSON().slice(0,10).replace(/-/g,'/')),
			cardsStatusOptions: [
				{id: 0, nameRu: 'Все'},
			],
			cardsStatus: {id: 0, nameRu: 'Все'},
			openCardVisitorsDialog: false,
			cardVisitorsData: null,
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			canAgreeSelectedCard: false,
			selectedCardId: null,
			selectedCardStatusId: null,
		}
	}

	componentDidMount() {
		const {token, toAgreement} = this.props
		const {cardsStatusOptions} = this.state
		this.isLoaded(false)

		getRequest(`${allConstants.serverUrl}/api/Cards/GetCardStatuses?isAgreement=${(toAgreement == true)}`, token, result => {
			this.isLoaded(true)
			if (result && result.isSuccess) {
				this.setState({
					cardsStatusOptions: [...cardsStatusOptions, ...result.data],
				})
			} else {
				this.handleSnackbarOpen(`Во время получения состояний пропусков произошла ошибка: ${(result && result.msg ? result.msg : '')}`, 'error')
			}
		},
		error => {
			this.isLoaded(true)
			this.handleSnackbarOpen(`Во время получения состояний пропусков произошла ошибка: ${error}`, 'error')
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

handleCardVisitorsDialogOpen = () => {
	this.setState({
		openCardVisitorsDialog: true,
	})
}

handleCardVisitorsDialogClose = () => {
	this.setState({
		openCardVisitorsDialog: false,
		selectedCardId: null,
		selectedCardStatusId: null,
		cardVisitorsData: null,
		canAgreeSelectedCard: false,
	})
}

getStyles = (name, selected, theme) => ({
	fontWeight:
		Array.isArray(selected) && selected.indexOf(name) === -1 || !Array.isArray(selected) && selected == name
			? theme.typography.fontWeightRegular
			: theme.typography.fontWeightMedium,
})

handleLaunchClick = row => {
	this.setState({
		openCardVisitorsDialog: true,
		selectedCardId: row.id,
		selectedCardStatusId: row.cardsStatusId,
		canAgreeSelectedCard: row.canAgree == true,
	})
}

handleDeleteClick = id => {
	this.setState({
		// loading: true,
		selectedCardId: id,
	})
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
	})
}

handleAgreementActionComplete = () => {
	this.setState({
		openCardVisitorsDialog: false,
		selectedCardId: null,
		cardVisitorsData: null,
		canAgreeSelectedCard: false,
		cardsStatus: {id: 1, nameRu: 'Все'},
	})
}

render() {
	const {classes, toAgreement, token, isDesktop} = this.props
	const {
		openSnackbar,
		snackbarMsg,
		snackbarSeverity,
		loading,
		openCardVisitorsDialog,
		selectedCardId,
		selectedCardStatusId,
		cardsStatus,
		cardsStatusOptions,
		beginDate,
		endDate,
	} = this.state
	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>{toAgreement == true ? 'Пропуска на согласование' : 'Мои пропуска'}</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
					<Grid item xs={isDesktop ? 6 : 12}>
						<Paper className={classes.paper}>
							<Autocomplete
								id='cardsStatus'
								name='cardsStatus'
								disableClearable={true}
								value={cardsStatus}
								options={cardsStatusOptions}
								fullWidth
								onChange={(e, v) => { this.handleAutocompleteChange('cardsStatus', v) }}
								getOptionLabel={option => option.nameRu}
								renderInput={params => <TextField {...params} label='Состояние пропуска' variant='outlined' />}
							/>
						</Paper>
					</Grid>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardDatePicker
								inputVariant='outlined'
								variant='dialog'
								cancelLabel='Отменить'
								okLabel='Выбрать'
								fullWidth
								label='Дата создания пропуска с'
								format='dd.MM.yyyy'
								value={beginDate}
								onChange={this.handleBeginDateChange}
								invalidDateMessage='Неверный формат даты'
								KeyboardButtonProps={{'aria-label': 'change date'}}/>
						</Paper>
					</Grid>
					<Grid item xs={isDesktop ? 3 : 12}>
						<Paper className={classes.paper}>
							<KeyboardDatePicker
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
				</MuiPickersUtilsProvider>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<CrmTable
							url={`${allConstants.serverUrl}/api/Cards/GetCards`}
							filterData={{
								statusType: cardsStatus.id,
								beginDate,
								endDate,
								isAgreement: toAgreement === true,
							}}
							token={token}
							columns={cardsTableColumns}
							isLoaded={this.isLoaded}
							tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
							handleSnackbarOpen={this.handleSnackbarOpen}
							handleLaunchClick={this.handleLaunchClick}
							handleDeleteClick={this.handleDeleteClick}
							canOpen={true}
							// canDelete={true}
						/>
					</Paper>
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
			<div>
				<Dialog
					open={openCardVisitorsDialog}
					onClose={this.handleCardVisitorsDialogClose}
					scroll={'paper'}
					fullWidth={true}
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<CardVisitors
						handleVisitorDialogClose={this.handleCardVisitorsDialogClose}
						handleSnackbarOpen={this.handleSnackbarOpen}
						isLoaded={this.isLoaded}
						toAgreement={toAgreement}
						token={token}
						handleAgreementActionComplete={this.handleAgreementActionComplete}
						selectedCardStatusId={selectedCardStatusId}
						selectedCardId={selectedCardId} />
				</Dialog>
			</div>
		</div>
	)
}
}

export default withStyles(styles, {withTheme: true})(CardsList)
