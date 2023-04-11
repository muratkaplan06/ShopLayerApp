namespace ShopApp.Core.QueryParameters
{
    public class QueryParametersModel
    {
        const int _maxSize = 20;
        private int _size = 4;
        public int Page { get; set; } = 1;

        public int Size
        {
            get { return _size; }
            set { _size = Math.Min(_maxSize, value); }
        }
    }
}
