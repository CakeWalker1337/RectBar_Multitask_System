/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 10/12/2017
 * Time: 18:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, содержащий данные о расписании
	/// </summary>
	public class Schedule
	{
		private List<ScheduleRow> rows; //лист строк расписания
		public int RowCount{ 
			get
			{
				return rows.Count;
			}
		}
		public int Id{get;set;} //Айди расписания в БД
		public String ScheduleName{get;set;} //название
		public Schedule()
		{
			rows = new List<ScheduleRow>();
			ScheduleName = "";
			Id = 0;
		}
		
		public ScheduleRow getRow(int num)
		{
			return rows[num];
		}
		
		public void addRow(ScheduleRow gr)
		{
			rows.Add(gr);
		}
		
		public bool deleteRow(ScheduleRow gr)
		{
			return rows.Remove(gr);
		}
		
		/// <summary>
		/// Поиск строки в расписании по айди
		/// </summary>
		/// <param name="id">айди для поиска</param>
		/// <returns>Строка расписания или null в случае провала</returns>
		public ScheduleRow findRowById(int id) //Бинарный поиск
		{
			if(rows.Count == 0 || id == 0) return new ScheduleRow();
			int l = 0, r = rows.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(rows[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(rows[l].Id != id) return null;
			return rows[l];
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
