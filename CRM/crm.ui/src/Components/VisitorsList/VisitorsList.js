import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import TextField from '@material-ui/core/TextField'
import Snackbar from '@material-ui/core/Snackbar'
import Alert from '@material-ui/lab/Alert'
import Autocomplete from '@material-ui/lab/Autocomplete'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Grid from '@material-ui/core/Grid'
import Typography from '@material-ui/core/Typography'
import Backdrop from '@material-ui/core/Backdrop'
import CircularProgress from '@material-ui/core/CircularProgress'
import CrmTable from '../CrmTable'
import {visitorsTableColumns} from '../../Constants/TableColumns.js'
import {allConstants} from '../../Constants/AllConstants.js'

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
})

class VisitorsList extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			visitorsTypeOptions: [
				{id: 1, nameRu: 'Все посетители'},
				{id: 2, nameRu: 'Посетители в здании'},
				{id: 3, nameRu: 'Вышедшие посетители'},
			],
			visitorsType: {id: 1, nameRu: 'Все посетители'},
			openVisitorDialog: false,
			tableRows: [],
			editVisitorData: null,
			openSnackbar: false,
			selectedCardId: null,
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

handleAutocompleteChange = (e, v) => {
	this.setState({...this.state, [e]: v})
}

handleVisitorDialogOpen = () => {
	this.setState({
		openVisitorDialog: true,
	})
}

handleVisitorDialogClose = visitor => {
	if (visitor) {
		let rows = [...this.state.tableRows]
		let lastId = 0
		if (rows.sort((a, b) => b.Id - a.Id).length > 0) {
			lastId = rows.sort((a, b) => b.Id - a.Id)[0].Id
		}
		if (rows.some(row => row.Id == visitor.Id)) {
			let ix = -1
			rows.forEach((v, i) => {
				if (v.Id == visitor.Id) {
					ix = i
				}
			})
			if (ix > -1) {
				rows.splice(ix, 1)
				rows = [...rows, {...visitor, Id: lastId + 1}]
			}
		} else {
			rows = [...rows, {...visitor, Id: lastId + 1}]
		}

		this.setState({
			openVisitorDialog: false,
			tableRows: rows,
			editVisitorData: null,
		})
	} else {
		this.setState({
			openVisitorDialog: false,
			editVisitorData: null,
		})
	}
}

getStyles = (name, selected, theme) => ({
	fontWeight:
Array.isArray(selected) && selected.indexOf(name) === -1 || !Array.isArray(selected) && selected == name
	? theme.typography.fontWeightRegular
	: theme.typography.fontWeightMedium,
})

handleLaunchClick = id => {
	this.setState({
		selectedCardId: id,
		openVisitorDialog: true,
		editVisitorData: null,
	})
}

handleDeleteClick = id => {
	let rows = [...this.state.tableRows]
	if (rows.some(row => row.Id == id)) {
		let ix = -1
		rows.forEach((v, i) => {
			if (v.Id == id) {
				ix = i
			}
		})
		if (ix > -1) {
			rows.splice(ix, 1)
		}
	}

	this.setState({
		openVisitorDialog: false,
		tableRows: rows,
		editVisitorData: null,
	})
}

isLoaded = loading => {
	this.setState({
		loading: !loading,
	})
}

render() {
	const {classes, token} = this.props

	return (
		<div>
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<Typography variant='h4' display='block'>Посетители</Typography>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={6}>
					<Paper className={classes.paper}>
						<Autocomplete
							id='visitorsType'
							name='visitorsType'
							disableClearable
							fullWidth
							value={this.state.visitorsType}
							options={this.state.visitorsTypeOptions}
							onChange={(e, v) => { this.handleAutocompleteChange('visitorsType', v) }}
							getOptionLabel={option => option.nameRu}
							renderInput={params => <TextField {...params} label='Тип посетителей' variant='outlined' />}
						/>
					</Paper>
				</Grid>
			</Grid>
			<Divider className={classes.divider} />
			<Grid container className={classes.container}>
				<Grid item xs={12}>
					<Paper className={classes.paper}>
						<CrmTable
							url={`${allConstants.serverUrl}/api/Visitors/GetVisitors`}
							columns={visitorsTableColumns}
							token={token}
							filterData={{
								statusType: this.state.visitorsType.id,
							}}
							isLoaded={this.isLoaded}
							tableContainerStyles={{display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '30vh'}}
							handleSnackbarOpen={this.handleSnackbarOpen}
							// handleLaunchClick={this.handleLaunchClick}
							handleDeleteClick={this.handleDeleteClick}
							// canOpen={true}
						/>
					</Paper>
				</Grid>
			</Grid>
			<Snackbar open={this.state.openSnackbar} autoHideDuration={6000} onClose={this.handleSnackbarClose}>
				<Alert onClose={this.handleSnackbarClose} severity={this.state.snackbarSeverity}>
					{this.state.snackbarMsg} {this.state.tickValue}
				</Alert>
			</Snackbar>
			<Backdrop className={classes.backdrop} open={this.state.loading}>
				<CircularProgress color='inherit' />
			</Backdrop>
			{/* <div>
				<Dialog
					open={this.state.openVisitorDialog}
					onClose={this.handleAddVisitorClose}
					scroll={'paper'}
					fullWidth={true}
					maxWidth={'lg'}
					aria-labelledby='scroll-dialog-title'
					aria-describedby='scroll-dialog-description'
				>
					<AddVisitor handleVisitorDialogClose={this.handleVisitorDialogClose} editVisitorData={this.state.editVisitorData} />
				</Dialog>
			</div> */}
		</div>
	)
}
}

export default withStyles(styles, {withTheme: true})(VisitorsList)
