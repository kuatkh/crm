import React from 'react'
import {withStyles} from '@mui/styles'
import {
	Grid,
	Typography,
	Dialog,
	Button,
	Divider,
	Paper,
} from '@mui/material'
import PersonAddIcon from '@mui/icons-material/PersonAdd'
import AddDictionaryData from './AddDictionaryData'
import AbTable from 'components/AbTable'
import {appConstants} from 'constants/app.constants.js'
import {dictionariesColumns} from 'constants/columns.constants.js'

const styles = theme => ({
	container: {
		flexWrap: 'wrap',
		gridTemplateColumns: 'repeat(12, 1fr)',
		gridGap: theme.spacing(1),
		margin: 0,
		padding: 20,
	},
	button: {
		margin: theme.spacing.unit,
	},
	paper: {
		paddingRight: theme.spacing(1),
		// textAlign: 'center',
		color: theme.palette.text.secondary,
		whiteSpace: 'nowrap',
		marginBottom: theme.spacing(1),
		boxShadow: 'none',
	},
	divider: {
		margin: 0,
		fontWeight: 'normal',
	},
	backdrop: {
		zIndex: theme.zIndex.drawer + 3,
		color: '#fff',
	},
	typography: {
		padding: '10px',
		wordWrap: 'break-word',
		whiteSpace: 'break-spaces',
		display: 'inline-block',
	},
	gridPaddingRight: {
		paddingRight: '10px',
	},
})

class Dictionaries extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			openEditDictionaryDialog: false,
			editDictionaryData: null,
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
		}

		this._abTableGetData = null
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

handleEditDictionaryDialogOpen = () => {
	this.setState({
		openEditDictionaryDialog: true,
		editDictionaryData: null,
	})
}

handleEditDictionaryDialogClose = isSuccess => {
	this.setState({
		openEditDictionaryDialog: false,
		editDictionaryData: null,
	})
	if (isSuccess) {
		this.handleSnackbarOpen('Изменения успешно сохранены!', 'success')
		if (this._abTableGetData) {
			this._abTableGetData()
		}
	}
}

handleEditClick = row => {
	this.setState({
		openEditDictionaryDialog: true,
		editDictionaryData: row,
	})
}

render() {
	const {classes, token, dictionaryName, pageTitle} = this.props
	const {
		openSnackbar,
		snackbarMsg,
		snackbarSeverity,
		loading,
		openEditDictionaryDialog,
		editDictionaryData,
	} = this.state

	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>{pageTitle}</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={6}>
					<Paper className={classes.paper}>
						<Button startIcon={<PersonAddIcon />} variant='outlined' color='secondary' className={classes.button} style={{float: 'left', width: '50%'}} onClick={this.handleEditDictionaryDialogOpen}>
							Добавить значение
						</Button>
					</Paper>
				</Grid>
			</Grid>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<AbTable
							url={`${appConstants.serverUrl}/api/Dictionaries/GetDictionaryData`}
							filterData={{
								dictionaryName,
							}}
							setGetDataFunc={func => this._abTableGetData = func}
							tableStyle={{tableLayout: 'fixed'}}
							defaultOrderByColumn='nameRu'
							columns={dictionariesColumns[dictionaryName]}
							childrenColumns={dictionariesColumns[dictionaryName]}
							handleEditClick={this.handleEditClick}
							childrenHeader='Филиалы'
							isLoaded={this.isLoaded}
							canEdit={true}
							canExpand={true}
							tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
							handleSnackbarOpen={this.handleSnackbarOpen}
						/>
					</Paper>
				</Grid>
			</Grid>
			<Snackbar open={openSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
				<Alert onClose={this.handleSnackbarClose} severity={snackbarSeverity}>
					{snackbarMsg}
				</Alert>
			</Snackbar>
			<Backdrop className={classes.backdrop} open={loading}>
				<CircularProgress color='inherit' />
			</Backdrop>
			<div>
				<Dialog
					open={openEditDictionaryDialog}
					onClose={() => this.handleEditDictionaryDialogClose(false)}
					scroll={'paper'}
					fullWidth={true}
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<AddDictionaryData
						handleEditDictionaryDialogClose={this.handleEditDictionaryDialogClose}
						isLoaded={this.isLoaded}
						editDictionaryData={editDictionaryData}
						token={token}
						dictionaryName={dictionaryName}
						pageTitle={pageTitle}
						handleSnackbarOpen={this.handleSnackbarOpen} />
				</Dialog>
			</div>
		</div>
	)
}
}

export default withStyles(styles, {withTheme: true})(Dictionaries)
