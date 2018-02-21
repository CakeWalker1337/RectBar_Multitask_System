/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 23.01.2017
 * Time: 18:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data.Common;
using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;
using RectBar_Multitask_System;


namespace RectBar_Multitask_System
{
	
	/// <summary>
	/// Класс для отправки запросов на сервер MySQL.
	/// </summary>
	public class Mysql
	{
		public bool isReady = false; // Готовность БД
		private string Connect; //Строка для формирования запросов
		private MySqlConnection myConnection; // Переменная для соединения
		private MySqlCommand myCommand; // Переменная содержащая инструкции для отправки комманд на сервер
		String[] dayNames = {"mondayrows", "tuesdayrows", "wednesdayrows", "thursdayrows", "fridayrows", "saturdayrows", "sundayrows"};
		//Массив названий таблиц, содержащих расписание на дни недели
		public Mysql(String[] data)
		{
			bool Success = false;
			try
			{
				String[] secretdata = GetPrivateData();
				if(secretdata == null)
				{
					isReady = false;
					return;
				}
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Database={0};Data Source={1};User Id={2};Password={3};", data[0], data[1], secretdata[0], secretdata[1]);
				Connect = sb.ToString();
				myConnection = new MySqlConnection(Connect);
				myConnection.Open();
				Success = true;
			}
			catch(MySqlException e)
			{
				MessageBox.Show("Ошибка при подключении к MySQL серверу!" + Environment.NewLine + e);
			}
			if(Success)
			{
				try
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("delete from sessions where time < '{0}';", DateTime.Today.AddDays(-180).ToString("yyyy-MM-dd HH:mm:ss"));
					myCommand =  new MySqlCommand(sb.ToString(), myConnection);
					myCommand.ExecuteNonQuery();
				}
				catch(MySqlException e)
				{
					Success = false;
					MessageBox.Show("Ошибка: Таблицы не созданы!" + Environment.NewLine + e);
				}
				isReady = Success;
			}
		}
		
		/// <summary>
		/// Метод, получающий приватные данные о БД
		/// </summary>
		/// <returns>Логин и пароль от БД в массиве строк ([0]-пароль, [1]-логин)</returns>
		public String[] GetPrivateData()
		{
			try
			{
				if(File.Exists("private.bin"))
				{
					using (BinaryReader reader = new BinaryReader(File.Open("private.bin", FileMode.Open)))
		            {
						MessageBox.Show("READ");
						String[] str = new string[2];
						
						str[1] = reader.ReadString(); 
						MessageBox.Show(str[1]);
						str[0] = reader.ReadString();
						MessageBox.Show(str[0]);
						reader.Close();
						return str;
		            }
				}
				else
				{
					MessageBox.Show("Файл private.bin не найден!");
					return null;
				}
				
				
			}
			catch(IOException e)
			{
				MessageBox.Show(e.ToString());
				return null;
			}
			
		}
		
