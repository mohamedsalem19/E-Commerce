using ECommerce.Core.Entities;
using ECommerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);



        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);


        Task Add(T entity);
        void Update(T entity);  
        void Delete(T entity);    

        
    }
}
