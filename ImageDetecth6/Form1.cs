using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageDetecth6
{
    public partial class Form1 : Form
    {
        private string outputImagePath = @"D:\cam6\output.jpg";  
        public Form1()
        {
            InitializeComponent();
            this.button6.Click += new System.EventHandler(this.button6_Click); 
            this.button2.Click += new System.EventHandler(this.button2_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        listBox1.Items.Add(fileName);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedImagePath = listBox1.SelectedItem.ToString();
                RunPythonScript(@"D:\cam6\cam1.py", selectedImagePath);
                DisplayOutputImage();
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir resim seçin.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedImagePath = listBox1.SelectedItem.ToString();
                RunPythonScript(@"D:\cam6\cam2.py", selectedImagePath);
                DisplayOutputImage();
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir resim seçin.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedImagePath = listBox1.SelectedItem.ToString();
                RunPythonScript(@"D:\cam6\haarcam2.py", selectedImagePath);
                DisplayOutputImage();
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir resim seçin.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedImagePath = listBox1.SelectedItem.ToString();
                RunPythonScript(@"D:\cam6\all.py", selectedImagePath);
                DisplayOutputImage();
            }
            else
            {
                MessageBox.Show("Lütfen listeden bir resim seçin.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;

            string[] filesToDelete =
            {
        @"D:\cam6\output.jpg", 
    };

            foreach (string filePath in filesToDelete)
            {
                if (File.Exists(filePath))
                {
                    try
                    {
                        
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        File.Delete(filePath);
                        MessageBox.Show($"{Path.GetFileName(filePath)} başarıyla silindi.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Dosya silinirken bir hata oluştu: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show($"{Path.GetFileName(filePath)} bulunamadı.");
                }
            }

            
            listBox1.Items.Clear();
        }

        private void RunPythonScript(string scriptPath, string inputImagePath)
        {
            string pythonExePath = @"C:\Users\Bugra Kagan\AppData\Local\Programs\Python\Python311\python.exe";
            string outputImagePath = @"D:\cam6\output.jpg";

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = $"\"{scriptPath}\" \"{inputImagePath}\" \"{outputImagePath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(start))
                {
                    using (StreamReader outputReader = process.StandardOutput)
                    {
                        string result = outputReader.ReadToEnd();
                        MessageBox.Show(result); 
                    }
                    using (StreamReader errorReader = process.StandardError)
                    {
                        string error = errorReader.ReadToEnd();
                        if (!string.IsNullOrEmpty(error))
                        {
                            MessageBox.Show($"Error: {error}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void DisplayOutputImage()
        {
            if (File.Exists(outputImagePath))
            {
                pictureBox1.Image = new Bitmap(outputImagePath);
            }
            else
            {
                MessageBox.Show("Görüntü dosyası bulunamadı.");
            }
        }
    }
}
