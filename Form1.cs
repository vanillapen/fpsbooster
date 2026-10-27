using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FPS_Booster
{
    public partial class Form1 : Form
    {
        private const string ListFilter = "Text (*.txt)|*.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Sorted = true;
            listBox2.Sorted = true;
            RefreshProcessList();
        }

        // Process list

        private void RefreshProcessList()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (Process p in Process.GetProcesses())
            {
                listBox1.Items.Add(p.ProcessName + " | " + p.Id + " | Responding: " + p.Responding);
                p.Dispose();
            }
            listBox1.EndUpdate();
        }

        // Entries look like "name | pid | Responding: x"; the pid is the middle field.
        private static bool TryGetPid(object item, out int pid)
        {
            pid = 0;
            string[] parts = (item == null ? "" : item.ToString()).Split('|');
            return parts.Length >= 2 && int.TryParse(parts[1].Trim(), out pid);
        }

        private static void KillProcess(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is System.ComponentModel.Win32Exception)
            {
                // Already gone, or protected (access denied); nothing more to do.
            }
            finally
            {
                process.Dispose();
            }
        }

        private void KillBlacklistedProcesses()
        {
            foreach (object item in listBox2.Items)
            {
                foreach (Process process in Process.GetProcessesByName(item.ToString()))
                {
                    KillProcess(process);
                }
            }
        }

        // Handlers

        private void Button1_Click(object sender, EventArgs e) // refresh
        {
            RefreshProcessList();
        }

        private void Button2_Click(object sender, EventArgs e) // kill selected process
        {
            int pid;
            if (!TryGetPid(listBox1.SelectedItem, out pid))
            {
                return;
            }

            try
            {
                KillProcess(Process.GetProcessById(pid));
            }
            catch (ArgumentException)
            {
                // Process exited since the list was built.
            }
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Button3_Click(object sender, EventArgs e) // add selected process to the kill list
        {
            string item = listBox1.SelectedItem as string;
            if (item == null)
            {
                return;
            }

            // The kill list stores the bare process name, which is what GetProcessesByName expects.
            string name = item.Split('|')[0].Trim();
            if (!listBox2.Items.Contains(name))
            {
                listBox2.Items.Add(name);
            }
        }

        private void Button4_Click(object sender, EventArgs e) // remove from the kill list
        {
            if (listBox2.SelectedIndex >= 0)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            }
        }

        private void Button5_Click(object sender, EventArgs e) // kill everything on the kill list
        {
            KillBlacklistedProcesses();
            RefreshProcessList();
        }

        private void Timer1_Tick(object sender, EventArgs e) // same as button 5, on a timer
        {
            KillBlacklistedProcesses();
        }

        private void Button6_Click(object sender, EventArgs e) // toggle the timer
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void Button7_Click(object sender, EventArgs e) // save kill list
        {
            using (var dialog = new SaveFileDialog { Filter = ListFilter })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                File.WriteAllLines(dialog.FileName, listBox2.Items.Cast<object>().Select(i => i.ToString()));
                MessageBox.Show("Success");
            }
        }

        private void Button8_Click(object sender, EventArgs e) // load kill list
        {
            using (var dialog = new OpenFileDialog { Filter = ListFilter })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                foreach (string line in File.ReadLines(dialog.FileName))
                {
                    if (line.Length > 0 && !listBox2.Items.Contains(line))
                    {
                        listBox2.Items.Add(line);
                    }
                }
            }
        }

        // Search boxes: select every entry containing the typed text.

        private static void SelectMatches(ListBox list, string query)
        {
            query = query.ToLowerInvariant();
            list.BeginUpdate();
            list.SelectedItems.Clear();
            for (int i = list.Items.Count - 1; i >= 0; i--)
            {
                if (list.Items[i].ToString().ToLowerInvariant().Contains(query))
                {
                    list.SetSelected(i, true);
                }
            }
            list.EndUpdate();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            SelectMatches(listBox1, textBox1.Text);
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            SelectMatches(listBox2, textBox2.Text);
        }
    }
}
