using System;
using System.Windows.Forms;
using LogThis;

namespace PayrollExport
{
    public partial class _payrollExportMainForm : Form
    {
        PayrollExportModel _payrollExportModel = new PayrollExportModel();

        public _payrollExportMainForm()
        {
            InitializeComponent();

            PopulateExportPeriodCombobox();
            _payrollExportModel.SetExportPeriodEndDate();
            _endDate.DataBindings.Add(new Binding("Value", _payrollExportModel, "PeriodEndDate", false, DataSourceUpdateMode.OnPropertyChanged));
            _endDate.DataBindings.Add(new Binding("MaxDate", _payrollExportModel, "MaxEndDate", false, DataSourceUpdateMode.OnPropertyChanged));
            _endDate.DataBindings.Add(new Binding("MinDate", _payrollExportModel, "MinEndDate", false, DataSourceUpdateMode.OnPropertyChanged));

            _startDate.DataBindings.Add(new Binding("Value", _payrollExportModel, "PeriodStartDate", false, DataSourceUpdateMode.OnPropertyChanged));
            _startDate.DataBindings.Add(new Binding("Enabled", _payrollExportModel, "PeriodStartDateEnabled", false, DataSourceUpdateMode.OnPropertyChanged));
            _startDate.DataBindings.Add(new Binding("MaxDate", _payrollExportModel, "MaxStartDate", false, DataSourceUpdateMode.OnPropertyChanged));

            _exportPeriodComboBox.DataBindings.Add(new Binding("SelectedItem", _payrollExportModel, "SelectedExportPeriod", false, DataSourceUpdateMode.OnPropertyChanged));

        }

        void PopulateExportPeriodCombobox()
        {
            _exportPeriodComboBox.DisplayMember = "DisplayName";
            _exportPeriodComboBox.ValueMember = "PeriodIndex";
            _exportPeriodComboBox.Items.AddRange(_payrollExportModel.GetExportPeriods().ToArray());
            _exportPeriodComboBox.SelectedIndex = 0;
        }

        void _closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void _exportPeriodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_exportPeriodComboBox.DataBindings["SelectedItem"] != null)
                _exportPeriodComboBox.DataBindings["SelectedItem"].WriteValue();
        }
        
        void _doExportPayrollButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this,
                            string.Format(
                                "Have all times for the payroll export period been verified?\n\nThe Payroll Export will create an export file for the following period:\nStart Date: {0}\nEnd Date: {1}\nDuration: {2} days",
                                _startDate.Value.ToString("dddd dd MMMM yyyy"), _endDate.Value.ToString("dddd dd MMMM yyyy"), (_endDate.Value.Date - _startDate.Value.Date).Days + 1), "Payroll Export",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

           Refresh();
           Cursor.Current = Cursors.WaitCursor;
           bool exportComplete = false;

           try
           {
               if (result == DialogResult.OK)
               {
                   _doExportPayrollButton.Enabled = false;
                   Log.LogThis("User confirmed all payroll times are verified", eloglevel.info);
                   exportComplete = _payrollExportModel.DoExport(_startDate.Value.Date, _endDate.Value.Date);

                   if(!exportComplete)
                   {
                       MessageBox.Show(this, "No payroll data for the dates provided", "Payroll Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }

               }
           }
           catch(Exception ex)
           {
               Log.LogThis("An exception occurred generating the payroll export.", eloglevel.error);
               Log.LogThis(string.Format("EXCEPTION: {0}", ex), eloglevel.error);
           }
           finally
           {
               Cursor.Current = Cursors.Default;
               if (exportComplete)
               {
                   MessageBox.Show(this, "Export complete", "Payroll export", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
               }
               _doExportPayrollButton.Enabled = true;
           }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _closeButton_Click(sender, e);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _doExportPayrollButton_Click(sender, e);
        }
    }
}
