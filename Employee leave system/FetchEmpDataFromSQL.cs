using Employee_leave_system.Models;
using System.Data;
using System.Data.SqlClient;




namespace Employee_leave_system
{
    public class FetchEmpDataFromSQL
    {

        public bool CheckEmpUsernameInDB(EmpLogin empLoginObj)
        {
            string dBUserName;
            SqlConnection concheckEmp = GetConnectionString();
            SqlCommand cmdCheckEmp = new SqlCommand("exec CheckEmpUsername @UserName", concheckEmp);
            concheckEmp.Open();
            cmdCheckEmp.Parameters.AddWithValue("@UserName", empLoginObj.UserName);
            SqlDataReader readEmpUsername= cmdCheckEmp.ExecuteReader();
            if (readEmpUsername.HasRows)
            {
                return true;
            }
            return false;
        }


        public bool CheckPasswordInDB(EmpLogin empLoginObj)
        {
            string dBPassword;
            SqlConnection concheckEmp = GetConnectionString();
            SqlCommand cmdCheckEmp = new SqlCommand("exec CheckEmpPassword @UserName", concheckEmp);
            concheckEmp.Open();
            cmdCheckEmp.Parameters.AddWithValue("@UserName", empLoginObj.UserName);
            SqlDataReader readEmpPassword = cmdCheckEmp.ExecuteReader();
            if (readEmpPassword.HasRows)
            {
                while (readEmpPassword.Read())
                {
                    dBPassword = readEmpPassword["Password"].ToString();
                    if (empLoginObj.Password == dBPassword)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public EmployeeRegistration GetAllEmpDetails(string employeeUsername)
        {
            EmployeeRegistration EmpDetailsList = new EmployeeRegistration();
            string dbFName, dbLName, dbDesignation, dbUserName, dbPassword, dbImgData;
            DateTime dbDateOfReg;
            int dbEmpId, dbCasualLeaves, dbSickLeaves, dbMatternityLeaves, dbPatternityLeaves;
            char dbGender;
            SqlConnection conGetEmpDetails = GetConnectionString();
            SqlCommand cmdGetEmpDetails = new SqlCommand("exec GetAllEmpDetails @Username", conGetEmpDetails);
            conGetEmpDetails.Open();
            cmdGetEmpDetails.Parameters.AddWithValue("@Username", employeeUsername);
            SqlDataReader readEmpCred = cmdGetEmpDetails.ExecuteReader();
            if (readEmpCred.HasRows)
            {
                while (readEmpCred.Read())
                {
                    dbEmpId = Convert.ToInt16(readEmpCred["EmpId"]);
                    dbUserName = readEmpCred["UserName"].ToString();                    
                    dbFName = readEmpCred["FirstName"].ToString();
                    dbLName = readEmpCred["LastName"].ToString();
                    dbGender = Convert.ToChar(readEmpCred["Gender"]);
                    dbDesignation = readEmpCred["Desgination"].ToString();
                    dbCasualLeaves = Convert.ToInt16(readEmpCred["CasualLeaves"]);
                    dbSickLeaves = Convert.ToInt16(readEmpCred["SickLeaves"]);
                    dbMatternityLeaves = Convert.ToInt16(readEmpCred["MatternityLeaves"]);
                    dbPatternityLeaves = Convert.ToInt16(readEmpCred["PatternityLeaves"]);
                    dbPassword = Convert.ToString(readEmpCred["Password"]);
                    dbDateOfReg = Convert.ToDateTime(readEmpCred["RegistrationDate"].ToString().Split(" ")[0]);
                    dbImgData = Convert.ToString(readEmpCred["ImgData"]);

                    EmpDetailsList.EmpID = dbEmpId;
                    EmpDetailsList.Username = dbUserName;
                    EmpDetailsList.FirstName = dbFName;
                    EmpDetailsList.LastName = dbLName;
                    EmpDetailsList.Gender = dbGender;
                    EmpDetailsList.Designation = dbDesignation;
                    EmpDetailsList.CasualLeaves = dbCasualLeaves;
                    EmpDetailsList.SickLeaves = dbSickLeaves;
                    EmpDetailsList.MatternityLeaves = dbMatternityLeaves;
                    EmpDetailsList.PatternityLeaves = dbPatternityLeaves;
                    EmpDetailsList.Password = dbPassword;
                    EmpDetailsList.RegistrationDate = dbDateOfReg.Date;
                    EmpDetailsList.ImgData = dbImgData;
                }
                conGetEmpDetails.Close();
            }
            return EmpDetailsList;
        }



        public bool AddEmpLoginLogToDB(EmployeeRegistration empDetailsList)
        {
            SqlConnection conAddEmpLog = GetConnectionString();
            SqlCommand cmdAddEmpLog = new SqlCommand("execute SP_AddEmpLog @UserRole, @UserID, @FirstName, @LastName,@DateOfLogin, @Username", conAddEmpLog);
            conAddEmpLog.Open();
            cmdAddEmpLog.Parameters.AddWithValue("@UserRole", "E");
            cmdAddEmpLog.Parameters.AddWithValue("@UserID", empDetailsList.EmpID);
            cmdAddEmpLog.Parameters.AddWithValue("@FirstName", empDetailsList.FirstName);
            cmdAddEmpLog.Parameters.AddWithValue("@LastName", empDetailsList.LastName);
            cmdAddEmpLog.Parameters.AddWithValue("@DateOfLogin", empDetailsList.RegistrationDate);
            cmdAddEmpLog.Parameters.AddWithValue("@Username", empDetailsList.Username);
            int numberOfRowsAffected = cmdAddEmpLog.ExecuteNonQuery();
            if (numberOfRowsAffected > 0)
            {
                return true;
            }
            return false;
        }




        public bool UpdateEmpDetails(string empUsername, string firstName, string lastName)
        {
            SqlConnection conUpdateEmpDetails = GetConnectionString();
            SqlCommand cmdUpdateEmpDetails = new SqlCommand("execute SP_UpdateEmpDetails @FirstName, @LastName, @UserName", conUpdateEmpDetails);
            conUpdateEmpDetails.Open();
            
            cmdUpdateEmpDetails.Parameters.AddWithValue("@FirstName", firstName);
            cmdUpdateEmpDetails.Parameters.AddWithValue("@LastName", lastName);            
            cmdUpdateEmpDetails.Parameters.AddWithValue("@UserName", empUsername);

            int noOfRowsUpdated = cmdUpdateEmpDetails.ExecuteNonQuery();
            conUpdateEmpDetails.Close();
            if (noOfRowsUpdated > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateProfilePic(string imgData, string username)
        {
            SqlConnection conUpdateEmpPic = GetConnectionString();
            SqlCommand cmdUpdateEmpPic = new SqlCommand("update EmployeeRegData set ImgData = @ImgData where UserName= @Username", conUpdateEmpPic);
            conUpdateEmpPic.Open();

            cmdUpdateEmpPic.Parameters.AddWithValue("@ImgData", imgData);
            cmdUpdateEmpPic.Parameters.AddWithValue("@Username", username);
            

            int noOfRowsUpdated = cmdUpdateEmpPic.ExecuteNonQuery();
            conUpdateEmpPic.Close();
            if (noOfRowsUpdated > 0)
            {
                return true;
            }
            return false;
        }


        public bool SubmitLeaveRequestData(int empID, string leaveType,string leaveReason,DateTime fromDt, DateTime toDt)
        {
            SqlConnection conEmpInsertLeaveRequest = GetConnectionString();
            SqlCommand cmdEmpInsertLeaveRequest = new SqlCommand("execute SP_InsertEmpLeaveRequest @UserId, @TypeOfLeave, @Description, @Status, @DateOfApplication, @LeaveFromDt, @LeaveToDt", conEmpInsertLeaveRequest);
            conEmpInsertLeaveRequest.Open();

            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@UserId", empID.ToString());
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@TypeOfLeave", leaveType);
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@Description", leaveReason);
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@Status", "Pending");
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@DateOfApplication", DateTime.Today);
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@LeaveFromDt", fromDt);
            cmdEmpInsertLeaveRequest.Parameters.AddWithValue("@LeaveToDt", toDt);
            int noOfRowsUpdated = cmdEmpInsertLeaveRequest.ExecuteNonQuery();
            conEmpInsertLeaveRequest.Close();
            if (noOfRowsUpdated > 0)
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
