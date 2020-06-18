namespace Migration
{
   partial class MainUI
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
         this.btnBrowse = new System.Windows.Forms.Button();
         this.btnMigrate = new System.Windows.Forms.Button();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
         this.groupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnBrowse
         // 
         this.btnBrowse.Dock = System.Windows.Forms.DockStyle.Right;
         this.btnBrowse.Location = new System.Drawing.Point(389, 3);
         this.btnBrowse.Name = "btnBrowse";
         this.btnBrowse.Size = new System.Drawing.Size(75, 39);
         this.btnBrowse.TabIndex = 0;
         this.btnBrowse.Text = "Browse";
         this.btnBrowse.UseVisualStyleBackColor = true;
         this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
         // 
         // btnMigrate
         // 
         this.btnMigrate.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnMigrate.Location = new System.Drawing.Point(309, 3);
         this.btnMigrate.Name = "btnMigrate";
         this.btnMigrate.Size = new System.Drawing.Size(161, 151);
         this.btnMigrate.TabIndex = 1;
         this.btnMigrate.Text = "Migrate";
         this.btnMigrate.UseVisualStyleBackColor = true;
         this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
         // 
         // textBox1
         // 
         this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.textBox1.Location = new System.Drawing.Point(134, 48);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(330, 22);
         this.textBox1.TabIndex = 3;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.tableLayoutPanel1);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox1.Location = new System.Drawing.Point(3, 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(473, 253);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Config";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.26552F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.73447F));
         this.tableLayoutPanel1.Controls.Add(this.checkedListBox2, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 142F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(467, 232);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // checkedListBox2
         // 
         this.checkedListBox2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.checkedListBox2.FormattingEnabled = true;
         this.checkedListBox2.Location = new System.Drawing.Point(134, 93);
         this.checkedListBox2.Name = "checkedListBox2";
         this.checkedListBox2.Size = new System.Drawing.Size(330, 136);
         this.checkedListBox2.TabIndex = 7;
         this.checkedListBox2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox2_ItemCheck);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Dock = System.Windows.Forms.DockStyle.Left;
         this.label2.Location = new System.Drawing.Point(3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(97, 45);
         this.label2.TabIndex = 5;
         this.label2.Text = "Source Folder";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Dock = System.Windows.Forms.DockStyle.Left;
         this.label1.Location = new System.Drawing.Point(3, 45);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(82, 45);
         this.label1.TabIndex = 4;
         this.label1.Text = "DefaultStaff";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Dock = System.Windows.Forms.DockStyle.Left;
         this.label3.Location = new System.Drawing.Point(3, 90);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(106, 142);
         this.label3.TabIndex = 6;
         this.label3.Text = "Migrate Section";
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 1;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.46789F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.53211F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(479, 422);
         this.tableLayoutPanel2.TabIndex = 5;
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 2;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.90486F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.09514F));
         this.tableLayoutPanel3.Controls.Add(this.btnMigrate, 1, 0);
         this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 262);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 1;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(473, 157);
         this.tableLayoutPanel3.TabIndex = 5;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.checkedListBox1);
         this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox2.Location = new System.Drawing.Point(3, 3);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(300, 151);
         this.groupBox2.TabIndex = 4;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Required Files";
         // 
         // checkedListBox1
         // 
         this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.checkedListBox1.Enabled = false;
         this.checkedListBox1.FormattingEnabled = true;
         this.checkedListBox1.Location = new System.Drawing.Point(3, 18);
         this.checkedListBox1.Name = "checkedListBox1";
         this.checkedListBox1.Size = new System.Drawing.Size(294, 130);
         this.checkedListBox1.TabIndex = 3;
         // 
         // MainUI
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(479, 422);
         this.Controls.Add(this.tableLayoutPanel2);
         this.MaximizeBox = false;
         this.Name = "MainUI";
         this.Text = "Migration";
         this.Load += new System.EventHandler(this.MainUI_Load);
         this.groupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.groupBox2.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnBrowse;
      private System.Windows.Forms.Button btnMigrate;
      private System.Windows.Forms.TextBox textBox1;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.CheckedListBox checkedListBox1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.CheckedListBox checkedListBox2;
      private System.Windows.Forms.Label label3;
   }
}

