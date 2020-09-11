using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.Services.Interfaces
{
    public interface ISeedDefaultData : IDisposable
    {
        void Seed();
    }
}
