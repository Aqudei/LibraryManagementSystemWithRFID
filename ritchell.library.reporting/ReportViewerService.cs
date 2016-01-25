using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ritchell.library.reporting.Reports;

namespace ritchell.library.reporting
{
    public class ReportViewerService
    {
        librarycontextDataSet reportSource;

        public ReportViewerService()
        {
            reportSource = new librarycontextDataSet();
            reportSource.DataSetName = "librarycontext";
        }

        private Reports.BooksReport booksReport;

        public BooksReport BooksReport
        {
            get
            {
                if (booksReport == null)
                {
                    using (var adptr = new librarycontextDataSetTableAdapters.bookcopiesTableAdapter())
                    using (var adptr1 = new librarycontextDataSetTableAdapters.bookinfoesTableAdapter())
                    using (var adptr2 = new librarycontextDataSetTableAdapters.sectionsTableAdapter())
                    {
                        adptr.Fill(reportSource.bookcopies);
                        adptr1.Fill(reportSource.bookinfoes);
                        adptr2.Fill(reportSource.sections);
                    }

                    booksReport = new BooksReport();
                    booksReport.SetDataSource(reportSource);
                }

                return booksReport;
            }
        }

        public void ShowBookListReport()
        {
            var reportForm = new MainWindow();
            reportForm.ReportViewer.ViewerCore.ReportSource = BooksReport;
            reportForm.ShowDialog();
        }

        public void ShowForClearanceReport()
        {
            var reportForm = new MainWindow();
            reportForm.ShowDialog();
        }

        public void ShowPatronsReport()
        {
            var reportForm = new MainWindow();
            reportForm.ShowDialog();
        }
    }
}
