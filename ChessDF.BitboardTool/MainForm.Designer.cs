
using System;

namespace ChessDF.BitboardTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PickColorButton = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.Color = new System.Windows.Forms.ColumnHeader();
            this.BitboardValue = new System.Windows.Forms.ColumnHeader();
            this.ClearAllButton = new System.Windows.Forms.Button();
            this.CopySelectedBitboardsButton = new System.Windows.Forms.Button();
            this.BitboardTextBox = new System.Windows.Forms.TextBox();
            this.LoadBitboardButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 8;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(22, 22);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(651, 372);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // PickColorButton
            // 
            this.PickColorButton.Location = new System.Drawing.Point(22, 418);
            this.PickColorButton.Name = "PickColorButton";
            this.PickColorButton.Size = new System.Drawing.Size(80, 48);
            this.PickColorButton.TabIndex = 1;
            this.PickColorButton.Text = "Pick Color";
            this.PickColorButton.UseVisualStyleBackColor = true;
            this.PickColorButton.Click += new System.EventHandler(this.PickColorButton_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Color,
            this.BitboardValue});
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(184, 418);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(334, 92);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // Color
            // 
            this.Color.Name = "Color";
            this.Color.Text = "Color";
            // 
            // BitboardValue
            // 
            this.BitboardValue.Name = "BitboardValue";
            this.BitboardValue.Text = "Bitboard Value";
            this.BitboardValue.Width = 200;
            // 
            // ClearAllButton
            // 
            this.ClearAllButton.Location = new System.Drawing.Point(576, 418);
            this.ClearAllButton.Name = "ClearAllButton";
            this.ClearAllButton.Size = new System.Drawing.Size(97, 35);
            this.ClearAllButton.TabIndex = 3;
            this.ClearAllButton.Text = "Clear all";
            this.ClearAllButton.UseVisualStyleBackColor = true;
            this.ClearAllButton.Click += new System.EventHandler(this.ClearAllButton_Click);
            // 
            // CopySelectedBitboardsButton
            // 
            this.CopySelectedBitboardsButton.Location = new System.Drawing.Point(576, 460);
            this.CopySelectedBitboardsButton.Name = "CopySelectedBitboardsButton";
            this.CopySelectedBitboardsButton.Size = new System.Drawing.Size(97, 50);
            this.CopySelectedBitboardsButton.TabIndex = 4;
            this.CopySelectedBitboardsButton.Text = "Copy selected bitboards";
            this.CopySelectedBitboardsButton.UseVisualStyleBackColor = true;
            this.CopySelectedBitboardsButton.Click += new System.EventHandler(this.CopySelectedBitboardsButton_Click);
            // 
            // BitboardTextBox
            // 
            this.BitboardTextBox.Location = new System.Drawing.Point(184, 530);
            this.BitboardTextBox.Name = "BitboardTextBox";
            this.BitboardTextBox.Size = new System.Drawing.Size(334, 23);
            this.BitboardTextBox.TabIndex = 5;
            // 
            // LoadBitboardButton
            // 
            this.LoadBitboardButton.Location = new System.Drawing.Point(576, 530);
            this.LoadBitboardButton.Name = "LoadBitboardButton";
            this.LoadBitboardButton.Size = new System.Drawing.Size(97, 23);
            this.LoadBitboardButton.TabIndex = 6;
            this.LoadBitboardButton.Text = "Load bitboard";
            this.LoadBitboardButton.UseVisualStyleBackColor = true;
            this.LoadBitboardButton.Click += new System.EventHandler(this.LoadBitboardButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 581);
            this.Controls.Add(this.LoadBitboardButton);
            this.Controls.Add(this.BitboardTextBox);
            this.Controls.Add(this.CopySelectedBitboardsButton);
            this.Controls.Add(this.ClearAllButton);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.PickColorButton);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "MainForm";
            this.Text = "Bitboard Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button PickColorButton;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader Color;
        private System.Windows.Forms.ColumnHeader BitboardValue;
        private System.Windows.Forms.Button ClearAllButton;
        private System.Windows.Forms.Button CopySelectedBitboardsButton;
        private System.Windows.Forms.TextBox BitboardTextBox;
        private System.Windows.Forms.Button LoadBitboardButton;
    }
}

