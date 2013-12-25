Import("System.Windows.*");
Import("System.Windows.Controls.*");

xaml = Application.Current.LoadRootVisual(new UserControl(), 'app.xaml')

xaml.TextBlock1.MouseLeftButtonUp += function (sender, args) {
    sender.FontSize *= 2
}
