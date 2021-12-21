using System;

namespace ZionCodes.Core.Models
{
    public interface IAuditable
    {
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset UpdatedDate { get; set; }
        int CreatedBy { get; set; }
        int UpdatedBy { get; set; }
    }
}
