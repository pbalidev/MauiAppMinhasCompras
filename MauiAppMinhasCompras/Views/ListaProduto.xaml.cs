using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    private List<Produto> _todosProdutos = new List<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        _todosProdutos = await App.Db.GetAll();
        AtualizarLista(_todosProdutos);
    }

    private void AtualizarLista(List<Produto> produtos)
    {
        lista.Clear();
        foreach (var produto in produtos)
        {
            lista.Add(produto);
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string termoBusca = e.NewTextValue;
        if (string.IsNullOrWhiteSpace(termoBusca))
        {
            AtualizarLista(_todosProdutos);
        }
        else
        {
            var produtosFiltrados = _todosProdutos
                .Where(p => p.Descricao.Contains(termoBusca, StringComparison.OrdinalIgnoreCase))
                .ToList();
            AtualizarLista(produtosFiltrados);
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlert("Opa", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total � {soma:C}";
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            // Obt�m o item de menu que foi clicado
            var menuItem = sender as MenuItem;

            // Obt�m o produto associado ao item de menu clicado
            var produto = menuItem?.BindingContext as Produto;

            if (produto == null)
            {
                await DisplayAlert("Erro", "Produto n�o encontrado.", "OK");
                return;
            }

            // Confirma��o do usu�rio antes de remover
            bool confirmacao = await DisplayAlert("Confirmar Remo��o", $"Deseja remover o produto '{produto.Descricao}'?", "Sim", "N�o");

            if (confirmacao)
            {
                // Remove o produto do banco de dados
                await App.Db.Delete(produto.Id);

                // Remove o produto da lista ObservableCollection
                lista.Remove(produto);

                // Atualiza a lista tempor�ria (_todosProdutos) para manter a consist�ncia
                _todosProdutos.Remove(produto);

                await DisplayAlert("Sucesso", "Produto removido com sucesso.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao remover o produto: {ex.Message}", "OK");
        }
    }

}