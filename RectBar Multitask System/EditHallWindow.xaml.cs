/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/05/2018
 * Time: 21:55
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
	/// Класс, содержащий логику окна редактирования зала
	/// </summary>
	public partial class EditHallWindow : Window
	{
		WindowOperationFlag fl; //флаг операции (создать или редактировать)
		EditHallPresenter presenter; // презентер окна
		
		/// <summary>
		/// Делегат, отвечающий за эвент изменения данных в окне.
		/// Отправляет сообщение главному окну для обновления данных.
		/// </summary>
		public delegate void ChangeWindowData(); 
		public event ChangeWindowData ChangeWindowDataEvent;
				
		public EditHallWindow(WindowOperationFlag of)
		{
			InitializeComponent();
			presenter = new EditHallPresenter();
			fl = of;
			if(of == WindowOperationFlag.Edit)
			{
				this.Title = "Редактировать зал";
				deleteButton.IsEnabled = true;
				choicePanel.Visibility = Visibility.Visible;
				presenter.InitNameComboBox(nameComboBox); //Инициализация презентера
			}
			else
			{
				saveButton.Content = "Создать";
				this.Title = "Создать зал";
				deleteButton.IsEnabled = false;
			}
		}
		
		/// <summary>
		/// Метод-callback на нажатие кнопки "Сохранить"
		/// В зависимости от флага операции создает или сохраняет зал.
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if(fl == WindowOperationFlag.Edit)
			{
				if(presenter.SaveHall(nameComboBox.SelectedIndex, nameBox.Text))
				{
					MessageBox.Show("Сохранение успешно!");
					ChangeWindowDataEvent();
				}
				else
					MessageBox.Show("Ошибка сохранения!");
			}
			else
			{
				if(presenter.CreateHall(nameBox.Text))
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
		/// Метод-callback для кнопки удаления зала.
		/// Вызывает окно подтверждения.
		/// </summary>
		void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			if(nameComboBox.SelectedItem == null) return;
			ConfirmWindow cw = new ConfirmWindow("Вы точно хотите удалить зал " + MTSystem.getHall(nameComboBox.SelectedIndex).Name + "?");
			cw.ConfirmEvent += DeleteHall_CallBack;
			cw.ShowDialog();
		}
		
		/// <summary>
		/// Метод-callback для эвента изменения редактируемого зала
		/// </summary>
		void nameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			presenter.ComboBoxSelectionChanged(nameComboBox, nameBox);
		}
		
		/// <summary>
		/// Метод-callback для эвента подтверждения удаления зала.
		/// Если было получено подтверждение удаления, то удаляем зал.
		/// </summary>
		/// <param name="result">Результат из окна подтверждения</param>
		void DeleteHall_CallBack(bool result)
		{
			if(result)
			{
				if(presenter.DeleteHall(nameComboBox.SelectedIndex))
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