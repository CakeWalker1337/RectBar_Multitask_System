/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 10/27/2017
 * Time: 20:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace RectBar_Multitask_System
{
	/// <summary>
	/// Description of MTSystem.
	/// </summary>
	public static class MTSystem
	{
		private static Mysql mysql; //Экземпляр для взаимодействия с БД
		private static List<Schedule> Schedules; //Список всех расписаний
		private static List<GroupType> GroupTypes; //Список всех типов групп
		private static List<StudentStatus> Statuses; //Список всех статусов учеников
		private static List<GroupLevel> GroupLevels; //Список всех уровней групп
		private static List<Teacher> Teachers; //Список всех преподавателей
		private static List<Group> Groups; //Список всех групп
		private static List<EventType> EventTypes; //Список всех типов достижений
		private static List<Hall> Halls; //Список всех залов

		 ////////////////////////////////////////////////PROPERTY METHODS///////////////////////////////////////////////////////////
		 
		public static Schedule getSchedule(int num)
		{
			return Schedules[num];
		}
		
		public static void addSchedule(Schedule gr)
		{
			Schedules.Add(gr);
		}
		
		public static bool deleteSchedule(Schedule gr)
		{
			return Schedules.Remove(gr);
		}
		
		public static int SchedulesCount
		{
			get{ return Schedules.Count;}
		}
		
		public static Schedule findScheduleById(int id) //Бинарный поиск
		{
			if(Schedules.Count == 0 || id == 0) return null;
			int l = 0, r = Schedules.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(Schedules[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(Schedules[l].Id != id) return null;
			return Schedules[l];
		}
		
		///////////////////////////////////////
		
		public static Hall getHall(int num)
		{
			return Halls[num];
		}
		
		public static void addHall(Hall gr)
		{
			Halls.Add(gr);
		}
		
		public static bool deleteHall(Hall gr)
		{
			return Halls.Remove(gr);
		}
		
		public static int HallsCount
		{
			get{ return Halls.Count;}
		}
		
		///////////////////////////////////////
		
		public static GroupType getGroupType(int num)
		{
			return GroupTypes[num];
		}
		
		public static void addGroupType(GroupType gr)
		{
			GroupTypes.Add(gr);
		}
		
		public static bool deleteGroupType(GroupType gr)
		{
			return GroupTypes.Remove(gr);
		}
		
		public static void ClearGroupTypes()
		{
			 GroupTypes.Clear();
		}
		
		public static int GroupTypesCount
		{
			get{ return GroupTypes.Count; }
		}
		
		public static GroupType findGroupTypeById(int id) //Бинарный поиск
		{
			if(GroupTypes.Count == 0 || id == 0) return new GroupType();
			int l = 0, r = GroupTypes.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(GroupTypes[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(GroupTypes[l].Id != id) return null;
			return GroupTypes[l];
		}
		
		public static GroupType findGroupTypeByName(String name)
		{
			for(int i = 0; i<GroupTypesCount; i++)
				if(getGroupType(i).Name == name) return getGroupType(i);
			return null;
		}
		
		///////////////////////////////////////
		
		public static StudentStatus getStudentStatus(int num)
		{
			return Statuses[num];
		}
		
		public static void addStudentStatus(StudentStatus gr)
		{
			Statuses.Add(gr);
		}
		
		public static bool deleteStudentStatus(StudentStatus gr)
		{
			return Statuses.Remove(gr);
		}
		
		public static int StudentStatusesCount
		{
			get{ return Statuses.Count; }
		}
		
		public static void ClearStudentStatuses()
		{
			 Statuses.Clear();
		}
		
		public static StudentStatus findStudentStatusById(int id) //Бинарный поиск
		{
			if(Statuses.Count == 0 || id == 0) return new StudentStatus();
			int l = 0, r = Statuses.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(Statuses[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(Statuses[l].Id != id) return null;
			return Statuses[l];
		}
		
		public static StudentStatus findStudentStatusByName(String name)
		{
			for(int i = 0; i<StudentStatusesCount; i++)
				if(getStudentStatus(i).Name == name) return getStudentStatus(i);
			return null;
		}
		
		///////////////////////////////////////
		
		public static GroupLevel getGroupLevel(int num)
		{
			return GroupLevels[num];
		}
		
		public static void addGroupLevel(GroupLevel gr)
		{
			GroupLevels.Add(gr);
		}
		
		public static bool deleteGroupLevel(GroupLevel gr)
		{
			return GroupLevels.Remove(gr);
		}
		
		public static int GroupLevelCount
		{
			get{ return GroupLevels.Count;}
		}
		
		public static void ClearGroupLevels()
		{
			 GroupLevels.Clear();
		}
		
		
		
		public static GroupLevel findGroupLevelById(int id) //Бинарный поиск
		{
			if(GroupLevels.Count == 0 || id == 0) return new GroupLevel();
			int l = 0, r = GroupLevels.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(GroupLevels[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(GroupLevels[l].Id != id) return null;
			return GroupLevels[l];
		}
		
		public static GroupLevel findGroupLevelByName(String name)
		{
			for(int i = 0; i<GroupLevelCount; i++)
				if(getGroupLevel(i).Name == name) return getGroupLevel(i);
			return null;
		}
		
		
		///////////////////////////////////////
		
		public static Teacher getTeacher(int num)
		{
			return Teachers[num];
		}
		
		public static void addTeacher(Teacher gr)
		{
			Teachers.Add(gr);
		}
		
		public static bool deleteTeacher(Teacher gr)
		{
			return Teachers.Remove(gr);
		}
		
		public static int TeachersCount
		{
			get{ return Teachers.Count;}
		}
		
		public static void ClearTeachers()
		{
			 GroupLevels.Clear();
		}
		
		public static Teacher findTeacherById(int id) //Бинарный поиск
		{
			if(Teachers.Count == 0) return null;
			int l = 0, r = Teachers.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(Teachers[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(Teachers[l].Id != id) return null;
			return Teachers[l];
		}
		
		public static List<Teacher> findTeachersByName(String name)
		{
			List<Teacher> lt = new List<Teacher>();
			for(int i = 0; i<TeachersCount; i++)
			{
				if(getTeacher(i).Name.Contains(name))
				{
					lt.Add(getTeacher(i));
				}
			}
			return lt;
		}
		
	
		///////////////////////////////////////
		
		public static Group getGroup(int index)
		{
			return Groups[index];
		}
		
		public static void addGroup(Group gr)
		{
			Groups.Add(gr);
		}
		
		public static bool deleteGroup(Group gr)
		{
			return Groups.Remove(gr);
		}
		
		public static int GroupsCount
		{
			get{ return Groups.Count;}
		}
		
		public static Group findGroupById(int id) //Бинарный поиск
		{
			if(Groups.Count == 0) return null;
			int l = 0, r = Groups.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(Groups[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(Groups[l].Id != id) return null;
			return Groups[l];
		}
		
		public static List<Group> findGroupsByName(String name)
		{
			List<Group> lt = new List<Group>();
			for(int i = 0; i<GroupsCount; i++)
			{
				if(getGroup(i).Name.Contains(name))
				{
					lt.Add(getGroup(i));
				}
			}
			return lt;
		}
		
		public static List<int> getGroupIdsList(int id, ObjectType ot)
		{
			List<int> ids = new List<int>();
			if(ot == ObjectType.Teacher)
			{
				for(int i = 0; i<Groups.Count; i++)
				{
					for(int j = 0; j<Groups[i].TeachersCount; j++)
					{
						if(Groups[i].getTeacherId(j) == id) ids.Add(Groups[i].Id);
					}
				}
				return ids;
			}
			if(ot == ObjectType.Student)
			{
				for(int i = 0; i<Groups.Count; i++)
				{
					for(int j = 0; j<Groups[i].StudentsCount; j++)
					{
						if(Groups[i].getStudentId(j) == id) ids.Add(Groups[i].Id);
					}
				}
				return ids;
			}
			return null;
		}
		
		//////////////////////////////////////
		
		public static EventType getEventType(int num)
		{
			return EventTypes[num];
		}
		
		public static void addEventType(EventType gr)
		{
			EventTypes.Add(gr);
		}
		
		public static bool deleteEventType(EventType gr)
		{
			return EventTypes.Remove(gr);
		}
		
		public static int EventTypesCount
		{
			get{ return EventTypes.Count; }
		}
		
		public static EventType findEventTypeById(int id) //Бинарный поиск
		{
			if(EventTypes.Count == 0) return null;
			int l = 0, r = EventTypes.Count-1, m = r/2;
			while(l<r)
			{
				m = (l+r)/2;
				if(EventTypes[m].Id < id)
					l = m+1;
				else 
					r = m;
			}
			if(EventTypes[l].Id != id) return null;
			return EventTypes[l];
		}
		
		public static EventType findEventTypeByName(String name)
		{
			for(int i = 0; i<EventTypesCount; i++)
				if(getEventType(i).Name == name) return getEventType(i);
			return null;
		}
		
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Метод для подключения к БД
        /// </summary>
        /// <returns>Успешность подключения</returns>
		public static bool ConnectBase()
		{
			mysql = new Mysql(LoadConnectionData());
			return mysql.isReady;
		}
        
        /// <summary>
        /// Инициализация системы
        /// </summary>
		public static void GetAllData()
		{
			Schedules = mysql.GetAllSchedules();
			Halls = mysql.GetAllHalls();
			mysql.GetAllScheduleRows();
			GroupTypes = mysql.GetGroupTypes();
			GroupLevels = mysql.GetGroupLevels();
			Statuses = mysql.GetStudentStatuses();
			EventTypes = mysql.GetAllEventTypes();
			Groups = mysql.GetAllGroups();
			Teachers = mysql.GetAllTeachers();
		}
		public static List<Teacher> GetAllTeachers(){return mysql.GetAllTeachers();}
		public static List<Admin> GetAllAdmins(){return mysql.GetAllAdmins();}
		public static List<Student> GetAllStudents(){return mysql.GetAllStudents();}
		
		
		/// <summary>
		/// Метод, позволяющий авторизироваться в системе
		/// </summary>
		/// <param name="login">Логин для авторизации</param>
		/// <param name="pass">Пароль для авторизации</param>
		/// <returns>0 - неверный логин,
		/// 1 - неверный пароль,
		/// 2 - успешно </returns>
		public static int LogIn(String login, String pass)
		{
			if(mysql.CheckLogin(login))
			{
				if(mysql.CheckPass(login, pass))
					return 2;
				return 1;
			}
			return 0;
		}
		
		
		public static bool CheckLogin(String login){return mysql.CheckLogin(login);}
		
		/// <summary>
		/// Метод, загружающий данные для подключения (имя БД, айпи сервера)
		/// </summary>
		/// <returns>
		/// 
		public static String[] LoadConnectionData()
		{
			String[] str = new string[2];
			if(File.Exists("connection.bin"))
			{
				using (BinaryReader reader = new BinaryReader(File.Open("connection.bin", FileMode.Open)))
	            {
					String buf;
					buf = reader.ReadString(); //db name
					if(buf == "NULL") str[0] = "";
					else str[0] = buf;
					
					buf = reader.ReadString(); //ip
					if(buf == "NULL") str[1] = "";
					else str[1] = buf;
					
					reader.Close();
	            }
			}
			else
			{
				using (BinaryWriter writer = new BinaryWriter(File.Open("connection.bin", FileMode.OpenOrCreate)))
		        {
		            writer.Write("NULL"); //ip
		            writer.Write("NULL"); //port
		            str[0] = "";
					str[1] = "";
					writer.Close();
		        }
			}
			return str;
		}
		
		/// <summary>
		/// Метод, сохраняющий данные для подключения (имя бд и айпи)
		/// </summary>
		/// <param name="data">Массив строк: [0] = имя бд, [1] - айпи</param>
		public static void SaveConnectionData(String[] data)
		{
			using (BinaryWriter writer = new BinaryWriter(File.Open("connection.bin", FileMode.Open)))
	        {
				writer.Write(data[0]); //db name
	            writer.Write(data[1]); //ip
	        }
		}
		public static List<User> LoadClients(String name, PermType dt){return mysql.LoadClients(name, dt);}
		public static User LoadClient(String login, PermType dt){return mysql.LoadClient(login, dt);} 
		public static User LoadClient(int id, PermType pt){return mysql.LoadClient(id, pt);}
		
		public static List<Student> LoadStudents(String name){return mysql.LoadStudents(name);} 
		public static Student LoadStudent(int id){return mysql.LoadStudent(id);}
		public static List<Student> LoadStudents(List<int> ids){return mysql.LoadStudents(ids);}
		public static List<Student> LoadStudentsWithAchievementId(int achId){return mysql.LoadStudentsWithAchievementId(achId);}
		
		public static List<Group> LoadGroups(String name){return mysql.LoadGroups(name);} 
		public static Group LoadGroup(int id){return mysql.LoadGroup(id);}
		
		
		public static bool SaveClient(User u){return mysql.SaveClient(u);}
		public static bool SaveGroup(Group u){return mysql.SaveGroup(u);}
		public static bool SaveStudent(Student u){return mysql.SaveStudent(u);}
		
		public static void CreateClient(User u){ mysql.CreateClient(u);}
		public static void CreateGroup(Group u){ mysql.CreateGroup(u);}
		public static void CreateStudent(Student u){ mysql.CreateStudent(u);}
		
		public static bool DeleteStudent(int id){return mysql.DeleteStudent(id);}
		public static bool DeleteGroup(int id){return mysql.DeleteGroup(id);}
		public static bool DeleteClient(int id){return mysql.DeleteUser(id);}
	
/*//////////////////////////////////////////////////////////////////////////////////*/
		
		public static void CreateStudentStatus(StudentStatus u){ mysql.CreateStudentStatus(u);}
		public static bool SaveStudentStatus(StudentStatus u){return mysql.SaveStudentStatus(u);}
		public static bool DeleteStudentStatus(int id){return mysql.DeleteStudentStatus(id);}
		
		public static void CreateGroupType(GroupType u){ mysql.CreateGroupType(u);}
		public static bool SaveGroupType(GroupType u){return mysql.SaveGroupType(u);}
		public static bool DeleteGroupType(int id){return mysql.DeleteGroupType(id);}
		
		public static void CreateGroupLevel(GroupLevel u){ mysql.CreateGroupLevel(u);}
		public static bool SaveGroupLevel(GroupLevel u){return mysql.SaveGroupLevel(u);}
		public static bool DeleteGroupLevel(int id){return mysql.DeleteGroupLevel(id);}
		
		public static void CreateEventType(EventType u){ mysql.CreateEventType(u);}
		public static bool SaveEventType(EventType u){return mysql.SaveEventType(u);}
		public static bool DeleteEventType(int id){return mysql.DeleteEventType(id);}
		
		public static void CreateSchedule(Schedule u){ mysql.CreateSchedule(u);}
		public static bool SaveSchedule(Schedule u){return mysql.SaveSchedule(u);}
		public static bool DeleteSchedule(Schedule sch){return mysql.DeleteSchedule(sch);}
		
		public static void CreateScheduleRow(ScheduleRow u){ mysql.CreateScheduleRow(u);}
		public static bool SaveScheduleRow(ScheduleRow u){return mysql.SaveScheduleRow(u);}
		public static bool DeleteScheduleRow(int id){return mysql.DeleteScheduleRow(id);}
		
		
		public static void CreateSession(Session u){ mysql.CreateSession(u);}
		public static bool SaveSession(Session u){return mysql.SaveSession(u);}
		public static bool DeleteSession(int id){return mysql.DeleteSession(id);}
		
		
		public static void CreateHall(Hall u){ mysql.CreateHall(u);}
		public static bool SaveHall(Hall u){return mysql.SaveHall(u);}
		public static bool DeleteHall(int id){return mysql.DeleteHall(id);}
		
		public static List<Session> LoadSessionsByGroupIdByTime(int id, DateTime s, DateTime e){return mysql.LoadSessionsByGroupIdByTime(id, s, e);}
		public static List<Session> LoadSessionsByTime(DateTime start, DateTime end){return mysql.LoadSessionsByTime(start, end);}
		public static Session LoadSessionById(int id){return mysql.LoadSessionById(id);}
		public static List<Session> LoadPresentSessionsByStudentId(int id){return mysql.LoadPresentSessionsByStudentId(id);}
/*//////////////////////////////////////////////////////////////////////////////////*/		
		
		public static int GetLastInsertId(String tablename){return mysql.GetLastInsertId(tablename);}
		
		/// <summary>
		/// Метод - конвертер строки в цвет
		/// </summary>
		/// <param name="str">строка с цветом</param>
		/// <returns>объект цвета</returns>
		public static Color GetColorFromString(String str)
		{
			Color color = new Color();
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}{1}", str[1], str[2]);
			color.A = Convert.ToByte(sb.ToString(), 16);
			sb.Clear();
			sb.AppendFormat("{0}{1}", str[3], str[4]);
			color.R = Convert.ToByte(sb.ToString(), 16);
			sb.Clear();
			sb.AppendFormat("{0}{1}", str[5], str[6]);
			color.G = Convert.ToByte(sb.ToString(), 16);
			sb.Clear();
			sb.AppendFormat("{0}{1}", str[7], str[8]);
			color.B = Convert.ToByte(sb.ToString(), 16);
			sb.Clear();
			
			return color;
		}
		
		/// <summary>
		/// Парсер айдишников из строки
		/// </summary>
		/// <param name="s">Строка для парсинга</param>
		/// <returns>Лист айдишников</returns>
		public static List<int> GetIdsFromString(String s)
		{
			List<int> l = new List<int>();
			if(s == "_") return l;
			String[] res = s.Split('_');
			
			for(int i = 1; i<(res.Length)-1; i++)
			{
				l.Add(Convert.ToInt32(res[i]));
			}
			return l;
		}
		
		/// <summary>
		/// Компоновщик айдишников в строку
		/// </summary>
		/// <param name="ids">Лист с айдишниками</param>
		/// <returns>Преобразованная строка</returns>
		public static String GetStringFromIds(List<int> ids)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("_");
			for(int i = 0; i<ids.Count; i++)
			{
				sb.AppendFormat("{0}_", ids[i]);
			}
			return sb.ToString();
		}
		
	}
}
