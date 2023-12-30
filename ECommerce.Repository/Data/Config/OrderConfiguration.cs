using ECommerce.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(O => O.Status)
                .HasConversion(

                    Ostatus => Ostatus.ToString(),
                    Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus) , Ostatus)

                );

            builder.Property(O=>O.Subtotal).HasColumnType("decimal(18,2)"); 
            
           
        }
    }
}

