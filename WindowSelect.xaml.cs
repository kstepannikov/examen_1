using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowSelect.xaml
	/// </summary>
	public partial class WindowSelect : Window
	{
		bearEntities BD;
		СписокПозиций чек = null;
		bool CostIsActive;

		public WindowSelect(bearEntities bd, СписокПозиций вход_чек = null, bool CostIsActive = false)
		{
			InitializeComponent();
			this.BD = bd;
			чек = вход_чек;
			dg_catalog.ItemsSource = BD.Товар.ToList();
			this.CostIsActive = CostIsActive;
			if (!CostIsActive)
			{
				tb_cost.Visibility = Visibility.Collapsed;
				tbl_cost.Visibility = Visibility.Collapsed;
			}
			this.DataContext = чек;
			if (чек != null)
			{
				dg_catalog.SelectedValue = чек.Товар;
			}
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void but_save_Click(object sender, RoutedEventArgs e)
		{
			if (dg_catalog.SelectedValue != null & (tb_cost.Text != "" | !CostIsActive) & tb_count.Text != "")
			{
				чек = new СписокПозиций();
				var i = (Товар)dg_catalog.SelectedValue;
				чек.Товар = i;
				if (CostIsActive)
				{
					чек.Цена_Ед = decimal.Parse(tb_cost.Text.Replace('.', ','));
				}
				чек.Количество = float.Parse(tb_count.Text.Replace('.', ','));


				this.Close();
			}
			else
			{
				MessageBox.Show("Не все поля заполнены", "Ошибка");
			}
		}
		public СписокПозиций GetGoods()
		{
			this.ShowDialog();
			return чек;

		}

		private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
			   && (!(sender as TextBox).Text.Contains(".")
			   && (sender as TextBox).Text.Length != 0)))
			{
				e.Handled = true;
			}
		}
	}
}
