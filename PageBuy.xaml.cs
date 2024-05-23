using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PageBuy.xaml
	/// </summary>
	public partial class PageBuy : Page
	{
		bearEntities BD = new bearEntities();
		public PageBuy()
		{
			InitializeComponent();
			FillDataGrid();
			if (LoginUtility.сотрудник.Должность.ЭтоВодитель == true)
			{
				bt_add.IsEnabled = false;
			}
		}

		private void bt_addbuy_Click(object sender, RoutedEventArgs e)
		{
			var item = new ЗаявкаСнабжение();
			new WindowBuy(BD, item, true).ShowDialog();
			FillDataGrid();
		}

		private void dg_buy_click(object sender, SelectionChangedEventArgs e)
		{
			if (dg_buy.SelectedValue != null)
			{
				int i = (int)dg_buy.SelectedValue;
				ЗаявкаСнабжение item = BD.ЗаявкаСнабжение.Where(a => a.ЗаявкаСнабж_ИД == i).Single();
				new WindowBuy(BD, item).ShowDialog();
				tb_search.Text = "";

			}
			dg_buy.SelectedValue = null;
			FillDataGrid();
		}

		private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
		{
			FillDataGrid(tb_search.Text);
		}
		public void FillDataGrid(string search = "")
		{
			dg_buy.ItemsSource = BD.ЗаявкаСнабжение.Where(_ => _.НомерЗаявки.Contains(search)).ToList();
		}
	}
}
