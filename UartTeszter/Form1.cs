using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace UartTeszter
{
    public partial class Form1 : Form
    {

        private static System.Timers.Timer aTimer;

        public Form1()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            comboBox1.SelectedIndex = 0;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int readed_something = 0;
                int frame_numbers = 0;
                if (serialPort1.IsOpen)
                {


                    Stopwatch sw = new Stopwatch();
                    sw.Start();



                    String value = serialPort1.ReadExisting();
                    int readed_values = 0;
                    Console.WriteLine("String length :" + value.Length);

                    while (readed_something == 0)
                    {
                        value = serialPort1.ReadExisting();
                        readed_values += value.Length;

                        frame_numbers = readed_values / 5769;
                        while (!value.Equals(""))
                        {
                            value += serialPort1.ReadExisting();
                            readed_values += value.Length;
                            frame_numbers = readed_values / 5769;
                            readed_something = 1;
                            if (sw.ElapsedMilliseconds > 5000)
                            {
                                break;
                            }
                            //richTextBox1.AppendText(value);
                        }
                    }

                    //richTextBox1.Text = value;


                    string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
                    {
                            outputFile.WriteLine(value);
                    }

                    Console.WriteLine("Frame numbers : " + frame_numbers);
                    Console.WriteLine("Data : " + readed_values);
                }

            } catch (Exception ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                Console.WriteLine("Closed");
                button1.Enabled = true;
                button2.Enabled = false;
            } else
            {
                Console.WriteLine("Not Opened");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
