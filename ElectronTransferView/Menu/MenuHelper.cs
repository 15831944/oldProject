using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.Cad;

namespace ElectronTransferView.Menu
{
    public class MenuHelper
    {
        /// <summary>
        /// 属性复制
        /// </summary>
        public static void GetAttributeCopy()
        {
            MessageBox.Show("请选择设备", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
            var objectId = PublicMethod.Instance.GetEntity("请选择设备\n");
            //复制源数据的属性
            var attribute = AttributeCopy.GetAttribute(objectId);
            if (attribute != null)
            {
                MessageBox.Show("请选择需要复制的设备", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                var objIds = PublicMethod.Instance.GetObjectIds("请选择需要复制的设备\n");
                if (objIds != null)
                {
                    if (MessageBox.Show(string.Format("确定要将属性复制到这{0}个设备吗？", objIds.Count()), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        //复制属性到选择的设备
                        var count = AttributeCopy.CopyAttribute(objIds, attribute);
                        MessageBox.Show(string.Format("{0}个设备复制成功！",count), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
