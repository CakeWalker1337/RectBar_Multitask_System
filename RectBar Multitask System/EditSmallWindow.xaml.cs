/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 11.12.2017
 * Time: 21:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Логика окна создания/редактирования дополнительных параметров системы
	/// (достижения, тип группы, уровень группы, статус учеников)
	/// </summary>
	public partial class EditSmallWindow : Window
	{
		private Object currentObject; //Объект редактирования (доп параметр системы)
		
		/// <summary>
		/// Делегат эвента изменения данных для обновления этих данных в главном окне.
		/// </summary>
		/// <param name="ot">Тип обработанного объекта</param>
		public delegate void SaveWindowData(ObjectType ot);
		public event SaveWindowData SaveWindowDataEvent;
		
		public EditSmallWindow(ObjectType ot)
		{
			InitializeComponent(); //Конструктор окна в случае создания доп параметра системы
			deleteButton.IsEnabled = false;
			if(ot == ObjectType.EventType)
			{
				Title = "Создать достижение";	
				EventType t = new EventType();
				currentObject = (Object)t;
			}
			else if(ot == ObjectType.StudentStatus)
			{
				Title = "Создать статус студента";
				StudentStatus t = new StudentStatus();
				currentObject = (Object)t;
			}
			else if(ot == ObjectType.GroupLevel)
			{
				Title = "Создать уровень группы";
				GroupLevel t = new GroupLevel();
				currentObject = (Object)t;
			}
			else if(ot == ObjectType.GroupType)
			{
				Title = "Создать тип группы";
				GroupType t = new GroupType();
				currentObject = (Object)t;
			}
			else currentObject = null;
		}
		
		public EditSmallWindow(Object ot)
		{
			InitializeComponent(); //Конструктор окна в случае редактирования доп параметра системы
			currentObject = ot;
			if(ot is EventType)
			{
				Title = "Редактировать достижение";	
				nameBox.Text = ((EventType)ot).Name;
			}
				
			else if(ot is StudentStatus)
			{
				Title = "Редактировать статус студента";	
				nameBox.Text = ((StudentStatus)ot).Name;
			}
			
			else if(ot is GroupLevel)
			{
				Title = "Редактировать уровень группы";	
				nameBox.Text = ((GroupLevel)ot).Name;
			}
			
			else if(ot is GroupType)
			{
				Title = "Редактировать тип группы";	
				nameBox.Text = ((GroupType)ot).Name;
			}
			
		}
		
		/// <summary>
		/// Метод-callback для кнопки сохранения (создания) расписания
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if(nameBox.Text == "")
			{
				MessageBox.Show("Название не может быть пустым!");
				return;
			}
			if(currentObject is EventType)
			{
				EventType t = (EventType) currentObject;
				t.Name = nameBox.Text;
				if(t.IsValid())
				{
					if(MTSystem.SaveEventType(t))
						MessageBox.Show("Достижение сохранено!");
				}
					
				else
				{
					MTSystem.CreateEventType(t);
					t.Id = MTSystem.GetLastInsertId("eventtypes");
					MTSystem.addEventType(t);
					MessageBox.Show("Достижение создано!");
				}
				SaveWindowDataEvent(ObjectType.EventType);
				Close();
			}
			else if(currentObject is StudentStatus)
			{
				StudentStatus t = (StudentStatus) currentObject;
				t.Name = nameBox.Text;
				if(t.IsValid())
					MTSystem.SaveStudentStatus(t);
				else
				{
					MTSystem.CreateStudentStatus(t);
					t.Id = MTSystem.GetLastInsertId("statuses");
					MTSystem.addStudentStatus(t);
				}
				SaveWindowDataEvent(ObjectType.StudentStatus);
				Close();	
			}
			else if(currentObject is GroupLevel)
			{
				GroupLevel t = (GroupLevel) currentObject;
				t.Name = nameBox.Text;
				if(t.IsValid())
					MTSystem.SaveGroupLevel(t);
				else
				{
					MTSystem.CreateGroupLevel(t);
					t.Id = MTSystem.GetLastInsertId("grouplevels");
					MTSystem.addGroupLevel(t);
				}
				SaveWindowDataEvent(ObjectType.GroupLevel);
				Close();	
			}
			else if(currentObject is GroupType)
			{
				GroupType t = (GroupType) currentObject;
				t.Name = nameBox.Text;
				if(t.IsValid())
					MTSystem.SaveGroupType(t);
				else
				{
					MTSystem.CreateGroupType(t);
					t.Id = MTSystem.GetLastInsertId("grouptypes");
					MTSystem.addGroupType(t);
				}
				SaveWindowDataEvent(ObjectType.GroupType);
				Close();	
			}
			else 
			{
				MessageBox.Show("Ошибка!");
			}
		}
		
		/// <summary>
		/// Метод-callback для кнопки выхода
		/// </summary>
		void exitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Метод-callback для кнопки удаления расписания.
		/// Вызывает окно подтверждения.
		/// </summary>
		void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			
			StringBuilder sb = new StringBuilder();
			if(currentObject is EventType)
				sb.AppendFormat("достижение {0}?", ((EventType)currentObject).Name);
			if(currentObject is GroupType)
				sb.AppendFormat("тип группы {0}?", ((GroupType)currentObject).Name);
			if(currentObject is GroupLevel)
				sb.AppendFormat("уровень группы {0}?", ((GroupLevel)currentObject).Name);
			if(currentObject is StudentStatus)
				sb.AppendFormat("статус ученика {0}?", ((StudentStatus)currentObject).Name);
			
			ConfirmWindow cw = new ConfirmWindow("Вы точно хотите удалить " + sb.ToString());
			cw.ConfirmEvent += DeleteWindow_CallBack;
			cw.ShowDialog();
			
			
		}
		
			/// <summary>
		/// Метод-обработчик эвента удаления расписания.
		/// Обрабатывает результат окна подтверждения.
		/// </summary>
		void DeleteWindow_CallBack(bool confirm)
		{
			if(confirm == false) return;
			if(currentObject is EventType)
			{
				EventType t = (EventType) currentObject;
				int id = t.Id;
				MTSystem.deleteEventType(t);
				
				List<Student> l = MTSystem.LoadStudentsWithAchievementId(id);
				if(l[0] != null)
				{
					for(int i = 0; i<l.Count; i++)
					{
						l[i].deleteAchievement(l[i].findAchievementById(t.Id));
						MTSystem.SaveStudent(l[i]);
					}
				}
				
				if(MTSystem.DeleteEventType(id))
					MessageBox.Show("Удаление успешно");
				SaveWindowDataEvent(ObjectType.EventType);
				Close();
			}
			else if(currentObject is StudentStatus)
			{
				StudentStatus t = (StudentStatus) currentObject;
				int id = t.Id;
				MTSystem.deleteStudentStatus(t);
				if(MTSystem.DeleteStudentStatus(id))
					MessageBox.Show("Удаление успешно");
				SaveWindowDataEvent(ObjectType.StudentStatus);
				Close();
			}
			else if(currentObject is GroupLevel)
			{
				GroupLevel t = (GroupLevel) currentObject;
				int id = t.Id;
				MTSystem.deleteGroupLevel(t);
				if(MTSystem.DeleteGroupLevel(id))
					MessageBox.Show("Удаление успешно");
				for(int i = 0; i<MTSystem.GroupsCount; i++)
				{
					if(MTSystem.getGroup(i).Level.Id == id)
						MTSystem.getGroup(i).Level.Clear();
				}
				SaveWindowDataEvent(ObjectType.GroupLevel);
				Close();	
			}
			else if(currentObject is GroupType)
			{
				GroupType t = (GroupType) currentObject;
				int id = t.Id;
				MTSystem.deleteGroupType(t);
				for(int i = 0; i<MTSystem.GroupsCount; i++)
				{
					if(MTSystem.getGroup(i).Type.Id == id)
						MTSystem.getGroup(i).Type.Clear();
				}
				if(MTSystem.DeleteGroupType(id))
						MessageBox.Show("Удаление успешно");
				SaveWindowDataEvent(ObjectType.GroupType);
				Close();
			}
			else 
			{
				MessageBox.Show("Ошибка!");
			}
			Close();
		
		}
	}
}