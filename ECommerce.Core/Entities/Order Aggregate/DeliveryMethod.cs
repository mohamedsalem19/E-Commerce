using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity
    {

        public DeliveryMethod()
        {

        }

        public DeliveryMethod(string shortName, string description, decimal cost, string delievryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DelievryTime = delievryTime;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DelievryTime { get; set; }
        
    }
}