		/*///////////////////////////////////////////////////////////////////////////////USERS/////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод получения пользователей из БД, соответствующих введенному имени или его части
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		/// <param name="pt">Тип привилегии</param>
		/// <returns>Список пользователей</returns>
		public List<User> LoadClients(String name, PermType pt)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `users` where `name` like '%{0}%'", name);	
			if(pt != PermType.All)
			{
				str.AppendFormat(" and `type` = '{0}';", (int)pt);
			}
			else str.Append(";");
			List<User> l = GetUser(str.ToString());
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Метод получения пользователя из БД по логину
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="pt">Тип привилегии</param>
		/// <returns>Искомый пользователь</returns>
		public User LoadClient(String login, PermType pt)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `users` where `login` = '{0}'", login);
			if(pt != PermType.All)
			{
				str.AppendFormat(" and `type` = '{0}';", (int)pt);
			}
			else str.Append(";");
			List<User> l = GetUser(str.ToString());
			if(l.Count == 0) return null;
			return l[0];
		}
		
		/// <summary>
		/// Метод получения пользователя из БД по айди
		/// </summary>
		/// <param name="id">айди пользователя</param>
		/// <param name="pt">Тип привилегии</param>
		/// <returns>Искомый пользователь</returns>
		public User LoadClient(Int32 id, PermType pt)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `users` where `id` = '{0}'", id);
			if(pt != PermType.All)
			{
				str.AppendFormat(" and `type` = '{0}';", (int)pt);
			}
			else str.Append(";");
			List<User> l = GetUser(str.ToString());
			if(l.Count == 0) return null;
			return l[0];
		}
		
		/// <summary>
		/// Непосредственно метод получения пользователя из БД по запросу
		/// </summary>
		/// <param name="query">Строка запроса</param>
		/// <returns>Список пользователей по запросу</returns>
		private List<User> GetUser(String query)
		{
			List<User> u = new List<User>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(query, myConnection);
				datareader = myCommand.ExecuteReader();

				if(datareader.Read())
				{
					User user = new User();
					user.Id = datareader.GetInt32(0);
					user.Login = datareader.GetString(1);
					user.Pass = datareader.GetString(2);;
					user.Name = datareader.GetString(3);			
					user.Type = (PermType)datareader.GetInt32(4);
					u.Add(user);
				}		
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}
			return u;			
		}
		
		
		/// <summary>
		/// Метод создания пользователя по данным
		/// </summary>
		/// <param name="user">Данные для создания пользователя</param>
		/// <returns>Успешность операции</returns>
		public bool CreateClient(User user)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `users` (`login`, `pass`, `name`, `type`) VALUES ('{0}', '{1}', '{2}', '{3}');", user.Login, user.Pass, user.Name, (int)user.Type);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод сохранения пользователя в БД
		/// </summary>
		/// <param name="user">Объект с данными для сохранения в БД</param>
		/// <returns>True, если сохранение прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveClient(User user)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `users` SET `login` = '{0}', `pass` = '{1}', `name` = '{2}', `type` = '{3}' where `id` = '{4}';", user.Login, user.Pass, user.Name, (int)user.Type, user.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Проверяет, есть ли логин в БД
		/// </summary>
		/// <param name="sLogin">Логин для проверки</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckLogin(String sLogin)
		{
			if(!IsLoginCorrect(sLogin)) return false;
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id` from `users` where `login` = '{0}';", sLogin);
			MySqlDataReader datareader = null;
			
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				bool res = datareader.Read();
				datareader.Close();
				return res;
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
				return false;
			}			
		}
		
		/// <summary>
		/// Проверяет логин на соответствие формату a-z, 0-9
		/// </summary>
		/// <param name="login">Логин для проверки</param>
		/// <returns>True - корректный, иначе false </returns>
		public bool IsLoginCorrect(String login)
		{
			if(login.Length == 0) return false;
			for(int i = 0; i<login.Length; i++)
			{
				if((login[i]<'A' || login[i]>='Z') && (login[i]<'a' || login[i]>='z') && (login[i]<'0' || login[i]>'9')) return false;
			}
			return true;
		}
		
		/// <summary>
		/// Проверяет, есть ли пароль в БД
		/// </summary>
		/// <param name="sLogin">Логин для проверки</param>
		/// <param name="sPass">Пароль для проверки в связке с логином</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckPass(String sLogin, String sPass)
		{
			if(!IsLoginCorrect(sLogin)) return false;
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `pass` from `users` where `login` = '{0}';", sLogin);
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				if(datareader.Read() && sPass == datareader.GetString(0))
				{
					datareader.Close();
					return true;
				}
				datareader.Close();
				
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}		
			return false;			
		}
		
		/// <summary>
		/// Удаляет пользователя из БД по айди
		/// </summary>
		/// <param name="id">Айди для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteUser(Int32 id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `users` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет пользователя из БД по логину
		/// </summary>
		/// <param name="login">Логин для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteUser(String login)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `users` where `login` = '{0}';", login);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}

			/*	//////////////////////////////////////////////////////////////////////////GROUPS///////////////////////////////*/

