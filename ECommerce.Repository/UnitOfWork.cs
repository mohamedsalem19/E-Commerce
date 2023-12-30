using ECommerce.Core;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.IRepositories;
using ECommerce.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;

        private Hashtable _repositories;
     
        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;

        }

        public IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories is null)
                _repositories = new Hashtable();    

            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbcontext);

                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
             => await _dbcontext.SaveChangesAsync();
        
        public async ValueTask DisposeAsync()
             => await _dbcontext.DisposeAsync();

       
    }
}
