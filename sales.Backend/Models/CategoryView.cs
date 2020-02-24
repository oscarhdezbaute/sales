namespace sales.Backend.Models
{
    using sales.Common.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class CategoryView : Category
    {
        [Display(Name = "Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
