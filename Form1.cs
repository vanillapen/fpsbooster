using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

/* Needs work
 * You've been warned.
 */

namespace FPS_Booster
{
    public partial class Form1 : Form
    {
        bool killon = false;
        Process[] pArry = Process.GetProcesses(); // creates process array
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Sorted = true; // alphabetically sorts listbox
            listBox2.Sorted = true;
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (Process p in pArry) // for each, in array - pretty straight foward.
            {
               
                string s = p.ProcessName; // gets process name, and sets to string
                s = s.ToLower(); // lowercases processname
                listBox1.Items.Add(p.ProcessName); // adds process name to list, need to change this to a list checkbox
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int indx = listBox1.SelectedIndex; // gets selected item
            Process proc = pArry[indx]; // selected index is now equal to a process index
            if (proc.HasExited == false) // checks if process has exited
            {
                proc.Kill(); // kills process
                proc.Dispose(); // disposes process
                listBox1.Items.RemoveAt(indx); //removes selected process from list, after it has been killed and disposed of
            } else if (proc.HasExited == true)
            {
                listBox1.Items.RemoveAt(indx);
            }
            else
            {
                MessageBox.Show("Error 100 - Message this error code to the developer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e) // search bar, will change from textbox1 later.
        {
          
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem); // adds item to listbox2, no actual  saving function yet
        }

        private void Button4_Click(object sender, EventArgs e) // remove function for listbox2
        {
            int indx = listBox2.SelectedIndex; 
            listBox2.Items.RemoveAt(indx);
        }

        private void Button5_Click(object sender, EventArgs e) // kills all processes
        {
            string itemname = "";
            
            foreach (var item in listBox2.Items)
            {
                itemname = item.ToString();
                foreach (var process in Process.GetProcessesByName(itemname))
                {
                    if (process.HasExited == false)
                    {
                        process.Kill();
                        process.Dispose();
                        listBox1.Items.Remove(itemname);
                         
                    } else if (process.HasExited == true)
                    {
                        
                    } else
                    {
                        MessageBox.Show("Error 101 - Message this error code to the developer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e) // kills all processes, but its on a timer ( 1 minute )
        {
            string itemname = "";
            foreach (var item in listBox2.Items)
            {
                itemname = item.ToString();
                foreach (var process in Process.GetProcessesByName(itemname))
                {
                    if (process.HasExited == false)
                    {
                        process.Kill();
                        process.Dispose();
                    }
                    else if (process.HasExited == true)
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("Error 102 - Message this error code to the developer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    
                }
            }
        }

        private void Button6_Click(object sender, EventArgs e) // enables and disables timer1.
        {
            killon = !killon;

            if (killon == true)
            {
                timer1.Start();
            } else if (killon == false)
            {
                timer1.Stop();
            }
        }

        private void Button7_Click(object sender, EventArgs e) // Save file
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text (*.txt)|*.txt";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var sw = new StreamWriter(saveFile.FileName, false))
                    foreach (var item in listBox2.Items)
                        sw.Write(item.ToString() + Environment.NewLine);
                MessageBox.Show("Success");
            }
        }

        private void Button8_Click(object sender, EventArgs e) // open file
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text (*.txt)|*.txt";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> lines = new List<string>();
                using (var sr = new StreamReader(openFile.FileName, false))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        listBox2.Items.Add(line);
                    }
                }
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            listBox2.SelectedItems.Clear();  // clears list, only shows items with searched letter
            for (int i = listBox2.Items.Count - 1; i >= 0; i--)
            {
                if (listBox2.Items[i].ToString().ToLower().Contains(textBox2.Text)) // does so many things, but mainly adds the searched items back to he list
                {
                    listBox2.Sorted = true;
                    listBox2.SetSelected(i, true);
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.SelectedItems.Clear();  // clears list, only shows items with searched letter
            for (int i = listBox1.Items.Count - 1; i >= 0; i--)
            {
                if (listBox1.Items[i].ToString().ToLower().Contains(textBox1.Text)) // does so many things, but mainly adds the searched items back to he list
                {
                    listBox1.Sorted = true;
                    listBox1.SetSelected(i, true);
                }
            }
        }
    }
}
