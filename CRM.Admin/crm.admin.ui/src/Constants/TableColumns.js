export const usersColumns = [
	{id: 'userName', label: 'Логин', minWidth: 100},
	{
		id: 'surname',
		label: 'Фамилия',
		align: 'center',
		canSort: true,
		noWrap: true,
	},
	{
		id: 'name',
		label: 'Имя',
		align: 'center',
		canSort: true,
		noWrap: true,
	},
	{
		id: 'middlename',
		label: 'Отчество',
		align: 'center',
		canSort: true,
		noWrap: true,
	},
	{id: 'iin', label: 'ИИН', minWidth: 100, canSort: true},
	{id: 'departmentName', label: 'Сруктурное подразделение', minWidth: 100, align: 'center', noWrap: true},
	{id: 'positionName', label: 'Должность', minWidth: 100, align: 'center', noWrap: true},
	{id: 'email', label: 'Email', minWidth: 100, canSort: true, noWrap: true},
	{id: 'birthDateStr', label: 'Дата рождения', minWidth: 100, noWrap: true},
	{id: 'roleName', label: 'Роль', minWidth: 100, maxWidth: 150, align: 'center', noWrap: true},
]

export const dictionariesColumns = {
	DictCountries: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
	],
	DictCities: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'parentName', label: 'Страна (рус.)', minWidth: 100, canSort: true},
	],
	DictDepartments: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'parentName', label: 'Компания/филиал (рус.)', minWidth: 100, canSort: true},
	],
	DictPositions: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'positionCategory', label: 'Категория', minWidth: 100},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'parentName', label: 'Компания/филиал (рус.)', minWidth: 100, canSort: true},
		{id: 'description', label: 'Описание (рус.)', minWidth: 100, canSort: true},
	],
	DictServices: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'amount', label: 'Цена', minWidth: 100},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'parentName', label: 'Структурное подразделение (рус.)', minWidth: 100, canSort: true},
		{id: 'description', label: 'Описание (рус.)', minWidth: 100, canSort: true},
	],
	DictIntolerances: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'description', label: 'Описание (рус.)', minWidth: 100, canSort: true},
	],
	DictGenders: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
	],
	DictLoyaltyPrograms: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'amount', label: 'Значение скидки (%)', minWidth: 100},
		{id: 'description', label: 'Описание (рус.)', minWidth: 100, canSort: true},
	],
	DictStatuses: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
	],
	DictEnterprises: [
		{id: 'id', label: 'Идентификатор', minWidth: 100, canSort: true},
		{id: 'name', label: 'Название (рус.)', align: 'center', minWidth: 100, canSort: true},
		{id: 'createdDateTimeStr', label: 'Дата создания', minWidth: 100},
		{id: 'phoneNumber', label: 'Номер телефона', minWidth: 100, canSort: true},
		{id: 'address', label: 'Адрес', minWidth: 100, canSort: true},
	],
}
