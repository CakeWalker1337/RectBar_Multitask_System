/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 16:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс достижений ученика
	/// </summary>
	public class Achievement
	{
		
		public int Type {get; set;}  //ID типа достижения
		public int Place {get; set;} //Место (1, 2, 3, участие)
		
		public Achievement()
		{
			Type = -1;
			Place = -1;
		}
		
		/// <summary>
		/// Метод для парсинга строки и разделения ее на достижения
		/// </summary>
		/// <param name="s"> Строка для разделения (формат "_t-p_t-p_" , где t - тип достижения, p - место) </param>
		/// <returns>Лист достижений</returns>
		public static List<Achievement> GetAchievementsFromString(String s)
		{
			List<Achievement> l = new List<Achievement>();
			if(s == "_") return l;
			String[] res = s.Split('_');
			
			String[] buf2;
			for(int i = 1; i<res.Length - 1; i++)
			{
				buf2 = res[i].Split('-');
				Achievement pair = new Achievement();
				pair.Type = Convert.ToInt32(buf2[0]);  //айди достижения
				pair.Place = Convert.ToInt32(buf2[1]); //место
				l.Add(pair);
			}
			return l;
		}
		
		/// <summary>
		/// Метод преобразует объект Achievement в объект смежного класса SampleGrid
		/// </summary>
		/// <returns>Объект класса SampleGrid</returns>
		public SampleGrid toSample()
		{
			SampleGrid sp = new SampleGrid();
			EventType et = MTSystem.findEventTypeById(Type);
			sp.P1 = et.Id.ToString();
			sp.P2 = et.Name;
			sp.P3 = (Place == 0)?"Участвовал":Place.ToString();
			return sp;
		}
		
	}
}
