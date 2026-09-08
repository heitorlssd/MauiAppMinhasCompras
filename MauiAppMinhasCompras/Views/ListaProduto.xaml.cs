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

        try
        {
            List<Produto> produtos = await App.Db.GetAll();

            lista.Clear();

            foreach (Produto p in produtos)
            {
                lista.Add(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async void ToolbarItem_Clicked(
    object sender,
    EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(
                new NovoProduto()
            );
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async void txt_search_TextChanged(
    object sender,
    TextChangedEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async void MenuItem_Clicked(
    object sender,
    EventArgs e)
    {
        try
        {
            MenuItem item = sender as MenuItem;

            Produto produto =
                item.BindingContext as Produto;

            bool resposta = await DisplayAlert(
                "Atenção",
                $"Deseja remover {produto.Descricao}?",
                "Sim",
                "Não"
            );

            if (resposta)
            {
                await App.Db.Delete(produto.Id);

                lista.Remove(produto);

                await DisplayAlert(
                    "Sucesso!",
                    "Produto removido.",
                    "OK"
                );
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async void lst_produtos_ItemSelected(
    object sender,
    SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto produto =
                e.SelectedItem as Produto;

            if (produto == null)
                return;

            await Navigation.PushAsync(
                new EditarProduto
                {
                    BindingContext = produto
                }
            );

            lst_produtos.SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }
}