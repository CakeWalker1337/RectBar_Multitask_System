/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 19:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс-конвертер значений цветов в расписании
	/// </summary>
	public class ScheduleColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if((int)value == 0)
			{
				return Brushes.White;
			}
			Group g = MTSystem.findGroupById((int)value); 
			return g.Color; //возвращает значение цвета группы
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
        	return 0;
        }
	}
	
	/// <summary>
	/// Класс-конвертер значений в ячейках расписании
	/// </summary>
	public class ScheduleTextBoxConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if((int)value == 0)
			{
				return "Пусто";
			}
			Group g = MTSystem.findGroupById((int)value);
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}{1}{2}{1}{3}{1}{4}", g.Name, Environment.NewLine, g.Type.Name, g.Level.Name, (g.TeachersCount == 0)?"":MTSystem.findTeacherById(g.getTeacherId(0)).Name);
			return sb.ToString(); //возвращает форматированную строку в ячейке расписания (название группы, тип группы, уровень, имя препода)
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
        	return 0;
        }
		
		
	}
	
	
	/// <summary>
	/// Класс-конвертер значений времени в расписании
	/// </summary>
	public class ScheduleTimeColumnConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			TimeSpan Time = (TimeSpan) value;
			return string.Format("{0}{1}:{2}{3}",(Time.Hours<10)? "0" : "", Time.Hours, (Time.Minutes<10)?"0":"", Time.Minutes);
			//возвращает форматированное значение времени в формате (HH:mm)
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
        	return 0;
        }
		
		
	}
	
}