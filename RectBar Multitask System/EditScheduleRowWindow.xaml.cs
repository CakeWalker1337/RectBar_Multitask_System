/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 14:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс-обработчик окна изменения и создания строки расписания.
	/// </summary>
	public partial class EditScheduleRowWindow : Window
	{
		EditScheduleRowPresenter presenter = null; //Презентер окна
		Schedule currentSchedule; //Текущее расписание - "родитель" строки
		ScheduleRow currentRow; //Текущая строка для редактирования
		WindowOperationFlag fl; //флаг операции (изменить или создать)
		
		/// <summary>
		/// Делегат для эвента изменения данных.
		/// Эвент ловится в главном окне.
		/// </summary>
		public delegate void ChangeWindowData();
		public event ChangeWindowData ChangeWindowDataEvent;
		
		public EditScheduleRowWindow(WindowOperationFlag of, ScheduleRow row, Schedule sch)
		{	
			InitializeComponent(); //Инициализация данных окна в случае редактирования строки (конструктор редактирования)
			presenter = new EditScheduleRowPresenter();
			currentSchedule = sch;
			currentRow = row;
			fl = of;
			StringBuilder sb = new StringBuilder();
			
			nameBox.Text = string.Format("{0}{1}:{2}{3}",(row.Time.Hours<10)? "0" : "", row.Time.Hours, (row.Time.Minutes<10)?"0":"", row.Time.Minutes);
			this.Title = "Редактировать строку";
			saveButton.Content = "Сохранить";
			deleteButton.IsEnabled = true;
		}
		
		public EditScheduleRowWindow(WindowOperationFlag of, Schedule sch)
		{
			InitializeComponent();//Инициализация данных окна в случае создания строки (конструктор создания)
			presenter = new EditScheduleRowPresenter();
			currentSchedule = sch;
			fl = of;
			deleteButton.IsEnabled = false;
			saveButton.Content = "Создать";
			this.Title = "Создать строку";
		}
		
		/// <summary>
		/// Метод-callback для кнопки сохранения/создания строки в зависимости от флага
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if(fl == WindowOperationFlag.Edit)
			{
				if(presenter.SaveRow(nameBox.Text, currentRow))
				{
					MessageBox.Show("Сохранение успешно!");
					ChangeWindowDataEvent();
				}
				else
					MessageBox.Show("Ошибка сохранения!");
			}
			else
			{
				if(presenter.CreateRow(nameBox.Text, currentSchedule))
				{
					MessageBox.Show("Создание успешно!");
					ChangeWindowDataEvent();	
				}
				else
					MessageBox.Show("Ошибка создания!");
			}
			Close();
		}
		
		/// <summary>
		/// Метод-callback для кнопки выхода
		/// </summary>
		void exitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Метод-callback для кнопки удаления. Вызывает окно подтверждения.
		/// </summary>
		void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			ConfirmWindow cw = new ConfirmWindow("Вы точно хотите удалить строку с ID = " + currentRow.Id.ToString() + "?");
			cw.ConfirmEvent += DeleteRow_CallBack;
			cw.ShowDialog();
		}
		
		/// <summary>
		/// Метод-callback для эвента подтверждения удаления. Обрабатывает результат окна подтверждения.
		/// </summary>
		void DeleteRow_CallBack(bool result)
		{
			if(result)
			{
				if(presenter.DeleteRow(currentRow, currentSchedule))
			{
				MessageBox.Show("Удаление успешно!");
				ChangeWindowDataEvent();
				Close();
			}
			else
				MessageBox.Show("Ошибка удаления!");
			}
		}
		
	}
}