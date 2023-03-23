﻿using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class PresenceDAO : EmployeeContext
    {
        public static void AddPresence(Presence presence)
        {
			try
			{
				db.Presence.InsertOnSubmit(presence);
				db.SubmitChanges();
			}
			catch (Exception)
			{

				throw;
			}
        }

        public static List<PresenceDetailDTO> GetPresences()
        {
            List<PresenceDetailDTO> presences= new List<PresenceDetailDTO>();
            var list = (from p in db.Presence
                        join e in db.Employee on p.EmployeeID equals e.ID
                        select new
                        {
                            UserNo = e.UserNo,
                            name = e.Name,
                            Surname = e.Surname,
                            startdate = p.PresenceStartDate, 
                            endDate = p.PresenceEndDate,   
                            employeeID = p.EmployeeID,
                            PresenceID = p.ID,
                            explanation = p.PresenceExplanation,
                            amount = p.PermissionDay,
                            departmentID = e.DepartmentID,
                            positionID = e.PositionID
                        }).OrderBy(x=> x.startdate).ToList();
            foreach (var item in list)
            {
                PresenceDetailDTO dto = new PresenceDetailDTO();
                dto.UserNo= item.UserNo;
                dto.Name = item.name;
                dto.Surname= item.Surname;
                dto.EmployeeID = item.employeeID;
                dto.PresenceAmount = item.amount;
                dto.StartDate = item.startdate;
                dto.EndDate= item.endDate;
                dto.DepartmentID = item.departmentID;
                dto.PositionID = item.positionID;
                dto.Explanation = item.explanation;
                presences.Add(dto);

            }
            return presences;
            
        }   
    }
}