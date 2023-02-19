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
                permissions.Add(dto);


            }
            return permissions;
        }

        public static List<PermissionState> GetStates()
        {
            return db.PermissionState.ToList();
        }
    }
}
