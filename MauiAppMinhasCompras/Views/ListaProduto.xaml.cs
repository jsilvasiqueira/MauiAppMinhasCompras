using System.Collections.ObjectModel;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> produtos;

    public ListaProduto()
    {
        InitializeComponent();

        produtos = new ObservableCollection<Produto>();

        lst_produtos.ItemsSource = produtos;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        try
        {
            var lista = await App.Db.GetAll();

            produtos.Clear();

            foreach (var item in lista)
            {
                produtos.Add(item);
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

    private async void NovoProduto_Clicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(
            new NovoProduto()
        );
    }

    private async void txt_busca_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        try
        {
            string busca = e.NewTextValue;

            List<Produto> resultado;

            if (string.IsNullOrWhiteSpace(busca))
            {
                resultado = await App.Db.GetAll();
            }
            else
            {
                resultado = await App.Db.Search(busca);
            }

            produtos.Clear();

            foreach (var item in resultado)
            {
                produtos.Add(item);
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

    private async void Excluir_Clicked(
        object sender,
        EventArgs e)
    {
        try
        {
            if (sender is Button botao &&
                botao.CommandParameter is Produto produto)
            {
                bool resposta =
                    await DisplayAlertAsync(
                        "Excluir",
                        $"Deseja excluir {produto.Descricao}?",
                        "Sim",
                        "Não"
                    );

                if (resposta)
                {
                    await App.Db.Delete(produto.Id);

                    produtos.Remove(produto);

                    await DisplayAlertAsync(
                        "Sucesso",
                        "Produto excluído.",
                        "OK"
                    );
                }
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

    private async void lst_produtos_ItemSelected(
        object sender,
        SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not Produto produto)
            return;

        await Navigation.PushAsync(
            new EditarProduto
            {
                BindingContext = produto
            }
        );

        lst_produtos.SelectedItem = null;
    }
}