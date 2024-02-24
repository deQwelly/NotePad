using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace NotePad
{
    public partial class NotePad : Form
    {
        private bool isSaved = false;
        private bool isSavedAs = false;
        private string filePath = "";
        ///private PrintDocument documentToPrint = new PrintDocument();

        /// <summary>
        /// Вызывает YesNo MessageBox с названием и сообщение заданными в параметрах
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <returns></returns>
        private DialogResult MessageBoxYesNo(string text1, string text2)
        {
            DialogResult result = MessageBox.Show(text1, text2,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                return result;
            }
            return result;
        }

        public NotePad()
        {
            InitializeComponent();
        }

        private void NotePadLoad(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Меню -> Файл -> Создать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewFile(object sender, EventArgs e)
        {
            Application.Restart();
        }

        /// <summary>
        /// Меню -> Файл -> Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveFile(object sender, EventArgs e)
        {
            if (isSaved == false)
            {
                if (isSavedAs == false)
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "Text file | *.txt";
                    save.Title = "Save text file";
                    save.ShowDialog();
                    filePath = save.FileName;
                    if (!filePath.Equals(""))
                    {
                        File.WriteAllText(filePath, richTextBox1.Rtf);
                        isSaved = true;
                        isSavedAs = true;
                    }
                }
                else
                {
                    File.WriteAllText(filePath, richTextBox1.Rtf);
                    isSaved = true;
                }
            }
        }

        /// <summary>
        /// Изменение в текст боксе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeTextBox(object sender, EventArgs e)
        {
            isSaved = false;
        }

        /// <summary>
        /// Закрытие приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotePad_ExitClick(object sender, FormClosingEventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBoxYesNo("Сохранить текущий файл?", "Сохранение") == DialogResult.Yes)
                {
                    SaveFile(sender, e);
                }
            }
        }

        /// <summary>
        /// Меню -> Файл -> Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Меню -> Файл -> Открыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuOpenClick(object sender, EventArgs e)
        {
            if (isSaved == false)
            {
                if (MessageBoxYesNo("Сохранить текущий файл переод открытием нового?", "Сохранение") == DialogResult.Yes)
                {
                    SaveFile(sender, e);
                }
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "| *.txt";
            open.ShowDialog();
            if (!open.FileName.Equals(""))
            {
                richTextBox1.Rtf = File.ReadAllText(open.FileName);
                isSaved = true;
                isSavedAs = true;
            }
        }

        /// <summary>
        /// Меню -> Справка -> О программе 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutProgrammClick(object sender, EventArgs e)
        {
            MessageBox.Show("NotePad\nЛеша Антонов 09-322\nКФУ",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Меню -> Правка -> Копировать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyClick(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        /// <summary>
        /// Меню -> Правка -> Вставить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasteClick(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        /// <summary>
        /// Меню -> Правка -> Вырезать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutClick(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        /// <summary>
        /// Меню -> Файл -> Печать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintClick(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;
            printDialog.AllowSomePages = true;
            printDialog.ShowHelp = true;
            printDialog.Document = printDocument;
            
            DialogResult result = printDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        /// <summary>
        /// Обработичк события печати страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Rtf, richTextBox1.Font, Brushes.Black, 10, 10);
        }

        /// <summary>
        /// Меню -> Шрифт -> Цвет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontColorClick(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            { 
                if (!richTextBox1.SelectedText.Equals(""))
                { 
                    richTextBox1.SelectionColor = cd.Color;
                    isSaved = false;
                }
                else
                {
                    richTextBox1.SelectionColor = cd.Color;
                    isSaved = false;
                }
            }
        }

        /// <summary>
        /// Меню -> Шрифт -> Параметры шрифта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontSettingClick(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            DialogResult result = fontDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!richTextBox1.SelectedText.Equals(""))
                {
                    richTextBox1.SelectionFont = fontDialog.Font;
                    isSaved = false;
                }
                else
                {
                    richTextBox1.SelectionFont = fontDialog.Font;
                    isSaved = false;
                }
            }
        }
    }
}
