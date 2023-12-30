using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications
{
    public class ProductWithBrandAndCategorySpecification :BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpecification(ProductSpecParams specParams) 
            :base(P=>
                     (string.IsNullOrEmpty(specParams.Search)||P.Name.ToLower().Contains(specParams.Search) )&&
                     (!specParams.BrandId.HasValue    || P.ProductBrandId == specParams.BrandId.Value)&&
                     (!specParams.CategoryId.HasValue || P.ProductCategoryId == specParams.CategoryId.Value)
                 
                 )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductCategory);

            AddOrderByAsc(P=>P.Name);

            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort) 
                {
                    case "priceAsc":
                        AddOrderByAsc(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);   
                        break;
                    default :
                        AddOrderByAsc(P => P.Name);    
                        break;
                }
            }

            ApplyPagination(specParams.PageSize*(specParams.PageIndex -1), specParams.PageSize);

        }

        public ProductWithBrandAndCategorySpecification(int id): base(P=>P.Id ==id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductCategory);

        }
    }
}
