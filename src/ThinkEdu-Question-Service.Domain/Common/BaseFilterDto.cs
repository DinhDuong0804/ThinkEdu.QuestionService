namespace ThinkEdu_Question_Service.Domain.Common
{
    public class BaseFilterDto
    {
        private int? _page;
        private int? _rows;
        private bool? _all;
        public int? Page
        {
            get => _page;
            set => _page = value;
        }

        public int? Rows
        {
            get => _rows;
            set => _rows = value;
        }
        public bool? All
        {
            get => _all;
            set => _all = value;
        }
        public bool GetFieldAll()
        {
            if (_all == true)
                return true;
            return _rows is < 0;
        }
    }
}