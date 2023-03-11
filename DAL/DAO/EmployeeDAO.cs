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

        public static void DeleteEmployee(int employeeID)
        {
            try
            {
                Employee emp = db.Employee.First(x => x.ID == employeeID);
                db.Employee.DeleteOnSubmit(emp);
                db.SubmitChanges();
                List<Task> tasks = db.Task.Where(x=> x.EmployeeID == employeeID).ToList();
                db.Task.DeleteAllOnSubmit(tasks);
                db.SubmitChanges();

                List<Salary> salary = db.Salary.Where(x=> x.ID == employeeID).ToList(); 
                db.Salary.DeleteAllOnSubmit(salary);
                db.SubmitChanges();

                List<Permission> permissions = db.Permission.Where(x=> x.EmployeeID == employeeID).ToList();
                db.Permission.DeleteAllOnSubmit(permissions);
                db.SubmitChanges(); 

            }
            catch (Exception)
            {

                throw;
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
                dto.BirthDay = item.BirthDay;
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

        public static void UpdateEmployee(int employeeID, int amount)
        {
            try
            {
                Employee employee = db.Employee.First(x => x.ID == employeeID);
                employee.Salary= amount;
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void UpdateEmployee(Employee employee)
        {
            try
            {
                Employee emp = db.Employee.First(x => x.ID == employee.ID);
                emp.UserNo = employee.UserNo;
                emp.Name= employee.Name;
                emp.Surname= employee.Surname;  
                emp.Password = employee.Password;
                emp.isAdmin = employee.isAdmin;
                emp.BirthDay = employee.BirthDay;
                emp.Adress = employee.Adress;
                emp.DepartmentID = employee.DepartmentID;
                emp.PositionID= employee.PositionID;
                emp.Salary = employee.Salary;
                db.SubmitChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdateEmployee(Position position)
        {
            List<Employee> list = db.Employee.Where(x=> x.PositionID == position.ID).ToList();
            foreach (var item in list)
            {
                item.DepartmentID = position.DepartmentID;
            }
            db.SubmitChanges();
        }
    }
}
