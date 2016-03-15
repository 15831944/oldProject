using System;
using ElectronTransferModel.Config;
using System.Linq.Expressions;
namespace ElectronTransferDal.Common
{
    interface ISelectQueryBuilderFactory
    {
        QueryBuilder Create(QueryVersion version, Type type, System.Collections.Generic.IEnumerable<string> avoidFields, SimpleMapping mapping, GeometryQuery geometryQuery, Expression expr, bool selectSingle);
    }
}
