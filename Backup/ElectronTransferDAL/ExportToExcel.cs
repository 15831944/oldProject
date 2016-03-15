using System;
using System.Linq;
using ElectronTransferDal.QueryVerifyHelper;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System.IO;
using System.Collections.Generic;

namespace ElectronTransferDal
{
    public class ExportToExcel
    {
        public HSSFWorkbook WorkBook { get; set; }
        public ISheet Sheet { get; set; }
        private HSSFCellStyle cellstyleForOneRow = null;
        private HSSFCellStyle cellstyleForVerifySuccess = null;
        private HSSFCellStyle cellstyleForVerifyFailed = null;
        public ExportToExcel()
        {
            WorkBook = new HSSFWorkbook();
            SetStyleForRow();
        }
        public void CreateSheet(string sheetName)
        {
            if (WorkBook==null)
            {
                throw new Exception("工作薄未初始化");
            }
            Sheet = WorkBook.CreateSheet(sheetName);
        }
        private void FillCell(int rownum, string[] cellValue)
        {
            if (WorkBook == null || Sheet == null)
            {
                throw new Exception("工作薄或工作页面没被初始化");
            }
            Sheet.SetColumnWidth(0, 20 * 256);
            Sheet.SetColumnWidth(1, 22 * 256);
            Sheet.SetColumnWidth(2, 50 * 256);
            Sheet.SetColumnWidth(3, 20 * 256);
            Sheet.SetColumnWidth(4, 20 * 2000);
            var lastOrDefault = cellValue.LastOrDefault();
            bool flag = lastOrDefault != null && (lastOrDefault.Equals("校验成功") ? false : true);
            var row = Sheet.CreateRow(rownum);
            for (int i = 0; i < cellValue.Count(); i++)
            {
                var cell = row.CreateCell(i, CellType.STRING);
                if (rownum==0)
                {
                    cell.CellStyle = cellstyleForOneRow;
                }
                else
                {
                    if (flag)
                    {
                        cell.CellStyle = cellstyleForVerifyFailed;
                    }
                    else
                    {
                        cell.CellStyle = cellstyleForVerifySuccess;
                    }

                }
                cell.SetCellValue(cellValue[i]);
            }
        }

        public void FillCells(List<VerifyClass> collection)
        {
            if (WorkBook == null || Sheet == null)
            {
                throw new Exception("工作薄或工作页面没被初始化");
            }
            FillCell(0, new string[] { "G3E_FID", "设备类型", "设备名称", "修改状态", "校验结果" });
            int i = 1;
            foreach (var item in collection)
            {
                FillCell(i, new string[]
                {
                    item.G3E_FID.ToString(),
                    item.DeviceType,
                    item.SBMC,
                    item.DevState,
                    item.VerifyResult

                });
                i++;
            }
        }
        private void SetStyleForRow()
        {
            #region 标题行
            cellstyleForOneRow = (HSSFCellStyle)WorkBook.CreateCellStyle();
            HSSFFont font = (HSSFFont)WorkBook.CreateFont();
            font.Color = HSSFColor.BLACK.index;
            font.FontHeightInPoints = 15;
            cellstyleForOneRow.SetFont(font);
            #endregion
           
            #region 校验失败行
            cellstyleForVerifyFailed = (HSSFCellStyle)WorkBook.CreateCellStyle();
            cellstyleForVerifyFailed.FillForegroundColor = HSSFColor.LIGHT_ORANGE.index;
            cellstyleForVerifyFailed.FillPattern = FillPatternType.SOLID_FOREGROUND;
            HSSFFont font1 = (HSSFFont)WorkBook.CreateFont();
            font1.Color = HSSFColor.BLACK.index;
            font1.FontHeightInPoints = 12;
            cellstyleForVerifyFailed.SetFont(font1);
            #endregion

            #region 校验成功行
            cellstyleForVerifySuccess = (HSSFCellStyle)WorkBook.CreateCellStyle();
            cellstyleForVerifySuccess.FillForegroundColor = HSSFColor.SKY_BLUE.index;
            cellstyleForVerifySuccess.FillPattern = FillPatternType.SOLID_FOREGROUND;
            HSSFFont font2 = (HSSFFont)WorkBook.CreateFont();
            font2.Color = HSSFColor.BLACK.index;
            font2.FontHeightInPoints = 12;
            cellstyleForVerifySuccess.SetFont(font2);
            #endregion
        }
        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="rownum"></param>
        /// <param name="isFill">是否填充背景色</param>
        /// <returns></returns>
        //private HSSFCellStyle SetStyle(int rownum,bool isFill)
        //{
            
        //    if (isFill)
        //    {
        //        if (rownum%2 == 0)
        //        {
        //            cellstyle.FillForegroundColor = HSSFColor.LIGHT_ORANGE.index;
        //        }
        //        else
        //            cellstyle.FillForegroundColor = HSSFColor.SKY_BLUE.index;
        //        cellstyle.FillPattern = FillPatternType.SOLID_FOREGROUND;  
        //        //cellstyle.FillBackgroundColor = HSSFColor.RED.index;
        //    }
        //    return cellstyle;
        //}
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        private short GetColor(System.Drawing.Color sysColor)
        {
            short s = 0;
            HSSFPalette xlPalette = WorkBook.GetCustomPalette();
            HSSFColor xlColor = xlPalette.FindColor(sysColor.R, sysColor.G, sysColor.B);
            if (xlColor == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE<=255)
                {
                    if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 64)
                    {
                        NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE = 64;
                        NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE += 1;
                        xlColor = xlPalette.AddColor(sysColor.R, sysColor.G, sysColor.B);
                    }
                    else
                    {
                        xlColor = xlPalette.FindSimilarColor(sysColor.R, sysColor.G, sysColor.B);
                    }
                    s = xlColor.GetIndex();
                }
            }
            else
                s = xlColor.GetIndex();
            return s;
        }
        public void ExportToFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("请选择保存的路径");
            }
            using (FileStream stream = new FileStream(filePath, System.IO.FileMode.Create, FileAccess.ReadWrite))
            {
                WorkBook.Write(stream);
            }
        }
    }
}
