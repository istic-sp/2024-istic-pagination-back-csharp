using ISTIC.Pagination.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace ISTIC.Pagination.Core;

/// <summary>
/// Classe base para requisições de paginação.
/// </summary>
public abstract class PageRequest
{
    /// <summary>
    /// Tamanho máximo da página.
    /// </summary>
    [JsonIgnore]
    [BindNever]
    public virtual int MaxPageSize { get; set; } = 50;

    private int _pageSize = 30;

    /// <summary>
    /// Página atual.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Tamanho da página.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// Direção da ordenação.
    /// </summary>
    public SortDirection SortDirection { get; set; }

    /// <summary>
    /// Propriedade de ordenação dinâmica.
    /// </summary>
    public string SortingProperty { get; set; }
}
