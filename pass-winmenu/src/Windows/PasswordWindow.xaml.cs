﻿using System;
using System.Windows;
using System.Windows.Input;
using PassWinmenu.Configuration;

namespace PassWinmenu.Windows
{
	/// <summary>
	/// Interaction logic for PathWindow.xaml
	/// </summary>
	public partial class PasswordWindow
	{
		private bool allowSymbols = true;
		private bool allowNumbers = true;
		private bool allowLower = true;
		private bool allowUpper = true;
		private bool allowWhitespace = true;

		public PasswordWindow(string filename)
		{
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			InitializeComponent();

			// We'll keep supporting DefaultUsername for now, but eventually it'll have to go.
			var defaultUsername = ConfigManager.Config.PasswordGeneration.DefaultUsername;
			if (defaultUsername == null)
			{
				var now = DateTime.Now;
				var extraContent = ConfigManager.Config.PasswordGeneration.DefaultContent
					.Replace("$filename", filename)
					.Replace("$date", now.ToString("yyyy-MM-dd"))
					.Replace("$time", now.ToString("HH:mm:ss"));

				ExtraContent.Text = extraContent;
			}
			else
			{
				var username = defaultUsername == "$filename" ? filename : defaultUsername;
				ExtraContent.Text = $"Username: {username}\n";
			}


			RegeneratePassword();
			Password.Focus();
		}

		private string GeneratePassword(int length = 20)
		{
			if (!allowSymbols && !allowNumbers && !allowLower && !allowUpper) return null;

			var chars = new char[length];
			var rand = new Random();

			for (var i = 0; i < length;)
			{
				var ch = (char)rand.Next(32, 127);
				if (((char.IsSymbol(ch) || char.IsPunctuation(ch)) && allowSymbols)
				    || (char.IsNumber(ch) && allowNumbers)
				    || (char.IsLower(ch) && allowLower)
				    || (char.IsUpper(ch) && allowUpper)
				    || (char.IsWhiteSpace(ch) && allowWhitespace))
				{
					chars[i++] = ch;
				}
				else if (char.IsControl(ch))
				{
					
				}
				else
				{

				}
			}
			return new string(chars);
		}

		private void RegeneratePassword()
		{
			Password.Text = GeneratePassword();
			Password.CaretIndex = Password.Text.Length;
		}

		private void Btn_Generate_Click(object sender, RoutedEventArgs e)
		{
			Password.Text = GeneratePassword();
		}

		private void Btn_OK_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				DialogResult = false;
				Close();
			}
		}

		private void HandleCheckedChanged(object sender, RoutedEventArgs e)
		{
			if (Cbx_Symbols == null || Cbx_Numbers == null || Cbx_Lower == null || Cbx_Upper == null || Cbx_Whitespace == null) return;

			allowSymbols = Cbx_Symbols.IsChecked.Value;
			allowNumbers = Cbx_Numbers.IsChecked.Value;
			allowLower = Cbx_Lower.IsChecked.Value;
			allowUpper = Cbx_Upper.IsChecked.Value;
			allowWhitespace = Cbx_Whitespace.IsChecked.Value;

			RegeneratePassword();
		}
	}
}
