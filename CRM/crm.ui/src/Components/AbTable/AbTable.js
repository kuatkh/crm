import React from 'react'
import {withStyles, makeStyles} from '@material-ui/core/styles'
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
import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown'
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp'
import CardMedia from '@material-ui/core/CardMedia'
import Typography from '@material-ui/core/Typography'
import {connect} from 'react-redux'
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
		margin: theme.spacing(1),
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

class AbTable extends React.Component {
	_isMounted = false

	constructor(props) {
		super(props)
		this.state = {
			page: 0,
			rowsPerPage: 10,
			selectedRow: '',
			order: 'asc',
			orderBy: 'id',
			columns: [],
			allRows: [],
			rows: [],
			count: 0,
			openedRows: [],
		}
	}

	componentDidMount() {
		this._isMounted = true

		const {columns, rows, url, setGetDataFunc} = this.props

		if (this._isMounted) {
			this.setState({
				columns: Array.isArray(columns) ? columns : [],
			})
		}

		if (url) {
			this.onGetData()
		} else {
			this.setState({
				allRows: Array.isArray(rows) ? rows : [],
				count: Array.isArray(rows) ? rows.length : 0,
			}, () => {
				if (Array.isArray(rows)) {
					this.createSortHandler(this.state.orderBy, true)
				}
			})
		}

		if (setGetDataFunc) {
			setGetDataFunc(this.onGetData)
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

	componentWillUnmount() {
		this._isMounted = false
	}

onGetData = () => {
	const {page, rowsPerPage, order, orderBy} = this.state
	const {token, url, rows, filterData, isLoaded, handleSnackbarOpen, defaultOrderByColumn} = this.props

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
		orderBy: orderBy != 'id' ? orderBy : defaultOrderByColumn,
	}

	postRequest(url, token, postBody, result => {
		if (this._isMounted) {
			if (isLoaded) {
				isLoaded(true)
			}
			if (result && Array.isArray(result.data)) {
				this.setState({
					rows: result.data,
					count: result.rowsCount,
				})
			} else {
				this.setState({
					allRows: Array.isArray(rows) ? rows : [],
					count: Array.isArray(rows) ? rows.length : 0,
				})
			}
		}
	},
	error => {
		if (this._isMounted) {
			if (isLoaded) {
				isLoaded(true)
			}
			this.setState({
				page: 0,
				rowsPerPage: 10,
				selectedRow: '',
				order: 'asc',
				orderBy: 'Id',
				rows: [],
				count: 0,
				openedRows: [],
			}, () => {
				if (handleSnackbarOpen) {
					handleSnackbarOpen(`Во время выполнения запроса произошла ошибка: ${error}`, 'error')
				}
			})
		}
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

handleEditClick = row => {
	if (this.props.handleEditClick) {
		this.props.handleEditClick(row)
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

handleOpenRowClick = id => {
	let openedRowsArr = [...this.state.openedRows]
	if (openedRowsArr.some(r => r == id)) {
		var index = openedRowsArr.indexOf(id)
		if (index !== -1) {
			openedRowsArr.splice(index, 1)
			this.setState({
				openedRows: [...openedRowsArr],
			})
		}
	} else {
		this.setState({
			openedRows: [...openedRowsArr, id],
		})
	}
}

render() {
	const {classes, canOpen, canEdit, canExpand, canDelete, tableContainerStyles, tableStyle} = this.props
	const {columns, childrenColumns, childrenHeader, rows, orderBy, order, page, rowsPerPage, count, openedRows} = this.state
	/* eslint-disable */
	return (
		<Paper className={classes.root}>
			<TableContainer style={tableContainerStyles || {display: 'flex', flexWrap: 'wrap', minHeight: '10vh', maxHeight: '80vh'}}>
				<Table stickyHeader size='small' aria-label='sticky table' style={tableStyle || {}}>
					<TableHead>
						<TableRow>
							{canOpen || canEdit || canDelete || canExpand
								? <TableCell style={{maxWidth: 100}} className={classes.headerStyle}></TableCell>
								: null}
							{columns.map(column => (
								<TableCell
									className={classes.headerStyle}
									key={'column' + column.id}
									align={column.align}
									style={{minWidth: column.minWidth, maxWidth: column.maxWidth}}
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
							<React.Fragment key={'row-' + row.id}>
								<TableRow
									hover
									// onClick={(event) => this.handleTableRowClick(event, row.name)}
									// aria-checked={this.isSelected(row.name)}
									// selected={this.isSelected(row.name)}
									role='checkbox'
									tabIndex={-1} >
									{canOpen || canEdit || canDelete || canExpand
										? <TableCell>
											{ canOpen && <Button color='default' title='Открыть' onClick={() => this.handleLaunchClick(row)}>
												<LaunchIcon />
											</Button> }
											{ canEdit && <Button color='primary' title='Редактировать' onClick={() => this.handleEditClick(row)}>
												<EditIcon />
											</Button> }
											{ canDelete && <Button color='secondary' title='Удалить' onClick={() => this.handleDeleteClick(row.id)}>
												<DeleteIcon />
											</Button> }
											{ canExpand && Array.isArray(row.children) && row.children.length > 0 && Array.isArray(childrenColumns) &&
											 <Button color='secondary' title='Удалить' onClick={() => this.handleOpenRowClick(row.id)}>
												{openedRows.some(r => r == row.id) ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
											</Button> }
										</TableCell>
										: null}
									{columns.map(column => (
										<TableCell key={row.id + '-' + column.id} align={column.align}>
											{
												column.id == 'photoB64' && row[column.id]
												? (<CardMedia component='img' className={classes.media} src={`data:image/jpeg;base64,${row[column.id]}`} title='Фото пользователя' />)
												: column.id == 'photoB64' && !row[column.id]
												? (<CardMedia component='img' className={classes.media} src={require('../../Static/important-person.jpg')} title='Фото пользователя' />)
												:(<Typography title={row[column.id]} noWrap={!!column.noWrap}>{row[column.id]}</Typography>)
											}
										</TableCell>
									))}
								</TableRow>
								{
									canExpand && Array.isArray(row.children) && row.children.length > 0 && Array.isArray(childrenColumns) && (
										<TableRow>
											<TableCell style={{paddingBottom: 0, paddingTop: 0}} colSpan={6}>
												<Collapse in={openedRows.some(r => r == id)} timeout='auto' unmountOnExit>
													<Box margin={1}>
														<Typography variant='h6' gutterBottom component='div'>
															{childrenHeader || ''}
														</Typography>
														<Table size='small' aria-label='purchases'>
															<TableHead>
																<TableRow>
																	{canOpen || canEdit || canDelete
																		? <TableCell style={{maxWidth: 100}} className={classes.headerStyle}></TableCell>
																		: null}
																	{childrenColumns.map(childColumn => (
																		<TableCell
																			className={classes.headerStyle}
																			key={'child-' + row.id + '-' + childColumn.id}
																			align={childColumn.align}
																			style={{minWidth: childColumn.minWidth, maxWidth: childColumn.maxWidth}}
																		>
																			{childColumn.label}
																		</TableCell>
																	))}
																</TableRow>
															</TableHead>
															<TableBody>
																{row.children.map(childRow => (
																	<TableRow
																		hover
																		role='checkbox'
																		tabIndex={-1}
																		key={'child-row-' + childRow.id + '-' + childColumn.id} >
																		{canOpen || canEdit || canDelete
																			? <TableCell>
																				{ canOpen && <Button color='default' title='Открыть' onClick={() => this.handleLaunchClick(childRow)}>
																					<LaunchIcon />
																				</Button> }
																				{ canEdit && <Button color='primary' title='Редактировать' onClick={() => this.handleEditClick(childRow.id)}>
																					<EditIcon />
																				</Button> }
																				{ canDelete && <Button color='secondary' title='Удалить' onClick={() => this.handleDeleteClick(childRow.id)}>
																					<DeleteIcon />
																				</Button> }
																			</TableCell>
																			: null}
																		{childColumn.map(childColumn => (
																			<TableCell key={'child-cell-' + childRow.id + '-' + childColumn.id} align={childColumn.align}>
																				{
																					childColumn.id == 'photoB64' && childRow[childColumn.id]
																					? (<CardMedia component='img' className={classes.media} src={`data:image/jpeg;base64,${childRow[childColumn.id]}`} title='Фото пользователя' />)
																					: childColumn.id == 'photoB64' && !childRow[childColumn.id]
																					? (<CardMedia component='img' className={classes.media} src={require('../../Static/important-person.jpg')} title='Фото пользователя' />)
																					:(<Typography title={childRow[childColumn.id]} noWrap={!!childColumn.noWrap}>{childRow[childColumn.id]}</Typography>)
																				}
																			</TableCell>
																		))}
																	</TableRow>
																))}
															</TableBody>
														</Table>
													</Box>
												</Collapse>
											</TableCell>
										</TableRow>
									)
								}
							</React.Fragment>
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

function mapStateToProps(state) {
	const {currentUser, token} = state
	return {
		currentUser,
		token,
	}
}

export default connect(mapStateToProps)(withStyles(styles, {withTheme: true})(AbTable))
