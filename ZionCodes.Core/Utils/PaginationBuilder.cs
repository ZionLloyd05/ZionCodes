using System;
using System.Collections.Generic;
using System.Linq;
using ZionCodes.Core.Dtos.Generic;

namespace ZionCodes.Core.Utils
{
    public class PaginationBuilder
    {
        public static PagedResponse<ICollection<T>> CreatePaginatedResponse<T>(PaginationFilter pagination, ICollection<T> responseFromCaller)
        {
            var totalItems = responseFromCaller.Count();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pagination.PageSize);

            var hasNextPage = pagination.PageNumber < totalPages;

            var hasPreviousPage = pagination.PageNumber > 1;

            var nextPage = pagination.PageNumber >= 1 && hasNextPage ?
                        pagination.PageNumber + 1 : -1;

            var previousPage = pagination.PageNumber - 1 >= 1 && hasPreviousPage ?
                pagination.PageNumber - 1 : -1;

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            responseFromCaller = responseFromCaller.Skip(skip).Take(pagination.PageSize).ToList();

            return new PagedResponse<ICollection<T>>
            {
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                NextPage = responseFromCaller.Any() ? nextPage : -1,
                totalData = totalItems,
                PreviousPage = previousPage,
                Data = responseFromCaller
            };
        }
    }
}
