/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 20:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Description of StudentStatus.
	/// </summary>
	public class StudentStatus
	{
		public int Id {get; set;}
		public String Name {get; set;}
		public StudentStatus()
		{
			Id = 0;
			Name = "Не выбрано";
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
	}
}
