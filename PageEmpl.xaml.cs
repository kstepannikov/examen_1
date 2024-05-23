using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PageAgent.xaml
	/// </summary>
	public partial class PageEmpl : Page
	{
		bearEntities BD = new bearEntities();

		public PageEmpl()
		{
			InitializeComponent();
			FillDataGrid();
		}

		private void dg_buy_click(object sender, SelectionChangedEventArgs e)
		{
			if (dg_buy.SelectedValue != null)
			{
				int i = (int)dg_buy.SelectedValue;
				Сотрудники item = BD.Сотрудники.Where(a => a.Сотрудники_ИД == i).Single();
				new WindowEmpl(BD, item).ShowDialog();
				tb_search.Text = "";

			}
			dg_buy.SelectedValue = null;
			FillDataGrid();
		}

		private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
		{
			FillDataGrid(tb_search.Text);
		}

		private void bt_addbuy_Click(object sender, RoutedEventArgs e)
		{
			var сотрудник = new Сотрудники();
			new WindowEmpl(BD, сотрудник, true).ShowDialog();
			FillDataGrid();
		}
		public void FillDataGrid(string search = "")
		{
			dg_buy.ItemsSource = BD.Сотрудники.Where(g => (g.Фамилия + " " + g.Имя + " " + g.Отчество + " " + g.Логин).Contains(search)).ToList();
		}
	}
}
