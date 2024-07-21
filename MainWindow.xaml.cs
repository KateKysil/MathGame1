using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameIStole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string rnum = "";
        int loop = 0;
        Rectangle rectG = new Rectangle
        {
            Width = 15,
            Height = 15,
            Fill = new SolidColorBrush(Colors.Green),
            Margin = new Thickness(5)
        };
        TextBlock textG = new TextBlock { Text = "0" };
        Rectangle rectY = new Rectangle
        {
            Width = 15,
            Height = 15,
            Fill = new SolidColorBrush(Colors.Yellow),
            Margin = new Thickness(5)
        };
        TextBlock textY = new TextBlock { Text = "0" };
        TextBlock textAtempt = new TextBlock { Text = "Atempts: 0" };
        StackPanel stackPanel = new StackPanel();
        StackPanel stackHelp = new StackPanel();
        StackPanel stackGreen = new StackPanel { Orientation = Orientation.Horizontal };
        StackPanel stackYellow = new StackPanel { Orientation = Orientation.Horizontal };
        StackPanel stackText = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
        StackPanel stackNumbers = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
        Button btncheck = new Button { Width = 200, IsEnabled = false };
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            firstStack.Visibility = Visibility.Collapsed;
            int number = 0;
            if (sender is Button button && int.TryParse(button.Content.ToString(), out number))
            {
                List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                int ind = 0;
                Random random = new Random();
                for (int i = 0; i < number; i++)
                {
                    ind = random.Next(1, 9 - i);
                    rnum += list[ind].ToString();
                    list.RemoveAt(ind);
                }
                
                stackPanel = new StackPanel();
                stackHelp = new StackPanel();
                stackGreen = new StackPanel { Orientation = Orientation.Horizontal};
                stackYellow = new StackPanel { Orientation = Orientation.Horizontal};
                
                stackGreen.Children.Add(rectG);
                stackGreen.Children.Add(textG);
                
                stackYellow.Children.Add(rectY);
                stackYellow.Children.Add(textY);
                stackHelp.Children.Add(stackGreen);
                stackHelp.Children.Add(stackYellow);
                stackHelp.Children.Add(textAtempt);
                stackPanel.Children.Add(stackHelp);
                stackText = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                for (int i = 0; i < number; i++)
                {
                    Border border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(2),
                        Margin = new Thickness(5),
                        MinWidth = 20,
                        MinHeight = 20,
                        Background = new SolidColorBrush(Colors.LightGray),
                        Name = "Usual"
                    };
                    TextBlock txtBl = new TextBlock
                    {
                        Margin = new Thickness(5),
                        MinWidth = 15,
                        MinHeight = 20,
                        FontSize = 15,
                        Name = $"txtBl{i + 1}"
                    };
                    txtBl.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;
                    border.Child = txtBl;
                    stackText.Children.Add(border);
                }
                stackPanel.Children.Add(stackText);
                stackNumbers = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment =HorizontalAlignment.Center};
                for (int i = 1; i < 10; i++)
                {
                    stackNumbers.Children.Add(new Button { Content = i.ToString(), Margin = new Thickness(5), IsEnabled = false });
                    ((Button)stackNumbers.Children[i - 1]).Click += BtnNum_Click;
                }
                stackPanel.Children.Add(stackNumbers);
                
                btncheck = new Button { Width = 200, IsEnabled = false };
                btncheck.Content = "Check";
                btncheck.Click += BtnCheck_Click;
                stackPanel.Children.Add(btncheck);
                DynamicContentControl.Content = stackPanel;
            }
            
        }
        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(button);
                if(parent is StackPanel stackPanel)
                {
                    StackPanel stackText = (StackPanel)stackPanel.Children[1];
                    string guess = "";
                    for(int i = 0;i < stackText.Children.Count; i++)
                    {
                        guess += ((TextBlock)((Border)stackText.Children[i]).Child).Text;
                    }
                    if(guess == rnum)
                    {
                        MessageBox.Show($"Congratulations! You guessed number right. You needed {loop} attempts for it");
                        firstStack.Visibility = Visibility.Visible;
                        rnum = "";
                        loop = 0;
                        textG.Text = "0";
                        textY.Text = "0";
                        textAtempt.Text = "Atempts: 0";
                        stackGreen.Children.Clear();
                        stackYellow.Children.Clear();
                        stackText.Children.Clear(); 
                        stackHelp.Children.Clear();
                        stackNumbers.Children.Clear();  
                        stackPanel.Children.Clear();
                    }
                    else
                    {
                        if (AreAllNumbersUnique(guess))
                        {
                            loop++;
                            int green = countGreen(guess.Length, guess);
                            int yellow = countYellow(guess.Length, guess);
                            textG.Text = green.ToString();
                            textY.Text = yellow.ToString();
                            textAtempt.Text = "Atempts: " + loop.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"Not all numbers are unique. Please, try again");
                        }
                    }
                }
            }
        }
        private void BtnNum_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            if(sender is Button button && int.TryParse(button.Content.ToString(), out int num))
            {
                if(((StackPanel)button.Parent).Parent is StackPanel stackPanel)
                {
                    for(int i = 0; i < ((StackPanel)stackPanel.Children[1]).Children.Count; i++)
                    {
                        if (((Border)((StackPanel)stackPanel.Children[1]).Children[i]).Name == "Show")
                        {
                            ((TextBlock)((Border)((StackPanel)stackPanel.Children[1]).Children[i]).Child).Text = num.ToString();
                        }
                        if(((TextBlock)((Border)((StackPanel)stackPanel.Children[1]).Children[i]).Child).Text != "")
                        {
                            count++;
                        }
                    }
                    if(btncheck.IsEnabled == false && count == ((StackPanel)stackPanel.Children[1]).Children.Count)
                    {
                        btncheck.IsEnabled = true;
                    }


                }
            }
        }
        static bool AreAllNumbersUnique(string input)
        {
            HashSet<char> uniqueNumbers = new HashSet<char>();
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    if (!uniqueNumbers.Add(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is TextBlock textBlock)
            {
                if(textBlock.Parent is Border border)
                {
                    if(border.Parent is StackPanel stackPanel)
                    {
                        for(int i = 0; i < stackPanel.Children.Count; i++)
                        {
                            if (((Border)stackPanel.Children[i]).Name == "Show")
                            {
                                ((Border)stackPanel.Children[i]).Name = "Usual";
                                ((Border)stackPanel.Children[i]).BorderBrush = new SolidColorBrush(Colors.Black);
                                ((Border)stackPanel.Children[i]).BorderThickness = new Thickness(2);
                            }
                        }
                        if(stackPanel.Parent is StackPanel panel)
                        {
                            for (int i = 0; i < ((StackPanel)panel.Children[2]).Children.Count; i++) {
                                ((Button)((StackPanel)panel.Children[2]).Children[i]).IsEnabled = true;
                            }
                        }
                    }
                    border.Name = "Show";
                    border.BorderBrush = new SolidColorBrush(Colors.Blue);
                    border.BorderThickness = new Thickness(4);

                }
            }
        }
        private int countGreen(int num, string guess)
        {
            int green = 0;
            if(guess != rnum)
            {
                for(int i = 0;i < num;i++)
                {
                    if (guess[i] == rnum[i])
                    {
                        green++;
                    }
                }
            }
            return green;
        }
        //add -green
        private int countYellow(int num, string guess)
        {
            int yellow = 0;
            if (guess != rnum)
            {
                for (int i = 0; i < num; i++)
                {
                    for(int j = 0; j < num; j++)
                    {
                        if (guess[j] == rnum[i] && rnum[i] != rnum[j])
                        {
                            yellow++;
                        }
                    }
                }
            }
            return yellow;
        }
    }
}
