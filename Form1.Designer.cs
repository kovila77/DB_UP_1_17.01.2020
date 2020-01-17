namespace DB_UP_1_17_01_2020
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
            this.tvServers = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvServers
            // 
            this.tvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvServers.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tvServers.Location = new System.Drawing.Point(0, 0);
            this.tvServers.Name = "tvServers";
            this.tvServers.Size = new System.Drawing.Size(800, 450);
            this.tvServers.TabIndex = 0;
            this.tvServers.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvServers_AfterCollapse);
            this.tvServers.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvServers_AfterExpand);
            this.tvServers.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvServers_NodeMouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tvServers);
            this.Name = "Form1";
            this.Text = "Server_tree";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvServers;
    }
}

