using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Common;
using System.Text.RegularExpressions;
using ElectronTransferModel;
using ElectronTransferView.FunctionManager;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class SHBManager : Form
    {

        public class Pair : IComparable
        {
            public bool First { get; set; }
            public Gg_pd_dyshb_n Second { get; set; }
            public Pair(Gg_pd_dyshb_n arg) { Second = arg; First = true; }
            
            #region IComparable 成员

            public int CompareTo(object obj)
            {
                Pair tmp = obj as Pair;
                if (Second.G3E_FID > tmp.Second.G3E_FID) return 1;
                if (Second.G3E_FID < tmp.Second.G3E_FID) return -1;
                return 0;
            }

            #endregion
        }
        /// <summary>
        /// 一页有多少个户表数
        /// </summary>
        private int onePageCount = 8;


        public SHBManager()
        {
            InitializeComponent();
        }
        public SHBManager(long g3efid)
        {
            g3e_fid = g3efid;
            InitializeComponent();
        }
        private long g3e_fid;
        /// <summary>
        /// 加载集抄箱里的所有户表实体
        /// </summary>
        private void LoadSHBS()
        {
            try
            {
                shbs.Clear();//清除之前的数据
                var t = DBManager.Instance.GetEntity<Detailreference_n>(o => o.G3E_FID == g3e_fid);
                if (t == null) return;
                var ents = DBEntityFinder.Instance.GetDYSHB_PT(t.G3E_DETAILID);
                if (ents!=null)
                {
                    foreach (long tmp in ents)
                    {
                        var ent = SHBDeleteManager.GetDYSHB_N(tmp);
                        if (ent != null)
                        {
                            if (ent.EntityState != EntityState.Delete)
                                shbs.Add(new Pair(ent));
                        }
                    }
                    shbs.Sort();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(String.Format("加载数据过程中出现错误:{0}", ex.Message));
            }
        }

        private void SHBManager_Load(object sender, EventArgs e)
        {
            textBox1.Text = "搜索"; 
            textBox1.TextChanged += textBox1_TextChanged;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            progressBar2.Value = 0;
            progressBar2.Step = 1;
            LoadSHBS();
            PageingSHB(1);
            Pageing(1);
        }
        /// <summary>
        /// 散户表的实体列表
        /// </summary>
        private List<Pair> shbs = new List<Pair>();

        /// <summary>
        /// imageindex
        /// </summary>
        private const int imgIndex = 1;

        /// <summary>
        /// 当前第几页
        /// </summary>
        private int curPageNum;
        /// <summary>
        /// 根据当前第几页加载散户表数据
        /// </summary>
        /// <param name="pageNum">当前第几页</param>
        private void PageingSHB(int pageNum)
        {
            //清除之前的数据
            listView1.Clear();
            //加载
            for (int i = onePageCount * (pageNum - 1),j=0; j < onePageCount&&i<shbs.Count;i++ )
            {
                if (shbs[i].First)
                {
                    listView1.Items.Add(String.Format("{0}", shbs[i].Second.G3E_FID), shbs[i].Second.HH, imgIndex);
                    j++;
                }
            }

        }
        /// <summary>
        /// 用于保存上一次分页使用到的控件
        /// </summary>
        SortedDictionary<int, Control> conList = new SortedDictionary<int, Control>();
        /// <summary>
        /// 根据当前第几页加载分页按钮
        /// </summary>
        /// <param name="pageNum">当前第几页</param>
        private void Pageing(int pageNum)
        {
           
            //计算页数
            int itmp = 0;
            foreach (var v in shbs)
            {
                if (v.First)
                    itmp++;
            }
            int pageCount = (int)Math.Ceiling(itmp / (double)onePageCount);

            //清除上一次的控件
            if (conList.Count > 0)
            {
                for (int i = 0; i <= conList.Last().Key; i++)
                {
                    Control tmp;
                    if (conList.TryGetValue(i, out tmp))
                    {
                        Controls.Remove(tmp);
                    }
                }
                conList.Clear();
            }
            

            if (pageCount > 1)//超过1页情况
            {
                //添加上一页
                if (pageNum == 1)
                {
                    Label up = new Label();up.AutoSize=true;up.Text = "上一页";conList.Add(0, up);
                }
                else
                {
                    LinkLabel up = new LinkLabel(); up.AutoSize = true; up.Text = "上一页"; conList.Add(0, up);
                }
                //添加下一页
                if (pageNum == pageCount)
                {
                    Label down = new Label(); down.AutoSize = true; down.Text = "下一页"; conList.Add(pageCount + 1, down);
                }
                else
                {
                    LinkLabel down = new LinkLabel(); down.AutoSize = true; down.Text = "下一页"; conList.Add(pageCount + 1, down);
                }
                //添加当前页
                Label curPage = new Label();
                curPage.AutoSize = true;
                curPage.Text = String.Format("{0}",pageNum);
                conList.Add(pageNum, curPage);

                //添加第一页
                if (pageNum > 1)
                {
                    LinkLabel lb1 = new LinkLabel(); lb1.AutoSize = true; lb1.Text = "1"; conList.Add(1, lb1);
                }
                //添加最后一页
                if (pageNum < pageCount)
                {
                    LinkLabel lblast = new LinkLabel(); lblast.AutoSize = true; lblast.Text = String.Format("{0}", pageCount); conList.Add(pageCount, lblast);
                }
                // 添加当前页前后两页内的页数
                for (int i = pageNum - 2; i < pageNum + 3; i++)
                {
                    if (i <= 1 || i >= pageCount||i==pageNum) continue;
                    LinkLabel lb = new LinkLabel();
                    lb.AutoSize = true;
                    lb.Text = String.Format("{0}", i);
                    conList.Add(i, lb);
                }
                //添加 "..." 按钮
                if (pageNum - 1 > 3)
                {
                    Label tmp = new Label(); tmp.AutoSize = true; tmp.Text = "..."; conList.Add(2, tmp);
                }
                if (pageCount - pageNum > 3)
                {
                    Label tmp = new Label(); tmp.AutoSize = true; tmp.Text = "..."; conList.Add(pageCount - 1, tmp);
                }
            }
            else// 只有1页则只添加页数1,下一页与上一页的文本,让人知道这里是显示页数的
            {
                Label up = new Label(); up.AutoSize = true; up.Text = "上一页"; conList.Add(0, up);
                Label lb = new Label(); lb.AutoSize = true; lb.Text = "1"; conList.Add(1, lb);
                Label down = new Label(); down.AutoSize = true; down.Text = "下一页"; conList.Add(2, down);
            }


            // 计算控件的位置并放入窗体中
            const int space = 12; //按钮间的间隔
            int len = 0;//所有控件总宽度
            //计算所有控件总宽度
            for (int i = 0; i <= conList.Last().Key;i++ )
            {
                Control tmp;
                if (conList.TryGetValue(i,out tmp))
                {
                    Graphics g = Graphics.FromHwnd(tmp.Handle);
                    SizeF size = g.MeasureString(tmp.Text, tmp.Font);//获取字体宽度
                    g.Dispose();
                    tmp.Width = (int)size.Width;
                    if (tmp.GetType() == typeof(LinkLabel))
                    {
                        tmp.MouseClick += linklabel_mouseclick;
                    }
                    len += tmp.Width;
                    if (i != conList.Last().Key - 1)
                        len += space;
                }
            }
            //计算控件left位置
            int leftPos = Width / 2 - len / 2;
            //计算控件top位置
            int topPos = Size.Height - 47;
            //放入按钮
            for (int i = 0; i <= conList.Last().Key;i++ )
            {
                Control tmp;
                if (conList.TryGetValue(i, out tmp))
                {
                    tmp.Left = leftPos;
                    tmp.Top = topPos;
                    tmp.Anchor = AnchorStyles.Bottom;
                    Controls.Add(tmp);
                    leftPos += tmp.Width + space;
                }
            }

            curPageNum = pageNum;
        }

        /// <summary>
        /// 用于分页的链接点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linklabel_mouseclick(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(LinkLabel))
            {
                int num;
                if (int.TryParse(((LinkLabel)sender).Text, out num))
                {
                    PageingSHB(num);
                    Pageing(num);
                }
                else
                {
                    if (((LinkLabel)sender).Text == "上一页")
                    {
                        PageingSHB(curPageNum - 1);
                        Pageing(curPageNum - 1);
                    }
                    else if (((LinkLabel)sender).Text == "下一页")
                    {
                        PageingSHB(curPageNum + 1);
                        Pageing(curPageNum + 1);
                    }
                }
            }
        }
        /// <summary>
        /// 鼠标悬停于选择项上时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            var ent = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(o => o.G3E_FID==long.Parse(e.Item.Name));
            if(ent!=null)
                e.Item.ToolTipText = String.Format("房号:{0}\n表号:{1}\n备注:{2}\n门牌地址:{3}\n用户用电号:{4}\n用户姓名:{5}", ent.AZDZ, ent.BH, ent.BZ, ent.HH, ent.YDH, ent.YHXM);
            
        }
        /// <summary>
        /// 在所有属性中搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Regex reg = new Regex(String.Format(".*{0}.*", textBox1.Text));
            progressBar1.Maximum = shbs.Count;
            progressBar1.Value = 0;
            foreach (var v in shbs)
            {
                progressBar1.PerformStep();
                progressBar2.Maximum = v.Second.GetType().GetProperties().Count();
                progressBar2.Value = 0;
                foreach (var t in v.Second.GetType().GetProperties())
                {
                    Match mat = reg.Match(String.Format("{0}", t.GetValue(v.Second, null)));
                    if (!mat.Success)
                    {
                        v.First = false;
                    }
                    else
                    {
                        v.First = true;
                        break;
                    }
                    progressBar2.PerformStep();
                }
                progressBar2.Value = 0;
            }
            progressBar1.Value = 0;
            PageingSHB(curPageNum);
            Pageing(curPageNum);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var isLock = DBEntityFinder.Instance.VerifyLTTID(detail_fid);
            //SHBEditer se = new SHBEditer(formType.regionAdd, g3e_fid,isLock);
            //if (DialogResult.OK == se.ShowDialog())
            //{
            //    LoadSHBS();
            //    PageingSHB(curPageNum);
            //    Pageing(curPageNum);
            //}
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                //long fid = long.Parse(listView1.SelectedItems[0].Name);
                //SHBEditer se = new SHBEditer(formType.regionEdit, fid);
                //if (DialogResult.OK == se.ShowDialog())
                //{
                //    LoadSHBS();
                //    PageingSHB(curPageNum);
                //    Pageing(curPageNum);
                //}
            }
        }

        

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除?", "确定?", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            try
            {
                int count = listView1.SelectedItems.Count;
                for (int i=0;i<count;i++)
                {
                    long fid = long.Parse(listView1.SelectedItems[i].Name);
                    //删除户表数据本身
                    SHBDeleteManager.DeleteItem(fid,new List<DBEntity>());
                }                
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(String.Format("删除过程中出现错误:{0}", ex.Message));
            }
            finally
            {
                LoadSHBS();
                PageingSHB(curPageNum);
                Pageing(curPageNum);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int count = listView1.SelectedItems.Count ;
            if (1==count)//有一个选择项
            {
                contextMenuStrip1.Items[0].Enabled = true;
                contextMenuStrip1.Items[1].Enabled = true;
                contextMenuStrip1.Items[2].Enabled = true;
            }
            else if (1 < count)//有多个选择项
            {
                contextMenuStrip1.Items[0].Enabled = true;
                contextMenuStrip1.Items[1].Enabled = false;
                contextMenuStrip1.Items[2].Enabled = true;
            }
            else//无选择项
            {
                contextMenuStrip1.Items[0].Enabled = true;
                contextMenuStrip1.Items[1].Enabled = false;
                contextMenuStrip1.Items[2].Enabled = false;
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(!textBox1.Focused)
                textBox1.SelectAll();
        }
        /// <summary>
        /// 重新计算在当前listView中一页的数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SHBManager_ResizeEnd(object sender, EventArgs e)
        {
            int hCount = listView1.Size.Width / 127;//横数量
            int vCount = (listView1.Size.Height+60) / 120;//竖数量
            //MessageBox.Show(String.Format("width:{0},height:{1},hCount:{2},vCount:{3}", listView1.Size.Width, listView1.Size.Height, hCount, vCount));
            onePageCount = hCount * vCount;
            PageingSHB(1);
            Pageing(1);
        }
        /// <summary>
        /// 双击选项打开查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            editToolStripMenuItem_Click(sender, e);
        }
        /// <summary>
        /// 按键响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SHBManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }




    //public static class SHBDeleteManager
    //{
    //    /// <summary>
    //    /// 获取户表自身属性
    //    /// </summary>
    //    /// <param name="g3e_fid"></param>
    //    /// <returns></returns>
    //    public static Gg_pd_dyshb_n GetDYSHB_N(long g3e_fid)
    //    {
    //        try
    //        {
    //            return DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(g3e_fid);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }
      
    //    /// <summary>
    //    /// 批量删除
    //    /// </summary>
    //    /// <param name="fid"></param>
    //    /// <param name="backupDBEntity"> </param>
    //    /// <returns></returns>
    //    public static List<DBEntity> BatchDeleteFromJCX(long fid, List<DBEntity> backupDBEntity)
    //    {
    //        try
    //        {
    //            //获取集抄箱下的所有户表
    //            var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
    //            if (t != null)
    //            {
    //                var ents = DBEntityFinder.Instance.GetDYSHB_PT(t.G3E_DETAILID);
    //                if (ents != null)
    //                {
    //                    foreach (long tmp in ents)
    //                    {
    //                        DeleteItem(tmp, backupDBEntity);
    //                    }
    //                }
    //            }
    //        }catch(Exception ex)
    //        {
    //            LogManager.Instance.Error(ex);
    //        }
    //        return backupDBEntity;
    //    }

    //    /// <summary>
    //    /// 删除单个户表
    //    /// </summary>
    //    /// <param name="fid"></param>
    //    /// <param name="backupDBEntity"> </param>
    //    public static List<DBEntity> DeleteItem(long fid, List<DBEntity> backupDBEntity)
    //    {
    //        backupDBEntity = DeleteItemUndo(fid, backupDBEntity);
    //        return backupDBEntity;
    //    }

    //    private static List<DBEntity> DeleteItemUndo(long g3e_fid, List<DBEntity> backupDBEntity)
    //    {
    //        //删除户表数据本身
    //        var ent = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(g3e_fid);
    //        if (ent != null)
    //        {
    //            if (ent.EntityState == EntityState.Delete) return backupDBEntity;

    //            if (ent.EntityState == EntityState.Insert)
    //            {
    //                backupDBEntity.Add(ent.Clone() as DBEntity);
    //                DBManager.Instance.Delete(ent);
    //            }
    //            else if (ent.EntityState == EntityState.None || ent.EntityState == EntityState.Update)
    //            {
    //                ent.RedoState = true;
    //                ent.LTT_ID = MapConfig.Instance.LTTID;
    //                backupDBEntity.Add(ent.Clone() as DBEntity);
    //                ent.EntityState = EntityState.Delete;
    //                DBManager.Instance.Update(ent);
    //            }

    //            //删除从属关系
    //            var ent2 = DBManager.Instance.GetEntity<Gg_jx_shbd_pt>(g3e_fid);
    //            if (ent2 != null)
    //            {
    //                if (ent2.EntityState == EntityState.Insert)
    //                {
    //                    backupDBEntity.Add(ent2.Clone() as DBEntity);
    //                    DBManager.Instance.Delete(ent2);
    //                }
    //                else if (ent2.EntityState == EntityState.None || ent2.EntityState == EntityState.Update)
    //                {
    //                    ent2.RedoState = true;
    //                    ent2.LTT_ID = MapConfig.Instance.LTTID;
    //                    backupDBEntity.Add(ent2.Clone() as DBEntity);
    //                    ent2.EntityState = EntityState.Delete;
    //                    DBManager.Instance.Update(ent2);
    //                }
    //            }
    //            else
    //            {
    //                throw new Exception("找不到从属关系");
    //            }
    //        }
    //        else
    //        {
    //            throw new Exception("找不到实体");
    //        }
    //        return backupDBEntity;
    //    }
    //}
}
