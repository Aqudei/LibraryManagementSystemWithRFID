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
                    using (var adptrbookcopies = new librarycontextDataSetTableAdapters.bookcopiesTableAdapter())
                    using (var adptrbookinfoes = new librarycontextDataSetTableAdapters.bookinfoesTableAdapter())
                    using (var adptrsections = new librarycontextDataSetTableAdapters.sectionsTableAdapter())
                    using (var adptrnumberofcopies = new librarycontextDataSetTableAdapters.numberofcopiesTableAdapter())
                    using (var adptrnumberoftimesborrowed = new librarycontextDataSetTableAdapters.numberoftimesborrowedTableAdapter())
                    {
                        adptrbookcopies.Fill(reportSource.bookcopies);
                        adptrbookinfoes.Fill(reportSource.bookinfoes);
                        adptrsections.Fill(reportSource.sections);
                        adptrnumberofcopies.Fill(reportSource.numberofcopies);
                        adptrnumberoftimesborrowed.Fill(reportSource.numberoftimesborrowed);
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
