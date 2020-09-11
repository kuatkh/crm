using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Services.Interfaces
{
    public interface ICacheManager
    {
        /// <summary>
        /// Генерация уникального ключа для объекта, где
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="keyField">Название поля идентификатор объекта</param>
        /// <returns></returns>
        string GenKey(object obj, string keyField);

        //// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        object Get(Type type, string id);

        /// <summary>
        /// Устанавливет кэш объекта  (obj,keyField)
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="keyField">Название поля идентификатор объекта</param>
        void Set(object obj, string keyField);

        /// <summary>
        /// Устанавливет кэш объекта (key,obj), где
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="obj">Объект</param>
        void Set(string key, object obj);

        /// <summary>
        /// Устанавливет кэш объекта (obj), где obj гарантированно имеет ключевое поле "Id"
        /// </summary>
        /// <param name="obj">Объект</param>
        void Set(object obj);

        /// <summary>
        /// Устанавливет кэш объекта (key, obj, timeSpan), где
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="obj">Объект</param>
        /// <param name="timeSpan">Срок, по истечению которого объект удалится из кэша</param>
        void Set(string key, object obj, TimeSpan expiration);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="key">Ключ</param>
        void Remove(string key);
        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="key">Ключ</param>
        void Remove(object obj);

        /// <summary>
        /// Удалить из списка
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="id">ИД</param>
        //void RemoveFromList(string key, int id);
        /// <summary>
        /// Очистить
        /// </summary>
        void Clear();
    }
}
