import React, {Component, useState} from 'react'
import {withStyles} from '@mui/styles'
import {
	Container,
} from '@mui/material'
import {
	BrowserRouter,
	Routes,
	Route,
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
	const userData = userServices.getCurrentUser()
	const currentUser = userData || {roleId: 1}

	return (
		<Container component='main' sx={{minHeight: '90vh', minWidth: '80vw'}}>
			<BrowserRouter>
				<MenuBar isAuthorized={isAuthorized} setIsAuthorized={setIsAuthorized} />
				{isAuthorized
					? <Routes>
						<Route exact path='/' element={<Home />} />
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
								? <React.Fragment>
									<Route path='/users-list' element={<Users />} />
									<Route path='/profile' element={<Profile />} />
									<Route path='/dictionary-services'
										element={<Dictionaries dictionaryName='DictServices' pageTitle='Спарвочник предоставляемых услуг' />} />
									<Route path='/dictionary-intolerances'
										element={<Dictionaries dictionaryName='DictIntolerances' pageTitle='Справочник аллергический заболеваний' />} />
									<Route path='/dictionary-genders'
										element={<Dictionaries dictionaryName='DictGenders' pageTitle='Справочник пола человека' />} />
									<Route path='/dictionary-loyalty-programs'
										element={<Dictionaries dictionaryName='DictLoyaltyPrograms' pageTitle='Справочник бонусных программ' />} />
									{
										currentUser && currentUser.roleId == 1 && (
											<React.Fragment>
												<Route path='/dictionary-contries'
													element={<Dictionaries dictionaryName='DictCountries' pageTitle='Справочник стран' />} />
												<Route path='/dictionary-cities'
													element={<Dictionaries dictionaryName='DictCities' pageTitle='Справочник городов' />} />
												<Route path='/dictionary-departments'
													element={<Dictionaries dictionaryName='DictDepartments' pageTitle='Справочник структурных подразделений' />} />
												<Route path='/dictionary-positions'
													element={<Dictionaries dictionaryName='DictPositions' pageTitle='Справочник должностей' />} />
												<Route path='/dictionary-enterprises'
													element={<Dictionaries dictionaryName='DictEnterprises' pageTitle='Справочник компаний/филиалов' />} />
												<Route path='/dictionary-statuses'
													element={<Dictionaries dictionaryName='DictStatuses' pageTitle='Справочник статусов' />} />
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
			{/* <main className={props.classes.content}>
				<div className={props.classes.toolbar} />
			</main> */}
		</Container>
	)
}

export default withStyles(styles, {withTheme: true})(App)
