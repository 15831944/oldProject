using System;
using System.Data.Common;
namespace ElectronTransferDal.OracleDal
{
    interface IGeometryCommandFactory
    {
        GeometryCommand GetInsertCommand(ElectronTransferModel.Base.ElectronSymbol entity, ElectronTransferModel.Config.SimpleMapping mapping, ElectronTransferDal.Common.RDBManagerBase dbManager, bool byView,DbConnection conn);
        GeometryCommand GetUpdateCommand(ElectronTransferModel.Base.ElectronSymbol entity, ElectronTransferModel.Config.SimpleMapping mapping, ElectronTransferDal.Common.RDBManagerBase dbManager, bool byView,DbConnection conn);
    }
}
