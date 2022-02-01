using System.Data.Entity;

namespace Aprovi.Data.Models
{
    public partial class CNEntities : DbContext
    {
        public CNEntities(string connectionString) : base(connectionString)
        {
        }
    }
}