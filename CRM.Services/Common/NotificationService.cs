using CRM.DataModel.Data;
using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using CRM.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Common
{
    public class NotificationService : INotificationService
    {
        private readonly CrmDbContext _abContext;
        private static ILogger<NotificationService> _logger;

        public NotificationService(CrmDbContext abContext, ILogger<NotificationService> logger)
        {
            _abContext = abContext;
            _logger = logger;
        }

        public async Task<List<Notifications>> GetNotifications(long receiverId, bool isAll = false)
        {
            try
            {
                var result = await _abContext.Notifications
                    .Where(n => n.ReceiverId == receiverId && n.DeletedDateTime == null && (isAll || !isAll && n.ReadDateTime == null))
                    .AsNoTracking()
                    .Select(n => new Notifications()
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Description = n.Description
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetNotifications. MSG: {JsonConvert.SerializeObject(ex)}");
                return null;
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }

        public async Task<long> GetNotificationsCount(long receiverId, bool isAll = false)
        {
            try
            {
                var result = await _abContext.Notifications
                    .AsNoTracking()
                    .CountAsync(n => n.ReceiverId == receiverId && n.DeletedDateTime == null && (isAll || !isAll && n.ReadDateTime == null));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR GetNotificationsCount. MSG: {JsonConvert.SerializeObject(ex)}");
                return 0;
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }

        public async Task AddNotification(long receiverId, string title, string description)
        {
            try
            {
                await _abContext.Notifications.AddAsync(new Notifications() {
                    CreatedDateTime = DateTime.Now,
                    ReceiverId = receiverId,
                    Title = title,
                    Description = description
                });
                await _abContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"ERROR AddNotification. MSG: {JsonConvert.SerializeObject(ex)}");
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }

        public async Task ReadNotification(long notificationId)
        {
            try
            {
                var notification = await _abContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
                if (notification != null)
                {
                    notification.ReadDateTime = DateTime.Now;
                    _abContext.Notifications.Update(notification);
                    await _abContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR ReadNotification. MSG: {JsonConvert.SerializeObject(ex)}");
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }

        public async Task DeleteNotification(long notificationId)
        {
            try
            {
                var notification = await _abContext.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
                if (notification != null)
                {
                    notification.DeletedDateTime = DateTime.Now;
                    _abContext.Notifications.Update(notification);
                    await _abContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR DeleteNotification. MSG: {JsonConvert.SerializeObject(ex)}");
            }
            finally
            {
                await _abContext.DisposeAsync();
            }
        }
    }
}
