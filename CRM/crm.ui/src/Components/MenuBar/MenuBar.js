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
import Avatar from '@material-ui/core/Avatar'
import Badge from '@material-ui/core/Badge'
import NotificationsIcon from '@material-ui/icons/Notifications'
import AccountCircleIcon from '@material-ui/icons/AccountCircle'
import LibraryBooksIcon from '@material-ui/icons/LibraryBooks'
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
		padding: 0,
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
						CRM
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
								{/* <MenuItem onClick={this.handleClose}>My account</MenuItem> */}
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
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Главная страница'} />
						</ListItem>
						{/* <ListItem button onClick={() => { this.openRoute('/add-card') }}>
							<Tooltip title='Создать пропуск'>
								<ListItemIcon><AddBoxIcon /></ListItemIcon>
							</Tooltip>
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Создать пропуск'} />
						</ListItem>
						<ListItem button onClick={() => { this.openRoute('/cards-list') }}>
							<Tooltip title='Мои пропуска'>
								<ListItemIcon><AllInboxIcon /></ListItemIcon>
							</Tooltip>
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Мои пропуска'} />
						</ListItem>
						<ListItem button onClick={() => { this.openRoute('/agreement-cards-list') }}>
							<Tooltip title='Пропуска на согласование'>
								<ListItemIcon><LibraryBooksIcon /></ListItemIcon>
							</Tooltip>
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Пропуска на согласование'} />
						</ListItem>
						<ListItem button onClick={() => { this.openRoute('/visitors-list') }}>
							<Tooltip title='Посетители'>
								<ListItemIcon><PeopleIcon /></ListItemIcon>
							</Tooltip>
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Посетители'} />
						</ListItem> */}
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
