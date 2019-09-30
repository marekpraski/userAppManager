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
    public partial class DBRolaEditorForm : DBEditorForm
    {

        //private DataGridHandler dg1Handler;
        //private FormFormatter formatter;
        //private DataGridCell changedCell;
        //private DBConnector connector;
        //private bool configFileValidated;
        //private string sqlQuery;
        //private string tableName;
        //private QueryData queryData;
        //private SqlConnection dbConnection = new SqlConnection();

        //private List<object[]> dbData;

        //private int datagridRowIndex = 0;
        //private int rowsLoaded = 0;

        private ListViewItem currentSelectedRola = null;

        private int appModuleListViewPadding = 30;


        public DBRolaEditorForm(SqlConnection dbConnection, string sqlQuery)
            : base(dbConnection, sqlQuery)
        {
            //this.dbConnection = dbConnection;
            //this.sqlQuery = sqlQuery;
            InitializeComponent();
            setRolaEditorFormLayout();
        }

        private void setRolaEditorFormLayout()
        {
            int baseFormWidth = formatter.calculateBDEditorFormWidth();
            appModuleListView.Location = new System.Drawing.Point(baseFormWidth, 13);

            this.Width = baseFormWidth + appModuleListView.Width + appModuleListViewPadding;
        }

        //private void getAppModules();


        //private void populateModuleListView()
        //{
        //    if (currentSelectedRola != null)
        //    {
        //        appModuleListView.Enabled = true;

        //        //resetuję ustawienia widoku listy modułów aplikacji
        //        //i wypełniam listę  uprawnieniami do modułów właściwymi dla zaznaczonej roli
        //        resetAppModuleListView();

        //        App app = null;
        //        appModuleDictionary.TryGetValue(currentSelectedRola.Name, out app);

        //        List<string> rolaIdList = app.rolaIdList;
        //        if (rolaIdList.Count > 0)
        //        {
        //            ListViewItem[] roleRange = new ListViewItem[rolaIdList.Count];
        //            Rola rola = null;
        //            int i = 0;

        //            foreach (string rolaId in rolaIdList)
        //            {
        //                rolaDict.TryGetValue(rolaId, out rola);
        //                string name = rola.name;
        //                string descr = rola.description;
        //                ListViewItem item = new ListViewItem(name);
        //                item.Name = rolaId;
        //                item.SubItems.Add(descr);
        //                roleRange[i] = item;
        //                i++;
        //            }
        //            rolaListView.Items.AddRange(roleRange);

        //            //jeżeli zaznaczony użytkownik ma uprawnienia do zaznaczonej aplikacji, wówczas zaznacza się rola tego użytkownika w tej aplikacji
        //            checkRolaCheckbox();
        //        }
        //    }
        //}
    }
}
