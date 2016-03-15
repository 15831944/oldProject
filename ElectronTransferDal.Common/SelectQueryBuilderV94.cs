using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Config;
using System.Linq.Expressions;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.Common
{
    class SelectQueryBuilderV94 : SelectQueryBuilderBase
    {

        public SelectQueryBuilderV94(Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle)
            : base(type, avoidFields, mapping, geometryQuery, expr, selectSingle)
        {
        }
        protected override string GetTableName(Type type)
        {
            return Mapping.GetTableName(type.Name).Replace("CADGIS.","");
        }
        protected override string ExtendColumns
        {
            get
            {
                return Type.IsSubclassOf(typeof(ElectronSymbol)) ? ",SDO_ETYPE,SDO_X1,SDO_X2,SDO_X3,SDO_X4,SDO_Y1,SDO_Y2,SDO_Y3,SDO_Y4,SDO_ORIENTATION" : string.Empty ;
            }
        }
        protected override string RowExenstion
        {
            get { return string.Empty; }
        }

    }
}
