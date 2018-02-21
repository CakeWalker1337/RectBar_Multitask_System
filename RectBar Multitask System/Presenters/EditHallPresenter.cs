/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, отвечающий за обработку представления окна редактирования зала.
	/// </summary>
	public class EditHallPresenter
	{		
		int currentHall = 0;
		
		/// <summary>
		/// Метод, инициализирующий комбобокс с залами.
		/// </summary>
		/// <param name="cb">Комбобокс с залами.</param>
		public void InitNameComboBox(ComboBox cb)
		{
			for(int i = 0; i<MTSystem.HallsCount; i++)
			{
				cb.Items.Add(MTSystem.getHall(i).Name);
			}
			if(MTSystem.HallsCount != 0) cb.SelectedIndex = 0;
		}
		
		/// <summary>
		/// Метод, обрабатывающий изменение комбобокса с залами.
		/// </summary>
		/// <param name="cb">Комбобокс с залами.</param>
		/// <param name="tb">Текстбокс, в который выводится название текущего зала.</param>
		public void ComboBoxSelectionChanged(ComboBox cb, TextBox tb)
		{
			tb.Text = cb.SelectedItem.ToString();
		}
		
		/// <summary>
		/// Метод сохранения зала (сохраняется название).
		/// </summary>
		/// <param name="index">индекс текущего зала в массиве залов</param>
		/// <param name="tb">Новое название зала</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool SaveHall(int index, String tb)
		{
			Hall t = MTSystem.getHall(index);
			if(tb == "") return false;
			t.Name = tb;
			return MTSystem.SaveHall(t);
		}
		
		/// <summary>
		/// Метод создания зала
		/// </summary>
		/// <param name="tb">Название нового зала</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool CreateHall(String tb)
		{
			if(tb == "") return false;
			Hall t = new Hall();
			t.Name = tb;
			MTSystem.CreateHall(t);
			t.Id = MTSystem.GetLastInsertId("halls");
			MTSystem.addHall(t);
			for(int i = 0; i<MTSystem.SchedulesCount; i++)
			{
				for(int j = 0; j<MTSystem.getSchedule(i).RowCount; j++)
				{
					for(int k = 1; k<=7; k++)
					{
						MTSystem.getSchedule(i).getRow(j).GroupIds.Insert(MTSystem.HallsCount*k-1, 0);
					}
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Метод удаления зала
		/// </summary>
		/// <param name="index">Индекс удаляемого зала</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool DeleteHall(int index)
		{
			currentHall = index;
			Hall t = MTSystem.getHall(currentHall);
			int id = t.Id;
			for(int i = 0; i<MTSystem.SchedulesCount; i++)
			{
				for(int j = 0; j<MTSystem.getSchedule(i).RowCount; j++)
				{
					for(int k = 0; k<7; k++)
					{
						MTSystem.getSchedule(i).getRow(j).GroupIds.RemoveAt(currentHall + (MTSystem.HallsCount-1)*k);
					}
				}
			}

			MTSystem.deleteHall(t);
			return MTSystem.DeleteHall(id);
			
		}
		
	}
}
