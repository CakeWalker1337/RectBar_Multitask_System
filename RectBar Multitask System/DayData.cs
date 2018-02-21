/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 02.01.2018
 * Time: 15:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, представляющий расписание в день недели (во всех залах).
	/// Нужен, чтобы выгружать данные из таблиц, т.к. день недели = своя таблица.
	/// </summary>
	public class DayData
	{
		public int RowId{get;set;} // айди строки, к которой принадлежит этот сектор
		private List<int> groupIds; //Айди групп по порядку залов в день недели
		
		public int getGroupId(int num) // Стандартные методы доступа к листу групп
		{
			return groupIds[num];
		}
		
		public void addGroupId(int gr)
		{
			groupIds.Add(gr);
		}
		
		public bool deleteGroupId(int gr)
		{
			return groupIds.Remove(gr);
		}

		public int GroupIdsCount
		{
			get{ return groupIds.Count;}
		}
		
		public DayData()
		{
			RowId = 0;
			groupIds = new List<int>();
		}
	}
}
