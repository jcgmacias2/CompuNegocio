﻿using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ReportesRepository : BaseRepository<Reporte>, IReportesRepository
    {
        public ReportesRepository(CNEntities context) : base(context) { }
    }
}
