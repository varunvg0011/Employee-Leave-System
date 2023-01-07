using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Employee_leave_system.Models;



namespace Employee_leave_system.Controllers
{
    
    public class AdminController : Controller
    {
        public FetchDataFromSQL fetchObj { get; set; } = new FetchDataFromSQL();
        public FetchEmpDataFromSQL fetchEmpObj { get; set; } = new FetchEmpDataFromSQL();
        public IActionResult AdminLogin()
        {           
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AdminLogin adminObj)
        {
            
            //check whether the entered details of admin matches from DB
            if (ModelState.IsValid)
            {               
                if (CheckAdminUsernameInDB(adminObj))
                {
                    HttpContext.Session.SetString("Username", adminObj.UserName);
                    HttpContext.Session.SetString("AdminRole", "Admin");

                    //never store unnecessary data in Session
                    //never store password as its of no use to be used in other controllers
                    //HttpContext.Session.SetString("adminPass", adminObj.Password);
                    if (CheckAdminPasswordInDB(adminObj))
                    {
                        
                        if (AdminLoginLog())
                        {
                            //return View("AllEmployees", AllEmployeeData);
                            return RedirectToAction("AllEmployees");
                        }
                        ViewBag.msgFailure = "Error entering the login log to Database. Please contact the admin!";
                        return View();
                    }
                    ViewBag.msgFailure = "Password incorrect. Enter correct credentials!";
                    return View();
                }
                ViewBag.msgFailure = "No admin present in database with that Username. Enter correct credentials!";
                return View();
            }
            ViewBag.msgFailure = "Enter the correct format for Admin details.";
            return View();
        }


        
        [AdminAttribute]
        public IActionResult AllEmployees()
        {
            List<EmployeeRegistration> AllEmployeeData = fetchObj.GetAllEmployees();
            return View(AllEmployeeData);
        }
        //validating admin username at login

        public bool CheckAdminUsernameInDB(AdminLogin adminObj)
        {
            
            if (fetchObj.CheckUsernameInDB(adminObj))
            {                
                return true;
            }
            return false;
        }
        //validating admin password at login
        public bool CheckAdminPasswordInDB(AdminLogin adminObj)
        {
            if (fetchObj.CheckPasswordInDB(adminObj))
            {
                return true;
            }
            return false;
        }


        public bool AdminLoginLog()
        {
            string adminUsername = HttpContext.Session.GetString("Username");
            AdminUserDetails AdminDetailsList = new AdminUserDetails();
            AdminDetailsList = fetchObj.GetAllAdminDetails(adminUsername);
            if (fetchObj.AddAdminLoginLogToDB(AdminDetailsList))
            {
                return true;
            }
            return false;
        }



        
        [AdminAttribute]
        public IActionResult AddEmployee()
        {

            return View();
        }


        
        
        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AdminLogin");
            //return View("AdminLogin");
        }

        
        [AdminAttribute]
        public IActionResult EditAdminProfile()
        {
            string adminUsername = HttpContext.Session.GetString("Username");
            AdminUserDetails AdminDetailsList = new AdminUserDetails();
            AdminDetailsList = fetchObj.GetAllAdminDetails(adminUsername);
            //ViewBag.AdminDetailsList = AdminDetailsList;
            return View(AdminDetailsList);
        }

        public bool UpdateAdminProfile(string firstName, string lastName, int age, string designation, string username)
        {
            bool isAdminDetailsUpdated = fetchObj.UpdateAdminDetails(firstName, lastName, age, designation, username);
            if (isAdminDetailsUpdated)
            {
                return true;
            }
            return false ;
        }

        [AdminAttribute]
        public IActionResult CancelUpdatingDetails()
        {
            return RedirectToAction("AllEmployees");
        }

        public bool AddEmployeeToDB(string username, string firstName, string lastName, char gender, string designation, int casualLeaves, int sickLeaves, int matternityLeaves, int patternityLeaves, string password, DateTime regDate, string imgData)
        {
            bool IsEmployeeAdded = fetchObj.AddEmpToDB( username,  firstName,  lastName,  gender,  designation,  casualLeaves,  sickLeaves,  matternityLeaves,  patternityLeaves,  password,  regDate, imgData);
            if (IsEmployeeAdded)
            {
                return true;
            }
            return false;
        }

        public IActionResult UnauthorizedAccess()
        {
            return View();
        }

        EmployeeController empObj = new EmployeeController();
        
        public IActionResult ManageLeaves()
        {
            
            List<AppliedEmployeeLeaves> leaveDetailsObj = fetchObj.GetAllEmpLeaveDetails();
             //fetchObj.GetAllEmpLeaveRequests();
             
            return View(leaveDetailsObj);
        }



    }
}
