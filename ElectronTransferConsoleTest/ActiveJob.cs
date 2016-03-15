using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Common;

namespace ElectronTransferConsoleTest
{
    public class ActiveJob : ISurround
    {
        public string Configuration { get; private set; }
        public string JobName { get; private set; }

        public ActiveJob(string configuration, string jobName)
        {
            Configuration = configuration;
            JobName = jobName;
        }
        public string Begin()
        {
            return string.Format("BEGIN\nLTT_USER.SETCONFIGURATION('{0}');\nLTT_USER.EDITJOB('{1}');\nEND;", Configuration, JobName);
        }

        public string End()
        {
            return "BEGIN\nLtt_user.Done;\nEND;";
        }
    }
}