		/// <summary>
		/// Метод для получения всех групп из БД
		/// </summary>
		/// <returns>Лист всех групп</returns>
		public List<Group> GetAllGroups()
		{
			List<Group> groups = new List<Group>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `groups`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Group gr = new Group();
					gr.Id = datareader.GetInt32(0);
					gr.Name = datareader.GetString(1);
					gr.Type = MTSystem.findGroupTypeById(datareader.GetInt16(2));
					gr.Level = MTSystem.findGroupLevelById(datareader.GetInt16(3));
					gr.PlanHours = datareader.GetInt32(4);
					gr.Color.Color = MTSystem.GetColorFromString(datareader.GetString(5));
					gr.addTeacherIdsList(MTSystem.GetIdsFromString(datareader.GetString(6)));
					gr.addStudentIdsList(MTSystem.GetIdsFromString(datareader.GetString(7)));
					groups.Add(gr);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return groups;
		}
		
		/// <summary>
		/// Метод получения группы из БД по айди
		/// </summary>
		/// <param name="id">айди группы</param>
		/// <returns>Искомая группа</returns>
		public Group LoadGroup(int id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `groups` where `id` = '{0}';", id);
			List<Group> l = GetGroups(str);
			if(l.Count == 0) l.Add(null);
			return l[0];
		}
		
		/// <summary>
		/// Метод получения групп из БД по названию или его части
		/// </summary>
		/// <param name="name">название группы</param>
		/// <returns>Искомая группа</returns>
		public List<Group> LoadGroups(String name)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `groups` where `name` like ('%{0}%');", name);
			List<Group> l = GetGroups(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Непосредственно метод получения групп из БД по запросу
		/// </summary>
		/// <param name="str">Объект StringBuilder запроса</param>
		/// <returns>Список групп по запросу</returns>
		public List<Group> GetGroups(StringBuilder str)
		{
			List<Group> groups = new List<Group>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					Group gr = new Group();
				 	gr.Id = datareader.GetInt32(0);
					gr.Name = datareader.GetString(1);
					gr.Type = MTSystem.findGroupTypeById(datareader.GetInt32(2));
					gr.Level = MTSystem.findGroupLevelById(datareader.GetInt32(3));
					gr.PlanHours = datareader.GetInt32(4);
					
					gr.Color.Color = MTSystem.GetColorFromString(datareader.GetString(5));
					gr.addTeacherIdsList(MTSystem.GetIdsFromString(datareader.GetString(6)));
					gr.addStudentIdsList(MTSystem.GetIdsFromString(datareader.GetString(7)));
					groups.Add(gr);
				}				
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return groups;	
		}
		
