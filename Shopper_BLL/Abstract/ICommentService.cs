using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_BLL.Abstract
{
    public interface ICommentService
    {
        void Create(Comment entity);
        void Update(Comment entity);
        void Delete(Comment entity);

        Comment GetById(int id);
    }
}
