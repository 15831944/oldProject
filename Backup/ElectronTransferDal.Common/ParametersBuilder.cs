using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ElectronTransferFramework;

namespace ElectronTransferDal.Common
{
    public class ParametersBuilder
    {
        IList<DbParameter> _parameters = new List<DbParameter>();
        private const string SEPERATOR = ":";
        public IList<DbParameter> Parameters
        {
            get { return _parameters; }
        }

        public ParametersBuilder(object entity,DbCommand command,IEnumerable<string> fields )
        {
            foreach (var field in fields)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = Seperator + field;
                parameter.Value = entity.GetValue(field);//check 
                _parameters.Add(parameter);
            }
        }

        public virtual string Seperator
        {
            get { return SEPERATOR; }
        }

    }
}
