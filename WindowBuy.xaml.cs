
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;


namespace BuildingKirill
{
	/// <summary>
	/// Логика взаимодействия для WindowBuy.xaml
	/// </summary>
	public partial class WindowBuy : Window
	{
		bearEntities BD = new bearEntities();
		List<СписокПозиций> goods = new List<СписокПозиций>();
		ЗаявкаСнабжение заявка;
		Адрес адрес;
		bool IsNew;
		public WindowBuy(bearEntities bd, ЗаявкаСнабжение заявка, bool isnew = false)
		{
			InitializeComponent();
			this.BD = bd;
			this.заявка = заявка;
			this.IsNew = isnew;
			this.DataContext = this.заявка;

			//Адрес адрес = new Адрес();

			cmb_status.ItemsSource = BD.Статус.ToList();
			cmb_sity.ItemsSource = BD.НаселПункт.ToList();
			cmb_street.ItemsSource = BD.Улица.ToList();
			if (!isnew)
			{
				this.goods = BD.СписокПозиций.Where(b => b.ЗаявкаСнабж_ИД == this.заявка.ЗаявкаСнабж_ИД).ToList();
				dg_catalog.ItemsSource = goods;
				dg_catalog.IsReadOnly = true;
				//bt_del.IsEnabled = false;
				bt_new.IsEnabled = false;
				адрес = заявка.Адрес;
			}
			else
			{
				заявка.ДатаСоздания = DateTime.Now;
				заявка.Сотрудники = BD.Сотрудники.Where(emp => emp.Сотрудники_ИД == LoginUtility.сотрудник.Сотрудники_ИД).Single();
				заявка.Статус_ИД = 1;
				адрес = new Адрес();
				cmb_status.SelectedIndex = 0;
			}
			gb_adress.DataContext = адрес;
			if (LoginUtility.сотрудник.Должность.ЭтоВодитель == true)
			{
				bt_new.IsEnabled = false;
				bt_reserve.IsEnabled = false;
				bt_del.IsEnabled = false;
				but_save.Content = "Закрыть";
			}

		}

		private void but_new_Click(object sender, RoutedEventArgs e)
		{
			СписокПозиций item = new WindowSelect(BD).GetGoods();
			if (item != null)
			{
				goods = goods.Append(item).ToList();
			}
			dg_catalog.ItemsSource = goods;
		}

