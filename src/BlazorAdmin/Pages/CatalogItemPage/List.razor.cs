using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorAdmin.Helpers;
using BlazorShared.Interfaces;
using BlazorShared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Pages.CatalogItemPage;

public partial class List : ComponentBase //BlazorAdmin.Helpers.BlazorComponentX
{
    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogItemService CatalogItemService { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogLookupDataService<CatalogBrand> CatalogBrandService { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    public ICatalogLookupDataService<CatalogType> CatalogTypeService { get; set; }

    private List<CatalogItem> catalogItems = new();
    private List<CatalogType> catalogTypes = new();
    private List<CatalogBrand> catalogBrands = new();

    private Edit EditComponent;
    private Delete DeleteComponent;
    private Details DetailsComponent;
    private Create CreateComponent;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            catalogItems = await CatalogItemService.List();
            catalogTypes = await CatalogTypeService.List();
            catalogBrands = await CatalogBrandService.List();

            
            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async void DetailsClick(int id)
    {
        await DetailsComponent.Open(id);
    }

    private async Task CreateClick()
    {
        await CreateComponent.Open();
    }

    private async Task EditClick(int id)
    {
        await EditComponent.Open(id);
    }

    private async Task DeleteClick(int id)
    {
        await DeleteComponent.Open(id);
    }

    private async Task ReloadCatalogItems()
    {
        catalogItems = await CatalogItemService.List();
        StateHasChanged();
    }
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
    }

    public void CallRequestRefresh()
    {
        _refresh.CallRequestRefresh();
    }

    private void DoRefresh()
    {
        StateHasChanged();
    }
}
