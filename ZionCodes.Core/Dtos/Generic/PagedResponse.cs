namespace ZionCodes.Core.Dtos.Generic
{
    public class PagedResponse<T>
    {
        public PagedResponse()
        {

        }

        public PagedResponse(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
        public int? PageNumber { get; set; }
        public int totalData { get; set; }
        public int? PageSize { get; set; }
        public int NextPage { get; set; }
        public int PreviousPage { get; set; }
    }
}
