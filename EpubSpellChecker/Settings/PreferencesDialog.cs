using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EpubSpellChecker
{
    public partial class PreferencesDialog : Form
    {
        public PreferencesDialog()
        {
            InitializeComponent();
            InitializeControls();
            FillForm();
        }

        private void InitializeControls()
        {
            var tests = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                                  .Where(t => t.GetInterfaces().Contains(typeof(ITest)))
                                                  .Select(t => new TestItem(t))
                                                  .ToArray();

            chklstTests.Items.AddRange(tests);
        }

        /// <summary>
        /// A test item in the checked listbox
        /// </summary>
        private class TestItem
        {
            public TestItem(Type t)
            {
                TestType = t;
                var attr = (DisplayNameAttribute)t.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
                if (attr != null)
                    Name = attr.DisplayName;
                else
                    Name = t.Name;
            }

            public Type TestType { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name + "";
            }
        }

        /// <summary>
        /// Fill in the form
        /// </summary>
        private void FillForm()
        {
            
            var settings = SettingsManager.GetSettings();

            var enabledTests = new HashSet<string>(settings.EnabledTests);
            for (int i = 0; i < chklstTests.Items.Count; i++)
            {
                var item = (chklstTests.Items[i] as TestItem);
                if (enabledTests.Contains(item.TestType.Name))
                    chklstTests.SetItemChecked(i, true);
            }
            chkOCRWarnings.Checked = settings.OCRWarnings;
            chkOnlyUseAppliedOCRPatterns.Checked = settings.OnlyUseAppliedOCRPatternsForWarnings;
        }

        /// <summary>
        /// Checks if the settings are valid
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Saves the settings if valid
        /// </summary>
        private void Save()
        {
            try
            {
                if (IsValid())
                {
                    var settings = SettingsManager.GetSettings();
                    var enabledItems = chklstTests.CheckedItems.Cast<TestItem>().Select(itm => itm.TestType.Name).ToArray();
                    settings.EnabledTests = enabledItems;
                    settings.OCRWarnings = chkOCRWarnings.Checked;
                    settings.OnlyUseAppliedOCRPatternsForWarnings = chkOnlyUseAppliedOCRPatterns.Checked;

                    SettingsManager.SaveSettings(settings);

                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving preferences: " + ex.GetType().FullName + " - " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// Occurs when an item is selected in the test checkedlistbox
        /// Shows the description of the test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chklstTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            var itm = chklstTests.SelectedItem as TestItem;
            if (itm != null)
            {
                string description = "";
                var attr = (DescriptionAttribute)itm.TestType.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                if (attr != null)
                    description = attr.Description;

                lblDescription.Text = description;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
