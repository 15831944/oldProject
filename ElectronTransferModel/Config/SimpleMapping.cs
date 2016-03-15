using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ElectronTransferModel.Config
{
    /// <summary>
    /// 实体映射
    /// </summary>
    public class SimpleMapping
    {
        public const string MAP_FILE = "mapping.xml";
        public List<SimpleMappingPair> Mappings { get; set; }

        public SimpleMapping()
        {
            Mappings = new List<SimpleMappingPair>();
        }
        public void Add(SimpleMappingPair o)
        {
            Mappings.Add(o);
        }
        public string GetTableName(string className) 
        {
            var found = Mappings.FirstOrDefault(o => o.ClassName == className);
            if (found == null)
            {
                //to do .not exits;
                throw new Exception(className);
            }
            else 
            {
                return found.TableName;
            }
        }

        public string GetUpdateView(string className)
        {
            var found = Mappings.FirstOrDefault(o => o.ClassName == className);
            if (found == null)
            {
                return string.Empty;
            }
            else
            {
                return found.UpdateView;
            }
        }

        public string GetClassName(string tableName)
        {
            var found = Mappings.FirstOrDefault(o => o.TableName == tableName);
            if (found == null)
            {
                return string.Empty;
            }
            else
            {
                return found.ClassName;
            }
        }

        public static SimpleMapping Instance { get; private set; }

        public static SimpleMapping Load(string path) 
        {
            using (XmlReader reader = XmlReader.Create(path))
            {
                
                XmlSerializer serializer = new XmlSerializer(typeof(SimpleMapping));
                SimpleMapping mapping = (SimpleMapping)serializer.Deserialize(reader);
                Instance = mapping;
                return mapping;
            }
        }
        public void Save(string path) 
        {
            using( XmlTextWriter writer=new XmlTextWriter(path,Encoding.UTF8 ))
            {
                writer.Formatting = Formatting.Indented;
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
            }
        }
    }
}
