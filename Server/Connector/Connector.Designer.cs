namespace Server
{
    partial class Connector
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
            this.connectorbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // connectorbutton
            // 
            this.connectorbutton.Location = new System.Drawing.Point(36, 24);
            this.connectorbutton.Name = "connectorbutton";
            this.connectorbutton.Size = new System.Drawing.Size(75, 23);
            this.connectorbutton.TabIndex = 0;
            this.connectorbutton.Text = "Connect";
            this.connectorbutton.UseVisualStyleBackColor = true;
            this.connectorbutton.Click += new System.EventHandler(this.connectorbutton_Click);
            // 
            // Connector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 261);
            this.Controls.Add(this.connectorbutton);
            this.Name = "Connector";
            this.Text = "Connector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connectorbutton;
    }
}

