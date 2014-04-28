namespace Server
{
    partial class MainServerForm
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
            this.serverstartbutton = new System.Windows.Forms.Button();
            this.consolelabel = new System.Windows.Forms.Label();
            this.serverstopbutton = new System.Windows.Forms.Button();
            this.consoletextbox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.iplabel = new System.Windows.Forms.Label();
            this.portlabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverstartbutton
            // 
            this.serverstartbutton.Location = new System.Drawing.Point(482, 23);
            this.serverstartbutton.Name = "serverstartbutton";
            this.serverstartbutton.Size = new System.Drawing.Size(75, 23);
            this.serverstartbutton.TabIndex = 0;
            this.serverstartbutton.Text = "Start Server";
            this.serverstartbutton.UseVisualStyleBackColor = true;
            this.serverstartbutton.Click += new System.EventHandler(this.serverstartbutton_Click);
            // 
            // consolelabel
            // 
            this.consolelabel.AutoSize = true;
            this.consolelabel.Location = new System.Drawing.Point(25, 171);
            this.consolelabel.Name = "consolelabel";
            this.consolelabel.Size = new System.Drawing.Size(48, 13);
            this.consolelabel.TabIndex = 1;
            this.consolelabel.Text = "Console:";
            // 
            // serverstopbutton
            // 
            this.serverstopbutton.Location = new System.Drawing.Point(482, 76);
            this.serverstopbutton.Name = "serverstopbutton";
            this.serverstopbutton.Size = new System.Drawing.Size(75, 23);
            this.serverstopbutton.TabIndex = 2;
            this.serverstopbutton.Text = "Stop Server";
            this.serverstopbutton.UseVisualStyleBackColor = true;
            this.serverstopbutton.Click += new System.EventHandler(this.serverstopbutton_Click);
            // 
            // consoletextbox
            // 
            this.consoletextbox.Location = new System.Drawing.Point(28, 199);
            this.consoletextbox.Multiline = true;
            this.consoletextbox.Name = "consoletextbox";
            this.consoletextbox.Size = new System.Drawing.Size(529, 170);
            this.consoletextbox.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(110, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(110, 78);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 5;
            // 
            // iplabel
            // 
            this.iplabel.AutoSize = true;
            this.iplabel.Location = new System.Drawing.Point(25, 23);
            this.iplabel.Name = "iplabel";
            this.iplabel.Size = new System.Drawing.Size(61, 13);
            this.iplabel.TabIndex = 6;
            this.iplabel.Text = "IP Address:";
            // 
            // portlabel
            // 
            this.portlabel.AutoSize = true;
            this.portlabel.Location = new System.Drawing.Point(25, 81);
            this.portlabel.Name = "portlabel";
            this.portlabel.Size = new System.Drawing.Size(69, 13);
            this.portlabel.TabIndex = 7;
            this.portlabel.Text = "Port Number:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 381);
            this.Controls.Add(this.portlabel);
            this.Controls.Add(this.iplabel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.consoletextbox);
            this.Controls.Add(this.serverstopbutton);
            this.Controls.Add(this.consolelabel);
            this.Controls.Add(this.serverstartbutton);
            this.Name = "MainForm";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button serverstartbutton;
        private System.Windows.Forms.Label consolelabel;
        private System.Windows.Forms.Button serverstopbutton;
        private System.Windows.Forms.TextBox consoletextbox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label iplabel;
        private System.Windows.Forms.Label portlabel;
    }
}

