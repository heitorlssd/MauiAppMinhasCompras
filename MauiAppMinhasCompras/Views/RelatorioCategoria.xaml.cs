using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    ObservableCollection<ResumoCategoria> relatorio = new();

    public RelatorioCategoria()
    {
        InitializeComponent();

        lista_relatorio.ItemsSource = relatorio;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            List<Produto> produtos =
                await App.Db.GetAll();

            relatorio.Clear();

            var grupos = produtos
                .GroupBy(p =>
                    string.IsNullOrWhiteSpace(p.Categoria)
                    ? "Sem categoria"
                    : p.Categoria
                )
                .Select(g => new ResumoCategoria
                {
                    Categoria = g.Key,

                    Total = g.Sum(
                        p => p.Total
                    )
                })
                .OrderBy(
                    r => r.Categoria
                );

            foreach (ResumoCategoria item in grupos)
            {
                relatorio.Add(item);
            }

            double totalGeral =
                produtos.Sum(p => p.Total);

            lbl_total_geral.Text =
                $"{totalGeral:C}";
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

public class ResumoCategoria
{
    public string Categoria { get; set; }
        = string.Empty;

    public double Total { get; set; }

    public string TotalFormatado
    {
        get
        {
            return $"{Total:C}";
        }
    }
}