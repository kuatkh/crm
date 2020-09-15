import React from 'react'
import PropTypes from 'prop-types'
import {withStyles} from '@material-ui/core/styles'
import clsx from 'clsx'
import Drawer from '@material-ui/core/Drawer'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import IconButton from '@material-ui/core/IconButton'
import MenuItem from '@material-ui/core/MenuItem'
import Menu from '@material-ui/core/Menu'
import MenuList from '@material-ui/core/MenuList'
import Collapse from '@material-ui/core/Collapse'
import Avatar from '@material-ui/core/Avatar'
import Badge from '@material-ui/core/Badge'
import BallotIcon from '@material-ui/icons/Ballot'
import LoupeIcon from '@material-ui/icons/Loupe'
import NotificationsIcon from '@material-ui/icons/Notifications'
import AccountCircleIcon from '@material-ui/icons/AccountCircle'
import LibraryBooksIcon from '@material-ui/icons/LibraryBooks'
import ExpandLess from '@material-ui/icons/ExpandLess'
import ExpandMore from '@material-ui/icons/ExpandMore'
import ViewListIcon from '@material-ui/icons/ViewList'
import PublicIcon from '@material-ui/icons/Public'
import LocationCityIcon from '@material-ui/icons/LocationCity'
import AccountTreeIcon from '@material-ui/icons/AccountTree'
import PortraitIcon from '@material-ui/icons/Portrait'
import InsertLinkIcon from '@material-ui/icons/InsertLink'
import BlockIcon from '@material-ui/icons/Block'
import WcIcon from '@material-ui/icons/Wc'
import LoyaltyIcon from '@material-ui/icons/Loyalty'
import BusinessIcon from '@material-ui/icons/Business'

import Typography from '@material-ui/core/Typography'
import Divider from '@material-ui/core/Divider'
import List from '@material-ui/core/List'
import MenuIcon from '@material-ui/icons/Menu'
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft'
import ChevronRightIcon from '@material-ui/icons/ChevronRight'
import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import Tooltip from '@material-ui/core/Tooltip'
import HomeIcon from '@material-ui/icons/Home'
import AddBoxIcon from '@material-ui/icons/AddBox'
import AllInboxIcon from '@material-ui/icons/AllInbox'
import PeopleIcon from '@material-ui/icons/People'

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
	large: {
		width: theme.spacing(7),
		height: theme.spacing(7),
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
		padding: theme.spacing(1),
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
})

class MenuBar extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			open: false,
			openDrawer: false,
			openDictionaries: false,
		}
	}

	componentDidMount() {
		const drawerState = localStorage.getItem('drawerState')
		this.setState({
			openDrawer: drawerState == 'opened',
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
									currentUser && currentUser.photoB64
										? <Avatar alt='Фото пользователя' src={`data:image/jpeg;base64,${currentUser.photoB64}`} className={classes.large} />
										: <AccountCircleIcon />
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
								<MenuItem onClick={this.handleClose}>Профиль</MenuItem>
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
							<ListItemText primary={'Главная страница'} />
						</ListItem>
						<ListItem button onClick={() => { this.openRoute('/users-list') }}>
							<Tooltip title='Пользователи системы'>
								<ListItemIcon><PeopleIcon /></ListItemIcon>
							</Tooltip>
							<ListItemText primary={'Пользователи системы'} />
						</ListItem>
						<ListItem button onClick={this.handleOpenDictionariesClick}>
							<ListItemIcon>
								<ViewListIcon />
							</ListItemIcon>
							<ListItemText primary='Справочники' />
							{this.state.openDictionaries ? <ExpandLess /> : <ExpandMore />}
						</ListItem>
						<Collapse in={this.state.openDictionaries} timeout='auto' unmountOnExit>
							<List component='div' disablePadding>
								<ListItem button onClick={() => { this.openRoute('/dictionary-contries') }}>
									<Tooltip title='Страны'>
										<ListItemIcon><PublicIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Страны'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-cities') }}>
									<Tooltip title='Города'>
										<ListItemIcon><LocationCityIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Города'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-departments') }}>
									<Tooltip title='Структурные подразделения'>
										<ListItemIcon><AccountTreeIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Структурные подразделения'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-positions') }}>
									<Tooltip title='Должности'>
										<ListItemIcon><PortraitIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Должности'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-services') }}>
									<Tooltip title='Предоставляемые услуги'>
										<ListItemIcon><InsertLinkIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Предоставляемые услуги'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-intolerances') }}>
									<Tooltip title='Аллергические заболевания'>
										<ListItemIcon><BlockIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Аллергические заболевания'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-genders') }}>
									<Tooltip title='Пол'>
										<ListItemIcon><WcIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Пол'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-loyalty-programs') }}>
									<Tooltip title='Бонусные программы'>
										<ListItemIcon><LoyaltyIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Бонусные программы'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-statuses') }}>
									<Tooltip title='Статусы'>
										<ListItemIcon><LoupeIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Статусы'} />
								</ListItem>
								<ListItem button onClick={() => { this.openRoute('/dictionary-enterprises') }}>
									<Tooltip title='Компании/филиалы'>
										<ListItemIcon><BusinessIcon /></ListItemIcon>
									</Tooltip>
									<ListItemText primary={'Компании/филиалы'} />
								</ListItem>
							</List>
						</Collapse>
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
