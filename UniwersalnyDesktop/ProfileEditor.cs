using DatabaseInterface;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class ProfileEditor : Form
    {
        private Dictionary<string, DesktopProfile> profileDict;     //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
        private ProfileEditor()
        {
            InitializeComponent();
        }
        public ProfileEditor(Dictionary<string, DesktopProfile> profileDict) : this()
        {
            this.profileDict = profileDict;
            fillProfileCombo();
        }

        private void fillProfileCombo()
        {
            cbProfiles.Items.Clear();
            cbProfiles.Text = "";
            foreach(string id in profileDict.Keys)
            {
                cbProfiles.Items.Add(new ComboboxItem(profileDict[id].name, id));
            }
            cbProfiles.DisplayMember = "displayText";
            cbProfiles.ValueMember = "value";
        }

        #region kliknięcie przycisków na pasku narzędziowym

        private void btnDodajAplikacje_Click(object sender, EventArgs e)
        {
            string idProfile = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            ProfileAppSelector addAppForm = new ProfileAppSelector(profileDict[idProfile]);
            addAppForm.ShowDialog();
            if(addAppForm.DialogResult == DialogResult.OK)
                fillDgvProfileApps(idProfile);
        }

        private void btnUsunAplikacje_Click(object sender, EventArgs e)
        {
            string idProfile = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            string[] idApps = getSelectedApps();
            for (int i = 0; i < idApps.Length; i++)
            {
                profileDict[idProfile].removeAppFromProfile(idApps[i]);
            }
            
            string query = "delete from profile_app where ID_profile = " + idProfile + "  and ID_app in (" + String.Join(",", idApps) + "); ";
            new DBWriter(LoginForm.dbConnection).executeQuery(query);
            fillDgvProfileApps(idProfile);
        }

        private string[] getSelectedApps()
        {
            string[] ids = new string[dgvProfileApps.SelectedRows.Count];
            int i = 0;
            foreach(DataGridViewRow row in dgvProfileApps.SelectedRows)
            {
                ids[i] = row.Cells["colId"].Value.ToString();
                i++;
            }
            return ids;
        }
        private void btnDodajProfil_Click(object sender, EventArgs e)
        {
            DesktopProfile newProfile = new DesktopProfile();
            runProfileEditor(FormMode.NEW, newProfile);
        }

        private void btnEdytujProfil_Click(object sender, EventArgs e)
        {
            string profileId = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            runProfileEditor(FormMode.EDIT, profileDict[profileId]);
        }

        private void runProfileEditor(FormMode formMode, DesktopProfile profile)
        {
            ProfileNew profileNewForm = new ProfileNew(formMode, profile);
            profileNewForm.ShowDialog();
            if (profileNewForm.DialogResult == DialogResult.OK)
            {
                if(formMode == FormMode.NEW)
                    this.profileDict.Add(profile.id, profile);
                fillProfileCombo();
                profileNewForm.Close();
            }
        }

        #endregion

        #region zmiana wyboru w kombo
        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profileId = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            fillProfileDetais(profileId);
            fillDgvProfileApps(profileId);
        }

        private void fillProfileDetais(string profileId)
        {
            labelDomena.Text = profileDict[profileId].domena;
            labelLdap.Text = profileDict[profileId].ldap;
        }
        #endregion

        #region wypełnianie datagrida
        private void fillDgvProfileApps(string profileId)
        {
            dgvProfileApps.Rows.Clear();
            DesktopProfile profile = profileDict[profileId];
            foreach (string appId in profile.applications.Keys)
            {
                App app = profile.applications[appId];
                addOneDgvRow(app);
            }
            dgvProfileApps.Sort(dgvProfileApps.Columns["colNazwa"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void addOneDgvRow(App app)
        {
            if (String.IsNullOrEmpty(app.displayName))
                return;
            dgvProfileApps.Rows.Add(
                                app.id,
                                app.displayName,
                                app.serverName,
                                app.databaseName,
                                app.reportSerwerLink
                                );
        }
        #endregion

    }
}
