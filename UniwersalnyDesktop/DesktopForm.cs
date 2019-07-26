using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class DesktopForm : Form
    {
        private SqlConnection dbConnection;
        private QueryData desktopData;         //ap.ID_app=0, ap.appName=1, ap.appPath=2, ap.appDisplayName=3, au.Grant_app=4, ap.name_db=5
        private QueryData userData;         //login_user=0, windows_user=1,imie_user=2, nazwisko_user=3
        private string userName;
        private string userPassword;

        //zmienne dotyczące rozmiarów i identyfikatorów tworzonych obiektów
        private int squareButtonsGroupboxWidth;
        private int squareButtonsGroupboxHeight;
        private int rectangularButtonsGroupboxHeight;
        private int groupboxID = 0;
        private int buttonID = 0;
        private int numberOfRectangularButtons;

        private string currentPath="";     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                             //dla DEBUGA ustawiony jest w metodzie ReadAllData

        public DesktopForm(string userName, string userPassword)
        {
            InitializeComponent();
            this.userName = userName;
            this.userPassword = userPassword;
            setupDesktop();            
        }
        
        private void setupDesktop()
        {
            readAllData();
            this.Text = "Desktop. Zalogowano jako " + userData.getQueryData()[0].ToList()[2] + " " + userData.getQueryData()[0].ToList()[3];
            generateDesktopLayout();
        }

        private void generateDesktopLayout()
        {
            int numberOfSquareButtonBlocks = calculateNumberOfSquareButtonBlocks();

            //liczbę wszystkich buttonów dzielę na liczbę buttonów kwadratowych w jednym bloku
            //te które zostają umieszczam jako prostokątne w osobnym bloku
            numberOfRectangularButtons = desktopData.getQueryData().Count - (numberOfSquareButtonBlocks * DesktopLayoutSettings.numberOfSquareButtonsInOneBlock);
            calculateGroupboxSize();
            for (int i=0; i<numberOfSquareButtonBlocks; i++)
            {
                generateOneGroupbox(DesktopLayoutSettings.GroupboxType.squareButtons);
            }
            generateOneGroupbox(DesktopLayoutSettings.GroupboxType.rectangularButtons);
            setDesktopFormSize();
        }

        private void calculateGroupboxSize()
        {
            squareButtonsGroupboxWidth = DesktopLayoutSettings.numberOfSquareButtonsInOneBlock * (DesktopLayoutSettings.squareButtonWidth + DesktopLayoutSettings.horizontalButtonPadding) + DesktopLayoutSettings.horizontalButtonPadding;
            squareButtonsGroupboxHeight = DesktopLayoutSettings.squareButtonHeigth + 2 * DesktopLayoutSettings.verticalButtonPadding;
        }

        private void setDesktopFormSize()
        {
            int desktopWidth = 2 * DesktopLayoutSettings.horizontalGroupboxPadding + squareButtonsGroupboxWidth + DesktopLayoutSettings.desktopFormHorizontalPadding;
            int desktopHeigth = 2 * DesktopLayoutSettings.firstGroupboxVerticalLocation + squareButtonsGroupboxHeight * (groupboxID - 1) + DesktopLayoutSettings.desktopFormVerticalPadding + rectangularButtonsGroupboxHeight;
            this.Width = desktopWidth;
            this.Height = desktopHeigth;
        }
       
        private void generateOneButton(GroupBox groupBox, DesktopLayoutSettings.ButtonType buttonType, int buttonNr)
        {
            Button button = new Button();
            int buttonHorizontalLocation = DesktopLayoutSettings.horizontalButtonPadding;
            int buttonVerticalLocation = DesktopLayoutSettings.verticalButtonPadding;
            switch (buttonType)
            {
                case DesktopLayoutSettings.ButtonType.rectangular:
                    int rectangularButtonWidth = DesktopLayoutSettings.numberOfSquareButtonsInOneBlock * (DesktopLayoutSettings.squareButtonWidth + DesktopLayoutSettings.horizontalButtonPadding) - DesktopLayoutSettings.horizontalButtonPadding;
                    button.Size = new Size(rectangularButtonWidth, DesktopLayoutSettings.rectangularButtonHeigth);
                    button.Location = new Point(buttonHorizontalLocation, buttonVerticalLocation + buttonNr * (button.Height + DesktopLayoutSettings.verticalButtonPadding));
                    break;
                case DesktopLayoutSettings.ButtonType.square:
                    button.Size = new Size(DesktopLayoutSettings.squareButtonWidth, DesktopLayoutSettings.squareButtonHeigth);
                    button.Location = new Point(buttonHorizontalLocation + buttonNr * (button.Width + DesktopLayoutSettings.horizontalButtonPadding), buttonVerticalLocation);
                    break;
            }
            
            button.Click += new EventHandler(ButtonClick);
            button.Name = "button" + buttonID;
            string appName = desktopData.getQueryData()[buttonID][1].ToString();
            string programPath = desktopData.getQueryData()[buttonID][2].ToString();
            string buttonProgram = programPath + @"\" + appName + ".exe";       //program uruchamiany z tego przycisku
            button.Tag = buttonProgram;
            if (appName.Equals(""))
            {
                button.Text = "button" + buttonID;
                button.Enabled = false;
            }
            else
            {
                string appDisplayName = desktopData.getQueryData()[buttonID][3].ToString();
                button.Text = appDisplayName;
            }
            Controls.Add(button);
            groupBox.Controls.Add(button);
        }

        private void generateOneGroupbox(DesktopLayoutSettings.GroupboxType groupboxType)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Name = "groupBox" + groupboxID;
            int groupboxVerticalLocation = DesktopLayoutSettings.firstGroupboxVerticalLocation + DesktopLayoutSettings.verticalGroupboxPadding + groupboxID * squareButtonsGroupboxHeight;
            groupBox.Location = new System.Drawing.Point(DesktopLayoutSettings.horizontalGroupboxPadding, groupboxVerticalLocation);

            switch (groupboxType)
            {
                case DesktopLayoutSettings.GroupboxType.squareButtons:
                    groupBox.Size = new System.Drawing.Size(squareButtonsGroupboxWidth, squareButtonsGroupboxHeight);
                    for (int i = 0; i < DesktopLayoutSettings.numberOfSquareButtonsInOneBlock; i++)
                    {
                        generateOneButton(groupBox, DesktopLayoutSettings.ButtonType.square, i);
                        buttonID++;
                    }
                    break;
                case DesktopLayoutSettings.GroupboxType.rectangularButtons:
                    rectangularButtonsGroupboxHeight = numberOfRectangularButtons * (DesktopLayoutSettings.rectangularButtonHeigth + DesktopLayoutSettings.verticalButtonPadding);
                    groupBox.Size = new System.Drawing.Size(squareButtonsGroupboxWidth, rectangularButtonsGroupboxHeight);
                    for (int i = 0; i < numberOfRectangularButtons; i++)
                    {
                        generateOneButton(groupBox, DesktopLayoutSettings.ButtonType.rectangular, i);
                        buttonID++;
                    }
                    break;
            }

            this.Controls.Add(groupBox);
            groupboxID++;
        }
       
        private void ButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string appToRun = button.Tag.ToString();
                runApp(appToRun);
            }
        }

        private void runApp(string appToRun)
        {
            string appLocation = currentPath + @"\..\" + appToRun;
            FileManipulator fm = new FileManipulator();
            fm.runProgram(appLocation, userName + " " + userPassword);

    }

        private int calculateNumberOfSquareButtonBlocks()
        {
            return desktopData.getQueryData().Count / DesktopLayoutSettings.numberOfSquareButtonsInOneBlock;
        }

        private void readAllData()
        {
            DBConnector connector = new DBConnector(userName, userPassword);
#if DEBUG
            currentPath = @"C:\SMD\SoftMineDesktop";
#else
            currentPath = connector.currentPath;
#endif
            connector.validateConfigFile();
            dbConnection = connector.getDBConnection(ConnectionSources.serverNameInFile, ConnectionTypes.sqlAuthorisation);
            DBReader reader = new DBReader(dbConnection);
            string query = ProgramSettings.desktopAppDataQueryTemplate + "'" + userName + "'";
            desktopData = reader.readFromDB(query);
            query = ProgramSettings.desktopUserDataQueryTemplate + "'" + userName + "'";
            userData = reader.readFromDB(query);
        }
    }
}
