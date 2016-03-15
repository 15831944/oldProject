using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferFramework.TextProcess;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using System.Linq.Expressions;
using CYZFramework.DB;
using CYZFramework.Log;

namespace ElectronTransferDal.Common
{
    /// <summary>
    /// 关系数据库管理器基类
    /// </summary>
    public abstract class RDBManagerBase : IDBManager
    {
        private SimpleMapping _mapping;
        //private IList<string> _avoidFields;
        public ISequenceValueGenerator SequenceValueGenerator { get; protected set; }
        protected QueryVersion QueryVersion { get; set; }


        protected RDBManagerBase()
        {
            Initialize();
        }

        private void Initialize()
        {

            var path = Path.Combine(Assembly.GetExecutingAssembly().GetPhysicalDirectory(), SimpleMapping.MAP_FILE);
            _mapping = SimpleMapping.Load(path);

            OnInitialize();
        }

        private string GetTableName(Type type)
        {
            return _mapping.GetTableName(type.Name);
        }

        protected SimpleMapping Mapping
        {
            get { return _mapping; }
        }

        protected virtual void OnInitialize()
        {
        }

        protected abstract Geometry GetGeometry(string valueString);

        protected abstract GeometryQuery GeometryQuery { get; }

        protected abstract object GetValue(DbDataReader reader, PropertyInfo property);

        protected virtual DBEntity ConstructEntity(DbDataReader reader, Type type)
        {
            //check the type later
            return ConstructEntityRaw(reader, type);
        }


        protected virtual DataTableConverter GetConverter() 
        {
            return new DataTableConverter();
        }

        private T ConstructEntity<T>(DbDataReader reader) where T : DBEntity, new()
        {
            return ConstructEntity(reader, typeof(T)) as T;
        }

        private T ConstructEntity<T>(DataTable table) where T : DBEntity, new()
        {
            return ConstructEntity(table, typeof(T)) as T;
        }

        private bool IsNonColumn(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(NonColumnAttribute), true).Any();
        }

