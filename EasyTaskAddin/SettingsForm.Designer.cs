namespace EasyTaskAddin
{
    partial class SettingsForm
    {
        public SettingsForm()
        {
            InitializeComponent();
        }
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dueDateParserTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.categoryParserTab = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbxProcessChangedTask = new System.Windows.Forms.CheckBox();
            this.chbxCreateNewCategory = new System.Windows.Forms.CheckBox();
            this.chbxRemoveCategoryText = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbCustomCategoryPattern = new System.Windows.Forms.TextBox();
            this.rbtnCustomCategoryPattern = new System.Windows.Forms.RadioButton();
            this.rbtnUseArrowCategoryPattern = new System.Windows.Forms.RadioButton();
            this.rbtnUseBrackets = new System.Windows.Forms.RadioButton();
            this.rbtnStandardCategory = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dueDateParserTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.categoryParserTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(635, 346);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(545, 346);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // dueDateParserTab
            // 
            this.dueDateParserTab.Controls.Add(this.groupBox3);
            this.dueDateParserTab.Location = new System.Drawing.Point(4, 22);
            this.dueDateParserTab.Name = "dueDateParserTab";
            this.dueDateParserTab.Padding = new System.Windows.Forms.Padding(3);
            this.dueDateParserTab.Size = new System.Drawing.Size(697, 302);
            this.dueDateParserTab.TabIndex = 1;
            this.dueDateParserTab.Text = "Due Date";
            this.dueDateParserTab.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(683, 43);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = global::EasyTaskAddin.Properties.Settings.Default.RemoveDueDateString;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyTaskAddin.Properties.Settings.Default, "RemoveDueDateString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(7, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(188, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Remove date text after processing";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // categoryParserTab
            // 
            this.categoryParserTab.Controls.Add(this.groupBox2);
            this.categoryParserTab.Controls.Add(this.groupBox1);
            this.categoryParserTab.Location = new System.Drawing.Point(4, 22);
            this.categoryParserTab.Name = "categoryParserTab";
            this.categoryParserTab.Padding = new System.Windows.Forms.Padding(3);
            this.categoryParserTab.Size = new System.Drawing.Size(697, 302);
            this.categoryParserTab.TabIndex = 0;
            this.categoryParserTab.Text = "Category";
            this.categoryParserTab.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbxProcessChangedTask);
            this.groupBox2.Controls.Add(this.chbxCreateNewCategory);
            this.groupBox2.Controls.Add(this.chbxRemoveCategoryText);
            this.groupBox2.Location = new System.Drawing.Point(7, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General";
            // 
            // chbxProcessChangedTask
            // 
            this.chbxProcessChangedTask.AutoSize = true;
            this.chbxProcessChangedTask.Checked = global::EasyTaskAddin.Properties.Settings.Default.ProcessChangedTask;
            this.chbxProcessChangedTask.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbxProcessChangedTask.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyTaskAddin.Properties.Settings.Default, "ProcessChangedTask", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxProcessChangedTask.Location = new System.Drawing.Point(7, 19);
            this.chbxProcessChangedTask.Name = "chbxProcessChangedTask";
            this.chbxProcessChangedTask.Size = new System.Drawing.Size(242, 17);
            this.chbxProcessChangedTask.TabIndex = 2;
            this.chbxProcessChangedTask.Text = "Process task when you change its description";
            this.chbxProcessChangedTask.UseVisualStyleBackColor = true;
            // 
            // chbxCreateNewCategory
            // 
            this.chbxCreateNewCategory.AutoSize = true;
            this.chbxCreateNewCategory.Checked = global::EasyTaskAddin.Properties.Settings.Default.CreateNewCategoryIfNotExists;
            this.chbxCreateNewCategory.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyTaskAddin.Properties.Settings.Default, "CreateNewCategoryIfNotExists", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxCreateNewCategory.Location = new System.Drawing.Point(7, 65);
            this.chbxCreateNewCategory.Name = "chbxCreateNewCategory";
            this.chbxCreateNewCategory.Size = new System.Drawing.Size(201, 17);
            this.chbxCreateNewCategory.TabIndex = 1;
            this.chbxCreateNewCategory.Text = "Create new category if it doesn\'t exist";
            this.toolTip1.SetToolTip(this.chbxCreateNewCategory, "This option hasn\'t been implemented yet");
            this.chbxCreateNewCategory.UseVisualStyleBackColor = true;
            // 
            // chbxRemoveCategoryText
            // 
            this.chbxRemoveCategoryText.AutoSize = true;
            this.chbxRemoveCategoryText.Checked = global::EasyTaskAddin.Properties.Settings.Default.RemoveCategoryName;
            this.chbxRemoveCategoryText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbxRemoveCategoryText.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::EasyTaskAddin.Properties.Settings.Default, "RemoveCategoryName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chbxRemoveCategoryText.Location = new System.Drawing.Point(7, 42);
            this.chbxRemoveCategoryText.Name = "chbxRemoveCategoryText";
            this.chbxRemoveCategoryText.Size = new System.Drawing.Size(208, 17);
            this.chbxRemoveCategoryText.TabIndex = 0;
            this.chbxRemoveCategoryText.Text = "Remove category text after processing";
            this.chbxRemoveCategoryText.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbCustomCategoryPattern);
            this.groupBox1.Controls.Add(this.rbtnCustomCategoryPattern);
            this.groupBox1.Controls.Add(this.rbtnUseArrowCategoryPattern);
            this.groupBox1.Controls.Add(this.rbtnUseBrackets);
            this.groupBox1.Controls.Add(this.rbtnStandardCategory);
            this.groupBox1.Location = new System.Drawing.Point(7, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 116);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keyword pattern";
            // 
            // tbCustomCategoryPattern
            // 
            this.tbCustomCategoryPattern.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::EasyTaskAddin.Properties.Settings.Default, "CustomCategoryPattern", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbCustomCategoryPattern.Enabled = global::EasyTaskAddin.Properties.Settings.Default.UseCustomCategoryPattern;
            this.tbCustomCategoryPattern.Location = new System.Drawing.Point(184, 87);
            this.tbCustomCategoryPattern.Name = "tbCustomCategoryPattern";
            this.tbCustomCategoryPattern.Size = new System.Drawing.Size(281, 20);
            this.tbCustomCategoryPattern.TabIndex = 4;
            this.tbCustomCategoryPattern.Text = global::EasyTaskAddin.Properties.Settings.Default.CustomCategoryPattern;
            // 
            // rbtnCustomCategoryPattern
            // 
            this.rbtnCustomCategoryPattern.AutoSize = true;
            this.rbtnCustomCategoryPattern.Checked = global::EasyTaskAddin.Properties.Settings.Default.UseCustomCategoryPattern;
            this.rbtnCustomCategoryPattern.Location = new System.Drawing.Point(7, 88);
            this.rbtnCustomCategoryPattern.Name = "rbtnCustomCategoryPattern";
            this.rbtnCustomCategoryPattern.Size = new System.Drawing.Size(155, 17);
            this.rbtnCustomCategoryPattern.TabIndex = 3;
            this.rbtnCustomCategoryPattern.Text = "Use custom RegEx pattern:";
            this.rbtnCustomCategoryPattern.UseVisualStyleBackColor = true;
            this.rbtnCustomCategoryPattern.CheckedChanged += new System.EventHandler(this.rbtnCustomCategoryPattern_CheckedChanged);
            // 
            // rbtnUseArrowCategoryPattern
            // 
            this.rbtnUseArrowCategoryPattern.AutoSize = true;
            this.rbtnUseArrowCategoryPattern.Checked = global::EasyTaskAddin.Properties.Settings.Default.UseArrowCategoryPattern;
            this.rbtnUseArrowCategoryPattern.Location = new System.Drawing.Point(7, 65);
            this.rbtnUseArrowCategoryPattern.Name = "rbtnUseArrowCategoryPattern";
            this.rbtnUseArrowCategoryPattern.Size = new System.Drawing.Size(373, 17);
            this.rbtnUseArrowCategoryPattern.TabIndex = 2;
            this.rbtnUseArrowCategoryPattern.Text = "Use \">\" to define category. Example: \"Category Name> Task description\"";
            this.rbtnUseArrowCategoryPattern.UseVisualStyleBackColor = true;
            // 
            // rbtnUseBrackets
            // 
            this.rbtnUseBrackets.AutoSize = true;
            this.rbtnUseBrackets.Checked = global::EasyTaskAddin.Properties.Settings.Default.UseBracketCategoryPattern;
            this.rbtnUseBrackets.Location = new System.Drawing.Point(7, 42);
            this.rbtnUseBrackets.Name = "rbtnUseBrackets";
            this.rbtnUseBrackets.Size = new System.Drawing.Size(376, 17);
            this.rbtnUseBrackets.TabIndex = 1;
            this.rbtnUseBrackets.TabStop = true;
            this.rbtnUseBrackets.Text = "Use \"[ ]\" to define category. Example: \"[Category Name] Task description\"";
            this.rbtnUseBrackets.UseVisualStyleBackColor = true;
            // 
            // rbtnStandardCategory
            // 
            this.rbtnStandardCategory.AutoSize = true;
            this.rbtnStandardCategory.Checked = global::EasyTaskAddin.Properties.Settings.Default.UseKeywordCategoryPattern;
            this.rbtnStandardCategory.Location = new System.Drawing.Point(7, 19);
            this.rbtnStandardCategory.Name = "rbtnStandardCategory";
            this.rbtnStandardCategory.Size = new System.Drawing.Size(411, 17);
            this.rbtnStandardCategory.TabIndex = 0;
            this.rbtnStandardCategory.Text = "Use \"category:()\" keyword. Example: \"Task description category:(Category Name)";
            this.rbtnStandardCategory.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.categoryParserTab);
            this.tabControl1.Controls.Add(this.dueDateParserTab);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(705, 328);
            this.tabControl1.TabIndex = 2;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(731, 381);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Easy Task Settings (v 0.3)";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.dueDateParserTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.categoryParserTab.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabPage dueDateParserTab;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TabPage categoryParserTab;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbxCreateNewCategory;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chbxRemoveCategoryText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbCustomCategoryPattern;
        private System.Windows.Forms.RadioButton rbtnCustomCategoryPattern;
        private System.Windows.Forms.RadioButton rbtnUseArrowCategoryPattern;
        private System.Windows.Forms.RadioButton rbtnUseBrackets;
        private System.Windows.Forms.RadioButton rbtnStandardCategory;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox chbxProcessChangedTask;
    }
}