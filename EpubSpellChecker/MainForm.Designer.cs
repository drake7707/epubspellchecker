namespace EpubSpellChecker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recheckOCRPatternsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllSuggestionsToFixedTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.wordDistributionAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnlyErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grid = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lstOccurrences = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblWordCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUnknown = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSuggestion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFixed = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblIgnored = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWarning = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.tmrFilter = new System.Windows.Forms.Timer(this.components);
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Suggestion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FixedText = new EpubSpellChecker.CustomAutocompleteColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.countDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ignore = new System.Windows.Forms.DataGridViewImageColumn();
            this.AddToDictionary = new System.Windows.Forms.DataGridViewImageColumn();
            this.Copy = new System.Windows.Forms.DataGridViewImageColumn();
            this.wordEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnIgnoreLines = new System.Windows.Forms.Button();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wordEntryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(808, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recheckOCRPatternsToolStripMenuItem,
            this.copyAllSuggestionsToFixedTextToolStripMenuItem,
            this.toolStripMenuItem1,
            this.wordDistributionAnalysisToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // recheckOCRPatternsToolStripMenuItem
            // 
            this.recheckOCRPatternsToolStripMenuItem.Name = "recheckOCRPatternsToolStripMenuItem";
            this.recheckOCRPatternsToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.recheckOCRPatternsToolStripMenuItem.Text = "Recheck OCR Patterns";
            this.recheckOCRPatternsToolStripMenuItem.Click += new System.EventHandler(this.recheckOCRPatternsToolStripMenuItem_Click);
            // 
            // copyAllSuggestionsToFixedTextToolStripMenuItem
            // 
            this.copyAllSuggestionsToFixedTextToolStripMenuItem.Name = "copyAllSuggestionsToFixedTextToolStripMenuItem";
            this.copyAllSuggestionsToFixedTextToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.copyAllSuggestionsToFixedTextToolStripMenuItem.Text = "Copy all suggestions to fixed text";
            this.copyAllSuggestionsToFixedTextToolStripMenuItem.Click += new System.EventHandler(this.copyAllSuggestionsToFixedTextToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(244, 6);
            // 
            // wordDistributionAnalysisToolStripMenuItem
            // 
            this.wordDistributionAnalysisToolStripMenuItem.Name = "wordDistributionAnalysisToolStripMenuItem";
            this.wordDistributionAnalysisToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.wordDistributionAnalysisToolStripMenuItem.Text = "Word distribution analysis";
            this.wordDistributionAnalysisToolStripMenuItem.Click += new System.EventHandler(this.wordDistributionAnalysisToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnlyErrorsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showOnlyErrorsToolStripMenuItem
            // 
            this.showOnlyErrorsToolStripMenuItem.Checked = true;
            this.showOnlyErrorsToolStripMenuItem.CheckOnClick = true;
            this.showOnlyErrorsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOnlyErrorsToolStripMenuItem.Name = "showOnlyErrorsToolStripMenuItem";
            this.showOnlyErrorsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.showOnlyErrorsToolStripMenuItem.Text = "Show only errors && warnings";
            this.showOnlyErrorsToolStripMenuItem.Click += new System.EventHandler(this.showOnlyErrorsToolStripMenuItem_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoGenerateColumns = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.textDataGridViewTextBoxColumn,
            this.Type,
            this.countDataGridViewTextBoxColumn,
            this.Suggestion,
            this.Ignore,
            this.AddToDictionary,
            this.Copy,
            this.FixedText});
            this.tableLayoutPanel1.SetColumnSpan(this.grid, 2);
            this.grid.DataSource = this.wordEntryBindingSource;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 26);
            this.grid.Name = "grid";
            this.grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 30;
            this.grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid.Size = new System.Drawing.Size(802, 286);
            this.grid.TabIndex = 1;
            this.grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
            this.grid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellContentClick);
            this.grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_EditingControlShowing);
            this.grid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.grid_RowPrePaint);
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(808, 465);
            this.splitContainer1.SplitterDistance = 315;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grid, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFilter, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(808, 315);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFilter
            // 
            this.txtFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter.Location = new System.Drawing.Point(62, 3);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(743, 20);
            this.txtFilter.TabIndex = 3;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel2.Controls.Add(this.lstOccurrences, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.statusStrip1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnIgnoreLines, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(808, 146);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // lstOccurrences
            // 
            this.lstOccurrences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOccurrences.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstOccurrences.FormattingEnabled = true;
            this.lstOccurrences.Location = new System.Drawing.Point(3, 3);
            this.lstOccurrences.Name = "lstOccurrences";
            this.tableLayoutPanel2.SetRowSpan(this.lstOccurrences, 2);
            this.lstOccurrences.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstOccurrences.Size = new System.Drawing.Size(776, 118);
            this.lstOccurrences.TabIndex = 0;
            this.lstOccurrences.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstOccurrences_DrawItem);
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.statusStrip1, 2);
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblWordCount,
            this.lblUnknown,
            this.lblSuggestion,
            this.lblFixed,
            this.lblIgnored,
            this.lblWarning,
            this.lblStatus,
            this.progress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 124);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(808, 22);
            this.statusStrip1.TabIndex = 2;
            // 
            // lblWordCount
            // 
            this.lblWordCount.Name = "lblWordCount";
            this.lblWordCount.Size = new System.Drawing.Size(55, 17);
            this.lblWordCount.Text = "Words: - ";
            // 
            // lblUnknown
            // 
            this.lblUnknown.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblUnknown.Name = "lblUnknown";
            this.lblUnknown.Size = new System.Drawing.Size(73, 17);
            this.lblUnknown.Text = "Unknown: -";
            // 
            // lblSuggestion
            // 
            this.lblSuggestion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblSuggestion.Name = "lblSuggestion";
            this.lblSuggestion.Size = new System.Drawing.Size(77, 17);
            this.lblSuggestion.Text = "Suggested: -";
            // 
            // lblFixed
            // 
            this.lblFixed.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblFixed.Name = "lblFixed";
            this.lblFixed.Size = new System.Drawing.Size(49, 17);
            this.lblFixed.Text = "Fixed: -";
            // 
            // lblIgnored
            // 
            this.lblIgnored.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblIgnored.Name = "lblIgnored";
            this.lblIgnored.Size = new System.Drawing.Size(63, 17);
            this.lblIgnored.Text = "Ignored: -";
            // 
            // lblWarning
            // 
            this.lblWarning.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(67, 17);
            this.lblWarning.Text = "Warning: -";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(307, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Loading epub...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblStatus.Visible = false;
            // 
            // progress
            // 
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Visible = false;
            // 
            // tmrFilter
            // 
            this.tmrFilter.Interval = 250;
            this.tmrFilter.Tick += new System.EventHandler(this.tmrFilter_Tick);
            // 
            // Type
            // 
            this.Type.DataPropertyName = "UnknownType";
            this.Type.FillWeight = 105.2321F;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Suggestion
            // 
            this.Suggestion.DataPropertyName = "Suggestion";
            this.Suggestion.FillWeight = 105.2321F;
            this.Suggestion.HeaderText = "Suggestion";
            this.Suggestion.Name = "Suggestion";
            this.Suggestion.ReadOnly = true;
            // 
            // FixedText
            // 
            this.FixedText.DataPropertyName = "FixedText";
            this.FixedText.FillWeight = 105.2321F;
            this.FixedText.HeaderText = "FixedText";
            this.FixedText.Name = "FixedText";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "UnknownType";
            this.dataGridViewTextBoxColumn1.HeaderText = "Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 742;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.FillWeight = 20F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::EpubSpellChecker.Properties.Resources.ignore_icon_16px;
            this.dataGridViewImageColumn1.MinimumWidth = 20;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.ToolTipText = "Ignore";
            this.dataGridViewImageColumn1.Width = 391;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn2.FillWeight = 20F;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::EpubSpellChecker.Properties.Resources.book_add;
            this.dataGridViewImageColumn2.MinimumWidth = 20;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn2.Width = 391;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn3.FillWeight = 20F;
            this.dataGridViewImageColumn3.HeaderText = "";
            this.dataGridViewImageColumn3.Image = global::EpubSpellChecker.Properties.Resources.arrow_right_blue;
            this.dataGridViewImageColumn3.MinimumWidth = 20;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn3.ToolTipText = "Copy to fixed text";
            this.dataGridViewImageColumn3.Width = 20;
            // 
            // textDataGridViewTextBoxColumn
            // 
            this.textDataGridViewTextBoxColumn.DataPropertyName = "Text";
            this.textDataGridViewTextBoxColumn.FillWeight = 159.2809F;
            this.textDataGridViewTextBoxColumn.HeaderText = "Text";
            this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
            this.textDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // countDataGridViewTextBoxColumn
            // 
            this.countDataGridViewTextBoxColumn.DataPropertyName = "Count";
            this.countDataGridViewTextBoxColumn.FillWeight = 35.99567F;
            this.countDataGridViewTextBoxColumn.HeaderText = "Count";
            this.countDataGridViewTextBoxColumn.Name = "countDataGridViewTextBoxColumn";
            this.countDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Ignore
            // 
            this.Ignore.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Ignore.FillWeight = 20F;
            this.Ignore.HeaderText = "";
            this.Ignore.Image = global::EpubSpellChecker.Properties.Resources.ignore_icon_16px;
            this.Ignore.MinimumWidth = 20;
            this.Ignore.Name = "Ignore";
            this.Ignore.ReadOnly = true;
            this.Ignore.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Ignore.ToolTipText = "Ignore";
            this.Ignore.Width = 20;
            // 
            // AddToDictionary
            // 
            this.AddToDictionary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AddToDictionary.FillWeight = 20F;
            this.AddToDictionary.HeaderText = "";
            this.AddToDictionary.Image = global::EpubSpellChecker.Properties.Resources.book_add;
            this.AddToDictionary.MinimumWidth = 20;
            this.AddToDictionary.Name = "AddToDictionary";
            this.AddToDictionary.ReadOnly = true;
            this.AddToDictionary.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AddToDictionary.Width = 20;
            // 
            // Copy
            // 
            this.Copy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Copy.FillWeight = 20F;
            this.Copy.HeaderText = "";
            this.Copy.Image = global::EpubSpellChecker.Properties.Resources.arrow_right_blue;
            this.Copy.MinimumWidth = 20;
            this.Copy.Name = "Copy";
            this.Copy.ReadOnly = true;
            this.Copy.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Copy.ToolTipText = "Copy to fixed text";
            this.Copy.Width = 20;
            // 
            // wordEntryBindingSource
            // 
            this.wordEntryBindingSource.DataSource = typeof(EpubSpellChecker.WordEntry);
            // 
            // btnIgnoreLines
            // 
            this.btnIgnoreLines.Image = global::EpubSpellChecker.Properties.Resources.ignore_icon_16px;
            this.btnIgnoreLines.Location = new System.Drawing.Point(785, 3);
            this.btnIgnoreLines.Name = "btnIgnoreLines";
            this.btnIgnoreLines.Size = new System.Drawing.Size(20, 21);
            this.btnIgnoreLines.TabIndex = 4;
            this.btnIgnoreLines.UseVisualStyleBackColor = true;
            this.btnIgnoreLines.Click += new System.EventHandler(this.btnIgnoreLines_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 489);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Epub spell checker";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wordEntryBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.BindingSource wordEntryBindingSource;
        private System.Windows.Forms.ListBox lstOccurrences;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showOnlyErrorsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recheckOCRPatternsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblWordCount;
        private System.Windows.Forms.ToolStripStatusLabel lblUnknown;
        private System.Windows.Forms.ToolStripStatusLabel lblFixed;
        private System.Windows.Forms.ToolStripStatusLabel lblWarning;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progress;
        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn countDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Suggestion;
        private System.Windows.Forms.DataGridViewImageColumn Ignore;
        private System.Windows.Forms.DataGridViewImageColumn AddToDictionary;
        private System.Windows.Forms.DataGridViewImageColumn Copy;
        private CustomAutocompleteColumn FixedText;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.ToolStripMenuItem copyAllSuggestionsToFixedTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblSuggestion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Timer tmrFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnIgnoreLines;
        private System.Windows.Forms.ToolStripStatusLabel lblIgnored;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem wordDistributionAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

