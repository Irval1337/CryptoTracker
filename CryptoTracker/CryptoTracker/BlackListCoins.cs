using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoTracker
{
    public partial class BlackListCoins : Form
    {
        public BlackListCoins()
        {
            InitializeComponent();
        }

        private void BlackList_Load(object sender, EventArgs e)
        {
            App.LoadSettings();

            foreach (var market in App.Settings.BlackListedCoins)
            {
                listBox1.Items.Add(market);
            }
        }

        private void BlackList_FormClosed(object sender, FormClosedEventArgs e)
        {
            App.SaveSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            listBox1.Items.Add(textBox1.Text);
            App.Settings.BlackListedCoins.Add(textBox1.Text);

            textBox1.Clear();
            listBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            App.Settings.BlackListedCoins.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);

            listBox1.SelectedIndex = -1;
        }
    }
}
