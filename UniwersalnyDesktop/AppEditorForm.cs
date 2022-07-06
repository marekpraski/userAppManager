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
        private DataGridHandler dgHandler;

        private int moduleDatagridPadding = 30;

        App selectedApp = null;                          //aplikacja aktualnie zaznaczona w datagridzie

        int moduleIdColumnIndex = 0;
        int moduleNamesColumnIndex = 1;



        public AppEditorForm(SqlConnection dbConnection, string sqlQuery, Dictionary<string, App> appDictionary)
            : base(dbConnection, sqlQuery)
        {
            this.appDictionary = appDictionary;
            InitializeComponent();
            moduleDatagrid.saveButtonClicked += this.moduleDatagrid_saveButtonClicked;
            setUpThisForm();
        }


        private void setUpThisForm()
        {
            //na starcie wyświetlam moduły aplikacji pierwszej od góry
            baseDatagrid.CurrentCell = baseDatagrid.Rows[0].Cells[0];


            //datagrid w formatce nie ma żadnych kolumn na starcie, dodaję je; kolejność dodawania jest istotna
            //podczas aktualizacji odwołuję się do indeksu kolumny
            //nie ma to znaczenia, ale przyjmuję, że pierwsza kolumna zawiera klucz główny, służący do zapisywania zmian w bazie
            moduleDatagrid.insertTextDatagridColumn(moduleIdColumnIndex, "Id modułu", 80);
            moduleDatagrid.insertTextDatagridColumn(moduleNamesColumnIndex, "moduły aplikacji", 250);
            //kolumna zawierająca klucz główny będzie ukryta
            moduleDatagrid.disableDatagridColumn(moduleIdColumnIndex);

            populateDatagrid();

            resizeThisForm();
        }

        private void populateDatagrid()
        {
            getSelectedApp();

            if (selectedApp != null)
            {

                moduleDatagrid.setHeaderText(moduleNamesColumnIndex, "moduły " + selectedApp.displayName);

                //po dodaniu kolumn dodaję wiersze
                moduleDatagrid.addDatagridRows(selectedApp.getModuleNameList().Count);

                //i je wypełniam
                moduleDatagrid.populateTextDatagridColumn(moduleIdColumnIndex, selectedApp.moduleIdList);
                moduleDatagrid.populateTextDatagridColumn(moduleNamesColumnIndex, selectedApp.getModuleNameList());
            }
            else
            {
                moduleDatagrid.setHeaderText(moduleNamesColumnIndex, "moduły aplikacji");
            }
        }


        private void getSelectedApp()
        {
            DataGridViewCell cell = baseDatagrid.CurrentCell;
            int columnIndex = cell.ColumnIndex;
            if (columnIndex == 0 && cell.Value != null) //położenie ID aplikacji
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
            int baseFormWidth = formatter.calculateBaseFormWidth(baseDatagrid);
            moduleDatagrid.Location = new System.Drawing.Point(baseFormWidth, 13);
            moduleDatagrid.resizeThisForm();

            this.Width = baseFormWidth + moduleDatagrid.Width + moduleDatagridPadding;
        }




        #region Region - zdarzenia wywołane przez użytkownika

        
        protected void moduleDatagrid_saveButtonClicked(object sender, EditableDatagridControlEventArgs e)
        {
            this.dgHandler = e.dgHandler;
            saveChanges();
        }



        protected override void BaseDatagridClickedEvent()
        {

            //czyszczę stare wypełnienia
            moduleDatagrid.clearDatagrid();

            //wypełniam datagrid danymi nowej aplikacji
            populateDatagrid();
        }


        #endregion



        #region Region - zapisywanie zmian

        //--zapisywanie zmian w tej formatce
        //
        //datagrid po lewej
        //
        //1. zmiana nazwy lub opisu aplikacji - obsłużone funkcjami dziedziczonymi
        //
        //2. dodawanie aplikacji
        //insert into app_list(name_app, path_app, name_db, srod_app, descr_app, variant, appName, appPath, appDisplayName) 
        //values(@nameApp, @pathApp, @nameDb, @srodApp, @descrApp, @variant, @appName, @appPath, @appDisplayName)
        //
        //3. usuwanie aplikacji
        //zabudować sprawdzenie, czy do aplikacji przypisane są jekieś role i/lub uprawnienia i jeżeli tak to uniemożliwić usunięcie
        //select count(*) as numberOfEntries from rola_app where ID_app =
        //select count(*) as numberOfEntries from app_users  where ID_app =
        //select count(*) as numberOfEntries from app_upr where ID_app =
        //gdy wynik wszystkich powyższych pytań jest 0
        //delete from app_list where ID_app = 
        //
        //
        //datagrid po prawej
        //
        //--1. edycja nazw / opisów ról aplikacji
        //--update rola_app set name_rola = @newRolaName, descr_rola = @newRolaDescr where ID_rola = 
        //
        //--2. dodanie roli
        //--insert into rola_app values(ID_app, name_rola, descr_rola) values(@idApp, @rolaNewName, @rolaNewDescr)
        //
        //--3. usuwanie roli
        //zabudować sprawdzenie, czy do roli przypisane są jekieś uprawnienia i jeżeli tak to uniemożliwić usunięcie
        //select count(*) as numberOfEntries from rola_upr where ID_rola = 
        //gdy wynik powyższego pytania jest 0
        //delete from rola_app where ID_rola = @idRola


        private void saveChanges()
        {
            MyMessageBox.display("naciśnięty przycisk zapisu");
            
            //DBWriter writer = new DBWriter(dbConnection);

            //string query;
            //while (dgHandler.checkChangesExist())
            //{
            //    DataGridCell cell = dgHandler.getLastCellChangedAndUndoChanges();
            //    query = generateUpdateQuery(cell);
            //    writer.writeToDB(query);
            //    moduleDatagrid.changeCellTextColour(cell, Color.Black);
            //}
            //blokuję przyciski zapisu i cofania, bo po zapisaniu zmian już nie ma czego zapisać ani cofnąć
            moduleDatagrid.undoButtonDisable();
            moduleDatagrid.saveButtonDisable();
        }


        private string generateUpdateQuery(DataGridCell cell)
        {
            //string newModuleName = cell.getCellValue(cellValueTypes.newValue).ToString();
            //string moduleId;



            //int columnIndex = cell.getCellIndex(cellIndexTypes.columnIndex);
            //CellConverter cellConverter = new CellConverter();
            //string columnName = queryData.getHeaders()[columnIndex];
            //string primaryKeyColumnName = queryData.getHeaders()[0];    //kluczem głównym MUSI być pierwsza kolumna
            //object primaryKey = dgHandler.getCellPrimaryKey(cell);
            //string newValue = cellConverter.getConvertedValue(ref cell);
            //if (newValue == null)
            //{
            //    return "update " + tableName + " set " + columnName + "= null" + " where " + primaryKeyColumnName + "='" + primaryKey.ToString() + "'";
            //}
            //return "update " + tableName + " set " + columnName + "=" + cellConverter.getConvertedValue(ref cell) + " where " + primaryKeyColumnName + "='" + primaryKey.ToString() + "'";
            return null;
        }

        #endregion

    }
}
