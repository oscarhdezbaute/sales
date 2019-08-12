namespace sales.Backend.Models
{
    using System.Web;
    using sales.Common.Models;    

    public class ProductView : Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}