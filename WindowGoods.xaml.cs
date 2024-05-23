using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowGoods.xaml
	/// </summary>
	public partial class WindowGoods : Window
	{
		bearEntities BD = new bearEntities();
		Товар товар = new Товар();
		bool New;
		public WindowGoods(bearEntities BD, Товар goods, bool New = false)
		{
			this.BD = BD;
			this.товар = goods;
			this.DataContext = goods;
			this.New = New;
			InitializeComponent();
			//this.BD = bd;
			cmb_category.ItemsSource = BD.Категории.ToList();
			cmb_edizm.ItemsSource = BD.ЕдиницыИзмерения.ToList();
			cmb_postav.ItemsSource = BD.Контрагенты.ToList().Prepend(new Контрагенты() { ПолноеНаименование = "Поставщик не выбран" });
			if (goods.Контрагенты == null)
			{
				cmb_postav.SelectedIndex = 0;
			}
			if (New)
			{
				cmb_postav.SelectedIndex = 0;
				tb_ost.Text = "";
			}
		}


		private void but_save_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(tb_name.Text))
			{
				MessageBox.Show("Поле Наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(cmb_category.Text))
			{
				MessageBox.Show("Поле Категория не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;

			}
			if (string.IsNullOrEmpty(cmb_edizm.Text))
			{
				MessageBox.Show("Поле Ед. Измер не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_ost.Text))
			{
				MessageBox.Show("Поле Остаток не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (cmb_postav.SelectedIndex == 0)
			{
				товар.Контрагенты = null;
			}
			if (New == true)
			{
				BD.Товар.Add(товар);
			}
			BD.SaveChanges();
			this.Close();
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void tb_ost_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
			   && (!tb_ost.Text.Contains(".")
			   && tb_ost.Text.Length != 0)))
			{
				e.Handled = true;
			}
		}
	}
}
