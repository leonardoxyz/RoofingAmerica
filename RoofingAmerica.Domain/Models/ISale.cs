using RoofingAmerica.Domain.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Domain.Models
{
    public interface ISale : IRepository<Sale, Guid>
    {

    }
}
