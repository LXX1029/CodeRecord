namespace Services.EF
{
    public class ClickCountReportEntity
    {
        private int _ChildrenId;
        private string _ChildrenName;
        private int _ClickCount = 0;
        private int _ParentId;

        private string _ParentName;

        public int ChildrenId
        {
            get { return _ChildrenId; }
            set { _ChildrenId = value; }
        }

        public string ChildrenName
        {
            get { return _ChildrenName; }
            set { _ChildrenName = value; }
        }

        public int ClickCount
        {
            get { return _ClickCount; }
            set { _ClickCount = value; }
        }

        public int ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        public string ParentName
        {
            get { return _ParentName; }
            set { _ParentName = value; }
        }
    }
}