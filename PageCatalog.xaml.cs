using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PageCatalog.xaml
	/// </summary>
	public partial class PageCatalog : Page
	{
		bearEntities BD = new bearEntities();
		public PageCatalog()
		{
			InitializeComponent();
			FillDataGrid();
		}

		private void dg_buy_click(object sender, SelectionChangedEventArgs e)
		{
			if (dg_buy.SelectedValue != null)
			{
				int i = (int)dg_buy.SelectedValue;
				Товар item = BD.Товар.Where(a => a.Товар_ИД == i).Single();
				new WindowGoods(BD, item).ShowDialog();
				FillDataGrid();
				tb_search.Text = "";

			}
			dg_buy.SelectedValue = null;
			FillDataGrid();
		}

		private void bt_addbuy_Click(object sender, RoutedEventArgs e)
		{
			var goods = new Товар();
			new WindowGoods(BD, goods, true).ShowDialog();
			FillDataGrid();
		}

		private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
		{
			FillDataGrid(tb_search.Text);
		}
		public void FillDataGrid(string search = "")
		{
			dg_buy.ItemsSource = BD.Товар.Where(g => g.Наименование.Contains(search)).ToList();
		}
	}
}
