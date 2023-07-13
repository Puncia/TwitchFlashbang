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
            flashbangTime = new Label();
            NthLabel = new Label();
            SuspendLayout();
            // 
            // flashbangTime
            // 
            flashbangTime.AutoSize = true;
            flashbangTime.Location = new Point(128, 5);
            flashbangTime.Name = "flashbangTime";
            flashbangTime.Size = new Size(31, 15);
            flashbangTime.TabIndex = 1;
            flashbangTime.Text = "1000";
            flashbangTime.Visible = false;
            // 
            // NthLabel
            // 
            NthLabel.AutoSize = true;
            NthLabel.Font = new Font("Fira Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            NthLabel.ForeColor = Color.FromArgb(64, 64, 64);
            NthLabel.Location = new Point(8, 5);
            NthLabel.Name = "NthLabel";
            NthLabel.Size = new Size(63, 19);
            NthLabel.TabIndex = 2;
            NthLabel.Text = "label1";
            // 
            // UsedFlashbangUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ControlLight;
            Controls.Add(NthLabel);
            Controls.Add(flashbangTime);
            Margin = new Padding(5);
            Name = "UsedFlashbangUI";
            Padding = new Padding(5);
            Size = new Size(343, 178);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label flashbangTime;
        private Label NthLabel;
    }
}
