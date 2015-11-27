using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace RegexBuilder.Modules.Library.Helpers
{
    [ContentProperty("Content")]
    public class ErrorTriggerAction : TriggerAction<Grid>, INotifyPropertyChanged
    {
        private string message;
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public InteractionRequestedEventArgs InteractionEventArgs { get; set; }

        public ICommand YesCommand { get; private set; }
        public ICommand NoCommand { get; private set; }

        public ErrorTriggerAction()
        {
            YesCommand = new DelegateCommand(YesCommandHandler);
            NoCommand = new DelegateCommand(NoCommandHandler);
        }

        private void NoCommandHandler()
        {
            AssociatedObject.Visibility = Visibility.Collapsed;
            if (InteractionEventArgs.Context is IConfirmation)
            {
                (InteractionEventArgs.Context as IConfirmation).Confirmed = false;
            }
            InteractionEventArgs.Callback();
        }

        private void YesCommandHandler()
        {
            AssociatedObject.Visibility = Visibility.Collapsed;
            if (InteractionEventArgs.Context is IConfirmation)
            {
                (InteractionEventArgs.Context as IConfirmation).Confirmed = true;
            }
            InteractionEventArgs.Callback();
        }

        protected override void Invoke(object parameter)
        {
            var arg = parameter as InteractionRequestedEventArgs;
            InteractionEventArgs = arg;
            Title = InteractionEventArgs.Context.Title;
            Message = InteractionEventArgs.Context.Content.ToString();
            AssociatedObject.Visibility = Visibility.Visible;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.Visibility = Visibility.Collapsed;
                AssociatedObject.Children.Add(Content);
                Content.DataContext = this;
            }
        }

        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(ErrorTriggerAction), new PropertyMetadata(null, OnContentPropertyChanged));

        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var errorTriggerAction = d as ErrorTriggerAction;
            var associatedObject = errorTriggerAction.AssociatedObject;
            if (associatedObject != null)
            {
                associatedObject.Visibility = Visibility.Collapsed;
                var frameworkElement = e.NewValue as FrameworkElement;
                associatedObject.Children.Add(frameworkElement);
                frameworkElement.DataContext = errorTriggerAction;
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
