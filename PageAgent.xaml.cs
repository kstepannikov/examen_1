using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PageAgent.xaml
	/// </summary>
	public partial class PageAgent : Page
	{
		bearEntities BD = new bearEntities();

		public PageAgent()
		{
			InitializeComponent();
			dg_buy.ItemsSource = BD.Контрагенты.ToList();
		}

		private void dg_buy_click(object sender, SelectionChangedEventArgs e)
		{
			if (dg_buy.SelectedValue != null)
			{
				int i = (int)dg_buy.SelectedValue;
				Контрагенты item = BD.Контрагенты.Where(a => a.Контрагент_ИД == i).Single();
				new WindowAgent(BD, item).ShowDialog();
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
			var agent = new Контрагенты();
			new WindowAgent(BD, agent, true).ShowDialog();
			FillDataGrid();
		}
		public void FillDataGrid(string search = "")
		{
			dg_buy.ItemsSource = BD.Контрагенты.Where(g => (g.ПолноеНаименование + " " + g.КраткоеНаименование + " " + g.ИНН + " " + g.ОГРН).Contains(search)).ToList();
		}
	}
}
