/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 01/06/2018
 * Time: 13:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Перечисление типов привилегий
	/// </summary>
	public enum PermType
	{
		Error = 0,
		Teacher = 1,
		Admin = 2,
		Director = 3,
		All = 4
	};
	
	/// <summary>
	/// Перечисление флагов операций
	/// </summary>
	public enum WindowOperationFlag
	{
		Create = 0,
		Edit = 1
	};
	
	/// <summary>
	/// Перечисление флагов кнопок в окне авторизации
	/// </summary>
	public enum ButtonFlags
	{
		Options = 1,
		LogIn = 2,
	};
	
	/// <summary>
	/// Перечисление строчных типов данных во вводимых пользователем строках 
	/// </summary>
	public enum DataType
	{
		Login = 1,
		Name = 2
	};
	
	/// <summary>
	/// Перечисление типов объектов системы
	/// </summary>
	public enum ObjectType
	{
		Error = 0,
		Student = 1,
		Group = 2,
		Teacher = 3,
		Admin = 4,
		EventType = 5,
		StudentStatus = 6,
		GroupType = 7,
		GroupLevel = 8,
		Schedule = 9,
		ScheduleRow = 10,
		Hall = 11
	};
}