
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using UtilityTools;

namespace UniwersalnyDesktop
{
   
    /// <summary>
    /// główny formularz Desktopu otwierany po zalogowaniu w LoginForm
    /// </summary>
    public partial class DesktopForm : Form
    {

        #region prywatne własciwości związane z układem i rozmiarami formatki
        //zmienne dotyczące rozmiarów i identyfikatorów tworzonych obiektów
        private int squareButtonsGroupboxWidth;
        private int squareButtonsGroupboxHeight;
        private int rectangularButtonsGroupboxHeight;
        private int groupboxID = 0;
        private int numberOfRectangularButtons;
        private int numberOfSquareButtonsInOneGroupbox = DesktopSettings.numberOfSquareButtonsInOneBlock;    //ustawienia domyślne, ale daję tu żeby program mógł automatycznie zmienić 
        #endregion

        private DesktopUser user = LoginForm.user;
        private string currentPath = LoginForm.mainPath;     //katalog z którego uruchamiany jest program, wykrywany przez DBConnector i ustawiany tutaj
                                                             //dla DEBUGA ustawiony jest w metodzie ReadAllData
        private DesktopDataHandler dataHandler;
        private Dictionary<string, DesktopProfile> profileDict;
        private DesktopProfile selectedProfile;

        public DesktopForm()
        {
            dataHandler = new DesktopDataHandler();
            InitializeComponent();
        }

        #region metody podczas ładowania formatki
        private void DesktopForm_Load(object sender, EventArgs e)
        {
            getDesktopData();
            if (profileDict == null || profileDict.Count == 0)
            {
                MyMessageBox.display("użytkownik nie ma dostępu do żadnych programów", MessageBoxType.Error);
                return;
            }
            fillProfileCombo();
            cbProfile.SelectedIndex = getIndexFromProfileId(user.lastUsedProfileId);
        }

        private int getIndexFromProfileId(string lastUsedProfileId)
        {
            for (int i = 0; i < cbProfile.Items.Count; i++)
            {
                ComboboxItem item = cbProfile.Items[i] as ComboboxItem;
                if (item.value.ToString() == lastUsedProfileId)
                    return i;
            }
            return -1;
        }

        #endregion

        #region metody podczas zamykania formatki
        private void DesktopForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            new DesktopSettings().saveCurrentSettings(selectedProfile.id);
            //Application.Exit();
        }

        #endregion

        #region czytanie danych Desktopu z bazy
        private void getDesktopData()
        {
            this.profileDict = dataHandler.profileDict;
        }
        #endregion

        #region wypełnianie kombo profili
        private void fillProfileCombo()
        {
            if (profileDict == null || profileDict.Count == 0)
                return;
            foreach (string profileId in profileDict.Keys)
            {
                ComboboxItem cbItem = new ComboboxItem(profileDict[profileId].name, profileId);
                cbProfile.Items.Add(cbItem);
            }
            cbProfile.DisplayMember = "displayText";
            cbProfile.ValueMember = "value";
        } 
        #endregion

