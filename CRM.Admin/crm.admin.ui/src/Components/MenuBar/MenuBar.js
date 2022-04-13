import React, {useState} from 'react'
import {compose} from 'react-recompose'
import {styled, useTheme} from '@mui/styles'
import {useNavigate, useLocation} from 'react-router-dom'
import {
	Box,
	CssBaseline,
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

const DrawerHeader = styled('div')(({theme}) => ({
	display: 'flex',
	alignItems: 'center',
	justifyContent: 'flex-end',
	padding: theme.spacing(0, 1),
	// necessary for content to be below app bar
	...theme.mixins.toolbar,
}))

const MenuBar = props => {
	const theme = useTheme()
	const navigate = useNavigate()
	const location = useLocation()

	const [open, setOpen] = useState(false)
	const [openDrawer, setOpenDrawer] = useState(localStorage.getItem('drawerState') == 'opened')
	const [openDictionaries, setOpenDictionaries] = useState(false)
	const [photoB64, setPhotoB64] = useState(null)
	const [anchorEl, setAnchorEl] = useState(null)
	const userData = userServices.getCurrentUser()
	const currentUser = userData || {roleId: 1}

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

	const handleLogOut = () => {
		localStorage.clear()
		sessionStorage.clear()
		location.reload()
	}

	const drawerStyle = {
		flexShrink: 0,
		whiteSpace: 'nowrap',
		boxSizing: 'border-box',
		...openDrawer && {
			width: drawerWidth,
			transition: theme.transitions.create('width', {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.enteringScreen,
			}),
			overflowX: 'hidden',
			'& .MuiDrawer-paper': {
				width: drawerWidth,
				transition: theme.transitions.create('width', {
					easing: theme.transitions.easing.sharp,
					duration: theme.transitions.duration.enteringScreen,
				}),
				overflowX: 'hidden',
			},
		},
		...!openDrawer && {
			transition: theme.transitions.create('width', {
				easing: theme.transitions.easing.sharp,
				duration: theme.transitions.duration.leavingScreen,
			}),
			overflowX: 'hidden',
			width: `calc(${theme.spacing(7)} + 1px)`,
			[theme.breakpoints.up('sm')]: {
				width: `calc(${theme.spacing(8)} + 1px)`,
			},
			'& .MuiDrawer-paper': {
				transition: theme.transitions.create('width', {
					easing: theme.transitions.easing.sharp,
					duration: theme.transitions.duration.leavingScreen,
				}),
				overflowX: 'hidden',
				width: `calc(${theme.spacing(7)} + 1px)`,
				[theme.breakpoints.up('sm')]: {
					width: `calc(${theme.spacing(8)} + 1px)`,
				},
			},
		},
	}

	return (
		<Box sx={{display: 'flex'}}>
			<CssBaseline />
			<AppBar
				position='fixed'
				style={{
					zIndex: theme.zIndex.drawer + 1,
					marginLeft: openDrawer ? drawerWidth : 0,
					width: openDrawer ? `calc(100% - ${drawerWidth}px)` : '100%',
					transition: theme.transitions.create(['width', 'margin'], {
						easing: theme.transitions.easing.sharp,
						duration: openDrawer ? theme.transitions.duration.enteringScreen : theme.transitions.duration.leavingScreen,
					}),
				}}
			>
				<Toolbar>
					{props.isAuthorized && (
						<IconButton
							color='inherit'
							aria-label='open drawer'
							onClick={handleDrawerOpen}
							edge='start'
							sx={{
								marginRight: 5,
								...openDrawer && {display: 'none'},
							}}
						>
							<MenuIcon />
						</IconButton>
					)}
					<Typography variant='h6' noWrap>
						CRM. Страница администратора
					</Typography>
					{
						props.isAuthorized && <React.Fragment>
							<div style={{flexGrow: 1}} />
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
										? <Avatar
											alt='Фото пользователя'
											src={`data:image/jpeg;base64,${photoB64}`} />
										: <Avatar
											alt='Фото пользователя'
											style={{color: theme.palette.getContrastText(deepOrange[500]), backgroundColor: deepOrange[500]}} >
											{currentUser && currentUser.shortNameRu ? currentUser.shortNameRu[0] : 'A'}
										</Avatar>
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
								<MenuItem onClick={() => { navigate('/profile') }}>Профиль</MenuItem>
								<MenuItem onClick={handleLogOut}>Выйти</MenuItem>
							</Menu>
						</React.Fragment>
					}
				</Toolbar>
			</AppBar>
			{
				props.isAuthorized && <Drawer
					variant='permanent'
					sx={drawerStyle}
				>
					<DrawerHeader>
						<IconButton onClick={handleDrawerClose}>
							{theme.direction === 'rtl'? <ChevronRightIcon /> : <ChevronLeftIcon />}
						</IconButton>
					</DrawerHeader>
					<Divider />
					<List>
						<ListItem button sx={{pl: 2}} onClick={() => { navigate('/') }}>
							<Tooltip title='Главная страница'>
								<ListItemIcon><HomeIcon /></ListItemIcon>
							</Tooltip>
							<Tooltip title='Главная страница'>
								<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Главная страница'} />
							</Tooltip>
						</ListItem>
						{
							currentUser && (currentUser.roleId == 1 || currentUser.roleId == 2) && <ListItem button sx={{pl: 2}} onClick={() => { navigate('/users-list') }}>
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
									<ListItem button sx={{pl: 2}} onClick={handleOpenDictionariesClick}>
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
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-contries') }}>
														<Tooltip title='Страны'>
															<ListItemIcon><PublicIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Страны'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Страны'} />
														</Tooltip>
													</ListItem>
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-cities') }}>
														<Tooltip title='Города'>
															<ListItemIcon><LocationCityIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Города'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Города'} />
														</Tooltip>
													</ListItem>
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-departments') }}>
														<Tooltip title='Структурные подразделения'>
															<ListItemIcon><AccountTreeIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Структурные подразделения'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Структурные подразделения'} />
														</Tooltip>
													</ListItem>
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-positions') }}>
														<Tooltip title='Должности'>
															<ListItemIcon><PortraitIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Должности'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Должности'} />
														</Tooltip>
													</ListItem>
												</React.Fragment>
											}
											<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-services') }}>
												<Tooltip title='Предоставляемые услуги'>
													<ListItemIcon><InsertLinkIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Предоставляемые услуги'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Предоставляемые услуги'} />
												</Tooltip>
											</ListItem>
											<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-intolerances') }}>
												<Tooltip title='Аллергические заболевания'>
													<ListItemIcon><BlockIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Аллергические заболевания'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Аллергические заболевания'} />
												</Tooltip>
											</ListItem>
											<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-loyalty-programs') }}>
												<Tooltip title='Бонусные программы'>
													<ListItemIcon><LoyaltyIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Бонусные программы'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Бонусные программы'} />
												</Tooltip>
											</ListItem>
											<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-genders') }}>
												<Tooltip title='Пол'>
													<ListItemIcon><WcIcon /></ListItemIcon>
												</Tooltip>
												<Tooltip title='Пол'>
													<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Пол'} />
												</Tooltip>
											</ListItem>
											{
												currentUser && currentUser.roleId == 1 && <React.Fragment>
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-statuses') }}>
														<Tooltip title='Статусы'>
															<ListItemIcon><LoupeIcon /></ListItemIcon>
														</Tooltip>
														<Tooltip title='Статусы'>
															<ListItemText primaryTypographyProps={{noWrap: true}} primary={'Статусы'} />
														</Tooltip>
													</ListItem>
													<ListItem button sx={{pl: openDrawer ? 4 : 2}} onClick={() => { navigate('/dictionary-enterprises') }}>
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
			}
		</Box>
	)
}

export default compose(withSnackbar, loading)(MenuBar)
