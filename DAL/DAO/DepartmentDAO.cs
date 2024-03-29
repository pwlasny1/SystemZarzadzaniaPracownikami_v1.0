﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class DepartmentDAO : EmployeeContext
    {
        public static void AddDepartment(Department department)
        {
            try
            {
                db.Department.InsertOnSubmit(department);
                db.SubmitChanges();
            }
            catch(Exception ex) {

                throw ex;
            
            }
        }

        public static void DeleteDepartment(int iD)
        {
            try
            {
                Department dep = db.Department.First(x => x.ID == iD);
                db.Department.DeleteOnSubmit(dep);
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<Department> GetDepartments()
        {
            return db.Department.ToList();
        }

        public static void UpdateDepartment(Department department)
        {
            try
            {
                Department dep = db.Department.First(x => x.ID == department.ID);
                dep.DepartmentName = department.DepartmentName;
                db.SubmitChanges();
            }
            catch (Exception )
            {

                throw;
            }
        }
    }
}
