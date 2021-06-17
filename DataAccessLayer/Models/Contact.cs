using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DataAccessLayer.Models
{
    public partial class Contact
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [StringLength(30, ErrorMessage ="FirstName maximum allowed length is 30")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "LastName maximum allowed length is 30")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Email maximum allowed length is 30")]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [MinLength(10, ErrorMessage = "Phone Number must be 10 digits")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        
        public bool Status { get; set; }
    }
}
