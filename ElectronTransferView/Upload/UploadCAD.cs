using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferFramework;
using ElectronTransferModel.Config;

namespace ElectronTransferView.Upload
{
    public partial class UploadCAD : Form
    {
        /// <summary>
        /// 工程ID
        /// </summary>
        private string GCID;
        /// <summary>
        /// 增量全路径
        /// </summary>
        private string xmlFileNamePath;
        /// <summary>
        /// 增量文件名
        /// </summary>
        private string xmlFileName;
        public UploadCAD(string fileName)
        {
            InitializeComponent();
            xmlFileNamePath = fileName;
            xmlFileName=Path.GetFileName(fileName);
            GCID = MapConfig.Instance.GCID;
        }

        private void Btn_Up_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(GCID))
                {
                    MessageBox.Show("请填写工程ID！","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Question);
                    Txt_GCID.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(xmlFileName))
                {
                    MessageBox.Show("请选择增量数据包");
                    Btn_Select.Focus();
                    return;
                }
                if (MessageBox.Show("确定要上传数据吗?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        Command.GetBackgroundWorker("正在上传数据,请稍后...");
                        //cadftp路径
                        var ftpPath = string.Format(@"D:\cad\cadftp\Import\{0}", GCID);
                        if (!Directory.Exists(ftpPath))
                            Directory.CreateDirectory(ftpPath);
                        //拷贝到ftp
                        var b = CopyFtp(xmlFileNamePath, string.Format(@"{0}\{1}",ftpPath, xmlFileName), true);
                        if (b)
                            ImportCad();
                    }
            }
            catch (Exception ex)
            {
                ElectronTransferDal.Cad.PublicMethod.Instance.ShowMessage(ex.ToString());
                MessageBox.Show("导入失败!");
                Command.CloseProgressBar();
            }
        }

        private void ImportCad()
        {
            CadService.ElectronTransferService eservice = new CadService.ElectronTransferService();
            if (!string.IsNullOrEmpty(MapConfig.Instance.CadServiceUrl))
                eservice.Url = MapConfig.Instance.CadServiceUrl;
            //导入cad
            var result = eservice.UpLoadAppend(xmlFileName, GCID);
            Command.CloseProgressBar();
            if (result.ToLower().Contains("true"))
            {
                MessageBox.Show("导入成功!");
                Btn_ImportCad.Enabled = false;
            }
            else
                MessageBox.Show("导入失败!");
        }

        private void Btn_Select_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "xml文件|*.xml|" + SystemSetting.FileExtension;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFile.FileName;
                xmlFileNamePath = fileName;
                xmlFileName = Path.GetFileName(fileName);
                textBox1.Text = xmlFileName;
            }
        }


        private void UploadCAD_Load(object sender, EventArgs e)
        {
            textBox1.Text = xmlFileName;
            Txt_GCID.Text = MapConfig.Instance.GCID;
            if (string.IsNullOrEmpty(GCID))
                Txt_GCID.Enabled = true;
            else
                Txt_GCID.Enabled = false;
        }
        /// <summary>
        /// 拷贝增量数据包
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="isDeleteTzPacket"></param>
        /// <returns></returns>
        public static bool CopyFtp(string sourcePath, string targetPath, bool isDeleteTzPacket)
        {
            try
            {
                if (isDeleteTzPacket)
                {
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);
                }
                if (!File.Exists(sourcePath))
                {
                    return false;
                }

                if (File.Exists(targetPath))
                    File.Delete(targetPath);

                //拷贝
                File.Copy(sourcePath, targetPath, true);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }

        private void Btn_Dxt_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定要预生成单线图吗?", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DxtService.CreateIncrementDXTService service = new DxtService.CreateIncrementDXTService();
                    Command.GetBackgroundWorker("正在预生成单线图,请稍后...");
                    var result = service.YCreateDXT(GCID);
                    Command.CloseProgressBar();
                    if (result.ToLower().Contains("true"))
                    {
                        if (MessageBox.Show("预生成单线图成功!\n是否进行单线图微调?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var url = string.Format("http://192.168.1.102:802/OPSYS.Web_Schematic.UI.xbap?ticketid={0}&operatetype=weitiao", GCID);
                            System.Diagnostics.Process.Start(url);
                        }
                    }
                    else
                        MessageBox.Show("预生成单线图失败!");
                }
            }
            catch (Exception ex)
            {
                ElectronTransferDal.Cad.PublicMethod.Instance.ShowMessage(ex.ToString());
            }
        }

        private void Btn_ExportCad_Click(object sender, EventArgs e)
        {
            var url = string.Format("http://192.168.1.102/webgis/index.html?gc_id={0}&gc_username=admin&op_type=0", GCID);
            System.Diagnostics.Process.Start(url);
        }

        private void Btn_PreViewWebGis_Click(object sender, EventArgs e)
        {
            var url = string.Format("http://192.168.1.102/webgis/index.html?gc_id={0}&gc_username=admin&op_type=1", GCID);
            System.Diagnostics.Process.Start(url);
        }
    }
}
