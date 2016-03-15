using System.Data.Common;
using ElectronTransferDal.Common;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    internal class GeometryCommandFactoryImplement : ElectronTransferDal.OracleDal.IGeometryCommandFactory
    {
        public GeometryCommand GetInsertCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, bool byView, DbConnection conn)
        {
            if (byView)
            {
                return new GeometryInsertViewCommand(entity,mapping,dbManager,conn);
            }
            else
            {
                return new GeometryInsertTableCommand(entity, mapping, dbManager, conn);
            }
        }

        public GeometryCommand GetUpdateCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, bool byView, DbConnection conn)
        {
            if (byView)
            {
                return new GeometryUpdateViewCommand(entity, mapping, dbManager, conn);
            }
            else
            {
                return new GeometryUpdateTableCommand(entity, mapping, dbManager, conn);
            }
        }
    }

    public class GeometryCommandFactory : Singleton<GeometryCommandFactory>,IGeometryCommandFactory
    {
        IGeometryCommandFactory _factory = new GeometryCommandFactoryImplement();
        #region IGeometryCommandFactory 成员

        public GeometryCommand GetInsertCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, bool byView, DbConnection conn)
        {
            return _factory.GetInsertCommand(entity, mapping, dbManager, byView,conn);
        }

        public GeometryCommand GetUpdateCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, bool byView, DbConnection conn)
        {
            return _factory.GetUpdateCommand(entity, mapping, dbManager, byView,conn);
        }

        #endregion
    }
}
