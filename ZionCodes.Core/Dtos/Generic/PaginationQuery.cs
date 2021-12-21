namespace ZionCodes.Core.Dtos.Generic
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            this.PageNumber = 1;
            this.PageSize = 5;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
