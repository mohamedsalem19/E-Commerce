using ECommerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIdWithSpecifcation : BaseSpecification<Order>
    {
        public OrderWithPaymentIdWithSpecifcation(string paymentintentId)
            :base(O => O.PaymentIntentId == paymentintentId)
        {

        }
    }
}
