using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ElectronTransferFramework;
using ElectronTransferFramework.Serialize;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;
using System.Xml;
using ElectronTransferDal.Common;

namespace ElectronTransferDal.XmlDal
{
    [Serializable]
    public class XmlDB : IDisposable, ICanSerialize//:XmlSerialized<XmlDB>
    {
        private static string[] NAMESPACES = new string[] { "ElectronTransferModel.V9_4", "ElectronTransferModel.Geo" };
        private Dictionary<string, XmlTable> TableIndex { get; set; }
        private string _password=string.Empty;
        private XmlDB()
        {
            //Tables = new List<XmlTable>();
        }
        private void BuildIndex() 
        {
            TableIndex = this.Tables.ToDictionary(o => o.Name);
            Tables.ForEach(o => o.BuildIndex());
        }

        public static XmlDB Load(string filename) 
        {
            return Load(filename, string.Empty);
        }

        public static XmlDB Load(string filename,string password)
        {
            XmlDB db = null;
            var memoryStream = new MemoryStream();
            try
            {
                if (File.Exists(filename))
                {
                    var types = new List<Type>();
                    var compress = !string.IsNullOrEmpty(password) && CompressUtils.CheckZip(filename, password);
                    if (compress)
                    {
                        using (var fileStream = new FileStream(filename, FileMode.Open))
                        {
                            CompressUtils.Decompress(fileStream, memoryStream, password);
                        }
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }

                    using (var xmlStream = compress ? (Stream) memoryStream : new FileStream(filename, FileMode.Open))
                    {
                        using (var reader = XmlReader.Create(xmlStream))
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "DBEntity")
                                {
                                    var typeName = reader.GetAttribute("xsi:type");
                                    types.Add(TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), typeName));
                                }
                            }
                        }

                        types.AddRange(ReflectionUtils.FindTypes(typeof (DBEntity).Assembly, typeof (Geometry)));

                        xmlStream.Seek(0, SeekOrigin.Begin);
                        db = XmlSerializeUtils.Load<XmlDB>(xmlStream, types.Distinct().ToArray()
                                                           /*GetExtraTypes(NAMESPACES)*/, string.Empty);
                        db.DecodeUnprited();
                    }
                }else
                {
                    db = new XmlDB { FileName = filename, Tables = new List<XmlTable>() };
                }
            }
            catch (Exception ex)//check it later
            {
                db = new XmlDB { FileName = filename, Tables = new List<XmlTable>() };
                LogManager.Instance.Error(ex);
            }
            finally
            {
                db.FileName = filename;
                db.BuildIndex();
                memoryStream.Dispose();
            }
            return db;
        }
        public XmlTable GetTable(string name)
        {
            try
            {
                return TableIndex[name];
            }
            catch(KeyNotFoundException)
            {
                return null;
            }
        }
        public void AddTable(XmlTable table) 
        {
            Tables.Add(table);
            TableIndex.Add(table.Name, table);
        }
        public static XmlDB Create(string filename)
        {
            XmlDB db = null;
            //if (File.Exists(filename))
            //{
            //    try
            //    {
            //        return XmlDB.Load(filename);
            //    }
            //    catch (Exception ex)//modify later
            //    {
            //    }
            //}
            //else
            //{
                db = new XmlDB { FileName = filename, Tables = new List<XmlTable>() };
                db.BuildIndex();
            //}
            return db;
        }
        static Type[] GetExtraTypes(string[] namespaces)
        {
            var modelAssembly = typeof(DBEntity).Assembly;
            var electronTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(ElectronBase));
            var geoTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(Geometry));
            var dbEntityTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(DBEntity));
            return electronTypes.Concat(geoTypes).Concat(dbEntityTypes).Where(o => namespaces.Contains( o.Namespace)).ToArray();
        }
        public void Save()
        {
            this.EncodeUnprited();
            Save(FileName,_password);
        }
        public void Save(string filename) 
        {
            Save(filename, string.Empty);
        }
        static XmlDB()
        {
            var modelAssembly = typeof(DBEntity).Assembly;
            var geoTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(Geometry));
            var dbEntityTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(DBEntity));//Tables.Select(o => o.EntityType);
            _extraTypes = geoTypes.Concat(dbEntityTypes).ToArray();
        }
        static Type[] _extraTypes = null;
        private Type[] GetExtraTypes()
        {
            //if ( _extraTypes == null)
            //{
            //    var modelAssembly = typeof(DBEntity).Assembly;
            //    var geoTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(Geometry));
            //    var dbEntityTypes = ReflectionUtils.FindTypes(modelAssembly,typeof(DBEntity));//Tables.Select(o => o.EntityType);
            //    _extraTypes = geoTypes.Concat(dbEntityTypes).ToArray();
            //}
            return _extraTypes;
            //return geoTypes.Concat(dbEntityTypes).ToArray();

        }
        public void Save(string fileName,string password)
        {
            //var modelAssembly = typeof(DBEntity).Assembly;
            ////var electronTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(ElectronBase));
            //var geoTypes = ReflectionUtils.FindTypes(modelAssembly, typeof(Geometry));
            //var dbEntityTypes = Tables.Select(o => o.EntityType);
            //var types = geoTypes.Concat(dbEntityTypes).ToArray();
            //_password = password;
            this.SaveAs(fileName, GetExtraTypes() /*GetExtraTypes(NAMESPACES)*/,password);
        }
        //[NonSerialized]
        //private string _filename;
        //public string Filename { get { return _filename; } set { _filename = value; } }
        [XmlIgnore]
        public string FileName { get;private set; }
        public List<XmlTable> Tables { get; set; }

        #region IDisposable 成员

        public void Dispose()
        {
            this.TableIndex.Clear();
            this.Tables.ForEach(o=>o.Dispose());
            this.Tables.Clear();
        }

        #endregion
    }
}
