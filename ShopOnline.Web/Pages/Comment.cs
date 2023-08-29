namespace ShopOnline.Web.Pages
{

        public class Comment
        {
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Comment> Replies { get; set; }
        public bool ShowReplyForm { get; set; }
        

        public Comment()
        {
            Author = "Taha";
            Timestamp = DateTime.Now;
            Replies = new List<Comment>();
           
        }
    }
}
