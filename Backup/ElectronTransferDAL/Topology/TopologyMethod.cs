using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ElectronTransferDal.Topology
{
    /// <summary>
    /// 
    /// </summary>
    public struct DLAdjNode
    {
        public int fid;
        public int g3e_fno;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string cd_dqzt;		//当前状态
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string cd_sfdd;		//是否带电
        public int node1_id;		//连接点1
        public int node2_id;		//连接点1
    }

    /// <summary>
    /// 拓扑追踪方法
    /// </summary>
    public class TopologyMethod
    {
        /// <summary>
        /// 初始化图,传入图的顶点 
        /// </summary>
        /// <param name="dd"></param>
        /// <param name="ddCoung"></param>
        public static void CYZCreateGraphFromCSharp(DLAdjNode[] dd, int ddCoung)
        {
            createGraphFromCSharp(dd, ddCoung);
        }
        /// <summary>
        /// 求两点最短路径
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="endFid"></param>
        /// <param name="retDistance"></param>
        /// <param name="retPathFids"></param>
        /// <param name="retPathNodeCount"></param>
        /// <returns>两点之间没有连通的路径时，返回false. 否则返回true.</returns>
        public static bool CYZP2pPath(int startFid, int endFid, ref float retDistance, ref List<int> retPathFids)
        {
            bool flag = false;
            try
            {
                int retPathNodeCount = 0;
                IntPtr ppp = IntPtr.Zero;

                flag = p2pPath(startFid, endFid, ref retDistance, ref retPathNodeCount, ref ppp);
                if (!flag) { return false; }

                int reByteCount = retPathNodeCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp, iii);
                    iii++;
                }
                iii = 0;
                //retPathFids = new int[retPathNodeCount];
                retPathFids = new List<int>();
                while (iii < retPathNodeCount)
                {
                    int xfid = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    retPathFids.Add(xfid);
                    iii++;
                }

                TopologyMethod.removeMemery(ppp, retPathNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }
        /// <summary>
        /// 求指定点的上游(到电源点)路径
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="retDistance"></param>
        /// <param name="retPathFids"></param>
        /// <param name="retPathNodeCount"></param>
        /// <returns>没有上游(到电源点)路径时，返回false. 否则返回true.</returns>
        public static bool CYZUpPath(int startFid, ref float retDistance, ref List<int> retPathFids)
        {
            bool flag = false;
            try
            {
                int retPathNodeCount = 0;
                IntPtr ppp = IntPtr.Zero;

                flag = upPath(startFid, ref retDistance, ref retPathNodeCount, ref ppp);
                if (!flag) { return false; }

                int reByteCount = retPathNodeCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp, iii);
                    iii++;
                }
                iii = 0;
                //retPathFids = new int[retPathNodeCount];
                retPathFids = new List<int>();
                while (iii < retPathNodeCount)
                {
                    int xfid = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    retPathFids.Add(xfid);
                    iii++;
                }

                TopologyMethod.removeMemery(ppp, retPathNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="retPathsFids"></param>
        /// <returns></returns>
        public static bool CYZDownPath(int startFid, ref List<List<int>> retPathsFids)
        {
            bool flag = false;
            try
            {
                IntPtr ppp1 = IntPtr.Zero;
                IntPtr ppp2 = IntPtr.Zero;
                int retPathsCount = 0;

                flag = downPath(startFid, ref retPathsCount, ref ppp1, ref ppp2);
                if (!flag) { return false; }
                if (retPathsCount <= 0) { return false; }
                if (ppp1 == IntPtr.Zero) { return false; }
                if (ppp2 == IntPtr.Zero) { return false; }

                //读取ppp1指针的内存数据. retPathsCount个int
                int reByteCount = retPathsCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp1, iii);
                    iii++;
                }
                iii = 0;
                int vNodeCount = 0;
                List<int> perPathCount = new List<int>();
                while (iii < retPathsCount)
                {
                    int xcount = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    perPathCount.Add(xcount);
                    vNodeCount += xcount;
                    iii++;
                }

                //读取ppp2指针的内存数据. vNodeCount个int
                reByteCount = vNodeCount * sizeof(int);
                retBytes = new byte[reByteCount];
                iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp2, iii);
                    iii++;
                }
                iii = 0;
                int vNodeIndex = 0;
                retPathsFids = new List<List<int>>();
                for (int i = 0; i < retPathsCount; i++)
                {
                    List<int> pfids = new List<int>();
                    for (int j = 0; j < perPathCount[i]; j++)
                    {
                        int xfid = (int)retBytes[vNodeIndex * 4] +
                            ((int)retBytes[vNodeIndex * 4 + 1]) * 256 +
                            ((int)retBytes[vNodeIndex * 4 + 2]) * 65536 +
                            ((int)retBytes[vNodeIndex * 4 + 3]) * 16777216;
                        pfids.Add(xfid);

                        ++vNodeIndex;
                    }
                    retPathsFids.Add(pfids);
                }


                TopologyMethod.removeMemery(ppp1, retPathsCount);
                TopologyMethod.removeMemery(ppp2, vNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="retPathsFids"></param>
        /// <returns></returns>
        public static bool CYZAllPath(int startFid, ref List<List<int>> retPathsFids)
        {
            bool flag = false;
            try
            {
                IntPtr ppp1 = IntPtr.Zero;
                IntPtr ppp2 = IntPtr.Zero;
                int retPathsCount = 0;

                flag = allPath(startFid, ref retPathsCount, ref ppp1, ref ppp2);
                if (!flag) { return false; }
                if (retPathsCount <= 0) { return false; }
                if (ppp1 == IntPtr.Zero) { return false; }
                if (ppp2 == IntPtr.Zero) { return false; }

                //读取ppp1指针的内存数据. retPathsCount个int
                int reByteCount = retPathsCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp1, iii);
                    iii++;
                }
                iii = 0;
                int vNodeCount = 0;
                List<int> perPathCount = new List<int>();
                while (iii < retPathsCount)
                {
                    int xcount = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    perPathCount.Add(xcount);
                    vNodeCount += xcount;
                    iii++;
                }

                //读取ppp2指针的内存数据. vNodeCount个int
                reByteCount = vNodeCount * sizeof(int);
                retBytes = new byte[reByteCount];
                iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp2, iii);
                    iii++;
                }
                iii = 0;
                int vNodeIndex = 0;
                retPathsFids = new List<List<int>>();
                for (int i = 0; i < retPathsCount; i++)
                {
                    List<int> pfids = new List<int>();
                    for (int j = 0; j < perPathCount[i]; j++)
                    {
                        int xfid = (int)retBytes[vNodeIndex * 4] +
                            ((int)retBytes[vNodeIndex * 4 + 1]) * 256 +
                            ((int)retBytes[vNodeIndex * 4 + 2]) * 65536 +
                            ((int)retBytes[vNodeIndex * 4 + 3]) * 16777216;
                        pfids.Add(xfid);

                        ++vNodeIndex;
                    }
                    retPathsFids.Add(pfids);
                }


                TopologyMethod.removeMemery(ppp1, retPathsCount);
                TopologyMethod.removeMemery(ppp2, vNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="retPathsFids"></param>
        /// <returns></returns>
        public static bool CYZUpPath2(int startFid, ref List<List<int>> retPathsFids)
        {
            bool flag = false;
            try
            {
                IntPtr ppp1 = IntPtr.Zero;
                IntPtr ppp2 = IntPtr.Zero;
                int retPathsCount = 0;

                flag = upPath2(startFid, ref retPathsCount, ref ppp1, ref ppp2);
                if (!flag) { return false; }
                if (retPathsCount <= 0) { return false; }
                if (ppp1 == IntPtr.Zero) { return false; }
                if (ppp2 == IntPtr.Zero) { return false; }

                //读取ppp1指针的内存数据. retPathsCount个int
                int reByteCount = retPathsCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp1, iii);
                    iii++;
                }
                iii = 0;
                int vNodeCount = 0;
                List<int> perPathCount = new List<int>();
                while (iii < retPathsCount)
                {
                    int xcount = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    perPathCount.Add(xcount);
                    vNodeCount += xcount;
                    iii++;
                }

                //读取ppp2指针的内存数据. vNodeCount个int
                reByteCount = vNodeCount * sizeof(int);
                retBytes = new byte[reByteCount];
                iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp2, iii);
                    iii++;
                }
                iii = 0;
                int vNodeIndex = 0;
                retPathsFids = new List<List<int>>();
                for (int i = 0; i < retPathsCount; i++)
                {
                    List<int> pfids = new List<int>();
                    for (int j = 0; j < perPathCount[i]; j++)
                    {
                        int xfid = (int)retBytes[vNodeIndex * 4] +
                            ((int)retBytes[vNodeIndex * 4 + 1]) * 256 +
                            ((int)retBytes[vNodeIndex * 4 + 2]) * 65536 +
                            ((int)retBytes[vNodeIndex * 4 + 3]) * 16777216;
                        pfids.Add(xfid);

                        ++vNodeIndex;
                    }
                    retPathsFids.Add(pfids);
                }


                TopologyMethod.removeMemery(ppp1, retPathsCount);
                TopologyMethod.removeMemery(ppp2, vNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startFid"></param>
        /// <param name="retPathsFids"></param>
        /// <returns></returns>
        public static bool CYZDownPath2(int startFid, ref List<List<int>> retPathsFids)
        {
            bool flag = false;
            try
            {
                IntPtr ppp1 = IntPtr.Zero;
                IntPtr ppp2 = IntPtr.Zero;
                int retPathsCount = 0;

                flag = downPath2(startFid, ref retPathsCount, ref ppp1, ref ppp2);
                if (!flag) { return false; }
                if (retPathsCount <= 0) { return false; }
                if (ppp1 == IntPtr.Zero) { return false; }
                if (ppp2 == IntPtr.Zero) { return false; }

                //读取ppp1指针的内存数据. retPathsCount个int
                int reByteCount = retPathsCount * sizeof(int);
                byte[] retBytes = new byte[reByteCount];
                int iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp1, iii);
                    iii++;
                }
                iii = 0;
                int vNodeCount = 0;
                List<int> perPathCount = new List<int>();
                while (iii < retPathsCount)
                {
                    int xcount = (int)retBytes[iii * 4] +
                   ((int)retBytes[iii * 4 + 1]) * 256 +
                   ((int)retBytes[iii * 4 + 2]) * 65536 +
                   ((int)retBytes[iii * 4 + 3]) * 16777216;
                    perPathCount.Add(xcount);
                    vNodeCount += xcount;
                    iii++;
                }

                //读取ppp2指针的内存数据. vNodeCount个int
                reByteCount = vNodeCount * sizeof(int);
                retBytes = new byte[reByteCount];
                iii = 0;
                while (iii < reByteCount)
                {
                    retBytes[iii] = Marshal.ReadByte(ppp2, iii);
                    iii++;
                }
                iii = 0;
                int vNodeIndex = 0;
                retPathsFids = new List<List<int>>();
                for (int i = 0; i < retPathsCount; i++)
                {
                    List<int> pfids = new List<int>();
                    for (int j = 0; j < perPathCount[i]; j++)
                    {
                        int xfid = (int)retBytes[vNodeIndex * 4] +
                            ((int)retBytes[vNodeIndex * 4 + 1]) * 256 +
                            ((int)retBytes[vNodeIndex * 4 + 2]) * 65536 +
                            ((int)retBytes[vNodeIndex * 4 + 3]) * 16777216;
                        pfids.Add(xfid);

                        ++vNodeIndex;
                    }
                    retPathsFids.Add(pfids);
                }


                TopologyMethod.removeMemery(ppp1, retPathsCount);
                TopologyMethod.removeMemery(ppp2, vNodeCount);
            }
            catch (Exception ex) { }
            return flag;
        }

        ///
        ///调用c++ dll接口初始化图,传入图的顶点 
        ///
        [DllImport("CyzToPology.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void createGraphFromCSharp(DLAdjNode[] dd, int ddCoung);
        ///
        ///清理c++ dll 里面new的内存.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void removeMemery(IntPtr retPtr, int retPathNodeCount);
        ///
        ///调用c++ dll接口,求两点最短路径.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool p2pPath(int startFid, int endFid, ref float retDistance, ref int retPathNodeCount, ref IntPtr retPtr);

        ///
        ///求一点到到电源点的上游追踪.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll")]
        private static extern bool upPath(int startFid, ref float retDistance, ref int retPathNodeCount, ref IntPtr retPtr);
        ///
        ///求一点到到下游叶子的下游追踪.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll")]
        private static extern bool downPath(int startFid, ref int retPathsCount, ref IntPtr retPathsNodeCount, ref IntPtr retPtr);

        ///
        ///求一点到其他所有叶子点的最短路径 
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll")]
        private static extern bool allPath(int startFid, ref int retPathsCount, ref IntPtr retPathsNodeCount, ref IntPtr retPtr);

        ///
        ///求一点到到 多 电源点的上游追踪.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll")]
        private static extern bool upPath2(int startFid, ref int retPathsCount, ref IntPtr retPathsNodeCount, ref IntPtr retPtr);
        ///
        ///求一点到到下游叶子的下游追踪.
        ///用户不需要调用
        ///
        [DllImport("CyzToPology.dll")]
        private static extern bool downPath2(int startFid, ref int retPathsCount, ref IntPtr retPathsNodeCount, ref IntPtr retPtr);

    }
}
