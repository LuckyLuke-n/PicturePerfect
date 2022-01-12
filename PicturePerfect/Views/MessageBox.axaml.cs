using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PicturePerfect.Models;
using System.Threading.Tasks;

namespace PicturePerfect.Views
{
    public partial class MessageBox : Window
    {
        /// <summary>
        /// Creates and instance of the MessageBox object.
        /// Source: https://stackoverflow.com/questions/55706291/how-to-show-a-message-box-in-avaloniaui-beta#55707749
        /// Use like this var res = await MessageBox.Show(text: "Test message box", buttons: MessageBox.MessageBoxButtons.YesNoCancel, icon: MessageBox.MessageBoxIcon.Information);
        /// </summary>
        public MessageBox()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        public enum MessageBoxIcon
        {
            Error,
            Information,
            Question,
            Warning,
            None
        }

        /// <summary>
        /// Method to show the message box.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns>Returns the Task given by the user.</returns>
        public static Task<MessageBoxResult> Show(string text, string? title = null, MessageBoxButtons buttons = MessageBoxButtons.Ok, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBox messageBox = new();

            // check for title
            if (title != null) { messageBox.Title = title; }
            else { messageBox.Title = ThisApplication.ApplicationName; }

            messageBox.FindControl<TextBlock>("textBlockMessage").Text = text;
            var buttonPanel = messageBox.FindControl<StackPanel>("stackPanelButtons");
            var imageIcon = messageBox.FindControl<Image>("imageIcon");

            var res = MessageBoxResult.Ok;

            // add button to the stack panel
            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption };
                btn.Click += (_, __) => {
                    res = r;
                    messageBox.Close();
                };
                buttonPanel.Children.Add(btn);
                if (def)
                    res = r;
            }

            // check for buttons
            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
                AddButton("OK", MessageBoxResult.Ok, true);

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
                AddButton("Cancel", MessageBoxResult.Cancel, true);

            // check for icon
            switch (icon)
            {
                case MessageBoxIcon.Error:
                    imageIcon.Source = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/MessageBoxIcons/error.png");
                    imageIcon.Width = 40;
                    break;
                case MessageBoxIcon.Information:
                    imageIcon.Source = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/MessageBoxIcons/info.png");
                    imageIcon.Width = 40;
                    break;
                case MessageBoxIcon.Question:
                    imageIcon.Source = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/MessageBoxIcons/question.png");
                    imageIcon.Width = 40;
                    break;
                case MessageBoxIcon.Warning:
                    imageIcon.Source = BitmapValueConverter.Convert("avares://PicturePerfect/Assets/MessageBoxIcons/warning.png");
                    imageIcon.Width = 40;
                    break;
                default:
                    // no icon
                    break;
            }


            // get the info about the button click
            var taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();
            messageBox.Closed += delegate { taskCompletionSource.TrySetResult(res); };
            messageBox.Show();

            return taskCompletionSource.Task;
        }
    }
}
