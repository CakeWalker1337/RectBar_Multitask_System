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
using System.Text;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Description of Student.
	/// </summary>
	public class Student
	{
		public int Id {get; set;}
		public String FullName {get; set;}
		public int Age {get; set;}
		public StudentStatus Status {get; set;}
		private List<Achievement> achievements;
		private List<int> groupIds;
		
		
		public Student()
		{
			achievements = new List<Achievement>();
			groupIds = new List<int>();
			Status = new StudentStatus();
			Id = -1;
			FullName = "";
		}
		
		public Achievement getAchievement(int num)
		{
			return achievements[num];
		}
		public void addAchievement(Achievement gr)
		{
			achievements.Add(gr);
		}
		public bool deleteAchievement(Achievement gr)
		{
			return achievements.Remove(gr);
		}
		public int AchievementsCount
		{
			get{return achievements.Count;}
		}
		
		public void ClearAchievements()
		{
			achievements.Clear();		
		}
		
		/// <summary>
		/// Метод добавления листа достижений к текущему листу достижений
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="ids">Лист достижений для добавления</param>
		public void addAchievementsList(List<Achievement> ids)
		{
			for(int i = 0; i<ids.Count; i++) achievements.Add(ids[i]);
		}
		
		/// <summary>
		/// Метод поиска достижения в списке по айди
		/// </summary>
		/// <param name="id">Айди достижения для поиска</param>
		/// <returns>Объект достижения</returns>
		public Achievement findAchievementById(int id)
		{
			for(int i = 0; i<achievements.Count; i++)
			{
				if(achievements[i].Type == id)
				{
					return achievements[i];
				}
			}
			return null;
		}
		
		public int getGroupId(int index)
		{
			return groupIds[index];
		}
		public void addGroupId(int id)
		{
			groupIds.Add(id);
		}
		public bool deleteGroupId(int id)
		{
			return groupIds.Remove(id);
		}
		public int GroupIdsCount
		{
			get{return groupIds.Count;}
		}
		
		public bool checkGroupId(int id)
		{
			for(int i = 0; i<groupIds.Count; i++)
			{
				if(groupIds[i] == id) return true;
			}
			return false;
		}
		
		public void ClearGroupIds()
		{
			groupIds.Clear();		
		}
		
		/// <summary>
		/// Проверяет объект на валидность (id>0)
		/// </summary>
		/// <returns>True - валидный, иначе false</returns>
		public bool IsValid()
		{
			return Id > 0;
		}
		
		/// <summary>
		/// Метод добавления листа айди групп к текущему листу
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="ids">Лист айди групп для добавления</param>
		public void addGroupIdsList(List<int> ids)
		{
			for(int i = 0; i<ids.Count; i++) groupIds.Add(ids[i]);
		}
		
		/// <summary>
		/// Метод для форматирования достижений в строку формата
		/// "_id-place_id-place_..."
		/// </summary>
		/// <returns>Форматированная строка (_id-place_id-place_...)</returns>
		public String GetStringFromAchievements()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<achievements.Count; i++)
			{
				sb.AppendFormat("{0}-{1}_", achievements[i].Type, achievements[i].Place);
			}
			return sb.ToString();
		}
		
		/// <summary>
		/// Метод для форматирования айдишников групп в строку формата
		/// "_id_id_..."
		/// </summary>
		/// <returns>Форматированная строка (_id_id_id_)</returns>
		public String GetStringFromGroupIds()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<groupIds.Count; i++)
			{
				sb.AppendFormat("{0}_", groupIds[i]);
			}
			return sb.ToString();
		}
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public SampleGrid toSample()
		{
			SampleGrid sg = new SampleGrid();
			
			sg.P1 = Id.ToString();
			sg.P2 = FullName;
			sg.P3 = Age.ToString();
			sg.P4 = Status.Name;
			return sg;
		}
	}
}
