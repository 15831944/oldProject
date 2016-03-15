using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    internal class DeleteQueryBuilder : QueryBuilder
    {
        private object _entity;
        private bool _byView;
        public IList<string> Fields { get; private set; }

        public DeleteQueryBuilder(object entity, Type type, SimpleMapping mapping, bool byView)
            : base(type, new string[] {}, mapping, null, null, true)
        {
            _byView = byView;
        }

        public override string BuildQueryString()
        {
            var keyFields = KeyFieldCache.Instance.FindKeyFields(this.Type);
            this.Fields = keyFields.Select(o => o.Name).ToList();
            var condition = string.Join(" AND ",
                                        keyFields.Select(o => string.Format("{0}={1}{0}", o.Name, Seperator)).ToArray());
            return string.Format("DELETE FROM {0} WHERE {1}",
                                 _byView ? Mapping.GetUpdateView(Type.Name) : Mapping.GetTableName(this.Type.Name),
                                 condition);
        }
    }

}
