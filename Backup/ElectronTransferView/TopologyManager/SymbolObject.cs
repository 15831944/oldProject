using System;
using System.Drawing;

namespace ElectronTransferView.ConnectivityManager
{
    public class SymbolObject : ConnectivityObject
    {
        public int nodeIndex = 0;
        public string sName { get; set; }
        public string sFid { get; set; }
        public string sType { get; set; }

        public NodeObject getnode()
        {
            if (nodeIndex == 1) {
                return node1;
            }
            if (nodeIndex == 2)
            {
                return node2;
            }
            return null;
        }

        public NodeObject node1 { get; set; }
        public NodeObject node2 { get; set; }

        public void draw2node(Graphics _g,NodeObject _n1,NodeObject _n2)
        {
            try
            {
                Pen p = new Pen(Color.FromArgb(255, 0, 0), 1);
                if (node1 != null && (node1 == _n1 || node1 == _n2))
                {
                    _g.DrawLine(p, (int)X, (int)Y, (int)node1.X, (int)node1.Y);
                }

                if (node2 != null && (node2 == _n1 || node2 == _n2))
                {
                    _g.DrawLine(p, (int)X, (int)Y, (int)node2.X, (int)node2.Y);
                }
            }
            catch (Exception) { }
        }
        public System.Windows.Point pointTemp = new System.Windows.Point(-1234, -4321);
        public void draw2point(Graphics _g)
        {
            try
            {
                var p = new Pen(Color.FromArgb(255, 0, 0), 1);

                _g.DrawLine(p, (int) X, (int) Y, (int) pointTemp.X, (int) pointTemp.Y);
            }
            catch (Exception) { }
        }
    }
}
