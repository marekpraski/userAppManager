using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
   
    public partial class EditableDatagridControl : UserControl
    {
        private DataGridHandler dgHandler = new DataGridHandler();

        private bool checkBoxColumnAdded = false;           //używam przy sprawdzaniu, że checkbox jest zaznaczany/odznaczany przez użytkownika
        private int checkBoxColumnIndex = 0;
        private bool undoButtonClicked = false;

        private DataGridCell changedCell;

        //1. define a delegate
        //2. define an event based on that delegate
        //3. raise the event

        public delegate void SaveButtonClickedEventHandler(object source, EditableDatagridControlEventArgs args);
        public event SaveButtonClickedEventHandler saveButtonClicked;



        public EditableDatagridControl()
        {
            InitializeComponent();
            initialSetup();
        }


        //3. raise the event, tzn tworzę metodę virtual, która najpierw sprawdza, czy event nie jest null, i jeżeli nie jest to wysyła odpowiednie dane przypisane do eventu
        //tzn obiekt, który wysyła ten event oraz argumenty
        protected virtual void OnSaveButtonClicked()
        {
            if (saveButtonClicked != null)
            {
                EditableDatagridControlEventArgs args = new EditableDatagridControlEventArgs();
                args.dgHandler = dgHandler;
                saveButtonClicked(this, args);
            }
        }


        private void initialSetup()
        {
            saveButton.Enabled = false;
            undoButton.Enabled = false;
        }



        #region Region - operacje na kontrolkach tej klasy (np. button disable/enable; datagrid column add, set header text)

        public void saveButtonDisable()
        {
            saveButton.Enabled = false;
        }


        public void saveButtonEnable()
        {
            saveButton.Enabled = true;
        }

        public void undoButtonDisable()
        {
            undoButton.Enabled = false;
        }


        public void undoButtonEnable()
        {
            undoButton.Enabled = true;
        }


        public void addDatagridRows(int numberOfRows)
        {
            for(int i = 0; i<numberOfRows; i++)
            {
                dataGridView1.Rows.Add();
            }
        }


        public void insertCheckboxColumn(int index, string headerText, int width)
        {
            DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn();
            chbCol.Width = width;
            chbCol.HeaderText = headerText;
            dataGridView1.Columns.Insert(index, chbCol);
            this.checkBoxColumnIndex = index;
            checkBoxColumnAdded = true;
        }

        public void hideDatagridColumn(int index)
        {
            dataGridView1.Columns[index].Visible = false;
        }


        public void disableDatagridColumn(int index)
        {
            dataGridView1.Columns[index].ReadOnly = true;
            dataGridView1.Columns[index].DefaultCellStyle.BackColor = Color.LightGray;
        }



        public void insertTextDatagridColumn(int index, string headerText, int width)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = headerText;
            col.Width = width;
            dataGridView1.Columns.Insert(index,col);
        }


        public void setHeaderText(int index, string text)
        {
            dataGridView1.Columns[index].HeaderText = text;
        }


        #endregion


        #region Region - dodawanie danych do datagridu, czyszczenie datagridu


        public void populateTextDatagridColumn(int columnIndex, List<string> columnValues, List<string> primaryKey = null)
        {
            for (int i = 0; i < columnValues.Count; i++)
            {
                dataGridView1.Rows[i].Cells[columnIndex].Value = columnValues[i];
                dataGridView1.Columns[columnIndex].ToolTipText = columnValues[i];
            }
        }


        public void clearTextDatagridColumn(int columnIndex)
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[columnIndex].Value = "";
            }
        }



        public void populateCheckboxColumn(int columnIndex, List<bool> checkBoxValues)
        {
            for (int i = 0; i < checkBoxValues.Count; i++)
            {
                dataGridView1.Rows[i].Cells[columnIndex].Value = checkBoxValues[i];
            }
        }


        public void clearCheckboxColumn(int index)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[index].Value = false;
            }
        }


        public void clearDatagrid()
        {
            dataGridView1.Rows.Clear();
        }

        #endregion




        #region Region - zdarzenia wywołane przez użytkownika


        public void SaveButton_Click(object sender, EventArgs e)
        {
            OnSaveButtonClicked();
        }



        private void UndoButton_Click(object sender, EventArgs e)
        {
            undoButtonClicked = true;
            DataGridCell recoveredCell = dgHandler.getLastCellChangedAndUndoChanges();

            object oldCellValue = recoveredCell.getCellValue(cellValueTypes.oldValue);
            int rowIndex = recoveredCell.getCellIndex(cellIndexTypes.rowIndex);
            int columnIndex = recoveredCell.getCellIndex(cellIndexTypes.columnIndex);
            dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = oldCellValue;

            changeCellTextColour(recoveredCell, Color.Black);

            if (!dgHandler.checkChangesExist())
            {
                undoButton.Enabled = false;
                saveButton.Enabled = false;
                undoButtonClicked = false;
            }
        }


        //stawiam warunek na naciśnięcie przycisku "cofnij", żeby w tym przypadku zdarzenia  cell beginEdit i endEdit się nie wywoływały
        //bez tego warunku wywołują się one w przypadku cofania zmian w checkboxie
        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!undoButtonClicked)
            {
                changedCell = new DataGridCell();
                object oldCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                changedCell.setCellValue(cellValueTypes.oldValue, oldCellValue);
            }
        }


        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!undoButtonClicked)
            {
                object newCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                //samo kliknięcie na komórkę zawierającą checkbox powoduje wywołanie zdarzenia cellBeginEdit i CellEndEdit, przy czym obie wartości są takie same (stan checkboxu się nie zmienił)
                //nie chcę w takiej sytuacji generować celki chengedCell
                if (changedCell.getCellValue(cellValueTypes.oldValue) != newCellValue)
                {
                    //changedCell została utworzona gdy użytkownik rozpoczął edycję, metoda dataGridView_CellBeginEdit
                    changedCell.setCellValue(cellValueTypes.newValue, newCellValue);

                    changedCell.setCellIndex(cellIndexTypes.rowIndex, e.RowIndex);
                    changedCell.setCellIndex(cellIndexTypes.columnIndex, e.ColumnIndex);


                    //jeżeli nie zdefiniuję typu celki w ten sposób jak poniżej, celka nie przechodzi weryfikacji i nie zostaje zmieniona
                    //jest to zaszłość z DBEditor, gdzie typ danych jest czytany z bazy i weryfikowany przez CellConverter
                    //co jest konieczne do automatyzacji zapisu do bazy.
                    //Tutaj wartość celki jest zawsze tekstem, czyli typ bazodanowy "varchar". Wyjątkiem jest typ "Boolean" w checkboxie,

                    changedCell.DataTypeName = "varchar"; //(queryData.getDataTypes()[e.ColumnIndex]);

                    if (dgHandler.addChangedCell(changedCell))
                    {
                        changeCellTextColour(changedCell, Color.Red);
                        undoButton.Enabled = true;
                        saveButton.Enabled = true;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = changedCell.getCellValue(cellValueTypes.oldValue);
                    }
            }
            else
            {
                changedCell.Dispose();
            }
        }
            undoButtonClicked = false;          //w przypadku cofania zmian w checkboxie wywoływane są zdarzenia cell begin edit i end edit
                                                //nie chcę tego stąd warunek wykluczający gdy undoButton jest kliknięty
                                                //reset tej zmiennej dopiero, gdy już sobie wywoła oba zdarzenia nie wchodząc do ich kodu
        }

        //
        // Region -ta metoda TYLKO wychwytywanie zaznaczania checkboxu w datagridzie
        //stawiam warunek na inne kolumny, żeby to zdarzenie nie było wywoływane w przypadku edycji innych kolumn
        //
        private void DataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (checkBoxColumnAdded)
            {                
                if (e.ColumnIndex == checkBoxColumnIndex && e.RowIndex > -1)
                {
                    dataGridView1.EndEdit();
                }
            }
        }

        //
        // koniec region  wychwytywanie zaznaczania checkboxu w datagridzie
        //

        #endregion



        public void resizeThisForm()
        {
            int datagridWidth = dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.None) + 5*dataGridView1.Columns.Count; //muszę dodać, bo suma szerokości jest za mała, pojawia się pasek przewijania
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Width = datagridWidth;
            int buttonHorizontalLocation = datagridWidth + 3 + 3;       //odstęp datagridu od brzegu formatki po lewej i od buttona po prawej
            saveButton.Location = new System.Drawing.Point(buttonHorizontalLocation, 50);
            undoButton.Location = new System.Drawing.Point(buttonHorizontalLocation, 91);
            this.Width = datagridWidth + 3 + 3 + undoButton.Width + 15;     //odstępy i padding
        }



        public void changeCellTextColour(DataGridCell cell, Color colour)
        {
            int rowIndex = cell.getCellIndex(cellIndexTypes.rowIndex);
            int columnIndex = cell.getCellIndex(cellIndexTypes.columnIndex);
            dataGridView1.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = colour;
        }

    }
}
