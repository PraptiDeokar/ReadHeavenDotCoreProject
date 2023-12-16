using Project.DataAccess.Data;
using Project.DataAccess.Repository.IRepository;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.DataAccess.Repository
{
    public class CategoryRepository :Repository<Category>, ICategoryRepository
    {

        public readonly ApplicationDBContext _db;
        public CategoryRepository(ApplicationDBContext db) : base(db)
        {
            _db= db;
        }

     
        public void Update(Category obj)
        {
            _db.categories.Update(obj); 
        }
    }
}
