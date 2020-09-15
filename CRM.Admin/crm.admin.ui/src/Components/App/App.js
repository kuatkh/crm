import React, {Component} from 'react'
import {withStyles} from '@material-ui/core/styles'
import {connect} from 'react-redux'
import {Router, Route} from 'react-router-dom'
import {createBrowserHistory} from 'history'
import MenuBar from '../MenuBar'
import LogIn from '../LogIn'
import Home from '../Home'
import Users from '../Users'
import Dictionaries from '../Dictionaries'
import {allActions} from '../../Actions/AllActions'
import {allConstants} from '../../Constants/AllConstants.js'
import {getRequest} from '../../Services/RequestsServices.js'
require('./App.css')

const history = createBrowserHistory({basename: 'ab-app'})

const styles = theme => ({
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

class App extends Component {

	constructor(props) {
		super(props)
		this.state = {
			isAuthorized: false,
		}
	}

	componentDidMount() {
		if (!localStorage.getItem('abToken')) {
			localStorage.clear()
		} else {
			this.logInSuccess()
		}
	}

	logInSuccess = () => {
		this.setState({
			isAuthorized: true,
		})
		const {dispatch, currentUser, token} = this.props

		if (!token && localStorage.getItem('abToken')) {
			dispatch(allActions.addToken(localStorage.getItem('abToken')))
		}

		if ((!currentUser || !currentUser.Id) && !localStorage.getItem('currentUser')) {
			this.getCurrentUser()
		} else if ((!currentUser || !currentUser.Id) && localStorage.getItem('currentUser')) {
			dispatch(allActions.addCurrentUser(JSON.parse(localStorage.getItem('currentUser'))))
		}
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
		let {isAuthorized} = this.state
		isAuthorized = true
		return (
			<div className='App'>
				<MenuBar isAuthorized={isAuthorized} currentUser={currentUser}/>
				<main className={classes.content}>
					<div className={classes.toolbar} />
					{isAuthorized
						? <Router history={history}>
							<Route exact path='/' component={Home} />
							<Route path='/users-list'
								component={Users} />
							<Route path='/dictionary-contries'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictCountries' pageTitle='Справочник стран' />} />
							<Route path='/dictionary-cities'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictCities' pageTitle='Справочник городов' />} />
							<Route path='/dictionary-departments'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictDepartments' pageTitle='Справочник структурных подразделений' />} />
							<Route path='/dictionary-positions'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictPositions' pageTitle='Справочник должностей' />} />
							<Route path='/dictionary-services'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictServices' pageTitle='Спарвочник предоставляемых услуг' />} />
							<Route path='/dictionary-intolerances'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictIntolerances' pageTitle='Справочник аллергический заболеваний' />} />
							<Route path='/dictionary-genders'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictGenders' pageTitle='Справочник пола человека' />} />
							<Route path='/dictionary-loyalty-programs'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictLoyaltyPrograms' pageTitle='Справочник бонусных программ' />} />
							<Route path='/dictionary-statuses'
								component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictStatuses' pageTitle='Справочник статусов' />} />
							{
								currentUser && currentUser.roleId == 1 && (
									<Route path='/dictionary-enterprises'
										component={() => <Dictionaries currentUser={currentUser} token={token} dictionaryName='DictEnterprises' pageTitle='Справочник компаний/филиалов' />} />
								)
							}
						</Router>
						: <LogIn logInSuccess={this.logInSuccess}/>
					}
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
