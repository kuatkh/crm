import React, {Component} from 'react'
import {withStyles} from '@material-ui/core/styles'
import Alert from '@material-ui/lab/Alert'
import {Button, TextField, Grid, Paper, Typography, Link, CircularProgress, Snackbar, Backdrop} from '@material-ui/core'
import InputAdornment from '@material-ui/core/InputAdornment'
import IconButton from '@material-ui/core/IconButton'
import Visibility from '@material-ui/icons/Visibility'
import VisibilityOff from '@material-ui/icons/VisibilityOff'
import {allConstants} from '../../Constants/AllConstants.js'

const styles = theme => ({
	toolbar: {
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'flex-end',
		padding: theme.spacing(0, 1),
		...theme.mixins.toolbar,
	},
	content: {
		flexGrow: 1,
		padding: theme.spacing(3),
	},
	backdrop: {
		zIndex: theme.zIndex.drawer + 3,
		color: '#fff',
	},
})

class LogIn extends Component {

	constructor(props) {
		super(props)
		this.state = {
			userName: '',
			userSecret: '',
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
			showUserSecret: false,
		}
		this.userSecretNumberRegexStr = '^[0-9]*$'
		this.userSecretRegexStr = '^(?:[A-Za-z]+|\d+)$'
	}

	componentDidMount() {
		if (!localStorage.getItem('abToken')) {
			localStorage.clear()
		}
	}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleSubmit = e => {
	e.preventDefault()
	const {userName, userSecret} = this.state
	const {logInSuccess} = this.props
	if (userName && userSecret) {
		this.isLoaded(false)
		fetch(`${allConstants.serverUrl}/api/Auth/LogIn`, {
			method: 'POST',
			headers: {
				...allConstants.requestHeaders,
			},
			body: JSON.stringify({userName, userSecret}),
		})
			.then(res => res.json())
			.then(
				result => {
					this.isLoaded(true)
					if (result.isSuccess && result.data) {
						this.handleSnackbarOpen('Добро пожаловать в CRM!', 'success')
						localStorage.setItem('abToken', result.data)
						if (logInSuccess) {
							logInSuccess()
						}
					} else if (!result.isSuccess && result.data == 'Invalid_username_or_password') {
						this.handleSnackbarOpen('Неверный логин или пароль!', 'error')
					} else {
						this.handleSnackbarOpen('Во время авторизации произошла ошибка!', 'error')
					}
				},
				error => {
					console.log(error)
				}
			)
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
	const userSecretNumberRegEx = new RegExp(this.userSecretNumberRegexStr)
	/* eslint-disable */
	const replaced = userSecretKey.replace(/[^A-Za-z]/gi, "")
	if (replaced || userSecretNumberRegEx.test(userSecretCh) || event.keyCode > 95 && event.keyCode < 106) {
		return
	} else {
		event.preventDefault()
	}
	/* eslint-enable */
}

handleClickShowUserSecret = () => {
	this.setState({
		showUserSecret: !this.state.showUserSecret,
	})
}

handleMouseDownUserSecret = () => {
	this.setState({
		showUserSecret: !this.state.showUserSecret,
	})
}

render() {
	const {classes} = this.props
	return (
		<div className='LogIn'>
			<Grid container direction='column' justify='center' spacing={2} className='login-form' >
				<Paper variant='elevation' elevation={2} className='login-background' >
					<Grid item>
						<Typography component='h1' variant='h5'>Авторизация</Typography>
					</Grid>
					<Grid item>
						<form onSubmit={this.handleSubmit}>
							<Grid container direction='column' spacing={2}>
								<Grid item>
									<TextField
										type='userName'
										placeholder='Email'
										fullWidth
										autoComplete='off'
										name='userName'
										variant='outlined'
										value={this.state.userName}
										onChange={this.handleChange}
										required
										autoFocus />
								</Grid>
								<Grid item>
									<TextField
										placeholder='Пароль'
										type={this.state.showUserSecret ? 'text' : 'password'}
										fullWidth
										error={(!this.state.userSecret || this.state.userSecret && this.state.userSecret.length < 6)}
										name='userSecret'
										autoComplete='off'
										variant='outlined'
										value={this.state.userSecret}
										onChange={this.handleChange}
										required
										InputProps={{
											'aria-label': 'Description',
											onKeyDown: this.handleUserSecretKeydown,
											endAdornment: <InputAdornment position='end'>
												<IconButton
													aria-label='Показать пароль'
													onClick={this.handleClickShowUserSecret}
													onMouseDown={this.handleMouseDownUserSecret}
												>{this.state.showUserSecret ? <Visibility /> : <VisibilityOff />}</IconButton>
											</InputAdornment>,
										}} />
								</Grid>
								<Grid item>
									<Button variant='contained' color='primary' type='submit' className='button-block' >Войти</Button>
								</Grid>
							</Grid>
						</form>
					</Grid>
					{/* <Grid item>
						<Link href='#' variant='body2'>Забыли пароль?</Link>
					</Grid> */}
				</Paper>
			</Grid>
			<Snackbar open={this.state.openSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
				<Alert onClose={this.handleSnackbarClose} severity={this.state.snackbarSeverity}>
					{this.state.snackbarMsg}
				</Alert>
			</Snackbar>
			<Backdrop className={classes.backdrop} open={this.state.loading}>
				<CircularProgress color='inherit' />
			</Backdrop>
		</div>
	)
}
}

export default withStyles(styles, {withTheme: true})(LogIn)