		private void bt_del_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Вы точно хотите отменить изменения", "Несохраненные изменения", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				this.Close();
			}
		}

		private void but_save_Click(object sender, RoutedEventArgs e)
		{
			if (LoginUtility.сотрудник.Должность.ЭтоВодитель == true)
			{
				this.Close();
				return;
			}

			if (string.IsNullOrEmpty(cmb_status.Text))
			{
				MessageBox.Show("Поле Статус наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(cmb_sity.Text))
			{
				MessageBox.Show("Поле Насел. пункт наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(cmb_street.Text))
			{
				MessageBox.Show("Поле Улица наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_contactname.Text))
			{
				MessageBox.Show("Поле Контактное лицо наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (string.IsNullOrEmpty(tb_contactphone.Text))
			{
				MessageBox.Show("Поле Телефон наименование не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (goods.Count == 0)
			{
				MessageBox.Show("Список позиций не содержит не элементов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}




			var result = MessageBoxResult.None;
			if (goods.Where(a => a.Количество > a.Товар.Остаток).Count() != 0 & !заявка.ТоварСписан)
			{
				result = MessageBox.Show("В заявке есть товары которых не хватает на складе." +
					"\nНажмите ДА, чтобы создать заявку на поступление товаров\nНажмите НЕТ, для сохранения без создания заявки на поступление" +
					"\nНажмите ОТМЕНА чтобы не сохранять заявку", "Предупреждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Cancel | result == MessageBoxResult.None)
				{
					return;
				}
			}

			if (IsNew)
			{

				заявка.НомерЗаявки = " ";
				заявка.Адрес = адрес;
				BD.ЗаявкаСнабжение.Add(заявка);
				BD.SaveChanges();
				var id = BD.ЗаявкаСнабжение.ToList().Last().ЗаявкаСнабж_ИД;
				заявка.НомерЗаявки = id.ToString().PadLeft(6, '0');
				//BD.SaveChanges();
				foreach (var item in goods)
				{

					var i = new СписокПозиций
					{
						Количество = item.Количество,
						Цена_Ед = item.Цена_Ед,
						Товар_ИД = item.Товар.Товар_ИД,
						Товар = BD.Товар.Where(a => a.Товар_ИД == item.Товар.Товар_ИД).Single(),
						ЗаявкаСнабж_ИД = id
					};

					BD.СписокПозиций.Add(i);
				}
				//item.Реализация = реализация;
				BD.SaveChanges();
			}
			if (result == MessageBoxResult.Yes)
			{
				var item = new Поступления();
				new WindowDeliv(BD, item, true, заявка).Show();
			}
			this.Close();
		}


		private void but_docx_Click(object sender, RoutedEventArgs e)
		{
			string tempFilePath = Path.GetTempFileName().Replace(".tmp", ".pdf");

			Document doc = new Document();

			PdfWriter.GetInstance(doc, new FileStream(tempFilePath, FileMode.Create));
			doc.Open();

			BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
			iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

			PdfPTable table = new PdfPTable(2);
			PdfPTable table2 = new PdfPTable(3);

			table.AddCell(new PdfPCell(new Phrase("Номер заявки: ", font)));

			table.AddCell(new PdfPCell(new Phrase(заявка.НомерЗаявки, font)));

			table.AddCell(new PdfPCell(new Phrase("Cотрудник: ", font)));

			table.AddCell(new PdfPCell(new Phrase(заявка.Сотрудники.Фамилия + " " + заявка.Сотрудники.Имя + " " + заявка.Сотрудники.Отчество + " (" + заявка.Сотрудники.Должность.Наименование + ")", font)));

			table.AddCell(new PdfPCell(new Phrase("Время создания: ", font)));
			table.AddCell(заявка.ДатаСоздания.ToString("dd.MM.yy HH:mm"));

			table.AddCell(new PdfPCell(new Phrase("Cтатус: ", font)));
			table.AddCell(new PdfPCell(new Phrase(заявка.Статус.Наименование, font)));

			table.AddCell(new PdfPCell(new Phrase("Адрес: ", font)));
			table.AddCell(new PdfPCell(new Phrase(заявка.Адрес.НаселПункт.Наименование + " " + заявка.Адрес.Улица.Наименование + " " + заявка.Адрес.Дом + " " + заявка.Адрес.Подъезд, font)));

			table.AddCell(new PdfPCell(new Phrase("Контактное лицо: ", font)));
			table.AddCell(new PdfPCell(new Phrase(заявка.Адрес.КонтактноеЛицо + " " + заявка.Адрес.Телефон, font)));

			table2.AddCell(new PdfPCell(new Phrase("Наименование", font)) { BackgroundColor = BaseColor.LIGHT_GRAY });
			table2.AddCell(new PdfPCell(new Phrase("На складе", font)) { BackgroundColor = BaseColor.LIGHT_GRAY });
			table2.AddCell(new PdfPCell(new Phrase("Требуется", font)) { BackgroundColor = BaseColor.LIGHT_GRAY });

			foreach (var item in goods)
			{
				table2.AddCell(new PdfPCell(new Phrase(item.Товар.Наименование.ToString(), font)));
				table2.AddCell(item.Товар.Остаток.ToString("f3"));
				table2.AddCell(item.Количество.ToString("f3"));
			}

			doc.Add(table);

			doc.Add(new Paragraph("\n \n \n"));

			doc.Add(table2);

			//Закрываем документ
			doc.Close();

			Process.Start(tempFilePath);

			MessageBox.Show("Pdf-документ сохранен");
		}

		private void bt_reserve_Click(object sender, RoutedEventArgs e)
		{
			if (goods.Where(_ => _.Количество > _.Товар.Остаток).Count() > 0)
			{

				MessageBox.Show("По некоторым позициям товаров не хватает на складе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;

			}


			if (MessageBox.Show("Это действие необратимо. Выполнить?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
			{
				return;
			}

			foreach (var item in goods)
			{
				item.Товар.Остаток -= item.Количество;
			}
			заявка.ТоварСписан = true;
			BD.SaveChanges();

			bt_reserve.IsEnabled = false;

			MessageBox.Show("Успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);


		}
	}
}
