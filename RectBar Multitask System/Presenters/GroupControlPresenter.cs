/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/09/2018
 * Time: 17:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RectBar_Multitask_System.Presenters
{
	/// <summary>
	/// Класс, отвечающий за обработку представления вкладки управления группами в главном окне.
	/// </summary>
	public class GroupControlPresenter
	{
		Group currentGroup = null; //Переменная, содержащая текущую (выбранную) группу
		CalendarDateRange currentDateRange = null; //Период времени для просчета некоторых данных группы
		
		public GroupControlPresenter(Window1 w)
		{
			currentDateRange = new CalendarDateRange();
			currentDateRange.Start = DateTime.Today.AddDays(-7);
			currentDateRange.End = DateTime.Today.Add(new TimeSpan(23,59,59));
		}
		
		/// <summary>
		/// Метод, инициализирующий комбобокс групп
		/// </summary>
		/// <param name="w">Объект главного окна для вывода результата</param>
		public void InitializeComboBox(Window1 w)
		{
			Teacher t = (Teacher) w.currentUser;
			if(t.GroupIdsCount == 0)
			{
				currentGroup = null;
				return;
			}
			w.groupControlComboBox.Items.Clear();
			for(int i = 0; i<t.GroupIdsCount; i++)
				w.groupControlComboBox.Items.Add(MTSystem.findGroupById(t.getGroupId(i)).Name);
			w.groupControlComboBox.SelectedIndex = 0;
		}
		
		/// <summary>
		/// Метод, обрабатывающий изменение выбранной группы в комбобоксе.
		/// </summary>
		/// <param name="window">Объект окна для получения данных из комбобокса</param>
		public void GroupControlComboBox_SelectionChanged(Window1 window)
		{
			if(window.groupControlComboBox.SelectedItem == null) return;
			currentGroup = MTSystem.findGroupById(((Teacher)window.currentUser).getGroupId(window.groupControlComboBox.SelectedIndex));
			RefreshData(window);
		}
		
		/// <summary>
		/// Метод, просчитывающий текучку и отработанные часы по данному периоду времени.
		/// </summary>
		/// <param name="w">Объект главного окна для отображения данных</param>
		void CountDataFields(Window1 w)
		{
			int fulltime = 0, startStudentsCount = 0, endStudentsCount = 0;
			List<Session> l = MTSystem.LoadSessionsByGroupIdByTime(currentGroup.Id, currentDateRange.Start, currentDateRange.End);
			if(l[0] != null)
			{
				for(int i = 0; i<l.Count; i++)
					fulltime += l[i].Duration;
				
				startStudentsCount = l[0].StudentsCount;
				endStudentsCount = l[l.Count-1].StudentsCount;
				w.loseBlock.Text = ((startStudentsCount - endStudentsCount)*(-100)/startStudentsCount).ToString() + "%";
			}
			else w.loseBlock.Text = "Отсутствуют данные";
			w.hoursBlock.Text = (fulltime/60) + " час.";
			return;
		}
		
		/// <summary>
		/// Метод сохранения группы.
		/// </summary>
		/// <param name="infoBigGrid">Сетка для сохранения учеников группы</param>
		public void SaveGroup(DataGrid infoBigGrid)
		{	
			if(currentGroup == null)
			{
				MessageBox.Show("Группа не выбрана!");
				return;
			}
			List<int> students = new List<int>();
			for(int i = 0; i<currentGroup.TeachersCount; i++) students.Add(currentGroup.getTeacherId(i));
			
			currentGroup.ClearStudentIds();
			
			
			for(int i = 0; i<infoBigGrid.Items.Count; i++)
			{
				SampleGrid sg = (SampleGrid) infoBigGrid.Items[i];
				Student s = sg.toStudent();
				if(sg.P5 == "Ходит")
				{
					if(!s.checkGroupId(currentGroup.Id))
					{
						s.addGroupId(currentGroup.Id);
						MTSystem.SaveStudent(s);
					}
				}
				else
				{
					if(s.checkGroupId(currentGroup.Id))
					{
						s.deleteGroupId(currentGroup.Id);
						MTSystem.SaveStudent(s);
					}
				}
				currentGroup.addStudentId(s.Id);
			}
			int count = 0;
			for(int i = 0; i<students.Count; i++)
			{
				for(int j = 0; j<currentGroup.StudentsCount; j++)
				{
					if(students[i] == currentGroup.getStudentId(j))
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
						if(s.checkGroupId(currentGroup.Id))
						{
							s.deleteGroupId(currentGroup.Id);
							MTSystem.SaveStudent(s);
						}
					}
				}
				count = 0;
			}
			
			if(MTSystem.SaveGroup(currentGroup))
				MessageBox.Show("Сохранение прошло успешно!");
			
		}
		
		/// <summary>
		/// Метод добавления учеников в таблицу по введенному имени (или его части) или айди в БД.
		/// Если результатов поиска больше 1, то вызывает окно результатов поиска.
		/// </summary>
		/// <param name="infoBigGrid">Сетка для отображения добавленных учеников.</param>
		/// <param name="data">Введенные данные для поиска</param>
		public void AddStudent(DataGrid infoBigGrid, String data)
		{
			if(currentGroup == null)
			{
				MessageBox.Show("Группа не выбрана!");
				return;
			}
			List<Student> students = Finder.FindStudents(data);
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
			SearchResultWindow srw = new SearchResultWindow(infoBigGrid, students);
			srw.SearchWindowResultEvent += SearchResult_CallBack;
			srw.ShowDialog();
			
			
		}
		
		/// <summary>
		/// Удаление ученика из группы.
		/// </summary>
		/// <param name="infoBigGrid">Сетка учеников группы для получения данных</param>
		public void DeleteStudent(DataGrid infoBigGrid)
		{
			if(currentGroup == null)
			{
				MessageBox.Show("Группа не выбрана!");
				return;
			}
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
		}
		
		/// <summary>
		/// Метод обновления данных в окне. Вызывается при выборе группы в комбобоксе.
		/// </summary>
		/// <param name="window">Объект окна для отображения.</param>
		public void RefreshData(Window1 window)
		{
			if(currentGroup == null) return;
			window.idBlock.Text = currentGroup.Id.ToString();
			window.nameBlock.Text = currentGroup.Name;
			window.groupColorPicker.SelectedColor = currentGroup.Color.Color;
			window.typeBlock.Text = currentGroup.Type.Name;
			window.levelBlock.Text = currentGroup.Level.Name;
			window.planHoursBlock.Text = currentGroup.PlanHours.ToString();
			
			CountDataFields(window);
	
			window.infoBigGrid.Items.Clear();
			window.infoSmallGrid.Items.Clear();
			if(currentGroup.StudentsCount > 0)
			{
				List<int> studentIds = new List<int>();
				for(int i = 0; i<currentGroup.StudentsCount; i++)
					studentIds.Add(currentGroup.getStudentId(i));
				List<Student> students = MTSystem.LoadStudents(studentIds);
				for(int i = 0; i<students.Count; i++)
				{
					SampleGrid sg = students[i].toSample();
					sg.P5 = (students[i].checkGroupId(currentGroup.Id))?"Ходит":"Выбыл";
					window.infoBigGrid.Items.Add(sg);
				}
			}
			for(int i = 0; i<currentGroup.TeachersCount; i++)
				window.infoSmallGrid.Items.Add(MTSystem.findTeacherById(currentGroup.getTeacherId(i)).toSample());
			
		}
		
		/// <summary>
		/// Метод обработки изменения начальной даты периода.
		/// Происходит перерасчет текучки и отработанных часов.
		/// </summary>
		/// <param name="w">Объект окна для отображения.</param>
		public void StartDateChanged(Window1 w)
		{
			currentDateRange.Start = (DateTime) w.groupStartDatePicker.SelectedDate;
			w.groupEndDatePicker.DisplayDateStart = currentDateRange.Start;
			if(currentGroup != null) CountDataFields(w);
		}
		
		/// <summary>
		/// Метод обработки изменения конечной даты периода.
		/// Происходит перерасчет текучки и отработанных часов.
		/// </summary>
		/// <param name="w">Объект окна для отображения.</param>
		public void EndDateChanged(Window1 w)
		{
			currentDateRange.End = ((DateTime) w.groupEndDatePicker.SelectedDate).Add(new TimeSpan(23,59,59));
			w.groupStartDatePicker.DisplayDateEnd = (DateTime) w.groupEndDatePicker.SelectedDate;
			if(currentGroup != null) CountDataFields(w);
		}
		
		/// <summary>
		/// Метод, обрабатывающий результаты поиска из окна результатов поиска.
		/// </summary>
		/// <param name="outputGrid">Сетка текущих учеников группы</param>
		/// <param name="list">Новые ученики группы (возможны повторения с текущими учениками)</param>
		/// <param name="ot">Тип данных результатов поиска</param>
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
								SampleGrid ns = students[i].toSample();
								ns.P5 = "Ходит";
								outputGrid.Items.Add(ns);
							}
							count = 1;
							break;
						}
					}
					if(count == 0)
						outputGrid.Items.Add(students[i].toSample());
					count = 0;
				}
				MessageBox.Show("Ученики добавлены!");
			}
		}
	}
}
