using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Common
{
    public interface ISequenceValueGenerator
    {
        RDBManagerBase DbManager { get; set; }
        long GenerateTableId(Type type);
        long GenerateGlobalId();
        long GenerateNodeId();
        long GenerateDetailId();
    }
}
