import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import TextField from '@material-ui/core/TextField'
import Button from '@material-ui/core/Button'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import CardMedia from '@material-ui/core/CardMedia'
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
	media: {
		margin: 'auto',
		maxWidth: '450px',
	},
})

class AddVisitor extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			id: 0,
			iin: '',
			documentNumber: '',
			additionalDocumentNumber: '',
			citizenship: '',
			nameRu: '',
			surnameRu: '',
			middlenameRu: '',
			jobPlaceName: '',
			positionName: '',
			photoB64: null,
		}

		this.regexStr = '^[0-9]*$'
	}

	componentDidMount() {
		if (this.props.editVisitorData) {
			this.setState({...this.props.editVisitorData})
		} else {
			this.setState({
				id: 0,
				iin: '',
				documentNumber: '',
				additionalDocumentNumber: '',
				citizenship: '',
				nameRu: '',
				surnameRu: '',
				middlenameRu: '',
				jobPlaceName: '',
				positionName: '',
				photoB64: null,
			})
		}
	}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleFindByIinClick = () => {
	const {iin} = this.state
	const {token, isLoaded, handleSnackbarOpen} = this.props
	if (iin && iin.length == 12) {
		if (isLoaded) {
			isLoaded(false)
		}
		getRequest(`${allConstants.serverUrl}/api/Visitors/GetVisitorByIin?iin=${iin}`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (result && result.isSuccess && result.data) {
				this.setState({
					iin,
					documentNumber: result.data.documentNumber || '',
					additionalDocumentNumber: result.data.additionalDocumentNumber || '',
					citizenship: result.data.citizenship || '',
					nameRu: result.data.nameRu || '',
					surnameRu: result.data.surnameRu || '',
					middlenameRu: result.data.middlenameRu || '',
					jobPlaceName: result.data.jobPlaceName || '',
					positionName: result.data.positionName || '',
					photoB64: result.data.photoB64 || null,
				})
			} else if (result && result.isSuccess && !result.data && handleSnackbarOpen) {
				handleSnackbarOpen('Посетитель не найден. Пожалуйста, введите данные вручную', 'error')
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время поиска посетителя по ИИН произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время поиска посетителя по ИИН произошла ошибка: ${error}`, 'error')
			}
		})
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

handleSaveClick = () => {
	if (this.state.iin && this.state.iin.length < 12) {
		if (this.props.handleSnackbarOpen) {
			this.props.handleSnackbarOpen('ИИН должен состоять из 12 цифр!', 'error') // Вы ввели ${this.state.Iin.length} цифр
		}
		return
	}
	if (this.state.documentNumber && this.state.documentNumber.length < 4) {
		if (this.props.handleSnackbarOpen) {
			this.props.handleSnackbarOpen('Номер документа должен содержать минимум 3 символа', 'error')
		}
		return
	}
	if (!this.state.surnameRu) {
		if (this.props.handleSnackbarOpen) {
			this.props.handleSnackbarOpen('Вы не заполнили поле "Фамилия"', 'error')
		}
		return
	}
	if (!this.state.nameRu) {
		if (this.props.handleSnackbarOpen) {
			this.props.handleSnackbarOpen('Вы не заполнили поле "Имя"', 'error')
		}
		return
	}
	if ((this.state.iin && this.state.iin.length == 12 || this.state.documentNumber && this.state.documentNumber.length > 3) && this.props.handleVisitorDialogClose) {
		this.props.handleVisitorDialogClose({...this.state})
	} else if (this.props.handleSnackbarOpen) {
		this.props.handleSnackbarOpen('Номер документа либо ИИН заполнен не правильно', 'error')
	}
}

handleCancelClick = () => {
	this.setState({
		id: 0,
		iin: '',
		documentNumber: '',
		additionalDocumentNumber: '',
		citizenship: '',
		nameRu: '',
		surnameRu: '',
		middlenameRu: '',
		jobPlaceName: '',
		positionName: '',
		photoB64: null,
	})
	if (this.props.handleVisitorDialogClose) {
		this.props.handleVisitorDialogClose()
	}
}

render() {
	const {classes} = this.props
	return (
		<React.Fragment>
			<DialogTitle className={classes.headerStyle} id='add-visitor-dialog-title'>Данные посетителя</DialogTitle>
			<DialogContent dividers={true}>
				<div className={classes.modalRoot}>
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='iin'
									error={(this.state.iin && this.state.iin.length < 12 || !this.state.iin && !this.state.documentNumber)}
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
						<Grid item xs={5}>
							<Paper className={classes.paper}>
								<Button variant='outlined' color='primary' size='large' className={classes.button} onClick={this.handleFindByIinClick}>
									Поиск
								</Button>
							</Paper>
						</Grid>
					</Grid>
					{/* <Divider className={classes.divider} /> */}
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									error={!this.state.surnameRu}
									name='surnameRu'
									fullWidth={true}
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
									error={!this.state.nameRu}
									name='nameRu'
									fullWidth={true}
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
									fullWidth={true}
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
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									required
									error={(this.state.documentNumber && this.state.documentNumber.length < 4 || !this.state.iin && !this.state.documentNumber)}
									name='documentNumber'
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.documentNumber}
									label='Номер документа'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description', onKeyDown: this.handleDocumentNumberKeydown}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='additionalDocumentNumber'
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.additionalDocumentNumber}
									label='Доп. номер документа'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description', onKeyDown: this.handleDocumentNumberKeydown}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='citizenship'
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.citizenship}
									label='Гражданство'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
					<Divider className={classes.divider} />
					<Grid container spacing={1}>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='jobPlaceName'
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.jobPlaceName}
									label='Место работы'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
						<Grid item xs={6}>
							<Paper className={classes.paper}>
								<TextField
									name='positionName'
									fullWidth={true}
									size='small'
									autoComplete='off'
									value={this.state.positionName}
									label='Должность'
									variant='outlined'
									className={classes.input}
									inputProps={{'aria-label': 'Description'}}
									onChange={this.handleChange}/>
							</Paper>
						</Grid>
					</Grid>
					{
						this.state.photoB64 && <React.Fragment>
							<Divider className={classes.divider} />
							<Grid container spacing={1}>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
										<CardMedia className={classes.media} src={`data:image/jpeg;base64,${this.state.photoB64}`} title='Фото посетителя' />
									</Paper>
								</Grid>
								<Grid item xs={6}>
									<Paper className={classes.paper}>
									</Paper>
								</Grid>
							</Grid>
						</React.Fragment>
					}
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

export default withStyles(styles, {withTheme: true})(AddVisitor)
