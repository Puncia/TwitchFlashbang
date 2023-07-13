namespace TwitchFlashbang
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            viewSwitcher = new ViewManager();
            tabPage1 = new TabPage();
            label6 = new Label();
            button2 = new Button();
            redirectUriLabel = new Label();
            label5 = new Label();
            warningLabel = new Label();
            progressBar1 = new ProgressBar();
            button_saveCredentials = new Button();
            linkLabel1 = new LinkLabel();
            label4 = new Label();
            textBox_ClientSecret = new TextBox();
            textBox_ClientID = new TextBox();
            label2 = new Label();
            textBox_channelName = new TextBox();
            label3 = new Label();
            tabPage2 = new TabPage();
            button1 = new Button();
            label1 = new Label();
            checkedListBox1 = new CheckedListBox();
            tabPage3 = new TabPage();
            flowLayoutPanel1 = new FlowLayoutPanel();
            viewSwitcher.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // viewSwitcher
            // 
            viewSwitcher.Controls.Add(tabPage1);
            viewSwitcher.Controls.Add(tabPage2);
            viewSwitcher.Controls.Add(tabPage3);
            viewSwitcher.Dock = DockStyle.Fill;
            viewSwitcher.Location = new Point(0, 0);
            viewSwitcher.Name = "viewSwitcher";
            viewSwitcher.SelectedIndex = 0;
            viewSwitcher.Size = new Size(482, 301);
            viewSwitcher.TabIndex = 8;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(31, 31, 35);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(redirectUriLabel);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(warningLabel);
            tabPage1.Controls.Add(progressBar1);
            tabPage1.Controls.Add(button_saveCredentials);
            tabPage1.Controls.Add(linkLabel1);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(textBox_ClientSecret);
            tabPage1.Controls.Add(textBox_ClientID);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(textBox_channelName);
            tabPage1.Controls.Add(label3);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(474, 273);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 204);
            label6.Name = "label6";
            label6.Size = new Size(162, 15);
            label6.TabIndex = 16;
            label6.Text = "Use the above as Redirect Uri.";
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(31, 31, 35);
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Flat;
            button2.ForeColor = Color.FromArgb(31, 31, 35);
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.Location = new Point(8, 169);
            button2.Name = "button2";
            button2.Size = new Size(36, 32);
            button2.TabIndex = 15;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // redirectUriLabel
            // 
            redirectUriLabel.AutoSize = true;
            redirectUriLabel.Location = new Point(42, 178);
            redirectUriLabel.Name = "redirectUriLabel";
            redirectUriLabel.Size = new Size(0, 15);
            redirectUriLabel.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 151);
            label5.Name = "label5";
            label5.Size = new Size(422, 15);
            label5.TabIndex = 13;
            label5.Text = "Go to the Twitch Dev Console and create a new App. Copy the credentials here.";
            // 
            // warningLabel
            // 
            warningLabel.AutoSize = true;
            warningLabel.ForeColor = Color.Red;
            warningLabel.Location = new Point(8, 136);
            warningLabel.Name = "warningLabel";
            warningLabel.Size = new Size(0, 15);
            warningLabel.TabIndex = 12;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Top;
            progressBar1.Location = new Point(3, 3);
            progressBar1.MarqueeAnimationSpeed = 10;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(468, 15);
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 11;
            progressBar1.Visible = false;
            // 
            // button_saveCredentials
            // 
            button_saveCredentials.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_saveCredentials.BackColor = Color.FromArgb(145, 71, 255);
            button_saveCredentials.Enabled = false;
            button_saveCredentials.FlatStyle = FlatStyle.Flat;
            button_saveCredentials.ForeColor = Color.FromArgb(239, 239, 241);
            button_saveCredentials.Location = new Point(379, 238);
            button_saveCredentials.Name = "button_saveCredentials";
            button_saveCredentials.Size = new Size(87, 27);
            button_saveCredentials.TabIndex = 3;
            button_saveCredentials.Text = "Save";
            button_saveCredentials.UseVisualStyleBackColor = false;
            button_saveCredentials.Click += button_saveCredentials_Click;
            // 
            // linkLabel1
            // 
            linkLabel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            linkLabel1.AutoSize = true;
            linkLabel1.ForeColor = Color.FromArgb(239, 239, 241);
            linkLabel1.LinkColor = Color.FromArgb(239, 239, 241);
            linkLabel1.Location = new Point(6, 136);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(191, 15);
            linkLabel1.TabIndex = 10;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://dev.twitch.tv/console/apps";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 30);
            label4.Name = "label4";
            label4.Size = new Size(84, 15);
            label4.TabIndex = 9;
            label4.Text = "Channel name";
            // 
            // textBox_ClientSecret
            // 
            textBox_ClientSecret.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_ClientSecret.BorderStyle = BorderStyle.FixedSingle;
            textBox_ClientSecret.Enabled = false;
            textBox_ClientSecret.Location = new Point(96, 84);
            textBox_ClientSecret.Name = "textBox_ClientSecret";
            textBox_ClientSecret.Size = new Size(370, 23);
            textBox_ClientSecret.TabIndex = 2;
            textBox_ClientSecret.UseSystemPasswordChar = true;
            // 
            // textBox_ClientID
            // 
            textBox_ClientID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_ClientID.BorderStyle = BorderStyle.FixedSingle;
            textBox_ClientID.Enabled = false;
            textBox_ClientID.Location = new Point(96, 55);
            textBox_ClientID.Name = "textBox_ClientID";
            textBox_ClientID.Size = new Size(370, 23);
            textBox_ClientID.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(239, 239, 241);
            label2.Location = new Point(6, 57);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 3;
            label2.Text = "Client ID";
            // 
            // textBox_channelName
            // 
            textBox_channelName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox_channelName.Enabled = false;
            textBox_channelName.Location = new Point(96, 27);
            textBox_channelName.Name = "textBox_channelName";
            textBox_channelName.Size = new Size(370, 23);
            textBox_channelName.TabIndex = 0;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.ForeColor = Color.FromArgb(239, 239, 241);
            label3.Location = new Point(6, 86);
            label3.Name = "label3";
            label3.Size = new Size(72, 15);
            label3.TabIndex = 4;
            label3.Text = "Client secret";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.FromArgb(31, 31, 35);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(checkedListBox1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(474, 273);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.BackColor = Color.FromArgb(145, 71, 255);
            button1.Enabled = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(239, 239, 241);
            button1.Location = new Point(379, 238);
            button1.Name = "button1";
            button1.Size = new Size(87, 27);
            button1.TabIndex = 11;
            button1.Text = "Next";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 3);
            label1.Name = "label1";
            label1.Size = new Size(259, 15);
            label1.TabIndex = 1;
            label1.Text = "Select which reward(s) will trigger the flashbang";
            // 
            // checkedListBox1
            // 
            checkedListBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            checkedListBox1.BackColor = Color.FromArgb(30, 30, 46);
            checkedListBox1.BorderStyle = BorderStyle.None;
            checkedListBox1.ForeColor = SystemColors.Info;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(6, 30);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(458, 72);
            checkedListBox1.TabIndex = 0;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(flowLayoutPanel1);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(474, 273);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.ForeColor = SystemColors.ControlText;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(474, 273);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // App
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 37);
            ClientSize = new Size(482, 301);
            Controls.Add(viewSwitcher);
            ForeColor = Color.FromArgb(239, 239, 241);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(498, 340);
            Name = "App";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Twitch Flashbang";
            FormClosed += App_FormClosed;
            Load += App_Load;
            viewSwitcher.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ViewManager viewSwitcher;
        private TabPage tabPage2;
        private CheckedListBox checkedListBox1;
        private Label label1;
        private Button button1;
        private TabPage tabPage3;
        private FlowLayoutPanel flowLayoutPanel1;
        private TabPage tabPage1;
        private Button button2;
        private Label redirectUriLabel;
        private Label label5;
        private Label warningLabel;
        private ProgressBar progressBar1;
        private Button button_saveCredentials;
        private LinkLabel linkLabel1;
        private Label label4;
        private TextBox textBox_ClientSecret;
        private TextBox textBox_ClientID;
        private Label label2;
        private TextBox textBox_channelName;
        private Label label3;
        private Label label6;
    }
}