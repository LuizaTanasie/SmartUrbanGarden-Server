
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace DataAccess.DbContexts
{
    public class Context : SGContext
    {
        public Context(DbContextOptions<SGContext> options)
            : base(options)
        {
        }

        private readonly List<Type> builtInTypes = new List<Type> { typeof(int), typeof(string), typeof(bool), typeof(DateTime) };

        public Context New
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<SGContext>();
                optionsBuilder.UseSqlServer(Database.GetDbConnection().ConnectionString);
                return new Context(optionsBuilder.Options);
            }
        }

        #region SPs

        //public List<DailyRouteData> GetDailyRoutes(Guid charterId)
        //{
        //    var res = ExecuteSP<DailyRouteData>("Data.GetDailyRoutes",
        //                                        new Param("@charterID", charterId));


        //    return res.Result;
        //}


        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        private SPResult<T> ExecuteSP<T>(string spName, params Param[] spParams) where T : new()
        {
            SPResult<T> res = new SPResult<T>();
            var connection = Database.GetDbConnection();
            var initialConnectionState = connection.State;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                var cmd = (SqlCommand)connection.CreateCommand();
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var spParam in spParams)
                {
                    var param = cmd.Parameters.AddWithValue(spParam.Name, spParam.Value ?? DBNull.Value);
                    if (spParam is OutputParam)
                    {
                        param.Direction = ParameterDirection.Output;
                    }
                }


                using (var reader = cmd.ExecuteReader())
                {
                    res.Result = MapToList<T>(reader);
                }

                foreach (var spParam in spParams.Where(p => p is OutputParam))
                {
                    res.OutParams.Add(spParam.Name, cmd.Parameters[spParam.Name]);
                }

                return res;
            }
            finally
            {
                if (initialConnectionState == ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        private List<T> MapToList<T>(DbDataReader reader) where T : new()
        {
            var entities = new List<T>();
            if (builtInTypes.Contains(typeof(T)))
            {
                while (reader.Read())
                {
                    var val = reader.GetValue(0);
                    var entity = (val == DBNull.Value) ? null : val;
                    entities.Add((T)entity);
                }
                return entities;
            }

            var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

            while (reader.Read())
            {
                T entity = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i).ToUpper();
                    if (propDict.ContainsKey(fieldName))
                    {
                        var info = propDict[fieldName];
                        if (info != null && info.CanWrite)
                        {
                            var val = reader.GetValue(i);
                            info.SetValue(entity, val == DBNull.Value ? null : val, null);
                        }
                    }
                }
                entities.Add(entity);
            }

            return entities;
        }

    }

    public class Param
    {
        public Param(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class OutputParam : Param

    {
        public OutputParam(string name, object value)
            : base(name, value)
        {
        }
    }

    public class SPResult<T>
    {
        public SPResult()
        {
            OutParams = new Dictionary<string, SqlParameter>();
        }


        public List<T> Result { get; set; }
        public Dictionary<string, SqlParameter> OutParams { get; set; }
    }
}
