/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 21.12.2017
 * Time: 20:11
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
	/// Класс-обработчик окна изменения и создания расписания.
	/// </summary>
	public partial class EditScheduleWindow : Window
	{
		EditSchedulePresenter presenter = null; //Презентер
		Schedule currentSchedule; //Текущее расписание для редактирования
		WindowOperationFlag fl; //Флаг операции (создать или изменить)
		
		/// <summary>
		/// Делегат для эвента изменения данных.
		/// Эвент ловится в главном окне.
		/// </summary>
		public delegate void ChangeWindowData();
		public event ChangeWindowData ChangeWindowDataEvent;
		
		public EditScheduleWindow(WindowOperationFlag of, Schedule sch)
		{	
			InitializeComponent();//Инициализация окна в случае редактирования
			presenter = new EditSchedulePresenter();
			currentSchedule = sch;
			fl = of;
			nameBox.Text = sch.ScheduleName;
			this.Title = "Редактировать расписание";
			saveButton.Content = "Сохранить";
			deleteButton.IsEnabled = true;
		}
		
		public EditScheduleWindow(WindowOperationFlag of)
		{
			InitializeComponent();//Инициализация окна в случае создания
			presenter = new EditSchedulePresenter();
			fl = of;
			deleteButton.IsEnabled = false;
			saveButton.Content = "Создать";
			this.Title = "Создать расписание";
		}
		
		/// <summary>
		/// Метод-callback для кнопки сохранения/создания расписания в зависимости от флага
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if(fl == WindowOperationFlag.Edit)
			{
				if(presenter.SaveSchedule(nameBox.Text, currentSchedule))
				{
					MessageBox.Show("Сохранение успешно!");
					ChangeWindowDataEvent();
				}
				else
					MessageBox.Show("Ошибка сохранения!");
			}
			else
			{
				if(presenter.CreateSchedule(nameBox.Text))
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
			ConfirmWindow cw = new ConfirmWindow("Вы точно хотите удалить расписание " + currentSchedule.ScheduleName + "?");
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
				if(presenter.DeleteSchedule(currentSchedule))
				{
					MessageBox.Show("Удаление успешно!");
					ChangeWindowDataEvent();
					Close();
				}
				else MessageBox.Show("Ошибка удаления");
				
			}
		}
				
	}
}