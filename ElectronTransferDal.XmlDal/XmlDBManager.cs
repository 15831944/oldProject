using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using System.ComponentModel;
using System.Linq.Dynamic;

namespace ElectronTransferDal.XmlDal
{
    public class XmlDBManager:IDBManager,IDisposable
    {
        public string FileName { get; set; }

        public string Password { get; set; }

        internal XmlDB DB {
            get { return _db; }
        }

        public bool Has(Type type) 
        {
            return _db.GetTable(type.Name) != null;
        }

        private XmlDB _db = null;
        public void Initialize() 
        {
            Reset();
        }

        public void InitializeAsync(EventHandler handler) 
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) => Initialize();
            worker.RunWorkerCompleted += (sender,e)=> handler(sender,e);
            worker.RunWorkerAsync();
        }
        public EventHandler OnLoad;
        public void Reset() 
        {
            if ( !string.IsNullOrEmpty(FileName))
            {
                if (_db != null)
                {
                    _db.Dispose();
                }
                _db = XmlDB.Load(FileName,Password);

            }
            else
            {
                throw new ArgumentNullException("FileName");
            }
        }
        #region IDBManager 成员

        public void Delete(DBEntity entity)
        {
            var type = entity.GetType();
            var table = GetTable(type);
            CheckTable(table, type.Name);
            var keyFields = KeyFieldCache.Instance.FindKeyFields(type);
            var found = table.Entities.FirstOrDefault( o=> ReflectionUtils.EqualsByProperties( entity, o ,keyFields.ToArray()));
            table.Remove(found);
        }
       
        private XmlTable GetTable(Type type) 
        {
            return GetTable(type,false);
        }

        private XmlTable GetTable(Type type,bool create) 
        {
            return GetTable(type.Name,create);
        }

        private XmlTable GetTable(string typeName,bool create)
        {
            var table = _db.GetTable(typeName);
            
            if (table == null && create) 
            {
                table = XmlTable.Create(typeName);
                _db.AddTable(table);
            }
            return table;
        }

        private void CheckTable(XmlTable table,string name) 
        {
            if (table == null)
            {
                throw new NotExistException(name + "不存在");
            }
        }

        private void CheckKeyFields( IEnumerable<PropertyInfo> properties,object obj) 
        {
            bool avaiable = ReflectionUtils.CheckPropertyAvailable(properties.ToArray(), obj);
            if (!avaiable) 
            {
                throw new NullKeyException(obj.GetType().Name+" " +string.Join(",", properties.Select(o => o.Name).ToArray()) + "键值没有赋值");
            }
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <exception cref="ElectronTransferDal.Common.Exceptions.NotExistException"/>
        /// <param name="type">实体类型</param>
        /// <returns>实体的公开枚举数</returns>
        public IEnumerable<DBEntity> GetEntities(Type type)
        {
            return GetEntitiesRaw(type);
        }
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <exception cref="ElectronTransferDal.Common.Exceptions.NotExistException"/>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体的公开枚举数</returns>
        public IEnumerable<T> GetEntities<T>() where T : DBEntity, new()
        {
            return GetEntitiesRaw(typeof (T)).Cast<T>();
        }

        IEnumerable<DBEntity> GetEntitiesRaw(Type type)
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Entities.Select(item => item.Clone()as DBEntity);
        }

        public IEnumerable<DBEntity> GetEntities(Type type, Expression<Func<DBEntity, bool>> expr)
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Entities.AsQueryable().Where(expr).Select(o => o.Clone()as DBEntity);
        }

        public IEnumerable<DBEntity> GetEntities(Type type, string expr)
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Entities.AsQueryable().Where(expr).Select(o => o.Clone() as DBEntity);
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <exception cref="ElectronTransferDal.Common.Exceptions.NotExistException"/>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expr">Lamba表达式</param>
        /// <returns>实体的公开枚举数</returns>
        public IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> expr) where T : DBEntity, new()
        {
            var type = typeof(T);
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Entities.Cast<T>().AsQueryable().Where(expr).Select(item => item.Clone() as T);
        }


        public IEnumerable<T> GetEntities<T>(string expr) where T : DBEntity, new() 
        {
            var type = typeof(T);
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Entities.Cast<T>().AsQueryable().Where(expr).Select(item => item.Clone() as T);
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <exception cref="ElectronTransferDal.Common.Exceptions.NotExistException"/>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体</returns>
        public T GetEntity<T>(Expression<Func<T, bool>> expr) where T : DBEntity, new()
        {
            var type = typeof(T);
            var table = GetTable(type);
            CheckTable(table, type.Name);
            var entity = table.Entities.Cast<T>().AsQueryable().SingleOrDefault(expr);
            return (entity != default(T)) ? entity.Clone() as T : null;
        }

        public T GetEntity<T>(string expr) where T : DBEntity, new()
        {
            var type = typeof(T);
            var table = GetTable(type);
            CheckTable(table, type.Name);
            var entity = table.Entities.Cast<T>().AsQueryable().Where(expr).SingleOrDefault();
            return (entity != default(T)) ? entity.Clone() as T : null;
        }

        public T GetEntity<T>(object key) where T : DBEntity, new()
        {
            return GetEntityRaw(typeof(T), key) as T;
        }

        public DBEntity GetEntity(Type type, Expression<Func<DBEntity, bool>> expr) 
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            var entity = table.Entities.AsQueryable().SingleOrDefault(expr);
            return (entity != null) ? entity.Clone() as DBEntity : null;
        }

        public DBEntity GetEntity(Type type, string expr)
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            var entity = table.Entities.AsQueryable().Where(expr).SingleOrDefault();
            return (entity != null) ? entity.Clone() as DBEntity : null;
        }

        public DBEntity GetEntity(Type type, object key) 
        {
            return GetEntityRaw(type, key);
        }

        private DBEntity GetEntityRaw(Type type, object key)
        {
            var table = GetTable(type);
            CheckTable(table, type.Name);
            return table.Get(key);
        }
        

        public bool InsertBulk(IEnumerable<DBEntity> entities)
        {
            foreach (var entity in entities) 
            {
                Insert(entity);
            }
            return true;
        }

        public bool InsertBulk_SetKxType(IEnumerable<DBEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.KxType = true;
                Insert(entity);
            }
            return true;
        }

        public bool InsertBulk(Type type,IEnumerable<DBEntity> entities)
        {
            return InsertBulkRaw(type, entities);
        }

        private bool InsertBulkRaw(Type type, IEnumerable<DBEntity> entities)
        {
            var table = GetTable(type, true);
            var keyFields = KeyFieldCache.Instance.FindKeyFields(type);
            foreach (var entity in entities)
            {
                CheckKeyFields(keyFields, entity);
            }
            table.AddRange(entities);
            return true;
        }

        public bool Insert(string typename, object entity)
        {

            var table = GetTable(typename,true);
            CheckTable(table, typename);
            var type = TypeCache.Instance.GetTypes(typeof (DBEntity))
                                .SingleOrDefault(o => o.Name == typename && !o.IsAbstract);// o.Namespace == "ElectronTransferModel.V9_4");
            if ( type == null ) throw new TypeLoadException(typename);
            table.AddEntity((DBEntity)ReflectionUtils.CreateObject(entity, type));
            return true;
        }

        public bool Insert(DBEntity entity)
        {
            return Insert(entity.GetType().Name, entity);
        }
        

        public void Update(DBEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            UpdateRaw(entity);
        }

        public void Update(DBEntity entity, object condition) 
        {
            var type = entity.GetType();
            var table = GetTable(type);
            var keyFields = KeyFieldCache.Instance.FindKeyFields(type);
            CheckTable(table, type.Name);
            CheckKeyFields(keyFields, entity);
            var conditionObject = ReflectionUtils.CreateObject(condition, type);
            DBEntity found = null;
            if (keyFields.Count == 1)
            {
                found = table.Get(conditionObject.GetValue(keyFields.First().Name));
            }
            else
            {
                found = table.Entities.FirstOrDefault(o => ReflectionUtils.EqualsByProperties(conditionObject, o, keyFields.ToArray()));
            }
            if (found != null) 
            {
                ReflectionUtils.Copy(entity, found);
            }
        }

        private void UpdateRaw(DBEntity entity)
        {
            Type type=entity.GetType();
            var table = GetTable(type);
            var keyFields = KeyFieldCache.Instance.FindKeyFields(type);
            CheckTable(table, type.Name);
            CheckKeyFields(keyFields, entity);
            DBEntity found = null;
            if (keyFields.Count == 1) 
            {
                found = table.Get( entity.GetValue( keyFields.First().Name) );
            }
            else 
            {
                found = table.Entities.FirstOrDefault(o => ReflectionUtils.EqualsByProperties(entity, o, keyFields.ToArray()));
            }
            if (found != null)
            {
                ReflectionUtils.Copy(entity, found);
            }
        }

        public void Submit()
        {
            _db.Save();
        }


        public int Count()
        {
            return _db.Tables.Count;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _db.Dispose();
        }

        #endregion
    }
}
