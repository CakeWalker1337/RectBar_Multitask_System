/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 07.10.2017
 * Time: 16:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс-наследник от User, но с привилегией Director.
	/// </summary>
	public class Director : User
	{
		public Director()
		{
			Type = PermType.Director;
		}
	}
}
