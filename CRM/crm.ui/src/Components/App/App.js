import React, {Component} from 'react'
import {connect} from 'react-redux'
import {Router, Route} from 'react-router-dom'
import {createBrowserHistory} from 'history'
import {withStyles} from '@material-ui/core/styles'
import useMediaQuery from '@material-ui/core/useMediaQuery'
import Toolbar from '@material-ui/core/Toolbar'
import useScrollTrigger from '@material-ui/core/useScrollTrigger'
import Fab from '@material-ui/core/Fab'
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp'
import Zoom from '@material-ui/core/Zoom'
import MenuBar from '../MenuBar'
import LogIn from '../LogIn'
import Home from '../Home'
// import AddCard from '../AddCard'
// import CardsList from '../CardsList'
// import VisitorsList from '../VisitorsList'
import {allActions} from '../../Actions/AllActions'
import {allConstants} from '../../Constants/AllConstants.js'
import {getRequest} from '../../Services/RequestsServices.js'
require('./App.css')

const history = createBrowserHistory({basename: 'ab-app'})

const styles = theme => ({
	scrollRoot: {
		position: 'fixed',
		zIndex: 1502,
		bottom: theme.spacing(4),
		right: theme.spacing(4),
	},
	toolbar: {
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'flex-end',
		padding: theme.spacing(0, 1),
		// necessary for content to be below app bar
		...theme.mixins.toolbar,
	},
	content: {
		flexGrow: 1,
		padding: theme.spacing(3),
	},
})

const WithMediaQuery = props => React.cloneElement(props.children, {isDesktop: useMediaQuery('(min-width:600px)')})

const ScrollTop = props => {
	const {children, classes} = props
	/* eslint-disable */
	const trigger = useScrollTrigger({
		target: window,
		disableHysteresis: true,
		threshold: 100,
	})
	/* eslint-enable */

	const handleClick = event => {
		const anchor = (event.target.ownerDocument || document).querySelector('#back-to-top-anchor')

		if (anchor) {
			anchor.scrollIntoView({behavior: 'smooth', block: 'center'})
		}
	}

	return (
		<Zoom in={trigger}>
			<div onClick={handleClick} role='presentation' className={classes.scrollRoot}>
				{children}
			</div>
		</Zoom>
	)
}

class App extends Component {

	constructor(props) {
		super(props)
		this.state = {
			isAuthorized: false,
		}
	}

	componentDidMount() {
		if (!localStorage.getItem('crmToken')) {
			localStorage.clear()
		} else {
			this.logInSuccess()
		}
	}

	logInSuccess = () => {
		this.setState({
			isAuthorized: true,
		}, () => {
			const {dispatch, currentUser, token} = this.props

			if (!token && localStorage.getItem('crmToken')) {
				dispatch(allActions.addToken(localStorage.getItem('crmToken')))
			}

			if ((!currentUser || !currentUser.id) && !localStorage.getItem('currentUser')) {
				this.getCurrentUser()
			} else if ((!currentUser || !currentUser.id) && localStorage.getItem('currentUser')) {
				dispatch(allActions.addCurrentUser(JSON.parse(localStorage.getItem('currentUser'))))
			}
		})
	}

	getCurrentUser = () => {
		const {dispatch, token} = this.props

		getRequest(`${allConstants.serverUrl}/api/Users/GetCurrentUserData`, token, result => {
			dispatch(allActions.addCurrentUser(result))
		},
		error => {
			console.log(error)
		})
	}

	render() {
		const {classes, currentUser, token} = this.props
		const {isAuthorized} = this.state

		return (
			<div className='App'>
				<MenuBar isAuthorized={isAuthorized} currentUser={currentUser}/>
				<main className={classes.content}>
					<Toolbar id='back-to-top-anchor'/>
					{isAuthorized
						? <Router history={history}>
							<Route exact path='/' component={Home} />
							{/* <Route path='/add-card' component={() => <WithMediaQuery><AddCard currentUser={currentUser} token={token} /></WithMediaQuery>} />
							<Route path='/cards-list' component={() => <WithMediaQuery><CardsList token={token} /></WithMediaQuery>} />
							<Route path='/agreement-cards-list' component={() => <WithMediaQuery><CardsList token={token} toAgreement={true} /></WithMediaQuery>} />
							<Route path='/visitors-list' component={() => <WithMediaQuery><VisitorsList token={token} /></WithMediaQuery>} /> */}
						</Router>
						: <LogIn logInSuccess={this.logInSuccess}/>
					}
					<ScrollTop {...this.props}>
						<Fab color='secondary' size='small' aria-label='scroll back to top'>
							<KeyboardArrowUpIcon />
						</Fab>
					</ScrollTop>
				</main>
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

export default connect(mapStateToProps)(withStyles(styles, {withTheme: true})(App))
