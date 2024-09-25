# ISTIC.Pagination

Para realizar páginação na sua aplicação é necessário utilizar as classes ***PageRequest*** e ***PageResult***.

Na controller, defina como o seguinte exemplo:
```csharp
[HttpGet]
public async Task<ResponseOf<PageResult<User>>> ListAllUsers([FromQuery] PageRequest request)
{
    // Código para retornar usuários páginados
}
```

Explicação: a classe ResponseOf receberá como tipo um resultado paginado ***PageResult***, onde terá uma lista de Usuários *User*

O usuário deverá especificar na query os parametros **Page**(página atual) e **PageSize**(quantidade de items por página) solicitados no objeto citado acima, assim como, a propriedade ***SortDirection***, indicando a direção de ordenação (ascendente ou descendente), por padrão retornará uma ordenação ascendente.

Para paginar o resultado do seu retorno basta utilizar o método de extensão ***Paginate()*** ou ***PaginateBy()*** para realizar uma ordenação por alguma propriedade específica.
```csharp
public async Task<ResponseOf<PageResult<User>>> ListAllUsers(PageRequest request)
{
    var result = await db.Users
        .Paginate(request) // Paginando sem ordenação
        .ToListAsync(cancellationToken);

    var result_ordered_by_name = await db.Users
        .PaginateBy(request, x => x.Name) // Paginando com ordenação
        .ToListAsync(cancellationToken);

    return new PageResult<User>(result);
}
```

Caso for necessário alterar o tamanho máximo de itens por página será necessário sobrescrever a propriedade ***MaxPageSize*** da classe abstrata ***PageRequest***, exemplo:

```csharp
public class MyPageWithPagination : PageRequest
{
    public override int MaxPageSize { get; set; } = 100;
}
```