/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 12/04/2017
 * Time: 21:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, реализующий тип достижения.
	/// </summary>
	public class EventType
	{
		public int Id{get;set;}		//Айди типа в базе
		public String Name{get;set;}//Название
		public EventType()
		{
			Id = 0;
			Name = "";
		}
		
		/// <summary>
		/// Метод, конвертирующая объект EventType в объект смежного класса SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public SampleGrid toSample()
		{
			SampleGrid sg = new SampleGrid();
			sg.P1 = Id.ToString();
			sg.P2 = Name;
			return sg;
		}
		
		/// <summary>
		/// Метод проверки объекта на валидность
		/// </summary>
		/// <returns>True если валидный, иначе false</returns>
		public bool IsValid()
		{
			return Id > 0;
		}
	}
}
