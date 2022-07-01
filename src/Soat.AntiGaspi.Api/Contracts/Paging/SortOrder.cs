using System.Runtime.Serialization;

namespace Soat.AntiGaspi.Api.Contracts.Paging;

public enum SortOrder 
{
    [EnumMember(Value="asc")]
    Ascending,

    [EnumMember(Value ="desc")]
    Descending
}