using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // Coleção mostrada na ListView
    ObservableCollection<Produto> lista = new();

    // Lista completa dos produtos carregados do banco
    List<Produto> todosProdutos = new();

    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;

        // Inicia mostrando todas as categorias
        pck_filtro_categoria.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await CarregarProdutos();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async Task CarregarProdutos()
    {
        todosProdutos = await App.Db.GetAll();

        AplicarFiltros();
    }

    private void AplicarFiltros()
    {
        IEnumerable<Produto> produtosFiltrados =
            todosProdutos;

        // -------------------------
        // FILTRO POR DESCRIÇÃO
        // -------------------------

        string texto =
            txt_search.Text?.Trim() ?? "";

        if (!string.IsNullOrWhiteSpace(texto))
        {
            produtosFiltrados =
                produtosFiltrados.Where(
                    p =>
                        p.Descricao != null &&
                        p.Descricao.Contains(
                            texto,
                            StringComparison.OrdinalIgnoreCase
                        )
                );
        }

        // -------------------------
        // FILTRO POR CATEGORIA
        // -------------------------

        string categoria =
            pck_filtro_categoria.SelectedItem?.ToString()
            ?? "Todas";

        if (categoria != "Todas")
        {
            produtosFiltrados =
                produtosFiltrados.Where(
                    p =>
                        p.Categoria != null &&
                        p.Categoria.Equals(
                            categoria,
                            StringComparison.OrdinalIgnoreCase
                        )
                );
        }

        // -------------------------
        // ATUALIZA A LISTVIEW
        // -------------------------

        lista.Clear();

        foreach (Produto p in produtosFiltrados)
        {
            lista.Add(p);
        }
    }

    // -------------------------
    // ADICIONAR
    // -------------------------

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
            await DisplayAlertAsync(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    // -------------------------
    // PESQUISA POR TEXTO
    // -------------------------

    private void txt_search_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        AplicarFiltros();
    }

    // -------------------------
    // FILTRO POR CATEGORIA
    // -------------------------

    private void pck_filtro_categoria_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        AplicarFiltros();
    }

    // -------------------------
    // EDITAR
    // -------------------------

    private async void lst_produtos_ItemSelected(
        object sender,
        SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto? produto =
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
            await DisplayAlertAsync(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    // -------------------------
    // REMOVER
    // -------------------------

    private async void MenuItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            MenuItem? item =
                sender as MenuItem;

            Produto? produto =
                item?.BindingContext as Produto;

            if (produto == null)
                return;

            bool resposta =
                await DisplayAlertAsync(
                    "Atenção",
                    $"Deseja remover {produto.Descricao}?",
                    "Sim",
                    "Não"
                );

            if (resposta)
            {
                await App.Db.Delete(produto.Id);

                // Remove também da lista completa
                todosProdutos.RemoveAll(
                    p => p.Id == produto.Id
                );

                // Atualiza a tela respeitando os filtros
                AplicarFiltros();

                await DisplayAlertAsync(
                    "Sucesso!",
                    "Produto removido.",
                    "OK"
                );
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }

    private async void Relatorio_Clicked(
    object sender,
    EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(
                new RelatorioCategoria()
            );
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Ops",
                ex.Message,
                "OK"
            );
        }
    }
}