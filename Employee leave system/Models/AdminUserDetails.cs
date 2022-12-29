using System.ComponentModel.DataAnnotations;



namespace Employee_leave_system.Models
{
    public class AdminUserDetails
    {
        public int AdminID { get; set; }



        [Required(ErrorMessage = "First Name field cannot be empty")]
        [Display(Name = "First Name:")]
        [RegularExpression("^[a-zA-Z]{2,}", ErrorMessage = "Please enter correct format using only alphabets for First Name")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "Last Name field cannot be empty")]
        [Display(Name = "Last Name:")]
        [RegularExpression("^[a-zA-Z]{1,}", ErrorMessage = "Please enter correct format using only alphabets for Last Name")]
        public string LastName { get; set; }



        [Required(ErrorMessage = "Designation field cannot be empty")]
        [Display(Name = "Designation:")]
        [RegularExpression("(^[A-Za-z]{3,16})([ ]{0,1})([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})?([ ]{0,1})?([A-Za-z]{3,16})", ErrorMessage = "Please enter correct format using only alphabets for Designation")]
        public string Designation { get; set; }



        public int Age { get; set; }

        //[Required(ErrorMessage = "Gender field cannot be empty")]
        //[Display(Name = "Gender:")]
        public char Gender { get; set; }


        [Display(Name= "Registration Date:")]
        public DateTime RegistrationDate { get; set; }



        //[Required(ErrorMessage = "UserName field cannot be empty")]
        //[Display(Name = "UserName:")]
        //[RegularExpression("^[a-zA-Z0-9]{5,}", ErrorMessage = "Please enter correct format using only alphabets and numbers for username")]
        public string Username { get; set; }




        




        

        
    }
}
