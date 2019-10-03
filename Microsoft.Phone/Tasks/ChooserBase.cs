using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Phone.Internals;
using Microsoft.Phone.Controls;
using System.Collections.Generic;

namespace Microsoft.Phone.Tasks
{
    public abstract class ChooserBase<TTaskEventArgs> where TTaskEventArgs : TaskEventArgs
    {
        //监听页面的Type为Key
        private static Dictionary<Type, ChooserInfo<TTaskEventArgs>> register 
            = new Dictionary<Type, ChooserInfo<TTaskEventArgs>>();

        /// <summary>
        /// 注册Chooser监听
        /// </summary>
        /// <param name="chooserType">选择器的类型</param>
        /// <param name="eventHandler">监听的类中监听方法的委托</param>
        internal static void Register(Type chooserType, EventHandler<TTaskEventArgs> eventHandler)
        {
            ChooserInfo<TTaskEventArgs> info = new ChooserInfo<TTaskEventArgs>() { ChooserType = chooserType, EventHandler = eventHandler };
            register[eventHandler.Target.GetType()] = info;
        }

        internal static void UnRegister(Type eventListnerType)
        {
            register.Remove(eventListnerType);
        }

        internal static ChooserInfo<TTaskEventArgs> GetByListenerType(Type eventListnerType)
        {
            if (!register.ContainsKey(eventListnerType))
            {
                return null;
            }
            return register[eventListnerType];
        }

        public ChooserBase()
        {
            PhoneApplicationPage page = WinPhoneCtrl.Instance.frameScreen.Content as PhoneApplicationPage;
            page.NavigationService.Navigated += new System.Windows.Navigation.NavigatedEventHandler(NavigationService_Navigated);
        }

        public virtual void Show()
        {
            //启用缓存，这样返回以后就不会创建新页面了
            //虽然创建的时候NavigationCacheMode默认值就是Required，但是由于可能来电等模拟TombStone的机制会修改NavigationCacheMode，所以需要改回来
            AppHelper.GetCurrentPhoneAppPage().NavigationCacheMode = System.Windows.Navigation.NavigationCacheMode.Required;
        }

        void NavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //是否有和被导向目标页面Type一直的ChooseInfo
            var chooseInfo = GetByListenerType(e.Content.GetType());
            if (chooseInfo==null)
            {
                return;
                
            }
            //如果有，并且选择器的类型和当前的类型一致
            if (chooseInfo.ChooserType == GetType())
            {
                FireChooseComplete(e);
                PhoneApplicationPage page = AppHelper.GetCurrentPhoneAppPage();//已经转回的页
                page.NavigationService.Navigated -= NavigationService_Navigated;//反注册监听
            }            
        }

        //触发监听事件
        internal void FireEventHandler(System.Windows.Navigation.NavigationEventArgs navigateEventArgs, TaskEventArgs taskEventArgs)
        {
            var chooseInfo = GetByListenerType(navigateEventArgs.Content.GetType());
            chooseInfo.EventHandler(this, (TTaskEventArgs)taskEventArgs);
        }

        internal  virtual void FireChooseComplete(System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private EventHandler<TTaskEventArgs> _completed;        

        public event EventHandler<TTaskEventArgs> Completed
        {
            add
            {
                _completed += value;
                Register(GetType(), value);
            }
            remove
            {
                _completed -= value;
                UnRegister(value.Target.GetType());
            }
        }
    }
}
