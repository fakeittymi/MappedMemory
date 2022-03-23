using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.MemoryMappedFiles;
using System.Diagnostics;

namespace Writer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = "D://3 курс//ОС//Лаба4//Reader//bin//Debug//Reader.exe";
            Process.Start(procInfo);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            char[] msg;
            if (richTextBox1.Text.Length > 0)
            {
                msg = richTextBox1.Text.ToCharArray();
                int size = msg.Length;

                MemoryMappedFile sharedMemory = MemoryMappedFile.CreateOrOpen("MemoryFile", size * 2 + 4);
                using (MemoryMappedViewAccessor writer = sharedMemory.CreateViewAccessor(0, size * 2 + 4))
                {
                    writer.Write(0, size);
                    writer.WriteArray<char>(4, msg, 0, msg.Length);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Process proc = Process.GetProcessesByName("Reader")[0];
                proc.Kill();
            }
            catch{}          
        }
    }
}
