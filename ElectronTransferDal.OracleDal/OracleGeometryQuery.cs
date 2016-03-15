using ElectronTransferDal.Common;

namespace ElectronTransferDal.OracleDal
{
    class OracleGeometryQuery:GeometryQuery
    {
        
        public override string GeometryQueryText
        {
            get
            {
                return string.Empty;
            }
            //return string.Format("{0} AS {1}",GetGeometryField(),GetGeometryFieldAlias());
        }

    }
}
