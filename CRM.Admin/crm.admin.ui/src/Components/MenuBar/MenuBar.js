import React, {useState} from 'react'
import {compose} from 'react-recompose'
import clsx from 'clsx'
import {withStyles, styled, useTheme} from '@mui/styles'
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
import {appConstants} from 'constants/app.constants.js'
import {getRequest} from 'services/requests.services.js'
import {userServices} from 'services/user.services'
import {withSnackbar} from 'components/SnackbarWrapper'
import {loading} from 'components/LoadingWrapper'

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

const openedMixin = (theme) => ({
	width: drawerWidth,
	transition: theme.transitions.create('width', {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.enteringScreen,
	}),
	overflowX: 'hidden',
});
  
const closedMixin = (theme) => ({
	transition: theme.transitions.create('width', {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.leavingScreen,
	}),
	overflowX: 'hidden',
	width: `calc(${theme.spacing(7)} + 1px)`,
	[theme.breakpoints.up('sm')]: {
		width: `calc(${theme.spacing(8)} + 1px)`,
	},
});

const CrmDrawer = styled(Drawer, { shouldForwardProp: (prop) => prop !== 'open' })(
	({ theme, open }) => ({
		width: drawerWidth,
		flexShrink: 0,
		whiteSpace: 'nowrap',
		boxSizing: 'border-box',
		...(open && {
			...openedMixin(theme),
			'& .MuiDrawer-paper': openedMixin(theme),
		}),
		...(!open && {
			...closedMixin(theme),
			'& .MuiDrawer-paper': closedMixin(theme),
		}),
	}),
);

const DrawerHeader = styled('div')(({ theme }) => ({
	display: 'flex',
	alignItems: 'center',
	justifyContent: 'flex-end',
	padding: theme.spacing(0, 1),
	// necessary for content to be below app bar
	...theme.mixins.toolbar,
}));

const CrmAppBar = styled(AppBar, {
	shouldForwardProp: (prop) => prop !== 'open',
  })(({ theme, open }) => ({
	zIndex: theme.zIndex.drawer + 1,
	transition: theme.transitions.create(['width', 'margin'], {
		easing: theme.transitions.easing.sharp,
		duration: theme.transitions.duration.leavingScreen,
	}),
	...(open && {
		marginLeft: drawerWidth,
		width: `calc(100% - ${drawerWidth}px)`,
		transition: theme.transitions.create(['width', 'margin'], {
			easing: theme.transitions.easing.sharp,
			duration: theme.transitions.duration.enteringScreen,
		}),
	}),
}));

