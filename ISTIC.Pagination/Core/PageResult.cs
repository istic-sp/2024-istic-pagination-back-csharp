namespace ISTIC.Pagination.Core;

/// <summary>
/// Classe que representa o resultado de uma consulta paginada.
/// </summary>
public class PageResult<T>
{
    /// <summary>
    /// Propriedade que contém os dados de paginação.
    /// </summary>
    public PaginationData Pagination { get; set; }

    /// <summary>
    /// A coleção de itens paginados.
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Construtor padrão.
    /// </summary>
    public PageResult()
    {
        Pagination = new PaginationData();
        Items = Enumerable.Empty<T>();
    }

    /// <summary>
    /// Construtor que recebe uma coleção de itens.
    /// </summary>
    public PageResult(IEnumerable<T> items) => Items = items;

    /// <summary>
    /// Construtor que recebe os dados de paginação.
    /// </summary>
    public PageResult(PaginationData pagination)
    {
        Pagination = pagination;
        Items = Enumerable.Empty<T>();
    }

    /// <summary>
    /// Construtor que recebe os dados de paginação e a coleção de itens.
    /// </summary>
    public PageResult(PaginationData pagination, IEnumerable<T> items) : this(pagination)
        => Items = items;

    /// <summary>
    /// Construtor que recebe os dados de paginação, a coleção de itens, o total e os dados da requisição (PageRequest).
    /// </summary>
    public PageResult(PageRequest request, long total, IEnumerable<T> items) : this(new PaginationData(request, total), items)
    { }
}
