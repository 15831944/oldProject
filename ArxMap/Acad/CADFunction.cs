using Autodesk.AutoCAD.ApplicationServices;

namespace ArxMap.Acad
{
    public class CADFunction
    {
        public static void getWorldExtents(ref Autodesk.AutoCAD.Geometry.Vector3d wMin, ref Autodesk.AutoCAD.Geometry.Vector3d wMax, ref double winw, ref double winh)
        {
            double worldcx = 0, worldcy = 0, worldcz = 0;	//世界中心点
            double worldh = 0;			//世界纵向长度

            var _screensize =
            (Autodesk.AutoCAD.Geometry.Point2d)Application.GetSystemVariable("SCREENSIZE");

            winw = _screensize.X;
            winh = _screensize.Y;


            var _viewcenter =
                (Autodesk.AutoCAD.Geometry.Point3d)Application.GetSystemVariable("VIEWCTR");
            worldcx = _viewcenter.X;
            worldcy = _viewcenter.Y;
            worldcz = _viewcenter.Z;


            var _viewsize = (double)Application.GetSystemVariable("VIEWSIZE");
            worldh = _viewsize;

            wMin = new Autodesk.AutoCAD.Geometry.Vector3d(worldcx - (worldh * (winw / winh) / 2), worldcy - (worldh / 2), 0);
            wMax = new Autodesk.AutoCAD.Geometry.Vector3d(worldcx + (worldh * (winw / winh) / 2), worldcy + (worldh / 2), 0);
        }

        //    public static void setWorldExtents(ref Autodesk.AutoCAD.Geometry.Vector3d wMin, ref Autodesk.AutoCAD.Geometry.Vector3d wMax)
        //{
        //    double winw=0,winh=0;		//屏幕长宽
        //double worldcx=0,worldcy=0,worldcz=0;	//世界中心点
        //double worldh=0,worldw=0;			//世界纵向长度

        //worldcx=(wMin.X+wMax.X)/2;
        //worldcy=(wMin.Y+wMax.Y)/2;
        //worldcz=0;
        //worldw=wMax.X-wMin.X;
        //worldh=wMax.Y-wMin.Y;

        ////

        //AcDbViewTableRecord view ;
        //GetCurrentView(view);

        //// 设置视图的中心点
        //AcGePoint2d apcent;
        //apcent.x=worldcx;
        //apcent.y=worldcy;
        //view.setCenterPoint(apcent);

        //// 设置视图的高度和宽度
        //view.setHeight(worldh);
        //view.setWidth(worldw);

        //// 将视图对象设置为当前视图
        //Acad::ErrorStatus es = acedSetCurrentView(&view, NULL);}

        //    public static void GetCurrentView(ref Autodesk.AutoCAD.DatabaseServices.ViewportTableRecord vv)
        //    {
        //Autodesk.AutoCAD.DatabaseServices.ResultBuffer rb;
        //Autodesk.AutoCAD.DatabaseServices.ResultBuffer wcs, ucs, dcs; // 转换坐标时使用的坐标系统标记

        //wcs.restype = RTSHORT;
        //wcs.resval.rint = 0;
        //ucs.restype = RTSHORT;
        //ucs.resval.rint = 1;
        //dcs.restype = RTSHORT;
        //dcs.resval.rint = 2;

        //// 获得当前视口的“查看”模式
        //acedGetVar(L"VIEWMODE", &rb);
        //view.setPerspectiveEnabled(rb.resval.rint & 1);
        //view.setFrontClipEnabled(rb.resval.rint & 2);
        //view.setBackClipEnabled(rb.resval.rint & 4);
        //view.setFrontClipAtEye(!(rb.resval.rint & 16)); 

        //// 当前视口中视图的中心点（UCS坐标）
        //acedGetVar(L"VIEWCTR", &rb);
        //acedTrans(rb.resval.rpoint, &ucs, &dcs, 0, rb.resval.rpoint);
        //view.setCenterPoint(AcGePoint2d(rb.resval.rpoint[X], 
        //    rb.resval.rpoint[Y])); 

        //// 当前视口透视图中的镜头焦距长度（单位为毫米）
        //acedGetVar(L"LENSLENGTH", &rb);
        //view.setLensLength(rb.resval.rreal);

        //// 当前视口中目标点的位置（以 UCS 坐标表示）
        //acedGetVar(L"TARGET", &rb);
        //acedTrans(rb.resval.rpoint, &ucs, &wcs, 0, rb.resval.rpoint);
        //view.setTarget(AcGePoint3d(rb.resval.rpoint[X], 
        //    rb.resval.rpoint[Y], rb.resval.rpoint[Z]));

        //// 当前视口的观察方向（UCS）
        //acedGetVar(L"VIEWDIR", &rb);
        //acedTrans(rb.resval.rpoint, &ucs, &wcs, 1, rb.resval.rpoint);
        //view.setViewDirection(AcGeVector3d(rb.resval.rpoint[X], 
        //    rb.resval.rpoint[Y], rb.resval.rpoint[Z]));

        //// 当前视口的视图高度（图形单位）
        //acedGetVar(L"VIEWSIZE", &rb);
        //view.setHeight(rb.resval.rreal);
        //double height = rb.resval.rreal;

        //// 以像素为单位的当前视口的大小（X 和 Y 值）
        //acedGetVar(L"SCREENSIZE", &rb);
        //view.setWidth(rb.resval.rpoint[X] / rb.resval.rpoint[Y] * height);

        //// 当前视口的视图扭转角
        //acedGetVar(L"VIEWTWIST", &rb);
        //view.setViewTwist(rb.resval.rreal);

        //// 将模型选项卡或最后一个布局选项卡置为当前
        //acedGetVar(L"TILEMODE", &rb);
        //int tileMode = rb.resval.rint;
        //// 设置当前视口的标识码
        //acedGetVar(L"CVPORT", &rb);
        //int cvport = rb.resval.rint;

        //// 是否是模型空间的视图
        //bool paperspace = ((tileMode == 0) && (cvport == 1)) ? true : false;
        //view.setIsPaperspaceView(paperspace);

        //if (!paperspace)
        //{
        //    // 当前视口中前向剪裁平面到目标平面的偏移量
        //    acedGetVar(L"FRONTZ", &rb);
        //    view.setFrontClipDistance(rb.resval.rreal);

        //    // 获得当前视口后向剪裁平面到目标平面的偏移值
        //    acedGetVar(L"BACKZ", &rb);
        //    view.setBackClipDistance(rb.resval.rreal);
        //}
        //else
        //{
        //    view.setFrontClipDistance(0.0);
        //    view.setBackClipDistance(0.0);
        //}
        //    }
    }
}
