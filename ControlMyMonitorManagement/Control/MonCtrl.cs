using System.Windows.Controls;
using System.Windows;
using CMM.Library.ViewModel;
using CMM.Library.Base;
using CMM.Library.Method;
using System.Windows.Data;
using System;

namespace CMM.Management.Control
{
    /// <summary>
    /// 單一顆螢幕
    /// </summary>
    internal class MonCtrl : System.Windows.Controls.Control
    {
        public readonly static DependencyProperty MonProperty;
        private StackPanel _sp;

        static MonCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonCtrl), new FrameworkPropertyMetadata(typeof(MonCtrl)));

            MonProperty = DependencyProperty.Register(
                "Monitor",
                typeof(XMonitor),
                typeof(MonCtrl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMonChangedCallback));
        }

        public override void OnApplyTemplate()
        {
            _sp = Template.FindName("sp", this) as StackPanel;
        }

        public XMonitor Mon
        {
            get => (XMonitor)GetValue(MonProperty);
            set => SetValue(MonProperty, value);
        }

        static void OnMonChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var me = sender as MonCtrl;
            if (me != null)
            {
                me.OnMonChanged((XMonitor)args.NewValue);
            }
        }

        public virtual void OnMonChanged(XMonitor value)
        {
            
        }
    }
}
