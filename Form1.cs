using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_Editor {
    public partial class Form1 : Form {
        private string saveLocation;
        private bool unsavedChanges;
        public Form1() {
            InitializeComponent();
            this.saveLocation = "";
            this.unsavedChanges = false;
        }

        private void save(object sender, EventArgs e) {
            if (this.saveLocation == "") saveAs(sender, e);
            else {
                richTextBox1.SaveFile(this.saveLocation, RichTextBoxStreamType.UnicodePlainText);
                this.Text = this.saveLocation;
                this.unsavedChanges = false;
            }
        }

        private void saveAs(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                this.saveLocation = saveFileDialog1.FileName;
                save(sender, e);
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason != CloseReason.WindowsShutDown)
                if (!userConfirmation(sender, e)){
                    e.Cancel = true;
                    return;
                }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {
            if (unsavedChanges) return;
            this.unsavedChanges = true;
            this.Text += " *";
        }

        private void openFile(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                if (!userConfirmation(sender, e)) return;
                this.saveLocation = openFileDialog1.FileName;
                richTextBox1.LoadFile(this.saveLocation, RichTextBoxStreamType.UnicodePlainText);
                this.unsavedChanges = false;
                this.Text = this.saveLocation;
            }
        }

        private bool userConfirmation(object sender, EventArgs e) {
            /* return false if user cancels the operation*/
            if (unsavedChanges)
                switch (MessageBox.Show("Do you want to save?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
                    case DialogResult.Yes:
                        save(sender, e);
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        return false;
                }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!userConfirmation(sender, e)) return;
            richTextBox1.Clear();
            this.saveLocation = "";
            this.unsavedChanges = false;
            this.Text = "New File";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("An attempt to copy Window's Notepad app by tz.man.", "info", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "https://github.com/tzmanish");
        }
    }
}
