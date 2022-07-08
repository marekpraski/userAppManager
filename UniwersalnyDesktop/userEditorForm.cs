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
        }

        private void UserEditorForm_Load(object sender, EventArgs e)
        {
            resizeThisForm();
        }
        private void resizeThisForm()
        {
            int baseFormWidth = formatter.calculateBaseFormWidth(base.baseDatagrid);
            this.Width = baseFormWidth;
            int btnYLocation = btnNowy.Location.Y;
            btnNowy.Location = new Point(this.Width - 105, btnYLocation);
        }

        #region Region - zdarzenia wywołane przez użytkownika

        private void btnNowy_Click(object sender, EventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.Show();
        }

        #endregion
    }
}
