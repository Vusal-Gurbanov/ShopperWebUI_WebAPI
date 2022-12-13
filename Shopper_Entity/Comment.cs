using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
