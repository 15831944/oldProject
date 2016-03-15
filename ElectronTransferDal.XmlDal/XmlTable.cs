using System;
using System.Linq;
using System.Collections.Generic;
using ElectronTransferModel.Base;
using System.Xml.Serialization;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using System.Reflection;

namespace ElectronTransferDal.XmlDal
{
    [Serializable]
    public class XmlTable:IDisposable
    {
        Dictionary<object, DBEntity> _index = new Dictionary<object, DBEntity>();
        private XmlTable()
        {
            //Entities = new List<DBEntity>();
        }
        private List<PropertyInfo> Keys { get; set; }

        private string KeyName { get; set; }
        private Type _type;
        internal Type EntityType { 
            get { return _type; }
            set 
            { 
                _type = value;
                KeyName = GetKeyName(_type);
                Keys = KeyFieldCache.Instance.FindKeyFields(_type).ToList();
            }
        }



        private string GetKeyName(Type type)
        {
            try
            {
                return KeyFieldCache.Instance.FindKeyFields(type).FirstOrDefault().Name;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static XmlTable Create(string name)
        {
            return new XmlTable { Name = name, Entities = new List<DBEntity>(), 
                EntityType = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity),name) };
        }
        public string Name { get; set; }

        public List<DBEntity> Entities { get; set; }

        private bool IgnoreIndex { get { return /*!EntityType.IsSubclassOf(typeof(ElectronBase))*/ Keys.Count!=1|| string.IsNullOrEmpty(KeyName); } }

        public void AddEntity(DBEntity entity) 
        {
            Entities.Add(entity);
            if (IgnoreIndex)
                return;
            AddToIndex(entity);
        }



        public void AddRange(IEnumerable<DBEntity> entities) 
        {
            Entities.AddRange(entities);
            foreach (var entity in entities) 
            {
                if (IgnoreIndex)
                    continue;
                AddToIndex(entity);
            }
        }


        public void BuildIndex() 
        {
            if (EntityType == null)
                EntityType = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), Name);
            if ( IgnoreIndex )
                return;
            Entities.ForEach(AddToIndex);

        }

        //private DbKey BuildDbkey(DBEntity entity)
        //{
        //    var key = new DbKey();
        //    Keys.ForEach( o=> key.Add(o.Name, entity.GetValue(o.Name)));
        //    return key;
        //}

        private object GetKey(DBEntity entity) 
        {
            if (Keys.Count==1)
            {
                return entity.GetValue(KeyName);
            }
            //else if (Keys.Count>1)
            //{
            //    return BuildDbkey(entity);
            //}
            return null;
        }

        private void AddToIndex(DBEntity entity)
        {
            try
            {
                _index.Add(GetKey(entity), entity);
            }
            catch(Exception ex) 
            {
                entity.Duplicated = true;
            }
        }

        public void Remove(DBEntity entity) 
        {
            this.Entities.Remove(entity);
            RemoveIndex(entity);
        }

        private void RemoveIndex(DBEntity entity) 
        {
            try
            {
                var obj = GetKey(entity);
                if (obj == null)
                    return;
                if (_index.ContainsKey(obj))
                    _index.Remove(obj);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        public DBEntity Get(object key) 
        {
            DBEntity result = null;
            if (_index.TryGetValue(key, out result))
            {
                return result;
            }
            else
                return null;
        }
        #region IDisposable 成员

        public void Dispose()
        {
            Entities.Clear();
        }

        #endregion
    }
}