using System;
using System.Windows;
using System.Windows.Input;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowAgent.xaml
	/// </summary>
	public partial class WindowAgent : Window
	{
		bearEntities BD = new bearEntities();
		Контрагенты контрагент = new Контрагенты();
		bool New;
		public WindowAgent(bearEntities bD, Контрагенты контраг, bool isnew = false)
		{
			InitializeComponent();
			BD = bD;
			контрагент = контраг;
			New = isnew;
			this.DataContext = контраг;
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void but_save_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(tb_fullname.Text))
			{
				MessageBox.Show("Поле Полное наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_shotname.Text))
			{
				MessageBox.Show("Поле Краткое наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_INN.Text))
			{
				MessageBox.Show("Поле ИНН не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_OGRN.Text))
			{
				MessageBox.Show("Поле ОГРН не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (New)
				BD.Контрагенты.Add(контрагент);
			BD.SaveChanges();
			this.Close();
		}

		private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!Char.IsDigit(e.Text, 0))
			{
				e.Handled = true;
			}
		}

	}
}
