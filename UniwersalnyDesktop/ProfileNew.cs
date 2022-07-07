using System;
using DatabaseInterface;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class ProfileNew : Form
    {
        private readonly FormMode formMode;
        private readonly DesktopProfile editedProfile;

        public ProfileNew(FormMode formMode, DesktopProfile editedProfile)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.editedProfile = editedProfile;
        }

        private void ProfileNew_Load(object sender, EventArgs e)
        {
            if (formMode == FormMode.NEW)
                this.Text = "Dodawanie nowego profilu";
            else if (formMode == FormMode.EDIT)
            {
                this.Text = "Edycja profilu";
                tbNazwa.Text = editedProfile.name;
                tbDomena.Text = editedProfile.domena;
                tbLdap.Text = editedProfile.ldap;
            }
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbNazwa.Text))
                return;

            if (formMode == FormMode.NEW)
                insertProfileToDB();
            else if (formMode == FormMode.EDIT)
                updateProfileInDB();
        }


        private void insertProfileToDB()
        {            
            string query = "  insert into [profile_desktop] ([name_profile], [domena], [ldap]) VALUES ('" + tbNazwa.Text +
                            "', '" + tbDomena.Text + "', '" + tbLdap.Text + "')";
            if (new DBWriter(LoginForm.dbConnection).executeQuery(query))
            {
                readNewProfileDataFromDB();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Zapisano");
            }
        }

        private void updateProfileInDB()
        {
            string query = "update [profile_desktop] set name_profile = '" + tbNazwa.Text +
                        "', domena = '" + tbDomena.Text + "', ldap = '" + tbLdap.Text + "'  where ID_profile = " + editedProfile.id;
            if (new DBWriter(LoginForm.dbConnection).executeQuery(query))
            {
                editedProfile.name = tbNazwa.Text;
                editedProfile.domena = tbDomena.Text;
                editedProfile.ldap = tbLdap.Text;
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Zapisano");
            }
        }

        private void readNewProfileDataFromDB()
        {
            string query = "select ID_profile, name_profile, domena, ldap from [profile_desktop] where ID_profile = (select MAX(ID_profile) from profile_desktop)";
            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(query);
            editedProfile.id = qd.getDataValue(0, "ID_profile").ToString();
            editedProfile.name = qd.getDataValue(0, "name_profile").ToString();
            editedProfile.domena = qd.getDataValue(0, "domena").ToString();
            editedProfile.ldap = qd.getDataValue(0, "ldap").ToString();
        }
    }
}
