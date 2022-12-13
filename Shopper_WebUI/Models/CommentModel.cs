using Shopper_Entity;

namespace Shopper_WebUI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public DateTime CreateOn { get; set; }

    }
}
