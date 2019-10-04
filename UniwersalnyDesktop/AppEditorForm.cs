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
    public partial class AppEditorForm : DBEditorForm
    {

        private Dictionary<string, App> appDictionary;             //lista wszystkich aplikacji zdefiniowanych w desktopie, kluczem jest Id

        private int appModuleGridviewPadding = 30;


        public AppEditorForm(SqlConnection dbConnection, string sqlQuery, Dictionary<string, App> appDictionary)
            : base(dbConnection, sqlQuery)
        {
            this.appDictionary = appDictionary;
            InitializeComponent();
            resizeThisForm();
        }

        private void resizeThisForm()
        {
            int baseFormWidth = formatter.calculateBDEditorFormWidth();
            appModuleGridview.Location = new System.Drawing.Point(baseFormWidth, 13);

            this.Width = baseFormWidth + appModuleGridview.Width + appModuleGridviewPadding;
        }

        protected override void BaseDatagridClickedEvent()
        {
            populateAppModuleGridview();
        }


        private void populateAppModuleGridview()
        {
            appModuleGridview.Rows.Clear();
            App app = getClickedApp();
            if(app!= null && app.hasModules())
            {
                foreach(AppModule module in app.moduleList)
                {
                    appModuleGridview.Rows.Add(module.name);
                }
                moduleNameColumn.HeaderText = "moduły " + app.appDisplayName;
            }
        }


        private App getClickedApp()
        {
            App app = null;
            DataGridViewCell cellClicked = baseDataGridview.CurrentCell;
            int columnClicked = cellClicked.ColumnIndex;
            
            if (columnClicked == SqlQueries.getAppList_appIdIndex)
            {
                string appId = cellClicked.Value.ToString();
                if (appDictionary.ContainsKey(appId))
                {
                    appDictionary.TryGetValue(appId, out app);
                    return app;
                }
            }
            return null;
        }
    }
}
