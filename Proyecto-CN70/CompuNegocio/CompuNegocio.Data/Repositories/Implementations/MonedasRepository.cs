﻿using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class MonedasRepository : BaseRepository<Moneda>, IMonedasRepository
    {
        public MonedasRepository(CNEntities context) : base(context) { }
    }
}
