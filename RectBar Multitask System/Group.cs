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
using System.Windows.Media;


namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс группы в системе.
	/// </summary>
	/// 
	public class Group
	{
		public int Id{get;set;} //Айди группы
		public String Name {get; set;} //Название группы
		
		public GroupType Type {get; set;} //Тип группы
		public GroupLevel Level {get; set;} //Уровень группы
		public int RealHours {get; set;} //Количество проведенных часов
		public int PlanHours {get; set;} //Планируемое количество часов в месяц
		public SolidColorBrush Color{get;set;} //Цвет группы
		private List<int> StudentIds; // Лист с айди учеников в группе
		private List<int> TeacherIds; // Лист с айди учителей в группе
		
		public int LeavePart {get;set;} //Текучка группы в процентах
		
		public Group()
		{
			InitializeData();
		}
		public Group(String name)
		{
			InitializeData();
			Name = name;
		}
		private void InitializeData() //Обнуление данных
		{
			Name = "";
			Id = -1;
			TeacherIds = new List<int>();
			StudentIds = new List<int>();
			RealHours = -1;
			PlanHours = -1;
			
			Color = new SolidColorBrush();
			Type = new GroupType();
			Level = new GroupLevel();
		}
		
		public int getTeacherId(int index) //Стандартные методы доступа к листам
		{
			return TeacherIds[index];
		}
		
		/// <summary>
		/// Метод добавления листа айдишников к текущему листу преподавателей
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="res">Лист айдишников для добавления</param>
		public void addTeacherIdsList(List<int> res)
		{
			for(int i = 0; i<res.Count; i++)
			{
				TeacherIds.Add(res[i]);
			}
		}
		public void addTeacherId(int id)
		{
			TeacherIds.Add(id);
		}
		public bool deleteTeacherId(int id)
		{
			return TeacherIds.Remove(id);
		}
		public int TeachersCount
		{
			get {return TeacherIds.Count;}
		}
		
		public void ClearTeacherIds()
		{
			 TeacherIds.Clear();
		}
		
		
		public int getStudentId(int index)
		{
			return StudentIds[index];
		}
		
		/// <summary>
		/// Метод добавления листа айдишников к текущему листу студентов
		/// (Чтобы не добавлять по одному)
		/// </summary>
		/// <param name="res">Лист айдишников для добавления</param>
		public void addStudentIdsList(List<int> res)
		{
			for(int i = 0; i<res.Count; i++)
			{
				StudentIds.Add(res[i]);
			}
		}
		public void addStudentId(int id)
		{
			StudentIds.Add(id);
		}
		public bool deleteStudentId(int id)
		{
			return StudentIds.Remove(id);
		}
		
		public bool checkStudentId(int id)
		{
			for(int i = 0; i<StudentIds.Count; i++)
			{
				if(StudentIds[i] == id) return true;
			}
			return false;
		}
		
		public int StudentsCount
		{
			get {return StudentIds.Count;}
		}
		
		public void ClearStudentIds()
		{
			 StudentIds.Clear();
		}
		
		public bool IsValid()
		{
			return (Id>0);
		}
		
		/// <summary>
		/// Метод составления строки по айди преподавателей
		/// Нужна для форматирования данных для занесения в БД.
		/// </summary>
		/// <returns>Строка в формате "_id_id_..."</returns>
		public String GetStringFromTeacherIds()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<TeacherIds.Count; i++)
			{
				sb.AppendFormat("{0}_", TeacherIds[i]);
			}
			return sb.ToString();
		}
		
		/// <summary>
		/// Метод составления строки по айди учеников
		/// Нужна для форматирования данных для занесения в БД.
		/// </summary>
		/// <returns>Строка в формате "_id_id_..."</returns>
		public String GetStringFromStudentIds()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<StudentIds.Count; i++)
			{
				sb.AppendFormat("{0}_", StudentIds[i]);
			}
			return sb.ToString();
		}
		
		/// <summary>
		/// Метод, преобразующий текущий класс в объект класса SampleGrid
		/// </summary>
		/// <returns>Объект класса SampleGrid с данными текущего класса</returns>
		public SampleGrid toSample()
		{
			SampleGrid sg = new SampleGrid();
			sg.P1 = Id.ToString();
			sg.P2 = Name;
			sg.P3 = Type.Name;
			sg.P4 = Level.Name;
			DateTime start = new DateTime();
			start.AddYears(DateTime.Today.Year);
			start.AddMonths(DateTime.Today.Month);
			
			List<Session> sessions = MTSystem.LoadSessionsByGroupIdByTime(Id, start, DateTime.Today.Add(new TimeSpan(23,59,59)));
			if(sessions[0] != null)
			{
				int count = 0;
				for(int i = 0; i<sessions.Count; i++)
					count += sessions[i].Duration;
				RealHours = count/60;
			}
			else RealHours = 0;			
			sg.P5 = RealHours.ToString();
			sg.P6 = PlanHours.ToString();
			List<Student> st = MTSystem.LoadStudents(StudentIds);
			
			if(st != null)
			{
				int count = 0;
				for(int i = 0; i<st.Count; i++)
				{
					if(st[i].checkGroupId(Id))
						count++;
				}	
				sg.P7 = count.ToString();
			}
			else sg.P7 = "0";
			return sg;
		}
	}
}
