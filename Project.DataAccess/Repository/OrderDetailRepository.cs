﻿using Project.DataAccess.Data;
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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {

        public readonly ApplicationDBContext _db;
        public OrderDetailRepository(ApplicationDBContext db) : base(db)
        {
            _db= db;
        }

     
        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj); 
        }
    }
}
