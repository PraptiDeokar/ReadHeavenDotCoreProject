using Project.DataAccess.Data;
using Project.DataAccess.Repository.IRepository;
using Project.Models;
using Project.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{

		public readonly ApplicationDBContext _db;
		public OrderHeaderRepository(ApplicationDBContext db) : base(db)
		{
			_db = db;
		}


		public void Update(OrderHeader obj)
		{
			_db.OrderHeaders.Update(obj);
		}

		public void UpdatStatus(int id, string OrdeStatus, string? PaymentStatus = null)
		{
			var OrderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if (OrderFromDb != null){
				OrderFromDb.OrderStatus = OrdeStatus;
			    if( !string.IsNullOrEmpty(PaymentStatus)) {
				OrderFromDb.PaymentStatus= PaymentStatus;
				}
			}

		}
		public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
		{
			var OrderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
				OrderFromDb.SessionId = sessionId;
            }
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				OrderFromDb.PaymentIntentId = paymentIntentId;
				OrderFromDb.PaymentDate=DateTime.Now;
			}
		}


	}
}
