using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Exception = System.Exception;
using acApp = Autodesk.AutoCAD.ApplicationServices;

namespace ElectronTransferDal.Cad
{
    public class PreviewDwg
    {
        private struct BITMAPFILEHEADER
        {
            public short bfType;
            public int bfSize;
            public short bfReserved1;
            public short bfReserved2;
            public int bfOffBits;
        }

        public static Image GetDwgImage(string FileName)
        {
            if (!(File.Exists(FileName)))
            {
                throw new FileNotFoundException("文件没有被找到");
            }
            MemoryStream BMPF = new MemoryStream(); //保存位图的内存文件流
            BinaryWriter bmpr = new BinaryWriter(BMPF); //写二进制文件类
            Image myImg = null;
            try
            {
                FileStream DwgF = new FileStream(FileName, FileMode.Open, FileAccess.Read); //文件流
                BinaryReader br = new BinaryReader(DwgF); //读取二进制文件
                DwgF.Seek(13, SeekOrigin.Begin); //从第十三字节开始读取
                int PosSentinel = br.ReadInt32(); //文件描述块的位置
                DwgF.Seek(PosSentinel + 30, SeekOrigin.Begin); //将指针移到缩略图描述块的第31字节
                int TypePreview = br.ReadByte(); //缩略图格式
                if (TypePreview == 1)
                {
                }
                else if (TypePreview == 2 || TypePreview == 3)
                {
                    int PosBMP = br.ReadInt32(); //缩略图位置
                    int LenBMP = br.ReadInt32(); //缩略图大小
                    DwgF.Seek(PosBMP + 14, SeekOrigin.Begin); //移动指针到位图块
                    short biBitCount = br.ReadInt16(); //缩略图比特深度
                    DwgF.Seek(PosBMP, SeekOrigin.Begin); //从位图块开始处读取全部位图内容备用
                    byte[] BMPInfo = br.ReadBytes(LenBMP); //包含在DWG文件中的BMP文件体
                    br.Close();
                    DwgF.Close();
                    BITMAPFILEHEADER biH; //BMP文件头，DWG文件中不包含位图文件头，要自行加上去
                    biH.bfType = 19778; //建立位图文件头
                    if (biBitCount < 9)
                    {
                        biH.bfSize = 54 + 4 * (int)(Math.Pow(2, biBitCount)) + LenBMP;
                    }
                    else
                    {
                        biH.bfSize = 54 + LenBMP;
                    }
                    biH.bfReserved1 = 0; //保留字节
                    biH.bfReserved2 = 0; //保留字节
                    biH.bfOffBits = 14 + 40 + 1024; //图像数据偏移
                    //以下开始写入位图文件头
                    bmpr.Write(biH.bfType); //文件类型
                    bmpr.Write(biH.bfSize); //文件大小
                    bmpr.Write(biH.bfReserved1); //0
                    bmpr.Write(biH.bfReserved2); //0
                    bmpr.Write(biH.bfOffBits); //图像数据偏移
                    bmpr.Write(BMPInfo); //写入位图
                    BMPF.Seek(0, SeekOrigin.Begin); //指针移到文件开始处
                    myImg = Image.FromStream(BMPF); //创建位图文件对象
                    bmpr.Close();
                    BMPF.Close();
                }
                return myImg;
            }
            catch (EndOfStreamException)
            {
                throw new EndOfStreamException("文件不是标准的DWG格式文件，无法预览！");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

      

        ///显示DWG文件
        ///

        /// 要显示的宽度
        /// 要显示的高度
        /// 
        public static Image ShowDWG(int Pwidth, int PHeight, string FilePath)
        {
            Image image = GetDwgImage(FilePath);
            if(image==null) return image;
            Bitmap bitmap = new Bitmap(image);
            int Height = bitmap.Height;
            int Width = bitmap.Width;
            Bitmap newbitmap = new Bitmap(Width, Height);
            Bitmap oldbitmap = (Bitmap)bitmap;
            Color pixel;
            for (int x = 1; x < Width; x++)
            {
                for (int y = 1; y < Height; y++)
                {

                    pixel = oldbitmap.GetPixel(x, y);
                    int r = pixel.R, g = pixel.G, b = pixel.B;
                    if (pixel.Name == "ffffffff" || pixel.Name == "ff000000")
                    {
                        r = 255 - pixel.R;
                        g = 255 - pixel.G;
                        b = 255 - pixel.B;
                    }

                    newbitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            Bitmap bt = new Bitmap(newbitmap, Pwidth, PHeight);
            return newbitmap;
        }
    }
    public class SaveImage
    {
        // For the coordinate tranformation we need... 

        // A Win32 function:

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

        // Command to capture the main and active drawing windows

        [CommandMethod("CSS")]
        static public void CaptureScreenShot()
        {
            //ScreenShotToFile(
            //  acApp.Application.MainWindow,
            //  0, 0, 0, 0,
            //  "c:\\main-window.png",
            //  false
            //);

            ScreenShotToFile(
              acApp.Application.DocumentManager.MdiActiveDocument.Window,
              30, 26, 10, 10,
              "F:\\dwg\\1.jpg",
              false
            );
        }

        // Command to capture a user-selected portion of a drawing

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        static public Bitmap WindowScreenShot(Point3d startPoint, Point3d endPoint,string filePath)
        {
            acApp.Document doc =
              acApp.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            short vp =
              (short)acApp.Application.GetSystemVariable("CVPORT");


            IntPtr hWnd = doc.Window.Handle;

            Point pt1 = ScreenFromDrawingPoint(ed, hWnd, startPoint, vp);
            Point pt2 = ScreenFromDrawingPoint(ed, hWnd, endPoint, vp);

            return ScreenShotToFile(pt1, pt2, filePath, true);
        }

        static public void WindowScreenShot(string savePath)
        {
            acApp.Document doc =acApp.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            PromptPointResult ppr =
              ed.GetPoint("\n选择第一个点的捕捉窗口:");
            if (ppr.Status != PromptStatus.OK)
                return;
            Point3d first = ppr.Value;

            ppr =
              ed.GetCorner(
                "\n选择第二点捕获窗口:",
                first
              );
            if (ppr.Status != PromptStatus.OK)
                return;
            Point3d second = ppr.Value;


            // Generate screen coordinate points based on the
            // drawing points selected


            // First we get the viewport number

            short vp =(short)acApp.Application.GetSystemVariable("CVPORT");

            // Then the handle to the current drawing window

            IntPtr hWnd = doc.Window.Handle;

            // Now calculate the selected corners in screen coordinates

            Point pt1 = ScreenFromDrawingPoint(ed, hWnd, first, vp);
            Point pt2 = ScreenFromDrawingPoint(ed, hWnd, second, vp);

            // Now save this portion of our screen as a raster image

            ScreenShotToFile(pt1, pt2, savePath, true);
        }

        // Perform our three tranformations to get from UCS (or WCS)
        // to screen coordinates

        private static Point ScreenFromDrawingPoint(
          Editor ed,
          IntPtr hWnd,
          Point3d ucsPt,
          short vpNum
        )
        {
            Point3d wcsPt =
              ucsPt.TransformBy(
                ed.CurrentUserCoordinateSystem
              );
            Point res = ed.PointToScreen(wcsPt, vpNum);
            ClientToScreen(hWnd, ref res);
            return res;
        }

        private static void ScreenShotToFile(
          Autodesk.AutoCAD.Windows.Window wd,
          int top, int bottom, int left, int right,
          string filename,
          bool clipboard
        )
        {
            Point pt = wd.Location;
            Size sz = wd.Size;

            pt.X += left;
            pt.Y += top;
            sz.Height -= top + bottom;
            sz.Width -= left + right;

            SaveScreenPortion(pt, sz, filename, clipboard);
        }

        private static Bitmap ScreenShotToFile(Point pt1, Point pt2, string filename, bool clipboard)
        {
            Point pt =
              new Point(Math.Min(pt1.X, pt2.X), Math.Min(pt1.Y, pt2.Y));

            Size sz =
              new Size(Math.Abs(pt1.X - pt2.X), Math.Abs(pt1.Y - pt2.Y));

           return  SaveScreenPortion(pt, sz, filename, clipboard);
        }

        private static Bitmap SaveScreenPortion(Point pt,Size sz,string filename, bool clipboard)
        {
            Bitmap bmp =
              new Bitmap(
                sz.Width,
                sz.Height,
                PixelFormat.Format32bppArgb
              );
            using (bmp)
            {
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    gfx.CopyFromScreen(
                      pt.X, pt.Y, 0, 0, sz,
                      CopyPixelOperation.SourceCopy
                    );
                }
                if(!string.IsNullOrEmpty(filename))
                {
                    bmp.Save(filename,ImageFormat.Jpeg);
                }
            }
            return bmp;
        }
    }
}
