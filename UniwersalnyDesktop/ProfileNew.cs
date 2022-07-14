using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace UniwersalnyDesktop
{
    public partial class ProfileNew : Form
    {
        private readonly FormMode formMode;
        private readonly DesktopProfile editedProfile;
        private DesktopDataHandler dataHandler = new DesktopDataHandler();

        public ProfileNew(FormMode formMode, DesktopProfile editedProfile)
        {
            InitializeComponent();
            this.formMode = formMode;
            this.editedProfile = editedProfile;
        }

        #region metody na starcie formatki
        private void ProfileNew_Load(object sender, EventArgs e)
        {
            if (formMode == FormMode.NEW)
                this.Text = "Dodawanie nowego profilu";
            else if (formMode == FormMode.EDIT)
            {
                this.Text = "Edycja profilu";
                tbNazwa.Text = editedProfile.name;
                //
                //sb.Append(" Update [profile_desktop] set [config_profile] = '" + tbConfig.Text + "' where [ID_profile] = " + selectedProfileId);
                tbConfig.Text = editedProfile.configXlm;
                pictureBoxLogo.Image = convertBytesToImage(editedProfile.logoImageAsBytes);
            }
        }
        #endregion

        #region konwersja pliku graficznego do byte[] i odwrotnie
        private byte[] convertImageToBytes(Image img)
        {
            if (img == null)
                return null;
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
        #endregion

        #region naciśnięcie przycisku Zapisz
        private void btnZapisz_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbNazwa.Text))
                return;

            if (formMode == FormMode.NEW)
                insertProfileToDB();
            else if (formMode == FormMode.EDIT)
                updateProfileInDB();
        }
        #endregion

        #region zapisywanie do bazy
        private void insertProfileToDB()
        {
            DesktopProfile newProfile = new DesktopProfile();
            setProfileProperies(newProfile);

            if (dataHandler.insertProfileToDB(newProfile))
            {
                readProfileIdFromDB();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Zapisano");
            }
        }

        private void updateProfileInDB()
        {
            setProfileProperies(editedProfile);

            if (dataHandler.updateProfileInDB(editedProfile))
            {
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("Zapisano");
            }
        }
        private void setProfileProperies(DesktopProfile profile)
        {
            profile.name = tbNazwa.Text;
            profile.configXlm = tbConfig.Text;
            profile.logoImageAsBytes = convertImageToBytes(pictureBoxLogo.Image);
        }

        #endregion

        #region czytanie z bazy
        private void readProfileIdFromDB()
        {
            DesktopProfile newProfile = dataHandler.readProfileFromDB(" ID_profile = (select MAX(ID_profile) from profile_desktop)");

            editedProfile.id = newProfile.id;
        }
        #endregion

        #region wybór obrazka logo z komputera
        private void btnLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Pliki graficzne(*.jpg;*.jpeg;*.png|*.jpg;*.jpeg;*.png", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    pictureBoxLogo.Image = Image.FromFile(ofd.FileName);
            }
        } 
        #endregion
    }
}
