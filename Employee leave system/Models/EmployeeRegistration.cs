using System.ComponentModel.DataAnnotations;


namespace Employee_leave_system.Models
{
    public class EmployeeRegistration
    {
        public int EmpID { get; set; }

        [Required(ErrorMessage = "UserName field cannot be empty")]
        [Display(Name = "UserName:")]
        [RegularExpression("^[a-zA-z][a-zA-Z0-9_]{2,}", ErrorMessage = "Please enter correct format using only alphabets,numbers or '_' for username")]
        public string Username { get; set; }


        [Required(ErrorMessage = "First Name field cannot be empty")]
        [Display(Name = "First Name:")]
        [RegularExpression("^[a-zA-Z]{2,}", ErrorMessage = "Please enter correct format using only alphabets for First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name field cannot be empty")]
        [Display(Name = "Last Name:")]
        [RegularExpression("^[a-zA-Z]{1,}", ErrorMessage = "Please enter correct format using only alphabets for Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender field cannot be empty")]
        [Display(Name = "Gender:")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Designation field cannot be empty")]
        [Display(Name = "Designation:")]
        [RegularExpression("(^[A-Za-z]{3,16})([ ]{0,1})([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})", ErrorMessage = "Please enter correct format using only alphabets for Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Casual Leaves field cannot be empty")]
        [Display(Name = "No. of Casual Leaves:")]
        [RegularExpression("^[1-9]{1,}", ErrorMessage = "Please enter correct format.")]
        public int CasualLeaves { get; set; }


        [Required(ErrorMessage = "Sick Leaves field cannot be empty")]
        [Display(Name = "No. of Sick Leaves:")]
        [RegularExpression("^[1-9]{1,}", ErrorMessage = "Please enter correct format.")]
        public int SickLeaves { get; set; }


        [Required(ErrorMessage = "Matternity Leaves field cannot be empty")]
        [Display(Name = "No. of Matternity Leaves:")]
        [RegularExpression("^[1-9]{1,}", ErrorMessage = "Please enter correct format.")]
        public int MatternityLeaves { get; set; }


        [Required(ErrorMessage = "Patternity Leaves field cannot be empty")]
        [Display(Name = "No. of Patternity Leaves:")]
        [RegularExpression("^[1-9]{1,}", ErrorMessage = "Please enter correct format.")]
        public int PatternityLeaves { get; set; }

        [Required(ErrorMessage = "Password field cannot be empty")]
        [Display(Name = "Password:")]
        [RegularExpression("^[a-zA-Z0-9]{8,}", ErrorMessage = "Password must be of 8 characters at least and contain small and capital letters as well as numbers")]
        public string Password { get; set; }


        //this field needs to be disabled
        
        public DateTime RegistrationDate { get; set; }

        public string ImgData { get; set; }

    }
}
