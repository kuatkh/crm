using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
    }
}
