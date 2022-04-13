import React from 'react'
import PropTypes from 'prop-types'
import clsx from 'clsx'
import {withStyles} from '@mui/styles'
import {
	Drawer,
	AppBar,
	Toolbar,
	Typography,
	IconButton,
	MenuItem,
	Menu,
	Collapse,
	Avatar,
	Divider,
	Badge,
	Tooltip,
	List,
	ListItem,
	ListItemIcon,
	ListItemText,
} from '@mui/material'
import BallotIcon from '@mui/icons-material/Ballot'
import LoupeIcon from '@mui/icons-material/Loupe'
import NotificationsIcon from '@mui/icons-material/Notifications'
import AccountCircleIcon from '@mui/icons-material/AccountCircle'
import LibraryBooksIcon from '@mui/icons-material/LibraryBooks'
import ExpandLess from '@mui/icons-material/ExpandLess'
import ExpandMore from '@mui/icons-material/ExpandMore'
import ViewListIcon from '@mui/icons-material/ViewList'
import PublicIcon from '@mui/icons-material/Public'
import LocationCityIcon from '@mui/icons-material/LocationCity'
import AccountTreeIcon from '@mui/icons-material/AccountTree'
import PortraitIcon from '@mui/icons-material/Portrait'
import InsertLinkIcon from '@mui/icons-material/InsertLink'
import BlockIcon from '@mui/icons-material/Block'
import WcIcon from '@mui/icons-material/Wc'
import LoyaltyIcon from '@mui/icons-material/Loyalty'
import BusinessIcon from '@mui/icons-material/Business'
import MenuIcon from '@mui/icons-material/Menu'
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft'
import ChevronRightIcon from '@mui/icons-material/ChevronRight'
import HomeIcon from '@mui/icons-material/Home'
import AddBoxIcon from '@mui/icons-material/AddBox'
import AllInboxIcon from '@mui/icons-material/AllInbox'
import PeopleIcon from '@mui/icons-material/People'
import {deepOrange} from '@mui/material/colors'
import {allConstants} from 'Constants/AllConstants.js'
import {getRequest} from 'Services/RequestsServices.js'

const drawerWidth = 240

const styles = theme => ({
	menuItem: {
		marginRight: 15,
		color: '#fff',
		textDecoration: 'none',
	},
	root: {
		display: 'flex',
		'& > *': {
			margin: theme.spacing(1),
		},
	},
	small: {
		width: theme.spacing(3),
		height: theme.spacing(3),
	},
	orange: {
		color: theme.palette.getContrastText(deepOrange[500]),
		backgroundColor: deepOrange[500],
	},
	appBar: {
		zIndex: 1202,
		transition: theme.transitions.create(['width', 'margin'], {
			easing: theme.transitions.easing.sharp,
			duration: theme.transitions.duration.leavingScreen,
		}),
		background: 'green',
	},
	appBarShift: {
		marginLeft: drawerWidth,
		width: `calc(100% - ${drawerWidth}px)`,
		transition: theme.transitions.create(['width', 'margin'], {
			easing: theme.transitions.easing.sharp,
			duration: theme.transitions.duration.enteringScreen,
		}),
	},
	menuButton: {
		marginRight: 36,
	},
	hide: {
		display: 'none',
	},
	drawer: {
		width: drawerWidth,
		flexShrink: 0,
		whiteSpace: 'nowrap',
		zIndex: 1200,
	},
	drawerOpen: {
		width: drawerWidth,
		transition: theme.transitions.create('width', {
			easing: theme.transitions.easing.sharp,
			duration: theme.transitions.duration.enteringScreen,
		}),
	},
	drawerClose: {
		transition: theme.transitions.create('width', {
			easing: theme.transitions.easing.sharp,
			duration: theme.transitions.duration.leavingScreen,
		}),
		overflowX: 'hidden',
		width: theme.spacing(7) + 1,
		[theme.breakpoints.up('sm')]: {
			width: theme.spacing(9) + 1,
		},
	},
	toolbar: {
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'flex-end',
		// necessary for content to be below app bar
		...theme.mixins.toolbar,
	},
	content: {
		flexGrow: 1,
		padding: theme.spacing(3),
	},
	grow: {
		flexGrow: 1,
	},
	nested: {
		paddingLeft: theme.spacing(4),
	},
})

