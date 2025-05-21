using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace MeuProjetoAvalonia
{
	public partial class MainWindow : Window
	{
        int qt = 0;
		string typeConversion = "nothing";
    	public MainWindow()
    	{
        	// Inicializa a interface a partir do XAML
        	InitializeComponent();
			allInvisible();
    	}

    	private void converter(object sender, RoutedEventArgs e){
			float entrada = float.Parse(input.Text.ToString());
			float resultado = 0;
        	switch (typeConversion){
				case "°C > °F":
					resultado = entrada*1.8f+32;
					break;
				case "°C > °K":
					resultado = entrada + 273.15f;
					break;
				case "°F > °C":
					resultado = (entrada-32)/1.8f;
					break;
				case "°F > °K":
					resultado = entrada - 273.15f;
					break;
				case "m > ft":
					resultado = entrada * 3.28084f;
					break;
				case "ft > m":
					resultado = entrada / 3.28084f;
					break;
				case "km > mi":
					resultado = entrada / 1.60934f;
					break;
				case "mi > km":
					resultado = entrada * 1.60934f;
					break;
				case "kg > lb":
					resultado = entrada * 2.20462f;
					break;
				case "lb > kg":
					resultado = entrada / 2.20462f;
					break;
				case "g > oz":
					resultado = entrada / 28.3495f;
					break;
				case "oz > g":
					resultado = entrada * 28.3495f;
					break;
				case "L > gal":
					resultado = entrada / 3.78541f;
					break;
				case "gal > L":
					resultado = entrada * 3.78541f;
					break;
				case "ml > fl oz":
					resultado = entrada / 29.5735f;
					break;
				case "fl oz > ml":
					resultado = entrada * 29.5735f;
					break;
				default:
					break;
			}
			output.Text = entrada.ToString()+typeConversion.Split(" > ")[0]+" > "+resultado.ToString()+typeConversion.Split(">")[1];
    	}

		private void allInvisible(){			
			Temperatura.IsVisible = false;
			Massa.IsVisible = false;
			Comprimento.IsVisible = false;
			Volume.IsVisible = false;
    	}

		private void drop_changed(object sender, SelectionChangedEventArgs e){
			var comboBox = sender as ComboBox;
			if (comboBox.SelectedItem is ComboBoxItem item)
			{
				switch (item.Content){
					case "Temperatura":
						allInvisible();
						Temperatura.IsVisible = true;
						break;
					case "Massa":
						allInvisible();
						Massa.IsVisible = true;
						break;
					case "Volume":
						allInvisible();
						Volume.IsVisible = true;
						break;
					case "Comprimento":
						allInvisible();
						Comprimento.IsVisible = true;
						break;
					default:
						break;
				}
			}
		}
	
		private void mass_changed(object sender, SelectionChangedEventArgs e){
			var vari = sender as ListBox;
            if (vari.SelectedItem is ListBoxItem item){
                typeConversion = item.Content.ToString();
			}
			output.Text = (string) (typeConversion);
        }

		private void comp_changed(object sender, SelectionChangedEventArgs e){
            var vari = sender as ListBox;
            if (vari.SelectedItem is ListBoxItem item){
                typeConversion = item.Content.ToString();
			}
			output.Text = (string) (typeConversion);
        }

		private void volu_changed(object sender, SelectionChangedEventArgs e){
            var vari = sender as ListBox;
            if (vari.SelectedItem is ListBoxItem item){
                typeConversion = item.Content.ToString();
			}
			output.Text = (string) (typeConversion);
        }

		private void temp_changed(object sender, SelectionChangedEventArgs e){
            var vari = sender as ListBox;
            if (vari.SelectedItem is ListBoxItem item){
                typeConversion = item.Content.ToString();
			}
			output.Text = (string) (typeConversion);
        }
	}
}
