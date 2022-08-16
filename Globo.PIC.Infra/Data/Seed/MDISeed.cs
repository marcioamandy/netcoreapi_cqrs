using Globo.PIC.Domain.Entities;
using Globo.PIC.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Globo.PIC.Infra.Data.Seed
{
    public class PICSeed
    {
        public static void Seed(PicDbContext context)
        {

            if (!context.Usuario.Any())
            {
                var users = new List<Usuario>
            {
                new Usuario
               {
                   Login = "msantiag",
                   IsActive = true
               }
            };
                context.AddRange(users);
                context.SaveChanges();
            }
        }
    }

}