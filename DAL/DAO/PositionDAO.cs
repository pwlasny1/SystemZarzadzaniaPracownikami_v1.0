﻿using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DAL.DAO
{
    public class PositionDAO : EmployeeContext
    {
        public static void AddPosition(Position position)
        {
			try
			{
				db.Position.InsertOnSubmit(position);
				db.SubmitChanges();
			}
			catch (Exception ex)
			{

				throw ex;
			}
        }

        public static void DeletePosition(int iD)
        {
            try
            {
                Position position = db.Position.FirstOrDefault(x => x.ID == iD);
                if (position != null)
                {
                    db.Position.DeleteOnSubmit(position);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<PositionDTO> GetPositions()
        {
			try
			{
				var list = (from p in db.Position
							join d in db.Department on p.DepartmentID equals d.ID
							select new
							{
								positionID = p.ID,
								positionName = p.PositionName,
								deparmentName = d.DepartmentName,
								departmentID = p.DepartmentID,
							}).OrderBy(x => x.positionID).ToList();

				List<PositionDTO> positionList = new List<PositionDTO>();

				foreach (var item in list)
				{
					PositionDTO dto = new PositionDTO();
					dto.ID = item.positionID;
					dto.PositionName = item.positionName;
					dto.DepartmentName = item.deparmentName;
					dto.DepartmentID = item.departmentID;
					positionList.Add(dto);
				}
				return positionList;

			}

			catch (Exception ex)
			{

				throw ex;
			}
        }

        public static void UpdatePosition(Position position)
        {
			try
			{
				Position pos = db.Position.First(x => x.ID == position.ID);
				pos.PositionName = position.PositionName;
				pos.DepartmentID = position.DepartmentID;
				db.SubmitChanges();
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
