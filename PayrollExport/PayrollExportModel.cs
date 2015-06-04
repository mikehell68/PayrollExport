using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using LogThis;
using PayrollExport.BusinessServices;

namespace PayrollExport
{
    public class PayrollExportModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        
        public PayrollExportModel()
        {
            PeriodEndDate = DateTime.Now;
            PayrollExportConfiguration.LoadConfigFile();
            SelectedExportPeriod = PayrollExportConfiguration.ExportPeriods.FirstOrDefault(p => p.PeriodIndex == PayrollExportConfiguration.DefaultExportPeriod) ??
                                   new ExportPeriod
                                       {
                                           DisplayName = "1 Week", 
                                           PeriodIndex = ExportPeriodEnum.OneWeek, 
                                           Visible = true
                                       };
        }

        ExportPeriod _selectedExportPeriod;
        public ExportPeriod SelectedExportPeriod
        {
            get { return _selectedExportPeriod; }
            set
            {
                SetField(ref _selectedExportPeriod, value, "SelectedExportPeriod");
                SetExportPeriodStartDate();
                SetExportStartEndDateRange();
            }
        }

        DateTime _periodEndDate;
        public DateTime PeriodEndDate
        {
            get { return _periodEndDate; }
            set
            {
                SetField(ref _periodEndDate, value, "PeriodEndDate");
                SetExportPeriodStartDate();
                SetExportStartEndDateRange();
            }
        }

        DateTime _periodStartDate;
        public DateTime PeriodStartDate
        {
            get { return _periodStartDate; }
            set
            {
                SetField(ref _periodStartDate, value, "PeriodStartDate");
                SetExportStartEndDateRange();
            }
        }

        bool _periodStartDateEnabled;
        public bool PeriodStartDateEnabled
        {
            get { return _periodStartDateEnabled; }
            set { SetField(ref _periodStartDateEnabled, value, "PeriodStartDateEnabled"); }
        }

        DateTime _minEndDate;
        public DateTime MinEndDate
        {
            get { return _minEndDate; }
            set { SetField(ref _minEndDate, value, "MinEndDate"); }
        }

        DateTime _maxEndDate;
        public DateTime MaxEndDate
        {
            get { return _maxEndDate; }
            set { SetField(ref _maxEndDate, value, "MaxEndDate"); }
        }

        DateTime _maxStartDate;
        public DateTime MaxStartDate
        {
            get { return _maxStartDate; }
            set { SetField(ref _maxStartDate, value, "MaxStartDate"); }
        }

        public bool ExportPeriodEnabled
        {
            get { return PayrollExportConfiguration.ExportPeriodEnabled; }
        }

