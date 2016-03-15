using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ElectronTransferDal
{

    /// <summary>
    /// 重写比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPorpertyCompare<T> : System.Collections.Generic.IComparer<T>
    {
        private PropertyDescriptor property;
        private ListSortDirection direction;
        public ObjectPorpertyCompare(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }
        #region  IComparer<T>
        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">相对属性X</param>
        /// <param name="y">相对属性Y</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);

            int returnValue;

            if (xValue!=null&&yValue!=null&&(xValue  is IComparable))
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue==null||yValue==null)
            {
                if (xValue==null&&yValue!=null)
                {
                    returnValue = 1;
                }else if (xValue!=null&&yValue==null)
                {
                    returnValue = -1;
                }else
                {
                    returnValue=0;
                }
            }
            else
            {
                returnValue = xValue.ToString().CompareTo(yValue.ToString());
            }

            if (direction == ListSortDirection.Ascending)
            {
                return returnValue;
            }
            else
            {
                return returnValue * -1;
            }
        }
        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
    /// <summary>
    /// 用来绑定数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindingCollection<T> : BindingList<T>
    {
        private bool isSorted;
        public PropertyDescriptor sortProperty;
        public ListSortDirection sortDirection;

        protected override bool IsSortedCore
        {
            get
            {
                return isSorted;
            }
        }
        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }
        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return sortDirection;
            }
        }
        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                return sortProperty;
            }
        }
        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;
            if (items != null)
            {
                ObjectPorpertyCompare<T> pc = new ObjectPorpertyCompare<T>(prop, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }
            sortProperty = prop;
            sortDirection = direction;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        public void Sort(PropertyDescriptor property, ListSortDirection direction)
        {
            this.ApplySortCore(property, direction);
        }

    }
}
