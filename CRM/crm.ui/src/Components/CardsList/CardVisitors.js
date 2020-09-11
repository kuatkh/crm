import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import CancelIcon from '@material-ui/icons/Cancel'
import CheckCircleIcon from '@material-ui/icons/CheckCircle'
import RemoveCircleIcon from '@material-ui/icons/RemoveCircle'
import Paper from '@material-ui/core/Paper'
import Grid from '@material-ui/core/Grid'
import CrmTable from '../CrmTable'
import {visitorsTableColumns} from '../../Constants/TableColumns.js'
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
		zIndex: theme.zIndex.drawer + 1,
		color: '#fff',
	},
	headerStyle: {
		color: '#fff !important',
		backgroundColor: '#3f51b5 !important',
	},
})

class CardVisitors extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			openJustificationDialog: false,
			justification: '',
		}
	}

handleCloseClick = () => {
	if (this.props.handleVisitorDialogClose) {
		this.props.handleVisitorDialogClose()
	}
}

handleAgreeClick = () => {
	const {selectedCardId, token, isLoaded, handleSnackbarOpen, handleAgreementActionComplete} = this.props
	if (selectedCardId && selectedCardId > 0) {
		if (isLoaded) {
			isLoaded(false)
		}
		getRequest(`${allConstants.serverUrl}/api/Cards/AgreeCard?cardId=${selectedCardId}`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			this.handleJustificationDialogClose()
			if (result && result.isSuccess) {
				if (handleSnackbarOpen) {
					handleSnackbarOpen('Пропуск успешно согласован!', 'success')
				}
				if (handleAgreementActionComplete) {
					handleAgreementActionComplete()
				}
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время согласования пропуска произошла ошибка: ${(result && result.msg ? result.msg : '')}`, 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время согласования пропуска произошла ошибка: ${error}`, 'error')
			}
		})
	} else if (handleSnackbarOpen) {
		handleSnackbarOpen('Номер пропуска не может быть меньше или равен нулю', 'error')
	}
}

handleRejectClick = () => {
	const {selectedCardId, token, isLoaded, handleSnackbarOpen, handleAgreementActionComplete} = this.props
	const {justification} = this.state
	if (selectedCardId && selectedCardId > 0) {
		if (isLoaded) {
			isLoaded(false)
		}
		postRequest(`${allConstants.serverUrl}/api/Cards/RejectCard`, token, {cardId: selectedCardId, justification}, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			this.handleJustificationDialogClose()
			if (result && result.isSuccess) {
				if (handleSnackbarOpen) {
					handleSnackbarOpen('Пропуск успешно отклонен!', 'success')
				}
				if (handleAgreementActionComplete) {
					handleAgreementActionComplete()
				}
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время отклонения пропуска произошла ошибка: ${(result && result.msg ? result.msg : '')}`, 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время отклонения пропуска произошла ошибка: ${error}`, 'error')
			}
		})
	} else if (handleSnackbarOpen) {
		handleSnackbarOpen('Номер пропуска не может быть меньше или равен нулю', 'error')
	}
}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleJustificationDialogOpen = () => {
	this.setState({
		openJustificationDialog: true,
	})
}

handleJustificationDialogClose = rejectWithJustification => {
	if (rejectWithJustification === true) {
		if (this.state.justification) {
			this.handleRejectClick()
		} else if (this.props.handleSnackbarOpen) {
			this.props.handleSnackbarOpen('Вы не заполнили причину', 'error')
		}
	} else {
		this.setState({
			openJustificationDialog: false,
		})
	}
}

render() {
	const {classes, isLoaded, handleSnackbarOpen, selectedCardId, selectedCardStatusId, toAgreement, token} = this.props

	return (
		<React.Fragment>
			<DialogTitle className={classes.headerStyle} id='add-visitor-dialog-title'>Данные посетителей</DialogTitle>
			<DialogContent dividers={true}>
				<div className={classes.modalRoot}>
					<Grid container className={classes.container}>
						<Grid item xs={12}>
							<Paper className={classes.paper}>
								<CrmTable
									url={`${allConstants.serverUrl}/api/Visitors/GetVisitors`}
									columns={visitorsTableColumns}
									filterData={{cardId: selectedCardId}}
									isLoaded={isLoaded}
									token={token}
									toAgreement={toAgreement}
									tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
									handleSnackbarOpen={handleSnackbarOpen}
									// canDelete={true}
								/>
							</Paper>
						</Grid>
					</Grid>
				</div>
			</DialogContent>
			<DialogActions>
				{
					toAgreement === true && selectedCardStatusId == 2 && (
						<React.Fragment>
							<Button onClick={this.handleAgreeClick} startIcon={<CheckCircleIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='primary'>
								Согласовать
							</Button>
							<Button onClick={this.handleJustificationDialogOpen} startIcon={<RemoveCircleIcon />} className={classes.actionButtons} variant='outlined' size='medium'>
								Отклонить
							</Button>
							<div>
								<Dialog
									open={this.state.openJustificationDialog}
									onClose={this.handleJustificationDialogClose}
									scroll={'paper'}
									fullWidth={true}
									maxWidth={'lg'}
									aria-labelledby='scroll-dialog-title'
									aria-describedby='scroll-dialog-description'
								>
									<DialogTitle className={classes.headerStyle} id='justification-dialog-title'>Причина отклонения</DialogTitle>
									<DialogContent dividers={true}>
										<div className={classes.modalRoot}>
											<Grid container className={classes.container}>
												<Grid item xs={12}>
													<Paper className={classes.paper}>
														<TextField
															name='justification'
															multiline
															rows={5}
															fullWidth={true}
															size='small'
															value={this.state.justification}
															label='Обоснование'
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
										<Button onClick={() => this.handleJustificationDialogClose(true)} startIcon={<RemoveCircleIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='primary'>
											Отклонить
										</Button>
										<Button onClick={this.handleJustificationDialogClose} startIcon={<CancelIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='secondary'>
											Закрыть
										</Button>
									</DialogActions>
								</Dialog>
							</div>
						</React.Fragment>
					)
				}
				<Button onClick={this.handleCloseClick} startIcon={<CancelIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='secondary'>
					Закрыть
				</Button>
			</DialogActions>
		</React.Fragment>
	)
}
}

export default withStyles(styles, {withTheme: true})(CardVisitors)
