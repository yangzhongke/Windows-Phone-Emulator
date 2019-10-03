using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Internals.ctrls;

namespace Microsoft.Phone.Shell.Internals
{
    public partial class PhoneAppBarCtrl : UserControl
    {
        internal static readonly double ShowMenuItems_Top = 285;//菜单部分显示时候AppBar的Top值
        internal static readonly double HideMenuItems_Top = 350;//菜单部分隐藏时候AppBar的Top值
        internal static readonly double ShowMenuItems_Height = 110;//菜单部分显示时候AppBar的Height值
        internal static readonly double HideMenuItems_Height = 50;//菜单部分隐藏时候AppBar的Height值

        private bool isShowMenuItems = false;//当前菜单部分是否在现实

        private Storyboard sbHideMenuItems;//隐藏菜单部分的动画
        private Storyboard sbShowMenuItems;//显示菜单部分的动画

        public PhoneAppBarCtrl()
        {
            InitializeComponent();
            sbHideMenuItems = new Storyboard();

            //菜单部分下降的动画
            DoubleAnimation animationMoveDown = new DoubleAnimation();
            animationMoveDown.From = ShowMenuItems_Top;
            animationMoveDown.To = HideMenuItems_Top;
            animationMoveDown.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(animationMoveDown, this);
            Storyboard.SetTargetProperty(animationMoveDown, new PropertyPath("(Canvas.Top)"));
            sbHideMenuItems.Children.Add(animationMoveDown);

            //工具栏变矮的动画
            DoubleAnimation animationShorten = new DoubleAnimation();
            animationShorten.From = ShowMenuItems_Height;
            animationShorten.To = HideMenuItems_Height;
            animationShorten.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTarget(animationShorten, this);
            //PropertyPath中的括号不能少
            Storyboard.SetTargetProperty(animationShorten, new PropertyPath("(PhoneAppBarCtrl.Height)"));
            sbHideMenuItems.Children.Add(animationShorten);

            //显示菜单的动画
            sbShowMenuItems = new Storyboard();
            //菜单部分上升的动画
            DoubleAnimation animationMoveUp = CreateReverse(animationMoveDown);
            Storyboard.SetTarget(animationMoveUp, this);
            DoubleAnimation animationHigher = CreateReverse(animationShorten);
            Storyboard.SetTarget(animationHigher, this);
            sbShowMenuItems.Children.Add(animationMoveUp);
            sbShowMenuItems.Children.Add(animationHigher);
        }

        /// <summary>
        /// 复制一个反向的DoubleAnimation动画，From和To换位置
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        private static DoubleAnimation CreateReverse(DoubleAnimation animation)
        {
            DoubleAnimation animationReverse = new DoubleAnimation();
            animationReverse.AutoReverse = animation.AutoReverse;
            animationReverse.BeginTime = animation.BeginTime;
            animationReverse.By = animation.By;
            animationReverse.Duration = animation.Duration;
            animationReverse.EasingFunction = animation.EasingFunction;
            animationReverse.FillBehavior = animation.FillBehavior;
            animationReverse.From = animation.To;
            animationReverse.To = animation.From;
            animationReverse.RepeatBehavior = animation.RepeatBehavior;
            animationReverse.SpeedRatio = animation.SpeedRatio;            ;
            Storyboard.SetTargetProperty(animationReverse, Storyboard.GetTargetProperty(animation));
            return animationReverse;
        }

        private void iconBtnOnClick(object sender, RoutedEventArgs e)
        {
            HideMenuItems();  
            FrameworkElement btn = sender as FrameworkElement;
            ApplicationBarIconButton appIconBtn = btn.DataContext as ApplicationBarIconButton;
            appIconBtn.FireClickEvent();
        }

        private void menuitemOnClick(object sender, MouseButtonEventArgs e)
        {
            HideMenuItems();  
            FrameworkElement txt = sender as FrameworkElement;//触发关联的ApplicationBarMenuItem对象的事件
            ApplicationBarMenuItem appMenuItem = txt.DataContext as ApplicationBarMenuItem;
            appMenuItem.FireClickEvent();
        }

        private void btnToggle_Click(object sender, RoutedEventArgs e)
        {
            ApplicationBar appBar = this.DataContext as ApplicationBar;
            if (appBar.IsMenuEnabled == false)
            {
                return;
            }
            //点击按钮切换菜单部分显示
            if (this.isShowMenuItems)
            {
                sbHideMenuItems.Begin();
                this.isShowMenuItems = false;
            }
            else
            {
                sbShowMenuItems.Begin();
                this.isShowMenuItems = true;
            }        
        }

        private void HideMenuItems()
        {
            if (this.isShowMenuItems)
            {
                sbHideMenuItems.Begin();
                this.isShowMenuItems = false;
            }
        }
    }
}
