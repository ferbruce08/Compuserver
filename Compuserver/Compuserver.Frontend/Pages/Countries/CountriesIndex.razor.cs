using Compuserver.Shared.Entites;
using Fantasy.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Compuserver.Frontend.Pages.Countries;

public partial class CountriesIndex
{
    [Inject] private IRepository Repository { get; set; } = null!;

    private List<Country>? Countries { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHppt = await Repository.GetAsync<List<Country>>("api/Countries");
        Countries = responseHppt.Response!;
    }
}