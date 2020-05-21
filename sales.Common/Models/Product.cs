namespace sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]        
        public DateTime PublishOn { get; set; }

        [NotMapped] //significa que lo voy a tener en el modelo, pero no se va a crear en la tabla de la bd
        public byte[] ImageArray { get; set; }

        [Display(Name = "Image")]
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

        [JsonIgnore]
        public virtual Category Category { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