		/// <summary>
		/// Сохраняет группу в БД по объекту с данными
		/// </summary>
		/// <param name="gr">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveGroup(Group gr)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `groups` SET `name` = '{0}', `type` = '{1}', `level` = '{2}', `planhours` = '{3}', `color` = '{4}', `teachers` = '{5}', `students` = '{6}' where `id` = '{7}';",
			                 gr.Name, gr.Type.Id, gr.Level.Id, gr.PlanHours, gr.Color.Color, gr.GetStringFromTeacherIds(), gr.GetStringFromStudentIds(), gr.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий группу в БД по объекту
		/// </summary>
		/// <param name="gr">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateGroup(Group gr)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `groups` (`name`, `type`, `level`, `planhours`, `color`, `teachers`, `students`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
			                 gr.Name, gr.Type.Id, gr.Level.Id, gr.PlanHours, gr.Color.Color, gr.GetStringFromTeacherIds(), gr.GetStringFromStudentIds());
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет группу из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteGroup(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `groups` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/*///////////////////////////////////////////////////////////////////////////////TEACHERS/////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод, получающий все объекты преподавателей из БД
		/// </summary>
		/// <returns>Лист преподавателей</returns>
		public List<Teacher> GetAllTeachers()
		{
			List<Teacher> teachers = new List<Teacher>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT `id`, `login`, `pass`, `name` from `users` where `type` = '1';", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Teacher teacher = new Teacher();
					teacher.Id = datareader.GetInt32(0);
					teacher.Login = datareader.GetString(1);
					teacher.Pass = datareader.GetString(2);
					teacher.Name = datareader.GetString(3);
					if(MTSystem.getGroupIdsList(teacher.Id, ObjectType.Teacher) == null) MessageBox.Show("NUL");
					teacher.addGroupIdsList(MTSystem.getGroupIdsList(teacher.Id, ObjectType.Teacher));
					teachers.Add(teacher);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return teachers;
		}

/*//////////////////////////////////////////////////////////////STUDENTS///////////////////////////////////////////////////////////////////////*/


		/// <summary>
		/// Метод получения учеников из БД по списку айди
		/// </summary>
		/// <param name="studentIds">список айди учеников</param>
		/// <returns>Список учеников</returns>
		public List<Student> LoadStudents(List<int> studentIds)
		{
			if(studentIds.Count == 0) return null;
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `students` where `id` in ({0}", studentIds[0]);
			for(int i = 1; i<studentIds.Count; i++)
			{
				str.AppendFormat(", {0}", studentIds[i]);
			}
			str.AppendFormat(");");
			List<Student> result = new List<Student>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Student st = new Student();
					st.Id = datareader.GetInt32(0);
					st.FullName = datareader.GetString(1);
					st.Age = datareader.GetInt32(2);;
					st.Status = MTSystem.findStudentStatusById(datareader.GetInt32(3));
					st.addGroupIdsList(MTSystem.GetIdsFromString(datareader.GetString(4)));
					st.addAchievementsList(Achievement.GetAchievementsFromString(datareader.GetString(5)));
					result.Add(st);
				}		
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return result;	
		}
		
		/// <summary>
		/// Метод, получающий все объекты учеников из БД
		/// </summary>
		/// <returns>Лист учеников</returns>
		public List<Student> GetAllStudents()
		{
			List<Student> result = new List<Student>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `students`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Student st = new Student();
					st.Id = datareader.GetInt32(0);
					st.FullName = datareader.GetString(1);
					st.Age = datareader.GetInt32(2);;
					st.Status = MTSystem.findStudentStatusById(datareader.GetInt32(3));
					st.addGroupIdsList(MTSystem.GetIdsFromString(datareader.GetString(4)));
					st.addAchievementsList(Achievement.GetAchievementsFromString(datareader.GetString(5)));
					result.Add(st);
				}		
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return result;	
		}
		
		/// <summary>
		/// Получает ученика из БД по айди
		/// </summary>
		/// <param name="id">Айди для загрузки</param>
		/// <returns>Запрашиваемый объект</returns>
		public Student LoadStudent(int id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `students` where `id` = '{0}';", id);
			List<Student> l = GetStudent(str);
			if(l.Count != 0) return l[0];
			return null;
		}
		
		/// <summary>
		/// Получает учеников из БД по имени или его части
		/// </summary>
		/// <param name="name">Имя или его часть для загрузки</param>
		/// <returns>Лист учеников по запросу</returns>
		public List<Student> LoadStudents(String name)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `students` where `name` like ('%{0}%');", name);
			List<Student> l = GetStudent(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Получает учеников из БД по айди достижения
		/// </summary>
		/// <param name="Id">Айди достижения для загрузки</param>
		/// <returns>Лист учеников по запросу</returns>
		public List<Student> LoadStudentsWithAchievementId(int Id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `students` where `achievements` like ('%_{0}-%');", Id);
			List<Student> l = GetStudent(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Непосредственно получает учеников из БД по запросу
		/// </summary>
		/// <param name="str">строка запроса</param>
		/// <returns>Лист учеников по запросу</returns>
		public List<Student> GetStudent(StringBuilder str)
		{
			List<Student> s = new List<Student>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
				 	Student st = new Student();
					st.Id = datareader.GetInt32(0);
					st.FullName = datareader.GetString(1);
					st.Age = datareader.GetInt32(2);;
					st.Status = MTSystem.findStudentStatusById(datareader.GetInt32(3));
					st.addGroupIdsList(MTSystem.GetIdsFromString(datareader.GetString(4)));
					st.addAchievementsList(Achievement.GetAchievementsFromString(datareader.GetString(5)));
					s.Add(st);
				}		
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return s;	
		}
		
		/// <summary>
		/// Сохраняет ученика в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveStudent(Student st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `students` SET `name` = '{0}', `age` = '{1}', `status` = '{2}', `groupids` = '{3}', `achievements` = '{4}' where `id` = '{5}';",
			                 st.FullName, st.Age, st.Status.Id, st.GetStringFromGroupIds(), st.GetStringFromAchievements(), st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий ученика в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateStudent(Student st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `students` (`name`, `age`, `status`, `groupids`, `achievements`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
			                  st.FullName, st.Age, st.Status.Id, st.GetStringFromGroupIds(), st.GetStringFromAchievements());
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет ученика из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteStudent(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `students` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
/*//////////////////////////////////////////////////////////////HALLS/////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод, получающий все объекты залов из БД
		/// </summary>
		/// <returns>Лист залов</returns>
		public List<Hall> GetAllHalls()
		{
			List<Hall> schedules = new List<Hall>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `halls`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Hall sch = new Hall();
					sch.Id = datareader.GetInt32(0);
					sch.Name = datareader.GetString(1);
					schedules.Add(sch);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return schedules;
		}
		
		/// <summary>
		/// Сохраняет зал в БД по объекту с данными
		/// </summary>
		/// <param name="h">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveHall(Hall h)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `halls` SET `name` = '{0}' where `id` = '{1}';", h.Name,  h.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий зал в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateHall(Hall st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `halls` (`name`) VALUES ('{0}');", st.Name);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				
				int id = GetLastInsertId("halls");
				for(int i = 0; i<7; i++)
				{
					str.Clear();
					str.AppendFormat("ALTER TABLE `{0}` add `hall{1}` integer default '0';",dayNames[i], id);
					myCommand.CommandText = str.ToString();
					myCommand.ExecuteNonQuery();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет зал из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteHall(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `halls` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				for(int i = 0; i<7; i++)
				{
					str.Clear();
					str.AppendFormat("ALTER TABLE `{0}` drop column `hall{1}`;", dayNames[i], id);
					myCommand.CommandText = str.ToString();
					myCommand.ExecuteNonQuery();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
/*//////////////////////////////////////////////////////////////SCHEDULES///////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод, получающий все объекты расписаний из БД
		/// </summary>
		/// <returns>Лист расписаний</returns>
		public List<Schedule> GetAllSchedules()
		{
			
			List<Schedule> schedules = new List<Schedule>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `schedules`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Schedule sch = new Schedule();
					sch.Id = datareader.GetInt32(0);
					sch.ScheduleName = datareader.GetString(1);
					schedules.Add(sch);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return schedules;
				
		}

		
		/// <summary>
		/// Сохраняет расписание в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveSchedule(Schedule st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `schedules` SET `name` = '{0}' where `id` = '{1}';", st.ScheduleName,  st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				for(int i = 0; i<st.RowCount; i++)
				{
					SaveScheduleRow(st.getRow(i));
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий расписание в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateSchedule(Schedule st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `schedules` (`name`) VALUES ('{0}');", st.ScheduleName);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет расписание из БД (вместе с удалением данных о расписании из других таблиц)
		/// </summary>
		/// <param name="sch">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteSchedule(Schedule sch)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `schedules` where `id` = '{0}';", sch.Id);
			try
			{
				
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				for(int i = 0; i<sch.RowCount; i++)
				{
					for(int k = 0; k<7; k++)
					{
						str.Clear();
						str.AppendFormat("DELETE from `{0}` where `rowid` = '{1}';", dayNames[k], sch.getRow(i).Id);
						myCommand.CommandText = str.ToString();
						myCommand.ExecuteNonQuery();
					}
				}
				
				str.Clear();
				str.AppendFormat("DELETE from `rows` where `scheduleid` = '{0}';", sch.Id);
				myCommand.CommandText = str.ToString();
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
/*//////////////////////////////////*/		
		
		/// <summary>
		/// Метод, получающий все объекты строк расписаний из БД.
		/// Сохраняет их сразу в статический лист данных.
		/// </summary>
		public void GetAllScheduleRows()
		{
			MySqlDataReader datareader = null;
			
			StringBuilder str = new StringBuilder();
			List<ScheduleRow> rows = new List<ScheduleRow>();
			try
			{
				str.AppendFormat("SELECT * from `rows` order by `time`;");
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
			
				while(datareader.Read())
				{
					ScheduleRow sch = new ScheduleRow();
					sch.Id = datareader.GetInt32(0);
					sch.Time = datareader.GetTimeSpan(1);
					sch.ScheduleId = datareader.GetInt32(2);
					rows.Add(sch);
				}
				datareader.Close();
				
				for(int i = 0; i<7; i++)
				{
					str.Clear();
					str.AppendFormat("SELECT * from `{0}`;", dayNames[i]);
					myCommand.CommandText = str.ToString();
					datareader = myCommand.ExecuteReader();
					int id = 0;
					
					while(datareader.Read())
					{			
						id = datareader.GetInt32(0);
						for(int j = 1; j<datareader.FieldCount; j++)
							rows.Find(x => x.Id == id).GroupIds.Add(datareader.GetInt32(j));
					}
					datareader.Close();
				}
				
				for(int j = 0; j<rows.Count; j++)
				{			
					MTSystem.findScheduleById(rows[j].ScheduleId).addRow(rows[j]);
				}
				
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
				
		}
		
		/// <summary>
		/// Сохраняет строку расписания в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveScheduleRow(ScheduleRow st)
		{
			try
			{
				StringBuilder str = new StringBuilder();	
				str.AppendFormat("UPDATE `rows` SET `time` = '{0}' where `id` = '{1}';", st.Time, st.Id);
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				int count = 0;
				for(int i = 0; i<7; i++)
				{
					DayData dd = new DayData();
					dd.RowId = st.Id;
					for(int j = 0; j<MTSystem.HallsCount; j++)
					{
						dd.addGroupId(st.GroupIds[count]);
						count++;
					}
					if(!SaveDayData(dd, dayNames[i]))
					{
						return false;
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		
		///	<summary>
		/// Сохраняет часть строки расписания, привязанную к конкретному дню, в БД по объекту с данными
		/// </summary>
		/// <param name="dd">Данные для сохранения</param>
		/// /// <param name="dayTableName">Название таблицы дня</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveDayData(DayData dd, String dayTableName)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `{0}` SET `hall{1}` = '{2}'", dayTableName, MTSystem.getHall(0).Id, dd.getGroupId(0));
			for(int i = 1; i<dd.GroupIdsCount; i++)
			{
				str.AppendFormat(", `hall{0}` = '{1}'", MTSystem.getHall(i).Id, dd.getGroupId(i));
			}
			str.AppendFormat(" where `rowid` = {0};", dd.RowId);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий строку расписания в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateScheduleRow(ScheduleRow st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `rows` (`scheduleid`, `time`) values ('{0}', '{1}');", st.ScheduleId,  st.Time);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				
				int id = GetLastInsertId("rows");
				for(int i = 0; i<7; i++)
				{
					str.Clear();
					str.AppendFormat("INSERT INTO `{0}` (`rowid`) values ('{1}');", dayNames[i], id);
					myCommand.CommandText = str.ToString();
					myCommand.ExecuteNonQuery();
				}				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет строку расписания из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteScheduleRow(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `rows` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				for(int i = 0; i<7; i++)
				{
					str.Clear();
					str.AppendFormat("DELETE from `{0}` where `rowid` = '{1}';", dayNames[i], id);
					myCommand.CommandText = str.ToString();
					myCommand.ExecuteNonQuery();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		
		
//////////////////////////////////////////////////////TYPES//////////////////////////////////////////////////////////////////////		
		
		/// <summary>
		/// Метод, получающий все объекты типов достижения из БД
		/// </summary>
		/// <returns>Лист преподавателей</returns>
		public List<EventType> GetAllEventTypes()
		{
			List<EventType> ach = new List<EventType>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `eventtypes`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					EventType a = new EventType();
					a.Id = datareader.GetInt32(0);
					a.Name = datareader.GetString(1);
					ach.Add(a);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return ach;
		}
		
		/// <summary>
		/// Сохраняет тип достижения в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveEventType(EventType st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `eventtypes` SET `name` = '{0}' where `id` = '{1}';", st.Name,  st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий тип достижения в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateEventType(EventType st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `eventtypes` (`name`) VALUES ('{0}');", st.Name);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет тип достижения из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteEventType(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `eventtypes` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, получающий все объекты уровней групп из БД
		/// </summary>
		/// <returns>Лист уровней групп</returns>
		public List<GroupLevel> GetGroupLevels()
		{
			List<GroupLevel> types = new List<GroupLevel>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `grouplevels`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					GroupLevel type = new GroupLevel();
					type.Id = datareader.GetInt32(0);
					type.Name = datareader.GetString(1);
					types.Add(type);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return types;
		}
		
		/// <summary>
		/// Сохраняет тип группы в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveGroupLevel(GroupLevel st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `grouplevels` SET `name` = '{0}' where `id` = '{1}';", st.Name,  st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий уровень группы в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateGroupLevel(GroupLevel st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `grouplevels` (`name`) VALUES ('{0}');", st.Name);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет тип группы из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteGroupLevel(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `grouplevels` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				
				str.Clear();
				str.AppendFormat("UPDATE `groups` SET `level` = '0' where `level` = '{0}';", id);
				myCommand.CommandText = str.ToString();
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, получающий все объекты типов групп из БД
		/// </summary>
		/// <returns>Лист типов групп</returns>
		public List<GroupType> GetGroupTypes()
		{
			List<GroupType> types = new List<GroupType>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `grouptypes`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					GroupType type = new GroupType();
					type.Id = datareader.GetInt32(0);
					type.Name = datareader.GetString(1);
					types.Add(type);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return types;
		}

		/// <summary>
		/// Сохраняет тип группы в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveGroupType(GroupType st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `grouptypes` SET `name` = '{0}' where `id` = '{1}';", st.Name,  st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий тип группы в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateGroupType(GroupType st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `grouptypes` (`name`) VALUES ('{0}');", st.Name);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет тип группы из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteGroupType(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `grouptypes` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				
				str.Clear();
				str.AppendFormat("UPDATE `groups` SET `type` = '0' where `type` = '{0}';", id);
				myCommand.CommandText = str.ToString();
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, получающий все объекты статусов учеников из БД
		/// </summary>
		/// <returns>Лист статусов учеников</returns>
		public List<StudentStatus> GetStudentStatuses()
		{
			List<StudentStatus> types = new List<StudentStatus>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `statuses`;", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					StudentStatus type = new StudentStatus();
					type.Id = datareader.GetInt32(0);
					type.Name = datareader.GetString(1);
					types.Add(type);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return types;
		}
		
		/// <summary>
		/// Сохраняет статус ученика в БД по объекту с данными
		/// </summary>
		/// <param name="st">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveStudentStatus(StudentStatus st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `statuses` SET `name` = '{0}' where `id` = '{1}';", st.Name,  st.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод, создающий статус ученика в БД по объекту
		/// </summary>
		/// <param name="st">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateStudentStatus(StudentStatus st)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `statuses` (`name`) VALUES ('{0}');", st.Name);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет статус студента из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteStudentStatus(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `statuses` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
				str.Clear();
				str.AppendFormat("UPDATE `students` SET `status` = '0' where `status` = '{0}';", id);
				myCommand.CommandText = str.ToString();
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
/*//////////////////////////////////////////////ADMINS////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод, получающий все объекты администраторов из БД
		/// </summary>
		/// <returns>Лист администраторов</returns>
		public List<Admin> GetAllAdmins()
		{
			List<Admin> admins = new List<Admin>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand("SELECT * from `users` where `type` = '2';", myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Admin admin = new Admin();
					admin.Id = datareader.GetInt32(0);
					admin.Login = datareader.GetString(1);
					admin.Pass = datareader.GetString(2);
					admin.Name = datareader.GetString(3);
					admins.Add(admin);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return admins;
		
		}
/*////////////////////////////////////////////////VISIT LOG/////////////////////////////////////////////////////*/
/// 
		/// <summary>
		/// Метод, создающий сессию в БД по объекту
		/// </summary>
		/// <param name="s">Объект с данными для создания</param>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CreateSession(Session s)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `sessions` (`time`, `duration`, `groupid`, `userid`, `studentscount`, `present`, `absent`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
			                 s.Time.ToString("yyyy-MM-dd HH:mm:ss"), s.Duration, s.GroupId, s.UserId, s.StudentsCount, s.GetStringFromPresentIds(), s.GetStringFromAbsentIds());
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Сохраняет сессию в БД по объекту с данными
		/// </summary>
		/// <param name="s">Объект для сохранения</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveSession(Session s)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `sessions` SET `time` = '{0}', `duration` = '{1}', `groupid` = '{2}', `userid` = '{3}', `studentscount` = '{4}', `present` = '{5}', `absent` = '{6}' where `id` = '{7}';",
			                 s.Time.ToString("yyyy-MM-dd HH:mm:ss"), s.Duration, s.GroupId, s.UserId,  s.StudentsCount, s.GetStringFromPresentIds(), s.GetStringFromAbsentIds(), s.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет сессию из БД по id
		/// </summary>
		/// <param name="id">id для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteSession(int id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `sessions` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Получает сессии из БД по айди группы и периоду времени
		/// </summary>
		/// <param name="Id">Айди группы</param>
		/// <param name="start">Время начала периода</param>
		/// <param name="end">Время конца периода</param>
		/// <returns>Список сессий по запросу</returns>
		public List<Session> LoadSessionsByGroupIdByTime(int Id, DateTime start, DateTime end)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `sessions` where `groupid` = '{0}' and `time` >= '{1}' and `time` <= '{2}';",
			                 Id, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"));
			List<Session> l = GetSessions(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Получает сессии, на которых присутствовал ученик, из БД по айди ученика
		/// </summary>
		/// <param name="Id">Айди ученика</param>
		/// <returns>Список сессий по запросу</returns>
		public List<Session> LoadPresentSessionsByStudentId(int Id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `sessions` where `present` like '%_{0}_%';", Id);
			List<Session> l = GetSessions(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Получает сессии из БД по периоду времени
		/// </summary>
		/// <param name="start">Время начала периода</param>
		/// <param name="end">Время конца периода</param>
		/// <returns>Список сессий по запросу</returns>
		public List<Session> LoadSessionsByTime(DateTime start, DateTime end)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `sessions` where `time` >= '{0}' and `time` <= '{1}';", start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"));
			List<Session> l = GetSessions(str);
			if(l.Count == 0) l.Add(null);
			return l;
		}
		
		/// <summary>
		/// Получает сессию из БД по айди
		/// </summary>
		/// <param name="id">Айди сессии</param>
		/// <returns>Запрашиваемую сессию</returns>
		public Session LoadSessionById(int id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `sessions` where `id` = '{0}';", id);
			List<Session> l = GetSessions(str);
			if(l.Count == 0) return null;
			return l[0];
		}
		
		/// <summary>
		/// Непосредственно получает сессии из БД по запросу
		/// </summary>
		/// <param name="str">Строка запроса (StringBuilder)</param>
		/// <returns>Список сессий по запросу</returns>
		private List<Session> GetSessions(StringBuilder str)
		{
			List<Session> s = new List<Session>();
			MySqlDataReader datareader = null;
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
				 	Session st = new Session();
					st.Id = datareader.GetInt32(0);
					st.Time = datareader.GetDateTime(1);
					st.Duration = datareader.GetInt32(2);
					st.GroupId = datareader.GetInt32(3);
					st.UserId = datareader.GetInt32(4);
					st.StudentsCount = datareader.GetInt32(5);
					st.addPresentIdsList(MTSystem.GetIdsFromString(datareader.GetString(6)));
					st.addAbsentIdsList(MTSystem.GetIdsFromString(datareader.GetString(7)));
					s.Add(st);
				}		
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return s;	
		}
		
/*//////////////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод, получающий айди последней записи в таблице
		/// </summary>
		/// <param name="tablename">Название таблицы</param>
		/// <returns>Айди записи</returns>
		public int GetLastInsertId(String tablename)
		{
			int res = 0;
			MySqlDataReader datareader = null;
			try
			{
				StringBuilder str = new StringBuilder();
				str.AppendFormat("SELECT max(`id`) from `{0}`;", tablename);
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					res = datareader.GetInt32(0);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				if(datareader != null && datareader.IsClosed == false) datareader.Close();
				MessageBox.Show(ex.ToString());
			}	
			return res;
		}

/*//////////////////////////////////////////////OTHERS/////////////////////////////////////////////////*/

		~Mysql()
		{
			if(myConnection != null)
				myConnection.Close();
		}
	}
}
