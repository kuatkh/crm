export const visitorsTableColumns = [
	{id: 'photoB64', label: 'Фото', minWidth: 100, showFunc: toAgreement => toAgreement != true},
	{id: 'iin', label: 'ИИН', minWidth: 100},
	{id: 'documentNumber', label: 'Номер\u00a0документа', minWidth: 100},
	{
		id: 'surnameRu',
		label: 'Фамилия',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
		canSort: true,
	},
	{
		id: 'nameRu',
		label: 'Имя',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
	},
	{
		id: 'middlenameRu',
		label: 'Отчество',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toFixed(2),
	},
	{id: 'jobPlaceName', label: 'Место\u00a0работы', minWidth: 100},
	{id: 'positionName', label: 'Должность', minWidth: 100},
]

export const cardsTableColumns = [
	{id: 'id', label: 'Номер пропуска', minWidth: 100, canSort: true},
	{id: 'authorName', label: 'Автор', minWidth: 100},
	{id: 'createdDateTimeStr', label: 'Дата\u00a0создания', minWidth: 100},
	{
		id: 'inviterName',
		label: 'Приглашающий',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
		canSort: true,
	},
	{
		id: 'justification',
		label: 'Комментарий',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
		canSort: true,
	},
	{
		id: 'visitorsCount',
		label: 'Количество\u00a0посетителей',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toFixed(2),
	},
	{
		id: 'status',
		label: 'Статус',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toFixed(2),
	},
]

export const addCardColumns = [
	{id: 'iin', label: 'ИИН', minWidth: 100},
	{id: 'documentNumber', label: 'Номер\u00a0документа', minWidth: 100},
	{
		id: 'surnameRu',
		label: 'Фамилия',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
		canSort: true,
	},
	{
		id: 'nameRu',
		label: 'Имя',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toLocaleString('en-US'),
	},
	{
		id: 'middlenameRu',
		label: 'Отчество',
		// minWidth: 170,
		align: 'center',
		// format: (value) => value.toFixed(2),
	},
	{id: 'jobPlaceName', label: 'Место\u00a0работы', minWidth: 100},
	{id: 'positionName', label: 'Должность', minWidth: 100},
]
