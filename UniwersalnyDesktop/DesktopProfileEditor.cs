using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class DesktopProfileEditor : Form
    {
        private Dictionary<string, DesktopProfile> profileDict;     //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
        private DesktopProfileEditor()
        {
            InitializeComponent();
        }
        public DesktopProfileEditor(Dictionary<string, DesktopProfile> profileDict) : this()
        {
            this.profileDict = profileDict;
            fillProfileCombo();
        }

        private void fillProfileCombo()
        {
            foreach(string id in profileDict.Keys)
            {
                cbProfiles.Items.Add(new ComboboxItem(profileDict[id].name, id));
            }
            cbProfiles.DisplayMember = "displayText";
            cbProfiles.ValueMember = "value";
        }

        #region kliknięcie przycisków na pasku narzędziowym
        private void btnZapisz_Click(object sender, EventArgs e)
        {

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            string idProfile = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            AddApplicationToProfile addAppForm = new AddApplicationToProfile(profileDict[idProfile]);
            addAppForm.ShowDialog();
            if(addAppForm.DialogResult == DialogResult.OK)
                fillDgvProfileApps(idProfile);
        }

        private void btnUsun_Click(object sender, EventArgs e)
        {

        }

        private void btnOdswiez_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region zmiana wyboru w kombo
        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profileId = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            fillDgvProfileApps(profileId);
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
