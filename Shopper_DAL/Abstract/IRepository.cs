using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_DAL.Abstract
{
    public interface IRepository<T> // T generic yapıları tanımlar.
    {
        T GetById(int id);
        T Find(Expression<Func<T, bool>> filter);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);


        /*
      ToList() => Listeleme işlemini C# tarafında gerçekleştirir.

      IQueryable()
                      => Listeleme işlemini SQL de gerçekleştirip ramde bir yansımasını tutar.
      IEnumarable() 

       */
    }
}
