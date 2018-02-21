/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, описывающий уровень группы.
	/// </summary>
	public class GroupLevel
	{
		public int Id {get; set;} // айди в базе
		public String Name {get; set;} //Название
		public GroupLevel()
		{
			Clear();
		}
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public SampleGrid toSample()
		{
			SampleGrid s = new SampleGrid();
			s.P1 = Id.ToString();
			s.P2 = Name;
			return s;
		}
		
		/// <summary>
		/// Проверяет объект на валидность (id>0)
		/// </summary>
		/// <returns>True - валидный, иначе false</returns>
		public bool IsValid()
		{
			return Id > 0;
		}
		
		/// <summary>
		/// Метод очистки данных объекта
		/// </summary>
		public void Clear()
		{
			Id = 0;
			Name = "Не выбрано";
		}
	}
}
