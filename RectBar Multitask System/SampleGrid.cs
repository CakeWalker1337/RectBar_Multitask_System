/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 10/12/2017
 * Time: 19:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using System.Windows.Controls;
using System.Windows.Media;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс, предназначенный для вывода данных в таблицы.
	/// Состоит из набора текстовый полей, в которые конвертируются выводимые значения.
	/// </summary>
	public class SampleGrid
	{
		public bool ChB{get;set;}
		public String P1{get;set;}
		public String P2{get;set;}
		public String P3{get;set;}
		public String P4{get;set;}
		public String P5{get;set;}
		public String P6{get;set;}
		public String P7{get;set;}
		public String P8{get;set;}
		
		
		public SampleGrid()
		{
			ChB = false;
			P1 = "";
			P2 = "";
			P3 = "";
			P4 = "";
			P5 = "";
			P6 = "";
			P7 = "";
			P8 = "";		
		}
		
		/// <summary>
		/// Метод, конвертирующий текущий объект в класс Admin
		/// </summary>
		/// <returns>Объект класса Admin</returns>
		public Admin toAdmin()
		{
			Admin adm = new Admin();
			adm.Id = Convert.ToInt32(P1);
			adm.Name = P2;
			adm.Login = P3;
			adm.Pass = P4;
			return adm;
		}
		
		/// <summary>
		/// Метод, конвертирующий текущий объект в класс Teacher
		/// </summary>
		/// <returns>Объект класса Teacher</returns>
		public Teacher toTeacher()
		{			
			return MTSystem.findTeacherById(Convert.ToInt32(P1));	
		}
		
		/// <summary>
		/// Метод, конвертирующий текущий объект в класс Group
		/// </summary>
		/// <returns>Объект класса Group</returns>
		public Group toGroup()
		{
			return MTSystem.findGroupById(Convert.ToInt32(P1));
		}
		
		/// <summary>
		/// Метод, конвертирующий текущий объект в класс Student
		/// </summary>
		/// <returns>Объект класса Student</returns>
		public Student toStudent()
		{
			return MTSystem.LoadStudent(Convert.ToInt32(P1));
		}
	}	
}
