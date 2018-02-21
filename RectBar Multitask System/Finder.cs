/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 12/01/2017
 * Time: 15:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;

namespace RectBar_Multitask_System
{
	
	/// <summary>
	/// Класс поиска данных в системе по вводимым данным
	/// </summary>
	public class Finder
	{	
		/// <summary>
		/// Метод, осуществляющая поиск преподавателей в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными. Данные могут означать как имя (часть имени) преподавателя, так и его id в БД</param>
		/// <returns>Лист преподавателей. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<Teacher> FindTeachers(String data)
		{
			int res = 0;
			List<Teacher> t = new List<Teacher>();
			if(Int32.TryParse(data, out res))
			{
				t.Add(MTSystem.findTeacherById(res));
				return t;
			}
			t = MTSystem.findTeachersByName(data);
			if(t.Count == 0) t.Add(null);
			return t;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск администраторов в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными. Данные могут означать как имя (часть имени) админа, так и его id в БД</param>
		/// <returns>Лист администраторов. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<Admin> FindAdmins(String data)
		{
			int res = 0;
			List<Admin> t = new List<Admin>();
			if(Int32.TryParse(data, out res))
			{
				User u = MTSystem.LoadClient(res, PermType.Admin);
				if(u != null) 
					t.Add(u.toAdmin());
				else t.Add(null);
				return t;
			}
			List<User> users = MTSystem.LoadClients(data, PermType.Admin);
			for(int i = 0; i<users.Count; i++)
			{
				if(users[i] != null)
					t.Add(users[i].toAdmin());
				else t.Add(null);
			}
			return t;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск учеников в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными. Данные могут означать как имя (часть имени) ученика, так и его id в БД</param>
		/// <returns>Лист учеников. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<Student> FindStudents(String data)
		{
			int res = 0;
			
			if(Int32.TryParse(data, out res))
			{
				List<Student> st = new List<Student>();
				st.Add(MTSystem.LoadStudent(res));
				return st;
			}
			return MTSystem.LoadStudents(data);
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск групп в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными. Данные могут означать как имя (часть имени) группы, так и ее id в БД</param>
		/// <returns>Лист групп. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<Group> FindGroups(String data)
		{
			int res = 0;
			List<Group> lg = new List<Group>();
			if(Int32.TryParse(data, out res))
			{
				lg.Add(MTSystem.findGroupById(res));
				return lg;
			}
			lg = MTSystem.findGroupsByName(data);
			if(lg.Count == 0) lg.Add(null);
			return lg;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск типов достижений в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными (название или часть названия достижения).</param>
		/// <returns>Лист типов достижений. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<EventType> FindEventTypes(String data)
		{
			List<EventType> lg = new List<EventType>();
			for(int i = 0; i<MTSystem.EventTypesCount; i++)
			{
				if(MTSystem.getEventType(i).Name.Contains(data))
					lg.Add(MTSystem.getEventType(i));
			}
			if(lg.Count == 0) lg.Add(null);
			return lg;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск типов групп в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными (название или часть названия типа группы).</param>
		/// <returns>Лист типов групп. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<GroupType> FindGroupTypes(String data)
		{
			List<GroupType> lg = new List<GroupType>();
			for(int i = 0; i<MTSystem.GroupTypesCount; i++)
			{
				if(MTSystem.getGroupType(i).Name.Contains(data))
					lg.Add(MTSystem.getGroupType(i));
			}
			if(lg.Count == 0) lg.Add(null);
			return lg;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск уровней групп в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными (название или часть названия уровня группы).</param>
		/// <returns>Лист уровней групп. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<GroupLevel> FindGroupLevels(String data)
		{
			List<GroupLevel> lg = new List<GroupLevel>();
			for(int i = 0; i<MTSystem.GroupLevelCount; i++)
			{
				if(MTSystem.getGroupLevel(i).Name.Contains(data))
					lg.Add(MTSystem.getGroupLevel(i));
			}
			if(lg.Count == 0) lg.Add(null);
			return lg;
		}
		
		/// <summary>
		/// Метод, осуществляющая поиск статусов учеников в системе по вводимым данным
		/// </summary>
		/// <param name="data">Строка с вводимыми данными (название или часть названия статуса ученика).</param>
		/// <returns>Лист статусов учеников. Если их нет, то первый элемент листа будет равен null</returns>
		public static List<StudentStatus> FindStudentStatuses(String data)
		{
			List<StudentStatus> lg = new List<StudentStatus>();
			for(int i = 0; i<MTSystem.StudentStatusesCount; i++)
			{
				if(MTSystem.getStudentStatus(i).Name.Contains(data))
					lg.Add(MTSystem.getStudentStatus(i));
			}
			if(lg.Count == 0) lg.Add(null);
			return lg;
		}
	}
}
