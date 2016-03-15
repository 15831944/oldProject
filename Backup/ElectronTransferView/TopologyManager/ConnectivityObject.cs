using System;
using System.Drawing;

namespace ElectronTransferView.ConnectivityManager
{
    public class ConnectivityObject :ElectronTransferModel.Geo.Point
    {
        public int oType = 0;

        public void draw(Graphics _g)
        {
            try
            {
                //连接点－实心圆
                if (oType == 0)
                {
                    var p=new Pen(Color.FromArgb(255,0,0),10);
                    _g.DrawEllipse(p, (int)X - 10, (int)Y - 10, 20, 20);

                    var nodeObject = this as NodeObject;
                    if (nodeObject != null)
                    {
                        string s = "连接点" + nodeObject.nIndex + "(" + nodeObject .nFid+ ")";
                        _g.DrawString(s, new Font("宋体", 12), new SolidBrush(Color.FromArgb(0, 0, 0)), (float)X -40, (float)Y - 22);
                    }
                }
                else
                {//设备－空心圆
                    Pen p = new Pen(Color.FromArgb(255, 0, 0), 2);
                    _g.DrawEllipse(p, (int)X - 15, (int)Y - 15, 30, 30);

                    var symbolObject = this as SymbolObject;
                    if (symbolObject != null)
                    {
                        string s = symbolObject.sType + "(" + symbolObject.sFid + ")";
                        if (symbolObject.nodeIndex != 0) {
                            s += "->连接点" + symbolObject.nodeIndex;
                        }
                        _g.DrawString(s, new Font("宋体", 12), new SolidBrush(Color.FromArgb(0, 0, 0)), (float)X - 40, (float)Y - 27);
                    }
                }




            }
            catch (Exception) { }
        }
        
    }
}
