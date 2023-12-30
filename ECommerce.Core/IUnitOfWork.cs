using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core
{
    public interface IUnitOfWork : IAsyncDisposable 
    {
        IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> Complete();
    }
}
