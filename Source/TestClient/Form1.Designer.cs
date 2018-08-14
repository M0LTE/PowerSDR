namespace TestClient
{
    partial class Form1
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
            this.Init = new System.Windows.Forms.Button();
            this.setNotificationCallback = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Init
            // 
            this.Init.Location = new System.Drawing.Point(28, 29);
            this.Init.Name = "Init";
            this.Init.Size = new System.Drawing.Size(75, 23);
            this.Init.TabIndex = 0;
            this.Init.Text = "Init";
            this.Init.UseVisualStyleBackColor = true;
            this.Init.Click += new System.EventHandler(this.Init_Click);
            // 
            // setNotificationCallback
            // 
            this.setNotificationCallback.Location = new System.Drawing.Point(28, 91);
            this.setNotificationCallback.Name = "setNotificationCallback";
            this.setNotificationCallback.Size = new System.Drawing.Size(157, 23);
            this.setNotificationCallback.TabIndex = 1;
            this.setNotificationCallback.Text = "Set notification callback";
            this.setNotificationCallback.UseVisualStyleBackColor = true;
            this.setNotificationCallback.Click += new System.EventHandler(this.setNotificationCallback_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.setNotificationCallback);
            this.Controls.Add(this.Init);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Init;
        private System.Windows.Forms.Button setNotificationCallback;
    }
}

