using System;
using System.Data.Common;
using System.Linq.Expressions;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.OracleDal
{
    internal class GeometryUpdateViewCommand:GeometryCommand
    {
        public GeometryUpdateViewCommand(ElectronSymbol entity, SimpleMapping mapping, RDBManagerBase dbManager, DbConnection conn)
            : base(entity,mapping, dbManager,conn)
        {
        }

        public override bool Execute()
        {
            Expression<Func<ElectronSymbol, bool>> conditionExpression = o => o.G3E_FID == Entity.G3E_FID;
            var conditionVisitor = new ConditionVisitor(conditionExpression);
            //DbManager.DeleteByCondition(this.Entity.GetType(), true, conditionVisitor.ToString(), surrounds);//to do.add the preparation
            var db = DbManager as OracleDBManager;
            db.RevealedDeleteItem(Entity,true,Connection);
            return new GeometryInsertViewCommand(Entity, Mapping, db, Connection).Execute();
            //return DbManager.Insert(this.Entity.GetType().Name, Entity, true, Surrounds);
        }
    }
}
