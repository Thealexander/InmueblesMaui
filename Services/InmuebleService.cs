using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopApp.Models.Backend.Inmueble;
using ShopApp.Models.Config;
using System.Net.Http.Headers;
using System.Text;

namespace ShopApp.Services;
public class InmuebleService
{
    private readonly HttpClient client;

    private Settings settings;

    public InmuebleService(HttpClient client, IConfiguration configuration)
    {
        this.client = client;
        this.settings = configuration.GetRequiredSection(nameof(Settings)).Get<Settings>();
    }
    public async Task<List<CategoryResponse>> GetCategories()
    {
        var uri = $"{settings.UrlBase}/api/category";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<List<CategoryResponse>>(resultado);
    }
    public async Task<List<InmuebleResponse>> GetInmueblesByCategory(int categoryId)
    {
        var uri = $"{settings.UrlBase}/api/inmueble/category/{categoryId}";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);
        return JsonConvert.DeserializeObject<List<InmuebleResponse>>(resultado);
    }
        public async Task<List<InmuebleResponse>> GetInmueblesFavoritos()
    {
        var uri = $"{settings.UrlBase}/api/inmueble/trending";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);
        return JsonConvert.DeserializeObject<List<InmuebleResponse>>(resultado);
    }

    public async Task<InmuebleResponse> GetInmubleById(int inmuebleId)
    {
        var uri = $"{settings.UrlBase}/api/inmueble/{inmuebleId}";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);
        return JsonConvert.DeserializeObject<InmuebleResponse>(resultado);
    }

    public async Task<bool> SaveBookmark(BookmarkRequest bookmark)
    {
        var url = $"{settings.UrlBase}/api/bookmark";
        var json = JsonConvert.SerializeObject(bookmark);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var response =  await client.PostAsync(url, content);
        if (!response.IsSuccessStatusCode) return false;

        return true;
    }
    public async Task<List<InmuebleResponse>> GetBookmarks()
    {
        var uri = $"{settings.UrlBase}/api/inmueble/bookmark";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<List<InmuebleResponse>>(resultado);
    }
    public async Task<List<InmuebleResponse>> GetBusquedaInmuebles(string inmuebleValue)
    {
        var uri = $"{settings.UrlBase}/api/inmueble/search/{inmuebleValue}";
        client.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("bearer", Preferences.Get("accesstoken", string.Empty));

        var resultado = await client.GetStringAsync(uri);

        return JsonConvert.DeserializeObject<List<InmuebleResponse>>(resultado);
    }
}

