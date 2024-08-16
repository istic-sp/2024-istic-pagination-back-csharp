using ISTIC.Pagination.Enums;

namespace ISTIC.Pagination.Core;

public abstract class PageRequest
{
    public virtual int MaxPageSize { get; set; } = 50;
    private int _pageSize = 30;

    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public SortDirection SortDirection { get; set; }
}
