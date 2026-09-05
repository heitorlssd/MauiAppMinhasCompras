using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        List<Produto> produtos = await App.Db.GetAll();

        lista.Clear();

        foreach (Produto p in produtos)
        {
            lista.Add(p);
        }
    }

    private void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        Navigation.PushAsync(new NovoProduto());
    }

    private async void txt_search_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        string q = e.NewTextValue;

        lista.Clear();

        List<Produto> produtos;

        if (string.IsNullOrWhiteSpace(q))
        {
            produtos = await App.Db.GetAll();
        }
        else
        {
            produtos = await App.Db.Search(q);
        }

        foreach (Produto p in produtos)
        {
            lista.Add(p);
        }
    }
}