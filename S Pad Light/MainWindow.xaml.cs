﻿using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace S_Pad_Light
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 12, 14, 16, 18, 20, 22 ,24, 26 , 28 , 36 , 40 ,50 };
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            BoldButton.IsSelected = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            ItalicButton.IsSelected = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsSelected = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        public void btn_open_Click(object sender, RoutedEventArgs e)  //read file     // открытие файла на чтение
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текст (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                StreamReader reader = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1251));

                rtbEditor.Document.Blocks.Clear();                                                // очищаем richtxt перед открытием
                rtbEditor.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));        // читаем и заполняем richtxt

                reader.Close();   // close file after reading  //  закрываем файл после чтения
                return;
            }
        }

        public void btn_save_Click(object sender, RoutedEventArgs e)  //save file   // сохранение файла
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Текст (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                {
                    string richText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
                    sw.Write(richText);
                    sw.Close();
                }
            }
        }
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e) //font   // шрифт
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)   // font size   // размер шрифта 
        {
            try{
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            } catch {}
        }
        public void UnderlineText(object sender, RoutedEventArgs e)    // underline text  // подчёркивание текста
        {
            if (rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty) != TextDecorations.Underline)
                rtbEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                rtbEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        public void UnUnderlineText(object sender, RoutedEventArgs e)    // underline text  // подчёркивание текста
        {
            if (rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty) != TextDecorations.Underline)
                rtbEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                rtbEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        }

        public void BoldText(object sender, RoutedEventArgs e)  // bold text  // выделение текста  
        {
            rtbEditor.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Bold);
        }

        public void UnBoldText(object sender, RoutedEventArgs e)  // unbold text  // отмена выделения текста 
        {
            rtbEditor.Selection.ApplyPropertyValue(FontWeightProperty, FontWeights.Normal);
        }

        public void ItalicText(object sender, RoutedEventArgs e)   // курсив
        {
            rtbEditor.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Italic);
        }

        public void UnItalicText(object sender, RoutedEventArgs e)   // без курсива 
        {
            rtbEditor.Selection.ApplyPropertyValue(FontStyleProperty, FontStyles.Normal);
        }
    }
}