﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

/* This program is not done, and really just decreases FPS. It's kinda like an ugly barely working needed admin privilages task manager, which will be switched later
 The basic code is down, and if you spend 10 minutes you could make it an effective fps booster
 be warned, this could crash your computer as some really shitty coding practices have taken place, out of pure lazyness. you have been warned.
 */

namespace FPS_Booster // False name, Computer_Crasher would be better. Atleast for now.
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
        }

        private void Button1_Click(object sender, EventArgs e)
        {
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
            }
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e) // search bar, will change from textbox1 later.
        {
            listBox1.SelectedItems.Clear();  // clears list, only shows items with searched letter
            for (int i = listBox1.Items.Count -1; i >= 0;i--) 
            {
                if (listBox1.Items[i].ToString().ToLower().Contains(textBox1.Text)) // does so many things, but mainly adds the searched items back to he list
                {
                    listBox1.SetSelected(i, true);
                }
            }
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
                    process.Kill();
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e) // kills all processes, but its on a timer
        {
            string itemname = "";
            foreach (var item in listBox2.Items)
            {
                itemname = item.ToString();
                foreach (var process in Process.GetProcessesByName(itemname))
                {
                    process.Kill();
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
    }
}
