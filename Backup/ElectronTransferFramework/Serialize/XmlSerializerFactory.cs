using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ElectronTransferFramework.Serialize
{
    class XmlSerializerFactory : Singleton<XmlSerializerFactory>
    {
        Dictionary<Type, Dictionary<Type[], XmlSerializer>> _serializers = new Dictionary<Type, Dictionary<Type[], XmlSerializer>>();
        public XmlSerializer Create(Type type, Type[] types)
        {
            Dictionary<Type[], XmlSerializer> serializers = null;
            XmlSerializer serializer = null;
            if (!_serializers.TryGetValue(type, out serializers))
            {
                serializers = new Dictionary<Type[], XmlSerializer>();
                _serializers.Add(type, serializers);
            }
            if (!serializers.TryGetValue(types, out serializer))
            {
                serializer = new XmlSerializer(type, types);
                serializers.Add(types, serializer);
            }
            return serializer;
        }
    }
}
