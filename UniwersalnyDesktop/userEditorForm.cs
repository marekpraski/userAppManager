using System;
using System.Drawing;

namespace UniwersalnyDesktop
{
    public partial class UserEditorForm : DBEditorForm
    {


        public UserEditorForm(string sqlQuery)
            : base(sqlQuery)
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
