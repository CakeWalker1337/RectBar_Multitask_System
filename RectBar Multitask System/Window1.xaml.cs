/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 02.10.2017
 * Time: 19:12
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
using RectBar_Multitask_System.Presenters;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Главный класс системы.
	/// </summary>
	public partial class Window1 : Window
	{
		//Названия параметров информационной системы
		public String[] sortboxItemNames = {"Ученики", "Группы", "Преподаватели", "Администраторы", "Достижения", "Типы групп", "Уровни групп", "Статусы студентов"};
		//Дни недели для вывода в расписание
		private String[] days = {"Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье"};
		private Schedule currentSchedule = null; //Текущее расписание (выбранное в CB)
		private Group currentUsedGroup = null; //Выбранная группа в редакторе расписания
		private Group currentGroup = null; //Выбранная группа для показа в расписании (сортировка)
		private Group currentLogGroup = null; //Выбранная группа в журнале посещений
		private Session currentSession = null; //Выбранная сессия в журнале посещений
		private int currentSessionAliveStudentsCount = 0; // Костыль для передачи количества "ходящих" студентов в группе
		public User currentUser = null; //Текущий пользователь системы
		public ObjectType currentObjectType = ObjectType.Student; //Текущий тип объекта системы для обновления данных в админ.таблице
		
		GroupControlPresenter groupControlPresenter = null; //Презентер вкладки управления группами для препода
		
		CalendarDateRange currentDateRange = new CalendarDateRange(); //период времени для выведения списка сессий в журнале
		
		public Window1(User u)
		{
			MTSystem.GetAllData(); //Получение всех нужных данных из базы
			if(u.Type == PermType.Teacher) currentUser = u.toTeacher();
			else if(u.Type ==  PermType.Admin) currentUser = u.toAdmin();
			else if(u.Type == PermType.Director) currentUser = u.toDirector();
			else currentUser = null;
			
			if(currentUser.Type == PermType.Teacher)
			{
				groupControlPresenter = new GroupControlPresenter(this);
				
			}
			
			InitializeComponent(); //Инициализация компонентов окна
			InitializeScheduleGrid();
			InitializeScheduleComboBoxes();	
			InitializeLogComboBoxes();
			InitializeAdminComboBoxes();
			
			if(currentUser.Type == PermType.Teacher)
			{
				AdminPanel.Visibility = Visibility.Collapsed;
				createScheduleButton.IsEnabled = false;
				editScheduleButton.IsEnabled = false;
				createRowButton.IsEnabled = false;
				editRowButton.IsEnabled = false;
				createHallButton.IsEnabled = false;
				editHallButton.IsEnabled = false;
				saveScheduleButton.IsEnabled = false;
				groupControlPresenter.InitializeComboBox(this);
			}
			else
			{
				GroupControlPanel.Visibility = Visibility.Collapsed;
				if(currentUser.Type == PermType.Admin)
				{
					createAdminButton.IsEnabled = false;
					createTeacherButton.IsEnabled = false;
				}
			}
			currentDateRange.Start = DateTime.Today.AddDays(-7);
			currentDateRange.End = DateTime.Today.Add(new TimeSpan(23,59,59));
			startDatePicker.SelectedDate = currentDateRange.Start;
			endDatePicker.SelectedDate = currentDateRange.End;
			deleteSession.IsEnabled = false;
			saveSessionButton.IsEnabled = false;
		}
		
		
		/// <summary>
		/// Метод для инициализации расписания. Программно создает необходимое количество столбцов и строк
		/// в зависимости от количества залов и данных о строках.
		/// </summary>
		void InitializeScheduleGrid()
		{
			GridLength colWidth = new GridLength(100);
			GridLength rowHeight = new GridLength(26);
			
			DataGridTextColumn timeColumn = new DataGridTextColumn();
			timeColumn.Binding = new Binding{Path = new PropertyPath("Time"), Converter = new ScheduleTimeColumnConverter()};
			timeColumn.Header = "Время";
			timeColumn.Width = new DataGridLength(60);
			
			scheduleGrid.Columns.Add(timeColumn);
			
			int count = 0;
			for(int j = 0; j<7; j++)
			{
				DataGridTemplateColumn tc = new DataGridTemplateColumn();
				int halls = 0;
				if(MTSystem.HallsCount == 0) halls = 1;
				else halls = MTSystem.HallsCount;
				var hrfact = new FrameworkElementFactory(typeof(Grid));
				hrfact.SetValue(FrameworkElement.WidthProperty, colWidth.Value*halls);
				
				
				for(int i = 0; i<halls; i++)
				{
					var column = new FrameworkElementFactory(typeof(ColumnDefinition));
					column.SetValue(ColumnDefinition.WidthProperty, colWidth);
					hrfact.AppendChild(column);
				}
					
				for(int i = 0; i<2; i++)
				{
					var row = new FrameworkElementFactory(typeof(RowDefinition));
					row.SetValue(RowDefinition.HeightProperty, rowHeight);
					hrfact.AppendChild(row);
				}
			
				var label = new FrameworkElementFactory(typeof(Label));
				label.SetValue(ContentControl.ContentProperty, days[j]);
				label.SetValue(Grid.ColumnSpanProperty, halls);
				label.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
				label.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
				hrfact.AppendChild(label);
				
				for(int i = 0; i<MTSystem.HallsCount; i++)
				{
					var zal = new FrameworkElementFactory(typeof(Label));
					zal.SetValue(ContentControl.ContentProperty, MTSystem.getHall(i).Name);
					zal.SetValue(Grid.ColumnProperty, i);
					zal.SetValue(Grid.RowProperty, 1);
					zal.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
					hrfact.AppendChild(zal);
				}			
			
				DataTemplate dt = new DataTemplate(){VisualTree = hrfact};
				
				tc.HeaderTemplate = dt;
				
				
				var cellFact = new FrameworkElementFactory(typeof(Grid));
				cellFact.SetValue(FrameworkElement.WidthProperty, colWidth.Value*halls);
				cellFact.SetValue(Grid.ShowGridLinesProperty, true);
				
				for(int i = 0; i<MTSystem.HallsCount; i++)
				{
					var column = new FrameworkElementFactory(typeof(ColumnDefinition));
					column.SetValue(ColumnDefinition.WidthProperty, colWidth);
					cellFact.AppendChild(column);
				}	
				
				for(int i = 0; i<MTSystem.HallsCount; i++)
				{
					var tb = new FrameworkElementFactory(typeof(TextBox));
					tb.SetValue(Grid.ColumnProperty, i);
					tb.SetValue(System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyProperty, true);
					tb.SetBinding(TextBox.TextProperty, new Binding{Path = new PropertyPath("GroupIds["+count+"]"), Converter = new ScheduleTextBoxConverter()});
					tb.SetBinding(Control.BackgroundProperty, new Binding{Path = new PropertyPath("GroupIds["+count+"]"), Converter = new ScheduleColorConverter()});
					count++;
					tb.AddHandler(Control.MouseDoubleClickEvent, new MouseButtonEventHandler(ElementDoubleClick));
					tb.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
					tb.SetValue(TextBox.TextWrappingProperty, TextWrapping.WrapWithOverflow);
					cellFact.AppendChild(tb);
				}			
			
				DataTemplate dt2 = new DataTemplate(){VisualTree = cellFact};
				tc.CellTemplate = dt2;
				tc.IsReadOnly = true;
				scheduleGrid.Columns.Add(tc);
			}
		}
		
		/// <summary>
		/// Инициализирует комбобоксы расписаний, групп расписания для сортировки, элементов
		/// редактирования расписания.
		/// Вызывается один раз в начале работы программы, а также каждый раз, когда данные о
		/// расписании меняются (добавление/удаление/изменение расписаний).
		/// </summary>
		public void InitializeScheduleComboBoxes()
		{
			currentSchedule = null;
			currentGroup = null;
			scheduleComboBox.Items.Clear();
			for(int i = 0; i<MTSystem.SchedulesCount; i++)
					scheduleComboBox.Items.Add(MTSystem.getSchedule(i).ScheduleName);
			
			groupComboBox.Items.Clear();
			groupComboBox.Items.Add("Все");
			
			if(currentUser.Type != PermType.Teacher)
			{
				usedGroupComboBox.Items.Clear();
				usedGroupComboBox.Items.Add("Просмотр");
				usedGroupComboBox.Items.Add("Ластик");
				
				for(int i = 0; i<MTSystem.GroupsCount; i++)
					usedGroupComboBox.Items.Add(MTSystem.getGroup(i).Name);
				
				usedGroupComboBox.SelectedIndex = 0;
			}
			else editScheduleBorder.Visibility = Visibility.Hidden;
			for(int i = 0; i<MTSystem.GroupsCount; i++)
			{
				groupComboBox.Items.Add(MTSystem.getGroup(i).Name);	
			}
					
			if(MTSystem.SchedulesCount != 0)
				scheduleComboBox.SelectedIndex = 0;
			groupComboBox.SelectedIndex = 0;
			
		}
		
		/// <summary>
		/// Метод-callback эвента смены расписания в комбобоксе расписаний.
		/// Вызывается один раз в начале (при утверждении значения по-умолчанию), а также
		/// каждый раз, когда происходит смена расписания в комбобоксе или действие с текущим расписанием
		/// (удаление, редактирование).
		/// </summary>
		void scheduleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(scheduleComboBox.SelectedIndex < 0) return;
			currentSchedule = MTSystem.getSchedule(scheduleComboBox.SelectedIndex);
			if(groupComboBox.SelectedIndex != 0) groupComboBox.SelectedIndex = 0;
			else RefreshGroupComboBox();
		}
		
		/// <summary>
		/// Метод-callback эвента смены группы в комбобоксе группы для сортировки.
		/// Вызывается один раз в начале (при утверждении значения по-умолчанию), а также
		/// каждый раз, когда происходит смена группы в комбобоксе или действие с текущей группой
		/// (удаление, редактирование).
		/// </summary>
		void groupCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(scheduleComboBox.SelectedItem == null) return;
			RefreshGroupComboBox();
		}
		
		/// <summary>
		/// Метод, обновляющий данные в комбобоксе группы.
		/// </summary>
		void RefreshGroupComboBox()
		{
			if(groupComboBox.SelectedIndex < 0) return;
			if(groupComboBox.SelectedItem.ToString() == "Все")
			{
				currentGroup = null;
				teacherBox.Text = "";
				RefreshScheduleGrid();
				return;
			}
			
			currentGroup = MTSystem.getGroup(groupComboBox.SelectedIndex-1);
			if(currentGroup.TeachersCount == 0)
				teacherBox.Text = "";
			else
				teacherBox.Text = MTSystem.findTeacherById(currentGroup.getTeacherId(0)).Name;
			RefreshScheduleGrid();
		}
		
		/// <summary>
		/// Метод, обновляющий данные в таблице расписания. Новые данные зависят от переменных системы
		/// (текущей группы, текущего расписания). Метод вызывается при обновлении данных, связанных с 
		/// самим расписанием или его содержимым (например, если название группы, отмеченной в расписании, изменилось).
		/// </summary>
		public void RefreshScheduleGrid()
		{
			scheduleGrid.Items.Clear();
			if(currentSchedule == null)
			{
				return;
			}
			if(currentGroup == null)
			{
				for(int j = 0; j<currentSchedule.RowCount; j++)
				{
					scheduleGrid.Items.Add(currentSchedule.getRow(j));
				}
				return;
			}
			for(int j = 0; j<currentSchedule.RowCount; j++)
			{
				if(currentSchedule.getRow(j).CheckGroupId(currentGroup.Id))
				{
					scheduleGrid.Items.Add(currentSchedule.getRow(j));
				}
			}
		}
		
		
		/// <summary>
		/// Инициализирует комбобокс в админ-панели, который регулирует содержимое таблицы.
		/// Вызывается один раз в начале работы программы, а также каждый раз, когда данные о
		/// содержимом таблицы меняется (добавление/удаление/изменение объектов системы).
		/// </summary>
		public void InitializeAdminComboBoxes()
		{
			if(currentUser is Admin || currentUser is Director)
			{
				for(int i = 0; i<2; i++) sortBox.Items.Add(sortboxItemNames[i]);
				
				if(currentUser is Director)
					for(int i = 2; i<4; i++) sortBox.Items.Add(sortboxItemNames[i]);
				
				for(int i = 4; i<sortboxItemNames.Length; i++) sortBox.Items.Add(sortboxItemNames[i]);
				sortBox.SelectedIndex = 0;
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки создания расписаний.
		/// </summary>
		void createScheduleButton_Click(object sender, RoutedEventArgs e)
		{
			EditScheduleWindow esw = new EditScheduleWindow(WindowOperationFlag.Create);
			esw.ChangeWindowDataEvent += RefreshScheduleGrid_CallBack;
			esw.Show();
		}
		
		/// <summary>
		/// Callback-метод кнопки создания строки расписания.
		/// </summary>
		void createRowButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentSchedule == null)
			{
				MessageBox.Show("Не выбрано расписание!");
				return;
			}
			if(MTSystem.HallsCount == 0)
			{
				MessageBox.Show("Нет залов! Сначала создайте зал.");
				return;
			}
			EditScheduleRowWindow esw = new EditScheduleRowWindow(WindowOperationFlag.Create, currentSchedule);
			esw.ChangeWindowDataEvent += RefreshScheduleRow_CallBack;
			esw.ShowDialog();
		}
		
		/// <summary>
		/// Callback-метод кнопки редактирования расписаний.
		/// </summary>
		void editScheduleButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentSchedule == null)
			{
				MessageBox.Show("Не выбрано расписание!");
				return;
			}
			EditScheduleWindow esw = new EditScheduleWindow(WindowOperationFlag.Edit, currentSchedule);
			esw.ChangeWindowDataEvent += RefreshScheduleGrid_CallBack;
			esw.ShowDialog();
		}
		
		/// <summary>
		/// Callback-метод кнопки редактирования строки расписания.
		/// </summary>
		void editRowButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentSchedule == null)
			{
				MessageBox.Show("Не выбрано расписание!");
				return;
			}
			if(scheduleGrid.SelectedItem == null)
			{
				MessageBox.Show("Не выбрана строка для редактирования!");
				return;
			}
			
			EditScheduleRowWindow esw = new EditScheduleRowWindow(WindowOperationFlag.Edit,((ScheduleRow)scheduleGrid.SelectedItem), currentSchedule);
			esw.ChangeWindowDataEvent += RefreshScheduleRow_CallBack;
			esw.ShowDialog();
		}
		
		/// <summary>
		/// Callback-метод кнопки создания зала.
		/// </summary>
		void createHallButton_Click(object sender, RoutedEventArgs e)
		{
			EditHallWindow esw = new EditHallWindow(WindowOperationFlag.Create);
			esw.ChangeWindowDataEvent += RefreshHall_CallBack;
			esw.ShowDialog();
		}
		
		/// <summary>
		/// Callback-метод кнопки редактирования зала.
		/// </summary>
		void editHallButton_Click(object sender, RoutedEventArgs e)
		{
			if(MTSystem.HallsCount == 0)
			{
				MessageBox.Show("Нет залов! Сначала создайте зал.");
				return;
			}
			EditHallWindow esw = new EditHallWindow(WindowOperationFlag.Edit);
			esw.ChangeWindowDataEvent += RefreshHall_CallBack;
			esw.ShowDialog();
		}
		
		/// <summary>
		/// Callback-метод обновления данных в таблице расписания.
		/// Реинициализирует комбобокс группы и обновляет данные таблицы расписаний.
		/// </summary>
		public void RefreshScheduleGrid_CallBack()
		{	
			InitializeScheduleComboBoxes();			
			RefreshScheduleGrid();
		}
		
		/// <summary>
		/// Callback-метод обновления данных в таблице расписания при изменении зала(-ов).
		/// РЕИНИЦИАЛИЗИРУЕТ таблицу расписаний и обновляет данные в ней. Это происходит,
		/// потому что количество залов может измениться, и таблица поменяет вид.
		/// </summary>
		public void RefreshHall_CallBack()
		{	
			scheduleGrid.Items.Clear();
			scheduleGrid.Columns.Clear();
			InitializeScheduleGrid();
			RefreshScheduleGrid();
		}
		
		/// <summary>
		/// Callback-метод обновления данных в таблице расписания при изменении 
		/// строки (добавлении строки).
		/// </summary>
		public void RefreshScheduleRow_CallBack()
		{
			RefreshScheduleGrid();
		}
		
		/// <summary>
		/// Метод-callback, обрабатывающий двойной клик по ячейке расписания.
		/// Редактирует ячейку в зависимости от выбранного элемента редактирования в меню.
		/// </summary>
		void ElementDoubleClick(Object sender, MouseButtonEventArgs e)
		{
			if(usedGroupComboBox.SelectedIndex <= 0) return;
			if(usedGroupComboBox.SelectedIndex == 1)
			{
				if(sender is TextBox)
				{
					TextBox tb = (TextBox) sender;
					tb.Text = "Пусто";
					tb.Background = Brushes.White;
				}
				return;
			}
			if(usedGroupComboBox.SelectedIndex > 1)
			{
				if(sender is TextBox)
				{
					TextBox tb = (TextBox) sender;
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("{0}{1}{2}{1}{3}{1}{4}", currentUsedGroup.Name, Environment.NewLine, currentUsedGroup.Type.Name,
					                currentUsedGroup.Level.Name, (currentUsedGroup.TeachersCount == 0)?"":MTSystem.findTeacherById(currentUsedGroup.getTeacherId(0)).Name);
					tb.Text = sb.ToString();
					tb.Background = currentUsedGroup.Color;
					((ScheduleRow)scheduleGrid.SelectedItem).GroupIds[((scheduleGrid.CurrentColumn.DisplayIndex)-1)*MTSystem.HallsCount+((Grid)tb.Parent).Children.IndexOf(tb)] = currentUsedGroup.Id;
				}
				return;
			}				
		}
		
		/// <summary>
		/// Метод-callback, обрабатывающий изменение данных в меню редактирования расписания.
		/// </summary>
		void usedGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(usedGroupComboBox.SelectedIndex < 0) return;
			if(usedGroupComboBox.SelectedIndex > 1)
			{
				currentUsedGroup = MTSystem.getGroup(usedGroupComboBox.SelectedIndex-2);
				return;
			}
			currentUsedGroup = null;
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку сохранения расписания.
		/// </summary>
		void SaveScheduleButton_Click(object sender, RoutedEventArgs e)
		{
			if(MTSystem.SaveSchedule(currentSchedule))
			{
				MessageBox.Show("Сохранение успешно завершено!");
			}
			else MessageBox.Show("Ошибка сохранения!");
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку обновления данных в системе.
		/// </summary>
		void refreshDataButton_Click(object sender, RoutedEventArgs e)
		{
			MTSystem.GetAllData();
			
			RefreshScheduleGrid_CallBack();
			InitializeLogComboBoxes();
			if(currentUser.Type == PermType.Teacher)
			{
				currentUser = MTSystem.findTeacherById(currentUser.Id);
				groupControlPresenter.InitializeComboBox(this);
				groupControlPresenter.RefreshData(this);
			}
			else
			{
				RefreshAdminGrid();
			}
			
			MessageBox.Show("Данные обновлены!");
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку выхода из системы.
		/// </summary>
		void logOutButton_Click(object sender, RoutedEventArgs e)
		{
			LogWindow.WriteToRemeberDataFile(false, "NULL", "NULL");
			LogWindow w = new LogWindow();
			w.Show();
			this.Close();
		}
		
///*/////////////////////////////////////////////////////VISIT LOG///////////////////////////////////////////////////////*/

		/// <summary>
		/// Инициализация комбобоксов в журнале посещений
		/// </summary>
		public void InitializeLogComboBoxes()
		{
			groupLogComboBox.Items.Clear();
			if(currentUser.Type != PermType.Teacher)
			{
				groupLogComboBox.Items.Add("Все");
				for(int i = 0; i<MTSystem.GroupsCount; i++)
					groupLogComboBox.Items.Add(MTSystem.getGroup(i).Name);
				groupLogComboBox.SelectedIndex = 0;
			}
			else
			{
				Teacher t = (Teacher) currentUser;
				for(int i = 0; i<t.GroupIdsCount; i++)
					groupLogComboBox.Items.Add(MTSystem.findGroupById(t.getGroupId(i)).Name);
				groupLogComboBox.SelectedIndex = 0;
			}
			
		}

		/// <summary>
		/// Метод-callback, отвечающий за смену даты начала периода для вывода сессий в журнал.
		/// </summary>
		void startDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			currentDateRange.Start = (DateTime) startDatePicker.SelectedDate;
			endDatePicker.DisplayDateStart = currentDateRange.Start;
			RefreshLogGrid();
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за смену даты конца периода для вывода сессий в журнал.
		/// </summary>
		void endDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			currentDateRange.End = ((DateTime) endDatePicker.SelectedDate).Add(new TimeSpan(23,59,59));
			startDatePicker.DisplayDateEnd = ((DateTime) endDatePicker.SelectedDate);	
			RefreshLogGrid();
		}

		/// <summary>
		/// Метод, обновляющий сетку сессий в журнале посещений
		/// </summary>
		void RefreshLogGrid()
		{
			logGrid.Items.Clear();
			
			if(currentLogGroup == null)
			{
				if(currentUser.Type == PermType.Teacher) return;
				
				List<Session> l = MTSystem.LoadSessionsByTime(currentDateRange.Start, currentDateRange.End);
				if(l[0] == null)
				{
					return;
				}
				for(int i = 0; i<l.Count; i++)
					logGrid.Items.Add(l[i].toSample());
			}
			else
			{
				List<Session> l = MTSystem.LoadSessionsByGroupIdByTime(currentLogGroup.Id ,currentDateRange.Start, currentDateRange.End);
				if(l[0] == null)
				{
					return;
				}
				for(int i = 0; i<l.Count; i++)
					logGrid.Items.Add(l[i].toSample());
			}
		}
		
		/// <summary>
		/// Callback-метод для кнопки создания сессии для журнала. Физически не сможет вызваться,
		/// если не выбрана группа.
		/// </summary>
		void createSessionButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentLogGroup == null)
			{
				MessageBox.Show("Группа не выбрана!");
				return;
			}
			sessionGrid.Items.Clear();
			int aliveStudents = 0;
			for(int i = 0; i<currentLogGroup.StudentsCount; i++)
			{
				Student st = MTSystem.LoadStudent(currentLogGroup.getStudentId(i));
				if(st != null)
				{
					if(st.checkGroupId(currentLogGroup.Id))
					{
						aliveStudents++;
						SampleGrid s = st.toSample();
						sessionGrid.Items.Add(s);
					}
				}
			}
			currentSessionAliveStudentsCount = aliveStudents;
			saveSessionButton.IsEnabled = true;
			sessionDatePicker.SelectedDate = null;
		}
		
		/// <summary>
		/// Callback-метод для кнопки удаления сессии для журнала. Физически не сможет вызваться,
		/// если не выбрана сессия для удаления.
		/// </summary>
		void deleteSessionButton_Click(object sender, RoutedEventArgs e)
		{
			if(currentSession != null)
			{
				if(MTSystem.DeleteSession(currentSession.Id))
				{
					currentSession = null;
					sessionGrid.Items.Clear();
					deleteSession.IsEnabled = false;
					saveSessionButton.IsEnabled = false;
					sessionDatePicker.SelectedDate = null;
					RefreshLogGrid();
					MessageBox.Show("Удаление успешно!");
				}
					
				else MessageBox.Show("Ошибка удаления!");
			}
			else MessageBox.Show("Сессия не выбрана!");
		}
		
		/// <summary>
		/// Callback-метод для кнопки сохранения сессии для журнала. В нем осуществляются
		/// проверки на корректность введеных времени и длительности.
		/// </summary>
		void saveSessionButton_Click(object sender, RoutedEventArgs e)
		{
			int buf = 0;
			if(!Int32.TryParse(durationBlock.Text, out buf) || buf <= 0 || buf > 180)
			{
				MessageBox.Show("Некорректное значение длительности сессии!");
				return;
			}
			if(sessionDatePicker.SelectedDate.HasValue == false)
			{
				MessageBox.Show("Некорректное значение даты сессии!");
				return;
			}
			if(currentSession == null)
			{
				currentSession = new Session();
				currentSession.GroupId = currentLogGroup.Id;
				currentSession.Time = (DateTime) sessionDatePicker.SelectedDate;
				currentSession.UserId = currentUser.Id;
				currentSession.StudentsCount = currentSessionAliveStudentsCount;
			}
			currentSession.Duration = buf;
			currentSession.ClearAbsentIds();
			currentSession.ClearPresentIds();
			for(int i = 0; i<sessionGrid.Items.Count; i++)
			{
				SampleGrid sg = (SampleGrid)sessionGrid.Items[i];
				if(sg.ChB == true)
					currentSession.addPresentId(Convert.ToInt32(sg.P1));
				else
					currentSession.addAbsentId(Convert.ToInt32(sg.P1));				
			}
			if(currentSession.IsValid())
			{
				if(MTSystem.SaveSession(currentSession))
				{
					MessageBox.Show("Сохранение успешно");
				}
				else MessageBox.Show("Ошибка сохранения!");
			}
			else
			{
				MTSystem.CreateSession(currentSession);
				MessageBox.Show("Сохранение успешно");
			}
			currentSession = null;
			deleteSession.IsEnabled = false;
			saveSessionButton.IsEnabled = false;
			sessionDatePicker.SelectedDate = null;
			sessionGrid.Items.Clear();
			RefreshLogGrid();
			if(currentUser.Type == PermType.Teacher)
				groupControlPresenter.RefreshData(this);
			else
				RefreshAdminGrid();
		}
		
		/// <summary>
		/// Callback-метод эвента смены выбранной группы для сортировки сессий. 
		/// </summary>
		void groupLogComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(groupLogComboBox.SelectedItem == null) return;
			currentSession = null;
			sessionGrid.Items.Clear();
			saveSessionButton.IsEnabled = false;
			durationBlock.Text = "";
			sessionDatePicker.SelectedDate = null;
			deleteSession.IsEnabled = false;
			if(groupLogComboBox.SelectedItem.ToString() == "Все")
			{
				currentLogGroup = null;
				RefreshLogGrid();
				return;
			}
			for(int i = 0; i<MTSystem.GroupsCount; i++)
			{
				if(groupLogComboBox.SelectedItem.ToString() == MTSystem.getGroup(i).Name)
				{
					currentLogGroup = MTSystem.getGroup(i);
					
					RefreshLogGrid();
					break;
				}
			}
			
		}
		
		/// <summary>
		/// Callback-метод эвента появления галки в строке с учеником при редактировании/создании сессии.
		/// </summary>
		void sessionCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if(sessionGrid.SelectedItem == null) return;
			((SampleGrid)sessionGrid.SelectedItem).ChB = true;
		}
		
		/// <summary>
		/// Callback-метод эвента удаления галки в строке с учеником при редактировании/создании сессии.
		/// </summary>
		void sessionCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if(sessionGrid.SelectedItem == null) return;
			((SampleGrid)sessionGrid.SelectedItem).ChB = false;
		}
		
		/// <summary>
		/// Callback-метод эвента двойного клика по строке в таблице сессий.
		/// </summary>
		void logGrid_DoubleClick(object sender, RoutedEventArgs e)
		{
			if(logGrid.SelectedItem == null) return;
			currentSession = MTSystem.LoadSessionById(Convert.ToInt32(((SampleGrid)logGrid.SelectedItem).P1));
			if(currentSession == null)
			{
				MessageBox.Show("Ошибка");
				return;
			}
			deleteSession.IsEnabled = true;
			saveSessionButton.IsEnabled = true;
			sessionDatePicker.SelectedDate = currentSession.Time.Date;
			sessionGrid.Items.Clear();
			for(int i = 0; i<currentSession.PresentIdsCount; i++)
			{
				Student s = MTSystem.LoadStudent(currentSession.getPresentId(i));
				if(s != null)
				{
					SampleGrid sg = s.toSample();
					sg.ChB = true;
					sessionGrid.Items.Add(sg);
				}
			}
			for(int i = 0; i<currentSession.AbsentIdsCount; i++)
			{
				Student s = MTSystem.LoadStudent(currentSession.getAbsentId(i));
				if(s != null)
				{
					SampleGrid sg = s.toSample();
					sg.ChB = false;
					sessionGrid.Items.Add(sg);
				}
			}
			durationBlock.Text = currentSession.Duration.ToString();
		}
		
		
