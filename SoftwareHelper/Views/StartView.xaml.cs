﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SoftwareHelper.Helpers;

namespace SoftwareHelper.Views
{
    /// <summary>
    ///     StartView.xaml 的交互逻辑
    /// </summary>
    public partial class StartView : Window
    {
        private readonly ImageBrush backgroundBrush = new ImageBrush();
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public StartView()
        {
            InitializeComponent();
            myCanvas.Focus();

            timer.Tick += Engine;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            backgroundBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/background.jpg"));
            background.Fill = backgroundBrush;
            if (background.Fill.CanFreeze)
                background.Fill.Freeze();
            background2.Fill = backgroundBrush;
            if (background2.Fill.CanFreeze)
                background2.Fill.Freeze();
            Loaded += StartView_Loaded;
        }

        private void StartView_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
            var bw = new BackgroundWorker();
            bw.DoWork += delegate
            {
                Common.Init();
                Thread.Sleep(1000);
            };
            bw.RunWorkerCompleted += delegate
            {
                tbMsg.Text = "开始体验";
                timer.Stop();
                var embedDeasktopView = new EmbedDeasktopView();
                embedDeasktopView.Show();
                var closeAnimation = new DoubleAnimation
                {
                    From = Width,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new BackEase { EasingMode = EasingMode.EaseIn }
                };
                closeAnimation.Completed += delegate
                {
                    Close();
                };
                BeginAnimation(WidthProperty, closeAnimation);
            };
            tbMsg.Text = "即将进入";
            bw.RunWorkerAsync();
        }

        private void Engine(object sender, EventArgs e)
        {
            var backgroundLeft = Canvas.GetLeft(background) - 3;
            var background2Left = Canvas.GetLeft(background2) - 3;
            Canvas.SetLeft(background, backgroundLeft);
            Canvas.SetLeft(background2, background2Left);
            Canvas.SetLeft(tb1, Canvas.GetLeft(tb1) - 3);
            Canvas.SetLeft(tb2, Canvas.GetLeft(tb2) - 3);
            Canvas.SetLeft(tb3, Canvas.GetLeft(tb3) - 3);
            if (backgroundLeft <= -1262)
            {
                timer.Stop();
                Start();
            }
        }

        private void Start()
        {
            Canvas.SetLeft(background, 0);
            Canvas.SetLeft(background2, 1262);

            timer.Start();
        }
    }
}