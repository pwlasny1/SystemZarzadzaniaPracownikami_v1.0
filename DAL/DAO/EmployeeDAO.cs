using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class EmployeeDAO : EmployeeContext
    {
        public static void AddEmployee(Employee employee)
        {
			try
			{
				db.Employee.InsertOnSubmit(employee);
				db.SubmitChanges();
			}
			catch (Exception ex)
			{

				throw ex;
			}
        }

        public static List<EmployeeDetailDTO> GetEmployees()
        {
            List<EmployeeDetailDTO> employeeList = new List<EmployeeDetailDTO>();
            var list = (from e in db.Employee
                        join d in db.Department on e.DepartmentID equals d.ID
                        join p in db.Position on e.PositionID equals p.ID
                        select new
                        {
                            UserNo = e.UserNo,
                            Name = e.Name, 
                            Surname = e.Surname,   
                            EmployeeID = e.ID,
                            Password = e.Password,
                            DepartmentName = d.DepartmentName,
                            PositionName = p.PositionName,
                            DepartmentID = e.DepartmentID,
                            PositionID = e.PositionID,
                            isAdmin = e.isAdmin,
                            Salary = e.Salary, 
                            ImagePath = e.ImagePath,   
                            BirthDay= e.BirthDay,
                            Adress = e.Adress,


                        }).OrderBy(x=> x.UserNo).ToList();

            foreach (var item in list)
            {
                EmployeeDetailDTO dto = new EmployeeDetailDTO();
                dto.UserNo = item.UserNo;
                dto.Name = item.Name;
                dto.Surname = item.Surname;
                dto.EmployeeID = item.EmployeeID;  
                dto.Adress = item.Adress;
                dto.Password = item.Password;
                dto.DepartmentID= item.DepartmentID;
                dto.PositionName = item.PositionName;
                dto.DepartmentID = item.DepartmentID;
                dto.PositionID = item.PositionID;
                dto.DepartmentName = item.DepartmentName;
                dto.Salary = item.Salary;
                dto.ImagePath = item.ImagePath;
                dto.BirthDay = (DateTime)item.BirthDay;
                employeeList.Add(dto);
            }

            return employeeList;                   
                        
        }

        public static List<Employee> GetEmployees(int v, string text)
        {
            try
            {
                List<Employee> list = db.Employee.Where(x => x.UserNo == v && x.Password == text).ToList();
                return list;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static List<Employee> GetUsers(int v)
        {
            return db.Employee.Where(x=> x.UserNo == v).ToList();
        }
    }
}
