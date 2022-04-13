import React from 'react'

export const appConstants = {

	/* eslint-disable*/
	serverUrl: process.env.NODE_ENV == 'development' ? 'https://localhost:5021' : '',
	/* eslint-enable*/
	requestHeaders: {
		'Content-Type': 'application/json',
		'Cache-Control': 'no-cache, no-store, must-revalidate',
		'Pragma': 'no-cache',
		'Expires': 0,
	},
}

export const treeData = [
	{
		title: '`title`',
		subtitle: '`subtitle`',
		expanded: true,
		noDragging: true,
		children: [
			{
				title: 'Child Node',
				subtitle: 'Defined in `children` array belonging to parent',
			},
			{
				title: 'Nested structure is rendered virtually',
				subtitle: (
					<span>
			The tree uses&nbsp;
						<a href='https://github.com/bvaughn/react-virtualized'>
				react-virtualized
						</a>
			&nbsp;and the relationship lines are more of a visual trick.
					</span>
				),
			},
		],
	},
]
