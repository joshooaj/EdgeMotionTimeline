using System;

namespace EnhancedTimeline.Admin
{
  partial class TimelineSequenceDefinitionUserControl
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
      this.txtStopEvent = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtStartEvent = new System.Windows.Forms.TextBox();
      this.txtItemName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.txtRibbonColor = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.grid = new System.Windows.Forms.DataGridView();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.buttonColor = new System.Windows.Forms.Button();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.colorBox = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
      this.SuspendLayout();
      // 
      // txtStopEvent
      // 
      this.txtStopEvent.Location = new System.Drawing.Point(105, 64);
      this.txtStopEvent.Name = "txtStopEvent";
      this.txtStopEvent.Size = new System.Drawing.Size(152, 20);
      this.txtStopEvent.TabIndex = 11;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(26, 67);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(62, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Stop event:";
      // 
      // txtStartEvent
      // 
      this.txtStartEvent.Location = new System.Drawing.Point(105, 42);
      this.txtStartEvent.Name = "txtStartEvent";
      this.txtStartEvent.Size = new System.Drawing.Size(152, 20);
      this.txtStartEvent.TabIndex = 9;
      // 
      // txtItemName
      // 
      this.txtItemName.Location = new System.Drawing.Point(105, 19);
      this.txtItemName.Name = "txtItemName";
      this.txtItemName.Size = new System.Drawing.Size(152, 20);
      this.txtItemName.TabIndex = 8;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(26, 45);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(62, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Start event:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(78, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Timeline name:";
      // 
      // txtRibbonColor
      // 
      this.txtRibbonColor.Location = new System.Drawing.Point(105, 86);
      this.txtRibbonColor.Name = "txtRibbonColor";
      this.txtRibbonColor.Size = new System.Drawing.Size(152, 20);
      this.txtRibbonColor.TabIndex = 13;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(18, 89);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(70, 13);
      this.label4.TabIndex = 12;
      this.label4.Text = "Ribbon color:";
      // 
      // grid
      // 
      this.grid.AllowUserToAddRows = false;
      this.grid.AllowUserToDeleteRows = false;
      this.grid.AllowUserToResizeRows = false;
      this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grid.Location = new System.Drawing.Point(3, 3);
      this.grid.Name = "grid";
      this.grid.ReadOnly = true;
      this.grid.Size = new System.Drawing.Size(951, 588);
      this.grid.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(965, 620);
      this.tabControl1.TabIndex = 15;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.colorBox);
      this.tabPage1.Controls.Add(this.buttonColor);
      this.tabPage1.Controls.Add(this.txtItemName);
      this.tabPage1.Controls.Add(this.label1);
      this.tabPage1.Controls.Add(this.txtRibbonColor);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.txtStartEvent);
      this.tabPage1.Controls.Add(this.txtStopEvent);
      this.tabPage1.Controls.Add(this.label3);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(957, 594);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Definition";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // buttonColor
      // 
      this.buttonColor.Location = new System.Drawing.Point(263, 81);
      this.buttonColor.Name = "buttonColor";
      this.buttonColor.Size = new System.Drawing.Size(75, 28);
      this.buttonColor.TabIndex = 14;
      this.buttonColor.Text = "Select color";
      this.buttonColor.UseVisualStyleBackColor = true;
      this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.grid);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(957, 594);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Live Event Viewer";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // colorBox
      // 
      this.colorBox.Location = new System.Drawing.Point(344, 86);
      this.colorBox.Name = "colorBox";
      this.colorBox.Size = new System.Drawing.Size(20, 20);
      this.colorBox.TabIndex = 15;
      this.colorBox.TabStop = false;
      // 
      // TimelineSequenceDefinitionUserControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "TimelineSequenceDefinitionUserControl";
      this.Size = new System.Drawing.Size(965, 620);
      ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox txtStopEvent;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtStartEvent;
    private System.Windows.Forms.TextBox txtItemName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtRibbonColor;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Button buttonColor;
    private System.Windows.Forms.PictureBox colorBox;
  }
}
