using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElectronTransferModel.Geo;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ElectronTransferModel.Base
{
    /// <summary>
    /// 所有设备的符号基类
    /// </summary>
    /// 
    [Serializable]
    public abstract class ElectronSymbol : ElectronBase
    {
        /// <summary>
        /// 块定义名称
        /// </summary>
        [NonColumn]
        public string BlockName { set; get; }
        /// <summary>
        /// 自定义符号基点
        /// </summary>
        [NonColumn]
        public virtual Geometry BasePoint { get; set; }
        /// <summary>
        /// 符号类型
        /// </summary>
        [NonColumn]
        public EntityType EntityType { set; get; }
        /// <summary>
        /// 查找标注的函数序列号
        /// </summary>
        [NonColumn]
        public int FinderFuncNumber { set; get; }
        /// <summary>
        /// 最原始的G3e_Fid
        /// </summary>
        [NonColumn]
        public long OriginalG3e_Fid { set; get; }
        ///// <summary>
        ///// 数据类型（true为原始数据，false为新增数据)
        ///// </summary>
        //[NonColumn]
        //public bool DBEntityType { set; get; }

        [XmlIgnore]
        private DBEntityHolder _holder = new DBEntityHolder();

        public void AddSibling(ElectronBase sibling) 
        {
            _holder.Add(sibling);
        }

        public void ClearSiblings() 
        {
            _holder.Clear();
        }
        
        public IEnumerable<ElectronBase> GetSiblings() {  return _holder.Where(o => o != this);  }

        public virtual Geometry G3E_GEOMETRY { get; set; }

        public override object Clone()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        //public ElectronSymbol()
        //{
        //    common_n = new COMMON_N();           
        //}

        //private COMMON_N _common_n;
        ///// <summary>
        ///// 公共属性
        ///// </summary>
        //public COMMON_N common_n
        //{
        //    get { return _common_n; }
        //    set { _common_n = value; }
        //}

        //private ElectronItselfParams _itselfParams;
        ///// <summary>
        ///// 自身属性属性
        ///// </summary>
        //public ElectronItselfParams itselfParams
        //{
        //    get { return _itselfParams; }
        //    set { _itselfParams = value; }
        //}
    }
}
