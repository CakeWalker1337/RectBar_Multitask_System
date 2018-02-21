/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 13:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Windows.Controls;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, отвечающий за обработку представления окна редактирования расписания.
	/// </summary>
	public class EditSchedulePresenter
	{
		public EditSchedulePresenter()
		{
		}
		
		/// <summary>
		/// Метод сохранения расписания (сохраняется название).
		/// </summary>
		/// <param name="tb">Новое название расписания</param>
		/// <param name="current">Объект текущего расписания</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool SaveSchedule(String tb, Schedule current)
		{
			if(tb == "") return false;			
			current.ScheduleName = tb;
			return MTSystem.SaveSchedule(current);
		}
		
		/// <summary>
		/// Метод создания расписания
		/// </summary>
		/// <param name="tb">Название нового расписания</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool CreateSchedule(String tb)
		{
			if(tb == "") return false;
			Schedule t = new Schedule();
			t.ScheduleName = tb;
			MTSystem.CreateSchedule(t);
			t.Id = MTSystem.GetLastInsertId("schedules");
			MTSystem.addSchedule(t);
			return true;
		}
		
		/// <summary>
		/// Метод удаления расписания
		/// </summary>
		/// <param name="t">Удаляемое расписание</param>
		/// <returns>Успешность операции (true - успех, иначе false)</returns>
		public bool DeleteSchedule(Schedule t)
		{
			if(MTSystem.DeleteSchedule(t))
			{
				MTSystem.deleteSchedule(t);
				return true;
			}
			else return false;
		}
	}
}
