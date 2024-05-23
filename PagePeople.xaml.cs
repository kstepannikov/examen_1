using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для PagePeople.xaml
	/// </summary>
	public partial class PagePeople : Page
	{
		public PagePeople()
		{
			InitializeComponent();
			fr_1.Content = new PageAgent();
			fr_2.Content = new PageEmpl();

		}
	}
}
