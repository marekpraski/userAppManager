using DatabaseInterface;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
   
    /* 
     * główny formularz Desktopu otwierany po zalogowaniu w LoginForm
     */
    public partial class DesktopForm : Form
    {
        private DBReader dbReader;

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

        public DesktopForm(QueryData userData, string userPassword, DBReader dbReader, string currentPath)
        {            
            this.userData = userData;
            this.userPassword = userPassword;
            this.dbReader = dbReader;
            this.currentPath = currentPath;
            readDesktopData();
            if (desktopData.getQueryData().Count > 0)
            {
                InitializeComponent();
                setupDesktop();
            }
            // niczego nie przeczytał bo użytkownik nie ma uprawnień do żadnych programów
            // w chwili obecnej z tą kwerendą która jest zdefiniowana w ProgramSettings nie zadziała, bo wyświetlam wszystkie programy niezależnie od dostępu użytkownika
            //trzeba by filtrować np. po Grant_app
            else
            {
                MyMessageBox.display("użytkownik nie ma dostępu do żadnych programów", MessageBoxType.Error);
                this.Dispose();
            }         
        }

        #region Region - interakcja z użytkownikiem

        private void ButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string appToRun = button.Tag.ToString();
                runApp(appToRun);
            }
        }


        private void DesktopForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }


        #endregion

        #region wybór opcji z paska menu

        private void zmienHasloMenuItem_Click(object sender, EventArgs e)
        {
            Form formChangePass = new PasswordChanger();
            formChangePass.ShowDialog();
        }
        #endregion

        private void readDesktopData()
        {
            string userLogin = userData.getDataValue(0, "login_user").ToString();
            string query = @"select ap.ID_app, ap.name_app, ap.path_app, ap.show_name, au.Grant_app, ap.name_db, ap.runFromDesktop from [dbo].[app_list] as ap 
                                                    inner join app_users as au on ap.ID_app = au.ID_app 
                                                    inner join users_list as ul on ul.ID_user = au.ID_user 
                                                    where ap.name_db is not null and srod_app = 'Windows' and ap.runFromDesktop = 1 and ul.login_user = '" + userLogin + "'";
            desktopData = dbReader.readFromDB(query);
        }

        private void setupDesktop()
        {
            this.Text = "Zalogowano jako " + userData.getDataValue(0, "imie_user").ToString() + " " + userData.getDataValue(0, "nazwisko_user").ToString();
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
            while (estimatedDesktopHeigth > DesktopLayoutSettings.maxTabCtrlHeigth);

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
            int tabCtrlWidth = 2 * DesktopLayoutSettings.horizontalGroupboxPadding + squareButtonsGroupboxWidth + DesktopLayoutSettings.tabCtrlHorizontalPadding;
            int tabCtrlHeight = 2 * DesktopLayoutSettings.firstGroupboxVerticalLocation + squareButtonsGroupboxHeight * (groupboxID - 1) + DesktopLayoutSettings.tabCtrlVerticalPadding + rectangularButtonsGroupboxHeight;
            this.tabControl1.Width = tabCtrlWidth;
            this.tabControl1.Height = tabCtrlHeight;
            this.Width = tabControl1.Width + 40;
            this.Height = tabControl1.Height + 80;
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
            Controls.Add(button);           //czemu?
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

            this.tabSoftmine.Controls.Add(groupBox);
            groupboxID++;
        }
       

        private void runApp(string appToRun)
        {
            string userLogin = userData.getQueryData()[0].ToList()[0].ToString();
            string appLocation = currentPath + @"\..\" + appToRun;
            FileManipulator fm = new FileManipulator();
            fm.runProgram(appLocation, userLogin + " " + userPassword);

        }

        #region przyciski zakładki Bentley
        private void btnMicroModeler3D_Click(object sender, EventArgs e)
        {
            //createConfFile("1");
            //createUcfFile("Path3D");
            //try
            //{
            //    string s = "";
            //    FileStream file = new FileStream(SMLogowanie.mainPath + "conf/path_micro.xml", FileMode.Open, FileAccess.Read);
            //    //StreamReader streamReader = new StreamReader(file);
            //    XmlTextReader xmlReader = new XmlTextReader(file);
            //    while (xmlReader.Read())
            //    {
            //        if (xmlReader.Name == "Path3D")
            //        {
            //            s = xmlReader.ReadElementString();
            //        }
            //    }

            //    xmlReader.Close();
            //    file.Close();

            //    ProcessStartInfo startInfo = new ProcessStartInfo(s);
            //    if (cbDgnList.Text == "")
            //    {
            //        startInfo.Arguments = @"-wusoftmine";
            //        Process process = Process.Start(startInfo);
            //    }
            //    else
            //    {
            //        try
            //        {
            //            if (File.Exists(cbDgnList.Text))
            //            {
            //                startInfo.Arguments = "\"" + cbDgnList.Text + "\" -wusoftmine";
            //                Process process = Process.Start(startInfo);
            //            }
            //            else
            //            {
            //                MessageBox.Show("Podana ścieżka do pliku jest nieprawidłowa : " + cbDgnList.Text);
            //            }
            //        }
            //        catch
            //        {

            //        }

            //    }
            //}
            //catch (Exception exc)
            //{
            //    MessageBox.Show("Musisz wskazać poprawną ścieżkę do Microstation", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void btnMicrostation2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    FileStream file = new FileStream(SMLogowanie.mainPath + "conf/path_micro.xml", FileMode.Open, FileAccess.Read);
            //    //StreamReader streamReader = new StreamReader(file);
            //    XmlTextReader xmlReader = new XmlTextReader(file);

            //    string s = xmlReader.ReadElementString();
            //    //streamReader.Close();
            //    xmlReader.Close();
            //    file.Close();

            //    ProcessStartInfo startInfo = new ProcessStartInfo(s);
            //    if (cbDgnList.Text == "")
            //    {
            //        startInfo.Arguments = @"-wuuntitled";
            //        Process process = Process.Start(startInfo);
            //    }
            //    else
            //    {
            //        //startInfo.Arguments = @"ustation " + cbDgnList.Text;
            //        try
            //        {
            //            if (File.Exists(cbDgnList.Text))
            //            {
            //                startInfo.Arguments = "\"" + cbDgnList.Text + "\" -wusoftmine";
            //                Process process = Process.Start(startInfo);
            //            }
            //            else
            //            {
            //                MessageBox.Show("Podana ścieżka do pliku jest nieprawidłowa : " + cbDgnList.Text);
            //            }
            //        }
            //        catch
            //        {

            //        }
            //    }

            //}
            //catch (Exception exc)
            //{
            //    MessageBox.Show("Musisz wskazać poprawną ścieżkę do Microstation", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        } 
        #endregion
    }
}
