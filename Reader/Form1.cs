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

namespace Reader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }      

        private void timer1_Tick(object sender, EventArgs e)
        {
            string receivedMsg = "";
            try
            {
                MemoryMappedFile sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");           
                int size;
                char[] msg;
                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
                {
                    size = reader.ReadInt32(0);
                }

                using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
                {
                    msg = new char[size];
                    reader.ReadArray<char>(0, msg, 0, size);
                }

                label1.Text = "";
                foreach (char ch in msg)
                {
                    label1.Text += ch;
                    receivedMsg += ch;
                }

                listBox1.Items.Clear();
                var counted = receivedMsg
                        .GroupBy(c => c)
                        .Select(g => new { g.Key, Count = g.Count() })
                        .OrderByDescending(o => o.Count);
                foreach (var res in counted)
                {
                    listBox1.Items.Add($"{ res.Key}, {res.Count}, {100.0 / receivedMsg.Length * res.Count}");
                }
            }
            catch {}
        }
    }
}