        private bool IsSelectOnlyField(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(SelectOnlyAttribute), true).Any();
        }

        private IEnumerable<string> GetAvoidFields(Type type)
        {
            return GetAvoidFields(type, false);
        }

        private IEnumerable<string> GetAvoidFields(Type type, bool isUpdate)
        {

            return
                type.GetProperties()
                    .Where(o => IsNonColumn(o) || (isUpdate && IsSelectOnlyField(o)))
                    .Select(o => o.Name);

        }

        private DBEntity ConstructEntityRaw(DbDataReader reader, Type type)
        {

            DBEntity entity = Activator.CreateInstance(type) as DBEntity;
            bool hasGemetry = false;
            foreach (var property in type.GetProperties())
            {
                if (IsNonColumn(property))
                    continue;

                if (property.Name == GeometryQuery.GeometryField)
                {
                    hasGemetry = true;
                }
                else if (HasColumn(reader, property))
                {
                    var value = GetValue(reader, property);
                    property.SetValue(entity, value, null);                                                                                                                                                                                                    
                }
            }
            if (hasGemetry)
            {
                MakeGeometry(entity);
            }
            
            return entity;
        }

        private bool HasColumn(DbDataReader reader,PropertyInfo property) 
        {
            try 
            {
                reader.GetOrdinal(property.Name);
                return true;
            }
            catch 
            {
                return false;
            }
              
        }
        protected abstract string FetchGeometryText(string valueString);
        protected abstract void MakeGeometry(DBEntity entity);

        //protected DbConnection GetConnection()
        //{
        //    try
        //    {
        //        var connection =
        //            ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["dbInstance"]];

        //        var conn = DbProviderFactories.GetFactory(connection.ProviderName).CreateConnection();
        //        conn.ConnectionString = connection.ConnectionString;

        //        return conn;

        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Instance.Error(ex);
        //        throw;
        //    }


        //}

        protected abstract string[] GetNameAndPwd();
        protected DbConnection GetConnection()
        {
            try
            {
                var connection =
                       ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["dbInstance"]];
                var conn = DbProviderFactories.GetFactory(connection.ProviderName).CreateConnection();
                if (string.IsNullOrEmpty(GetNameAndPwd()[0]) || string.IsNullOrEmpty(GetNameAndPwd()[1]))
                {
                    conn.ConnectionString = connection.ConnectionString;
                }
                else
                {
                    string[] connectiongStr = connection.ConnectionString.Split(';');
                    var ipStr = connectiongStr[2].Split('=')[1];
                    var connStr = string.Format("User Id={0};Password={1};Data Source={2}", GetNameAndPwd()[0], GetNameAndPwd()[1], ipStr);
                    conn.ConnectionString = connStr;

                }
                return conn;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                throw;
            }


        }

        public object RunSqlScalar(string sql)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                return cmd.ExecuteScalar();
            }
        }

        #region IDBManager 成员

        public void Delete<T>(T entity, bool byView, params ISurround[] surrounds) where T : DBEntity
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Runsurrounds(surrounds, conn);

                DeleteItem(entity, byView, conn);

                RunEnds(surrounds, conn);
            }
        }

        public void DeleteBulk(IEnumerable<DBEntity> entities, bool byView, params ISurround[] surrounds) 
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Runsurrounds(surrounds, conn);
                foreach (var entity in entities)
                {
                 //  LogManager.Instance.Error("DEL" + (long)entity.GetValue("G3E_FID")+"");
                    DeleteItem(entity, byView, conn);
                }
                RunEnds(surrounds, conn);
            }
        }
        protected void DeleteItem(DBEntity entity, bool byView, DbConnection conn)
        {
            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    var type = entity.GetType();
                    var deleteQueryBuilder = new DeleteQueryBuilder(entity, type, _mapping, byView);
                    cmd.CommandText = deleteQueryBuilder.ToString();
                    var parametersBuilder = new ParametersBuilder(entity, cmd, deleteQueryBuilder.Fields);
                    cmd.Parameters.AddRange(parametersBuilder.Parameters.ToArray());
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Delete(DBEntity entity)
        {
            Delete(entity, false);
        }

        public IEnumerable<DBEntity> GetEntities(Type type)
        {

            type.CheckDbType();

            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion,type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     null, false);
            return GetEntitiesRaw(type, queryBuilder.ToString());

        }

        public IEnumerable<DBEntity> GetEntities(Type type,string condition)
        {

            type.CheckDbType();
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     null, false);
            return GetEntitiesRaw(type, string.Format("{0} {1}",queryBuilder.ToString(),condition));

        }

        public IEnumerable<T> GetEntities<T>() where T : DBEntity, new()
        {
            return GetEntities(typeof(T)).Cast<T>();
        }


        private IEnumerable<T> GetEntitiesRaw<T>(string expr)
            where T : DBEntity, new()
        {
            using (var conn = GetConnection())
            {
                var reader = ExecuteReader(conn, expr);
                var type = typeof(T);
                DataTable table = GetConverter().Convert(reader, type.GetProperties());
                //table.Load(reader);
                return ConstructEntities(table, type).Cast<T>();
                //return ConstructEntities(reader,typeof(T)).Cast<T>();
                //while (reader != null && reader.Read())
                //    yield return ConstructEntity<T>(reader);
            }
        }

        private DbDataReader ExecuteReader(DbConnection conn, string expr)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = expr;
            try
            {
                conn.Open();
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                
                LogManager.Instance.Error(ex);
                throw;
            }
        
        }

        private IEnumerable<DBEntity> GetEntitiesRaw(Type type,string expr)
        {

            using(var conn = GetConnection()){
                var reader = ExecuteReader(conn, expr);
                DataTable table = GetConverter().Convert(reader, type.GetProperties());//new DataTable();
                //table.Load(reader);
                return ConstructEntities(table, type);
                //return ConstructEntities(reader, type);
            }
        }

        protected virtual IEnumerable<DBEntity> ConstructEntities(DbDataReader reader, Type type) 
        {
            while (reader != null && reader.Read())
                yield return ConstructEntity(reader, type);
        }


        protected virtual IEnumerable<DBEntity> ConstructEntities(DataTable table, Type type) 
        {
            foreach (DataRow row in table.Rows) 
            {
                yield return ConstructEntity(row, type);
            }
        }

        protected virtual DBEntity ConstructEntity(DataTable table, Type type)
        {
            if (table.Rows.Count > 0)
                return ConstructEntity(table.Rows[0], type);
            else
                return null;
        }

        private DBEntity ConstructEntity(DataRow row, Type type) 
        {
            DBEntity entity = Activator.CreateInstance(type) as DBEntity;
            bool hasGemetry = false;
            foreach (var property in type.GetProperties())
            {
                if (IsNonColumn(property))
                    continue;

                if (property.Name == GeometryQuery.GeometryField)
                {
                    hasGemetry = true;
                }
                else //if (HasColumn(reader, property))
                {
                    try
                    {
                        var value = row[property.Name];//GetValue(reader, property);
                        property.SetValue(entity, value, null);
                    }
                    catch(Exception ex)
                    {
                    }
                }
            }
            if (hasGemetry)
            {
                MakeGeometry(entity);
            }

            return entity;
        }

        public IEnumerable<T> GetEntities<T>(string condition)
            where T : DBEntity, new()
        {
            StringBuilder sb = new StringBuilder();
            var type = typeof(T);
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     null, false);

            sb.Append(queryBuilder);
            sb.Append(" ");
            sb.Append(condition);
            return GetEntitiesRaw<T>(sb.ToString());
        }

        public IEnumerable<DBEntity> GetEntities(Type type, Expression<Func<DBEntity, bool>> expr) 
        {
            //check it later
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     expr, false);
            return GetEntitiesRaw(type,queryBuilder.ToString());
        }

        public IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> expr)
            where T : DBEntity, new()
        {
            var type = typeof(T);
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     expr, false);
            return GetEntitiesRaw<T>(queryBuilder.ToString());
        }

        public T GetEntity<T>(Expression<Func<T, bool>> expr) where T : DBEntity, new()
        {
            var type = typeof(T);
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     expr, true);
            using (var conn = GetConnection())
            {
                DbDataReader reader = ExecuteReader(conn,queryBuilder.ToString());
                var table = GetConverter().Convert(reader, type.GetProperties());
                return ConstructEntity<T>(table);
                //if (reader != null && reader.Read())
                //    return ConstructEntity<T>(reader);
                //else
                //    return default(T);
            }
        }

        public T GetEntity<T>(object key) where T : DBEntity, new() 
        {
            throw new NotImplementedException();
        }

        public DBEntity GetEntity(Type type, Expression<Func<DBEntity, bool>> expr)
        {
            //check it later
            QueryBuilder queryBuilder = SelectQueryBuilderFactory.Instance.Create(QueryVersion, type, GetAvoidFields(type), _mapping, GeometryQuery,
                                                                     expr, true);
            using (var conn = GetConnection())
            {
                DbDataReader reader = ExecuteReader(conn, queryBuilder.ToString());
                var table = GetConverter().Convert(reader, type.GetProperties());
                return ConstructEntity(table,type);
                //if (reader != null && reader.Read())
                //    return ConstructEntity<T>(reader);
                //else
                //    return default(T);
            }
        }
        public DBEntity GetEntity(Type type, object key) 
        {
            throw new NotImplementedException();
        }


        public bool Insert(string typename, object entity, params ISurround[] surrounds)
        {
            return Insert(typename, entity, false, surrounds);
        }

        public bool Insert(DBEntity entity) 
        {
            throw new NotImplementedException();
        }

        public bool Insert(string typename, object entity)
        {
            return Insert(typename, entity, false);
        }

        public bool Insert(DBEntity entity, bool byView, params ISurround[] surrounds) 
        {
            var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), entity.GetType().Name);
            return InsertProtected(type, entity, byView, surrounds);
        }

        public bool Insert(string typename, object entity, bool byView, params ISurround[] surrounds)
        {
            var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity),typename);
            return InsertProtected(type, entity, byView, surrounds);
        }

        protected virtual bool InsertProtected(Type type, object entity, bool byView, params ISurround[] surrounds)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Runsurrounds(surrounds, conn);
                InsertItemWrap(type, entity, byView, conn);
                RunEnds(surrounds, conn);
                return true;
            }
        }


        protected void Runsurrounds(ISurround[] surrounds, DbConnection conn)
        {
            try
            {
                foreach (var surround in surrounds)
                {
                    CYZLog.writeLog(surround.ToString());
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = surround.Begin();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) 
            {
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }
        }

        protected void RunEnds(ISurround[] surrounds, DbConnection conn)
        {
            try
            {
                foreach (var surround in surrounds)
                {
                    CYZLog.writeLog(surround.ToString());
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = surround.End();
                    cmd.ExecuteNonQuery();
                }
            }catch(Exception ex)
            {
                CYZLog.writeLog(ex.ToString());
                throw ex;
            }
        }

        void InsertItemWrap(Type type, object entity, bool byView, DbConnection conn) 
        {
            try
            {
                InsertItem(type, entity, byView, conn);
            }
            catch (Exception ex) 
            {
               
                LogManager.Instance.Error(ex);
                throw ex;
            }
        }

        protected virtual void InsertItem(Type type, object entity, bool byView, DbConnection conn)
        {
            using (var transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                
                var insertCmd = conn.CreateCommand();
                insertCmd.Transaction = transaction;
                try
                {
                    var avoidFields = GetAvoidFields(type, byView);
                    var insertQuerybuilder = new InsertQueryBuilder(entity, type,
                                                                    avoidFields.Concat(new string[] { GeometryQuery.GeometryField }), _mapping,
                                                                    GeometryQuery, byView);


                    //var insertCmd = conn.CreateCommand();
                    if (type.GetType().Name.Contains("Connectivity_n"))
                    {
                        CYZLog.writeLog(insertQuerybuilder.ToString());
                    }
                  

                    insertCmd.CommandText = insertQuerybuilder.ToString();
                    ParametersBuilder tempParameterBulider = new ParametersBuilder(entity, insertCmd, insertQuerybuilder.Fields);

                    insertCmd.Parameters.AddRange(tempParameterBulider.Parameters.ToArray());

                    insertCmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    LogManager.Instance.Error(ex);
                    transaction.Rollback();
                    throw ex;

                }
            }

           
           
        }

        


        public bool InsertBulk(IEnumerable<DBEntity> entities)
        {
            return InsertBulk(entities, false);
        }

        public bool InsertBulk(IEnumerable<DBEntity> entities, bool byView, params ISurround[] surrounds)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    Runsurrounds(surrounds, conn);
                    foreach (var entity in entities)
                    {
                        //if ((int)entity.GetValue("G3E_FNO") == 84)
                        //{
                        //    LogManager.Instance.Error("ASAS");
                        //}
                       
                        InsertItemWrap(entity.GetType(), entity, byView, conn);
                        CYZLog.writeLog(entity.GetType().ToString()+":",((long)entity.GetValue("G3E_ID")).ToString()+".");
                        
                        
                        
                    }
                    RunEnds(surrounds, conn);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                
                throw ex;
            }

            return true;
        }

        void UpdateItemWrap(Type type, object entity, bool byView, DbConnection conn) 
        {
            try
            {
                UpdateItem(type, entity, byView, conn);
            }
            catch (Exception ex) 
            {     

                LogManager.Instance.Error(ex);
                throw ex;
            }
        }
        protected virtual void UpdateItem( Type type, object entity, bool byView,DbConnection conn)
        {
            using (var transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                try
                {

                    var avoidFields = GetAvoidFields(type, byView);
                    var updateQueryBuilder = new UpdateQueryBuilder(entity, type, avoidFields, _mapping, GeometryQuery,
                                                                    byView);
                    string sql = updateQueryBuilder.ToString();
                    var parametersBuilder = new ParametersBuilder(entity, cmd, updateQueryBuilder.Fields);
                    cmd.Parameters.AddRange(parametersBuilder.Parameters.ToArray());
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void UpdateRaw(Type type,object entity, bool byView, params ISurround[] surrounds)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Runsurrounds(surrounds, conn);
                UpdateItemWrap( type, entity, byView,conn);
                RunEnds(surrounds, conn);
            }
        }

        protected virtual void UpdateProtected(Type type,object entity, bool byView, params ISurround[] surrounds)
        {
            UpdateRaw(type,entity, byView, surrounds);
        }

        public void UpdateBulk(IEnumerable<DBEntity> entities, bool byView, params ISurround[] surrounds)
        {
            
            using (var conn = GetConnection())
            {
                conn.Open();
                Runsurrounds(surrounds, conn);
                foreach( var entity in entities)
                    UpdateItemWrap(entity.GetType(),entity,byView,conn);
                RunEnds(surrounds,conn);
            }
        }


        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Update(DBEntity entity, bool byView, params ISurround[] surrounds) 
        {
            UpdateProtected(entity.GetType(), entity, byView, surrounds);
        }

        public void Update(DBEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(DBEntity entity, object condition)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
