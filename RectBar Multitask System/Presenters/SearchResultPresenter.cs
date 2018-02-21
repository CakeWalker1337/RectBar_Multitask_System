/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/12/2018
 * Time: 12:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RectBar_Multitask_System.Presenters
{
	/// <summary>
	/// Класс, отвечающий за обработку представления окна результатов поиска.
	/// </summary>
	public class SearchResultPresenter
	{
		List<Object> list; //Переменная для хранения листа необработанных результатов поиска
		public SearchResultPresenter()
		{
			list = new List<Object>();
		}
		
		/// <summary>
		/// Метод инициализации сетки с результатами поиска.
		/// Внутри сетки можно редактировать результаты поиска, оставив только нужные.
		/// </summary>
		/// <param name="resultGrid">сетка, хранящая необработанный результат поиска (элемент окна)</param>
		/// <param name="gr">Лист результатов поиска групп</param>
		public void InitGrid(DataGrid resultGrid, List<Group> gr)
		{
			resultGrid.Columns[1].Header = "Название";
			resultGrid.Columns[2].Header = "Тип";
			resultGrid.Columns[3].Header = "Уровень";
			
			for(int i = 0; i<gr.Count; i++)
			{
				resultGrid.Items.Add(gr[i].toSample());
				list.Add(gr[i]);
			}
				
		}
		
		/// <summary>
		/// Метод инициализации сетки с результатами поиска.
		/// Внутри сетки можно редактировать результаты поиска, оставив только нужные.
		/// </summary>
		/// <param name="resultGrid">сетка, хранящая необработанный результат поиска (элемент окна)</param>
		/// <param name="st">Лист результатов поиска учеников</param>
		public void InitGrid(DataGrid resultGrid, List<Student> st)
		{			
			for(int i = 0; i<st.Count; i++)
			{
				resultGrid.Items.Add(st[i].toSample());
				list.Add(st[i]);
			}
		}
		
		/// <summary>
		/// Метод, отправляющий обработанные результаты поиска обратно в окно.
		/// </summary>
		/// <param name="grid">Сетка обработанных результатов. Данные из нее переносятся в лист и отправляются в окно.</param>
		/// <param name="ot">Тип данных результатов</param>
		/// <returns>Возвращает лист обработанных данных поиска.</returns>
		public List<Object> Accept(DataGrid grid, ObjectType ot)
		{
			List<Object> result = new List<Object>();
			if(ot == ObjectType.Student)
			{
				
				for(int i = 0; i<grid.Items.Count; i++)
				{
					SampleGrid sg = (SampleGrid) grid.Items[i];
					for(int j = 0; j<list.Count; j++)
					{
						if(sg.P1 == ((Student)list[j]).Id.ToString())
						{
							result.Add(list[j]);
							break;
						}
					}
				}
				return result;
			}
			if(ot == ObjectType.Group)
			{
				
				for(int i = 0; i<grid.Items.Count; i++)
				{
					SampleGrid sg = (SampleGrid) grid.Items[i];
					for(int j = 0; j<list.Count; j++)
					{
						if(sg.P1 == ((Group)list[j]).Id.ToString())
						{
							result.Add(list[j]);
							break;
						}
					}
				}
				return result;
			}
			return null;
		}
		
		/// <summary>
		/// Метод, обрабатывающий двойной клик по таблице результатов.
		/// Удаляет выбранный элемент из таблицы.
		/// </summary>
		/// <param name="grid"> Таблица результатов поиска</param>
		public void DoubleClick(DataGrid grid)
		{
			if(grid.SelectedItem == null) return;
			grid.Items.RemoveAt(grid.SelectedIndex);
		}
	}
}
