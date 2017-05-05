﻿using Biblio;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace MyApp
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public event Action<User> Check;

        public Window2()
        {            
            InitializeComponent();
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Reduce(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Increase(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                increase.Content = "⇱";
            }
            else
            {
                WindowState = WindowState.Maximized;
                increase.Content = "⇲";
            }
        }

        private void Commit(object sender, RoutedEventArgs e)
        {                   
            if (IsValid(email.Text) && (pseudo.Text).Length>3 && (mdp.Password).Length > 3)
            {
                Check?.Invoke(new User(new MailAddress(email.Text, pseudo.Text), mdp.Password, null));
                Close();
            }
            else
            {
                SolidColorBrush red = new SolidColorBrush(Color.FromRgb(217, 30, 24));
                if (!IsValid(email.Text))
                {
                    labelemail.Content = "Email invalide";
                    labelemail.Foreground = red;
                }
                    
                if ((pseudo.Text).Length < 3)
                {
                    labelpseudo.Content = "4 caractères mini";
                    labelpseudo.Foreground = red;
                }
                    
                if ((mdp.Password).Length < 3)
                {
                    labelmdp.Content = "4 caractères mini";
                    labelmdp.Foreground = red;
                }
            }
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
