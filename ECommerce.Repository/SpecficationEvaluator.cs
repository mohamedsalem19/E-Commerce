using ECommerce.Core.Entities;
using ECommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository
{
    public static class SpecficationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        { 
           
            var query = inputQuery;   
           
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderByAsc is not null)
                query = query.OrderBy(spec.OrderByAsc);

            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPaginationEnabled)
                query= query.Skip(spec.Skip).Take(spec.Take);   


            query = spec.Includes.Aggregate(query,(currentQuery,includeExpression)=> currentQuery.Include(includeExpression));

            return query;
        }
    }
}
