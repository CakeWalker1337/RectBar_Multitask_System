/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 16:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс с типом привилегии Admin.
	/// </summary>
	public class Admin : User
	{
		public Admin()
		{
			Type = PermType.Admin;
		}
		
		/// <summary>
		/// Преобразует объект Admin в объект смежного класса Sample
		/// </summary>
		/// <returns>Объект класса SampleGrid</returns>
		public new SampleGrid toSample()
		{
			return base.toSample();
		}
	}
}
