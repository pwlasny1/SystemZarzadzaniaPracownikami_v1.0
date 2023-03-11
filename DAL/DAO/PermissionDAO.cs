using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class PermissionDAO : EmployeeContext
    {
        public static void AddPermission(Permission permission)
        {
			try
			{
				db.Permission.InsertOnSubmit(permission);
				db.SubmitChanges();
			}
			catch (Exception ex)
			{

				throw ex;
			}
        }


        public static List<PermissionDetailDTO> GetPermissions()
        {
            
            List<PermissionDetailDTO> permissions = new List<PermissionDetailDTO>();
            var list = (from p in db.Permission
                        join s in db.PermissionState on p.PermissionState equals s.ID
                        join e in db.Employee on p.EmployeeID equals e.ID
                        select new
                        {
                            UserNo = e.UserNo,
                            name = e.Name,
                            Surname = e.Surname,
                            StateName = s.StateName,
                            stateID = p.PermissionState,
                            startDate = p.PermissionStartDate,
                            endDate = p.PermissionEndDate,
                            employeeID = p.EmployeeID,
                            PermissionID = p.ID,
                            explanation = p.PermissionExplanation,
                            Dayamount = p.PermissionDay,
                            departmentID = e.DepartmentID,
                            positionID = e.PositionID

                        }).OrderBy(x=> x.startDate).ToList();
            foreach (var item in list)
            {
                PermissionDetailDTO dto = new PermissionDetailDTO();
                dto.UserNo = item.UserNo;
                dto.Name = item.name;
                dto.Surname= item.Surname;
                dto.EmployeeID = item.employeeID;
                dto.PermissionDayCount = item.Dayamount;
                dto.StartDate = item.startDate;
                dto.EndDate = item.endDate;
                dto.DepartmentID = item.departmentID;
                dto.PositionID = item.positionID;
                dto.State = item.stateID;
                dto.StateName = item.StateName;
                dto.Explanation = item.explanation;
                dto.PermissionID = item.PermissionID;
                permissions.Add(dto);


            }
            return permissions;
        }

        public static List<PermissionState> GetStates()
        {
            return db.PermissionState.ToList();
        }

        public static void UpdatePermission(Permission permission)
        {
            try
            {
                Permission per = db.Permission.First(x=> x.ID == permission.ID);
                per.PermissionStartDate = permission.PermissionStartDate;
                per.PermissionEndDate = permission.PermissionEndDate;
                per.PermissionExplanation = permission.PermissionExplanation;
                per.PermissionDay = permission.PermissionDay;
                db.SubmitChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void UpdatePermission(int permissionID, int approved)
        {
            try
            {
                Permission per = db.Permission.First(x => x.ID == permissionID);
                    per.PermissionState = approved; 
                    db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void DeletePermission(int permissionID)
        {
            try
            {
                Permission per = db.Permission.First(x => x.ID == permissionID);
                db.Permission.DeleteOnSubmit(per);
                db.SubmitChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
