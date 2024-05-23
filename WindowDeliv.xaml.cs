using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowDeliv.xaml
	/// </summary>
	public partial class WindowDeliv : Window
	{
		bearEntities BD = new bearEntities();
		List<СписокПозиций> goods = new List<СписокПозиций>();
		Поступления поступления;
		ЗаявкаСнабжение заявка;
		bool IsNew;
		public WindowDeliv(bearEntities bd, Поступления поступл, bool isnew = false, ЗаявкаСнабжение вход_заявка = null)
		{
			this.BD = bd;
			this.поступления = поступл;
			this.IsNew = isnew;
			InitializeComponent();
			this.DataContext = поступления;
			this.заявка = вход_заявка;
			cmb_status.ItemsSource = BD.Статус.ToList();

			if (!isnew)
			{
				goods = поступления.СписокПозиций.ToList();
				dg_catalog.ItemsSource = поступления.СписокПозиций.ToList();
				dg_catalog.IsReadOnly = поступл.УчтеноНаСкладе;
				bt_new.IsEnabled = !поступл.УчтеноНаСкладе;
				bt_addgoods.IsEnabled = !поступл.УчтеноНаСкладе;
			}
			else
			{
				if (заявка != null)
				{
					goods = BD.СписокПозиций.Where(_ => _.ЗаявкаСнабж_ИД == заявка.ЗаявкаСнабж_ИД & _.Количество > _.Товар.Остаток).ToList().Select(item => new СписокПозиций() { Товар = item.Товар, Количество = (item.Количество - item.Товар.Остаток) }).ToList();
					//goods = заявка.СписокПозиций.Where(item => item.Количество > item.Товар.Остаток).Select(item => new СписокПозиций() { Товар_ИД = item.Товар_ИД, Количество = (item.Количество - item.Товар.Остаток) }).ToList();
					dg_catalog.ItemsSource = goods;
				}
				поступления.ДатаСоздания = DateTime.Now;
				поступления.Сотрудники = BD.Сотрудники.Where(empl => empl.Сотрудники_ИД == LoginUtility.сотрудник.Сотрудники_ИД).Single();
				поступления.ЗаявкаСнабжение = заявка;
			}

		}

		private void but_new_Click(object sender, RoutedEventArgs e)
		{
			СписокПозиций item = new WindowSelect(BD, null, true).GetGoods();
			if (item != null)
			{
				item.Чек_ИД = goods.Count + 1;
				goods = goods.Append(item).ToList();
			}
			dg_catalog.ItemsSource = goods;
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void but_save_Click(object sender, RoutedEventArgs e)
		{

			if (string.IsNullOrEmpty(cmb_status.Text))
			{
				MessageBox.Show("Поле Статус наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (goods.Count == 0)
			{
				MessageBox.Show("Список позиций не содержит не элементов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (goods.Where(_ => (_.Цена_Ед ?? 0) == 0 | _.Количество == 0).Count() > 0)
			{
				MessageBox.Show("Список позиций заполнен не полностью", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (IsNew)
			{
				поступления.НомерПоступления = " ";
				BD.Поступления.Add(поступления);
				BD.SaveChanges();
				поступления.НомерПоступления = поступления.Поступления_ИД.ToString().PadLeft(6, '0');
				var id = BD.Поступления.ToList().Last().Поступления_ИД;
				//BD.SaveChanges();
				foreach (var item in goods)
				{

					var i = new СписокПозиций
					{
						Количество = item.Количество,
						Цена_Ед = item.Цена_Ед,
						Товар_ИД = item.Товар.Товар_ИД,
						Товар = BD.Товар.Where(a => a.Товар_ИД == item.Товар.Товар_ИД).Single(),
						Поступления_ИД = id
					};

					BD.СписокПозиций.Add(i);
				}
				//item.Реализация = реализация;
				BD.SaveChanges();
			}
			this.Close();
		}


		private void dg_catalog_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if (dg_catalog.SelectedValue != null)
			{
				int i = ((СписокПозиций)dg_catalog.SelectedValue).Чек_ИД;
				//goods.Remove(чек);
				var item = goods.Where(_ => _.Чек_ИД == i).First();
				new WindowSelect(BD, item, true).GetGoods();
			}

			dg_catalog.SelectedValue = null;
			FillDataGrid();

		}
		public void FillDataGrid()
		{
			dg_catalog.ItemsSource = goods;
		}

		private void bt_add_Click(object sender, RoutedEventArgs e)
		{
			if (IsNew)
			{
				MessageBox.Show("Перед добавлением товаров на склад необходимо сохранить поступление", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}


			if (MessageBox.Show("Это действие необратимо. Выполнить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
			{
				return;
			}

			foreach (var item in goods)
			{
				item.Товар.Остаток += item.Количество;
			}
			поступления.УчтеноНаСкладе = true;
			BD.SaveChanges();
			bt_addgoods.IsEnabled = false;

			MessageBox.Show("Успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

		}

	}
}
