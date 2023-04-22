using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class PresenceDAO : EmployeeContext
    {
        public static void AddPresence(Presences presences)
        {
			try
			{
				db.Presences.InsertOnSubmit(presences);
				db.SubmitChanges();
			}
			catch (Exception)
			{

				throw;
			}
        }

        public static void DeletePresence(int presenceID)
        {
            try
            {
                Presences pr = db.Presences.First(x => x.ID == presenceID);
                db.Presences.DeleteOnSubmit(pr);
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
            var list = (from p in db.Presences
                        join e in db.Employee on p.EmployeeID equals e.ID
                        select new
                        {
                            UserNo = e.UserNo,
                            name = e.Name,
                            Surname = e.Surname,
                            startdate = p.PresenceStart, 
                            endDate = p.PresenceEnd,   
                            employeeID = p.EmployeeID,
                            PresenceID = p.ID,
                            explanation = p.PresenceExplanation,
                            amount = p.PresenceHours,
                            departmentID = e.DepartmentID,
                            positionID = e.PositionID,
                            presenceDay = p.PresenceDate
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
                dto.PresenceID = item.PresenceID;
                presences.Add(dto);

            }
            return presences;
            
        }

        public static void UpdatePresence(Presences presences)
        {
            try
            {
                Presences pr = db.Presences.First(x => x.ID == presences.ID);
                pr.PresenceStart = presences.PresenceStart;
                pr.PresenceEnd = presences.PresenceEnd;
                pr.PresenceExplanation = presences.PresenceExplanation;
                pr.PresenceHours = presences.PresenceHours;               
                db.SubmitChanges();
               

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