class MenuBar extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			open: false,
			openDrawer: false,
			openDictionaries: false,
			photoB64: null,
		}
	}

	componentDidMount() {
		const drawerState = localStorage.getItem('drawerState')
		this.setState({
			openDrawer: drawerState == 'opened',
		})
	}

	getCurrentUserPhoto = () => {
		const {token} = this.props

		getRequest(`${allConstants.serverUrl}/api/Users/GetCurrentUserPhoto`, token, result => {
			if (result && result.isSuccess) {
				this.setState({
					photoB64: result.data,
				})
			}
		},
		error => {
			console.log(error)
		})
	}

handleMenu = event => {
	this.setState({
		anchorEl: event.currentTarget,
		open: true,
	})
}

handleClose = () => {
	this.setState({
		anchorEl: null,
		open: false,
	})
}

handleDrawerOpen = () => {
	this.setState({
		openDrawer: true,
	}, () => {
		localStorage.setItem('drawerState', 'opened')
	})
}

handleDrawerClose = () => {
	this.setState({
		openDrawer: false,
	}, () => {
		localStorage.setItem('drawerState', 'closed')
	})
}

handleOpenDictionariesClick = () => {
	this.setState({
		openDictionaries: !this.state.openDictionaries,
	})
}

openRoute = route => {
	window.location.href = route
}

handleLogOut = () => {
	localStorage.clear()
	location.reload()
}

