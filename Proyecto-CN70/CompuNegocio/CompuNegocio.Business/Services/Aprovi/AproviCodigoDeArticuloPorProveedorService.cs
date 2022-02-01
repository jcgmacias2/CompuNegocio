﻿using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public class AproviCodigoDeArticuloPorProveedorService : CodigoDeArticuloPorProveedorService
    {
        public AproviCodigoDeArticuloPorProveedorService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
