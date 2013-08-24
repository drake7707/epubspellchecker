using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace EpubSpellChecker
{

    public class CustomAutocompleteColumn : DataGridViewColumn
    {
        
        public CustomAutocompleteColumn()
            : base(new AutocompleteCell())
        {
            
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(AutocompleteCell)))
                {
                    throw new InvalidCastException("Must be a Autocomplete");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class AutocompleteCell : DataGridViewTextBoxCell
    {
        private int contextID;
        public AutocompleteCell()
            : base()
        {
            this.contextID = Thread.CurrentContext.ContextID;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {

            
            // Set the value of the editing control to the current cell value. 
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            AutocompleteTextEditingControl ctl =
                DataGridView.EditingControl as AutocompleteTextEditingControl;
            // Use the default row value when Value property is null. 

            if (Thread.CurrentContext.ContextID != contextID)
            {
                ctl.BeginInvoke(new Action(() =>
                {
                    if (this.Value == null)
                        ctl.Text = (string)this.DefaultNewRowValue;
                    else
                        ctl.Text = (string)this.Value;
                }));
            }
            else
            {
                if (this.Value == null)
                    ctl.Text = (string)this.DefaultNewRowValue;
                else
                    ctl.Text = (string)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses. 
                return typeof(AutocompleteTextEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains. 

                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value. 
                return "";
            }
        }
    }

    class AutocompleteTextEditingControl : ComboBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public AutocompleteTextEditingControl()
        {
            this.DropDownStyle = ComboBoxStyle.DropDown;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue  
        // property. 
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                if (value is String)
                {
                    this.Text = (string)value;
                }
            }
        }

        // Implements the  
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method. 
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the  
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method. 
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex  
        // property. 
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey  
        // method. 
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed. 
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                case Keys.Escape:
                    if (DroppedDown)
                        return true;
                    else
                        return !dataGridViewWantsInputKey;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit  
        // method. 
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
            this.DroppedDown = true;
        }

        // Implements the IDataGridViewEditingControl 
        // .RepositionEditingControlOnValueChange property. 
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlDataGridView property. 
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlValueChanged property. 
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingPanelCursor property. 
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Notify the DataGridView that the contents of the cell 
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }

    }
}