render() {
	const {classes, theme, isAuthorized, currentUser} = this.props

	return (
		<div className='App'>
			<AppBar
				position='fixed'
				className={clsx(classes.appBar, {
					[classes.appBarShift]: this.state.openDrawer && isAuthorized,
				})}
			>
				<Toolbar>
					{isAuthorized && (
						<IconButton
							color='inherit'
							aria-label='open drawer'
							onClick={this.handleDrawerOpen}
							edge='start'
							className={clsx(classes.menuButton, {
								[classes.hide]: this.state.openDrawer,
							})}
						>
							<MenuIcon />
						</IconButton>
					)}
					<Typography variant='h6' noWrap>
					CRM. Страница администратора
					</Typography>
					{isAuthorized && (
						<React.Fragment>
							<div className={classes.grow} />
							{/* <IconButton aria-label='show 17 new notifications' color='inherit'>
								<Badge badgeContent={17} color='secondary'>
									<NotificationsIcon />
								</Badge>
							</IconButton> */}
							<IconButton
								aria-label='account of current user'
								aria-controls='menu-appbar'
								aria-haspopup='true'
								onClick={this.handleMenu}
								color='inherit'
								edge='end'
							>
								{
									this.state.photoB64
										? <Avatar alt='Фото пользователя' src={`data:image/jpeg;base64,${this.state.photoB64}`} />
										: <Avatar alt='Фото пользователя' className={classes.orange} >{currentUser && currentUser.shortNameRu ? currentUser.shortNameRu[0] : 'A'}</Avatar>
								}
								{currentUser && <Typography variant='button' display='block'>{`${currentUser.shortNameRu}`}</Typography>}
							</IconButton>
							<Menu
								id='menu-appbar'
								anchorEl={this.state.anchorEl}
								getContentAnchorEl={null}
								anchorOrigin={{
									vertical: 'bottom',
									horizontal: 'right',
								}}
								keepMounted
								transformOrigin={{
									vertical: 'top',
									horizontal: 'right',
								}}
								open={this.state.open}
								onClose={this.handleClose}
							>
								<MenuItem onClick={() => { this.openRoute('/profile') }}>Профиль</MenuItem>
								<MenuItem onClick={this.handleLogOut}>Выйти</MenuItem>
							</Menu>
						</React.Fragment>
					)}
				</Toolbar>
			</AppBar>
			{isAuthorized && (
				<Drawer
					variant='permanent'
					className={clsx(classes.drawer, {
						[classes.drawerOpen]: this.state.openDrawer,
						[classes.drawerClose]: !this.state.openDrawer,
					})}
					classes={{
						paper: clsx({
							[classes.drawerOpen]: this.state.openDrawer,
							[classes.drawerClose]: !this.state.openDrawer,
						}),
					}}
				>
					<div className={classes.toolbar}>
						<IconButton onClick={this.handleDrawerClose}>
							{theme.direction === 'rtl' ? <ChevronRightIcon /> : <ChevronLeftIcon />}
						</IconButton>
					</div>
					<Divider />
					<List>
						<ListItem button onClick={() => { this.openRoute('/') }}>
							<Tooltip title='Главная страница'>
								<ListItemIcon><HomeIcon /></ListItemIcon>
							</Tooltip>
							<Tooltip title='Главная страница'>
								<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Главная страница'} />
							</Tooltip>
						</ListItem>
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) && <ListItem button onClick={() => { this.openRoute('/users-list') }}>
								<Tooltip title='Пользователи системы'>
									<ListItemIcon><PeopleIcon /></ListItemIcon>
								</Tooltip>
								<Tooltip title='Пользователи системы'>
									<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Пользователи системы'} />
								</Tooltip>
							</ListItem>
						}
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2)
								? <React.Fragment>
									<ListItem button onClick={this.handleOpenDictionariesClick}>
										<Tooltip title='Справочники'>
											<ListItemIcon><ViewListIcon /></ListItemIcon>
										</Tooltip>
										<Tooltip title='Справочники'>
											<ListItemText primaryTypographyProps={{noWrap: true}} primary='Справочники' />
										</Tooltip>
										{this.state.openDictionaries ? <ExpandLess /> : <ExpandMore />}
									</ListItem>
									<Collapse in={this.state.openDictionaries} timeout='auto' unmountOnExit>
										<List component='div' disablePadding>
											{
												currentUser && currentUser.roleId == 1 && <React.Fragment>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-contries') }}>
														<Tooltip title='Страны'>
															<ListItemIcon><PublicIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Страны'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Страны'} />
														</Tooltip>
													</ListItem>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-cities') }}>
														<Tooltip title='Города'>
															<ListItemIcon><LocationCityIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Города'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Города'} />
														</Tooltip>
													</ListItem>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-departments') }}>
														<Tooltip title='Структурные подразделения'>
															<ListItemIcon><AccountTreeIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Структурные подразделения'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Структурные подразделения'} />
														</Tooltip>
													</ListItem>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-positions') }}>
														<Tooltip title='Должности'>
															<ListItemIcon><PortraitIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Должности'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Должности'} />
														</Tooltip>
													</ListItem>
												</React.Fragment>
											}
											<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-services') }}>
												<Tooltip title='Предоставляемые услуги'>
													<ListItemIcon><InsertLinkIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Предоставляемые услуги'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Предоставляемые услуги'} />
												</Tooltip>
											</ListItem>
											<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-intolerances') }}>
												<Tooltip title='Аллергические заболевания'>
													<ListItemIcon><BlockIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Аллергические заболевания'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Аллергические заболевания'} />
												</Tooltip>
											</ListItem>
											<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-loyalty-programs') }}>
												<Tooltip title='Бонусные программы'>
													<ListItemIcon><LoyaltyIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Бонусные программы'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Бонусные программы'} />
												</Tooltip>
											</ListItem>
											<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-genders') }}>
												<Tooltip title='Пол'>
													<ListItemIcon><WcIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Пол'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Пол'} />
												</Tooltip>
											</ListItem>
											{
												currentUser && currentUser.roleId == 1 && <React.Fragment>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-statuses') }}>
														<Tooltip title='Статусы'>
															<ListItemIcon><LoupeIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Статусы'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Статусы'} />
														</Tooltip>
													</ListItem>
													<ListItem button className={classes.nested} onClick={() => { this.openRoute('/dictionary-enterprises') }}>
														<Tooltip title='Компании/филиалы'>
															<ListItemIcon><BusinessIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Компании/филиалы'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Компании/филиалы'} />
														</Tooltip>
													</ListItem>
												</React.Fragment>
											}
										</List>
									</Collapse>
								</React.Fragment>
								: null
						}
					</List>
				</Drawer>
			)}
		</div>
	)
}
}

MenuBar.propTypes = {
	classes: PropTypes.object.isRequired,
}

export default withStyles(styles, {withTheme: true})(MenuBar)
