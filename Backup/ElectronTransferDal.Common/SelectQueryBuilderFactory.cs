using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using System.Linq.Expressions;

namespace ElectronTransferDal.Common
{
    class SelectQueryBuilderFactory : Singleton<SelectQueryBuilderFactory>, ISelectQueryBuilderFactory
    {
        private SelectQueryBuilderFactoryImplement _implement = new SelectQueryBuilderFactoryImplement();
        public QueryBuilder Create(QueryVersion version, Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle) 
        {
            return _implement.Create(version, type, avoidFields, mapping, geometryQuery, expr, selectSingle);
        }
    }
    class SelectQueryBuilderFactoryImplement : ISelectQueryBuilderFactory
    {
        public QueryBuilder Create(QueryVersion version, Type type, IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle) 
        {
            if (version == QueryVersion.V10)
            {
                return new SelectQueryBuilder(type, avoidFields, mapping, geometryQuery, expr, selectSingle);
            }
            else
                return new SelectQueryBuilderV94(type, avoidFields, mapping, geometryQuery, expr, selectSingle);
        }
    }
}
