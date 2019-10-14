using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace UniwersalnyDesktop
{
    public partial class UserEditorForm : DBEditorForm
    {


        public UserEditorForm(SqlConnection dbConnection, string sqlQuery)
            : base(dbConnection, sqlQuery)
        {
            InitializeComponent();
            resizeThisForm();
        }

        private void resizeThisForm()
        {
            int baseFormWidth = formatter.calculateBaseFormWidth(base.baseDatagrid);
            this.Width = baseFormWidth;
        }

        #region Region - zdarzenia wywołane przez użytkownika



        #endregion
    }
}
