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
        private int moduleDatagridPadding = 30;

        private App currentApp;
        private Rola currentRola;


        public RolaEditorForm(SqlConnection dbConnection, string sqlQuery, App app)
            : base(dbConnection, sqlQuery)
        {
            this.currentApp = app;
            InitializeComponent();
            setUpThisForm();
        }

        private void setUpThisForm()
        {
            //na starcie wyświetlam moduły roli pierwszej od góry
            baseDataGridview.CurrentCell = baseDataGridview.Rows[0].Cells[0];
            getSelectedRola();

            //datagrid w formatce nie ma żadnych kolumn na starcie, dodaję je; kolejność dodawania jest istotna
            //podczas aktualizacji odwołuję się do indeksu kolumny


            //muszę dodać chociaż jedną kolumnę zanim dodam wiersze
            moduleDatagrid.addTextDatagridColumn("moduły " + currentApp.appDisplayName, 250);     
            moduleDatagrid.addDatagridRows(currentApp.getModuleNameList().Count);              

            //po podaniu wierszy dodaję kolejne kolumny
            moduleDatagrid.addCheckboxColumn();                                    //indeks tej kolumny zawsze jest 0, nawet jeżeli dodam ją później
            moduleDatagrid.addTextDatagridColumn("uprawnienia", 80);                   

            //po dodaniu, kolumny wypełniam danymi
            moduleDatagrid.populateCheckboxColumn(getModuleAccessList());                    //indeks tej kolumny zawsze jest 0
            moduleDatagrid.populateTextDatagridColumn(1, currentApp.getModuleNameList());    //indeks = 1 
            moduleDatagrid.populateTextDatagridColumn(2, getModuleAccessRights());          //indeks = 2

            resizeThisForm();
        }

        //dotyczy typu Grant_app, 1, 2 itp dla modułów, do których rola w ogóle ma dostęp, tzn checkbox jest zafajkowany
        private List<string> getModuleAccessRights()
        {
            List<string> moduleAccessRightsList = new List<string>();
            string listItem = "";
            foreach (AppModule module in currentApp.moduleList)
            {
                listItem = currentRola.moduleDict.ContainsKey(module.id) ? currentRola.getModuleAccessRight(module) : "";
                moduleAccessRightsList.Add(listItem);
            }
            return moduleAccessRightsList;
        }

        //czyszczenie kolumny z Grant_app
        private void clearAccessRightsColumn()
        {
            moduleDatagrid.clearTextDatagridColumn(2);
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
            getSelectedRola();

            //czyszczę stare wypełnienia
            clearAccessRightsColumn();
            uncheckModules();

            //wypełniam datagrid danymi nowej roli            
            showAccessRights();
            checkModules();
        }

        private void showAccessRights()
        {
            if (currentRola != null)
            {
                moduleDatagrid.populateTextDatagridColumn(2, getModuleAccessRights());
            }
        }

        private void uncheckModules()
        {
            moduleDatagrid.clearCheckboxColumn();
        }

        private void checkModules()
        {
            if (currentRola != null)
            {
                moduleDatagrid.populateCheckboxColumn(getModuleAccessList());
            }
        }

        //lista modułów do których rola ma dostęp, bez względu na poziom uprawnień
        private List<bool> getModuleAccessList()
        {
            List<bool> moduleAccessList = new List<bool>();
            foreach (AppModule module in currentApp.moduleList)
            {
                bool listItem = false;
                listItem = currentRola.moduleDict.ContainsKey(module.id) ? true : false;
                moduleAccessList.Add(listItem);
            }
            return moduleAccessList;
        }

        private void getSelectedRola()
        {
            string rolaId = "";
            DataGridViewCell cell = baseDataGridview.CurrentCell;
            int columnIndex = cell.ColumnIndex;
            if(columnIndex == SqlQueries.getRolaList_rolaIdIndex && cell.Value != null)
            {
                rolaId = cell.Value.ToString();
            }
            currentRola = currentApp.getRola(rolaId);
        }

    }
}
