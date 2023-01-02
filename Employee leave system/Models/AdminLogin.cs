using System.ComponentModel.DataAnnotations;


namespace Employee_leave_system.Models
{
    public class AdminLogin
    {
        
        [Required(ErrorMessage = "UserName field cannot be empty")]
        [Display(Name = "Enter your admin Username")]
        [RegularExpression("^[a-zA-Z0-9]{5,}", ErrorMessage = "Please enter correct format using only alphabets and numbers for username")]
        public string UserName { get; set; }

        [Display(Name ="Enter your password")]
        [Required(ErrorMessage ="Password field cannot be empty")]
        public string Password { get; set; }

        
        public bool RememberMe { get; set; }   
    }
}