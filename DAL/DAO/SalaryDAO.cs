using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class SalaryDAO : EmployeeContext
    {
        public static void AddSalary(Salary salary)
        {
            try
            {
                db.Salary.InsertOnSubmit(salary);
                db.SubmitChanges(); 
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void DeleteSalary(int salaryID)
        {
            try
            {
                Salary salary = db.Salary.First(x => x.ID == salaryID);
                db.Salary.DeleteOnSubmit(salary);
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<Months> GetMonths()
        {
            return db.Months.ToList();
        }

        public static List<ContractType> GetContractTypes()
        {
            return db.ContractType.ToList();
        }

        public static List<SalaryDetailDTO> GetSalaries()
        {
            List<SalaryDetailDTO> salaryList = new List<SalaryDetailDTO>();
            var list = (from s in db.Salary
                        join e in db.Employee on s.EmployeeID equals e.ID
                        join m in db.Months on s.MonthID equals m.ID
                        
                     
                        select new
                        {
                            UserNo = e.UserNo,
                            name = e.Name,
                            surname = e.Surname,
                            EmployeeID = s.EmployeeID,
                            amount = s.Amount,
                            year = s.Year,
                            monthName = m.MonthName,
                            monthID = s.MonthID,
                            salaryID = s.ID,
                            departmentID = e.DepartmentID,
                            positionID = e.PositionID,
                            
                            
                        }).OrderBy(x=> x.year).ToList();

            foreach ( var item in list ) {
            
                SalaryDetailDTO dto = new SalaryDetailDTO();

                dto.UserNo = item.UserNo;
                dto.Name = item.name;
                dto.Surname = item.surname;
                dto.EmployeeID = item.EmployeeID;
                dto.SalaryAmount = item.amount;
                dto.SalaryYear= item.year;
                dto.MonthID= item.monthID;
                dto.MonthName= item.monthName;
                dto.SalaryID = item.salaryID;
                dto.DepartmentID = item.departmentID;
                dto.PositionID = item.positionID;
                dto.OldSalary = item.amount;
        
               

                salaryList.Add(dto);

            }
            return salaryList;
        }

        public static void UpdateSalary(Salary salary)
        {
            try
            {
                Salary salaries = db.Salary.First(x => x.ID == salary.ID);
                salaries.Amount = salary.Amount;    
                salaries.Year = salary.Year;    
                salaries.MonthID = salary.MonthID; 
                
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
