using ECommerce.Core.Entities;
using ECommerce.Core.IRepositories;
using ECommerce.Core.Specifications;
using ECommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) ==typeof(Product))
                return (IReadOnlyList<T>) await _dbContext.Products.Include(P=>P.ProductBrand).Include(P=>P.ProductCategory).ToListAsync();  
            else
                return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();   
        }

        

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecficationEvaluator<T>.GetQuery(_dbContext.Set<T>(),spec);
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task Add(T entity)
        =>await _dbContext.Set<T>().AddAsync(entity);   
        

        public void Update(T entity)
         => _dbContext.Set<T>().Update(entity);
        public void Delete(T entity)
          => _dbContext.Set<T>().Remove(entity);    
    }
    
}
