using Compuserver.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Compuserver.Frontend.Shared;

public partial class GenericList<TItem>
{
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public RenderFragment? Loading { get; set; }
    [Parameter] public RenderFragment? NoRecords { get; set; }
    [EditorRequired, Parameter] public RenderFragment Body { get; set; } = null!;
    [EditorRequired, Parameter] public List<TItem> MyList { get; set; } = null!;
}