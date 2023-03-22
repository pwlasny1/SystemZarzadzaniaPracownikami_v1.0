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
    }
}
