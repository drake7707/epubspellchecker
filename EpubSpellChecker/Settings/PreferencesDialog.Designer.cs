namespace EpubSpellChecker
{
    partial class PreferencesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesDialog));
            this.btnSave = new System.Windows.Forms.Button();
            this.grpTests = new System.Windows.Forms.GroupBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chklstTests = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOCRWarnings = new System.Windows.Forms.CheckBox();
            this.chkOnlyUseAppliedOCRPatterns = new System.Windows.Forms.CheckBox();
            this.grpTests.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(472, 443);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpTests
            // 
            this.grpTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpTests.Controls.Add(this.lblDescription);
            this.grpTests.Controls.Add(this.chklstTests);
            this.grpTests.Location = new System.Drawing.Point(2, 12);
            this.grpTests.Name = "grpTests";
            this.grpTests.Size = new System.Drawing.Size(325, 418);
            this.grpTests.TabIndex = 5;
            this.grpTests.TabStop = false;
            this.grpTests.Text = "Available tests";
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(6, 357);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(313, 58);
            this.lblDescription.TabIndex = 1;
            // 
            // chklstTests
            // 
            this.chklstTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chklstTests.FormattingEnabled = true;
            this.chklstTests.Location = new System.Drawing.Point(6, 19);
            this.chklstTests.Name = "chklstTests";
            this.chklstTests.Size = new System.Drawing.Size(313, 334);
            this.chklstTests.TabIndex = 0;
            this.chklstTests.SelectedIndexChanged += new System.EventHandler(this.chklstTests_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(577, 443);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkOnlyUseAppliedOCRPatterns);
            this.groupBox1.Controls.Add(this.chkOCRWarnings);
            this.groupBox1.Location = new System.Drawing.Point(333, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(343, 418);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // chkOCRWarnings
            // 
            this.chkOCRWarnings.AutoSize = true;
            this.chkOCRWarnings.Location = new System.Drawing.Point(7, 20);
            this.chkOCRWarnings.Name = "chkOCRWarnings";
            this.chkOCRWarnings.Size = new System.Drawing.Size(275, 17);
            this.chkOCRWarnings.TabIndex = 0;
            this.chkOCRWarnings.Text = "Give warnings for possible OCR errors on valid words";
            this.chkOCRWarnings.UseVisualStyleBackColor = true;
            // 
            // chkOnlyUseAppliedOCRPatterns
            // 
            this.chkOnlyUseAppliedOCRPatterns.Location = new System.Drawing.Point(7, 43);
            this.chkOnlyUseAppliedOCRPatterns.Name = "chkOnlyUseAppliedOCRPatterns";
            this.chkOnlyUseAppliedOCRPatterns.Size = new System.Drawing.Size(330, 48);
            this.chkOnlyUseAppliedOCRPatterns.TabIndex = 1;
            this.chkOnlyUseAppliedOCRPatterns.Text = "Only give warnings for possible OCR errors when the OCR pattern fixed another wor" +
    "d in the book (this will greatly reduce the amount of warnings, but some OCR iss" +
    "ues might be missed)";
            this.chkOnlyUseAppliedOCRPatterns.UseVisualStyleBackColor = true;
            // 
            // PreferencesDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(688, 478);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpTests);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PreferencesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.grpTests.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpTests;
        private System.Windows.Forms.CheckedListBox chklstTests;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkOCRWarnings;
        private System.Windows.Forms.CheckBox chkOnlyUseAppliedOCRPatterns;
    }
}