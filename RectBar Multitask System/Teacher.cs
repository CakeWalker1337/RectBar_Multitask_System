/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 16:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Description of Teacher.
	/// </summary>
	public class Teacher : User
	{
		private List<int> groupIds;
		public Teacher()
		{
			groupIds = new List<int>();
			Type = PermType.Teacher;
		}
		
		public int getGroupId(int index)
		{
			return groupIds[index];
		}
		
		/// <summary>
		/// Метод добавления листа айди групп к текущему листу
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="res">Лист айди групп для добавления</param>
		public void addGroupIdsList(List<int> res)
		{
			if(res == null) MessageBox.Show("Groups = null");
			for(int i = 0; i<res.Count; i++)
			{
				groupIds.Add(res[i]);
			}
		}
		
		public void addGroupId(int id)
		{
			groupIds.Add(id);
		}
		
		public bool deleteGroupId(int id)
		{
			return groupIds.Remove(id);
		}
		
		public void ClearGroupIds()
		{
			groupIds.Clear();
		}
		
		public int GroupIdsCount
		{
			get {return groupIds.Count;}
		}
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public new SampleGrid toSample()
		{
			SampleGrid s = base.toSample();
			s.P5 = MTSystem.GetStringFromIds(groupIds);
			return s;
		}
		
	}
}
