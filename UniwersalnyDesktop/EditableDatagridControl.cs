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
        private bool undoButtonClicked = false;

        private bool bazeDatagridCreated = false;           //nie można zacząć dodawać kolumn lub wpisywać wartości do kolumn bez utworzenia bazowego datagrida

        private DataGridCell changedCell;


        public EditableDatagridControl()
        {
            InitializeComponent();
            initialSetup();
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


        public void addDatagridRows(int numberOfRows)
        {
            for(int i = 0; i<numberOfRows; i++)
            {
                dataGridView1.Rows.Add();
            }
        }


        public void addCheckboxColumn()
        {
            DataGridViewCheckBoxColumn chbCol = new DataGridViewCheckBoxColumn();
            chbCol.Width = 20;
            dataGridView1.Columns.Insert(0, chbCol);
        }



        public void addTextDatagridColumn(string headerText, int width)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = headerText;
            col.Width = width;
            dataGridView1.Columns.Add(col);
        }


        public void setHeaderText(int index, string text)
        {
            dataGridView1.Columns[index].HeaderText = text;
        }


        #endregion


        #region Region - dodawanie danych do datagridu, czyszczenie datagridu


        public void populateTextDatagridColumn(int columnIndex, List<string> columnValues)
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



        public void populateCheckboxColumn(List<bool> checkBoxValues)
        {
            for (int i = 0; i < checkBoxValues.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = checkBoxValues[i];
            }
        }


        public void clearCheckboxColumn()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[0].Value = false;
            }
        }


        public void clearDatagrid()
        {
            dataGridView1.Rows.Clear();
        }

        #endregion




        #region Region - zdarzenia wywołane przez użytkownika


        private void SaveButton_Click(object sender, EventArgs e)
        {
            MyMessageBox.display("save button clicked");
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
            }
        }


        //stawiam warunek na naciśnięcie przycisku "cofnij", żeby w tym przypadku zdarzenia  cell beginEdit i endEdit się nie wywoływały
        //bez tego warunku wywołują się one w przypadku cofania zmian w checkboxie
        protected void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!undoButtonClicked)
            {
                changedCell = new DataGridCell();
                object oldCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                changedCell.setCellValue(cellValueTypes.oldValue, oldCellValue);
            }
        }


        protected void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!undoButtonClicked)
            {
                object newCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

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
            undoButtonClicked = false;          //w przypadku cofania zmian w checkboxie wywoływane są zdarzenia cell begin edit i end edit
                                                //nie chcę tego stąd warunek wykluczający gdy undoButton jest kliknięty
                                                //reset tej zmiennej dopiero, gdy już sobie wywoła oba zdarzenia nie wchodząc do ich kodu
        }

        //
        // Region -te dwie metody TYLKO wychwytywanie zaznaczania checkboxu w datagridzie
        //checkboxy są zawsze tylko pierwszą kolumną i tak stawiam warunek
        //stawiam warunek na inne kolumny, żeby te zdarzenia nie były wywoływane w przypadku edycji innych kolumn
        //

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (checkBoxColumnAdded)
            {                
                if (e.ColumnIndex == 0 && e.RowIndex > -1)
                {
                    bool selected = (bool)dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                }
            }
        }

        private void DataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (checkBoxColumnAdded)
            {                
                if (e.ColumnIndex == 0 && e.RowIndex > -1)
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



        private void changeCellTextColour(DataGridCell cell, Color colour)
        {
            int rowIndex = cell.getCellIndex(cellIndexTypes.rowIndex);
            int columnIndex = cell.getCellIndex(cellIndexTypes.columnIndex);
            dataGridView1.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = colour;
        }

    }
}
