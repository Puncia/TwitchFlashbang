namespace TwitchFlashbang
{
    partial class UsedFlashbangUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flashingTimeLabel = new Label();
            NthLabel = new Label();
            fadingTimeLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            abortedLabel = new Label();
            IDLabel = new Label();
            SuspendLayout();
            // 
            // flashingTimeLabel
            // 
            flashingTimeLabel.AutoSize = true;
            flashingTimeLabel.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            flashingTimeLabel.Location = new Point(300, 5);
            flashingTimeLabel.Name = "flashingTimeLabel";
            flashingTimeLabel.Size = new Size(35, 13);
            flashingTimeLabel.TabIndex = 1;
            flashingTimeLabel.Text = "1000";
            flashingTimeLabel.Visible = false;
            // 
            // NthLabel
            // 
            NthLabel.AutoSize = true;
            NthLabel.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            NthLabel.ForeColor = Color.FromArgb(64, 64, 64);
            NthLabel.Location = new Point(8, 5);
            NthLabel.Name = "NthLabel";
            NthLabel.Size = new Size(14, 13);
            NthLabel.TabIndex = 2;
            NthLabel.Text = "1";
            // 
            // fadingTimeLabel
            // 
            fadingTimeLabel.AutoSize = true;
            fadingTimeLabel.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            fadingTimeLabel.Location = new Point(300, 19);
            fadingTimeLabel.Name = "fadingTimeLabel";
            fadingTimeLabel.Size = new Size(35, 13);
            fadingTimeLabel.TabIndex = 3;
            fadingTimeLabel.Text = "1000";
            fadingTimeLabel.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(196, 5);
            label1.Name = "label1";
            label1.Size = new Size(98, 13);
            label1.TabIndex = 4;
            label1.Text = "Flashing Time";
            label1.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(210, 19);
            label2.Name = "label2";
            label2.Size = new Size(84, 13);
            label2.TabIndex = 5;
            label2.Text = "Fading Time";
            label2.Visible = false;
            // 
            // abortedLabel
            // 
            abortedLabel.AutoSize = true;
            abortedLabel.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            abortedLabel.ForeColor = Color.Firebrick;
            abortedLabel.Location = new Point(28, 5);
            abortedLabel.Name = "abortedLabel";
            abortedLabel.Size = new Size(56, 13);
            abortedLabel.TabIndex = 6;
            abortedLabel.Text = "Aborted";
            abortedLabel.Visible = false;
            // 
            // IDLabel
            // 
            IDLabel.AutoSize = true;
            IDLabel.Font = new Font("Fira Code", 8.249999F, FontStyle.Regular, GraphicsUnit.Point);
            IDLabel.Location = new Point(8, 22);
            IDLabel.Name = "IDLabel";
            IDLabel.Size = new Size(63, 13);
            IDLabel.TabIndex = 7;
            IDLabel.Text = "fbadf00d";
            IDLabel.Visible = false;
            // 
            // UsedFlashbangUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ControlLight;
            Controls.Add(IDLabel);
            Controls.Add(abortedLabel);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(fadingTimeLabel);
            Controls.Add(NthLabel);
            Controls.Add(flashingTimeLabel);
            Margin = new Padding(2);
            Name = "UsedFlashbangUI";
            Padding = new Padding(5);
            Size = new Size(343, 178);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label flashingTimeLabel;
        private Label NthLabel;
        private Label fadingTimeLabel;
        private Label label1;
        private Label label2;
        private Label abortedLabel;
        private Label IDLabel;
    }
}
