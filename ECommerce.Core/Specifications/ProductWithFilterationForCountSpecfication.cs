using ECommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications
{
    public class ProductWithFilterationForCountSpecfication : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecfication(ProductSpecParams specParams)
         : base(P =>
                     (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                     (!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
                     (!specParams.CategoryId.HasValue || P.ProductCategoryId == specParams.CategoryId.Value)

               )
        {

        }
    }
}
