using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderByAsc { get ; set ; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }
        public int Skip { get ; set  ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnabled { get ; set ; }

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
               
        }

        public void AddOrderByAsc(Expression<Func<T, object>> orderByExprssion)
        {
            OrderByAsc = orderByExprssion;  
        }

        public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExprssion)
        {
            OrderByDesc = orderByDescExprssion;
        }


        public void ApplyPagination(int skip , int take)
        {
            IsPaginationEnabled = true; 
            Skip= skip; 
            Take= take; 
            
        }

    }

}
