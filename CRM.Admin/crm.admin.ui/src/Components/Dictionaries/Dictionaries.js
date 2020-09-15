import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import Typography from '@material-ui/core/Typography'
import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import PersonAddIcon from '@material-ui/icons/PersonAdd'
import AddDictionaryData from './AddDictionaryData'
import AbTable from '../AbTable'
import {allConstants} from '../../Constants/AllConstants.js'
import {dictionariesColumns} from '../../Constants/TableColumns.js'

const styles = theme => ({
	formControl: {
		margin: theme.spacing(1),
		minWidth: 120,
		maxWidth: 300,
	},
	chips: {
		display: 'flex',
		flexWrap: 'wrap',
	},
	chip: {
		margin: 2,
	},
	noLabel: {
		marginTop: theme.spacing(3),
	},
	selectEmpty: {
		marginTop: theme.spacing(2),
	},
	container: {
		flexWrap: 'wrap',
		gridTemplateColumns: 'repeat(12, 1fr)',
		gridGap: theme.spacing(1),
		margin: 0,
		padding: 20,
	},
	input: {
		// margin: theme.spacing.unit,
		margin: 0,
	},
	button: {
		margin: theme.spacing.unit,
	},
	gapSmall: {
		marginTop:50,
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
		zIndex: theme.zIndex.drawer + 1,
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
			openSnackbar: false,
			snackbarMsg: '',
			snackbarSeverity: 'success',
			loading: false,
		}
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

handleEditDictionaryDialogOpen = () => {
	this.setState({
		openEditDictionaryDialog: true,
	})
}

handleEditDictionaryDialogClose = isSuccess => {
	this.setState({
		openEditDictionaryDialog: false,
	})
	if (isSuccess) {
		this.handleSnackbarOpen('Изменения успешно сохранены!', 'success')
	}
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
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
			<br/>
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
							url={`${allConstants.serverUrl}/api/Dictionaries/GetDictionaryData`}
							filterData={{
								dictionaryName,
							}}
							tableStyle={{tableLayout: 'fixed'}}
							defaultOrderByColumn='nameRu'
							columns={dictionariesColumns[dictionaryName]}
							isLoaded={this.isLoaded}
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