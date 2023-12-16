using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreAppWithEntityfw1.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "maximum {1} characters allowed")]
        [DisplayName("Category Name")]
        public string  Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order must be between 1-100")]
        public int Display_Order { get; set; }
    }
}
