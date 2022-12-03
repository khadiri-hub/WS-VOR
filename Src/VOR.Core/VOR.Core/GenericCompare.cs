using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Reflection;

namespace VOR.Core
{
    public class GenericComparer<T> : IComparer<T>
    {
        private SortDirection _sortDirection;
        private string _sortExpression;

        public SortDirection SortDirection
        {
            get { return this._sortDirection; }
            set { this._sortDirection = value; }
        }

        public GenericComparer(string sortExpression, SortDirection sortDirection)
        {
            this._sortExpression = sortExpression;
            this._sortDirection = sortDirection;
        }

        public int Compare(T x, T y)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(_sortExpression);
            IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
            IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);

            if (SortDirection == SortDirection.Ascending)
            {
                if (obj1 == null && obj1 == obj2)
                    return 0;
                else if (obj1 == null)
                    return -1;

                return obj1.CompareTo(obj2);
            }
            else
            {
                if (obj2 == null && obj2 == obj1)
                    return 0;
                else if (obj2 == null)
                    return -1;

                return obj2.CompareTo(obj1);
            }
        }

    }
}