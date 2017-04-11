﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WMPLib;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WindowsMediaPlayer player = new WindowsMediaPlayer();
        public DispatcherTimer myTimer;
        public double time = 0.00;
        public LinkedList<string> CurrentlyPlaying;

        public MainWindow()
        {
            InitializeComponent();

            myTimer = new DispatcherTimer();
            myTimer.Tick += new EventHandler(MyEvent);
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            CurrentlyPlaying = MusiqueView.ChargerMusic();

            player.settings.volume = 50;          
            player.settings.autoStart = false; 
            player.URL = CurrentlyPlaying.First(); 
        }

            private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void Connexion(object sender, RoutedEventArgs e)
        {
            Window1 subWindow = new Window1();
            subWindow.Show();
        }

        private void Reduce(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Inscription(object sender, RoutedEventArgs e)
        {
            Window2 subWindow2 = new Window2();
            subWindow2.Show();
        }

        private void MyEvent(object sender, EventArgs e)
        {
            duration.Content = (Math.Floor(player.controls.currentPosition / 1440)).ToString("00") + ":" + (Math.Floor(player.controls.currentPosition / 60)).ToString("00") + ":" + (player.controls.currentPosition % 60).ToString("00");
            duration2.Content = (Math.Floor(player.currentMedia.duration / 1440)).ToString("00") + ":" + (Math.Floor(player.currentMedia.duration / 60)).ToString("00") + ":" + (player.currentMedia.duration % 60).ToString("00");

            image.Source = new BitmapImage(new Uri(@"C:\Users\adria\Desktop\C#\Appli Graphique\AppliGraphique\resources\e"+ player.currentMedia.name + ".jpg", UriKind.Absolute)); //A refaire avec classe Musique sans bug 
            title.Content = player.currentMedia.getItemInfo("Name");
            artist.Content = player.currentMedia.getItemInfo("Artist");

            Prog.Value = (player.currentMedia.duration != 0) ? (player.controls.currentPosition * 100) / player.currentMedia.duration : (player.controls.currentPosition * 100) / 0.01;
        }

        private void Replay(object sender, RoutedEventArgs e)
        {
            if (player.settings.getMode("Loop"))
            {
                player.settings.setMode("Loop", false);
                replay.Foreground = new SolidColorBrush(Color.FromRgb(255,255,255));
            }
            else
            {
                player.settings.setMode("Loop", true);
                replay.Foreground = new SolidColorBrush(Color.FromRgb(39, 174, 96));
            }
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            player.URL = (IndexOf(player.URL, 0) == 0) ? CurrentlyPlaying.ElementAt(CurrentlyPlaying.Count-1) : CurrentlyPlaying.ElementAt(IndexOf(player.URL, 0)-1);
        }

        private int IndexOf(string url, int n)
        {
            return (CurrentlyPlaying.ElementAt(n) == url) ? n : IndexOf(url, n + 1);          
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            player.URL = (IndexOf(player.URL, 0) == CurrentlyPlaying.Count-1) ? CurrentlyPlaying.ElementAt(0) : CurrentlyPlaying.ElementAt(IndexOf(player.URL, 0) + 1);
        }

        private void Random(object sender, RoutedEventArgs e)
        {
            //A faire lorsque tout sera debugué 
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if ((string)PausePlay.Content == "▶")
            {
                player.settings.autoStart = true;
                player.controls.currentPosition = time;              
                PausePlay.Content = "∥";
                PausePlay.ToolTip = "Pause";
                player.controls.play();
                myTimer.Start();
            } 
            else
            {
                time = player.controls.currentPosition;
                player.controls.pause();
                PausePlay.Content = "▶";
                PausePlay.ToolTip = "Lecture";
                myTimer.Stop();
            }
        }

        private void Change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.settings.volume = Convert.ToInt32(slider.Value);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    internal class MusiqueView //Refaire la classe avec class Musique voire List 
    {
        public static LinkedList<string> li = new LinkedList<string>();

        internal static LinkedList<string> ChargerMusic()
        {        
            try
            {
                using (StreamReader str = new StreamReader(@"C:\Users\adria\Desktop\C#\Appli Graphique\AppliGraphique\resources\File.txt"))
                {
                    while (str.EndOfStream==false)
                    {
                        li.AddFirst(@"C:\Users\adria\Desktop\C#\Appli Graphique\AppliGraphique\resources\"+ str.ReadLine());
                    }
                }               
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return li;
        }
    }
}