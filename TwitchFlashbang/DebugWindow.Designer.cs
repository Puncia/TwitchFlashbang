﻿namespace TwitchFlashbang
{
    partial class DebugWindow
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
            flashbangCountLabel = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // flashbangCountLabel
            // 
            flashbangCountLabel.AutoSize = true;
            flashbangCountLabel.Location = new Point(59, 37);
            flashbangCountLabel.Name = "flashbangCountLabel";
            flashbangCountLabel.Size = new Size(13, 15);
            flashbangCountLabel.TabIndex = 1;
            flashbangCountLabel.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 37);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 2;
            label1.Text = "count:";
            // 
            // DebugWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(234, 61);
            Controls.Add(label1);
            Controls.Add(flashbangCountLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DebugWindow";
            ShowIcon = false;
            Text = "Twitch Flashbanger";
            FormClosed += DebugWindow_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label flashbangCountLabel;
        private Label label1;
    }
}