/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 23.12.2017
 * Time: 19:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, содержащий данные о сессиях.
	/// </summary>
	public class Session
	{
		public int Id{get;set;} //айди сессии
		public DateTime Time{get;set;} //Время сессии (выбирается создателем сессии)
		public int GroupId{get;set;}  //Айди группы, для которой создана сессия
		public int UserId{get;set;} //Айди создателя сессии
		public int Duration{get;set;} //Продолжительность занятия (сессии)
		public int StudentsCount{get;set;} // Число студентов в группе на момент создания сессии
		private List<int> present; //Лист присутствующих учеников
		private List<int> absent; //Лист отсутствующих учеников
		
		public int getPresentId(int index)
		{
			return present[index];
		}
		public void addPresentId(int id)
		{
			present.Add(id);
		}
		
		public void ClearPresentIds()
		{
			present.Clear();
		}
		public void ClearAbsentIds()
		{
			absent.Clear();
		}
		
		/// <summary>
		/// Метод добавления листа айдишников присутствующих учеников к текущему листу преподавателей
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="res">Лист айдишников для добавления</param>
		public void addPresentIdsList(List<int> res)
		{
			if(res == null) return;
			for(int i = 0; i<res.Count; i++)
			{
				present.Add(res[i]);
			}
		}
		public bool deletePresentId(int id)
		{
			return present.Remove(id);
		}
		public int PresentIdsCount
		{
			get {return present.Count;}
		}
		
		
		
		public int getAbsentId(int index)
		{
			return absent[index];
		}
		public void addAbsentId(int id)
		{
			absent.Add(id);
		}
		
		/// <summary>
		/// Метод добавления листа айдишников отсутствующих учеников к текущему листу преподавателей
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="res">Лист айдишников для добавления</param>
		public void addAbsentIdsList(List<int> res)
		{
			if(res == null) return;
			for(int i = 0; i<res.Count; i++)
			{
				absent.Add(res[i]);
			}
		}
		public bool deleteAbsentId(int id)
		{
			return absent.Remove(id);
		}
		public int AbsentIdsCount
		{
			get {return absent.Count;}
		}
		
		/// <summary>
		/// Метод для форматирования айдишников присутствующих людей в строку формата
		/// "_id_id_..."
		/// </summary>
		/// <returns>Форматированная строка (_id_id_id_)</returns>
		public String GetStringFromPresentIds()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<present.Count; i++)
			{
				sb.AppendFormat("{0}_", present[i]);
			}
			return sb.ToString();
		}
		
		
		/// <summary>
		/// Метод для форматирования айдишников отсутствующих людей в строку формата
		/// "_id_id_..."
		/// </summary>
		/// <returns>Форматированная строка (_id_id_id_)</returns>
		public String GetStringFromAbsentIds()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<absent.Count; i++)
			{
				sb.AppendFormat("{0}_", absent[i]);
			}
			return sb.ToString();
		}
		
		public Session()
		{
			present = new List<int>();
			absent = new List<int>();
			Id = 0;
			GroupId = 0;
			UserId = 0;
			Duration = 0;
			StudentsCount = 0;
		}
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public SampleGrid toSample()
		{
			SampleGrid sg = new SampleGrid();
			sg.P1 = Id.ToString();
			sg.P2 = Time.Date.ToString("dd.MM.yyyy");
			sg.P3 = Duration.ToString();
			Group g = MTSystem.findGroupById(GroupId);
			sg.P4 = (g == null)?"Error":g.Name;
			User u = MTSystem.LoadClient(UserId, PermType.All);
			sg.P5 = (u == null)?"Error":u.Name;
			sg.P6 = StudentsCount.ToString();
			sg.P7 = present.Count.ToString();
			sg.P8 = absent.Count.ToString();
			return sg;
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
