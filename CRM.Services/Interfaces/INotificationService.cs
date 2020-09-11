using CRM.DataModel.Dto;
using CRM.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notifications>> GetNotifications(long receiverId, bool isAll = false);

        Task<long> GetNotificationsCount(long receiverId, bool isAll = false);

        Task AddNotification(long receiverId, string title, string description);

        Task ReadNotification(long notificationId);

        Task DeleteNotification(long notificationId);
    }
}
