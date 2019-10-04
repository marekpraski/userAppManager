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
    public partial class RolaEditorForm : DBEditorForm
    {

        private ListViewItem currentSelectedRola = null;

        private int appModuleListViewPadding = 30;

        private App app;


        public RolaEditorForm(SqlConnection dbConnection, string sqlQuery, App app)
            : base(dbConnection, sqlQuery)
        {
            this.app = app;
            InitializeComponent();
            resizeThisForm();
            populateAppModuleListview();
        }


        private void resizeThisForm()
        {
            int baseFormWidth = formatter.calculateBDEditorFormWidth();
            appModuleListView.Location = new System.Drawing.Point(baseFormWidth, 13);

            this.Width = baseFormWidth + appModuleListView.Width + appModuleListViewPadding;
        }

        protected override void BaseDatagridClickedEvent()
        {
            uncheckModules();
            checkModules();
        }

        private void uncheckModules()
        {
            foreach (ListViewItem item in appModuleListView.Items)
            {
                item.Checked = false;
            }

            //foreach(DataGridView.)
        }

        private void checkModules()
        {
            Rola rola = app.getRola(getSelectedRolaId());
            if (rola != null)
            {
                foreach (ListViewItem item in appModuleListView.Items)
                {
                    string moduleId = item.Name;
                    string grantApp;
                    if (rola.moduleDict.ContainsKey(moduleId))
                    {
                        item.Checked = true;
                        rola.modulePrivilageDict.TryGetValue(moduleId, out grantApp);
                        item.SubItems[1].Text = grantApp;
                    }
                }
                headerAppModule.Text = "uprawnienia roli " + rola.name + " do modułów aplikacji";
            }
        }

        private string getSelectedRolaId()
        {
            string rolaId = "";
            DataGridViewCell cell = baseDataGridview.CurrentCell;
            int columnIndex = cell.ColumnIndex;
            if(columnIndex == SqlQueries.getRolaList_rolaIdIndex)
            {
                rolaId = cell.Value.ToString();
            }
            return rolaId;
        }

        private void populateAppModuleListview()
        {
            appModuleListView.Items.Clear();

            if (app != null && app.hasModules())
            {
                ListViewItem[] moduleRange = new ListViewItem[app.moduleIdList.Count];
                int i = 0;

                foreach (AppModule module in app.moduleList)
                {
                    string name = module.name;
                    ListViewItem item = new ListViewItem(name);
                    item.Name = module.id;
                    item.SubItems.Add("");
                    moduleRange[i] = item;
                    i++;
                }
                appModuleListView.Items.AddRange(moduleRange);
            }
        }

    }
}
