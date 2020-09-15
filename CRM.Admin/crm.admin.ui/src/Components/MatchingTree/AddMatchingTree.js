import React from 'react'
import {withStyles} from '@material-ui/core/styles'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogTitle from '@material-ui/core/DialogTitle'
import Button from '@material-ui/core/Button'
import IconButton from '@material-ui/core/IconButton'
import DeleteIcon from '@material-ui/icons/Delete'
import AddBoxIcon from '@material-ui/icons/AddBox'
import SaveIcon from '@material-ui/icons/Save'
import CancelIcon from '@material-ui/icons/Cancel'
import EditIcon from '@material-ui/icons/Edit'
import TextField from '@material-ui/core/TextField'
import Autocomplete from '@material-ui/lab/Autocomplete'
import CircularProgress from '@material-ui/core/CircularProgress'
import Paper from '@material-ui/core/Paper'
import Divider from '@material-ui/core/Divider'
import Typography from '@material-ui/core/Typography'
import Tooltip from '@material-ui/core/Tooltip'
import Grid from '@material-ui/core/Grid'
import Checkbox from '@material-ui/core/Checkbox'
import {green, purple} from '@material-ui/core/colors'
import AssignmentReturnedOutlinedIcon from '@material-ui/icons/AssignmentReturnedOutlined'
import AssignmentReturnedRoundedIcon from '@material-ui/icons/AssignmentReturnedRounded'
import SortableTree, {toggleExpandedForAll, getNodeAtPath, addNodeUnderParent, removeNodeAtPath, walk} from 'react-sortable-tree'
import {allConstants, treeData} from '../../Constants/AllConstants.js'
import {getRequest} from '../../Services/RequestsServices.js'
require('react-sortable-tree/style.css')
require('./treeStyle.css')

const styles = theme => ({
	container: {
		flexWrap: 'wrap',
		display: 'grid',
		gridTemplateColumns: 'repeat(12, 1fr)',
		gridGap: theme.spacing(1),
	},
	input: {
		margin: theme.spacing.unit,
		minWidth: '300px',
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
		minHeight: '50vh',
	},
	divider: {
		margin: 0,
	},
	// modalRoot: {
	// 	flexGrow: 1,
	// },
	actionButtons: {
		marginRight: theme.spacing(2),
	},
	headerStyle: {
		color: '#fff !important',
		backgroundColor: '#3f51b5 !important',
	},
	nodeBtns: {
		'& > *': {
			margin: theme.spacing(1),
		},
	},
	nodeBtnsBackground: {
		'& > *': {
			backgroundColor: green[100],
		},
	},
	root: {
		color: green[400],
		'&$checked': {
			color: green[600],
		},
	},
	checked: {
		color: green[800],
	},
	autocomplete: {
		minWidth: '300px',
	},
	editButton: {
		color: theme.palette.getContrastText(purple[500]),
		backgroundColor: purple[500],
		'&:hover': {
			backgroundColor: purple[700],
		},
	},
})

const maxDepth = 3

class AddMatchingTree extends React.Component {

	constructor(props) {
		super(props)
		this.state = {
			treeData,
			autocompleteOpen: false,
			autocompleteOpenIndex: null,
			autocompleteOptions: [],
			autocompleteSelectedOption: null,
			editMode: false,
		}

		this.regexStr = '^[0-9]*$'
		this.userSecretRegexStr = '^(?:[A-Za-z]+|\d+)$'
	}

	componentDidMount() {

	}

	getDepartments = () => {
		const {token, isLoaded, handleSnackbarOpen} = this.props
		if (isLoaded) {
			isLoaded(false)
		}

		getRequest(`${allConstants.serverUrl}/api/Admin/GetDepartments`, token, result => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (Array.isArray(result)) {
				var department = null
				if (this.state.departmentsId && this.state.departmentsId > 0 && result.length > 0 && result.some(r => r.id == this.state.departmentsId)) {
					department = result.filter(r => r.id == this.state.departmentsId)[0]
				}
				this.setState({
					departmentsOptions: result,
					selectedDepartment: department,
				})
			} else if (handleSnackbarOpen) {
				handleSnackbarOpen('Во время получения списка подразделений произошла ошибка', 'error')
			}
		},
		error => {
			if (isLoaded) {
				isLoaded(true)
			}
			if (handleSnackbarOpen) {
				handleSnackbarOpen(`Во время получения списка подразделений произошла ошибка: ${error}`, 'error')
			}
		})
	}

handleChange = e => {
	this.setState({...this.state, [e.target.name]: e.target.value})
}

handleTreeOnChange = data => {
	this.setState({treeData: [...data]})
}

handleCancelClick = () => {
	this.setState({
		treeData,
		autocompleteOpen: false,
		autocompleteOpenIndex: null,
		autocompleteOptions: [],
		autocompleteSelectedOption: null,
		editMode: false,
	})
	if (this.props.handleEditTreeDialogClose) {
		this.props.handleEditTreeDialogClose()
	}
}

