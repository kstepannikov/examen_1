using System;
using System.Linq;
using System.Windows;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowAgent.xaml
	/// </summary>
	public partial class WindowEmpl : Window
	{
		bearEntities BD = new bearEntities();
		Сотрудники сотрудник = new Сотрудники();
		bool New;
		public WindowEmpl(bearEntities bD, Сотрудники сотрудник, bool isnew = false)
		{
			InitializeComponent();
			BD = bD;
			this.сотрудник = сотрудник;
			New = isnew;
			this.DataContext = сотрудник;
			cmb_category.ItemsSource = BD.Должность.ToList();
			if (New)
			{
				tbl_pass.Content = "Пароль: *";
			}
		}

		private void but_save_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(tb_firstname.Text))
			{
				MessageBox.Show("Поле Фамилия не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_name.Text))
			{
				MessageBox.Show("Поле Имя не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_login.Text))
			{
				MessageBox.Show("Поле Логин не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (New)
			{
				if (String.IsNullOrEmpty(tb_newpassword.Text) | String.IsNullOrEmpty(tb_login.Text))
				{
					MessageBox.Show("Не указан логин или пароль", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				else
				{
					сотрудник.ХешПароля = LoginUtility.GetHash(tb_newpassword.Text);
					BD.Сотрудники.Add(сотрудник);
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(tb_newpassword.Text))
				{
					сотрудник.ХешПароля = LoginUtility.GetHash(tb_newpassword.Text);
				}
			}

			BD.SaveChanges();
			this.Close();
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
