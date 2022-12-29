using Microsoft.AspNetCore.Mvc;
using Employee_leave_system.Models;
using System.Text;

namespace Employee_leave_system.Controllers
{
    
    public class EmployeeController : Controller
    {
        FetchEmpDataFromSQL fetchEmpObj = new FetchEmpDataFromSQL();
        public IActionResult EmpLogin()
        {
            if(Request.Cookies["empUsername"] != null && Request.Cookies["empPassword"] != null)
            {
                string encryptedPassword = Request.Cookies["empPassword"];
                byte[] b = Convert.FromBase64String(encryptedPassword);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.Username = Request.Cookies["empUsername"];
                ViewBag.Password = decryptedPassword;

            }
            return View();
        }

        [HttpPost]
        public IActionResult EmpLogin(EmpLogin empLoginObj)
        {
            if (ModelState.IsValid)
            {
                if (CheckEmployeeUsernameInDB(empLoginObj))
                {
                    CookieOptions cookie = new CookieOptions();

                    //never store unnecessary data in Session
                    //never store password as its of no use to be used in other controllers
                    //HttpContext.Session.SetString("adminPass", adminObj.Password);
                    if (CheckAdminPasswordInDB(empLoginObj))
                    {
                        if (empLoginObj.RememberMe)
                        {
                            Response.Cookies.Append("empUsername", empLoginObj.UserName, cookie);
                            //decrypting the password for security purposes
                            byte[] b = ASCIIEncoding.ASCII.GetBytes(empLoginObj.Password);
                            string encryptedPassword = Convert.ToBase64String(b);
                            Response.Cookies.Append("empPassword", encryptedPassword, cookie);

                            cookie.Expires = DateTime.Now.AddDays(2);
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                        }
                        HttpContext.Session.SetString("Username", empLoginObj.UserName);
                        HttpContext.Session.SetString("EmpRole", "Employee");
                        if (EmployeeLoginLog())
                        {
                            //return View("AllEmployees", AllEmployeeData);
                            return RedirectToAction("EmployeeDashboard");
                        }
                        ViewBag.msgFailure = "Error entering the login log to Database. Please contact the admin!";
                        return View();
                    }
                    ViewBag.msgFailure = "Password incorrect. Enter correct credentials!";
                    return View();
                }
                ViewBag.msgFailure = "No Employee present in database with that Username. Enter correct credentials!";
                return View();
            }
            ViewBag.msgFailure = "Enter the correct format for Employee details.";
            return View();
            
        } 

        [EmployeeAttribute]
        public IActionResult EmployeeDashboard()
        {
            return View();
        }


        public bool CheckEmployeeUsernameInDB(EmpLogin empLoginObj)
        {
            if (fetchEmpObj.CheckEmpUsernameInDB(empLoginObj))
            {
                return true;
            }
            return false;
        }


        public bool CheckAdminPasswordInDB(EmpLogin empLoginObj)
        {
            if (fetchEmpObj.CheckPasswordInDB(empLoginObj))
            {
                return true;
            }
            return false;
        }


        public bool EmployeeLoginLog()
        {
            string employeeUsername = HttpContext.Session.GetString("Username");
            EmployeeRegistration empDetailsList = new EmployeeRegistration();
            empDetailsList = fetchEmpObj.GetAllEmpDetails(employeeUsername);
            if (fetchEmpObj.AddEmpLoginLogToDB(empDetailsList))
            {
                return true;
            }
            return false;
        }

        [EmployeeAttribute]
        public IActionResult EmployeeLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("EmpLogin");
        }

        [EmployeeAttribute]
        public IActionResult ApplyCasualLeaves()
        {
            return View();
        }

        [EmployeeAttribute]
        public IActionResult EmployeeDetails()
        {
            string employeeUsername = HttpContext.Session.GetString("Username");
            EmployeeRegistration empDetailsList = new EmployeeRegistration();
            empDetailsList = fetchEmpObj.GetAllEmpDetails(employeeUsername);
            return View(empDetailsList);
        }

        [EmployeeAttribute]
        public IActionResult CancelUpdatingEmpDetails()
        {
            return RedirectToAction("EmployeeDashboard");
        }

        public bool UpdateEmpProfile(string firstName, string lastName)
        {
            string empUsername = HttpContext.Session.GetString("Username");

            bool isEmpUpdated = fetchEmpObj.UpdateEmpDetails(empUsername, firstName, lastName);
            if (isEmpUpdated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [EmployeeAttribute]
        public IActionResult ApplyLeaves()
        {
            string empUsername = HttpContext.Session.GetString("Username");
            EmployeeRegistration empDetailsList = new EmployeeRegistration();
            empDetailsList = fetchEmpObj.GetAllEmpDetails(empUsername);
            return View(empDetailsList);
        }

        [HttpPost]
        public int GetAvailableLeaves(string leaveType)
        {
            string empUsername = HttpContext.Session.GetString("Username");
            
            EmployeeRegistration empDetailsList = new EmployeeRegistration();
            empDetailsList = fetchEmpObj.GetAllEmpDetails(empUsername);
            if (leaveType.ToUpper() == "CASUALLEAVE")
            {
                return empDetailsList.CasualLeaves;
            }
            else if(leaveType.ToUpper() == "SICKLEAVE")
            {
                return empDetailsList.SickLeaves;
            }
            else if(leaveType.ToUpper() == "PATTERNITYLEAVE")
            {
                return empDetailsList.PatternityLeaves;
            }
            else /*if(leaveType == "MATTERNITYLEAVE")*/
            {
                return empDetailsList.MatternityLeaves;
            }
            
        }


        [HttpPost]
        public int NoOfLeavesTaken(DateTime toDate, DateTime fromDate)
        {            
            return (toDate.Date - fromDate.Date).Days+1;
        }

        [EmployeeAttribute]
        public IActionResult CancelLeaveRequest()
        {
            return RedirectToAction("EmployeeDashboard");
        }

        public bool UpdateEmpProfilePic(string imgData)
        {
            string username = HttpContext.Session.GetString("Username");
            if (fetchEmpObj.UpdateProfilePic(imgData, username))
            {
                return true;
            }
            return false;

        }

        
        public IActionResult UnauthorizedAccess()
        {
            return View();
        }

        [HttpPost]
        public string IncrementDate(DateTime toDate)
        {
            var returnToDate = toDate.AddDays(1).ToString().Split(' ')[0];
            return returnToDate;
        }

        [HttpPost]
        public bool RectifyOlderDates(DateTime fromDate, DateTime toDate)
        {
            if(fromDate >= DateTime.Today && toDate < DateTime.Today)
            {
                return true;
            }
            return false;
        }

    }
}
