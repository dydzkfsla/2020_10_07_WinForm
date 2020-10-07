using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2020_10_07_WinForm
{
    public partial class Form1 : Form
    {
        string editingFileName = null;

        bool dirty = false; //변경내용이 있는지 없는지
        string dirtyText = "*{0} - Windows 메모장";
        string notDirtyText = "{0} - Windows 메모장";

        public Form1()
        {
            InitializeComponent();
        }

        private void 새로만들ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editingFileName = null;
            textBox1.Text = null;
            dirty = false;
            this.Text = "제목없음 - Windows 메모장";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "제목없음 - Windows 메모장";
            this.textBox1.Focus();
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                editingFileName = openFileDialog1.FileName;

                try
                {
                    using (StreamReader sr = new StreamReader(editingFileName, Encoding.Default))
                    {
                        textBox1.Text = sr.ReadToEnd();
                    }
                    dirty = false;
                    UpdateText();
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (! dirty)
            {
                dirty = true;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            string fileNm;
            if (string.IsNullOrEmpty(editingFileName))
                fileNm = "제목없음";
            else
            {
                int idx = editingFileName.LastIndexOf('\\') + 1;
                fileNm = editingFileName.Substring(idx);

            }
            if (dirty)
                this.Text = string.Format(dirtyText, fileNm);
            else
                this.Text = string.Format(notDirtyText, fileNm);
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editingFileName))
            {
                NewSave();
                return;
            }
            using (FileStream file = new FileStream(editingFileName, FileMode.Open))
            {
                byte[] temp = Encoding.Default.GetBytes(textBox1.Text);
                file.Write(temp, 0, temp.Length);
                dirty = false;
                UpdateText();
            }


        }

        private void 다른이름으로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSave();
        }
        private void NewSave()
        {
            saveFileDialog1.AddExtension = true;
            //saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text Files(*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                editingFileName = saveFileDialog1.FileName;
                using (FileStream file = new FileStream(editingFileName, FileMode.Create))
                {
                    byte[] temp = Encoding.Default.GetBytes(textBox1.Text);
                    file.Write(temp, 0, temp.Length);
                    dirty = false;
                    UpdateText();
                }
            }
        }

    }
}