        #region naciśnięcie przycisku aplikacji

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
            fm.runProgram(appLocation, user.sqlLogin + " " + user.sqlPassword);

        }

        #endregion

        #region wybór opcji z paska menu

        private void btnZmienHaslo_Click(object sender, EventArgs e)
        {
            Form formChangePass = new PasswordChanger();
            formChangePass.ShowDialog();
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profileId = (cbProfile.SelectedItem as ComboboxItem).value.ToString();
            this.selectedProfile = profileDict[profileId];
            resetDesktop();
            setupDesktop();
        }

        private void resetDesktop()
        {
            tabSoftmine.Controls.Clear();
            this.groupboxID = 0;
        }
        #endregion

        #region tworzenie layoutu formatki
        private void setupDesktop()
        {
            this.Text = "Zalogowano jako " + user.displayName;
            generateDesktopLayout();
        }

        private void generateDesktopLayout()
        {
            int numberOfSquareButtonGroupboxes = 1;
            int estimatedDesktopHeigth = 100;
            numberOfSquareButtonsInOneGroupbox--;       //najpierw pomniejszam o jeden, żeby gdy wejdę do pętli to w pierwszej pętli liczyć dla liczby ustawionej przez użytkownika
            do
            {
                numberOfSquareButtonsInOneGroupbox++;
                numberOfSquareButtonGroupboxes = calculateNumberOfSquareButtonGroupboxes();
                estimatedDesktopHeigth = estimateDesktopHeigth(numberOfSquareButtonGroupboxes);
            }
            while (estimatedDesktopHeigth > DesktopSettings.maxTabCtrlHeigth);

            //liczbę wszystkich buttonów dzielę na liczbę buttonów kwadratowych w jednym bloku
            //resztę umieszczam jako prostokątne w osobnym bloku
            numberOfRectangularButtons = selectedProfile.validApplications.Count - (numberOfSquareButtonGroupboxes * numberOfSquareButtonsInOneGroupbox);
            Button[] buttons = generateButtons(numberOfSquareButtonsInOneGroupbox * numberOfSquareButtonGroupboxes, numberOfRectangularButtons);
            calculateGroupboxSize();
            int i;
            for (i = 0; i < numberOfSquareButtonGroupboxes; i++)
            {
                GroupBox gbs = generateOneGroupbox(DesktopSettings.GroupboxType.squareButtons);
                addButtonsToGroupbox(gbs, buttons, DesktopSettings.ButtonType.square, i);
            }
            GroupBox gbr = generateOneGroupbox(DesktopSettings.GroupboxType.rectangularButtons);
            addButtonsToGroupbox(gbr, buttons, DesktopSettings.ButtonType.rectangular, i);
            setDesktopFormSize();
            cbProfile.Width = this.Size.Width - 120;
        } 
        #endregion

        #region tworzenie groupboxów
        private GroupBox generateOneGroupbox(DesktopSettings.GroupboxType groupboxType)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.Name = "groupBox" + groupboxID;
            int groupboxVerticalLocation = DesktopSettings.firstGroupboxVerticalLocation + DesktopSettings.verticalGroupboxPadding + groupboxID * squareButtonsGroupboxHeight;
            groupBox.Location = new System.Drawing.Point(DesktopSettings.horizontalGroupboxPadding, groupboxVerticalLocation);

            switch (groupboxType)
            {
                case DesktopSettings.GroupboxType.squareButtons:
                    groupBox.Size = new System.Drawing.Size(squareButtonsGroupboxWidth, squareButtonsGroupboxHeight);
                    break;
                case DesktopSettings.GroupboxType.rectangularButtons:
                    rectangularButtonsGroupboxHeight = numberOfRectangularButtons * (DesktopSettings.rectangularButtonHeigth + DesktopSettings.verticalButtonPadding);
                    groupBox.Size = new System.Drawing.Size(squareButtonsGroupboxWidth, rectangularButtonsGroupboxHeight);
                    break;
            }

            this.tabSoftmine.Controls.Add(groupBox);
            groupboxID++;

            return groupBox;
        }
        #endregion

        #region dodawanie przycisków do groupboxów
        private void addButtonsToGroupbox(GroupBox gb, Button[] buttons, DesktopSettings.ButtonType buttonType, int gbIndex)
        {
            switch (buttonType)
            {
                case DesktopSettings.ButtonType.square:
                    addSquareButtons(gb, buttons, gbIndex);
                    break;
                case DesktopSettings.ButtonType.rectangular:
                    addRectangularButtons(gb, buttons, gbIndex);
                    break;
            }
        }

        private void addSquareButtons(GroupBox gb, Button[] buttons, int gbIndex)
        {
            int btnEndIndex = gbIndex * numberOfSquareButtonsInOneGroupbox + numberOfSquareButtonsInOneGroupbox;
            int buttonNrInGroupbox = 0;
            for (int btnIndex = gbIndex * numberOfSquareButtonsInOneGroupbox; btnIndex < btnEndIndex; btnIndex++)
            {
                int buttonHorizontalLocation = DesktopSettings.horizontalButtonPadding;
                int buttonVerticalLocation = DesktopSettings.verticalButtonPadding;
                Button button = buttons[btnIndex];
                button.Location = new Point(buttonHorizontalLocation + buttonNrInGroupbox * (button.Width + DesktopSettings.horizontalButtonPadding), buttonVerticalLocation);

                gb.Controls.Add(button);
                buttonNrInGroupbox++;
            }
        }
        private void addRectangularButtons(GroupBox gb, Button[] buttons, int gbIndex)
        {
            int buttonNrInGroupbox = 0;
            for (int btnIndex = gbIndex * numberOfSquareButtonsInOneGroupbox; btnIndex < buttons.Length; btnIndex++)
            {
                int buttonHorizontalLocation = DesktopSettings.horizontalButtonPadding;
                int buttonVerticalLocation = DesktopSettings.verticalButtonPadding;
                Button button = buttons[btnIndex];

                button.Location = new Point(buttonHorizontalLocation, buttonVerticalLocation + buttonNrInGroupbox * (button.Height + DesktopSettings.verticalButtonPadding));
                gb.Controls.Add(button);
                buttonNrInGroupbox++;
            }
        } 
        #endregion

        #region tworzenie przycisków
        private Button[] generateButtons(int numberOfSquareButtons, int numberOfRectangularButtons)
        {
            List<Button> bts = new List<Button>(numberOfSquareButtons + numberOfRectangularButtons);

            int index = 0;
            Dictionary<string, IProfileItem> appDictionary = profileDict[selectedProfile.id].validApplications;
            Button b;
            foreach (string appId in appDictionary.Keys)
            {
                App app = appDictionary[appId] as App;
                if (index < numberOfSquareButtons)
                    b = generateOneButton(app, DesktopSettings.ButtonType.square, index);
                else
                    b = generateOneButton(app, DesktopSettings.ButtonType.rectangular, index);

                if (b != null)
                    bts.Add(b);
                index++;
            }
            return bts.ToArray();
        }
        private Button generateOneButton(App app, DesktopSettings.ButtonType buttonType, int index)
        {
            if (!app.isValid)
                return null;

            Button button = new Button();
            button.Click += new EventHandler(ButtonClick);
            button.Name = index.ToString();
            button.Tag = app.executionPath;
            button.Text = app.displayName;
            string t = app.displayName;
            bool v = user.hasApp(app);

            if (user.hasApp(app))
                button.Enabled = true;
            else
                button.Enabled = false;

            switch (buttonType)
            {
                case DesktopSettings.ButtonType.rectangular:
                    int rectangularButtonWidth = numberOfSquareButtonsInOneGroupbox * (DesktopSettings.squareButtonWidth + DesktopSettings.horizontalButtonPadding) - DesktopSettings.horizontalButtonPadding;
                    button.Size = new Size(rectangularButtonWidth, DesktopSettings.rectangularButtonHeigth);
                    break;
                case DesktopSettings.ButtonType.square:
                    button.Size = new Size(DesktopSettings.squareButtonWidth, DesktopSettings.squareButtonHeigth);
                    break;
            }
            return button;
        } 
        #endregion

        #region obliczanie wielkości groupboxów i formatki
        //oszacowuję wysokość desktopu przy zdefiniowanej w DesktopLayoutSettings liczbie kwadratowych buttonów w rzędzie
        //jeżeli wysokość desktopu wychodzi większa niż założona, zwiększam liczbę buttonów w rzedzie aż uzyskam przyzwoity rozmiar
        private int estimateDesktopHeigth(int nrOfGroupBoxes)
        {
            return (nrOfGroupBoxes + 1) * DesktopSettings.squareButtonHeigth;      //dodaję 1 żeby szacunkowo uwzględnić poziome przyciski oraz odstępy między groupboxami
        }

        private int calculateNumberOfSquareButtonGroupboxes()
        {
            return selectedProfile.validApplications.Count / numberOfSquareButtonsInOneGroupbox;
        }

        private void calculateGroupboxSize()
        {
            squareButtonsGroupboxWidth = numberOfSquareButtonsInOneGroupbox * (DesktopSettings.squareButtonWidth + DesktopSettings.horizontalButtonPadding) + DesktopSettings.horizontalButtonPadding;
            squareButtonsGroupboxHeight = DesktopSettings.squareButtonHeigth + 2 * DesktopSettings.verticalButtonPadding;
        }

        private void setDesktopFormSize()
        {
            int tabCtrlWidth = 2 * DesktopSettings.horizontalGroupboxPadding + squareButtonsGroupboxWidth + DesktopSettings.tabCtrlHorizontalPadding;
            int tabCtrlHeight = 2 * DesktopSettings.firstGroupboxVerticalLocation + squareButtonsGroupboxHeight * (groupboxID - 1) + DesktopSettings.tabCtrlVerticalPadding + rectangularButtonsGroupboxHeight;
            this.tabControl1.Width = tabCtrlWidth;
            this.tabControl1.Height = tabCtrlHeight;
            this.Width = tabControl1.Width + 40;
            this.Height = tabControl1.Height + 80;
        } 
        #endregion

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