/////////////////////////////////////////////////////////////////////ADMIN_PANEL/////////////////////////////////////////		
		
		/// <summary>
		/// Callback-метод эвента клика по кнопке поиска в админ-панели.
		/// Вводимые данные могут быть строкой (имя, название или их часть) или айди в БД.
		/// </summary>
		void findButton_Click(object sender, RoutedEventArgs e)
		{
			if(findTextBox.Text == "")
			{
				MessageBox.Show("Пустой поисковой запрос!");
				return;
			}
			if(currentObjectType == ObjectType.Admin)
			{
				List<Admin> adm = Finder.FindAdmins(findTextBox.Text);
				if(adm[0] == null)
				{
					MessageBox.Show("Администраторы не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<adm.Count; i++)
				{
					adminMainGrid.Items.Add(adm[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.Group)
			{
				List<Group> gr = Finder.FindGroups(findTextBox.Text);
				if(gr[0] == null)
				{
					MessageBox.Show("Группы не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<gr.Count; i++)
				{
					adminMainGrid.Items.Add(gr[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.Student)
			{
				List<Student> st = Finder.FindStudents(findTextBox.Text);
				if(st[0] == null)
				{
					MessageBox.Show("Ученики не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<st.Count; i++)
				{
					adminMainGrid.Items.Add(st[i].toSample());
				}
				
			}
			else if(currentObjectType == ObjectType.Teacher)
			{
				List<Teacher> tc = Finder.FindTeachers(findTextBox.Text);
				if(tc[0] == null)
				{
					MessageBox.Show("Преподаватели не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<tc.Count; i++)
				{
					adminMainGrid.Items.Add(tc[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.EventType)
			{
				List<EventType> et = Finder.FindEventTypes(findTextBox.Text);
				if(et[0] == null)
				{
					MessageBox.Show("Достижения не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<et.Count; i++)
				{
					adminMainGrid.Items.Add(et[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.GroupType)
			{
				List<GroupType> et = Finder.FindGroupTypes(findTextBox.Text);
				if(et[0] == null)
				{
					MessageBox.Show("Типы групп не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<et.Count; i++)
				{
					adminMainGrid.Items.Add(et[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.GroupLevel)
			{
				List<GroupLevel> et = Finder.FindGroupLevels(findTextBox.Text);
				if(et[0] == null)
				{
					MessageBox.Show("Уровни групп не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<et.Count; i++)
				{
					adminMainGrid.Items.Add(et[i].toSample());
				}
			}
			else if(currentObjectType == ObjectType.StudentStatus)
			{
				List<StudentStatus> et = Finder.FindStudentStatuses(findTextBox.Text);
				if(et[0] == null)
				{
					MessageBox.Show("Статусы учеников не найдены!");
					return;
				}
				adminMainGrid.Items.Clear();
				for(int i = 0; i<et.Count; i++)
				{
					adminMainGrid.Items.Add(et[i].toSample());
				}
			}
		}	
	
		/// <summary>
		/// Callback-метод эвента изменения содержимого комбобокса, регулирующего содержимое таблицы админ-панели.
		/// </summary>
		void findCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RefreshAdminGrid();
		}
		
		/// <summary>
		/// Метод обновляющий содержимое таблицы админ-панели.
		/// </summary>
		void RefreshAdminGrid()
		{
			if(sortBox.SelectedItem.ToString() == "Ученики")
			{
				SelectGridHeaders(ObjectType.Student);
				adminMainGrid.Items.Clear();
				List<Student> students = MTSystem.GetAllStudents();
				for(int i = 0; i<students.Count; i++)
				{
					adminMainGrid.Items.Add(students[i].toSample());
				}
				currentObjectType = ObjectType.Student;
			}
			else if(sortBox.SelectedItem.ToString() == "Группы")
			{
				SelectGridHeaders(ObjectType.Group);
				adminMainGrid.Items.Clear();
				for(int i = 0; i<MTSystem.GroupsCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getGroup(i).toSample());
				}
				currentObjectType = ObjectType.Group;
			}
			else if(sortBox.SelectedItem.ToString() == "Преподаватели")
			{
				SelectGridHeaders(ObjectType.Teacher);
				adminMainGrid.Items.Clear();
				for(int i = 0; i<MTSystem.TeachersCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getTeacher(i).toSample());
				}
				currentObjectType = ObjectType.Teacher;
				
			}
			else if (sortBox.SelectedItem.ToString() == "Администраторы")
			{
				SelectGridHeaders(ObjectType.Admin);
				adminMainGrid.Items.Clear();
				currentObjectType = ObjectType.Admin;
				List<Admin> l = MTSystem.GetAllAdmins();
				for (int i = 0; i < l.Count; i++)
				{
					adminMainGrid.Items.Add(l[i].toSample());
				}
			}
			else if(sortBox.SelectedItem.ToString() == "Достижения")
			{
				SelectGridHeaders(ObjectType.EventType);
				adminMainGrid.Items.Clear();
				currentObjectType = ObjectType.EventType;
				for(int i = 0; i<MTSystem.EventTypesCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getEventType(i).toSample());
				}
			}
			else if(sortBox.SelectedItem.ToString() == "Типы групп")
			{
				SelectGridHeaders(ObjectType.GroupType);
				adminMainGrid.Items.Clear();
				currentObjectType = ObjectType.GroupType;
				for(int i = 0; i<MTSystem.GroupTypesCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getGroupType(i).toSample());
				}
			}
			else if(sortBox.SelectedItem.ToString() == "Уровни групп")
			{
				SelectGridHeaders(ObjectType.GroupLevel);
				adminMainGrid.Items.Clear();
				currentObjectType = ObjectType.GroupLevel;
				for(int i = 0; i<MTSystem.GroupLevelCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getGroupLevel(i).toSample());
				}
			}
			else if(sortBox.SelectedItem.ToString() == "Статусы студентов")
			{
				SelectGridHeaders(ObjectType.StudentStatus);
				adminMainGrid.Items.Clear();
				currentObjectType = ObjectType.StudentStatus;
				for(int i = 0; i<MTSystem.StudentStatusesCount; i++)
				{
					adminMainGrid.Items.Add(MTSystem.getStudentStatus(i).toSample());
				}
			}
		}
		
		/// <summary>
		/// Метод изменения колонок в таблице админ-панели.
		/// </summary>
		/// <param name="ot">Тип объекта, от которого зависит результат изменения таблицы</param>
		private void SelectGridHeaders(ObjectType ot)
		{
			if(ot == ObjectType.Student)
			{
				for(int i = 4; i<adminMainGrid.Columns.Count; i++) adminMainGrid.Columns[i].Visibility = Visibility.Hidden;
				adminMainGrid.Columns[2].Visibility = Visibility.Visible;
				adminMainGrid.Columns[0].Header = "ID";
				adminMainGrid.Columns[1].Header = "Имя";
				adminMainGrid.Columns[2].Header = "Возраст";
				adminMainGrid.Columns[3].Header = "Статус";
				return;
			}
			if(ot == ObjectType.Group)
			{
				for(int i = 2; i<adminMainGrid.Columns.Count; i++) adminMainGrid.Columns[i].Visibility = Visibility.Visible;
				adminMainGrid.Columns[0].Header = "ID";
				adminMainGrid.Columns[1].Header = "Название";
				adminMainGrid.Columns[2].Header = "Тип";
				adminMainGrid.Columns[3].Header = "Уровень";
				adminMainGrid.Columns[4].Header = "Кол-во часов";
				adminMainGrid.Columns[5].Header = "План. часов";
				adminMainGrid.Columns[6].Header = "Кол-во учеников";
				return;
			}
			if(ot == ObjectType.Teacher || ot == ObjectType.Admin)
			{
				for(int i = 2; i<4; i++) adminMainGrid.Columns[i].Visibility = Visibility.Visible;
				for(int i = 4; i<adminMainGrid.Columns.Count; i++) adminMainGrid.Columns[i].Visibility = Visibility.Hidden;
				adminMainGrid.Columns[0].Header = "ID";
				adminMainGrid.Columns[1].Header = "Имя";
				adminMainGrid.Columns[2].Header = "Логин";
				adminMainGrid.Columns[3].Header = "Пароль";
				return;
			}
			if(ot == ObjectType.EventType || ot == ObjectType.GroupType || ot == ObjectType.GroupLevel || ot == ObjectType.StudentStatus)
			{
				adminMainGrid.Columns[0].Header = "ID";
				adminMainGrid.Columns[1].Header = "Название";
				for(int i = 2; i<adminMainGrid.Columns.Count; i++) adminMainGrid.Columns[i].Visibility = Visibility.Hidden;
				return;
			}
		}
		
		/// <summary>
		/// Callback-метод двойного клика по таблице админ-панели. Вызывает окно редактирования объекта,
		/// на который кликнули.
		/// </summary>
		void EditObject(object sender, RoutedEventArgs e)
		{
			if(adminMainGrid.SelectedItem == null) return;
			if(currentObjectType == ObjectType.Admin)
			{
				EditWindow ew = new EditWindow(((SampleGrid)adminMainGrid.SelectedItem).toAdmin());
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.Student)
			{
				EditWindow ew = new EditWindow(((SampleGrid)adminMainGrid.SelectedItem).toStudent());
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.Teacher)
			{
				EditWindow ew = new EditWindow(MTSystem.findTeacherById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.Group)
			{
				EditWindow ew = new EditWindow(MTSystem.findGroupById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.EventType)
			{
				EditSmallWindow ew = new EditSmallWindow(MTSystem.findEventTypeById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.GroupLevel)
			{
				EditSmallWindow ew = new EditSmallWindow(MTSystem.findGroupLevelById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.GroupType)
			{
				EditSmallWindow ew = new EditSmallWindow(MTSystem.findGroupTypeById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(currentObjectType == ObjectType.StudentStatus)
			{
				EditSmallWindow ew = new EditSmallWindow(MTSystem.findStudentStatusById(Convert.ToInt32(((SampleGrid)adminMainGrid.SelectedItem).P1)));
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			
		}
		
		/// <summary>
		/// Callback-метод эвента клика по одной из кнопок создания основных параметров системы 
		/// (группы, ученика, администратора, преподавателя).
		/// </summary>
		void CreateMainButton_Click(object sender, RoutedEventArgs e)
		{
			if(((Button)sender).Name == "createGroupButton")
			{
				EditWindow ew = new EditWindow(ObjectType.Group);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createTeacherButton")
			{
				EditWindow ew = new EditWindow(ObjectType.Teacher);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createAdminButton")
			{
				EditWindow ew = new EditWindow(ObjectType.Admin);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createStudentButton")
			{
				EditWindow ew = new EditWindow(ObjectType.Student);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
		}
		
		/// <summary>
		/// Callback-метод эвента клика по одной из кнопок создания дополнительных параметров системы 
		/// (типа группы, уровня группы, статуса ученика, типа достижения).
		/// </summary>
		void CreateOtherButton_Click(object sender, RoutedEventArgs e)
		{
			if(((Button)sender).Name == "createEventTypeButton")
			{
				EditSmallWindow ew = new EditSmallWindow(ObjectType.EventType);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createStudentStatusButton")
			{
				EditSmallWindow ew = new EditSmallWindow(ObjectType.StudentStatus);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createGroupLevelButton")
			{
				EditSmallWindow ew = new EditSmallWindow(ObjectType.GroupLevel);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
			if(((Button)sender).Name == "createGroupTypeButton")
			{
				EditSmallWindow ew = new EditSmallWindow(ObjectType.GroupType);
				ew.SaveWindowDataEvent += RefreshAdminGrid_CallBack;
				ew.ShowDialog();
				return;
			}
		}
		
		/// <summary>
		/// Метод обновления содержимого элементов системы при изменении данных 
		/// (изменении группы, препода, и т.д).
		/// </summary>
		/// <param name="ot"></param>
		public void RefreshAdminGrid_CallBack(ObjectType ot)
		{
			if(ot == ObjectType.Teacher)
			{
				if(currentObjectType == ObjectType.Teacher)
					RefreshAdminGrid();
				InitializeLogComboBoxes();
				RefreshScheduleGrid();
			}
			else if(ot == ObjectType.Group)
			{
				if(currentObjectType == ObjectType.Group)
					RefreshAdminGrid();
				InitializeLogComboBoxes();
				InitializeScheduleComboBoxes();
			}
			else if(ot == ObjectType.Admin)
			{
				if(currentObjectType == ObjectType.Admin)
					RefreshAdminGrid();
			}
			else if(ot == ObjectType.Student)
			{
				if(currentObjectType == ObjectType.Student || currentObjectType == ObjectType.Group)
					RefreshAdminGrid();
			}
			else if(ot == ObjectType.EventType)
			{
				if(currentObjectType == ObjectType.EventType)
					RefreshAdminGrid();
			}
			else if(ot == ObjectType.GroupType)
			{
				if(currentObjectType == ObjectType.GroupType || currentObjectType == ObjectType.Group)
					RefreshAdminGrid();
				RefreshScheduleGrid();
			}
			else if(ot == ObjectType.GroupLevel)
			{
				if(currentObjectType == ObjectType.GroupLevel || currentObjectType == ObjectType.Group)
					RefreshAdminGrid();
				RefreshScheduleGrid();
			}
			else if(ot == ObjectType.StudentStatus)
			{
				if(currentObjectType == ObjectType.StudentStatus || currentObjectType == ObjectType.Student)
					RefreshAdminGrid();
			}
		}
		
//////////////////////////////
		
		
		/// <summary>
		/// Метод-callback эвента изменения содержимого комбобокса
		/// группы во вкладке управления группой (только для преподавателя).
		/// </summary>
		void groupControlComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			groupControlPresenter.GroupControlComboBox_SelectionChanged(this);
		}
		
		/// <summary>
		/// Метод-callback эвента сохранения данных о группе
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			groupControlPresenter.SaveGroup(infoBigGrid);
		}
		
		/// <summary>
		/// Метод-callback эвента отмены изменений данных о группе
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			groupControlPresenter.RefreshData(this);
		}
		
		/// <summary>
		/// Метод-callback эвента удаления данных о группе
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void deleteButtonBig_Click(object sender, RoutedEventArgs e)
		{
			groupControlPresenter.DeleteStudent(infoBigGrid);
		}
		
		/// <summary>
		/// Метод-callback эвента добавления учеников в группу
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void addButtonBig_Click(object sender, RoutedEventArgs e)
		{
			groupControlPresenter.AddStudent(infoBigGrid, findBoxBig.Text);
		}
		
		/// <summary>
		/// Метод-callback эвента изменения начальной даты периода для вычисления характеристик группы
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void groupStartDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			groupControlPresenter.StartDateChanged(this);
		}
		
		/// <summary>
		/// Метод-callback эвента изменения конечной даты периода для вычисления характеристик группы
		/// во вкладке управления группой (только для преподавателя).
		/// </summary>
		void groupEndDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			groupControlPresenter.EndDateChanged(this);
		}
	}
}