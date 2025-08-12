namespace ThinkEdu_Question_Service.Domain.Common
{
    public sealed class ListResponse<T>(

    IEnumerable<T> data,
    IEnumerable<HeaderTableResponse>? listHeader,
    int count = 0)
    {
        public IEnumerable<HeaderTableResponse>? Headers { get; set; } = listHeader;
        public IEnumerable<T> Results { get; set; } = data;
        public int Count { get; set; } = count;

        public ListResponse() : this(new List<T>
        {
            Capacity = 0
        }, new List<HeaderTableResponse>())
        {
        }
    }
}