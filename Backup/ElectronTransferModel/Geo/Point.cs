using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ElectronTransferModel.Geo
{
    [Serializable]
    public class Point : Geometry,IEquatable<Point>
    {
        private double[] _components = new double[3];
        public Point()
        {

        }
        public Point(params double[] components)
        {
            for (int index = 0; index < Math.Min(components.Length, 3); index++) 
            {
                _components[index] = components[index];
            }
        }
        private void SetComponent(int index, double value) 
        {
            _components[index] = value;
        }
        private double GetComponent(int index) 
        {
            return _components[index];
        }
        public double X { get { return GetComponent(0); } set { SetComponent(0, value); } }
        public double Y { get { return GetComponent(1); } set { SetComponent(1, value); } }
        public double Z { get { return GetComponent(2); } set { SetComponent(2, value); } }
        public static Point Parse(string text)
        {
            try
            {
                //var match = Regex.Match(text, @"\((.+)\)");
                //var body = match.Groups[1].Value;
                return new Point(text.Split(' ').Select(o => double.Parse(o)).ToArray());
            }
            catch (Exception ex) 
            {
                throw new GeometryException(text,ex);
            }
        }
        

        //public override string ToString()
        //{
        //    return string.Format("POINT({0} {1})", X, Y);
        //}

        public override egtype GeometryType
        {
            get { return egtype.point;}
        }


        #region IEquatable<Point> 成员

        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
