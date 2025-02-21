using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyBookWebRazor_Temp.Models
{
    public class Category
    {
        //to uniquely seperate entities
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public String Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
