using DataAccess.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGarden.Controllers.Base
{
    public class ControllerInitializer
    {
        public ControllerInitializer(SGContext context)
        {
            Context = context;
        }

        public SGContext Context { get; set; }
    }
}
