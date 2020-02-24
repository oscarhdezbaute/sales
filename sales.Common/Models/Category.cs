namespace sales.Common.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;    

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }

                return $"http://172.24.0.1/Sales.API/{this.ImagePath.Substring(1)}";
            }
        }

        public string ImageFullPathBackend
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }

                return $"http://localhost/Sales/{this.ImagePath.Substring(1)}";
            }
        }

        [NotMapped]
        public int ProductsCount { get; set; }
    }

}
