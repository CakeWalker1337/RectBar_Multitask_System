/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 28.12.2017
 * Time: 20:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, содержащий информацию о зале.
	/// </summary>
	public class Hall
	{ 
		public int Id{get;set;} //айди зала в БД
		public String Name{get;set;} // Название зала
		public Hall()
		{
			Id = 0;
			Name = "";
		}
		
		/// <summary>
		/// Проверяет объект на валидность (id>0)
		/// </summary>
		/// <returns>True - валидный, иначе false</returns>
		public bool IsValid()
		{
			return Id>0;
		}
	}
}
