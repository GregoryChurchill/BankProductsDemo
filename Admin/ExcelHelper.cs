using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Admin
{
    public class ExcelHelper
    {
        private Microsoft.Office.Interop.Excel.Application _appExcel;
        private Workbook _workbook;
        private _Worksheet _worksheet;

        public void Initialize(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            _appExcel = new Microsoft.Office.Interop.Excel.Application();

            if (!File.Exists(path)) return;

            _appExcel.DisplayAlerts = false;
            _workbook = _appExcel.Workbooks.Open(path, ReadOnly: false);
            _worksheet = (_Worksheet)_appExcel.ActiveWorkbook.ActiveSheet;
        }

        public void RunMacro(string macroName)
        {
            _appExcel.Run(macroName);
        }

        public void Close()
        {
            if (_appExcel == null) return;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Marshal.FinalReleaseComObject(_worksheet);

            _workbook.Close(Type.Missing, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(_workbook);

            _appExcel.Quit();
            Marshal.FinalReleaseComObject(_appExcel);
        }

        public void KillProcess(string fileName)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL") select p;

            foreach (var process in processes.Where(process => process.MainWindowTitle == ""))
            {
                process.Kill();
            }
        }

        public void SaveAsXml(string fileName)
        {
            var mapToExport = _workbook.XmlMaps["Submission_Map"];

            if (mapToExport.IsExportable)
            {
                _workbook.SaveAsXMLData(fileName, mapToExport);
            }
        }

        public void Save()
        {
            _workbook.Save();
        }

        public void Show()
        {
            _appExcel.Visible = true;
        }

        public void SetCellValue(string worksheetName, string cellLocation, object cellValue)
        {
            _worksheet = (_Worksheet)_workbook.Worksheets[worksheetName];

            _worksheet.Range[cellLocation].Value = cellValue;
        }

        public void SetCellFocus(string worksheetName, string cellLocation)
        {
            _worksheet = (_Worksheet)_workbook.Worksheets[worksheetName];

            _worksheet.Range[cellLocation].Select();
        }

        public string GetCellValue(string worksheetName, string cellLocation)
        {
            object value;

            try
            {
                _worksheet = (_Worksheet)_workbook.Worksheets[worksheetName];

                value = _worksheet.get_Range(cellLocation).get_Value();
            }
            catch (Exception ex)
            {
                throw new ValidationException("Unable to retrieve cell location \"" + cellLocation + "\" from \"" + _workbook.Name + "." + worksheetName + "\"" + Environment.NewLine + Environment.NewLine + ex.Message);
            }

            return value == null ? "" : value.ToString();
        }

        public string GetCellRangeSum(string worksheetName, string sourceCellFrom, string sourceCellTo)
        {
            object sum;

            try
            {
                _worksheet = (_Worksheet)_workbook.Worksheets[worksheetName];
                _worksheet.Range["A1"].Formula = "=SUM(" + sourceCellFrom + ":" + sourceCellTo + ")";
                _worksheet.Range["A1"].Calculate();
                sum = _worksheet.get_Range("A1").get_Value();
            }
            catch (Exception ex)
            {
                throw new ValidationException("Unable to retrieve range \"" + sourceCellFrom + ":" + sourceCellTo + "\" from \"" + _workbook.Name + "." + worksheetName + "\"" + Environment.NewLine + Environment.NewLine + ex.Message);
            }

            return sum == null ? "" : sum.ToString();
        }

        public bool SheetExists(string worksheetName)
        {
            try
            {
                return _workbook.Worksheets.Cast<object>().Any(worksheet => ((Worksheet)worksheet).Name == worksheetName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
