using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;

namespace ElectronTransferDal.Cad
{
     public static class DBObjectEx
    {
        public static UpgradeOpenManager UpgradeOpenAndRun(this DBObject obj)
        {
            return new UpgradeOpenManager(obj);
        }
    }
    public class UpgradeOpenManager : IDisposable
    {
        DBObject _obj;
        bool _isNotifyEnabled;
        bool _isWriteEnabled;
        internal UpgradeOpenManager(DBObject obj)
        {
            _obj = obj;
            _isNotifyEnabled = _obj.IsNotifyEnabled;
            _isWriteEnabled = _obj.IsWriteEnabled;
            if (_isNotifyEnabled)
                _obj.UpgradeFromNotify();
            else if (!_isWriteEnabled)
                _obj.UpgradeOpen();
        }
        #region IDisposable 成员
        public void Dispose()
        {
            if (_isNotifyEnabled)
                _obj.DowngradeToNotify(_isWriteEnabled);
            else if (!_isWriteEnabled)
                _obj.DowngradeOpen();
        }
        #endregion
    }
}
