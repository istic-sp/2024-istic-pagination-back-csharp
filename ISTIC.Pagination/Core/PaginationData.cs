namespace ISTIC.Pagination.Core;

/// <summary>
/// Propriedades de paginação.
/// </summary>
public class PaginationData
{
    /// <summary>
    /// Página atual.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de itens.
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// Total de páginas.
    /// </summary>
    public int PageCount => PageSize == 0 ? 0 : (int)Math.Ceiling((double)Total / PageSize);

    /// <summary>
    /// É a primeira página?
    /// </summary>
    public bool IsFirstPage => Page <= 1;

    /// <summary>
    /// É a última página?
    /// </summary>
    public bool IsLastPage => Page >= PageCount;

    /// <summary>
    /// 
    /// </summary>
    public PaginationData()
    {}

    /// <summary>
    /// Construtor que recebe os dados da requisição (PageRequest) e o total de itens.
    /// </summary>
    public PaginationData(PageRequest request, long total)
    {
        Page = request.Page;
        PageSize = request.PageSize;
        Total = total;
    }
}