toggleNodeExpansion = expanded => {
	this.setState(prevState => ({
		treeData: toggleExpandedForAll({treeData: [...prevState.treeData], expanded}),
	}))
}

getNodeKey = ({node: object, treeIndex: number}) => number

handleChangeIsSubstitute = (e, rowInfo) => {
	let newTree = [...this.state.treeData]
	walk({
		treeData: newTree,
		getNodeKey: ({treeIndex: number}) => number,
		ignoreCollapsed: false,
		callback: ({node, treeIndex, path}) => {
			if (rowInfo.treeIndex == treeIndex) {
				node.isSubstitute = e.target.checked
			}
		},
	})
	this.setState({treeData: [...newTree], editMode: false})
}

handleAutocompleteChange = (rowInfo, val) => {
	let newTree = [...this.state.treeData]
	walk({
		treeData: newTree,
		getNodeKey: ({treeIndex: number}) => number,
		ignoreCollapsed: false,
		callback: ({node, treeIndex, path}) => {
			if (rowInfo.treeIndex == treeIndex) {
				node.matchingUserId = val
			}
		},
	})
	this.setState({treeData: [...newTree]})
}

editNode = rowInfo => {
	let newTree = [...this.state.treeData]
	let selectedOption = null
	walk({
		treeData: newTree,
		getNodeKey: ({treeIndex: number}) => number,
		ignoreCollapsed: false,
		callback: ({node, treeIndex, path}) => {
			node.isEdit = rowInfo.treeIndex == treeIndex
			if (rowInfo.treeIndex == treeIndex && node.matchingUserId && node.matchingUserId > 0) {
				selectedOption = {
					id: node.matchingUserId,
					nameRu: node.matchingUserFullNameRu || '',
				}
			}
		},
	})
	this.setState({
		treeData: [...newTree],
		editMode: true,
		autocompleteOpen: false,
		autocompleteOpenIndex: null,
		autocompleteOptions: [],
		autocompleteSelectedOption: {...selectedOption},
	})
}

saveEditedMode = rowInfo => {
	let newTree = [...this.state.treeData]
	const {autocompleteSelectedOption} = this.state
	walk({
		treeData: newTree,
		getNodeKey: ({treeIndex: number}) => number,
		ignoreCollapsed: false,
		callback: ({node, treeIndex, path}) => {
			node.isEdit = false
			if (rowInfo.treeIndex == treeIndex) {
				node.matchingUserId = autocompleteSelectedOption ? autocompleteSelectedOption.id : 0
				node.matchingUserFullNameRu = autocompleteSelectedOption ? autocompleteSelectedOption.nameRu : ''
			}
		},
	})
	this.setState({
		treeData: [...newTree],
		editMode: false,
		autocompleteOpen: false,
		autocompleteOpenIndex: null,
		autocompleteOptions: [],
		autocompleteSelectedOption: null,
	})
}

addNode = rowInfo => {
	let NEW_NODE = {treeId: 0, parentId: null, cardsTypeId: null, matchingUserFullNameRu: '', matchingUserId: 0, noDragging: false, expanded: true}
	let {node, treeIndex, path} = rowInfo
	path.pop()
	let parentNode = getNodeAtPath({
		treeData: this.state.treeData,
		path,
		getNodeKey: ({treeIndex: number}) => number,
		ignoreCollapsed : true,
	})
	let parentKey = this.getNodeKey(parentNode)
	if (parentKey == -1) {
		parentKey = null
	}
	let newTree = addNodeUnderParent({
		treeData: this.state.treeData,
		newNode: NEW_NODE,
		expandParent: true,
		parentKey: treeIndex,
		getNodeKey: ({treeIndex: number}) => number,
	})
	this.setState({treeData: [...newTree.treeData]})
}

removeNode = rowInfo => {
	let {node, treeIndex, path} = rowInfo
	let newTree = removeNodeAtPath({
		treeData: this.state.treeData,
		path, // You can use path from here
		getNodeKey: ({node: TreeNode, treeIndex: number}) =>
		// console.log(number);
			number
		,
		ignoreCollapsed: false,
	})
	this.setState({treeData: [...newTree]})
}

handlInputChange = val => {
	setTimeout(() => {
		this.filterData(val)
	}, 1000)
}

handleAutocompleteOpen = () => {
	this.setState({autocompleteOpen: true})
}

handleAutocompleteClose = () => {
	this.setState({autocompleteOpen: false})
}

filterData = inputValue => {
	const {token} = this.props

	getRequest(`${allConstants.serverUrl}/api/Users/GetUsersBySearch?searchData=${inputValue}`, token, result => {
		this.setState({autocompleteOptions: result})
	},
	error => {
		console.log(error)
	})
}

