using UserStore.DAL.Entities;
using System;

namespace UserStore.DAL.Interfaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile item);
    }
}
