using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PageDeliv.xaml
	/// </summary>
	public partial class PageDeliv : Page
	{
		bearEntities BD = new bearEntities();
		public PageDeliv()
		{
			InitializeComponent();
			FillDataGrid();
		}

		private void bt_addbuy_Click(object sender, RoutedEventArgs e)
		{
			var item = new Поступления();
			new WindowDeliv(BD, item, true).ShowDialog();
			FillDataGrid();
		}

		private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
		{
			FillDataGrid(tb_search.Text);
		}

		private void dg_buy_click(object sender, SelectionChangedEventArgs e)
		{
			if (dg_buy.SelectedValue != null)
			{
				int i = (int)dg_buy.SelectedValue;
				Поступления item = BD.Поступления.Where(a => a.Поступления_ИД == i).Single();
				new WindowDeliv(BD, item).ShowDialog();
				tb_search.Text = "";

			}
			dg_buy.SelectedValue = null;
			FillDataGrid();
		}
		public void FillDataGrid(string search = "")
		{
			dg_buy.ItemsSource = BD.Поступления.Where(_ => _.НомерПоступления.Contains(search)).ToList();
		}
	}
}
