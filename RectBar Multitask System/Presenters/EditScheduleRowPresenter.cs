/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 14:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;


namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, отвечающий за обработку представления окна редактирования строки расписания.
	/// </summary>
	public class EditScheduleRowPresenter
	{
		public EditScheduleRowPresenter()
		{
		}
		
		/// <summary>
		/// Метод сохранения строки расписания (сохраняется название).
		/// </summary>
		/// <param name="text">Новое название строки расписания</param>
		/// <param name="row">Объект текущей строки расписания</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool SaveRow(String text, ScheduleRow row)
		{
			TimeSpan sp = new TimeSpan();
			if(!TimeSpan.TryParse(text, out sp)) return false;
			row.Time = sp;
			return MTSystem.SaveScheduleRow(row);	
		}
		
		/// <summary>
		/// Метод создания строки расписания
		/// </summary>
		/// <param name="text">Название новой строки расписания</param>
		/// <param name="sch">Объект расписания, которому принадлежит строка</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool CreateRow(String text, Schedule sch)
		{
			if(!text.Contains(":")) return false;
			TimeSpan sp = new TimeSpan();
			if(!TimeSpan.TryParse(text, out sp)) return false;
			ScheduleRow row = new ScheduleRow();
			row.ScheduleId = sch.Id;
			row.Time = sp;
			for(int i = 0; i<7*MTSystem.HallsCount; i++)
			{
				row.GroupIds.Add(0);
			}
			MTSystem.CreateScheduleRow(row);
			row.Id = MTSystem.GetLastInsertId("rows");
			sch.addRow(row);
			return true;
		}
		
		/// <summary>
		/// Метод удаления строки расписания
		/// </summary>
		/// <param name="row">Удаляемая строка расписания</param>
		/// <param name="sch">Объект расписания, которому принадлежит строка</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool DeleteRow(ScheduleRow row, Schedule sch)
		{
			int id = row.Id;
			sch.deleteRow(row);
			return MTSystem.DeleteScheduleRow(id);
		}
		
	}
}
