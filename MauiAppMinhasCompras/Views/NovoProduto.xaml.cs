using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Informe a descrição do produto.",
                    "OK"
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(txt_quantidade.Text))
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Informe a quantidade.",
                    "OK"
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(txt_preco.Text))
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Informe o preço.",
                    "OK"
                );

                return;
            }

            if (pck_categoria.SelectedItem == null)
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Selecione uma categoria.",
                    "OK"
                );

                return;
            }

            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,

                Quantidade =
                    Convert.ToDouble(txt_quantidade.Text),

                Preco =
                    Convert.ToDouble(txt_preco.Text),

                Categoria =
                    pck_categoria.SelectedItem.ToString()!
            };

            await App.Db.Insert(p);

            await DisplayAlertAsync(
                "Sucesso!",
                "Produto cadastrado.",
                "OK"
            );

            await Navigation.PopAsync();
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