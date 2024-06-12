using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Models.Backend.Inmueble;
using ShopApp.Services;

namespace ShopApp.ViewModels;

public partial class InmuebleDetailViewModel : ViewModelGlobal, IQueryAttributable
{

    [ObservableProperty]
    private string imagenSource;
    
    [ObservableProperty]
    private InmuebleResponse inmueble;

    private readonly InmuebleService _inmuebleService;

    private readonly INavegacionService _navegacionService;

    public InmuebleDetailViewModel(
        InmuebleService inmuebleService, 
        INavegacionService navegacionService)
    {
        _inmuebleService = inmuebleService;
        _navegacionService = navegacionService;
    }
    public async Task LoadDataAsync(int inmuebleId)
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            Inmueble = await _inmuebleService.GetInmubleById(inmuebleId);
            ImagenSource = Inmueble.IsBookmarkEnabled ? "bookmark_fill_icon" : "bookmark_empty_icon";
        }
        catch(Exception e)
        {
            await Application.Current.MainPage.DisplayAlert("Error", e.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = int.Parse(query["id"].ToString());
        await LoadDataAsync(id);
    }
    [RelayCommand]
    async Task GetBackEvent()
    {
        await _navegacionService.GoToAsync("..");
    }
    [RelayCommand]
    async Task SaveBookmark()
    {
        var bookmark = new BookmarkRequest
        {
            InmuebleId = Inmueble.Id,
            UsuarioId = Preferences.Get("userid", string.Empty)
        };

        await _inmuebleService.SaveBookmark(bookmark);
        await LoadDataAsync(Inmueble.Id);
}
    [RelayCommand]
    async Task CallOwner()
    {
        var confirmarLlamada = Application.Current.MainPage.DisplayAlert(
                "Marca este numero telefonico",
                $"Desea llamar a este numero: {Inmueble.Telefono}",
                "Si",
                "No"
            );
        if (await confirmarLlamada)
        {
            try
            {
                PhoneDialer.Open(Inmueble.Telefono);
            }
            catch (ArgumentNullException)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                    "No se puede realizar esta llamada",
                    "El numero telefonico no es valido",
                    "Ok");
            }
            catch (FeatureNotSupportedException)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                    "No se puede realizar esta llamada",
                    "El dispositivo no soporta llamadas telefonicas",
                    "Ok");
            }
            catch (Exception)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                    "No se puede realizar esta llamada",
                    "Errores en la marcacion del numero",
                    "Ok");
            }
        }
    }
    [RelayCommand]
    async Task TextMessageOwner()
    {

        var message = new SmsMessage("Hola, por favor enviame info sobre la vivienda", Inmueble.Telefono);
        var confirmarMensajeTexto = Application.Current.MainPage.DisplayAlert(
                "Envia un mensaje de texto",
                $"Desea enviar un mensaje de texto a este numero: {Inmueble.Telefono}",
                "Si",
                "No" );

        if (await confirmarMensajeTexto)
        {
            try
            {
               await Sms.ComposeAsync(message);
            }
            catch (ArgumentNullException)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                    "No se puede enviar este sms",
                    "El numero telefonico no es valido",
                    "Ok");
            }
            catch (FeatureNotSupportedException)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                   "No se puede enviar este sms",
                    "El dispositivo no soporta envio de sms",
                    "Ok");
            }
            catch (Exception)
            {
                await Application.Current.MainPage
                    .DisplayAlert(
                   "No se puede enviar este sms",
                    "Errores en el envio del sms",
                    "Ok");
            }
        }
    }
}

