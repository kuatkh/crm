using CRM.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Services.Common
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;
        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Генерация уникального ключа для объекта
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="keyField">Название поля идентификатор объекта</param>
        /// <returns></returns>
        public string GenKey(object obj, string keyField)
        {
            if (obj == null)
            {
                throw new ArgumentException($"Object is null!!!");
            }
            var type = obj.GetType();
            var field = type.GetProperty(keyField);
            if (field == null)
            {
                throw new ArgumentException($"Object doesn't contain {keyField} field!!!");
            }
            var id = field.GetValue(obj);
            return (type.Name + id);
        }

        /// <summary>
        /// Устанавливет кэш объекта  (obj,keyField)
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="keyField">Название поля идентификатор объекта</param>
        public void Set(object obj, string keyField)
        {
            var key = GenKey(obj, keyField);
            _cache.Set(key, obj, DateTimeOffset.UtcNow.AddDays(1));
        }

        /// <summary>
        /// Устанавливет кэш объекта (obj), где obj гарантированно имеет ключевое поле "Id"
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="expiration">Срок, по истечению которого объект удалится из кэша</param>
        public void Set(object obj, string keyField, DateTimeOffset expiration)
        {
            var key = GenKey(obj, keyField);
            _cache.Set(key, obj, expiration);
        }

        /// <summary>
        /// Устанавливет кэш объекта (obj), где obj гарантированно имеет ключевое поле "Id"
        /// </summary>
        /// <param name="obj">Объект</param>
        public void Set(object obj)
        {
            var key = GenKey(obj, "Id");
            TimeSpan timeSpan = new TimeSpan(1, 0, 0);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(timeSpan);
            _cache.Set(key, obj, cacheEntryOptions);
        }

        /// <summary>
        /// Устанавливет кэш объекта (key, obj, timeSpan), где
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="obj">Объект</param>
        /// <param name="timeSpan">Срок, по истечению которого объект удалится из кэша</param>
        public void Set(string key, object obj, TimeSpan timeSpan)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(timeSpan);
            _cache.Set(key, obj, cacheEntryOptions);
        }
        /// <summary>
        /// Устанавливет кэш объекта (key, obj), где
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="obj">Объект</param>
        public void Set(string key, object obj)
        {
            TimeSpan timeSpan = new TimeSpan(1, 0, 0);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(timeSpan);
            _cache.Set(key, obj, cacheEntryOptions);
        }

        /// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return _cache.Get(key);
        }
        /// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public object Get(Type type, string id)
        {
            return _cache.Get(type.Name + id);
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Remove(object obj)
        {
            var key = GenKey(obj, "Id");
            _cache.Remove(key);
        }

        /// <summary>
        /// Очистить
        /// </summary>
        public void Clear()
        {
            _cache.Dispose();
        }
    }
}
