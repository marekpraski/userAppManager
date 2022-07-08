using System;
using DatabaseInterface;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class PasswordChanger : Form
    {

        public PasswordChanger()
        {
            InitializeComponent();
            FillUserInformation();
        }

        private void FillUserInformation()
        {
            string query = "SELECT * FROM users_list WHERE login_user = '" + LoginForm.userLogin + "' ";

            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(query);
            lab_FirstName.Text = qd.getDataValue(0,"imie_user").ToString();
            lab_LastName.Text = qd.getDataValue(0,"nazwisko_user").ToString();
            lab_Login.Text = qd.getDataValue(0,"login_user").ToString();
        }

        private bool checkForm()
        {
            bool error = false;
            errorProvider.Clear();

            if (this.tbStareHaslo.Text == "")
            {
                errorProvider.SetError(this.tbStareHaslo, "Pole wymagane");
                error = true;
            }

            if (this.tbNoweHaslo1.Text == "")
            {
                errorProvider.SetError(this.tbNoweHaslo1, "Pole wymagane");
                error = true;
            }

            if (this.tbNoweHaslo2.Text == "")
            {
                errorProvider.SetError(this.tbNoweHaslo2, "Pole wymagane");
                error = true;
            }

            if (!error)
            {
                if (this.tbNoweHaslo1.Text != this.tbNoweHaslo2.Text)
                {
                    errorProvider.SetError(this.tbNoweHaslo1, "Has³a musz¹ byæ indentyczne");
                    errorProvider.SetError(this.tbNoweHaslo2, "Has³a musz¹ byæ indentyczne");
                    error = true;
                }
            }

            return !error;
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (checkForm())
            {
                string query = "sp_password '"+tbStareHaslo.Text+"','"+tbNoweHaslo1.Text+"'";
                if (new DBWriter(LoginForm.dbConnection).executeQuery(query))
                {
                    LoginForm.userPassword = tbNoweHaslo1.Text;
                    MessageBox.Show("Has³o zosta³o zmienione", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.None);
                    this.Close();
                }
            }
        }
    }
}