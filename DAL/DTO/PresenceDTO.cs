﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class PresenceDTO
    {
        public List<Department> Departments { get; set; }
        public List<PositionDTO> Positions { get; set; }
        public List<PresenceDetailDTO> Presences { get; set; }
    }
}
