using System.Data.Common;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    public abstract class GeometryCommand
    {
        protected RDBManagerBase DbManager;
        protected ElectronSymbol Entity;
        protected DbConnection Connection;
        protected SimpleMapping Mapping;
        protected GeometryCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, DbConnection conn)
        {
            Entity = entity;
            DbManager = dbManager;
            Mapping = mapping;
            Connection = conn;
        }

        public abstract bool Execute();
    }
}
