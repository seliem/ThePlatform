
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Tamkeen.MCS.Shared.CrossCutting.Logging.LogContext
{
    
    public interface ILogDbContext
    {
        
            DbSet<TEST_LOG> Logs { get; set; }
            int SaveChanges();

      
    }
}
