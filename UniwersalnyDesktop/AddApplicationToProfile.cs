using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DatabaseInterface;

namespace UniwersalnyDesktop
{
    public partial class AddApplicationToProfile : Form
    {

        private DesktopProfile editedProfile;
        private Dictionary<string, App> appDict = new Dictionary<string, App>();
        List<App> addedApps = new List<App>();

        public AddApplicationToProfile(DesktopProfile editedProfile)
        {
            InitializeComponent();
            this.editedProfile = editedProfile;
            labelProfileName.Text = editedProfile.name;
            getAllApplicationForProfiles(editedProfile.id);
        }

        private void getAllApplicationForProfiles(string idProfile)
        {
            applicationDGV.ClearSelection();

                string sql = @" WITH AppNewList 
                              AS 
                               ( 
                                 SELECT ID_app FROM app_list WHERE name_app <> 'SoftMineAdmin' 
                                 EXCEPT 
                                 SELECT ID_app FROM profile_app WHERE ID_profile = " + idProfile +
                                 @") 
                                 SELECT an.ID_app,
								 CASE WHEN variant = 'FULL' THEN al.name_app ELSE al.name_app + '  ' + variant END as nazwaAplikacji 
								 ,al.name_db, al.path_app
                                 FROM AppNewList as an 
								 INNER JOIN app_list as al ON an.ID_app = al.ID_app ";
            QueryData qd = new DBReader(LoginForm.dbConnection).readFromDB(sql);
            for (int i = 0; i < qd.dataRowsNumber; i++)
            {
                App app = new App();
                app.id = qd.getDataValue(i, "ID_app").ToString();
                app.displayName = qd.getDataValue(i, "nazwaAplikacji").ToString();
                app.databaseName = qd.getDataValue(i, "name_db").ToString();
                app.executionPath = qd.getDataValue(i, "path_app").ToString();
                appDict.Add(app.id, app);

                applicationDGV.Rows.Add(
                   app.id,
                    app.displayName
                    );
            }
            applicationDGV.Sort(applicationDGV.Columns["colNazwa"], System.ComponentModel.ListSortDirection.Ascending);
        }
        private void btnZapisz_Click(object sender, EventArgs e)
        {
            applicationDGV.EndEdit();
            addAppsToProfile();
            MessageBox.Show("Wszystkie aplikacje zostały dodane do profilu.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void addAppsToProfile()
        {
            for (int i = 0; i < applicationDGV.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell chkchecking = applicationDGV.Rows[i].Cells["colDodaj"] as DataGridViewCheckBoxCell;

                    bool v = Convert.ToBoolean(chkchecking.Value);
                if (applicationDGV.Rows[i].Cells["colDodaj"].Value != null)
                {
                    string appId = applicationDGV.Rows[i].Cells["colId"].Value.ToString();
                    editedProfile.addAppToProfile(appDict[appId]);
                    addedApps.Add(appDict[appId]);
                }
            }
            saveToDB();
        }

        private void saveToDB()
        {
            string query = "";
            for (int i = 0; i < addedApps.Count; i++)
            {
                query += "INSERT INTO profile_app (ID_profile,ID_app) VALUES (" + editedProfile.id + "," + addedApps[i].id + "); ";
            }
            new DBWriter(LoginForm.dbConnection).executeQuery(query);
        }
    }
}
