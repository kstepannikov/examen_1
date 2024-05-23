using System;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			fr_agent.Content = new PagePeople();
			fr_buy.Content = new PageBuy();
			fr_catalog.Content = new PageCatalog();
			fr_deliv.Content = new PageDeliv();
			tb_auth.Text += Environment.NewLine + LoginUtility.GetInfo();

			if (LoginUtility.сотрудник.Должность.ЭтоМенеджер == true)
			{
				tabitem_agent.Visibility = Visibility.Collapsed;
				TabCtrl.SelectedItem = tabitem_deliv;
			}
			if (LoginUtility.сотрудник.Должность.ЭтоСнабженец == true)
			{
				tabitem_agent.Visibility = Visibility.Collapsed;
				tabitem_buy.Visibility = Visibility.Collapsed;
				TabCtrl.SelectedItem = tabitem_deliv;
			}
			if (LoginUtility.сотрудник.Должность.ЭтоВодитель == true)
			{
				tabitem_agent.Visibility = Visibility.Collapsed;
				tabitem_catalog.Visibility = Visibility.Collapsed;
				tabitem_deliv.Visibility = Visibility.Collapsed;
				TabCtrl.SelectedItem = tabitem_buy;


			}
			if (LoginUtility.сотрудник.Должность.ЭтоАдмин == true)
			{
				tabitem_agent.Visibility = Visibility.Visible;
				TabCtrl.SelectedItem = tabitem_agent;

			}
		}

		private void bt_exit_Click(object sender, RoutedEventArgs e)
		{
			LoginUtility.сотрудник = null;
			var auth = new WindowAuth();
			auth.Show();
			this.Close();
		}

		private void TabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			fr_agent.Content = new PagePeople();
			fr_buy.Content = new PageBuy();
			fr_catalog.Content = new PageCatalog();
			fr_deliv.Content = new PageDeliv();
		}
	}
}
