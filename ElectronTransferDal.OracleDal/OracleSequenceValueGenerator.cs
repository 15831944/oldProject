using System;
using ElectronTransferDal.Common;
using ElectronTransferFramework;

namespace ElectronTransferDal.OracleDal
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public class OracleSequenceValueGenerator : Singleton<OracleSequenceValueGenerator>, ISequenceValueGenerator
    {
        ISequenceValueGenerator _generator = new OracleSequenceValueGeneratorImplement();
        #region ISequenceValueGenerator 成员

        public RDBManagerBase DbManager
        {
            get
            {
                return _generator.DbManager;
            }
            set
            {
                _generator.DbManager = value;
            }
        }

        public long GenerateTableId(Type type)
        {
            return _generator.GenerateTableId(type);
        }

        public long GenerateGlobalId()
        {
            return _generator.GenerateGlobalId();
        }

        public long GenerateNodeId() 
        {
            return _generator.GenerateNodeId();
        }

        public long GenerateDetailId() 
        {
            return _generator.GenerateDetailId();
        }
        #endregion
    }

    public class OracleSequenceValueGeneratorImplement : ISequenceValueGenerator
    {
        /// <summary>
        /// 生成G3E_ID
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public long GenerateTableId(Type type)
        {
            return RunSequence(type.Name.ToUpper()+"_SEQ");
        }

        private long RunSequence(string sequenceName)
        {
            string sql = string.Format("SELECT {0}.NEXTVAL FROM DUAL", sequenceName);
            return (long)(decimal)DbManager.RunSqlScalar(sql);
        }
        /// <summary>
        /// 生成G3E_FID
        /// </summary>
        /// <returns></returns>
        public long GenerateGlobalId()
        {
            return RunSequence("G3E_FID_SEQ");
        }
        /// <summary>
        /// 生成节点ID
        /// </summary>
        /// <returns></returns>
        public long GenerateNodeId() 
        {
            return RunSequence("G3E_NODE_SEQ");
        }
        public RDBManagerBase DbManager { get; set; }

        public long GenerateDetailId() 
        {
            return RunSequence("G3E_DETAILID_SEQ");
        }
    }
}
