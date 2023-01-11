using Employee_leave_system.Models;
using System.Data;
using System.Data.SqlClient;

namespace Employee_leave_system
{
    public class FetchDataFromSQL
    {
        public bool CheckUsernameInDB(AdminLogin adminLoginObj)
        {
            string  dBUserName;
            
            SqlConnection concheckAdmin = GetConnectionString();
            SqlCommand cmdCheckAdmin = new SqlCommand("exec CheckAdminUsername",concheckAdmin);
            concheckAdmin.Open();
            SqlDataReader readAdminCred = cmdCheckAdmin.ExecuteReader();
            if (readAdminCred.HasRows)
            {
                while (readAdminCred.Read())
                {
                    dBUserName = readAdminCred["Username"].ToString();                 
                    if (adminLoginObj.UserName == dBUserName)
                    {
                        return true;
                    }
                }                
            }
            return false;

            
        }


        public bool CheckPasswordInDB(AdminLogin adminLoginObj)
        {
            string dBPassword;
            SqlConnection concheckAdmin = GetConnectionString();
            SqlCommand cmdCheckAdmin = new SqlCommand("exec CheckAdminPassword", concheckAdmin);
            concheckAdmin.Open();
            SqlDataReader readAdminCred = cmdCheckAdmin.ExecuteReader();
            if (readAdminCred.HasRows)
            {
                while (readAdminCred.Read())
                {
                    dBPassword = readAdminCred["Password"].ToString();
                    if (adminLoginObj.Password == dBPassword)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public AdminUserDetails GetAllAdminDetails(string adminUsername)
        {
            AdminUserDetails AdminDetailsList = new AdminUserDetails();
            string  dbFName, dbLName, dbDesignation, dBUserName, dBPassword;
            DateTime dbDateOfReg;
            int dbAdminId, dbAge;

            SqlConnection conGetAdminDetails = GetConnectionString();
            SqlCommand cmdGetAdminDetails = new SqlCommand("exec GetAllAdminDetails @Username", conGetAdminDetails);
            conGetAdminDetails.Open();
            cmdGetAdminDetails.Parameters.AddWithValue("@Username", adminUsername);
            SqlDataReader readAdminCred = cmdGetAdminDetails.ExecuteReader();
            if (readAdminCred.HasRows)
            {
                while (readAdminCred.Read())
                {
                    
                    dBUserName = readAdminCred["Username"].ToString();
                    dbAdminId = Convert.ToInt16(readAdminCred["AdminId"]);
                    dbFName = readAdminCred["FirstName"].ToString();
                    dbLName = readAdminCred["LastName"].ToString();
                    dbDesignation = readAdminCred["Designation"].ToString();
                    dbAge = Convert.ToInt16(readAdminCred["Age"]);
                    var dbGender = Convert.ToChar(readAdminCred["gender"]);
                    dbDateOfReg = Convert.ToDateTime(readAdminCred["DateOfReg"]);

                    AdminDetailsList.AdminID =  dbAdminId;
                    AdminDetailsList.FirstName = dbFName;
                    AdminDetailsList.LastName = dbLName;
                    AdminDetailsList.Designation = dbDesignation;
                    AdminDetailsList.Age =  dbAge;
                    AdminDetailsList.Gender = dbGender;
                    AdminDetailsList.RegistrationDate = dbDateOfReg;
                    AdminDetailsList.Username = dBUserName;
                }                
            }
            return AdminDetailsList;
        }
            


        public bool AddAdminLoginLogToDB(AdminUserDetails AdminDetailsList)
        {           
            SqlConnection conAddAdminLog = GetConnectionString();
            SqlCommand cmdAddAdminLog = new SqlCommand("execute SP_AddAdminLog @UserRole, @UserID, @FirstName, @LastName, @Username", conAddAdminLog);
            conAddAdminLog.Open();
            cmdAddAdminLog.Parameters.AddWithValue("@UserRole", "Admin");
            cmdAddAdminLog.Parameters.AddWithValue("@UserID", AdminDetailsList.AdminID);
            cmdAddAdminLog.Parameters.AddWithValue("@FirstName", AdminDetailsList.FirstName);
            cmdAddAdminLog.Parameters.AddWithValue("@LastName", AdminDetailsList.LastName);
            cmdAddAdminLog.Parameters.AddWithValue("@Username", AdminDetailsList.Username);
            int numberOfRowsAffected = cmdAddAdminLog.ExecuteNonQuery();
            if(numberOfRowsAffected > 0)
            {
                return true;
            }
            return false;
        }


        public List<EmployeeRegistration> GetAllEmployees()
        {
            List<EmployeeRegistration> AllEmployeesList = new List<EmployeeRegistration>();
            SqlConnection conGetAllEmployees = GetConnectionString();
            SqlCommand cmdGetAllEmployees = new SqlCommand("SP_GetAllEmployees", conGetAllEmployees);
            cmdGetAllEmployees.CommandType = System.Data.CommandType.StoredProcedure;
            conGetAllEmployees.Open();
            SqlDataAdapter sqlDataget = new SqlDataAdapter(cmdGetAllEmployees);
            DataTable dataTableObj = new DataTable();
            sqlDataget.Fill(dataTableObj);
            conGetAllEmployees.Close();
            foreach (DataRow emp in dataTableObj.Rows)
            {
                EmployeeRegistration empRegObj = new EmployeeRegistration();
                empRegObj.Username = emp["UserName"].ToString();
                empRegObj.FirstName = emp["FirstName"].ToString();
                empRegObj.LastName = emp["LastName"].ToString();
                empRegObj.Gender = Convert.ToChar(emp["Gender"]);
                empRegObj.Designation = emp["Desgination"].ToString();
                empRegObj.CasualLeaves = Convert.ToInt16(emp["CasualLeaves"]);
                empRegObj.SickLeaves = Convert.ToInt16(emp["SickLeaves"]);
                empRegObj.MatternityLeaves = Convert.ToInt16(emp["MatternityLeaves"]);
                empRegObj.PatternityLeaves = Convert.ToInt16(emp["PatternityLeaves"]);
                empRegObj.Password = emp["PatternityLeaves"].ToString();
                empRegObj.RegistrationDate = Convert.ToDateTime(emp["RegistrationDate"]);
                empRegObj.ImgData = Convert.ToString(emp["ImgData"]);

                AllEmployeesList.Add(empRegObj);
            }
            return AllEmployeesList;
        }

        public List<AppliedEmployeeLeaves> GetEmpPendingLeaveDetails()
        {
            List<AppliedEmployeeLeaves> allEmpLeaves = new List<AppliedEmployeeLeaves>();
            SqlConnection conGetAllEmpLeaves = GetConnectionString();
            SqlCommand cmdGetAllEmpLeaves = new SqlCommand("SP_GetAllEmpPendingLeaves", conGetAllEmpLeaves);
            cmdGetAllEmpLeaves.CommandType = System.Data.CommandType.StoredProcedure;
            conGetAllEmpLeaves.Open();
            SqlDataAdapter sqlDataget = new SqlDataAdapter(cmdGetAllEmpLeaves);
            DataTable dataTableObj = new DataTable();
            sqlDataget.Fill(dataTableObj);
            conGetAllEmpLeaves.Close();
            foreach (DataRow empLeaveReq in dataTableObj.Rows)
            {
                AppliedEmployeeLeaves empLeaveRequests = new AppliedEmployeeLeaves();
                empLeaveRequests.UserId = Convert.ToInt16(empLeaveReq["UserId"]);
                empLeaveRequests.ApplicationId = Convert.ToInt16(empLeaveReq["ApplicationId"]);
                empLeaveRequests.TypeOfLeave = empLeaveReq["TypeOfLeave"].ToString();
                empLeaveRequests.Description = empLeaveReq["Description"].ToString();
                empLeaveRequests.Status = "Pending";
                empLeaveRequests.DateOfApplication = Convert.ToDateTime(empLeaveReq["DateOfApplication"]);
                empLeaveRequests.LeaveFromDt = Convert.ToDateTime(empLeaveReq["LeaveFromDt"]);
                empLeaveRequests.LeaveToDt = Convert.ToDateTime(empLeaveReq["LeaveToDt"]);



                allEmpLeaves.Add(empLeaveRequests);
            }
            return allEmpLeaves;
        }





        public bool UpdateAdminDetails(string firstName, string lastName, int age, string designation, string username)
        {
            SqlConnection conUpdateAdminDetails = GetConnectionString();
            SqlCommand cmdUpdateAdminDetails = new SqlCommand("execute SP_UpdateAdminDetails @Age, @FirstName, @LastName, @Designation, @Username", conUpdateAdminDetails);
            conUpdateAdminDetails.Open();
            cmdUpdateAdminDetails.Parameters.AddWithValue("@Age", age);
            cmdUpdateAdminDetails.Parameters.AddWithValue("@FirstName", firstName);
            cmdUpdateAdminDetails.Parameters.AddWithValue("@LastName", lastName);            
            cmdUpdateAdminDetails.Parameters.AddWithValue("@Designation", designation);
            cmdUpdateAdminDetails.Parameters.AddWithValue("@Username", username);
            int noOfRowsUpdated = cmdUpdateAdminDetails.ExecuteNonQuery();
            conUpdateAdminDetails.Close();
            if (noOfRowsUpdated > 0)
            {
                return true;
            }
            return false;
        }


        public bool AddEmpToDB(string username, string firstName, string lastName, char gender, string designation, int casualLeaves, int sickLeaves, int matternityLeaves, int patternityLeaves, string password, DateTime regDate, string imgData)
        {
            SqlConnection conAddEmpToDB = GetConnectionString();
            SqlCommand cmdAddEmpToDB = new SqlCommand("execute SP_AddEmpToDB @Username,@FirstName,@LastName,@Gender,@Designation,@CasualLeaves,@SickLeaves,@MatternityLeaves,@PatternityLeaves,@Password,@RegistrationDate,@ImgData", conAddEmpToDB);
            conAddEmpToDB.Open();
            cmdAddEmpToDB.Parameters.AddWithValue("@Username", username);
            cmdAddEmpToDB.Parameters.AddWithValue("@FirstName", firstName);
            cmdAddEmpToDB.Parameters.AddWithValue("@LastName", lastName);
            cmdAddEmpToDB.Parameters.AddWithValue("@Gender", gender);
            cmdAddEmpToDB.Parameters.AddWithValue("@Designation", designation);
            cmdAddEmpToDB.Parameters.AddWithValue("@CasualLeaves", casualLeaves);
            cmdAddEmpToDB.Parameters.AddWithValue("@SickLeaves", sickLeaves);
            cmdAddEmpToDB.Parameters.AddWithValue("@MatternityLeaves", matternityLeaves);
            cmdAddEmpToDB.Parameters.AddWithValue("@PatternityLeaves", patternityLeaves);
            cmdAddEmpToDB.Parameters.AddWithValue("@Password", password);
            cmdAddEmpToDB.Parameters.AddWithValue("@RegistrationDate", regDate);
            cmdAddEmpToDB.Parameters.AddWithValue("@ImgData", imgData);
            int moOfRowsInserted = cmdAddEmpToDB.ExecuteNonQuery();
            conAddEmpToDB.Close();
            if (moOfRowsInserted > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsLeaveApproved(int applicationID)
        {
            SqlConnection conApproveLeave = GetConnectionString();
            SqlCommand cmdApproveLeave = new SqlCommand("execute SP_ApproveLeave @ApplicationId", conApproveLeave);
            conApproveLeave.Open();
            cmdApproveLeave.Parameters.AddWithValue("@ApplicationId", applicationID);
            int numberOfRowsAffected = cmdApproveLeave.ExecuteNonQuery();
            if (numberOfRowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public string GetTypeOfLeave(int applicationID)
        {
            string TypeOfLeave = String.Empty;
            SqlConnection conGetTypeOfLeave = GetConnectionString();
            SqlCommand cmdGetTypeOfLeave = new SqlCommand("select TypeOfLeave from LeaveDetails where ApplicationId=@ApplicationId", conGetTypeOfLeave);
            conGetTypeOfLeave.Open();
            cmdGetTypeOfLeave.Parameters.AddWithValue("@ApplicationId", applicationID);
            SqlDataReader readTypeOfLeave = cmdGetTypeOfLeave.ExecuteReader();
            if (readTypeOfLeave.HasRows)
            {
                while (readTypeOfLeave.Read())
                {
                    TypeOfLeave = Convert.ToString(readTypeOfLeave["TypeOfLeave"]);
                    conGetTypeOfLeave.Close();
                    return TypeOfLeave;
                }                                
            }
            return TypeOfLeave;
        }


        public bool DecrementLeaveCount(string typeOfLeaveApplied, int empId, int leaveDays)
        {
            string readLeaveCountQuery = string.Empty;
            string WriteLeaveCountQuery = string.Empty;
            int getLeaveCount=0,setLeaveCount=0;

            if (typeOfLeaveApplied == "SickLeave" || typeOfLeaveApplied == "SickLeaves")
            {
                readLeaveCountQuery = "select SickLeaves from EmployeeRegData where EmpId = @EmpId";
                WriteLeaveCountQuery = "UPDATE EmployeeRegData SET SickLeaves = @SetLeaveCount where EmpId = @EmpId";
            }
            else if (typeOfLeaveApplied == "PatternityLeave" || typeOfLeaveApplied == "PatternityLeaves")
            {
                readLeaveCountQuery = "select PatternityLeaves from EmployeeRegData where EmpId = @EmpId";
                WriteLeaveCountQuery = "UPDATE EmployeeRegData SET PatternityLeaves = @SetLeaveCount where EmpId = @EmpId";
            }
            else if (typeOfLeaveApplied == "CasualLeave" || typeOfLeaveApplied == "CasualLeaves")
            {
                readLeaveCountQuery = "select CasualLeaves from EmployeeRegData where EmpId = @EmpId";
                WriteLeaveCountQuery = "UPDATE EmployeeRegData SET CasualLeaves = @SetLeaveCount where EmpId = @EmpId";
            }
            else if (typeOfLeaveApplied == "MatternityLeave" || typeOfLeaveApplied == "MatternityLeaves")
            {
                readLeaveCountQuery = "select MatternityLeaves from EmployeeRegData where EmpId = @EmpId";
                WriteLeaveCountQuery = "UPDATE EmployeeRegData SET MatternityLeaves = @SetLeaveCount where EmpId = @EmpId";
            }
            SqlConnection conReadLeaveCount = GetConnectionString();
            SqlCommand cmdReadLeaveCount = new SqlCommand(readLeaveCountQuery, conReadLeaveCount);
            conReadLeaveCount.Open();
            cmdReadLeaveCount.Parameters.AddWithValue("@EmpId", empId);
            SqlDataReader readTypeOfLeave = cmdReadLeaveCount.ExecuteReader();
            if (readTypeOfLeave.HasRows)
            {
                while (readTypeOfLeave.Read())
                {
                    getLeaveCount = Convert.ToInt16(readTypeOfLeave[0]);
                    
                }
                conReadLeaveCount.Close();
            }
            setLeaveCount = getLeaveCount - leaveDays;
            
            SqlConnection conWriteLeaveCount = GetConnectionString();
            SqlCommand cmdWriteLeaveCount = new SqlCommand(WriteLeaveCountQuery, conWriteLeaveCount);
            conWriteLeaveCount.Open();
            cmdWriteLeaveCount.Parameters.AddWithValue("@EmpId", empId);
            cmdWriteLeaveCount.Parameters.AddWithValue("@SetLeaveCount", setLeaveCount);
            int noOfRowsAffected = cmdWriteLeaveCount.ExecuteNonQuery();
            if (noOfRowsAffected > 0)
            {
                return true;
            }
            return false;

        }



        public Dictionary<string,object> GetAllEmpLeaveDetails(int applicationID)
        {
            string typeOfLeave=string.Empty, description=string.Empty, status = string.Empty;
            int userId;
            DateTime dateOfApplication, leaveFromDt, leaveToDt;

            Dictionary<string, object> EmpLeaveDetails = new Dictionary<string, object>();
            
            SqlConnection conGetLeaveDetails = GetConnectionString();
            SqlCommand cmdGetLeaveDetails = new SqlCommand("select * from LeaveDetails where ApplicationId=@ApplicationId", conGetLeaveDetails);
            conGetLeaveDetails.Open();
            cmdGetLeaveDetails.Parameters.AddWithValue("@ApplicationId", applicationID);
            
            SqlDataReader readTypeOfLeave = cmdGetLeaveDetails.ExecuteReader();
            if (readTypeOfLeave.HasRows)
            {
                while (readTypeOfLeave.Read())
                {
                    userId = Convert.ToInt16(readTypeOfLeave["UserId"]);
                    description = Convert.ToString(readTypeOfLeave["Description"]);
                    status = Convert.ToString(readTypeOfLeave["Status"]);
                    dateOfApplication = Convert.ToDateTime(readTypeOfLeave["DateOfApplication"]);
                    leaveFromDt = Convert.ToDateTime(readTypeOfLeave["LeaveFromDt"]);
                    leaveToDt = Convert.ToDateTime(readTypeOfLeave["LeaveToDt"]);

                    conGetLeaveDetails.Close();
                    EmpLeaveDetails.Add("userId", userId);
                    EmpLeaveDetails.Add("description", description);
                    EmpLeaveDetails.Add("status", status);
                    EmpLeaveDetails.Add("dateOfApplication", dateOfApplication);
                    EmpLeaveDetails.Add("leaveFromDt", leaveFromDt);
                    EmpLeaveDetails.Add("leaveToDt", leaveToDt);
                    return EmpLeaveDetails;

                }
            }
            return EmpLeaveDetails;
        }


        public bool DenyEmpLeave(int applicationID)
        {
            SqlConnection conDenyLeave = GetConnectionString();
            SqlCommand cmdDeniedLeave = new SqlCommand("execute SP_DenyLeave @ApplicationId", conDenyLeave);
            conDenyLeave.Open();
            cmdDeniedLeave.Parameters.AddWithValue("@ApplicationId", applicationID);
            int numberOfRowsAffected = cmdDeniedLeave.ExecuteNonQuery();
            if (numberOfRowsAffected > 0)
            {
                return true;
            }
            return false;
        }


        public SqlConnection GetConnectionString()
        {
            SqlConnection getCon = new SqlConnection("Data Source=localhost;Initial Catalog=Emp_Leave_System;Integrated Security=True");
            return getCon;
        }
    }


    
}
