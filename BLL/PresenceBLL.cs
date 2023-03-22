using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.DAO;

namespace BLL
{
    public class PresenceBLL
    {
        public static void AddPresence(Presence presence)
        {
            PresenceDAO.AddPresence(presence);
        }
    }
}
