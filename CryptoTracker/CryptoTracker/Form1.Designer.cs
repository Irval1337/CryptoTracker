namespace CryptoTracker
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.авторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.черныйСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MarketSellPrice = new System.Windows.Forms.Label();
            this.MarketSellCurrency = new System.Windows.Forms.Label();
            this.MarketSellName = new System.Windows.Forms.Label();
            this.MarketBuyPrice = new System.Windows.Forms.Label();
            this.Percent = new System.Windows.Forms.Label();
            this.OpenLink = new System.Windows.Forms.LinkLabel();
            this.MarketBuyCurrency = new System.Windows.Forms.Label();
            this.MarketBuyName = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 23);
            this.button1.TabIndex = 0;
            this.button1.TabStop = false;
            this.button1.Text = "Начать парсинг";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(225, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(349, 355);
            this.listBox1.TabIndex = 2;
            this.listBox1.TabStop = false;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Минимальный процент вилки";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 54);
            this.textBox1.MaxLength = 2;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 20);
            this.textBox1.TabIndex = 4;
            this.textBox1.TabStop = false;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выходToolStripMenuItem,
            this.авторToolStripMenuItem,
            this.черныйСписокToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(586, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // авторToolStripMenuItem
            // 
            this.авторToolStripMenuItem.Name = "авторToolStripMenuItem";
            this.авторToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.авторToolStripMenuItem.Text = "Автор";
            this.авторToolStripMenuItem.Click += new System.EventHandler(this.авторToolStripMenuItem_Click);
            // 
            // черныйСписокToolStripMenuItem
            // 
            this.черныйСписокToolStripMenuItem.Name = "черныйСписокToolStripMenuItem";
            this.черныйСписокToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.черныйСписокToolStripMenuItem.Text = "Черный список бирж";
            this.черныйСписокToolStripMenuItem.Click += new System.EventHandler(this.черныйСписокToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MarketSellPrice);
            this.groupBox1.Controls.Add(this.MarketSellCurrency);
            this.groupBox1.Controls.Add(this.MarketSellName);
            this.groupBox1.Controls.Add(this.MarketBuyPrice);
            this.groupBox1.Controls.Add(this.Percent);
            this.groupBox1.Controls.Add(this.OpenLink);
            this.groupBox1.Controls.Add(this.MarketBuyCurrency);
            this.groupBox1.Controls.Add(this.MarketBuyName);
            this.groupBox1.Location = new System.Drawing.Point(12, 144);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 249);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация";
            // 
            // MarketSellPrice
            // 
            this.MarketSellPrice.AutoSize = true;
            this.MarketSellPrice.Location = new System.Drawing.Point(7, 149);
            this.MarketSellPrice.Name = "MarketSellPrice";
            this.MarketSellPrice.Size = new System.Drawing.Size(112, 13);
            this.MarketSellPrice.TabIndex = 9;
            this.MarketSellPrice.Text = "Стоимость продажи:";
            // 
            // MarketSellCurrency
            // 
            this.MarketSellCurrency.AutoSize = true;
            this.MarketSellCurrency.Location = new System.Drawing.Point(7, 128);
            this.MarketSellCurrency.Name = "MarketSellCurrency";
            this.MarketSellCurrency.Size = new System.Drawing.Size(95, 13);
            this.MarketSellCurrency.TabIndex = 8;
            this.MarketSellCurrency.Text = "Валюта продажи:";
            // 
            // MarketSellName
            // 
            this.MarketSellName.AutoSize = true;
            this.MarketSellName.Location = new System.Drawing.Point(7, 108);
            this.MarketSellName.Name = "MarketSellName";
            this.MarketSellName.Size = new System.Drawing.Size(90, 13);
            this.MarketSellName.TabIndex = 7;
            this.MarketSellName.Text = "Биржа продажи:";
            // 
            // MarketBuyPrice
            // 
            this.MarketBuyPrice.AutoSize = true;
            this.MarketBuyPrice.Location = new System.Drawing.Point(7, 67);
            this.MarketBuyPrice.Name = "MarketBuyPrice";
            this.MarketBuyPrice.Size = new System.Drawing.Size(109, 13);
            this.MarketBuyPrice.TabIndex = 6;
            this.MarketBuyPrice.Text = "Стоимость покупки:";
            // 
            // Percent
            // 
            this.Percent.AutoSize = true;
            this.Percent.Location = new System.Drawing.Point(7, 190);
            this.Percent.Name = "Percent";
            this.Percent.Size = new System.Drawing.Size(100, 13);
            this.Percent.TabIndex = 5;
            this.Percent.Text = "Процент прибыли:";
            // 
            // OpenLink
            // 
            this.OpenLink.AutoSize = true;
            this.OpenLink.Location = new System.Drawing.Point(18, 224);
            this.OpenLink.Name = "OpenLink";
            this.OpenLink.Size = new System.Drawing.Size(158, 13);
            this.OpenLink.TabIndex = 4;
            this.OpenLink.TabStop = true;
            this.OpenLink.Text = "Открыть страницу арбитража";
            this.OpenLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenLink_LinkClicked);
            // 
            // MarketBuyCurrency
            // 
            this.MarketBuyCurrency.AutoSize = true;
            this.MarketBuyCurrency.Location = new System.Drawing.Point(7, 46);
            this.MarketBuyCurrency.Name = "MarketBuyCurrency";
            this.MarketBuyCurrency.Size = new System.Drawing.Size(92, 13);
            this.MarketBuyCurrency.TabIndex = 1;
            this.MarketBuyCurrency.Text = "Валюта покупки:";
            // 
            // MarketBuyName
            // 
            this.MarketBuyName.AutoSize = true;
            this.MarketBuyName.Location = new System.Drawing.Point(7, 26);
            this.MarketBuyName.Name = "MarketBuyName";
            this.MarketBuyName.Size = new System.Drawing.Size(87, 13);
            this.MarketBuyName.TabIndex = 0;
            this.MarketBuyName.Text = "Биржа покупки:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 113);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(130, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Отображать валюты";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(499, 398);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Version: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 416);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "CryptoTracker";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem черныйСписокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem авторToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Percent;
        private System.Windows.Forms.LinkLabel OpenLink;
        private System.Windows.Forms.Label MarketBuyCurrency;
        private System.Windows.Forms.Label MarketBuyName;
        private System.Windows.Forms.Label MarketSellPrice;
        private System.Windows.Forms.Label MarketSellCurrency;
        private System.Windows.Forms.Label MarketSellName;
        private System.Windows.Forms.Label MarketBuyPrice;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
    }
}

