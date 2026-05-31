using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Media;
using projekt_Jakub_Siwek_Z403_AV.ViewModels;

namespace projekt_Jakub_Siwek_Z403_AV.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InjectCustomControl();
        }

        
        private void InjectCustomControl()
        {
            //  Główny obiekt, kolor tła, zaokrąglone rogi
            var alertBorder = new Border
            {
                Background = Brushes.White,
                BorderThickness = new Avalonia.Thickness(2),
                CornerRadius = new Avalonia.CornerRadius(8),
                Padding = new Avalonia.Thickness(14, 12, 14, 12),
                Margin = new Avalonia.Thickness(0, 10, 0, 0),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
            };

            // binding koloru obramowania
            alertBorder.Bind(Border.BorderBrushProperty, new Binding("StatusColor")
            {
                Converter = new FuncValueConverter<string, IBrush>(hex =>
                    hex != null ? SolidColorBrush.Parse(hex) : Brushes.Gray)
            });

            //  Ikona + Tekst komunikatu obok siebie
            var contentGrid = new Grid();
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            // 1. Dynamiczna okona
            var iconTxt = new TextBlock
            {
                FontSize = 20,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 12, 0)
            };
            iconTxt.Bind(TextBlock.TextProperty, new Binding("StatusIcon"));
            Grid.SetColumn(iconTxt, 0);

            // 2. Dynamiczny tekst komunikatu
            var msgTxt = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeight.SemiBold,
                Foreground = SolidColorBrush.Parse("#2D3748"), 
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
            msgTxt.Bind(TextBlock.TextProperty, new Binding("StatusMessage"));
            Grid.SetColumn(msgTxt, 1);

           
            contentGrid.Children.Add(iconTxt);
            contentGrid.Children.Add(msgTxt);
            alertBorder.Child = contentGrid;

            
            var targetWrapper = this.FindControl<ContentControl>("MyCustomControlWrapper");
            if (targetWrapper != null)
            {
                targetWrapper.Content = alertBorder;
            }
        }

        protected override async void OnOpened(System.EventArgs e)
        {
            base.OnOpened(e);
            if (DataContext is MainWindowViewModel vm)
            {
                await vm.LoadAsync();
            }
        }

        private async void AddButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                await vm.AddStudentAsync();
            }
        }

        private async void DeleteButton_Click(object? sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                await vm.DeleteStudentAsync();
            }
        }
    }
}