render() {
	const {classes} = this.props
	const {treeData: data, autocompleteOpen, autocompleteOpenIndex, autocompleteOptions, autocompleteSelectedOption, editMode} = this.state
	return (
		<React.Fragment>
			<DialogTitle className={classes.headerStyle} id='add-tree-dialog-title'>Редактирование дерева согласования</DialogTitle>
			<DialogContent dividers={true}>
				<Grid container spacing={1}>
					<Grid item xs={12}>
						<Paper className={classes.paper}>
							<div className='wrapper'>
								<div className='tree-wrapper'>
									<SortableTree
										treeData={data}
										onChange={this.handleTreeOnChange}
										onMoveNode={({node, treeIndex, path}) =>
											global.console.debug(
												'node:',
												node,
												'treeIndex:',
												treeIndex,
												'path:',
												path
											)
										}
										maxDepth={maxDepth}
										canDrag={({node}) => !node.noDragging}
										canDrop={({nextParent}) => !nextParent || !nextParent.noChildren}
										isVirtualized={true}
										generateNodeProps={rowInfo => ({
											title: (<Tooltip title={`${rowInfo.node.matchingUserFullNameRu || ''}${rowInfo.node.isSubstitute == true ? ' (Замещающий)' : ''}`} aria-label='isSubstitute'>
												{
													rowInfo.node.isEdit
														? <Autocomplete
															id='matchingUserId'
															name='matchingUserId'
															fullWidth
															filterSelectedOptions
															className={classes.autocomplete}
															size='small'
															open={autocompleteOpen && autocompleteOpenIndex == rowInfo.treeIndex}
															onOpen={this.handleAutocompleteOpen}
															onClose={this.handleAutocompleteClose}
															options={autocompleteOptions}
															value={autocompleteSelectedOption}
															loading={autocompleteOpen && autocompleteOpenIndex == rowInfo.treeIndex && autocompleteOptions.length == 0}
															onChange={(e, v) => { this.handleAutocompleteChange(rowInfo, v) }}
															onInputChange={(e, v) => this.handlInputChange(v)}
															// getOptionSelected={(option, value) => option.name === value.name}
															getOptionLabel={option => option.nameRu}
															renderInput={params => <TextField
																{...params}
																label='Пользователь'
																variant='outlined'
																InputProps={{
																	...params.InputProps,
																	endAdornment: (
																		<React.Fragment>
																			{autocompleteOpen && autocompleteOpenIndex == rowInfo.treeIndex && autocompleteOptions.length == 0 ? <CircularProgress color='inherit' size={20} /> : null}
																			{params.InputProps.endAdornment}
																		</React.Fragment>
																	),
																}} />}
														/>
														: <TextField
															size='small'
															className={classes.input}
															fullWidth
															disabled
															value={rowInfo.node.matchingUserFullNameRu || ''}
															variant='outlined' />
												}
											</Tooltip>),
											subtitle: <Typography variant='button'>{rowInfo.node.matchingUserFullNameRu || ''}</Typography>,
											buttons: [
												<div className={`${classes.nodeBtns} ${rowInfo.node.isSubstitute == true ? classes.nodeBtnsBackground : null}`}>
													{
														rowInfo.node.isEdit
															? <Tooltip title='Сохранить' aria-label='remove'>
																<IconButton classes={{root: classes.editButton}} onClick={event => this.saveEditedMode(rowInfo)}>
																	<SaveIcon />
																</IconButton>
															</Tooltip>
															: !editMode
																? <Tooltip title='Редактировать' aria-label='remove'>
																	<IconButton classes={{root: classes.editButton}} onClick={event => this.editNode(rowInfo)}>
																		<EditIcon />
																	</IconButton>
																</Tooltip>
																: null
													}
													{
														!editMode
															? <React.Fragment>
																<Tooltip title={rowInfo.node.isSubstitute == true ? 'Замещающий' : 'Выбрать замещающим?'} aria-label='isSubstitute'>
																	<Checkbox
																		color='default'
																		onChange={e => this.handleChangeIsSubstitute(e, rowInfo)}
																		classes={{root: classes.root, checked: classes.checked}}
																		icon={<AssignmentReturnedOutlinedIcon />}
																		checkedIcon={<AssignmentReturnedRoundedIcon />} />
																</Tooltip>
																<Tooltip title='Удалить согласующих' aria-label='remove'>
																	<IconButton color='secondary' onClick={event => this.removeNode(rowInfo)}>
																		<DeleteIcon />
																	</IconButton>
																</Tooltip>
																<Tooltip title='Добавить согласующего' aria-label='add'>
																	<IconButton color='primary' onClick={event => this.addNode(rowInfo)}>
																		<AddBoxIcon />
																	</IconButton>
																</Tooltip>
															</React.Fragment>
															: null
													}
												</div>,
											],
											style: {
												height: '50px',
											},
										})}
									/>
								</div>
							</div>
						</Paper>
					</Grid>
				</Grid>
			</DialogContent>
			<DialogActions>
				<Button onClick={this.handleCancelClick} startIcon={<CancelIcon />} className={classes.actionButtons} variant='outlined' size='medium' color='secondary'>
					Отменить
				</Button>
				<Button onClick={this.handleCancelClick} startIcon={<SaveIcon />} disabled={editMode} className={classes.actionButtons} variant='outlined' size='medium' color='primary'>
					Сохранить
				</Button>
			</DialogActions>
		</React.Fragment>
	)
}
}

export default withStyles(styles, {withTheme: true})(AddMatchingTree)
