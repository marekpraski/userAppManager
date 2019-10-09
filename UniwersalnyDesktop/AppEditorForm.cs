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

        private int moduleDatagridPadding = 30;

        App selectedApp = null;                          //aplikacja aktualnie zaznaczona w datagridzie
        private bool bazeDatagridCreated = false;


        public AppEditorForm(SqlConnection dbConnection, string sqlQuery, Dictionary<string, App> appDictionary)
            : base(dbConnection, sqlQuery)
        {
            this.appDictionary = appDictionary;
            InitializeComponent();
            setUpThisForm();
        }

        private void setUpThisForm()
        {
            //na starcie wyświetlam moduły aplikacji pierwszej od góry
            baseDataGridview.CurrentCell = baseDataGridview.Rows[0].Cells[0];
            getSelectedApp();

            //datagrid w formatce nie ma żadnych kolumn na starcie, dodaję je; kolejność dodawania jest istotna
            //podczas aktualizacji odwołuję się do indeksu kolumny

            if (selectedApp != null)
            {
                moduleDatagrid.addTextDatagridColumn("moduły " + selectedApp.appDisplayName, 250);
                moduleDatagrid.addDatagridRows(selectedApp.getModuleNameList().Count);
                moduleDatagrid.populateTextDatagridColumn(0, selectedApp.getModuleNameList());
            }
            else
            {
                moduleDatagrid.addTextDatagridColumn("moduły aplikacji", 250);
            }

            resizeThisForm();
        }

        private void getSelectedApp()
        {
            DataGridViewCell cell = baseDataGridview.CurrentCell;
            int columnIndex = cell.ColumnIndex;
            if (columnIndex == SqlQueries.getAppList_appIdIndex && cell.Value != null)
            {
                string appId = cell.Value.ToString();
                if (appDictionary.ContainsKey(appId))
                {
                    appDictionary.TryGetValue(appId, out selectedApp);
                }
                else
                {
                    selectedApp = null;
                }
            }
            
        }

        private void resizeThisForm()
        {
            int baseFormWidth = formatter.calculateBDEditorFormWidth();
            moduleDatagrid.Location = new System.Drawing.Point(baseFormWidth, 13);
            moduleDatagrid.resizeThisForm();

            this.Width = baseFormWidth + moduleDatagrid.Width + moduleDatagridPadding;
        }

        protected override void BaseDatagridClickedEvent()
        {
            getSelectedApp();

            //czyszczę stare wypełnienia
            moduleDatagrid.clearDatagrid();

            //wypełniam datagrid danymi nowej aplikacji
            if (selectedApp != null)
            {
                moduleDatagrid.addDatagridRows(selectedApp.getModuleNameList().Count);
                moduleDatagrid.populateTextDatagridColumn(0, selectedApp.getModuleNameList());
                moduleDatagrid.setHeaderText(0, "moduły " + selectedApp.appDisplayName);
            }
            else
            {
                moduleDatagrid.setHeaderText(0, "moduły aplikacji");
            }
        }


    }
}
