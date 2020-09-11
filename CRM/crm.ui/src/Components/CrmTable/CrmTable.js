import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import Button from '@material-ui/core/Button'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableSortLabel from '@material-ui/core/TableSortLabel'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TablePagination from '@material-ui/core/TablePagination'
import TableRow from '@material-ui/core/TableRow'
import Paper from '@material-ui/core/Paper'
import EditIcon from '@material-ui/icons/Edit'
import DeleteIcon from '@material-ui/icons/Delete'
import LaunchIcon from '@material-ui/icons/Launch'
import CardMedia from '@material-ui/core/CardMedia'
import Typography from '@material-ui/core/Typography'
import _ from 'lodash'
import {postRequest} from '../../Services/RequestsServices.js'

const styles = theme => ({
	container: {
		display: 'flex',
		flexWrap: 'wrap',
		minHeight: '10vh',
		maxHeight: '50vh',
	},
	button: {
		margin: theme.spacing.unit,
	},
	root: {
		width: '100%',
		margin: '10px',
	},
	paper: {
		width: '100%',
		marginBottom: theme.spacing(2),
	},
	visuallyHidden: {
		border: 0,
		clip: 'rect(0 0 0 0)',
		height: 1,
		margin: -1,
		overflow: 'hidden',
		padding: 0,
		position: 'absolute',
		top: 20,
		width: 1,
	},
	headerStyle: {
		color: '#fff !important',
		backgroundColor: '#3f51b5 !important',
	},
	media: {
		margin: 'auto',
		maxWidth: '100px',
	},
})

class CrmTable extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			page: 0,
			rowsPerPage: 10,
			selectedRow: '',
			order: 'asc',
			orderBy: 'Id',
			columns: [],
			allRows: [],
			rows: [],
			count: 0,
		}
	}

	componentDidMount() {
		const {columns, rows, url} = this.props
		if (url) {
			this.onGetData()
		} else {
			this.setState({
				columns: Array.isArray(columns) ? columns : [],
				allRows: Array.isArray(rows) ? rows : [],
				count: Array.isArray(rows) ? rows.length : 0,
			}, () => {
				if (Array.isArray(rows)) {
					this.createSortHandler(this.state.orderBy, true)
				}
			})
		}
	}

	componentDidUpdate(prevProps) {
		if (prevProps.rows != this.props.rows) {
			this.setState({
				allRows: Array.isArray(this.props.rows) ? this.props.rows : [],
				count: Array.isArray(this.props.rows) ? this.props.rows.length : 0,
			}, () => {
				if (this.props.url) {
					this.onGetData()
				} else if (Array.isArray(this.props.rows)) {
					this.createSortHandler(this.state.orderBy, true)
				}
			})
		} else if (!_.isEqual(prevProps.filterData, this.props.filterData) && this.props.url) {
			this.onGetData()
		}
	}

