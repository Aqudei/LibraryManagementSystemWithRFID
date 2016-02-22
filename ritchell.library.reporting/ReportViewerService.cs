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
                    {
                        adptrbookcopies.Fill(reportSource.bookcopies);
                        adptrbookinfoes.Fill(reportSource.bookinfoes);
                        adptrsections.Fill(reportSource.sections);
                        adptrnumberofcopies.Fill(reportSource.numberofcopies);
                    }

                    booksReport = new BooksReport();
                    booksReport.SetDataSource(reportSource);
                }

                return booksReport;
            }
        }


        public ForClearance ForClearance
        {
            get
            {
                using (var adptrcopies = new librarycontextDataSetTableAdapters.bookcopiesTableAdapter())
                using (var adptrusers = new librarycontextDataSetTableAdapters.libraryusersTableAdapter())
                using (var adptrbookinfoes = new librarycontextDataSetTableAdapters.bookinfoesTableAdapter())
                using (var adptrtrans = new librarycontextDataSetTableAdapters.transactioninfoesTableAdapter())
                using (var adptrdepts = new librarycontextDataSetTableAdapters.departmentsTableAdapter())
                {
                    adptrcopies.Fill(reportSource.bookcopies);
                    adptrusers.Fill(reportSource.libraryusers);
                    adptrbookinfoes.Fill(reportSource.bookinfoes);
                    adptrtrans.Fill(reportSource.transactioninfoes);
                    adptrdepts.Fill(reportSource.departments);
                }

                var forClearance = new ForClearance();
                forClearance.SetDataSource(reportSource);
                return forClearance;
            }
        }

        public MostBorrowed MostBorrowed
        {
            get
            {
                using (var adptrbookinfoes = new librarycontextDataSetTableAdapters.bookinfoesTableAdapter())
                using (var adptrnumberoftimesborrowed = new librarycontextDataSetTableAdapters.numberoftimesborrowedTableAdapter())
                {
                    adptrnumberoftimesborrowed.Fill(reportSource.numberoftimesborrowed);
                    adptrbookinfoes.Fill(reportSource.bookinfoes);
                }

                var mostBorrowed = new MostBorrowed();
                mostBorrowed.SetDataSource(reportSource);
                return mostBorrowed;
            }
        }


        public Patrons Patrons
        {
            get
            {
                using (var adptrCourses = new librarycontextDataSetTableAdapters.coursesTableAdapter())
                using (var adptrUsers = new librarycontextDataSetTableAdapters.libraryusersTableAdapter())
                using (var adptrDepartments = new librarycontextDataSetTableAdapters.departmentsTableAdapter())
                {
                    adptrUsers.Fill(reportSource.libraryusers);
                    adptrDepartments.Fill(reportSource.departments);
                    adptrCourses.Fill(reportSource.courses);
                }

                var patrons = new Patrons();
                patrons.SetDataSource(reportSource);
                return patrons;
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
            reportForm.ReportViewer.ViewerCore.ReportSource = ForClearance;
            reportForm.ShowDialog();
        }

        public void ShowMostBOrrowed()
        {
            var reportForm = new MainWindow();
            reportForm.ReportViewer.ViewerCore.ReportSource = MostBorrowed;
            reportForm.ShowDialog();
        }

        public void ShowPatronsReport()
        {
            var reportForm = new MainWindow();
            reportForm.ReportViewer.ViewerCore.ReportSource = Patrons;
            reportForm.ShowDialog();
        }

        public Payments Payments
        {
            get
            {
                using (var adptr = new librarycontextDataSetTableAdapters.PaymentsTableAdapter())
                {
                    adptr.Fill(reportSource.Payments);

                    var rpt = new Payments();
                    rpt.SetDataSource(reportSource);
                    return rpt;
                }
            }
        }

        public void ShowPaymentsReport()
        {
            var reportForm = new MainWindow();
            reportForm.ReportViewer.ViewerCore.ReportSource = Payments;
            reportForm.ShowDialog();
        }
    }
}
