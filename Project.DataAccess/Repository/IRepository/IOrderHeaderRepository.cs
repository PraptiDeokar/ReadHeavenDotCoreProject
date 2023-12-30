using Project.Models;
using Project.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id,string OrdeStatus,string? PaymentStatus=null);
        void UpdateStripePaymentID(int id,string sessionId,string paymentIntentId);
       // void Save();
    }
}
