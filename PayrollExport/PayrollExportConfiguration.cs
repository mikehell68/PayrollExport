using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LogThis;

namespace PayrollExport
{
    static public class PayrollExportConfiguration
    {
        static XDocument _payrollExportConfig;

        public static string ExportFileSaveLocation { get; set; }
        public static string ExportFileBackupLocation { get; set; }
        public static bool UseDecimalHourFormat { get; set; }
        public static bool ExportHourlyPaid { get; set; }
        public static bool ExportSalaryPaid { get; set; }
        public static DayOfWeek ExportEndDay { get; set; }
        public static bool UseExportEndDay { get; set; }
        public static bool UseSSN { get; set; }
        public static List<ExportPeriod> ExportPeriods { get; set; }
        public static int WorkedJobsPerRow { get; set; }
        public static bool ExportPeriodEnabled { get; set; }
        public static int DefaultExportPeriod { get; set; }

        public static XDocument PayrollExportConfig
        {
            get { return _payrollExportConfig; }
        }

        public static void LoadConfigFile()
        {
            Log.LogThis("Loading Configuration File",eloglevel.info);
            try
            {
                _payrollExportConfig = XDocument.Load(@".\Configuration\PayrollExportConfig.xml");

                ExportFileSaveLocation = _payrollExportConfig.Root.Element("ExportFileSaveLocation").Attribute("path").Value;
                ExportFileBackupLocation = _payrollExportConfig.Root.Element("ExportFileSaveLocation").Attribute("backupPath").Value;
                UseDecimalHourFormat = bool.Parse(_payrollExportConfig.Root.Element("HoursFormat").Attribute("decimal").Value);
                ExportHourlyPaid = bool.Parse(_payrollExportConfig.Root.Element("PaySchemes").Attribute("hourly").Value);
                ExportSalaryPaid = bool.Parse(_payrollExportConfig.Root.Element("PaySchemes").Attribute("salary").Value);
                WorkedJobsPerRow = int.Parse(_payrollExportConfig.Root.Element("WorkedJobsPerRow").Attribute("value").Value);
                ExportEndDay = DayOfWeek.Sunday;
                DayOfWeek exportEndDay;

                if (Enum.TryParse<DayOfWeek>(_payrollExportConfig.Root.Element("ExportEndDay").Attribute("day").Value, true, out exportEndDay))
                    ExportEndDay = exportEndDay;

                UseExportEndDay = bool.Parse(_payrollExportConfig.Root.Element("ExportEndDay").Attribute("enabled").Value);
                UseSSN = bool.Parse(_payrollExportConfig.Root.Element("OptionalOutput").Attribute("SSN").Value);
                ExportPeriodEnabled = bool.Parse(_payrollExportConfig.Root.Element("ExportPeriods").Attribute("enabled").Value);
                DefaultExportPeriod = int.Parse(_payrollExportConfig.Root.Element("ExportPeriods").Attribute("defaultPeriodIndex").Value);

                ExportPeriods = new List<ExportPeriod>();

                if (!ExportPeriodEnabled)
                {
                    var defaultExportPeriod = _payrollExportConfig.Root.Element("ExportPeriods").Descendants("ExportPeriod").Where(e => e.Attribute("index").Value == DefaultExportPeriod.ToString()).FirstOrDefault();

                    ExportPeriodEnum defaultExportPeriodEnum;

                    if (!Enum.TryParse(defaultExportPeriod.Attribute("index").Value, out defaultExportPeriodEnum))
                        defaultExportPeriodEnum = ExportPeriodEnum.OneWeek;

                    ExportPeriods.Add(new ExportPeriod
                                          {
                                              PeriodIndex = defaultExportPeriodEnum,
                                              DisplayName = defaultExportPeriod.Attribute("displayName").Value,
                                              Visible = true
                                          });
                }
                else
                {
                    foreach (var exportPeriod in _payrollExportConfig.Root.Element("ExportPeriods").Descendants("ExportPeriod"))
                    {
                        ExportPeriodEnum periodIndex;

                        if (!Enum.TryParse(exportPeriod.Attribute("index").Value, out periodIndex))
                            continue;

                        ExportPeriods.Add(new ExportPeriod
                                              {
                                                  PeriodIndex = periodIndex,
                                                  DisplayName = exportPeriod.Attribute("displayName").Value,
                                                  Visible = bool.Parse(exportPeriod.Attribute("visible").Value)
                                              });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogThis(ex.StackTrace, eloglevel.warn);
                Log.LogThis("Unable to load configuration file, using default values:", eloglevel.warn);
                UseDefaultConfigurationValues();
            }

            Log.LogThis(ToString(), eloglevel.info);
        }

        static void UseDefaultConfigurationValues()
        {
            ExportFileSaveLocation = @".\Export";
            ExportFileBackupLocation = @".\Export\Backup";
            UseDecimalHourFormat = true;
            ExportHourlyPaid = true;
            ExportSalaryPaid = false;
            ExportEndDay = DayOfWeek.Sunday;
            UseExportEndDay = true;
            UseSSN = false;
            ExportPeriodEnabled = false;
            DefaultExportPeriod = 0;
        }

        public new static string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine();
            str.AppendLine("Payroll Export Configuration");
            str.AppendLine("ExportFileSaveLocation: " + ExportFileSaveLocation);
            str.AppendLine("ExportFileBackupLocation: " + ExportFileBackupLocation);
            str.AppendLine("UseDecimalHourFormat: " + UseDecimalHourFormat);
            str.AppendLine("ExportHourlyPaid: " + ExportHourlyPaid);
            str.AppendLine("ExportSalaryPaid: " + ExportSalaryPaid);
            str.AppendLine("ExportEndDay: " + ExportEndDay);
            str.AppendLine("UseExportEndDay: " + UseExportEndDay);
            str.AppendLine("UseSSN: " + UseSSN);
            str.AppendLine("WorkedJobsPerRow: " + WorkedJobsPerRow);
            str.AppendLine("ExportPeriodEnabled: " + ExportPeriodEnabled);
            str.AppendLine("DefaultExportPeriod: " + DefaultExportPeriod);

            return str.ToString();
        }
    }

    public class ExportPeriod
    {
        public ExportPeriodEnum PeriodIndex { get; set; }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
    }
}
