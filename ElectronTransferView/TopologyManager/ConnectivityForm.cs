using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ElectronTransferView.ConnectivityManager
{
    public sealed partial class ConnectivityForm : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public ConnectivityForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            //强制窗体获取焦点
            SetForegroundWindow(Handle);
        }
        /// <summary>
        /// 需要新建连接关系的设备
        /// </summary>
        public SymbolObject sym1 { get; set; }
        /// <summary>
        /// 被连接的设备
        /// </summary>
        public SymbolObject sym2 { get; set; }
        /// <summary>
        /// 被连接的设备的连接点
        /// </summary>
        public List<NodeObject> nodes { get; set; }
        /// <summary>
        /// 被连接的设备的邻居
        /// </summary>
        public List<SymbolObject> symbols { get; set; }

        private void ConnectivityForm_Load(object sender, EventArgs e)
        {
            /* 只有一个连接点的设备
             74 低压终端, 85 PT柜, 171 用户自带发电机, 172 电动机, 173 站房接地刀闸, 174 无功补偿器, 177 避雷器, 307 电压互感器, 309 高压电机, 43 站房母线, 159 低压集中抄表箱
             */
            /*12.05 
             * 1 低压终端有2个类型，其中一个类型有1个连接点，另外一个有2个连接点 
             * 2 低压母线 有两个节点 从gis数据库可知
            */ 
            string[] sinNode = { "站房母线", "低压集中抄表箱"/*, "低压母线", "低压终端"*/, "接地挂环", "PT柜", "用户自带发电机", "电动机", "站房接地刀闸", "无功补偿器", "避雷器", "电压互感器", "高压电机" };
            if (isfirst)
            {
                if (nodes[0] != null)
                {
                    nodes[0].X = Width / 2 - 120;
                    nodes[0].Y = Height / 2;
                }
                
                if (nodes[1] != null)
                {
                    nodes[1].X = Width / 2 + 120;
                    nodes[1].Y = Height / 2;
                }
                leftIndex = 0;
                rightIndex = 0;
               // List<SymbolObject> del_symbols=new List<SymbolObject>();
                foreach (SymbolObject _sy in symbols)
                {
                    if (_sy == sym2)
                    {
                        _sy.X = Width / 2;
                        _sy.Y = Height / 2 + 100;

                        if (sinNode.Contains(sym2.sType)) /*sym2.sType == "站房母线" || sym2.sType == "低压集中抄表箱" || sym2.sType == "低压母线" || sym2.sType == "10kV电缆终端头"*/
                        {
                            nodes[1] = null;
                        }

                    }
                    else if (_sy == sym1)
                    {
                        _sy.X = Width / 2;
                        _sy.Y = Height / 2 - 200;
                    }
                    else
                    {
                        if (_sy.node1 == nodes[0] || _sy.node2 == nodes[0])
                        {
                            _sy.X = Width / 2 - 120 - (leftIndex / 5 + 1) * 120;
                            _sy.Y = Height / 2 + Math.Pow(-1, leftIndex % 5) * ((leftIndex % 5 + 1) / 2) * 120
                                + Math.Pow(-1, leftIndex / 5) * 15;
                            leftIndex++;
                        }

                        if (sinNode.Contains(sym2.sType))/*sym2.sType == "站房母线" || sym2.sType == "低压集中抄表箱" || sym2.sType == "低压母线" || sym2.sType == "低压终端"|| sym2.sType == "10kV电缆终端头"*/
                        {
                        }
                        else
                        {
                            if (_sy.node1 == nodes[1] || _sy.node2 == nodes[1])
                            {
                                _sy.X = Width / 2 + 120 + (rightIndex / 5 + 1) * 120;
                                _sy.Y = Height / 2 + Math.Pow(-1, rightIndex % 5) * ((rightIndex % 5 + 1) / 2) * 120
                                    + Math.Pow(-1, rightIndex / 5) * 15;
                                rightIndex++;
                            }
                        }


                    }
                }
                //foreach (SymbolObject _sy in del_symbols)
                //{
                //    symbols.Remove(_sy);
                //}

                oldwidth = Width;
                oldheight = Height;
                isfirst = false;
            }

        }

        bool isfirst = true;
        int oldwidth;
        int oldheight;
        private void ConnectivityForm_Resize(object sender, EventArgs e)
        {
            try
            {                
                if(!isfirst)
                {
                    if (nodes[0] != null)
                    {
                        nodes[0].X = Width / 2 - 100;
                        nodes[0].Y = Height / 2;
                    }
                    if (nodes[1] != null)
                    {
                        nodes[1].X = Width / 2 + 100;
                        nodes[1].Y = Height / 2;
                    }
                    leftIndex = 0;
                    rightIndex = 0;
                    foreach (SymbolObject _sy in symbols)
                    {
                        _sy.X += (Width - oldwidth) / 2;
                        _sy.Y += (Height - oldheight) / 2;
                    }
                }
            }
            catch (Exception) { }
            finally {
                oldwidth = Width;
                oldheight = Height;
            }
        }            
        static int rightIndex;
        static int leftIndex;
        private void ConnectivityForm_Paint(object sender, PaintEventArgs e)
        {

           // draw();
        }

        private void draw(object _f)
        {
            try
            {
                Graphics g = Graphics.FromHwnd((IntPtr)_f);

                Bitmap bmp = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);
                Graphics gg = Graphics.FromImage(bmp);

                gg.Clear(Color.FromArgb(255, 255, 255));
                if (nodes[0]!=null)
                {    
                    nodes[0].draw(gg);
                }
                if (nodes[1]!=null)
                {
                    nodes[1].draw(gg);
                }
                foreach (SymbolObject sy in symbols)
                {

                    sy.draw(gg);
                    sy.draw2node(gg, nodes[0], nodes[1]);

                    if (sy.pointTemp.X != -1234 && sy.pointTemp.Y != -4321)
                    {
                        sy.draw2point(gg);
                    }

                }

                g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
            }
            catch (Exception) { }
        }

        bool isLmousedown;
        bool isRmousedown;

        private void ConnectivityForm_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                oldco = crossbypoint(new System.Windows.Point(e.X, e.Y));
                oldpoint = new System.Windows.Point(e.X, e.Y);
                if (e.Button == MouseButtons.Left)
                {
                    isLmousedown = true;
                    isRmousedown = false;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    isLmousedown = false;
                    isRmousedown = true;
                }
            }
            catch (Exception) { }
        }
        ConnectivityObject oldco;
        private void ConnectivityForm_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var symbolObject = oldco as SymbolObject;
                if (symbolObject != null)
                    symbolObject.pointTemp = new System.Windows.Point(-1234, -4321);

                var co = crossbypoint(new System.Windows.Point(e.X, e.Y));
                if (co != null && co is SymbolObject)
                {
                    label1.Visible = true;
                    label1.Location = new Point(e.X + 3, e.Y + 3);
                    label1.Text = (co as SymbolObject).sName;
                }
                else
                {
                    label1.Visible = false;
                }
              
                if (oldco != null && oldco is SymbolObject)
                {
                    if (isRmousedown) 
                    {
                        oldco.X += e.X - oldpoint.X;
                        oldco.Y += e.Y - oldpoint.Y;
                    }
                    else if (isLmousedown)
                    {
                        if ((oldco as SymbolObject) == sym1)
                        {
                            (oldco as SymbolObject).pointTemp = new System.Windows.Point(e.X, e.Y);
                            ConnectivityObject no = crossbypoint(new System.Windows.Point(e.X, e.Y));
                            if (no != null && no is NodeObject) 
                            {
                                if ((oldco as SymbolObject).nodeIndex == 1)
                                {
                                    (oldco as SymbolObject).node1 = (NodeObject)no;
                                }
                                else if((oldco as SymbolObject).nodeIndex==2)
                                {
                                    (oldco as SymbolObject).node2 = (NodeObject)no;
                                }

                            }
                        }
                    }
                }
                //else if (co != null && co is NodeObject)
                //{
                //    if (isLmousedown)
                //    {
                //        (co as SymbolObject).pointTemp = new System.Windows.Point(e.X, e.Y);
                //    }
                //}
              

                //draw();
            }
            catch (Exception) { }
            finally { 
                oldpoint = new System.Windows.Point(e.X, e.Y);
            }
        }

        private void ConnectivityForm_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //co = null;
                oldpoint = new System.Windows.Point(e.X, e.Y);
                isLmousedown = false;
                isRmousedown = false;
            }
            catch (Exception) { }
        }

        System.Windows.Point oldpoint;

        private void ConnectivityForm_MouseHover(object sender, EventArgs e)
        {
            try
            {

              
            }
            catch (Exception) { }
        }

        private ConnectivityObject crossbypoint(System.Windows.Point _p)
        {
            ConnectivityObject co = null;
            try
            {
                foreach (ConnectivityObject _co in symbols)
                {
                    if (Math.Abs(_co.X - _p.X) <= 15 &&
                        Math.Abs(_co.Y - _p.Y) <= 15)
                    {
                        co = _co;
                        return co;
                    }
                }
                foreach (ConnectivityObject _no in nodes)
                {
                    if (Math.Abs(_no.X - _p.X) <= 10 &&
                        Math.Abs(_no.Y - _p.Y) <= 10)
                    {
                        co = _no;
                        return co;
                    }
                }
            }
            catch (Exception) { }
            return co;
        }

        
        private void ConnectivityForm_Shown(object sender, EventArgs e)
        {
            var tt = new System.Threading.Thread(drawpro);
            tt.Start(Handle);
        }

        void drawpro(object g) 
        {
            try
            {
                while (true)
                {
                    draw(g);
                    System.Threading.Thread.Sleep(50);
                }
            }
            catch (Exception) { }
        }

        private void ConnectivityForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 || e.KeyValue == 13)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
