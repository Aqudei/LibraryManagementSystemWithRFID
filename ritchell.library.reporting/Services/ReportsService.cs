using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ritchell.library.reporting.Services
{
    public class ReportViewerService
    {
        private librarycontext librarycontext;

        public ReportViewerService()
        {
            librarycontext = new librarycontext();
            var tbladptmgr = new librarycontextTableAdapters.TableAdapterManager();


            tbladptmgr.bookcopies1TableAdapter = new librarycontextTableAdapters.bookcopies1TableAdapter();
            tbladptmgr.bookcopies1TableAdapter.Fill(librarycontext.bookcopies1);

            tbladptmgr.bookinfoes1TableAdapter = new librarycontextTableAdapters.bookinfoes1TableAdapter();
            tbladptmgr.bookinfoes1TableAdapter.Fill(librarycontext.bookinfoes1);

            tbladptmgr.departments1TableAdapter = new librarycontextTableAdapters.departments1TableAdapter();
            tbladptmgr.departments1TableAdapter.Fill(librarycontext.departments1);

            tbladptmgr.libraryusers1TableAdapter = new librarycontextTableAdapters.libraryusers1TableAdapter();
            tbladptmgr.libraryusers1TableAdapter.Fill(librarycontext.libraryusers1);

            tbladptmgr.sections1TableAdapter = new librarycontextTableAdapters.sections1TableAdapter();
            tbladptmgr.sections1TableAdapter.Fill(librarycontext.sections1);

            tbladptmgr.transactioninfoes1TableAdapter = new librarycontextTableAdapters.transactioninfoes1TableAdapter();
            tbladptmgr.transactioninfoes1TableAdapter.Fill(librarycontext.transactioninfoes1);
        }

        public UserControl BookListReport()
        {
            var rptDoc = new reporting.Reports.BookList();
            rptDoc.SetDataSource(librarycontext);

            var newControl = new Views.ReportViewer();
            newControl.CRViewer.ViewerCore.ReportSource = rptDoc;
            return newControl;
        }

        public UserControl ForClearanceReport()
        {
            var rptDoc = new reporting.Reports.ForCLearance();
            rptDoc.SetDataSource(librarycontext);

            var newControl = new Views.ReportViewer();
            newControl.CRViewer.ViewerCore.ReportSource = rptDoc;
            return newControl;
        }

        public UserControl PatronsReport()
        {
            var rptDoc = new reporting.Reports.StudentList();
            rptDoc.SetDataSource(librarycontext);

            var newControl = new Views.ReportViewer();
            newControl.CRViewer.ViewerCore.ReportSource = rptDoc;
            return newControl;
        }
    }
}
