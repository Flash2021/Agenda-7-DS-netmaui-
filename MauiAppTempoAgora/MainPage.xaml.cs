using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_cidade.Text))
            {
                // Verifica a conexão com a internet ANTES de chamar o serviço
                if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Sem Conexão", "Você está sem conexão com a internet. Verifique sua conexão e tente novamente.", "OK");
                    return; // Interrompe a execução se não houver internet
                }

                try
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsão = $"Latitude: {t.lat} \n" +
                                                 $"Longitude: {t.lon} \n" +
                                                 $"Nascer do Sol: {t.sunrise} \n" +
                                                 $"Por do Sol: {t.sunset} \n" +
                                                 $"Temp Máx: {t.temp_max} \n" +
                                                 $"Temp Min: {t.temp_min} \n" +
                                                 $"Clima: {t.description} \n" +
                                                 $"Velocidade do Vento: {t.speed} \n" +
                                                 $"Visibilidade: {t.visibility} \n";

                        lbl_res.Text = dados_previsão;
                    }
                    else
                    {
                        lbl_res.Text = "Cidade não encontrada. Verifique o nome digitado.";
                        await DisplayAlert("Nome incorreto", "Cidade não encontrada. verifique o nome digitado.", "OK");
                        return; // Interrompe a execução se não houver internet
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ops", ex.Message, "Ok");
                }
            }
            else
            {
                lbl_res.Text = "Preencha a cidade.";
            }
        }
    }
}