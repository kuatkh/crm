import React, {Component, useState} from 'react'
import {withStyles} from '@mui/styles'
import {connect} from 'react-redux'
import {
	BrowserRouter,
	Routes,
	Route,
	Navigate,
} from 'react-router-dom'
import {createBrowserHistory} from 'history'
import MenuBar from 'components/MenuBar'
import LogIn from 'components/LogIn'
import Home from 'components/Home'
import Users from 'components/Users'
import Profile from 'components/Profile'
import Dictionaries from 'components/Dictionaries'
import {tokenServices} from 'services/token.services'
import {userServices} from 'services/user.services'
require('./App.css')

const history = createBrowserHistory({basename: 'ab-app'})

const styles = theme => ({
	toolbar: {
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'flex-end',
		padding: theme.spacing(1),
		// necessary for content to be below app bar
		...theme.mixins.toolbar,
	},
	content: {
		flexGrow: 1,
		padding: theme.spacing(3),
	},
})

const App = props => {
	const [isAuthorized, setIsAuthorized] = useState(true) //tokenServices.getToken() ? true : false
	const currentUser = userServices.getCurrentUser()

	return (
		<div className='App'>
			<MenuBar isAuthorized={isAuthorized} setIsAuthorized={setIsAuthorized} />
			<main className={props.classes.content}>
				<div className={props.classes.toolbar} />
				<BrowserRouter>
				{isAuthorized
					? <Routes>
						<Route exact path='/'
							component={() => <Home />} />
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
								? <React.Fragment>
									<Route path='/users-list'
										component={Users} />
									<Route path='/profile'
										component={Profile} />
									<Route path='/dictionary-services'
										component={() => <Dictionaries dictionaryName='DictServices' pageTitle='Спарвочник предоставляемых услуг' />} />
									<Route path='/dictionary-intolerances'
										component={() => <Dictionaries dictionaryName='DictIntolerances' pageTitle='Справочник аллергический заболеваний' />} />
									<Route path='/dictionary-genders'
										component={() => <Dictionaries dictionaryName='DictGenders' pageTitle='Справочник пола человека' />} />
									<Route path='/dictionary-loyalty-programs'
										component={() => <Dictionaries dictionaryName='DictLoyaltyPrograms' pageTitle='Справочник бонусных программ' />} />
									{
										currentUser && currentUser.roleId == 1 && (
											<React.Fragment>
												<Route path='/dictionary-contries'
													component={() => <Dictionaries dictionaryName='DictCountries' pageTitle='Справочник стран' />} />
												<Route path='/dictionary-cities'
													component={() => <Dictionaries dictionaryName='DictCities' pageTitle='Справочник городов' />} />
												<Route path='/dictionary-departments'
													component={() => <Dictionaries dictionaryName='DictDepartments' pageTitle='Справочник структурных подразделений' />} />
												<Route path='/dictionary-positions'
													component={() => <Dictionaries dictionaryName='DictPositions' pageTitle='Справочник должностей' />} />
												<Route path='/dictionary-enterprises'
													component={() => <Dictionaries dictionaryName='DictEnterprises' pageTitle='Справочник компаний/филиалов' />} />
												<Route path='/dictionary-statuses'
													component={() => <Dictionaries dictionaryName='DictStatuses' pageTitle='Справочник статусов' />} />
											</React.Fragment>
										)
									}
								</React.Fragment>
								: null
						}
					</Routes>
					: <LogIn logInSuccess={setIsAuthorized}/>
				}
				</BrowserRouter>
			</main>
		</div>
	)
}

export default withStyles(styles, {withTheme: true})(App)
