using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ElectronTransferDal.QueryVerifyHelper;

namespace ElectronTransferDal.AutoGeneration
{
    public class ListViewColumnSorter:IComparer
    {
        private int ColumnToSort;
        private SortOrder orderSort;
        private CaseInsensitiveComparer ObjectCompare;
        public int SortColumn
        {
            get { return ColumnToSort; }
            set { ColumnToSort = value; }
        }

        public SortOrder Order
        {
            get { return orderSort; }
            set { orderSort = value; }
        }
        public ListViewColumnSorter()
        {
            ColumnToSort = 7;
            orderSort = SortOrder.Ascending;
            ObjectCompare = new CaseInsensitiveComparer();
        }

        #region IComparer 成员

        int IComparer.Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;
            listviewX = (ListViewItem) x;
            listviewY = (ListViewItem) y;
            if (ColumnToSort == 7)
            {
                try
                {
                    VerifyClass vc = (VerifyClass)listviewX.Tag;
                    var lvXValue = listviewX.SubItems[ColumnToSort].Text;
                    var lvYValue = listviewY.SubItems[ColumnToSort].Text;
                    if (lvXValue.Equals("校验成功") && !lvYValue.Equals("校验成功"))
                    {
                        compareResult = 1;
                    }
                    else if (!lvXValue.Equals("校验成功") && lvYValue.Equals("校验成功"))
                    {
                        compareResult = -1;
                    }
                    else
                        compareResult = 0;
                }
                catch (InvalidCastException)
                {
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text,
                   listviewY.SubItems[ColumnToSort].Text);
                }

            }
            else
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text,
                    listviewY.SubItems[ColumnToSort].Text);
            }
            if (orderSort == SortOrder.Ascending)
            {
                return compareResult;
            }else if (orderSort == SortOrder.Descending)
            {
                return -compareResult;
            }
            else
                return 0;
        }

        #endregion
    }
}
