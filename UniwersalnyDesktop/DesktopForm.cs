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
   
    /* 
     * główny formularz Desktopu otwierany po zalogowaniu w LoginForm
     */
    public partial class DesktopForm : Form
    {
        private QueryData desktopData;         //ap.ID_app=0, ap.appName=1, ap.appPath=2, ap.appDisplayName=3, au.Grant_app=4, ap.name_db=5
        private QueryData userData;         //login_user=0, windows_user=1,imie_user=2, nazwisko_user=3
        private string userPassword;

        //zmienne dotyczące rozmiarów i identyfikatorów tworzonych obiektów
        private int squareButtonsGroupboxWidth;
        private int squareButtonsGroupboxHeight;
        private int rectangularButtonsGroupboxHeight;
        private int groupboxID = 0;
        private int buttonID = 0;
        private int numberOfRectangularButtons;
        private int numberOfSquareButtonsInOneGroupbox = DesktopLayoutSettings.numberOfSquareButtonsInOneBlock;    //ustawienia domyślne, ale daję tu żeby program mógł automatycznie zmienić

        private string currentPath="";     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                           //dla DEBUGA ustawiony jest w metodzie ReadAllData

        public DesktopForm(QueryData userData, string userPassword, QueryData desktopData, string currentPath)
        {
            InitializeComponent();
            this.userData = userData;
            this.userPassword = userPassword;
            this.desktopData = desktopData;
            this.currentPath = currentPath;
            setupDesktop();            
        }

        private void setupDesktop()
        {
            this.Text = "Zalogowano jako " + userData.getQueryData()[0].ToList()[2] + " " + userData.getQueryData()[0].ToList()[3];
            generateDesktopLayout();
        }

        private void generateDesktopLayout()
        {
            int numberOfSquareButtonGroupboxes = 1;
            int estimatedDesktopHeigth = 100;
            numberOfSquareButtonsInOneGroupbox--;       //najpierw pomniejszam o jeden, żeby gdy wejdę do pętli to w pierwszej pętli liczyć dla liczby ustawionej przez użytkownika
            do {
                numberOfSquareButtonsInOneGroupbox++;
                numberOfSquareButtonGroupboxes = calculateNumberOfSquareButtonGroupboxes();
                estimatedDesktopHeigth = estimateDesktopHeigth(numberOfSquareButtonGroupboxes);
            }
            while (estimatedDesktopHeigth > DesktopLayoutSettings.maxDesktopHeigth);

            //liczbę wszystkich buttonów dzielę na liczbę buttonów kwadratowych w jednym bloku
            //resztę umieszczam jako prostokątne w osobnym bloku
            numberOfRectangularButtons = desktopData.getQueryData().Count - (numberOfSquareButtonGroupboxes * numberOfSquareButtonsInOneGroupbox);
            calculateGroupboxSize();
            for (int i=0; i<numberOfSquareButtonGroupboxes; i++)
            {
                generateOneGroupbox(DesktopLayoutSettings.GroupboxType.squareButtons);
            }
            generateOneGroupbox(DesktopLayoutSettings.GroupboxType.rectangularButtons);
            setDesktopFormSize();
        }

        //oszacowuję wysokość desktopu przy zdefiniowanej w DesktopLayoutSettings liczbie kwadratowych buttonów w rzędzie
        //jeżeli wysokość desktopu wychodzi większa niż założona, zwiększam liczbę buttonów w rzedzie aż uzyskam przyzwoity rozmiar
        private int estimateDesktopHeigth(int nrOfGroupBoxes)
        {
                return (nrOfGroupBoxes + 1) * DesktopLayoutSettings.squareButtonHeigth;      //dodaję 1 żeby szacunkowo uwzględnić poziome przyciski oraz odstępy między groupboxami
        }

        private int calculateNumberOfSquareButtonGroupboxes()
        {
            return desktopData.getQueryData().Count / numberOfSquareButtonsInOneGroupbox;
        }

        private void calculateGroupboxSize()
        {
            squareButtonsGroupboxWidth = numberOfSquareButtonsInOneGroupbox * (DesktopLayoutSettings.squareButtonWidth + DesktopLayoutSettings.horizontalButtonPadding) + DesktopLayoutSettings.horizontalButtonPadding;
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
                    int rectangularButtonWidth = numberOfSquareButtonsInOneGroupbox * (DesktopLayoutSettings.squareButtonWidth + DesktopLayoutSettings.horizontalButtonPadding) - DesktopLayoutSettings.horizontalButtonPadding;
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
                if(!checkUserAccess())
                {
                    button.Enabled = false;
                }
            }
            Controls.Add(button);
            groupBox.Controls.Add(button);
        }

        //sprawdza czy użytkownik ma uprawnienia do uruchomienia danej aplikacji
        private bool checkUserAccess()
        {
            int grantApp = int.Parse(desktopData.getQueryData()[buttonID][4].ToString());
            return (grantApp > 0);
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
                    for (int i = 0; i < numberOfSquareButtonsInOneGroupbox; i++)
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
            string userLogin = userData.getQueryData()[0].ToList()[0].ToString();
            string appLocation = currentPath + @"\..\" + appToRun;
            FileManipulator fm = new FileManipulator();
            fm.runProgram(appLocation, userLogin + " " + userPassword);

        }

        private void DesktopForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
