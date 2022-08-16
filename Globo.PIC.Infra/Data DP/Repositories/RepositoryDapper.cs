using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Interfaces;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using MySqlConnector;

namespace Globo.PIC.Infra.Data_DP.Repositories
{
    public class RepositoryDapper<TEntity> : IDisposable, IRepositoryDapper<TEntity> where TEntity : class
    {
        //private readonly IConfiguration _config;

        protected readonly MySqlConnection conn;

        public RepositoryDapper(/*IConfiguration config*/)
        {
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(c =>
                {
                    //c.AddMap(new SampleMap());
                    c.ForDommel();
                });
            }

            var conection = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            //_config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("properties/launchSettings.json")
            //    .Build();
                        
            conn = new MySqlConnection(conection);
        }

        public void Add(TEntity obj,  CancellationToken cancellationToken)
        {
            conn.Insert(obj);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var dados = conn.GetAll<TEntity>();

            return dados;
        }

        
        public virtual TEntity GetById(long id, CancellationToken cancellationToken)
        {
            return conn.Get<TEntity>(id);
        }

        public void Remove(TEntity obj)
        {
            conn.Delete(obj);
        }

        public void Update(TEntity obj)
        {
            conn.Update(obj);
        }

        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
