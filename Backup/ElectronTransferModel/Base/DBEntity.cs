using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ElectronTransferFramework;


namespace ElectronTransferModel.Base
{
    [Serializable]
    public abstract class DBEntity:ICloneable
    {
        [NonColumn]
        public EntityState EntityState { get; set; }
        [NonColumn]
        public EntityState2 EntityState2 { get; set; }
        [NonColumn]
        public bool Duplicated { get; set; }


        /// <summary>
        /// 撤销追加状态（true为Update，反之Insert）
        /// </summary>
        [NonColumn]
        public bool RedoState { set; get; }
        /// <summary>
        /// 是否坐标数据（pt表）
        /// </summary>
        [NonColumn]
        public bool IsCoordinate { set; get; }

        /// <summary>
        /// 在DBSymbolFinder缓存里是否删除
        /// </summary>
        [NonColumn]
        public bool IsErased { set; get; }

        /// <summary>
        /// 存储子设备的从属G3e_ID
        /// </summary>
        [NonColumn]
        public Int64? ErasedOWNER1_ID { set; get; }

        /// <summary>
        /// 馈线类别
        /// true当前编辑馈线
        /// 反之为参照馈线（不可编辑）
        /// </summary>
        [NonColumn]
        public bool KxType { set; get; }

        /// <summary>
        /// 版本号
        /// </summary>
        [NonColumn]
        public int Version { set; get; }

        #region ICloneable 成员

        public virtual object Clone()
        {
            return this.ToObjectCopy();   
        }

        #endregion
    }
}
