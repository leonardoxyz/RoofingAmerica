﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoofingAmerica.Domain.Core.Data
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
