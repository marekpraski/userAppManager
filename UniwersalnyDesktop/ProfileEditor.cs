﻿using DatabaseInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using UtilityTools;

namespace UniwersalnyDesktop
{
    public partial class ProfileEditor : Form
    {
        private Dictionary<string, DesktopProfile> profileDict;     //słownik wszystkich profili zdefiniowanych w Desktopie, kluczem jest id
        private readonly Dictionary<string, DesktopUser> allUsersDict;
        private readonly Dictionary<string, App> appDictionary;
        private string selectedProfileId;

        private ProfileEditor()
        {
            InitializeComponent();
        }
        public ProfileEditor(Dictionary<string, DesktopProfile> profileDict, Dictionary<string, DesktopUser> allUsersDict, Dictionary<string, App> appDictionary) : this()
        {
            this.profileDict = profileDict;
            this.allUsersDict = allUsersDict;
            this.appDictionary = appDictionary;
            fillProfileCombo();
        }

        #region metody na starcie formatki
        private void fillProfileCombo()
        {
            cbProfiles.Items.Clear();
            cbProfiles.Text = "";
            foreach (string id in profileDict.Keys)
            {
                cbProfiles.Items.Add(new ComboboxItem(profileDict[id].name, id));
            }
            cbProfiles.DisplayMember = "displayText";
            cbProfiles.ValueMember = "value";
        }
        #endregion

        #region kliknięcie przycisku Zapisz

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            dgvProfileApps.EndEdit();
            updateProfileApps(selectedProfileId);
            string query = generateUpdateQuery(selectedProfileId);
            updateToDB(query);
            if (cbProfiles.Text != profileDict[selectedProfileId].name)
            {
                profileDict[selectedProfileId].name = cbProfiles.Text;
                fillProfileCombo();
                cbProfiles.SelectedIndex = new ComboboxTools().getIndexFromStringValue(cbProfiles, selectedProfileId);
            }
        }

        private void updateProfileApps(string idProfile)
        {
            for (int i = 0; i < dgvProfileApps.RowCount; i++)
            {
                string appId = dgvProfileApps.Rows[i].Cells["colId"].Value.ToString();
                AppProfileParameters appParams = new AppProfileParameters(idProfile, appId);
                appParams.databaseName = dgvProfileApps.Rows[i].Cells["colBazaDanych"].Value == null ? "" : dgvProfileApps.Rows[i].Cells["colBazaDanych"].Value.ToString();
                appParams.serverName = dgvProfileApps.Rows[i].Cells["colSerwer"].Value == null ? "" : dgvProfileApps.Rows[i].Cells["colSerwer"].Value.ToString();
                appParams.reportPath = dgvProfileApps.Rows[i].Cells["colRaport"].Value == null ? "" : dgvProfileApps.Rows[i].Cells["colRaport"].Value.ToString();
                appParams.odbcDriver = dgvProfileApps.Rows[i].Cells["colSterownik"].Value == null ? "" : dgvProfileApps.Rows[i].Cells["colSterownik"].Value.ToString();
                appDictionary[appId].addAppProfileParameters(appParams);
            }
        }

