using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_BLL.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
    }
}