const MenuBar = props => {
	const theme = useTheme()

	const [open, setOpen] = useState(false)
	const [openDrawer, setOpenDrawer] = useState(localStorage.getItem('drawerState') == 'opened')
	const [openDictionaries, setOpenDictionaries] = useState(false)
	const [photoB64, setPhotoB64] = useState(null)
	const [anchorEl, setAnchorEl] = useState(null)
	const currentUser = userServices.getCurrentUser()

	const getCurrentUserPhoto = () => {
		getRequest(`${appConstants.serverUrl}/api/Users/GetCurrentUserPhoto`, result => {
			if (result && result.isSuccess) {
				setPhotoB64(result.data)
			}
		},
		error => {
			console.log(error)
		})
	}
	
	const handleMenu = event => {
		setAnchorEl(event.currentTarget)
		setOpen(true)
	}
	
	const handleClose = () => {
		setAnchorEl(null)
		setOpen(false)
	}

	const handleDrawerOpen = () => {
		console.log('open', props)
		setOpenDrawer(true)
		localStorage.setItem('drawerState', 'opened')
	}
	
	const handleDrawerClose = () => {
		setOpenDrawer(false)
		localStorage.setItem('drawerState', 'closed')
	}

	const handleOpenDictionariesClick = () => {
		setOpenDictionaries(!openDictionaries)
	}
	
	const openRoute = route => {
		window.location.href = route
	}
	
	const handleLogOut = () => {
		localStorage.clear()
		sessionStorage.clear()
		location.reload()
	}

	return (
	<div className='App'>
		<CrmAppBar
			position='fixed'
			open={openDrawer}
		>
			<Toolbar>
				{props.isAuthorized && (
					<IconButton
						color='inherit'
						aria-label='open drawer'
						onClick={handleDrawerOpen}
						edge='start'
						className={clsx(props.classes.menuButton, {
							[props.classes.hide]: openDrawer,
						})}
					>
						<MenuIcon />
					</IconButton>
				)}
				<Typography variant='h6' noWrap>
				CRM. Страница администратора
				</Typography>
				{props.isAuthorized && (
					<React.Fragment>
						<div className={props.classes.grow} />
						{/* <IconButton aria-label='show 17 new notifications' color='inherit'>
							<Badge badgeContent={17} color='secondary'>
								<NotificationsIcon />
							</Badge>
						</IconButton> */}
						<IconButton
							aria-label='account of current user'
							aria-controls='menu-appbar'
							aria-haspopup='true'
							onClick={handleMenu}
							color='inherit'
							edge='end'
						>
							{
								photoB64
									? <Avatar alt='Фото пользователя' src={`data:image/jpeg;base64,${photoB64}`} />
									: <Avatar alt='Фото пользователя' className={props.classes.orange} >{currentUser && currentUser.shortNameRu ? currentUser.shortNameRu[0] : 'A'}</Avatar>
							}
							{currentUser && <Typography variant='button' display='block'>{`${currentUser.shortNameRu}`}</Typography>}
						</IconButton>
						<Menu
							id='menu-appbar'
							anchorEl={anchorEl}
							anchorOrigin={{
								vertical: 'bottom',
								horizontal: 'right',
							}}
							keepMounted
							transformOrigin={{
								vertical: 'top',
								horizontal: 'right',
							}}
							open={open}
							onClose={handleClose}
							style={{zIndex: 1204}}
						>
							<MenuItem onClick={() => { openRoute('/profile') }}>Профиль</MenuItem>
							<MenuItem onClick={handleLogOut}>Выйти</MenuItem>
						</Menu>
					</React.Fragment>
				)}
			</Toolbar>
		</CrmAppBar>
		{props.isAuthorized && (
			<CrmDrawer
				variant='permanent'
				anchor='left'
				open={openDrawer}
			>
				<DrawerHeader>
					<IconButton onClick={handleDrawerClose}>
						{theme.direction === 'ltr'? <ChevronRightIcon /> : <ChevronLeftIcon />}
					</IconButton>
				</DrawerHeader>
				<Divider />
				<List>
					<ListItem button onClick={() => { openRoute('/') }}>
						<Tooltip title='Главная страница'>
							<ListItemIcon><HomeIcon /></ListItemIcon>
						</Tooltip>
						<Tooltip title='Главная страница'>
							<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Главная страница'} />
						</Tooltip>
					</ListItem>
					{
						currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) && <ListItem button onClick={() => { openRoute('/users-list') }}>
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
								<ListItem button onClick={handleOpenDictionariesClick}>
									<Tooltip title='Справочники'>
										<ListItemIcon><ViewListIcon /></ListItemIcon>
									</Tooltip>
									<Tooltip title='Справочники'>
										<ListItemText primaryTypographyProps={{noWrap: true}} primary='Справочники' />
									</Tooltip>
									{openDictionaries ? <ExpandLess /> : <ExpandMore />}
								</ListItem>
								<Collapse in={openDictionaries} timeout='auto' unmountOnExit>
									<List component='div' disablePadding>
										{
											currentUser && currentUser.roleId == 1 && <React.Fragment>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-contries') }}>
													<Tooltip title='Страны'>
														<ListItemIcon><PublicIcon /></ListItemIcon>
													</Tooltip>
													<Tooltip title='Страны'>
														<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Страны'} />
													</Tooltip>
												</ListItem>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-cities') }}>
													<Tooltip title='Города'>
														<ListItemIcon><LocationCityIcon /></ListItemIcon>
													</Tooltip>
													<Tooltip title='Города'>
														<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Города'} />
													</Tooltip>
												</ListItem>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-departments') }}>
													<Tooltip title='Структурные подразделения'>
														<ListItemIcon><AccountTreeIcon /></ListItemIcon>
													</Tooltip>
													<Tooltip title='Структурные подразделения'>
														<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Структурные подразделения'} />
													</Tooltip>
												</ListItem>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-positions') }}>
													<Tooltip title='Должности'>
														<ListItemIcon><PortraitIcon /></ListItemIcon>
													</Tooltip>
													<Tooltip title='Должности'>
														<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Должности'} />
													</Tooltip>
												</ListItem>
											</React.Fragment>
										}
										<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-services') }}>
											<Tooltip title='Предоставляемые услуги'>
												<ListItemIcon><InsertLinkIcon /></ListItemIcon>
											</Tooltip>
											<Tooltip title='Предоставляемые услуги'>
												<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Предоставляемые услуги'} />
											</Tooltip>
										</ListItem>
										<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-intolerances') }}>
											<Tooltip title='Аллергические заболевания'>
												<ListItemIcon><BlockIcon /></ListItemIcon>
											</Tooltip>
											<Tooltip title='Аллергические заболевания'>
												<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Аллергические заболевания'} />
											</Tooltip>
										</ListItem>
										<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-loyalty-programs') }}>
											<Tooltip title='Бонусные программы'>
												<ListItemIcon><LoyaltyIcon /></ListItemIcon>
											</Tooltip>
											<Tooltip title='Бонусные программы'>
												<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Бонусные программы'} />
											</Tooltip>
										</ListItem>
										<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-genders') }}>
											<Tooltip title='Пол'>
												<ListItemIcon><WcIcon /></ListItemIcon>
											</Tooltip>
											<Tooltip title='Пол'>
												<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Пол'} />
											</Tooltip>
										</ListItem>
										{
											currentUser && currentUser.roleId == 1 && <React.Fragment>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-statuses') }}>
													<Tooltip title='Статусы'>
														<ListItemIcon><LoupeIcon /></ListItemIcon>
													</Tooltip>
													<Tooltip title='Статусы'>
														<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Статусы'} />
													</Tooltip>
												</ListItem>
												<ListItem button className={props.classes.nested} onClick={() => { openRoute('/dictionary-enterprises') }}>
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
			</CrmDrawer>
		)}
	</div>
)
}

export default compose(withSnackbar, loading, withStyles(styles))(MenuBar)
