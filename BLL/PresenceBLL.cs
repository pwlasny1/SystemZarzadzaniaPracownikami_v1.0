using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.DTO;

namespace BLL
{
    public class PresenceBLL
    {
        public static void AddPresence(Presence presence)
        {
            PresenceDAO.AddPresence(presence);
        }

        public static void DeletePresence(int presenceID)
        {
            PresenceDAO.DeletePresence(presenceID);
        }

        public static PresenceDTO GetAll()
        {
            PresenceDTO dto = new PresenceDTO();    
            dto.Departments = DepartmentDAO.GetDepartments();
            dto.Positions= PositionDAO.GetPositions();
            dto.Presences = PresenceDAO.GetPresences();
            return dto;
        }

        public static void UpdatePresence(Presence presence)
        {
            PresenceDAO.UpdatePresence(presence);
        }
    }
}
