using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COSEC.Data
{
    public static class DBInitializer
    {
        public static void Initialize(CosecContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;  
            }
        }
    }
}
