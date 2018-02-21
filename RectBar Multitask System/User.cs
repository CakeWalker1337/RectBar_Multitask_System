/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 16:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Description of User.
	/// </summary>
	///
	
	
	
	public class User
	{
		public int Id {get; set;}
		public String Login {get; set;}
		public String Pass {get; set;}
		public String Name {get; set;}
		public PermType Type {get; set;}
		
		public User()
		{
			Id = -1;
			Login = "";
			Pass = "";
			Name = "";
			Type = PermType.Error;
		}
		
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс SampleGrid
		/// </summary>
		/// <returns>Возвращает объект SampleGrid</returns>
		public virtual SampleGrid toSample()
		{
			SampleGrid sg = new SampleGrid();
			sg.P1 = Id.ToString();
			sg.P2 = Name;
			sg.P3 = Login;
			sg.P4 = Pass;
			return sg;
		}
		
		/// <summary>
		/// Проверяет объект на валидность (id>0)
		/// </summary>
		/// <returns>True - валидный, иначе false</returns>
		public bool IsValid()
		{
			return (Id>0);
		}

		/// <summary>
		/// Метод, конвертирующая текущий объект в класс Teacher
		/// Используется только после проверки на соответствие классу Teacher
		/// </summary>
		/// <returns>Возвращает объект Teacher</returns>
		public Teacher toTeacher()
		{
			if(!IsValid()) return null;
			return MTSystem.findTeacherById(Id);
		}
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс Director
		/// Используется только после проверки на соответствие классу Director
		/// </summary>
		/// <returns>Возвращает объект Director</returns>
		public Director toDirector()
		{
			if(!IsValid()) return null;
			Director t = new Director();
			t.Id = Id;
			t.Login = Login;
			t.Pass = Pass;
			t.Name = Name;
			return t;
		}
		
		
		/// <summary>
		/// Метод, конвертирующая текущий объект в класс Admin
		/// Используется только после проверки на соответствие классу Admin
		/// </summary>
		/// <returns>Возвращает объект Admin</returns>
		public Admin toAdmin()
		{
			if(!IsValid()) return null;
			Admin t = new Admin();
			t.Id = Id;
			t.Login = Login;
			t.Pass = Pass;
			t.Name = Name;
			return t;
		}
	}
}
