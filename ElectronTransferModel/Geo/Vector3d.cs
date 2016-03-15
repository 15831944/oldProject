using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferModel.Geo
{
    public class Vector3d
    {
        public double[] vector;

        private const double E = 0.0000000001;

        public Vector3d()
        {
            vector = new double[3] { 0, 0, 0 };
        }
        public Vector3d(double x, double y, double z)
        {
            vector = new double[3] { x, y, z };
        }

        public Vector3d(Vector3d vct)
        {
            vector = new double[3];
            vector[0] = vct.x;
            vector[1] = vct.y;
            vector[2] = vct.z;
        }
        /// <summary>
        /// x分量
        /// </summary>
        public double x
        {
            get { return vector[0]; }
            set { vector[0] = value; }
        }
        /// <summary>
        /// y分量
        /// </summary>
        public double y
        {
            get { return vector[1]; }
            set { vector[1] = value; }
        }
        /// <summary>
        /// z分量
        /// </summary>
        public double z
        {
            get { return vector[2]; }
            set { vector[2] = value; }
        }
        /// <summary>
        /// 字符串格式的向量
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="lhs">向量1</param>
        /// <param name="rhs">向量2</param>
        /// <returns></returns>
        public static Vector3d operator +(Vector3d lhs, Vector3d rhs)
        {
            Vector3d result = new Vector3d(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            result.z += rhs.z;
            return result;
        }
        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="lhs">向量1</param>
        /// <param name="rhs">向量2</param>
        /// <returns></returns>
        public static Vector3d operator -(Vector3d lhs, Vector3d rhs)
        {
            Vector3d result = new Vector3d(lhs);
            result.x -= rhs.x;
            result.y -= rhs.y;
            result.z -= rhs.z;
            return result;
        }
        /// <summary>
        /// 向量除以数量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector3d operator /(Vector3d lhs, double rhs)
        {
            if (rhs != 0)
                return new Vector3d(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
            else
                return new Vector3d(0, 0, 0);
        }
        /// <summary>
        /// 左乘数量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector3d operator *(double lhs, Vector3d rhs)
        {
            return new Vector3d(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }
        /// <summary>
        /// 右乘数量
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Vector3d operator *(Vector3d lhs, double rhs)
        {
            return new Vector3d(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }
        /// <summary>
        /// 向量数学积。 点乘
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static double operator *(Vector3d lhs, Vector3d rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }
        /// <summary>
        /// 相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3d lhs, Vector3d rhs)
        {
            if (Math.Abs(lhs.x - rhs.x) == 0 && Math.Abs(lhs.y - rhs.y) == 0 && Math.Abs(lhs.z - rhs.z) == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 不相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3d lhs, Vector3d rhs)
        {
            return !(lhs == rhs);
        }
        /// <summary>
        /// 相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }        
        /// <summary>
        /// 向量叉积，求与两向量垂直的向量
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3d Cross(Vector3d v1, Vector3d v2)
        {
            Vector3d r = new Vector3d(0, 0, 0);
            r.x = (v1.y * v2.z) - (v1.z * v2.y);
            r.y = (v1.z * v2.x) - (v1.x * v2.z);
            r.z = (v1.x * v2.y) - (v1.y * v2.x);
            return r;
        }
        /// <summary>
        /// 求向量长度
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static double Length(Vector3d v1)
        {
            return (double)Math.Sqrt((v1.x * v1.x) + (v1.y * v1.y) + (v1.z * v1.z));
        }
        /// <summary>
        /// 单位化向量
        /// </summary>
        /// <param name="v1"></param>
        /// <returns></returns>
        public static Vector3d Normalize(Vector3d v1)
        {
            double magnitude = Length(v1);
            v1 = v1 / magnitude;

            return v1;
        }
    }
}
