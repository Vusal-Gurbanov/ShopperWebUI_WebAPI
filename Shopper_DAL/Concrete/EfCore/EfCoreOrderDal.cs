using Shopper_DAL.Abstract;
using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_DAL.Concrete.EfCore
{
    public class EfCoreOrderDal:EfCoreGenericRepository<Order,DataContext>,IOrderDal
    {
    }
}
