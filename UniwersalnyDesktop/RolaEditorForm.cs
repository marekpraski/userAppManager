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
        private Rola oldRola;

        private int moduleNamesColumnIndex = 0;
        private int checkboxColumnIndex = 1;
        private int accessRigthsColumnIndex = 2;


        public RolaEditorForm(SqlConnection dbConnection, string sqlQuery, App app)
            : base(dbConnection, sqlQuery)
        {
            this.currentApp = app;
            InitializeComponent();
            moduleDatagrid.saveButtonClicked += moduleDatagrid_saveButtonClicked;
            setUpThisForm();
        }


        private void setUpThisForm()
        {
            this.Text = "Edytor ról " + currentApp.displayName;

            //ukrywam kolumny id i nazwy aplikacji, bo i tak nie mogą być do edycji a wartości w nich niczego nie wnoszą
            base.baseDatagrid.Columns[3].Visible = false;
            base.baseDatagrid.Columns[4].Visible = false;
            base.baseDatagrid.ReadOnly = true;

            //na starcie okna wyświetlam moduły roli pierwszej od góry
            baseDatagrid.CurrentCell = baseDatagrid.Rows[0].Cells[0];
            getSelectedRola();

            //datagrid w formatce nie ma żadnych kolumn na starcie, dodaję je; kolejność dodawania jest istotna
            //podczas aktualizacji odwołuję się do indeksu kolumny


            //muszę dodać chociaż jedną kolumnę zanim dodam wiersze
            //kolumna z checkboxami NIE MOŻE BYĆ PIERWSZA bo diabli biorą zdarzenia
            moduleDatagrid.insertTextDatagridColumn(moduleNamesColumnIndex, "moduły " + currentApp.displayName, 250);
            moduleDatagrid.insertCheckboxColumn(checkboxColumnIndex, "dostęp", 50);
            moduleDatagrid.insertTextDatagridColumn(accessRigthsColumnIndex, "uprawnienia", 80);

            moduleDatagrid.addDatagridRows(currentApp.getModuleNameList().Count);              

            //po dodaniu, kolumny wypełniam danymi
            moduleDatagrid.populateTextDatagridColumn(moduleNamesColumnIndex, currentApp.getModuleNameList());     
            moduleDatagrid.disableDatagridColumn(moduleNamesColumnIndex);                                        //kolumna z nazwami modułów jest tylko do odczytu, edycja modułów jest w innym interfejsie
            moduleDatagrid.populateCheckboxColumn(checkboxColumnIndex, getModuleAccessList());
            moduleDatagrid.populateTextDatagridColumn(accessRigthsColumnIndex, getModuleAccessRights());          

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
            formatter.formatDatagridWithHiddenColumns(ref base.baseDatagrid);
            base.changeThisFormLayout();
            int baseFormWidth = formatter.calculateBaseFormWidth(base.baseDatagrid);
            moduleDatagrid.Location = new System.Drawing.Point(baseFormWidth, 13);
            moduleDatagrid.resizeThisForm();

            this.Width = baseFormWidth + moduleDatagrid.Width + moduleDatagridPadding;
        }


        #region Region - zdarzenia wywołane przez użytkownika


        //nadpisuję metodę w klasie bazowej wywoływaną przez naciśnięcie dowolnej celki w datagridzie klasy bazowej
        protected override void BaseDatagridClickedEvent()
        {
            getSelectedRola();

            if (currentRola != oldRola)
            {
                //czyszczę stare wypełnienia
                clearAccessRightsColumn();
                uncheckModules();

                //wypełniam datagrid danymi nowej roli            
                showAccessRights();
                checkModules();
            }
        }


        //naciśnięcie na przycisk "Zapisz" po prawej stronie okna (tzn. w kontrolce EditableDatagridControl)
        private void moduleDatagrid_saveButtonClicked(object source, EditableDatagridControlEventArgs args)
        {
            MyMessageBox.display("zapisuję");
        }

        #endregion


        private void showAccessRights()
        {
            if (currentRola != null)
            {
                moduleDatagrid.populateTextDatagridColumn(accessRigthsColumnIndex, getModuleAccessRights());
            }
        }

        private void uncheckModules()
        {
            moduleDatagrid.clearCheckboxColumn(checkboxColumnIndex);
        }

        private void checkModules()
        {
            if (currentRola != null)
            {
                moduleDatagrid.populateCheckboxColumn(checkboxColumnIndex, getModuleAccessList());
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
            DataGridViewCell cell = baseDatagrid.CurrentCell;
            int columnIndex = cell.ColumnIndex;
            if(columnIndex == 0 && cell.Value != null)  //położenie Id roli aplikacji
            {
                rolaId = cell.Value.ToString();
                oldRola = currentRola;
                currentRola = currentApp.getRola(rolaId);
            }
        }

        #region Region - zapisywanie zmian

        //--zapisywanie zmian w tej formatce
        //
        //datagrid po lewej
        //
        //tylko do odczytu
        //
        //
        //datagrid po prawej
        //
        //edycja nazw ról aplikacji oraz dodawanie i usuwanie ról jest zablokowane
        //
        //--1. sytuacja gdy rola miała dostęp do modułu, zmieniony został zakres dostępu(np z 1 na 2)
        //--update rola_upr set Grant_app = @newGrantAppValue where ID_rola = @idRola and ID_mod = @idModule
        //
        //--2. sytuacja, gdy rola miała dostęp do modułu, ale dostęp został jej odebrany
        //--delete from rola_upr where ID_rola = @idRola and ID_mod = @idModule
        //
        //--3. sytuacja, gdy rola nie miała dostępu do mudułu
        //--insert into rola_upr(ID_rola, ID_mod, Grant_app) values(@idRola, @idModule, @newGrantAppValue)

        #endregion

    }
}
