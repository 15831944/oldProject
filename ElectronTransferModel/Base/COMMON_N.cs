using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.Base
{
    /// <summary>
    /// 公共属性
    /// </summary>
    public class COMMON_N : ElectronBase
    {
        public COMMON_N() 
        {
            //gtype = egtype.none;
        }

        /// <summary>
        /// 功能位置名称	
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 功能位置编码	
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 所属市局
        /// </summary>
        public string unit{ get; set; }
        /// <summary>
        /// 所属区局	 
        /// </summary>
        public string Zones{ get; set; }
        /// <summary>
        /// 所属供电所
        /// </summary>
        public string ErpOrganisation{ get; set; }
        /// <summary>
        /// 设备编号	 
        /// </summary>
        public string mRID{ get; set; }
        /// <summary>
        /// 所在变电站	
        /// </summary>
        public string substation{ get; set; }
        /// <summary>
        /// 所在线路
        /// </summary>
        public string Circuits{ get; set; }
        /// <summary>
        /// 是否干线/支线	
        /// </summary>
        public string isTrunk{ get; set; }
        /// <summary>
        /// 所在干线/支线	
        /// </summary>
        public string connection_kind{ get; set; }
        /// <summary>
        /// 资产属性	
        /// </summary>
        public string AssetClassification{ get; set; }
        /// <summary>
        /// 电压等级
        /// </summary>
        public string BaseVoltage{ get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        public string 	status{ get; set; }
        /// <summary>
        /// 投运日期
        /// </summary>
        public DateTime commissioningDate{ get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string AssetModel{ get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        public string manufacturer{ get; set; }
        /// <summary>
        /// 所在台区
        /// </summary>
        public string TransformerDistrict{ get; set; }
        /// <summary>
        /// 所在低压线路
        /// </summary>
        public string lowcircuit{ get; set; }
        /// <summary>
        /// 所在低压支线
        /// </summary>
        public string LowBranch{ get; set; }
        /// <summary>
        /// 安装环境
        /// </summary>
        public ulong installPosition{ get; set; }
        /// <summary>
        /// 户内设备所在电房
        /// </summary>
        public string electricroom{ get; set; }
        /// <summary>
        /// 所在电柜
        /// </summary>
        public string Cabinet{ get; set; }
        /// <summary>
        /// 进出线符号名称
        /// </summary>
        public string Inoutlinename{ get; set; }
        /// <summary>
        /// 进出线符号状态
        /// </summary>
        public string Inoutlinestatus{ get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description{ get; set; }
        /// <summary>
        /// 最后编辑工单
        /// </summary>
        public string lasteditjob{ get; set; }
        /// <summary>
        /// 最后编辑人
        /// </summary>
        public string lastedituser{ get; set; }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime lastedittime{ get; set; }
        /// <summary>
        /// 原系统键值
        /// </summary>
        public ulong Oldg3e_fid{ get; set; }
        /// <summary>
        /// 所属市局编码
        /// </summary>
        public string 	Unit_code{ get; set; }
        /// <summary>
        /// 所属区局编码
        /// </summary>
        public string Zones_code{ get; set; }
        /// <summary>
        /// 所属供电所编码
        /// </summary>
        public ulong ErpOrganisation_code{ get; set; }
        /// <summary>
        /// 所在变电站键值
        /// </summary>
        public ulong substation_fid{ get; set; }
        /// <summary>
        /// 所在线路键值
        /// </summary>
        public ulong Circuits_fid{ get; set; }
        /// <summary>
        /// 所在干线/支线键值
        /// </summary>
        public ulong connection_kind_fid{ get; set; }
        /// <summary>
        /// 所在台区键值
        /// </summary>
        public ulong TransformerDistrict_fid{ get; set; }
        /// <summary>
        /// 所在低压线路键值
        /// </summary>
        public ulong Lowcircuit_fid{ get; set; }
        /// <summary>
        /// 所在低压支线键值
        /// </summary>
        public ulong LowBranch_fid{ get; set; }
        /// <summary>
        /// 安装环境编码
        /// </summary>
        public ulong installPosition_code{ get; set; }
        /// <summary>
        /// 户内设备所在电房键值
        /// </summary>
        public ulong Electricroom_fid{ get; set; }
        /// <summary>
        /// 所在开关柜键值
        /// </summary>
        public ulong Cabinet_fid{ get; set; }
        /// <summary>
        /// 配网生产键值
        /// </summary>
        public ulong pwmis_id{ get; set; }
        /// <summary>
        /// 资产属性编码
        /// </summary>
        public string AssetClassification_code{ get; set; }
        /// <summary>
        /// 电压等级编码
        /// </summary>
        public string BaseVoltage_code{ get; set; }
        /// <summary>
        /// 运行状态编码
        /// </summary>
        public string Status_code{ get; set; }
        /// <summary>
        /// 型号键值
        /// </summary>
        public ulong AssetModel_id{ get; set; }
        /// <summary>
        /// 生产厂商键值
        /// </summary>
        public ulong Manufacturer_id{ get; set; }


    }
}
