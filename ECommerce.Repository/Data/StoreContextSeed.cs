using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task seedAsync (StoreContext context)
        {
            if(!context.ProductBrands.Any()) 
            {
                var brandsData = File.ReadAllText("../ECommerce.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if(brands is not null && brands.Count> 0 ) 
                {
                    foreach( var brand in brands )
                        await context.Set<ProductBrand>().AddAsync(brand);  
                   
                    await context.SaveChangesAsync();   
                }

            }


            if (!context.ProductCategories.Any())
            {
                var CategoriesData = File.ReadAllText("../ECommerce.Repository/Data/DataSeed/types.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);
                if (Categories is not null && Categories.Count > 0)
                {
                    foreach (var category in Categories)
                        await context.Set<ProductCategory>().AddAsync(category);
                   
                    await context.SaveChangesAsync();
                }

            }

            if (!context.Products.Any())
            {
                var ProductsData = File.ReadAllText("../ECommerce.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (products is not null && products.Count > 0)
                {
                    foreach (var product in products)
                        await context.Set<Product>().AddAsync(product);
                    
                    await context.SaveChangesAsync();
                }

            }



            if (!context.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../ECommerce.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods is not null && DeliveryMethods.Count > 0)
                {
                    foreach (var item in DeliveryMethods)
                        await context.Set<DeliveryMethod>().AddAsync(item);

                    await context.SaveChangesAsync();
                }

            }
        }
    }
}
