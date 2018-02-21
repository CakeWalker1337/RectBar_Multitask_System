/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 04.12.2017
 * Time: 20:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RectBar_Multitask_System
{
	/// <summary>
	/// Класс окна подтверждения ConfirmWindow
	/// </summary>
	public partial class ConfirmWindow : Window
	{		
		public ConfirmWindow(String message)
		{
			InitializeComponent();
			messageBlock.Text = message;
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку подтверждения
		/// </summary>
		void confirmButton_Click(object sender, RoutedEventArgs e)
		{
			ConfirmEvent(true);
			Close();
		}
		
		/// <summary>
		/// Метод-callback, отвечающий за кнопку подтверждения
		/// </summary>
		void exitButton_Click(object sender, RoutedEventArgs e)
		{
			ConfirmEvent(false);
			Close();
			return;
		}
		
		/// <summary>
		/// Делегат, отвечающий за ответ от окна подтверждения
		/// </summary>
		/// <param name="confirm">Переменная подтверждения (true - да, false - нет)</param>
		public delegate void ConfirmEventHandler(bool confirm);
		public event ConfirmEventHandler ConfirmEvent;
	}
}