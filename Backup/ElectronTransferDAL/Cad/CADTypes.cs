using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.Cad
{
    public class CADTypes
    {
        /// <summary>
        /// 创建线样式
        /// </summary>
        [CommandMethod("CreateLineType")]
        public static void CreateLineType()
        {
            var db=HostApplicationServices.WorkingDatabase;
            using (Transaction trans=db.TransactionManager.StartTransaction())
            {
                //添加电缆
                var cable = db.AddLineType("DashLines");
                var ltr = trans.GetObject(cable, OpenMode.ForWrite) as LinetypeTableRecord;
                ltr.AsciiDescription = "电缆";
                ltr.PatternLength = 0.44 / MapConfig.Instance.earthscale;//组成线型的图案长度（划线、空格、点）
                ltr.NumDashes = 4;//组成线型的图案数目
                ltr.SetDashLengthAt(0, 0.165 / MapConfig.Instance.earthscale);//0.2个单位的划线
                ltr.SetDashLengthAt(1, -0.055 / MapConfig.Instance.earthscale);//0.1个单位的空格
                ltr.SetDashLengthAt(2, 0.165 / MapConfig.Instance.earthscale);//一个点
                ltr.SetDashLengthAt(3, -0.055 / MapConfig.Instance.earthscale);//0.1个单位的空格
                db.Celtype = cable;

          
                //打开文字样式表，用于文字类型的线型
                var tst = (TextStyleTable)trans.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                //添加带文字的线型
                var ltTextId = db.AddLineType("Texts");
                var ltrText = (LinetypeTableRecord)trans.GetObject(ltTextId, OpenMode.ForWrite);
                ltrText.AsciiDescription = "文字";//线型说明
                ltrText.PatternLength = 0.9;//组成线型的图案长度（划线、空格、点）
                ltrText.NumDashes = 3;//组成线型的图案数目
                ltrText.SetDashLengthAt(0, 0.5);//0.5个单位的划线
                ltrText.SetDashLengthAt(1, -0.2);//0.2个单位的空格
                ltrText.SetShapeStyleAt(1, tst["Standard"]);//设置文字的文字样式
                //文字在线型的 X 轴方向上向左移动0.1个单位，在Y轴方向向下移动0.05个单位。
                ltrText.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltrText.SetShapeScaleAt(1, 0.1);//文字的缩放比例
                ltrText.SetShapeRotationAt(1, 0);//文字的旋转角度为0（不旋转）
                ltrText.SetTextAt(1, "CAD");//文字内容
                ltrText.SetDashLengthAt(2, -0.2);//0.2个单位的空格
                //将ltypeshp.shx文件添加到当前数据库，该文件包含圆形图案
                var txtStyleId = db.AddShapeTextStyle("ShpText", "ltypeshp.shx");
                //添加圆型线型
                var ltCirId = db.AddLineType("Circles");
                var ltrCir = (LinetypeTableRecord)trans.GetObject(ltCirId, OpenMode.ForWrite);
                ltrCir.AsciiDescription = "圆";//线型说明
                ltrCir.PatternLength = 1.45;//组成线型的图案长度（划线、空格、点）
                ltrCir.NumDashes = 4;//组成线型的图案数目
                ltrCir.SetDashLengthAt(0, 0.25);//0.25个单位的划线
                ltrCir.SetDashLengthAt(1, -0.1);//0.1个单位的空格
                ltrCir.SetShapeStyleAt(1, txtStyleId);//设置空格处的图形文件
                //设置空格处要包含的图形为圆形
                ltrCir.SetShapeNumberAt(1, (int)LineTypeTools.Shape.Circle);
                //图形在线型的 X 轴方向上向左移动0.1个单位，在Y轴方向不移动。
                ltrCir.SetShapeOffsetAt(1, new Vector2d(-0.1, 0.0));
                ltrCir.SetShapeScaleAt(1, 0.1);//图形的缩放比例
                ltrCir.SetShapeRotationAt(1, 0);//文字的旋转角度为0（不旋转）
                ltrCir.SetDashLengthAt(2, -0.1);//0.1个单位的空格
                ltrCir.SetDashLengthAt(3, 1.0);//1个单位的划线 
                //db.Celtype = ltId;//设置当前线型为虚线
                trans.Commit();
            }
        }
        /// <summary>
        /// 创建文本样式
        /// </summary>
        public static ObjectId CreateStyle()
        {
            var TextstyleId = new ObjectId();
            var db = HostApplicationServices.WorkingDatabase;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                var st = (TextStyleTable)trans.GetObject(db.TextStyleTableId, OpenMode.ForWrite);
                const string StyleName = "LableStyle";
                const string StyleName2 = "MultLableStyle";
                if (st.Has(StyleName) == false)
                {
                    var str = new TextStyleTableRecord
                                  {
                                      Name = StyleName,
                                      FileName = "SIMHEI.TTF",
                                      ObliquingAngle = 0*Math.PI/180,
                                      XScale = 0.67
                                  };

                    // 设置TrueType字体(黑体）
                    //---------------------------------------------
                    // 设置SHX字体
                    // str.FileName = "gbenor"
                    //设置大字体.
                    // str.BigFontFileName = "gbcbig"
                    // --------------------------------------------
                    TextstyleId = st.Add(str);
                    trans.AddNewlyCreatedDBObject(str, true);
                    db.Textstyle = TextstyleId;
                }
                if (st.Has(StyleName2) == false)
                {
                    var str = new TextStyleTableRecord
                                  {
                                      Name = StyleName2,
                                      FileName = "SIMHEI.TTF",
                                      ObliquingAngle = 0*Math.PI/180,
                                      XScale = 1
                                  };
                    TextstyleId = st.Add(str);
                    trans.AddNewlyCreatedDBObject(str, true);
                    PublicMethod.Instance.MultTextStyleId = str.Id;
                }
                trans.Commit();
            }
            return TextstyleId;
        }

    }
}
