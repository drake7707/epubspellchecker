using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace EpubSpellChecker
{
    public class SortableBindingList<T> : BindingList<T>, IBindingListView
    {
        private readonly Dictionary<Type, PropertyComparer<T>> comparers;
        private bool isSorted;
        private ListSortDirection listSortDirection;
        private PropertyDescriptor propertyDescriptor;

        private IEnumerable<T> originalList;

        public IEnumerable<T> OriginalList
        {
            get { return originalList; }
        }

        public SortableBindingList()
            : base(new List<T>())
        {
            this.originalList = System.Linq.Enumerable.Empty<T>();

            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration))
        {
            this.originalList = enumeration;
            this.comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return this.isSorted; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.propertyDescriptor; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return this.listSortDirection; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> itemsList = (List<T>)this.Items;

            Type propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!this.comparers.TryGetValue(propertyType, out comparer))
            {
                comparer = new PropertyComparer<T>(property, direction);
                this.comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            this.propertyDescriptor = property;
            this.listSortDirection = direction;
            this.isSorted = true;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.propertyDescriptor = base.SortPropertyCore;
            this.listSortDirection = base.SortDirectionCore;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            int count = this.Count;
            for (int i = 0; i < count; ++i)
            {
                T element = this[i];
                if (property.GetValue(element).Equals(key))
                {
                    return i;
                }
            }

            return -1;
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {

        }

        internal void ApplyFilter(SingleFilterInfo filterParts)
        {
            List<T> results;

            // Check to see if the property type we are filtering by implements 
            // the IComparable interface. 
            Type interfaceType =
                TypeDescriptor.GetProperties(typeof(T))[filterParts.PropName]
                .PropertyType.GetInterface("IComparable");

            if (interfaceType == null)
                throw new InvalidOperationException("Filtered property" +
                " must implement IComparable.");

            results = new List<T>();

            // Check each value and add to the results list. 
            foreach (T item in this)
            {

                IComparable compareValue = filterParts.PropDesc.GetValue(item) as IComparable;


                if (compareValue != null)
                {
                    if (filterParts.OperatorValue == "%LIKE%" && (compareValue + "").ToLower().Contains((filterParts.CompareValue + "").ToLower()))
                    {
                        results.Add(item);
                    }
                    else
                    {
                        int result = compareValue.CompareTo(filterParts.CompareValue);
                        if (filterParts.OperatorValue == "=" && result == 0)
                            results.Add(item);
                        else if (filterParts.OperatorValue == ">" && result > 0)
                            results.Add(item);
                        else if (filterParts.OperatorValue == "<" && result < 0)
                            results.Add(item);
                    }
                }
            }
            this.ClearItems();
            foreach (T itemFound in results)
                this.Add(itemFound);


        }


        internal SingleFilterInfo ParseFilter(string filterPart)
        {
            SingleFilterInfo filterInfo = new SingleFilterInfo();
            filterInfo.OperatorValue = DetermineFilterOperator(filterPart);

            string[] filterStringParts =
                filterPart.Split(filterInfo.OperatorValue.ToCharArray()).Where(p => !string.IsNullOrEmpty(p)).ToArray();

            filterInfo.PropName =
                filterStringParts[0].Replace("[", "").
                Replace("]", "").Replace(" AND ", "").Trim();

            // Get the property descriptor for the filter property name. 
            PropertyDescriptor filterPropDesc =
                TypeDescriptor.GetProperties(typeof(T))[filterInfo.PropName];

            // Convert the filter compare value to the property type. 
            if (filterPropDesc == null)
                throw new InvalidOperationException("Specified property to " +
                    "filter " + filterInfo.PropName +
                    " on does not exist on type: " + typeof(T).Name);

            filterInfo.PropDesc = filterPropDesc;

            string comparePartNoQuotes = StripOffQuotes(filterStringParts[1]);
            try
            {
                TypeConverter converter =
                    TypeDescriptor.GetConverter(filterPropDesc.PropertyType);
                filterInfo.CompareValue =
                    converter.ConvertFromString(comparePartNoQuotes);
            }
            catch (NotSupportedException)
            {
                throw new InvalidOperationException("Specified filter" +
                    "value " + comparePartNoQuotes + " can not be converted" +
                    "from string. Implement a type converter for " +
                    filterPropDesc.PropertyType.ToString());
            }
            return filterInfo;
        }

        internal string DetermineFilterOperator(string filterPart)
        {
            // Determine the filter's operator. 
            if (Regex.IsMatch(filterPart, "[^>^<]="))
                return "=";
            else if (Regex.IsMatch(filterPart, "<[^>^=]"))
                return "<";
            else if (Regex.IsMatch(filterPart, "[^<]>[^=]"))
                return ">";
            else if (Regex.IsMatch(filterPart, "%LIKE%", RegexOptions.IgnoreCase))
                return "%LIKE%";
            else
                return "";
        }

        internal static string StripOffQuotes(string filterPart)
        {
            // Strip off quotes in compare value if they are present. 
            if (Regex.IsMatch(filterPart, "'.+'"))
            {
                int quote = filterPart.IndexOf('\'');
                filterPart = filterPart.Remove(quote, 1);
                quote = filterPart.LastIndexOf('\'');
                filterPart = filterPart.Remove(quote, 1);
                filterPart = filterPart.Trim();
            }
            return filterPart;
        }


        public struct SingleFilterInfo
        {
            internal string PropName;
            internal PropertyDescriptor PropDesc;
            internal Object CompareValue;
            internal string OperatorValue;
        }

        //// Enum to hold filter operators. The chars 
        //// are converted to their integer values. 
        //public enum FilterOperator
        //{
        //    EqualTo ="=",
        //    LessThan = "<",
        //    GreaterThan = ">",
        //    Like = "%LIKE",
        //    None = " "
        //}

        public void RemoveFilter()
        {
            Filter = "";
        }

        public ListSortDescriptionCollection SortDescriptions
        {
            get { return null; }
        }

        public bool SupportsAdvancedSorting
        {
            get { return false; }
        }

        public bool SupportsFiltering
        {
            get { return true; }
        }

        private string filterValue;

        public string Filter
        {
            get { return filterValue; }
            set
            {
                if (filterValue == value) return;

                // If the value is not null or empty, but doesn't 
                // match expected format, throw an exception. 
                //if (!string.IsNullOrEmpty(value) &&
                //    !Regex.IsMatch(value,
                //    BuildRegExForFilterFormat(), RegexOptions.Singleline))
                //    throw new ArgumentException("Filter is not in " +
                //          "the format: propName[<>=]'value'.");

                //Turn off list-changed events. 
                RaiseListChangedEvents = false;

                // If the value is null or empty, reset list. 
                if (string.IsNullOrEmpty(value))
                    ResetList();
                else
                {
                    int count = 0;
                    string[] matches = value.Split(new string[] { " AND " },
                        StringSplitOptions.RemoveEmptyEntries);

                    while (count < matches.Length)
                    {
                        string filterPart = matches[count].ToString();

                        // Check to see if the filter was set previously. 
                        // Also, check if current filter is a subset of 
                        // the previous filter. 
                        if (!String.IsNullOrEmpty(filterValue)
                                && !value.Contains(filterValue))
                            ResetList();

                        // Parse and apply the filter. 
                        SingleFilterInfo filterInfo = ParseFilter(filterPart);
                        ApplyFilter(filterInfo);
                        count++;
                    }
                }
                // Set the filter value and turn on list changed events. 
                filterValue = value;
                RaiseListChangedEvents = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        // Build a regular expression to determine if 
        // filter is in correct format. 
        public static string BuildRegExForFilterFormat()
        {
            StringBuilder regex = new StringBuilder();

            // Look for optional literal brackets, 
            // followed by word characters or space. 
            regex.Append(@"\[?[\w\s]+\]?\s?");

            // Add the operators: > < or =. 
            regex.Append(@"[><=]");

            //Add optional space followed by optional quote and 
            // any character followed by the optional quote. 
            regex.Append(@"\s?'?.+'?");

            return regex.ToString();
        }

        private void ResetList()
        {
            this.ClearItems();
            foreach (T t in originalList)
                this.Items.Add(t);
            if (IsSortedCore)
                ApplySortCore(SortPropertyCore, SortDirectionCore);
        }
    }


}