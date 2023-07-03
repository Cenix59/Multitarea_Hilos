using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Multitarea_Hilos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string ruta = @"C:\Users\PC-1\Desktop\Multitarea_Hilos\Test\";
        string archivo = "Prueba.txt";

        private void StartBtn_Click(object sender, EventArgs e)
        {
            FileStream fs;

            if (Numbertxt.Text == "")
            {
                MessageBox.Show("Complete con un número mayor a 0 para poder continuar");
            }
            else
            {
                try
                {
                    if (File.Exists(ruta))
                    {
                        fs = File.Create(ruta + archivo);
                        fs.Close();
                        MessageBox.Show("archivo creado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Escribir();
                    }
                    else
                    {
                        Directory.CreateDirectory(ruta);
                        fs = File.Create(ruta + archivo);
                        fs.Close();
                        MessageBox.Show("archivo creado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Escribir();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Escribir()
        {
            int linea = Convert.ToInt32(Numbertxt.Text);
            Progress.Maximum = linea;

            Thread escribirThread = new Thread(() =>
            {
                using (StreamWriter escribir = new StreamWriter(ruta + archivo))
                {
                    try
                    {
                        for (int x = 1; x <= linea; x++)
                        {
                            //Thread.Sleep(1000);
                            escribir.WriteLine("Linea: " + x.ToString());
                            Progress.Invoke((MethodInvoker)(() => Progress.Value = x));
                            int resultado = x * 100 / linea;
                            ProgressText.Invoke((MethodInvoker)(() => ProgressText.Text = resultado.ToString() + "%"));
                        }

                        escribir.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            });

            escribirThread.Start();
        }


        private void Numbertxt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Numbertxt.Text))
                StartBtn.Enabled = false;
            else
                StartBtn.Enabled = true;
        }
    }
}
