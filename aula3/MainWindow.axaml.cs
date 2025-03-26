using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MeuProjetoAvalonia
{
	public partial class MainWindow : Window
	{
        int qt = 0;
    	public MainWindow()
    	{
        	// Inicializa a interface a partir do XAML
        	InitializeComponent();
    	}

    	// Evento chamado quando o botão é clicado
    	private void BtnClique_Click(object sender, RoutedEventArgs e)
    	{
        	// Recupera o valor digitado no TextBox pelo nome definido no XAML (txtInput)
        	var nome = this.FindControl<TextBox>("txtInput").Text;
            qt ++;

        	// Exibe uma mensagem alterando o texto do próprio TextBox como exemplo
        	if (!string.IsNullOrEmpty(nome))
        	{
                if (qt == 1)
            	this.FindControl<TextBox>("txtInput").Text = $"Olá, {nome}!";
        	}
        	else
        	{
            	// Caso o usuário não tenha digitado nada, exibe uma mensagem padrão
            	this.FindControl<TextBox>("txtInput").Text = "Olá, visitante!";
        	}
    	}
	}
}