onGetData = () => {
	const {page, rowsPerPage, order, orderBy} = this.state
	const {token, url, columns, filterData, isLoaded, handleSnackbarOpen} = this.props

	if (isLoaded) {
		isLoaded(false)
	}
	let filter = {}
	if (filterData) {
		filter = {...filterData}
	}

	const postBody = {
		...filter,
		page,
		rowsPerPage,
		order,
		orderBy,
	}

	postRequest(url, token, postBody, result => {
		if (result && Array.isArray(result.data)) {
			this.setState({
				columns: Array.isArray(columns) ? columns : [],
				rows: result.data,
				count: result.rowsCount,
			}, () => {
				if (isLoaded) {
					isLoaded(true)
				}
			})
		} else if (isLoaded) {
			isLoaded(true)
		}
	}, error => {
		this.setState({
			page: 0,
			rowsPerPage: 10,
			selectedRow: '',
			order: 'asc',
			orderBy: 'Id',
			columns: [],
			rows: [],
			count: 0,
		}, () => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время выполнения запроса произошла ошибка: ${error}`, 'error')
			}
		})
	})
}

handleChangePage = (event, newPage) => {
	this.setState({
		page: newPage,
	}, () => {
		if (!this.props.handlePageChange && !this.props.url) {
			this.createSortHandler(this.state.orderBy, true)
		} else {
			if (this.props.url) {
				this.onGetData(this.props.url)
			}
			if (this.props.handlePageChange) {
				this.props.handlePageChange({...this.state})
			}
		}
	})
}

handleChangeRowsPerPage = event => {
	/* eslint-disable */
	this.setState({
		rowsPerPage: parseInt(event.target.value, 10),
		page: 0,
	}, () => {
		if (!this.props.handleRowsPerPageChange && !this.props.url) {
			this.createSortHandler(this.state.orderBy, true)
		} else {
			if (this.props.url) {
				this.onGetData(this.props.url)
			}
			if (this.props.handleRowsPerPageChange) {
				this.props.handleRowsPerPageChange({...this.state})
			}
		}
	})
	/* eslint-enable */
}

createSortHandler = (columnName, isNotSort) => {
	const isAsc = this.state.orderBy === columnName && this.state.order === 'asc'
	if ((!this.props.handleSortClick || isNotSort === true) && !this.props.url) {
		const sortedRows = this.stableSort(this.state.allRows, this.getComparator(this.state.order, this.state.orderBy))
			.slice(this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage)
		this.setState({
			order: isAsc ? 'desc' : 'asc',
			orderBy: columnName,
			rows: sortedRows,
		})
	} else {
		this.setState({
			order: isAsc ? 'desc' : 'asc',
			orderBy: columnName,
		}, () => {
			if (this.props.url) {
				this.onGetData(this.props.url)
			}
			if (this.props.handleSortClick) {
				this.props.handleSortClick({...this.state})
			}
		})
	}
}

getComparator = (order, orderBy) => order === 'desc'
	? (a, b) => this.descendingComparator(a, b, orderBy)
	: (a, b) => -this.descendingComparator(a, b, orderBy)

stableSort = (array, comparator) => {
	const stabilizedThis = array.map((el, index) => [el, index])
	stabilizedThis.sort((a, b) => {
		const order = comparator(a[0], b[0])
		if (order !== 0) return order
		return a[1] - b[1]
	})
	return stabilizedThis.map(el => el[0])
}

descendingComparator = (a, b, orderBy) => {
	if (b[orderBy] < a[orderBy]) {
		return -1
	}
	if (b[orderBy] > a[orderBy]) {
		return 1
	}
	return 0
}

handleEditClick = id => {
	if (this.props.handleEditClick) {
		this.props.handleEditClick(id)
	}
}

handleLaunchClick = row => {
	if (this.props.handleLaunchClick) {
		this.props.handleLaunchClick(row)
	}
}

handleDeleteClick = id => {
	if (this.props.handleDeleteClick) {
		this.props.handleDeleteClick(id)
	}
}

render() {
	const {classes, canOpen, canEdit, canDelete, tableContainerStyles, toAgreement} = this.props
	const {columns, rows, orderBy, order, page, rowsPerPage, count} = this.state
	/* eslint-disable */
	return (
		<Paper className={classes.root}>
			<TableContainer style={tableContainerStyles || {display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '80vh'}}>
				<Table stickyHeader size='small' aria-label='sticky table'>
					<TableHead>
						<TableRow>
							{canOpen || canEdit || canDelete
								? <TableCell style={{maxWidth: 100}} className={classes.headerStyle}></TableCell>
								: null}
							{columns.filter(c => (c.showFunc && c.showFunc(toAgreement)) || !c.showFunc).map(column => (
								<TableCell
									className={classes.headerStyle}
									key={column.id}
									align={column.align}
									style={{minWidth: column.minWidth}}
									sortDirection={orderBy === column.id && column.canSort ? order : false}
								>
									{column.canSort
										? <TableSortLabel
											active={orderBy === column.id}
											direction={orderBy === column.id ? order : 'asc'}
											onClick={() => this.createSortHandler(column.id)}
										>
											{column.label}
											{orderBy === column.id ? (
												<span className={classes.visuallyHidden}>
													{order === 'desc' ? 'sorted descending' : 'sorted ascending'}
												</span>
											) : null}
										</TableSortLabel>
										: column.label }
								</TableCell>
							))}
						</TableRow>
					</TableHead>
					<TableBody>
						{rows.map(row => (
							<TableRow
								hover
								// onClick={(event) => this.handleTableRowClick(event, row.name)}
								// aria-checked={this.isSelected(row.name)}
								// selected={this.isSelected(row.name)}
								role='checkbox'
								tabIndex={-1}
								key={row.id} >
								{canOpen || canEdit || canDelete
									? <TableCell>
										{ canOpen && <Button color='default' title='Открыть' onClick={() => this.handleLaunchClick(row)}>
											<LaunchIcon />
										</Button> }
										{ canEdit && <Button color='primary' title='Редактировать' onClick={() => this.handleEditClick(row.id)}>
											<EditIcon />
										</Button> }
										{ canDelete && <Button color='secondary' title='Удалить' onClick={() => this.handleDeleteClick(row.id)}>
											<DeleteIcon />
										</Button> }
									</TableCell>
									: null}
								{columns.filter(c => (c.showFunc && c.showFunc(toAgreement)) || !c.showFunc).map(column => (
									<TableCell key={column.id} align={column.align}>
										{
											column.id == 'photoB64' && row[column.id]
											? (<CardMedia component='img' className={classes.media} src={`data:image/jpeg;base64,${row[column.id]}`} title='Фото посетителя' />)
											: column.id == 'photoB64' && !row[column.id]
											? (<CardMedia component='img' className={classes.media} src={require('../../Static/important-person.jpg')} title='Фото посетителя' />)
											:(<Typography title={row[column.id]} noWrap>{row[column.id]}</Typography>)
										}
									</TableCell>
								))}
							</TableRow>
						))}
					</TableBody>
				</Table>
			</TableContainer>
			<TablePagination
				rowsPerPageOptions={[10, 25, 100]}
				component='div'
				count={count}
				rowsPerPage={rowsPerPage}
				page={page}
				onChangePage={this.handleChangePage}
				onChangeRowsPerPage={this.handleChangeRowsPerPage}
				labelRowsPerPage={''}
				labelDisplayedRows={({from, to, count}) => `Элементы ${from}-${to} из ${count !== -1 ? count : `${to} и больше`}`}
			/>
		</Paper>
	)
	/* eslint-enable */
}
}

export default withStyles(styles, {withTheme: true})(CrmTable)