        private string generateUpdateQuery(string idProfile)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dgvProfileApps.RowCount; i++)
            {
                string appId = dgvProfileApps.Rows[i].Cells["colId"].Value.ToString();
                string paramsAsXml = appDictionary[appId].getAppProfileSpecificSettingsAsXml(idProfile);
                sb.Append(" Update [profile_app] set app_params = ");
                sb.Append(paramsAsXml);
                sb.Append(" where ID_profile = ");
                sb.Append(idProfile);
                sb.Append(" and ID_app = ");
                sb.Append(appId);
                sb.Append("; ");
            }

            if(cbProfiles.Text != profileDict[idProfile].name)
                sb.Append(" Update [profile_desktop] set [name_profile] = '" + cbProfiles.Text + "' where [ID_profile] = " + selectedProfileId);

            return sb.ToString();
        }

        private void updateToDB(string query)
        {
            new DBWriter(LoginForm.dbConnection).executeQuery(query);
        }
        #endregion

        #region kliknięcie przycisków na paska narzędziowym

        private void btnDodajAplikacje_Click(object sender, EventArgs e)
        {
            string idProfile = selectedProfileId;
            ProfileItemSelector addAppForm = new ProfileItemSelector(profileDict[idProfile], new ProfileItemConverter().convertToIProfileItemDictionary(this.appDictionary));
            addAppForm.ShowDialog();
            if(addAppForm.DialogResult == DialogResult.OK)
                fillDgv(idProfile);
        }

        private void btnDodajUzytkownika_Click(object sender, EventArgs e)
        {
            string idProfile = selectedProfileId;
            ProfileItemSelector addAppForm = new ProfileItemSelector(profileDict[idProfile], new ProfileItemConverter().convertToIProfileItemDictionary(this.allUsersDict));
            addAppForm.ShowDialog();
            if (addAppForm.DialogResult == DialogResult.OK)
                fillDgv(idProfile);
        }

        private void btnUsunAplikacje_Click(object sender, EventArgs e)
        {
            string[] idApps = getSelectedApps();
            for (int i = 0; i < idApps.Length; i++)
            {
                profileDict[selectedProfileId].removeAppFromProfile(idApps[i]);
            }
            
            string query = "delete from profile_app where ID_profile = " + selectedProfileId + "  and ID_app in (" + String.Join(",", idApps) + "); ";
            new DBWriter(LoginForm.dbConnection).executeQuery(query);
            fillDgv(selectedProfileId);
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
            runProfileEditor(FormMode.EDIT, profileDict[selectedProfileId]);
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
                if (formMode == FormMode.EDIT)
                    cbProfiles.SelectedIndex = new ComboboxTools().getIndexFromStringValue(cbProfiles, selectedProfileId);
                profileNewForm.Close();
            }
        }

        #endregion

        #region kliknięcie przycisku "kopiuj nazwę serwera"
        private void btnKopiujSerwer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvProfileApps.RowCount; i++)
            {
                dgvProfileApps.Rows[i].Cells["colSerwer"].Value = tbSerwer.Text;
            }
        } 
        #endregion

        #region zmiana wyboru w kombo
        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedProfileId = (cbProfiles.SelectedItem as ComboboxItem).value.ToString();
            toggleControlsEnabled(true);
            fillProfileDetais(selectedProfileId);
            fillDgv(selectedProfileId);
        }

        private void toggleControlsEnabled(bool isEnabled)
        {
            btnDodajAplikacje.Enabled = isEnabled;
            btnDodajUzytkownika.Enabled = isEnabled;
            btnUsunAplikacje.Enabled = isEnabled;
            btnUsunUzytkownika.Enabled = isEnabled;
            btnUsunProfil.Enabled = isEnabled;
            btnEdytujProfil.Enabled = isEnabled;
        }

        private void fillProfileDetais(string profileId)
        {
            labelDomena.Text = profileDict[profileId].domena;
            labelLdap.Text = profileDict[profileId].ldap;
        }
        #endregion

        #region wypełnianie datagrida
        private void fillDgv(string profileId)
        {
            dgvProfileApps.Rows.Clear();
            DesktopProfile profile = profileDict[profileId];
            foreach (string appId in profile.applications.Keys)
            {
                App app = profile.applications[appId] as App;
                addOneDgvRow(app, profileId);
            }
            dgvProfileApps.Sort(dgvProfileApps.Columns["colNazwa"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void addOneDgvRow(App app, string profileId)
        {
            if (String.IsNullOrEmpty(app.displayName))
                return;
            dgvProfileApps.Rows.Add(
                                app.id,
                                app.displayName,
                                app.getServerName(profileId),
                                app.getDatabaseName(profileId),
                                app.getOdbcDriver(profileId),
                                app.getReportPath(profileId)
                                );
        }
        #endregion

    }
}