        public bool DoExport(DateTime startDate, DateTime endDate)
        {
            Log.LogThis("Begin DoExort()", eloglevel.info);
            Log.LogThis(string.Format("Generating payroll export for {0} to {1}", startDate, endDate), eloglevel.info);

            var siteRef = "";
            var sb = new StringBuilder(); 
            var payrollResultSet = AztecBusinessService.GetPayrollDataFromAztec(startDate, endDate);

            if (payrollResultSet == null)
            {
                Log.LogThis("No payroll data to export.", eloglevel.info);
                Log.LogThis("End DoExort()", eloglevel.info);
                return false;
            }

            string fileHeaderRow = "SiteName,SiteRef,StartDate,EndDate";

            sb.AppendLine(fileHeaderRow);
            siteRef = (string)payrollResultSet.Tables[0].Rows[0]["SiteRef"];

            sb.AppendLine( QuoteIfCommaExists((string) payrollResultSet.Tables[0].Rows[0]["SiteName"]) + "," +
                          QuoteIfCommaExists(siteRef) + "," + 
                          startDate.ToShortDateString() + "," + 
                          endDate.ToShortDateString());

            string payrollHeaderRow = "LastName,FirstName,EmpRef,NetSales,ChargedSales,ChargedTips,DeclaredTips";
            //"LastName,FirstName,EmpRef,NetSales,ChargedSales,ChargedTips,DeclaredTips,JobName1,JobCode1,PayType1,RegHours1,RegRate1,OTHours1,OTRate1,JobName2,JobCode2,PayType2,RegHours2,RegRate2,OTHours2,OTRate2,JobName3,JobCode3,PayType3,RegHours3,RegRate3,OTHours3,OTRate3,JobName4,JobCode4,PayType4,RegHours4,RegRate4,OTHours4,OTRate4";

            string workedJobsHeaderFormatStr = ",JobName{0},JobCode{0},PayType{0},RegHours{0},RegRate{0},OTHours{0},OTRate{0}";
            string workedJobsHeaderStr = "";

            for (int i = 1; i <= PayrollExportConfiguration.WorkedJobsPerRow; i++)
            {
                workedJobsHeaderStr += string.Format(workedJobsHeaderFormatStr, i);
            }

            sb.AppendLine(payrollHeaderRow + workedJobsHeaderStr);

            long currentUserId = -1;
            long currentRoleId = -1;
            int workedJobCount = 0;
            
            var userDetailsCsv = "";
            var jobsDetailsCsv = "";

            decimal netSalesTotal = 0;
            decimal chargedSalesTotal = 0;
            decimal chargeTipsTotal = 0;
            decimal declaredTipsTotal = 0;

            foreach (DataRow row in payrollResultSet.Tables[0].Rows)
            {
                if (currentUserId != (long)row["UserId"])
                {
                    if (workedJobCount != 0 && workedJobCount < PayrollExportConfiguration.WorkedJobsPerRow)
                    {
                        for (int i = workedJobCount; i <= PayrollExportConfiguration.WorkedJobsPerRow; i++)
                            jobsDetailsCsv += ",,,,,,,";

                        var totalsCsv = string.Format("{0},{1},{2},{3}", netSalesTotal, chargedSalesTotal, chargeTipsTotal, declaredTipsTotal);
                        sb.AppendLine(userDetailsCsv + totalsCsv + jobsDetailsCsv);

                        netSalesTotal = 0;
                        chargedSalesTotal = 0;
                        chargeTipsTotal = 0;
                        declaredTipsTotal = 0;
                    }

                    userDetailsCsv = QuoteIfCommaExists((string) row["LastName"]) + "," +
                                     QuoteIfCommaExists((string) row["FirstName"]) + "," +
                                     row["UserId"] + ",";

                   
                    netSalesTotal += (decimal)row["NetSales"];
                    chargedSalesTotal += (decimal)row["NetSales"];
                    chargeTipsTotal += (decimal)row["ChargeTips"];
                    declaredTipsTotal += (decimal)row["DeclaredTips"];

                    jobsDetailsCsv = "," + 
                                         QuoteIfCommaExists((string)row["RoleName"]) + "," + 
                                         row["RoleId"] + "," + 
                                         row["RolePayType"] + "," +
                                         row["StandardHours"] + "," + 
                                         row["StandardRate"] + "," + 
                                         row["OverTimeHours"] + "," +
                                         row["OverTimeRate"];

                    currentUserId = (long) row["UserId"];
                    currentRoleId = (int) row["RoleId"];
                    workedJobCount = 1;
                }
                else if (currentUserId == (long)row["UserId"] && currentRoleId != (int)row["RoleId"] && workedJobCount <= PayrollExportConfiguration.WorkedJobsPerRow)
                {
                    jobsDetailsCsv += "," + 
                                         QuoteIfCommaExists((string)row["RoleName"]) + "," + 
                                         row["RoleId"] + "," + row["RolePayType"] + "," +
                                         row["StandardHours"] + "," + 
                                         row["StandardRate"] + "," + 
                                         row["OverTimeHours"] + "," +
                                         row["OverTimeRate"];

                    netSalesTotal += (decimal)row["NetSales"];
                    chargedSalesTotal += (decimal)row["NetSales"];
                    chargeTipsTotal += (decimal)row["ChargeTips"];
                    declaredTipsTotal += (decimal)row["DeclaredTips"];

                    workedJobCount++;
                }
            }
            
            if(!Directory.Exists(PayrollExportConfiguration.ExportFileSaveLocation))
                Directory.CreateDirectory(PayrollExportConfiguration.ExportFileSaveLocation);

            //if(!Directory.Exists(@"PayrollExportFiles\Export"))
            //{
            //    Directory.CreateDirectory(@"PayrollExportFiles\Export");
            //}

            if (!Directory.Exists(PayrollExportConfiguration.ExportFileBackupLocation))
            {
                Directory.CreateDirectory(PayrollExportConfiguration.ExportFileBackupLocation);
            }

            foreach (var file in Directory.EnumerateFiles(PayrollExportConfiguration.ExportFileSaveLocation))
            {
                File.Delete(file);
            }

            var filename = string.Format("{0}_{1}_{2}.csv", siteRef, startDate.ToString("yyyyMMdd"),
                                         endDate.ToString("yyyyMMdd"));

            using (FileStream fs = File.Create(PayrollExportConfiguration.ExportFileSaveLocation + "\\" + filename))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

            File.Copy(PayrollExportConfiguration.ExportFileSaveLocation + "\\" + filename, PayrollExportConfiguration.ExportFileBackupLocation+"\\" + filename + "."+DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return true;
        }

        public List<ExportPeriod> GetExportPeriods()
        {
            return PayrollExportConfiguration.ExportPeriods.Where(p => p.Visible).ToList();
        }

        public void SetExportPeriodEndDate()
        {
            PeriodEndDate = DateTime.Now;

            if (PayrollExportConfiguration.UseExportEndDay)
            {
                while (PeriodEndDate.DayOfWeek > PayrollExportConfiguration.ExportEndDay)
                {
                    PeriodEndDate = PeriodEndDate.AddDays(-1);
                }
            }
        }

        public void SetExportPeriodStartDate()
        {
            if (SelectedExportPeriod == null)
                return;

            PeriodStartDateEnabled = false;
            
            switch (SelectedExportPeriod.PeriodIndex)
            {
                case ExportPeriodEnum.OneWeek:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(1));
                    break;
                case ExportPeriodEnum.TwoWeeks:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(2));
                    break;
                case ExportPeriodEnum.ThreeWeeks:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(3));
                    break;
                case ExportPeriodEnum.OneMonth:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(4));
                    break;
                case ExportPeriodEnum.TwoMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(8));
                    break;
                case ExportPeriodEnum.ThreeMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(12));
                    break;
                case ExportPeriodEnum.FourMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(16));
                    break;
                case ExportPeriodEnum.FiveMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(20));
                    break;
                case ExportPeriodEnum.SixMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(24));
                    break;
                case ExportPeriodEnum.SevenMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(28));
                    break;
                case ExportPeriodEnum.EightMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(32));
                    break;
                case ExportPeriodEnum.NineMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(36));
                    break;
                case ExportPeriodEnum.TenMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(40));
                    break;
                case ExportPeriodEnum.ElevenMonths:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(44));
                    break;
                case ExportPeriodEnum.OneYear:
                    PeriodStartDate = PeriodEndDate.AddDays(GetDaysForPeriod(53));
                    break;
                case ExportPeriodEnum.Custom:
                default:
                    PeriodStartDateEnabled = true;
                    PeriodStartDate = PeriodEndDate;
                    break;
            }
        }

        int GetDaysForPeriod(int weekMultipier)
        {
            return -(weekMultipier * 7 - 1);
        }

        void SetExportStartEndDateRange()
        {
            MaxEndDate = DateTime.Now;
            MaxStartDate = MaxEndDate;
        }

        string QuoteIfCommaExists(string value)
        {
            if (value.Contains(","))
                return string.Format("\"{0}\"", value);

            return value;
        }
    }

}
