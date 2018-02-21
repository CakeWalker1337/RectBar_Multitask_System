/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 12.01.2018
 * Time: 12:14
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
using RectBar_Multitask_System.Presenters;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, описывающий логику окна результатов поиска
	/// </summary>
	public partial class SearchResultWindow : Window
	{
		DataGrid outputGrid; //Таблица для вывода информации после подтверждения результатов поиска
		
		SearchResultPresenter presenter = new SearchResultPresenter(); //презентер
		
		/// <summary>
		/// Делегат для эвента подтверждения результатов поиска
		/// </summary>
		/// <param name="og">Таблица для вывода результатов после подтверждения</param>
		/// <param name="ob">Лист обработанных результатов поиска</param>
		/// <param name="ot">Тип объектов результата</param>
		public delegate void SearchWindowResult(DataGrid og, List<Object> ob, ObjectType ot);
		public event SearchWindowResult SearchWindowResultEvent;
		
		ObjectType currentObjectType = ObjectType.Error; //Текущий тип объекта результата
		
		/// <summary>
		/// Конструктор класса результатов поиска УЧЕНИКОВ
		/// </summary>
		/// <param name="outputGrid">Таблица для вывода информации после подтверждения результатов поиска</param>
		/// <param name="st">Лист необработанных результатов поиска учеников</param>
		public SearchResultWindow(DataGrid outputGrid, List<Student> st)
		{
			InitializeComponent();
			presenter.InitGrid(resultGrid, st);
			currentObjectType = ObjectType.Student;
			this.outputGrid = outputGrid;
		}
		
		/// <summary>
		/// Конструктор класса результатов поиска ГРУПП
		/// </summary>
		/// <param name="outputGrid">Таблица для вывода информации после подтверждения результатов поиска</param>
		/// <param name="gr">Лист необработанных результатов поиска групп</param>
		public SearchResultWindow(DataGrid outputGrid, List<Group> gr)
		{
			InitializeComponent();
			presenter.InitGrid(resultGrid, gr);
			currentObjectType = ObjectType.Group;
			this.outputGrid = outputGrid;
		}
		
		/// <summary>
		/// Метод-callback для кнопки "Принять". Запускает эвент с подтверждением обработки результатов поиска.
		/// </summary>
		void acceptButton_Click(object sender, RoutedEventArgs e)
		{
			SearchWindowResultEvent(outputGrid, presenter.Accept(resultGrid, currentObjectType), currentObjectType);
			Close();
		}
		
		/// <summary>
		/// Метод-callback для кнопки "Закрыть"
		/// </summary>
		void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Эвент двойного клика по таблице. Удаляет строку, по которой кликнули.
		/// Таким образом происходит обработка первичных результатов поиска.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="a"></param>
		void resultGrid_DoubleClick(object sender, MouseButtonEventArgs a)
		{
			presenter.DoubleClick(resultGrid);
		}
		
	}
}