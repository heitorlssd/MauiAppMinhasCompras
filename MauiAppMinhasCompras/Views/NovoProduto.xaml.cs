using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        Produto p = new Produto
        {
            Descricao = txt_descricao.Text,
            Quantidade = Convert.ToDouble(txt_quantidade.Text),
            Preco = Convert.ToDouble(txt_preco.Text)
        };

        await App.Db.Insert(p);

        await DisplayAlert("Sucesso", "Produto cadastrado!", "OK");

        await Navigation.PopAsync();
    }
}