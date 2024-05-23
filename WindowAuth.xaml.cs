using System.Linq;
using System.Windows;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowAuth.xaml
	/// </summary>
	public partial class WindowAuth : Window
	{
		bearEntities bd = new bearEntities();
		public WindowAuth()
		{
			InitializeComponent();
		}
		private void Bt_auth_Click(object sender, RoutedEventArgs e)
		{
			string pass = LoginUtility.GetHash(tb_Pass.Password);
			Сотрудники сотрудник = bd.Сотрудники.Where(a => a.Логин == tb_Login.Text & a.ХешПароля == pass).SingleOrDefault();

			if (сотрудник != null)
			{
				LoginUtility.сотрудник = сотрудник;
				var mainscreen = new MainWindow();
				mainscreen.Show();
				this.Close();
			}
			else
			{
				MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}


    }
}
