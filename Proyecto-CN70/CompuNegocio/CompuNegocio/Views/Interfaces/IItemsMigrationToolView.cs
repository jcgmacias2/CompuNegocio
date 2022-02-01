using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IItemsMigrationToolView : IBaseView
    {
        event Action Quit;
        event Action Migrate;

        List<VMEquivalenciaUnidades> Units { get; }
        List<VMEquivalenciaClasificacion> Classifications { get; }
        Impuesto VAT { get; }
        string dbcPath { get; }


        void Fill(List<VMEquivalenciaUnidades> units, List<UnidadesDeMedida> unitsOfMeasure, List<VMEquivalenciaClasificacion> familiesAndDepartments, List<Clasificacione> classifications, List<Impuesto> taxes);
    }
}
