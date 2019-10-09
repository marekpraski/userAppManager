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
    public  partial class DBEditorForm : Form
    {

        private DataGridHandler dg1Handler = new DataGridHandler();
        private DataGridCell changedCell;
        private DBConnector connector;
        private bool configFileValidated;
        private string sqlQuery;
        private string tableName;
        private List<object[]> dbData;
        private int datagridRowIndex = 0;
        private int rowsLoaded = 0;

        protected DBEditorFormatter formatter = new DBEditorFormatter();
        protected QueryData queryData;
        protected SqlConnection dbConnection = null;

        

        //konstruktor bez parametrów musi być nawet jeżeli się go nie używa bo inaczej klasa dziedzicząca wywala błąd
        private DBEditorForm()
        {
            InitializeComponent();;
        }


        public DBEditorForm( SqlConnection dbConnection, string sqlQuery)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.sqlQuery = sqlQuery;
            setUpDatagrid();
        }



        #region Region - zdarzenia na interakcję z użytkownikiem



        protected void DBEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dg1Handler.checkChangesExist())
            {
                if (MyMessageBox.display("Czy zapisać zmiany?", MessageBoxType.YesNo) == MyMessageBoxResults.Yes)
                {
                    //zaimplementować 
 
                }
            }
        }


        protected void UndoButton_Click(object sender, EventArgs e)
        {
            DataGridCell recoveredCell = dg1Handler.getLastCellChangedAndUndoChanges();

            object oldCellValue = recoveredCell.getCellValue(cellValueTypes.oldValue);
            int rowIndex = recoveredCell.getCellIndex(cellIndexTypes.rowIndex);
            int columnIndex = recoveredCell.getCellIndex(cellIndexTypes.columnIndex);
            baseDataGridview.Rows[rowIndex].Cells[columnIndex].Value = oldCellValue;

            changeCellTextColour(recoveredCell, Color.Black);

            if (!dg1Handler.checkChangesExist())
            {
                undoButton.Enabled = false;
                saveButton.Enabled = false;
            }
        }


        protected void saveButton_Click(object sender, EventArgs e)
        {
            DBWriter writer = new DBWriter(dbConnection);
            DBConnector connector = new DBConnector();
            tableName = connector.getTableName(sqlQuery.ToLower());

            string query;
            while (dg1Handler.checkChangesExist())
            {
                DataGridCell cell = dg1Handler.getLastCellChangedAndUndoChanges();
                query = generateUpdateQuery(cell);
                writer.writeToDB(query);
                changeCellTextColour(cell, Color.Black);
            }
            //blokuję przyciski zapisu i cofania, bo po zapisaniu zmian już nie ma czego zapisać ani cofnąć
            undoButton.Enabled = false;
            saveButton.Enabled = false;
        }


        protected void LoadNextButton_Click(object sender, EventArgs e)
        {
            if (dbData != null && datagridRowIndex <= dbData.Count)
            {
                loadRowPacket();
            }
        }


        protected void PomocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pomoc = "(*) pierwsza kolumna wyników kwerendy MUSI zawierać ID (lub inny klucz główny)" +
                "\r\nidentyfikujący jednoznacznie wiersz wyników kwerendy zasilającej datagrid";
            MyMessageBox.display(pomoc, MessageBoxType.Information);
        }



        protected void baseDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            changedCell = new DataGridCell();
            object oldCellValue = baseDataGridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            changedCell.setCellValue(cellValueTypes.oldValue, oldCellValue);
        }


        protected void baseDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object newCellValue = baseDataGridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            //changedCell została utworzona gdy użytkownik rozpoczął edycję, metoda dataGridView1_CellBeginEdit
            changedCell.setCellValue(cellValueTypes.newValue, newCellValue);

            changedCell.setCellIndex(cellIndexTypes.rowIndex, e.RowIndex);
            changedCell.setCellIndex(cellIndexTypes.columnIndex, e.ColumnIndex);

            changedCell.DataTypeName = (queryData.getDataTypes()[e.ColumnIndex]);

            if (dg1Handler.addChangedCell(changedCell))
            {
                changeCellTextColour(changedCell, Color.Red);
                undoButton.Enabled = true;
                saveButton.Enabled = true;
            }
            else
            {
                baseDataGridview.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = changedCell.getCellValue(cellValueTypes.oldValue);
            }
        }


        private void baseDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MyMessageBox.display("row header mouse clicked");
        }

        protected void baseDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BaseDatagridClickedEvent();
        }

        protected virtual void BaseDatagridClickedEvent()
        {
            MyMessageBox.display("db editor główna formatka");
        }



    #endregion



    protected void setUpDatagrid()
        {

            //pierwsza kolumna nie jest do edycji, w niej musi być primaryKey;
            baseDataGridview.Columns[0].ReadOnly = true;
            baseDataGridview.Columns[0].DefaultCellStyle.BackColor = Color.LightGray;

            DBReader reader = new DBReader(dbConnection);
            queryData = reader.readFromDB(sqlQuery);

            //jeżeli kwerenda błędna to nie zwróci wyników
            //przypadki błędnej kwerendy obsługiwane są przez DBReader
            if (queryData.getHeaders().Count != 0)
            {
                dbData = queryData.getQueryData();
                List<string> columnHeaders = queryData.getHeaders();

                //dopasowuję formatkę do wyników nowej kwerendy

                changeMainFormLayout(columnHeaders.Count, ref baseDataGridview);

                //nazywam nagłówki
                for (int i = 0; i < queryData.getHeaders().Count; i++)
                {
                    baseDataGridview.Columns[i].HeaderText = columnHeaders[i];
                }
            }
            if (dbData != null)
            {
                loadRowPacket();

                if (rowsLoaded < dbData.Count)
                {
                    int rowsRemaining = dbData.Count - rowsLoaded;

                    loadNextButton.Visible = true;
                    loadNextButton.Enabled = true;
                    remainingRowsLabel.Visible = true;
                    remainingRowsLabel.Text = "zostało " + rowsRemaining;

                    if (rowsRemaining > ProgramSettings.numberOfRowsToLoad)
                    {
                        loadNextButton.Text = "+" + ProgramSettings.numberOfRowsToLoad;

                    }
                }
            }
        }



        private void loadRowPacket()
        {
            for (int i = datagridRowIndex; i < ProgramSettings.numberOfRowsToLoad + rowsLoaded; i++)
            {
                if (i < dbData.Count)
                {
                    object[] row = dbData[i];
                    baseDataGridview.Rows.Add(row);
                    object primaryKey = row[0];
                    dg1Handler.addDataGridIndex(i, primaryKey);
                }
            }


            if (baseDataGridview.AllowUserToAddRows)
            {
                datagridRowIndex = baseDataGridview.Rows.Count - 1;   //gdy użytkownik ma możliwość dodawania wierszy w datagridzie, datagrid posiada dodatkowo jeden pusty wiersz na końcu
            }
            else
            {
                datagridRowIndex = baseDataGridview.Rows.Count;
            }
            rowsLoaded = datagridRowIndex;
            int rowsRemaining = dbData.Count - rowsLoaded;

            if ((rowsRemaining) < ProgramSettings.numberOfRowsToLoad)
            {
                loadNextButton.Text = "+" + rowsRemaining;
            }

            if (rowsLoaded == dbData.Count)
            {
                loadNextButton.Enabled = false;
                loadNextButton.Visible = false;
                remainingRowsLabel.Visible = false;
            }
            else
            {
                remainingRowsLabel.Text = "zostało " + rowsRemaining;
            }
        }


        //przyjmuje liczbę nagłówków z kwerendy oraz datagrid, w którym trzeba dopasować liczbę kolumn
        private void changeMainFormLayout(int numberOfHeaders, ref DataGridView dataGrid)
        {
            List<int> colWidths = queryData.getColumnWidths(dataGrid.Font, 50);         //szerokości kolummn datagridu z danych z kwerendy
            formatter.formatDatagrid(ref dataGrid, numberOfHeaders, colWidths);
            formatter.changeSaveButtonLocation(ref saveButton);
            formatter.changeUndoButtonLocation(ref undoButton);
            formatter.changeLoadNextButtonLocation(loadNextButton);
            formatter.changeRemainingRowsLabelLocation(remainingRowsLabel);
            this.Width = formatter.calculateBDEditorFormWidth();
        }


        private string generateUpdateQuery(DataGridCell cell)
        {
            int columnIndex = cell.getCellIndex(cellIndexTypes.columnIndex);
            CellConverter cellConverter = new CellConverter();
            string columnName = queryData.getHeaders()[columnIndex];
            string primaryKeyColumnName = queryData.getHeaders()[0];    //kluczem głównym MUSI być pierwsza kolumna
            object primaryKey = dg1Handler.getCellPrimaryKey(cell);
            string newValue = cellConverter.getConvertedValue(ref cell);
            if (newValue == null)
            {
                return "update " + tableName + " set " + columnName + "= null" + " where " + primaryKeyColumnName + "='" + primaryKey.ToString() + "'";
            }
            return "update " + tableName + " set " + columnName + "=" + cellConverter.getConvertedValue(ref cell) + " where " + primaryKeyColumnName + "='" + primaryKey.ToString() + "'"; ;
        }



        private void changeCellTextColour(DataGridCell cell, Color colour)
        {
            int rowIndex = cell.getCellIndex(cellIndexTypes.rowIndex);
            int columnIndex = cell.getCellIndex(cellIndexTypes.columnIndex);
            baseDataGridview.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = colour;
        }

    }
}
