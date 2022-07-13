using System;
using DatabaseInterface;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Data;

namespace UniwersalnyDesktop
{
    public partial class ProfileNew : Form
    {
        private readonly FormMode formMode;
        private readonly DesktopProfile editedProfile;
        private byte[] logoImage;

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
                pictureBoxLogo.Image = convertBytesToImage(editedProfile.logoImage);
            }
        }

        private byte[] convertImageToBytes (Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private Image convertBytesToImage(byte[] imageAsBytes)
        {
            if (imageAsBytes == null || imageAsBytes.Length == 0)
                return null;
            using (MemoryStream ms = new MemoryStream(imageAsBytes))
            {
                return Image.FromStream(ms);
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
            string query = "  insert into [profile_desktop] ([name_profile], [domena], [ldap], [logo_profile] ) VALUES ('" + tbNazwa.Text +
                            "', '" + tbDomena.Text + "', '" + tbLdap.Text + "',  @logoImageBytes )";
            if (runParameterisedQuery(query))
            {
                readNewProfileDataFromDB();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Zapisano");
            }
        }

        private bool runParameterisedQuery(string query)
        {
            DBWriter dbwriter = new DBWriter(LoginForm.dbConnection);
            dbwriter.initiateParameterizedCommand();
            dbwriter.addCommmandParameter("@logoImageBytes", SqlDbType.VarBinary, this.logoImage);
            return dbwriter.executeQuery(query);
        }

        private void updateProfileInDB()
        {
            string query = "update [profile_desktop] set name_profile = '" + tbNazwa.Text +
                        "', domena = '" + tbDomena.Text + "', ldap = '" + tbLdap.Text + "' , [logo_profile] =  @logoImageBytes where ID_profile = " + editedProfile.id;
            if (runParameterisedQuery(query))
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
            string query = "select ID_profile, name_profile, domena, ldap, logo_profile from [profile_desktop] where ID_profile = (select MAX(ID_profile) from profile_desktop)";
            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(query);
            editedProfile.id = qd.getDataValue(0, "ID_profile").ToString();
            editedProfile.name = qd.getDataValue(0, "name_profile").ToString();
            editedProfile.domena = qd.getDataValue(0, "domena").ToString();
            editedProfile.ldap = qd.getDataValue(0, "ldap").ToString();
            editedProfile.logoImage = qd.getDataValue(0, "logo_profile") as byte[];
            logoImage = editedProfile.logoImage;
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "Pliki graficzne(*.jpg;*.jpeg;*.png|*.jpg;*.jpeg;*.png", Multiselect = false})
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxLogo.Image = Image.FromFile(ofd.FileName);
                    logoImage = convertImageToBytes(pictureBoxLogo.Image);
                }
            }
        }
    }
}
