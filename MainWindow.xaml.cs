using System;
using System.Collections.Generic;
using System.IO;
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

namespace WordSearch
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Словарь русских слов
        string path = @"C:\Users\sozda\source\repos\WordSearch\russianWords.txt";
        // Промежуточный файл с подходящими словами
        string path2 = @"C:\Users\sozda\source\repos\WordSearch\test2.txt"; 
        int numLetters = 0;

        // Вывод слов
        public async void ShowWordsAsync()
        {
            int countSuitableWord = 0;
            FileStream test2 = null;
            try
            {
                test2 = new FileStream(path2, FileMode.OpenOrCreate);

                foreach (string word in File.ReadLines(path))
                {
                    if (word.Length == numLetters)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(word + "\n");
                        await test2.WriteAsync(buffer, 0, buffer.Length);
                        /*
                            Фильтр слов
                        */
                        countSuitableWord++;
                    }
                    else continue;
                }
            }
            catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            finally
                { test2?.Close(); }

            textFromFile.Text = File.ReadAllText(path2);
            countWord.Text = $"Количество подходящих слов: {countSuitableWord}";
        }

        //ячейки для букв
        public void FilterBox()
        {
            for (int i = 0; i < numLetters; i++)
            {
                TextBox letters = new TextBox();
                letters.Name = "lettersBox" + i;
                letters.Text = " ";
                letters.Width = 20;
                letters.Height = 20;
                lettersPanel.Children.Add(letters);
                lettersPanel.RegisterName(letters.Name, letters);
            }
        }

        /* * * Buttons * * */
        private void StartSort(object sender, RoutedEventArgs e)
        {
            ShowWordsAsync();
        }

        private void Clean(object sender, RoutedEventArgs e)
        {
            textFromFile.Clear();
            File.Delete(path2);
        }

        private void EnterNumberLetters(object sender, RoutedEventArgs e)
        {
            numLetters = Convert.ToInt32(numberLetters.Text);
            FilterBox();
            textInStackPanel.Visibility = Visibility;
        }
    }
}
