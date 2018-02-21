/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 27.10.2017
 * Time: 18:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
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
	/// Класс, описывающий логику окна авторизации
	/// </summary>
	public partial class LogWindow : Window
	{
		private ButtonFlags ButtonFlag = ButtonFlags.LogIn; //Текущее состояние кнопки авторизации
		
		public LogWindow()
		{
			InitializeComponent();
			
			if(File.Exists("mydata.bin"))	// Вытаскиваем данные "rememberMe"
			{
				using (BinaryReader reader = new BinaryReader(File.Open("mydata.bin", FileMode.Open)))
	            {
					bool isRemember = reader.ReadBoolean();
					if(isRemember)
					{
						rememberMeBox.IsChecked = true;
						LogBox.Text = reader.ReadString(); //login
						PassBox.Text = reader.ReadString(); //pass
						reader.Close();
						if(TryToLogIn() == 2) //Попытка авторизации 
						{
							User r = MTSystem.LoadClient(LogBox.Text, PermType.All);
							var main = new Window1(r);
							main.Show();
							this.Close();
						}
						
					}
					else 
						reader.Close();
	            }
			}
			else
				WriteToRemeberDataFile(false, "NULL", "NULL");
		}		
		
		/// <summary>
		/// Callback-Метод для входа игрока в систему
		/// </summary>
		void LogButton_Click(object sender, RoutedEventArgs e)
		{
			if(ButtonFlag == ButtonFlags.LogIn)
			{
				if(TryToLogIn() == 2) //Если в режиме авторизации, пробуем залогиниться
				{
						if(rememberMeBox.IsChecked == true)
							WriteToRemeberDataFile(true, LogBox.Text, PassBox.Text);
						else
							WriteToRemeberDataFile(false, "NULL", "NULL");
					
					User r = MTSystem.LoadClient(LogBox.Text, PermType.All);
					var main = new Window1(r);
					main.Show();
					Close();
				}				
			}
			else //Если в режиме опций, то сохраняем данные
			{
				ButtonFlag = ButtonFlags.LogIn;
				spRememberMe.Visibility = Visibility.Visible;
				String[] str = new string[2];
				if(LogBox.Text == "") str[0] = "NULL";
				else str[0] = LogBox.Text;
				
				if(PassBox.Text == "") str[1] = "NULL";
				else str[1] = PassBox.Text;
				
				MTSystem.SaveConnectionData(str);
				
				LogBox.Text = "";
				PassBox.Text = "";
				loginLabel.Text = "Логин";
				passLabel.Text = "Пароль";
				LogButton.Content = "Войти";
				OptionsButton.Content = "Опции";
			}
		}
		
		/// <summary>
		///  Метод-callback для кнопки "опции"
		/// </summary>
		void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			if(ButtonFlag == ButtonFlags.LogIn)
			{
				ButtonFlag = ButtonFlags.Options;
				String[] str = MTSystem.LoadConnectionData();
				spRememberMe.Visibility = Visibility.Hidden;
				LogBox.Text = str[0];
				PassBox.Text = str[1];
				loginLabel.Text = "Название БД";
				passLabel.Text = "IP-адрес";
				LogButton.Content = "Принять";
				OptionsButton.Content = "Закрыть";
			}
			else
			{
				spRememberMe.Visibility = Visibility.Visible;
				ButtonFlag = ButtonFlags.LogIn;
				LogBox.Text = "";
				PassBox.Text = "";
				loginLabel.Text = "Логин";
				passLabel.Text = "Пароль";
				LogButton.Content = "Войти";
				OptionsButton.Content = "Опции";
			}
		}
		
		/// <summary>
		/// Метод, позволяющий сделать безопасный коннект с некоторыми возможными исходами (неверный логин, пароль...)
		/// </summary>
		/// <returns>Код ошибки:
		/// 0 - неверный логин
		/// 1 - неверный пароль
		/// 2 - успешно</returns>
		int TryToLogIn()
		{
			if(MTSystem.ConnectBase())
			{
				int res = MTSystem.LogIn(LogBox.Text, PassBox.Text);
				Color col = new Color();
				col.A = 255;
				SolidColorBrush br = new SolidColorBrush();
				switch(res)
				{
					case 0:
						StatusBlock.Text = "Неверный логин!";
						col.R = 177;
						col.G = 66;
						col.B = 66;
						br.Color = col;
						StatusBlock.Foreground = br;
						break;
						
					case 1:
						StatusBlock.Text = "Неверный пароль!";
						col.R = 177;
						col.G = 66;
						col.B = 66;
						br.Color = col;
						StatusBlock.Foreground = br;
						break;
						
					case 2:
						StatusBlock.Text = "Вход";
						col.R = 74;
						col.G = 176;
						col.B = 40;
						br.Color = col;
						StatusBlock.Foreground = br;
						break;
				}
				return res;
			}
			return 0;
		}
	
		
		/// <summary>
		/// Запись в файл для rememberMe
		/// </summary>
		/// <param name="remember">значение галки "rememberMe"</param>
		/// <param name="login">Логин</param>
		/// <param name="pass">Пароль</param>
		public static void WriteToRemeberDataFile(bool remember, String login, String pass)
		{
			using (BinaryWriter writer = new BinaryWriter(File.Open("mydata.bin", FileMode.OpenOrCreate)))
	        {
				writer.Write(remember);
	            writer.Write(login); //login
	            writer.Write(pass); //pass
				writer.Close();
	        }
			
		}
		
	}
}