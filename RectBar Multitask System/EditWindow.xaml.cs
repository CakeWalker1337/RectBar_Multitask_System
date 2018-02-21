/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 12/03/2017
 * Time: 21:36
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
	/// Класс, описывающий логику взаимодействия с основными параметрами системы (ученики, преподаватели, группы, админы).
	/// Не имеет презентера, модель DockView.
	/// </summary>
	public partial class EditWindow : Window
	{
		private Object currentObject; // Объект для обработки (один из основных параметров системы)
		
		/// <summary>
		/// Делегат, отвечающий за эвент изменения данных в текущем окне.
		/// Эвент вызывается в главном окне для обновления данных в графических элементах окна.
		/// </summary>
		/// <param name="ot">Тип обработанного объекта</param>
		public delegate void SaveWindowData(ObjectType ot);
		public event SaveWindowData SaveWindowDataEvent;
		
		private CalendarDateRange currentDateRange = null; //Период для вычисления некоторых данных.
		public EditWindow(Object obj)
		{
			currentObject = obj;
			InitializeComponent(); // Конструктор окна в случае редактирования
			deleteButton.IsEnabled = true;
			if(obj is Admin)
			{
				Title = "Редактировать администратора";				
				Admin adm = (Admin)obj;
				SetConfiguration(ObjectType.Admin);
				ClearInfoBlocks();
				infoBlock1.Text = adm.Id.ToString();
				infoBlock2.Text = adm.Name;
				infoBlock3.Text = adm.Login;
				infoBlock5.Text = adm.Pass;
			}
			else if(obj is Group)
			{
				Title = "Редактировать группу";	
				Group gr = (Group)obj;
				SetConfiguration(ObjectType.Group);
				ClearInfoBlocks();
				infoBlock1.Text = gr.Id.ToString();
				infoBlock2.Text = gr.Name;
				infoBlock3.Text = gr.PlanHours.ToString();
				CountDataFields();
				groupColorPicker.SelectedColor = gr.Color.Color;
				
				for(int i = 0; i<gr.TeachersCount; i++)
				{
					infoSmallGrid.Items.Add(MTSystem.findTeacherById(gr.getTeacherId(i)).toSample());
				}
				List<int> studentIds = new List<int>();
				for(int i = 0; i<gr.StudentsCount; i++)
				{
					studentIds.Add(gr.getStudentId(i));
				}
				List<Student> students = MTSystem.LoadStudents(studentIds);
				for(int i = 0; i<studentIds.Count; i++)
				{
					SampleGrid sg = students[i].toSample();
					sg.P5 = (students[i].checkGroupId(gr.Id))?"Ходит":"Выбыл";
					infoBigGrid.Items.Add(sg);
				}
			}
			else if(obj is Student)
			{
				Title = "Редактировать ученика";	
				Student st = (Student)obj;
				SetConfiguration(ObjectType.Student);
				ClearInfoBlocks();
				infoBlock1.Text = st.Id.ToString();
				infoBlock2.Text = st.FullName;
				infoBlock3.Text = st.Age.ToString();
				CountDataFields();
				
				for(int i = 0; i<st.AchievementsCount; i++)
				{
					infoSmallGrid.Items.Add(st.getAchievement(i).toSample());
				}
				
				for(int i = 0; i<st.GroupIdsCount; i++)
				{
					infoBigGrid.Items.Add(MTSystem.findGroupById(st.getGroupId(i)).toSample());
				}
				
			}
			else if(obj is Teacher)
			{
				Title = "Редактировать преподавателя";	
				Teacher tc = (Teacher)obj;
				SetConfiguration(ObjectType.Teacher);
				ClearInfoBlocks();
				infoBlock1.Text = tc.Id.ToString();
				infoBlock2.Text = tc.Name;
				infoBlock3.Text = tc.Login;
				infoBlock5.Text = tc.Pass;
				
				for(int i = 0; i<tc.GroupIdsCount; i++)
				{
					infoBigGrid.Items.Add(MTSystem.findGroupById(tc.getGroupId(i)).toSample());
				}
				
			}
			
		}
		
		public EditWindow(ObjectType obj)
		{
			InitializeComponent(); // Конструктор окна в случае создания
			if(obj == ObjectType.Admin)
			{
				Title = "Создать администратора";
				Admin g = new Admin();
				currentObject = (Object) g;
			}
			else if(obj == ObjectType.Student)
			{
				Title = "Создать ученика";
				Student g = new Student();
				currentObject = (Object) g;
			}
			else if(obj == ObjectType.Teacher)
			{
				Title = "Создать преподавателя";
				Teacher g = new Teacher();
				currentObject = (Object) g;
			}
			else if(obj == ObjectType.Group)
			{
				Title = "Создать группу";
				Group g = new Group();
				currentObject = (Object) g;
			}
			SetConfiguration(obj);
		}
		
		/// <summary>
		/// Метод, задающая внешний вид всем элементам окна, в зависимости от типа объекта.
		/// </summary>
		/// <param name="ot">Тип объекта окна.</param>
		public void SetConfiguration(ObjectType ot)
		{
			if(ot == ObjectType.Student)
			{
				currentDateRange = new CalendarDateRange();
				startDatePicker.SelectedDate = DateTime.Today.AddDays(-7);
				endDatePicker.SelectedDate = DateTime.Today.Add(new TimeSpan(23,59,59));
				groupColorPicker.Visibility = Visibility.Hidden;
				addButtonBig.Content = "Добавить группу";
				addButtonSmall.Content = "Добавить достижение";
				deleteButtonBig.Content = "Удалить группу";
				InitializeComboBox(ObjectType.Student);
				ClearInfoBlocks();
				infoLabel1.Text = "ID";
				infoLabel2.Text = "Имя";
				infoLabel3.Text = "Возраст";
				infoLabel4.Text = "Статус";
				infoLabel5.Text = "Посещение";
				infoBlock5.IsEnabled = false;
				cbBlock7.Visibility = Visibility.Hidden;
				infoBigGrid.Columns[0].Header = "ID";
				infoBigGrid.Columns[1].Header = "Название";
				infoBigGrid.Columns[2].Header = "Тип";
				infoBigGrid.Columns[3].Header = "Уровень";
				for(int i = 4; i<infoBigGrid.Columns.Count; i++) infoBigGrid.Columns[i].Visibility = Visibility.Hidden;
				cbPlace.SelectedIndex = 0;
				infoSmallGrid.Columns[1].Header = "Название достижения";
				infoSmallGrid.Columns[2].Header = "Место";
			}
			else if(ot == ObjectType.Teacher)
			{
				groupColorPicker.Visibility = Visibility.Hidden;
				infoSmallGrid.Visibility = Visibility.Hidden;
				addButtonSmall.Visibility = Visibility.Hidden;
				addButtonBig.Content = "Добавить группу";
				deleteButtonBig.Content = "Удалить группу";
				cbSmall.Visibility = Visibility.Hidden;
				cbPlace.Visibility = Visibility.Hidden;
				ClearInfoBlocks();
				infoLabel1.Text = "ID";
				infoLabel2.Text = "Имя";
				infoLabel3.Text = "Логин";
				infoLabel5.Text = "Пароль";
				cbBlock4.Visibility = Visibility.Hidden;
				cbBlock7.Visibility = Visibility.Hidden;
				dateStackPanel.Visibility = Visibility.Hidden;
				infoBigGrid.Columns[0].Header = "ID";
				infoBigGrid.Columns[1].Header = "Название";
				infoBigGrid.Columns[2].Header = "Тип";
				infoBigGrid.Columns[3].Header = "Уровень";
				infoBigGrid.Columns[4].Header = "Кол-во ч.";
				infoBigGrid.Columns[5].Header = "План. ч.";
			}
			else if(ot == ObjectType.Group)
			{
				currentDateRange = new CalendarDateRange();
				startDatePicker.SelectedDate = DateTime.Today.AddDays(-7);
				endDatePicker.SelectedDate = DateTime.Today.Add(new TimeSpan(23,59,59));
				addButtonBig.Content = "Добавить ученика";
				addButtonSmall.Content = "Добавить преподавателя";

				InitializeComboBox(ObjectType.Group);
				
				ClearInfoBlocks();
				for(int i = 5; i<6; i++) infoBigGrid.Columns[i].Visibility = Visibility.Hidden;
				infoLabel1.Text = "ID";
				infoLabel2.Text = "Название";
				infoLabel3.Text = "План. часов";
				infoLabel4.Text = "Тип";
				infoLabel5.Text = "Кол-во часов";
				infoLabel6.Text = "Текучка";
				infoLabel7.Text = "Уровень";
				infoLabel8.Text = "Цвет группы";				
				infoBigGrid.Columns[0].Header = "ID";
				infoBigGrid.Columns[1].Header = "Имя";
				infoBigGrid.Columns[2].Header = "Возраст";
				infoBigGrid.Columns[3].Header = "Статус";
				infoBigGrid.Columns[4].Header = "Посещаемость";
				
				
				infoSmallGrid.Columns[1].Header = "Имя преподавателя";
				infoSmallGrid.Columns[2].Visibility = Visibility.Hidden;
				cbPlace.Visibility = Visibility.Hidden;
				infoBlock5.IsEnabled = false;
			}
			else if(ot == ObjectType.Admin)
			{
				addButtonBig.Visibility = Visibility.Hidden;
				addButtonSmall.Visibility = Visibility.Hidden;
				groupColorPicker.Visibility = Visibility.Hidden;
				infoSmallGrid.Visibility = Visibility.Hidden;
				findBoxBig.Visibility = Visibility.Hidden;
				cbSmall.Visibility = Visibility.Hidden;
				cbPlace.Visibility = Visibility.Hidden;
				dateStackPanel.Visibility = Visibility.Hidden;
				deleteButtonBig.Visibility = Visibility.Hidden;
				ClearInfoBlocks();
				cbBlock4.Visibility = Visibility.Hidden;
				cbBlock7.Visibility = Visibility.Hidden;
				infoLabel1.Text = "ID";
				infoLabel2.Text = "Имя";
				infoLabel3.Text = "Логин";
				infoLabel5.Text = "Пароль";
				for(int i = 0; i<6; i++) infoBigGrid.Columns[i].Visibility = Visibility.Hidden;
			}
		
		}
		
		/// <summary>
		/// Метод, инициализирующая комбобоксы окна, если тип объекта этого требует.
		/// </summary>
		/// <param name="ot">Тип объекта окна</param>
		private void InitializeComboBox(ObjectType ot)
		{
			if(ot == ObjectType.Student)
			{
				Student st = (Student)currentObject;
				cbBlock4.Items.Add("Не выбрано");
				cbBlock4.SelectedIndex = 0;
				for(int i = 0; i<MTSystem.StudentStatusesCount; i++)
				{
					cbBlock4.Items.Add(MTSystem.getStudentStatus(i).Name);
					if(st.Status.Id == MTSystem.getStudentStatus(i).Id) cbBlock4.SelectedIndex = i+1;
				}				
				
				for(int i = 0; i<MTSystem.EventTypesCount; i++)
				{
					int isFinded = 0;
					for(int j = 0; j<st.AchievementsCount; j++)
					{
						if(st.getAchievement(j).Type == MTSystem.getEventType(i).Id)
						{
							isFinded = 1;
							break;
						}
					}
					if(isFinded == 0)
					{
						cbSmall.Items.Add(MTSystem.getEventType(i).Name);
					}
				}
				if(cbSmall.Items.Count != 0) cbSmall.SelectedIndex = 0;
			}
			if(ot == ObjectType.Group)
			{
				Group g = (Group)currentObject;
				cbBlock4.Items.Add("Не выбрано");
				cbBlock4.SelectedIndex = 0;
				for(int i = 0; i<MTSystem.GroupTypesCount; i++)
				{
					cbBlock4.Items.Add(MTSystem.getGroupType(i).Name);
					if(g.Type.Id == MTSystem.getGroupType(i).Id) cbBlock4.SelectedIndex = i+1;
				}
				
				cbBlock7.Items.Add("Не выбрано");
				cbBlock7.SelectedIndex = 0;
				for(int i = 0; i<MTSystem.GroupLevelCount; i++)
				{
					cbBlock7.Items.Add(MTSystem.getGroupLevel(i).Name);
					if(g.Level.Id == MTSystem.getGroupLevel(i).Id) cbBlock7.SelectedIndex = i+1;
				}				
				
				int isFinded = 0;
				for(int i = 0; i<MTSystem.TeachersCount; i++)
				{
					for(int j = 0; j<g.TeachersCount; j++)
					{
						if(MTSystem.getTeacher(i).Id == g.getTeacherId(j))
						{
							isFinded = 1;
							break;
						}
					}
					if(isFinded == 0) cbSmall.Items.Add(MTSystem.getTeacher(i).Name);
					isFinded = 0;
				}
				if(cbSmall.Items.Count != 0) cbSmall.SelectedIndex = 0;
			}
		}
		
		/// <summary>
		/// Метод, очищающая визуальные элементы окна (текстблоки, гриды).
		/// </summary>
		private void ClearInfoBlocks()
		{
			infoBlock1.Text = "";
			infoBlock2.Text = "";
			infoBlock3.Text = "";
			infoBlock5.Text = "";
			infoSmallGrid.Items.Clear();
			infoBigGrid.Items.Clear();
		}
		
		/// <summary>
		/// Метод-callback для кнопки добавления данных меньшей важности (препод для группы, достижение для ученика).
		/// </summary>
		void addButtonSmall_Click(object sender, RoutedEventArgs e)
		{
			if(addButtonSmall.Content.ToString() == "Добавить преподавателя")
			{
				if(cbSmall.Items.Count == 0) return;
				infoSmallGrid.Items.Add(MTSystem.findTeachersByName(cbSmall.SelectedItem.ToString())[0].toSample());
				cbSmall.Items.Remove(cbSmall.SelectedItem);
				if(cbSmall.Items.Count != 0) cbSmall.SelectedIndex = 0;
			}
			else if(addButtonSmall.Content.ToString() == "Добавить достижение")
			{
				if(cbSmall.Items.Count == 0) return;
				Achievement a = new Achievement();
				a.Type = MTSystem.findEventTypeByName(cbSmall.SelectedItem.ToString()).Id;
				a.Place = cbPlace.SelectedIndex;
				infoSmallGrid.Items.Add(a.toSample());
				cbSmall.Items.Remove(cbSmall.SelectedItem);
				if(cbSmall.Items.Count != 0) cbSmall.SelectedIndex = 0;
			}
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку добавления более важных данных в таблицы
		/// (ученики для группы, группы для ученика).
		/// Осуществляет поиск данных по введенному значению.
		/// </summary>
		void addButtonBig_Click(object sender, RoutedEventArgs e)
		{
			if(findBoxBig.Text == "")
			{
				MessageBox.Show("Пустой поисковой запрос!");
				return;
			}
			if(addButtonBig.Content.ToString() == "Добавить ученика")
			{
				List<Student> students = Finder.FindStudents(findBoxBig.Text);
				if(students[0] == null)
				{
					MessageBox.Show("Ученики не найдены!");
					return;
				}
				if(students.Count == 1)
				{
					int count = 0;
					for(int i = 0; i<infoBigGrid.Items.Count; i++)
					{
						SampleGrid sg = (SampleGrid)infoBigGrid.Items[i];
						if(sg.P1 == students[0].Id.ToString())
						{
							if(sg.P5 == "Выбыл")
							{
								infoBigGrid.Items.RemoveAt(i);
								break;
							}
							count = 1;
							break;
						}
					}
					if(count == 0)
					{
						SampleGrid ns = students[0].toSample();
						ns.P5 = "Ходит";
						infoBigGrid.Items.Add(ns);
						MessageBox.Show("Ученик добавлен!");
					}
					else
						MessageBox.Show("Ученик уже присутствует!");
					return;
				}
				SearchResultWindow srw = new SearchResultWindow(infoBigGrid, students); //Если поиск дал больше одного результата
				srw.SearchWindowResultEvent += SearchResult_CallBack;
				srw.ShowDialog();
			}
			if(addButtonBig.Content.ToString() == "Добавить группу")
			{
				List<Group> gr = Finder.FindGroups(findBoxBig.Text);
				if(gr[0] == null)
				{
					MessageBox.Show("Группа не найдена!");
					return;
				}
				if(gr.Count == 1)
				{
					int count = 0;
					for(int i = 0; i<infoBigGrid.Items.Count; i++)
					{
						SampleGrid sg = (SampleGrid)infoBigGrid.Items[i];
						if(gr[0].Id.ToString() == ((SampleGrid)infoBigGrid.Items[i]).P1)
						{
							count = 1;
							break;
						}
					}
					if(count == 0)
						infoBigGrid.Items.Add(gr[0].toSample());
					MessageBox.Show("Группа добавлена!");
					return;
				}
				
				SearchResultWindow srw = new SearchResultWindow(infoBigGrid, gr);//Если поиск дал больше одного результата
				srw.SearchWindowResultEvent += SearchResult_CallBack;
				srw.ShowDialog();
			}	
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку удаления более важных данных из таблиц
		/// (удаление учеников для группы, групп для ученика).
		/// </summary>
		void deleteButtonBig_Click(object sender, RoutedEventArgs e)
		{
			if(currentObject is Admin) return;
			if(currentObject is Group)
			{
				if(infoBigGrid.SelectedItem == null)
				{
					MessageBox.Show("Ученик не выбран!");
					return;
				}
				SampleGrid sg = (SampleGrid)infoBigGrid.SelectedItem;
				if(sg.P5 == "Ходит")
				{
					infoBigGrid.Items.RemoveAt(infoBigGrid.SelectedIndex);
					sg.P5 = "Выбыл";
					infoBigGrid.Items.Add(sg);
					MessageBox.Show("Ученик выбыл!");
				}
				else if(sg.P5 == "Выбыл")
				{
					infoBigGrid.Items.RemoveAt(infoBigGrid.SelectedIndex);
					MessageBox.Show("Ученик удален!");
				}
				return;
			}
			if(infoBigGrid.SelectedItem == null)
			{
				MessageBox.Show("Группа не выбрана!");
				return;
			}
			infoBigGrid.Items.RemoveAt(infoBigGrid.SelectedIndex);
			MessageBox.Show("Группа удалена!");
		}
		
		/// <summary>
		/// Метод-callback для кнопки выхода
		/// </summary>
		void exitButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Метод-callback для кнопки сохранения/создания объекта.
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentObject is Teacher)
			{
				if(CheckDataBoxes(ObjectType.Teacher))
				{
					int flag = 0;
					Teacher t = (Teacher) currentObject;
					t.Login = infoBlock3.Text;
					t.Pass = infoBlock5.Text;
					t.Name = infoBlock2.Text;
					
					if(!t.IsValid())
					{
						if(MTSystem.CheckLogin(infoBlock3.Text))
						{
							MessageBox.Show("Логин уже зарегистрирован!");
							return;
						}
						MTSystem.CreateClient((User)t);
						t.Id = MTSystem.GetLastInsertId("users");
						MTSystem.addTeacher(t);
						flag = 1;
					}
					// Алгоритмы сохранения данных без дублирования
					int count = 0;
					List<int> groups = new List<int>();
					for(int i = 0; i<t.GroupIdsCount; i++) groups.Add(t.getGroupId(i));
				
					t.ClearGroupIds();
					
					for(int i = 0; i<infoBigGrid.Items.Count; i++) 
						t.addGroupId(Convert.ToInt32(((SampleGrid)infoBigGrid.Items[i]).P1));
					
					for(int i = 0; i<groups.Count; i++)
					{
						for(int j = 0; j<t.GroupIdsCount; j++)
						{
							if(groups[i] == t.getGroupId(j))
							{
								count++;
								break;
							}
						}
						if(count == 0) // Если старая группа не найдена в новых, то ее удалили
						{
							Group g = MTSystem.findGroupById(groups[i]);
							g.deleteTeacherId(t.Id);
							MTSystem.SaveGroup(g);
						}
						count = 0;
					}
					
					for(int i = 0; i<t.GroupIdsCount; i++)
					{
						for(int j = 0; i<groups.Count; j++)
						{
							if(t.getGroupId(i) == groups[j])
							{
								count++;
								break;
							}
						}
						if(count == 0) // Если новая группа не найдена в старых, то ее добавили
						{
							Group g = MTSystem.findGroupById(t.getGroupId(i));
							g.addTeacherId(t.Id);
							MTSystem.SaveGroup(g);
						}
						count = 0;
					}
					
					if(MTSystem.SaveClient((User)t))
					{
						if(flag == 1)
							MessageBox.Show("Преподаватель создан успешно!");
						else
							MessageBox.Show("Преподаватель сохранен!");
					}
					else
					{
						if(flag == 1)
							MessageBox.Show("Ошибка при создании!");
						else
							MessageBox.Show("Ошибка при сохранении!");
					}
					
					SaveWindowDataEvent(ObjectType.Teacher);
					Close();
				}
			}
			else if(currentObject is Admin)
			{
				
				if(CheckDataBoxes(ObjectType.Admin))
				{
					Admin t = (Admin) currentObject;
					t.Login = infoBlock3.Text;
					t.Pass = infoBlock5.Text;
					t.Name = infoBlock2.Text;
					if(t.IsValid())
					{
						if(MTSystem.SaveClient((User)t))
							MessageBox.Show("Администратор сохранен!");
						else
							MessageBox.Show("Ошибка сохранения!");
					}
					else
					{
						MTSystem.CreateClient((User)t);
						MessageBox.Show("Администратор создан успешно!");
					}
						
					SaveWindowDataEvent(ObjectType.Admin);
					Close();
				}
			}
			else if(currentObject is Group)
			{
				if(CheckDataBoxes(ObjectType.Group))
				{
					Group t = (Group) currentObject;
					t.Name = infoBlock2.Text;
					t.PlanHours = Convert.ToInt32(infoBlock3.Text);
					int flag = 0;
					if(cbBlock4.SelectedItem.ToString() == "Не выбрано")
					{
						GroupType grouptype = new GroupType();
						grouptype.Id = 0;
						grouptype.Name = "Не выбрано";
						t.Type = grouptype;
					}
					else t.Type = MTSystem.findGroupTypeByName(cbBlock4.SelectedItem.ToString());
					t.Color.Color = groupColorPicker.SelectedColor;
					if(cbBlock7.SelectedItem.ToString() == "Не выбрано")
					{
						GroupLevel grouplevel = new GroupLevel();
						grouplevel.Id = 0;
						grouplevel.Name = "Не выбрано";
						t.Level = grouplevel;
					}
					else t.Level = MTSystem.findGroupLevelByName(cbBlock7.SelectedItem.ToString());
					
					if(!t.IsValid())
					{
						MTSystem.CreateGroup(t);
						t.Id = MTSystem.GetLastInsertId("groups");
						MTSystem.addGroup(t);
						flag = 1;
					}
					
					// Алгоритмы сохранения данных без дублирования
					List<int> teachers = new List<int>();
					for(int i = 0; i<t.TeachersCount; i++) teachers.Add(t.getTeacherId(i));
					
					int count = 0;
					
					List<int> students = new List<int>();
					for(int i = 0; i<t.StudentsCount; i++) students.Add(t.getStudentId(i));
					
					t.ClearStudentIds();
					for(int i = 0; i<infoBigGrid.Items.Count; i++)
					{
						SampleGrid sg = (SampleGrid) infoBigGrid.Items[i];
						Student s = sg.toStudent();
						if(sg.P5 == "Ходит")
						{
							if(!s.checkGroupId(t.Id))
							{
								s.addGroupId(t.Id);
								MTSystem.SaveStudent(s);
							}
						}
						else
						{
							if(s.checkGroupId(t.Id))
							{
								s.deleteGroupId(t.Id);
								MTSystem.SaveStudent(s);
							}
						}
						t.addStudentId(s.Id);
					}
					
					for(int i = 0; i<students.Count; i++)
					{
						for(int j = 0; j<t.StudentsCount; j++)
						{
							if(students[i] == t.getStudentId(j))
							{
								count = 1;
								break;
							}
						}
						if(count == 0)
						{
							Student s = MTSystem.LoadStudent(students[i]);
							if(s != null)
							{
								if(s.checkGroupId(t.Id))
								{
									s.deleteGroupId(t.Id);
									MTSystem.SaveStudent(s);
								}
							}
						}
						count = 0;
					}
					t.ClearTeacherIds();
					
					for(int i = 0; i<infoSmallGrid.Items.Count; i++)
						t.addTeacherId(Convert.ToInt32(((SampleGrid)infoSmallGrid.Items[i]).P1));
					
					for(int i = 0; i<teachers.Count; i++)
					{
						for(int j = 0; j<t.TeachersCount; j++)
						{
							if(teachers[i] == t.getTeacherId(j))
							{
								count++;
								break;
							}
						}
						if(count == 0) // Если старый препод не найден в новых, то его удалили
						{
							MTSystem.findTeacherById(teachers[i]).deleteGroupId(t.Id);
						}
						count = 0;
					}
					
					for(int i = 0; i<t.TeachersCount; i++)
					{
						for(int j = 0; j<teachers.Count; j++)
						{
							if(t.getTeacherId(i) == teachers[j]) 
							{
								count++;
								break;
							}
						}
						if(count == 0) // Если новый препод не найден в старых, то его добавили
						{
							MTSystem.findTeacherById(t.getTeacherId(i)).addGroupId(t.Id);
						}
						count = 0;
					}
					
					if(MTSystem.SaveGroup(t))
					{
						if(flag == 1)
							MessageBox.Show("Группа создана успешно!");
						else
							MessageBox.Show("Группа сохранена!");
					}
					else
					{
						if(flag == 1)
							MessageBox.Show("Ошибка при создании!");
						else
							MessageBox.Show("Ошибка при сохранении!");
					}
					SaveWindowDataEvent(ObjectType.Group);
					Close();
				}
			
			}
			else if(currentObject is Student)
			{
				if(CheckDataBoxes(ObjectType.Student))
				{
					Student t = (Student) currentObject;
					t.FullName = infoBlock2.Text;
					t.Age = Convert.ToInt32(infoBlock3.Text);
					if(cbBlock4.SelectedItem.ToString() == "Не выбрано")
					{
						StudentStatus status = new StudentStatus();
						status.Id = 0;
						status.Name = "Не выбрано";
						t.Status = status;
					}
					else t.Status = MTSystem.findStudentStatusByName(cbBlock4.SelectedItem.ToString());
					int flag = 0;
					if(!t.IsValid())
					{
						MTSystem.CreateStudent(t);
						t.Id = MTSystem.GetLastInsertId("students");
						flag = 1;
					}
						// Алгоритмы сохранения данных без дублирования
					List<int> groups = new List<int>();
					for(int i = 0; i<t.GroupIdsCount; i++) groups.Add(t.getGroupId(i));
					
					t.ClearGroupIds();
					for(int i = 0; i<infoBigGrid.Items.Count; i++)
					{
						t.addGroupId(Convert.ToInt32(((SampleGrid)infoBigGrid.Items[i]).P1));
					}
					int count = 0;
					for(int i = 0; i<t.GroupIdsCount; i++)
					{
						for(int j = 0; i<groups.Count; j++)
						{
							if(t.getGroupId(i) == groups[j])
							{
								count++;
								break;
							}
						}
						if(count == 0) // Если новая группа не найдена в старых, то ее добавили
						{
							Group g = MTSystem.findGroupById(t.getGroupId(i));
							if(!g.checkStudentId(t.Id))
							{
								g.addStudentId(t.Id);
								MTSystem.SaveGroup(g);
							}
						}
						count = 0;
					}
					
					t.ClearAchievements();
					for(int i = 0; i<infoSmallGrid.Items.Count; i++)
					{
						Achievement a = new Achievement();
						a.Type = Convert.ToInt32(((SampleGrid)infoSmallGrid.Items[i]).P1);
						if(((SampleGrid)infoSmallGrid.Items[i]).P3 == "Участвовал") a.Place = 0;
						else a.Place = Convert.ToInt32(((SampleGrid)infoSmallGrid.Items[i]).P3);
						t.addAchievement(a);
					}
					
					if(MTSystem.SaveStudent(t))
					{
						if(flag == 1)
							MessageBox.Show("Ученик создан успешно!");
						else
							MessageBox.Show("Ученик сохранен!");
					}
					else
					{
						if(flag == 1)
							MessageBox.Show("Ошибка при создании!");
						else
							MessageBox.Show("Ошибка при сохранении!");
					}
					SaveWindowDataEvent(ObjectType.Student);
					Close();
				}
			}
		}
		
		/// <summary>
		/// Метод, осуществляющая проверку введенных данных.
		/// </summary>
		/// <param name="ot">Тип объекта, на соответствие которому надо проверить данные</param>
		private bool CheckDataBoxes(ObjectType ot)
		{
			if(ot == ObjectType.Admin || ot == ObjectType.Teacher)
			{
				if(infoBlock2.Text == "" || infoBlock3.Text == "" || infoBlock5.Text == "")
				{
					MessageBox.Show("Введены неверные данные!");
					return false;
				}
				return true;
			}
			if(ot == ObjectType.Student)
			{
				int res = 0;
				if(Int32.TryParse(infoBlock3.Text, out res) && res > 0 && infoBlock2.Text != "")
					return true;
				MessageBox.Show("Введены неверные данные!");
				return false;
			}
			if(ot == ObjectType.Group)
			{
				int res = 0;
				if(infoBlock2.Text != "" && infoBlock3.Text != "" && Int32.TryParse(infoBlock3.Text, out res) && res > 0)
					return true;
				MessageBox.Show("Введены неверные данные!");
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод-callback для эвента двойного клика по малой таблице.
		/// При двойном клике на малую таблицу происходит удаление данных из нее.
		/// </summary>
		void infoSmallGrid_DoubleClick(object sender, MouseButtonEventArgs a)
		{
			if(infoSmallGrid.SelectedItem == null) return;
			cbSmall.Items.Add(((SampleGrid)infoSmallGrid.SelectedItem).P2);
			cbSmall.SelectedIndex = 0;
			infoSmallGrid.Items.Remove(infoSmallGrid.SelectedItem);	
			if(currentObject is Student)
				MessageBox.Show("Достижение удалено");
			if(currentObject is Group)
				MessageBox.Show("Преподаватель удален");
		}
		
		/// <summary>
		/// Метод-callback для кнопки удаления.
		/// Вызывает окно подтверждения.
		/// </summary>
		void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			if(currentObject is Teacher)
				sb.AppendFormat("преподавателя {0}?", ((Teacher)currentObject).Name);
			if(currentObject is Group)
				sb.AppendFormat("группу {0}?", ((Group)currentObject).Name);
			if(currentObject is Student)
				sb.AppendFormat("ученика {0}?", ((Student)currentObject).FullName);
			if(currentObject is Admin)
				sb.AppendFormat("администратора {0}?", ((Admin)currentObject).Name);
			
			ConfirmWindow cw = new ConfirmWindow("Вы точно хотите удалить " + sb.ToString());
			cw.ConfirmEvent += DeleteWindow_CallBack;
			cw.ShowDialog();
		}
		
		/// <summary>
		/// Метод-callback для эвента подтверждения удаления.
		/// Обрабатывает результат окна подтверждения.
		/// </summary>
		/// <param name="confirm">Результат окна подтверждения</param>
		void DeleteWindow_CallBack(bool confirm)
		{
			if(confirm == false) return;
			if(currentObject is Teacher)
			{
				Teacher t = (Teacher) currentObject;
				for(int i = 0; i<t.GroupIdsCount; i++)
				{
					Group g = MTSystem.findGroupById(t.getGroupId(i));
					g.deleteTeacherId(t.Id);
					MTSystem.SaveGroup(g);
				}
				int id = t.Id;
				MTSystem.deleteTeacher(t);
				if(MTSystem.DeleteClient(id))
				{
					MessageBox.Show("Удаление прошло успешно!");
					SaveWindowDataEvent(ObjectType.Teacher);
					Close();
					return;
				}
				MessageBox.Show("Ошибка удаления!");
				return;
			}
			if(currentObject is Admin )
			{
				Admin adm = (Admin) currentObject;
				if(MTSystem.DeleteClient(adm.Id))
				{
					MessageBox.Show("Удаление прошло успешно!");
					SaveWindowDataEvent(ObjectType.Admin);
					Close();
					return;
				}
				MessageBox.Show("Ошибка удаления!");
				return;
				
			}
			if(currentObject is Student)
			{
				Student s = (Student) currentObject;
				for(int i = 0; i<MTSystem.GroupsCount; i++)
				{
					Group g = MTSystem.getGroup(i);
					if(g.deleteStudentId(s.Id))
						MTSystem.SaveGroup(g);
				}

				if(MTSystem.DeleteStudent(s.Id))
				{
					MessageBox.Show("Удаление прошло успешно!");
					SaveWindowDataEvent(ObjectType.Student);
					Close();
					return;
				}
				MessageBox.Show("Ошибка удаления!");
				return;
			}
			if(currentObject is Group)
			{
				Group g = (Group) currentObject;
				for(int i = 0; i<g.TeachersCount; i++)
				{
					Teacher t = MTSystem.findTeacherById(g.getTeacherId(i));
					t.deleteGroupId(g.Id);
					MTSystem.SaveClient(t);
				}
				for(int i = 0; i<g.StudentsCount; i++)
				{
					Student s = MTSystem.LoadStudent(g.getStudentId(i));
					if(s.deleteGroupId(g.Id))
						MTSystem.SaveStudent(s);
				}
				int id = g.Id;
				MTSystem.deleteGroup(g);
				if(MTSystem.DeleteGroup(id))
				{
					for(int i = 0; i<MTSystem.SchedulesCount; i++) // Обновление данных расписаний
					{
						for(int j = 0; j<MTSystem.getSchedule(i).RowCount; j++)
						{
							for(int k = 0; k<MTSystem.getSchedule(i).getRow(j).GroupIds.Count; k++)
							{
								if(MTSystem.getSchedule(i).getRow(j).GroupIds[k] == id)
								{
									MTSystem.getSchedule(i).getRow(j).GroupIds[k] = 0;
								}
							}
						}
						MTSystem.SaveSchedule(MTSystem.getSchedule(i));
					}
					MessageBox.Show("Удаление прошло успешно!");
					SaveWindowDataEvent(ObjectType.Group);
					Close();
					return;
				}
				MessageBox.Show("Ошибка удаления!");
				return;
			}
			MessageBox.Show("Ошибка удаления!");
		}
		
		/// <summary>
		/// Метод-callback для эвента изменения начальной даты в периоде.
		/// </summary>
		void startDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			currentDateRange.Start = (DateTime) startDatePicker.SelectedDate;
			endDatePicker.DisplayDateStart = currentDateRange.Start;
			if(currentObject != null) CountDataFields();//Данные высчитываются относительно нового периода
		}
		
		/// <summary>
		/// Метод-callback для эвента изменения конечной даты в периоде.
		/// </summary>
		void endDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			currentDateRange.End = ((DateTime) endDatePicker.SelectedDate).Add(new TimeSpan(23,59,59));
			startDatePicker.DisplayDateEnd = (DateTime) endDatePicker.SelectedDate;
			if(currentObject != null) CountDataFields(); //Данные высчитываются относительно нового периода
		}
		
		/// <summary>
		/// Метод, рассчитывающая данные (текучка, посещение) относительно выбранного периода.
		/// </summary>
		void CountDataFields()
		{
			if(currentObject is Student)
			{
				Student st = (Student) currentObject;
				int fulltime = 0;
				List<Session> l = MTSystem.LoadPresentSessionsByStudentId(st.Id);
				if(l[0] != null)
				{
					for(int i = 0; i<l.Count; i++)
						fulltime += l[i].Duration;
				}
				infoBlock5.Text = (fulltime/60) + " час.";
				return;
			}
			if(currentObject is Group)
			{
				Group gr = (Group) currentObject;
				int fulltime = 0, startStudentsCount = 0, endStudentsCount = 0;
				List<Session> l = MTSystem.LoadSessionsByGroupIdByTime(gr.Id, currentDateRange.Start, currentDateRange.End);
				if(l[0] != null)
				{
					for(int i = 0; i<l.Count; i++)
						fulltime += l[i].Duration;
					
					startStudentsCount = l[0].StudentsCount;
					endStudentsCount = l[l.Count-1].StudentsCount;
					infoBlock6.Text = ((startStudentsCount - endStudentsCount)*(-100)/startStudentsCount).ToString() + "%";
				}
				else infoBlock6.Text = "Отсутствуют данные";
				infoBlock5.Text = (fulltime/60) + " час.";
				
				return;
			}
		}
		
		/// <summary>
		/// Метод-callback для окна поиска (если наидено больше одного результата).
		/// Осуществляет перемещение выбранных позиций из таблицы результатов поиска в нужную таблицу
		/// </summary>
		/// <param name="outputGrid">Таблица, куда будут помещаться данные</param>
		/// <param name="list">Лист объектов-результатов поиска</param>
		/// <param name="ot">Тип объекта поиска</param>
		public void SearchResult_CallBack(DataGrid outputGrid, List<Object> list, ObjectType ot)
		{
			if(ot == ObjectType.Student)
			{
				if(list.Count == 0)
				{
					MessageBox.Show("Вы не выбрали ни одного ученика!");
					return;
				}
				List<Student> students = new List<Student>();
				for(int i = 0; i<list.Count; i++)
					students.Add((Student)list[i]);
				
				int count = 0;
				for(int i = 0; i<students.Count; i++)
				{
					for(int j = 0; j<outputGrid.Items.Count; j++)
					{
						SampleGrid sg = (SampleGrid)outputGrid.Items[j];
						if(students[i].Id.ToString() == sg.P1)
						{
							if(sg.P5 == "Выбыл")
							{
								outputGrid.Items.RemoveAt(j);
								break;
							}
							count = 1;
							break;
						}
					}
					if(count == 0)
					{
						SampleGrid ns = students[i].toSample();
						ns.P5 = "Ходит";
						outputGrid.Items.Add(ns);					
					}
					count = 0;
				}
				MessageBox.Show("Ученики добавлены!");
				return;
			}
			if(ot == ObjectType.Group)
			{
				if(list.Count == 0)
				{
					MessageBox.Show("Вы не выбрали ни одну группу!");
					return;
				}
				List<Group> gr = new List<Group>();
				for(int i = 0; i<list.Count; i++)
					gr.Add((Group)list[i]);
				int count = 0;
				for(int i = 0; i<gr.Count; i++)
				{
					for(int j = 0; j<infoBigGrid.Items.Count; j++)
					{
						if(gr[i].Id.ToString() == ((SampleGrid)infoBigGrid.Items[j]).P1)
						{
							count = 1;
							break;
						}
					}
					if(count == 0) //Если группы еще не было у ученика, добавляем ему эту группу
					{
						infoBigGrid.Items.Add(gr[i].toSample());
					}
					count = 0;
				}
				MessageBox.Show("Группы добавлены!");
				return;
			}
		}
		
		
	}
}