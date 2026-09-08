using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            Produto? produto_anexado =
                BindingContext as Produto;

            if (produto_anexado == null)
            {
                await DisplayAlertAsync(
                    "Ops",
                    "Não foi possível identificar o produto.",
                    "OK"
                );

                return;
            }

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
                Id = produto_anexado.Id,

                Descricao =
                    txt_descricao.Text,

                Quantidade =
                    Convert.ToDouble(txt_quantidade.Text),

                Preco =
                    Convert.ToDouble(txt_preco.Text),

                Categoria =
                    pck_categoria.SelectedItem.ToString()!
            };

            await App.Db.Update(p);

            await DisplayAlertAsync(
                "Sucesso!",
                "Registro atualizado.",
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