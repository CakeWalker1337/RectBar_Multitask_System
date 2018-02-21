/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 16.10.2017
 * Time: 20:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, содержащий данные о строке расписания.
	/// </summary>
	public class ScheduleRow
	{
		public int Id{get;set;} //Айди строки в БД
		public TimeSpan Time{get;set;} //Время строки
		public int ScheduleId{get;set;} //Айди расписания, к которому принадлежит строка
		public List<int> GroupIds{get;set;} //Айди групп, принадлежащих строке в расписании
		
		public ScheduleRow()
		{
			Id = 0;
			Time = new TimeSpan();
			ScheduleId = 0;
			GroupIds = new List<int>();
		}
		
		/// <summary>
		/// Проверяет, есть ли группа в строке по айди
		/// </summary>
		/// <param name="id">айди для проверки</param>
		/// <returns>True - есть, иначе false</returns>
		public bool CheckGroupId(int id)
		{
			for(int i = 0; i<GroupIds.Count; i++)
			{
				if(GroupIds[i] == id) return true;
			}
			return false;
		}
		
		/// <summary>
		/// Проверяет объект на валидность (id>0)
		/// </summary>
		/// <returns>True - валидный, иначе false</returns>
		public bool IsValid()
		{
			return Id>0;
		}
	}
}
