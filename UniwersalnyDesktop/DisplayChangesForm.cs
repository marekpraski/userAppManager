using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniwersalnyDesktop
{
    public partial class DisplayChangesForm : Form
    {
        private int tabControlWidth = 350;          //na starcie, dla max. 4 zakładek
        private int tabControlHeigth = 150;         //na starcie, dla jednej aplikacji

        private int tabPageWidth = 340;
        private int tabPageHeigth = 130;
        private int cumulativeTabPageHeadersLength = 0;     //służy do obliczenia potrzebnej szerokości formatki


        int appDataDisplayWidth = 330;
        int appDataDisplayHeigth = 100;


        private int appCount;                   //służy do obliczania wysokości TabPage
        private int formHorizontalPadding = 30;
        private int formVerticalPadding = 50;
        private int maxAppCount = 1;                //max liczba aplikacji dla jednego użytkownika, służy do obliczenia wysokości okna

        private ChangedDataBundle changedDataBundle;


        public DisplayChangesForm(ChangedDataBundle changedDataBundle)
        {
            InitializeComponent();
            this.changedDataBundle = changedDataBundle;
            generateTabs();
            setTabControlSize();
            setFormSize();
        }

        private void setTabControlSize()
        {

            if (cumulativeTabPageHeadersLength > tabControlWidth)
            {
                tabControlWidth = cumulativeTabPageHeadersLength;
            }
            tabControl1.Width = tabControlWidth;

            if (maxAppCount > 1)
            {
                tabControlHeigth += (maxAppCount - 1) * appDataDisplayHeigth;
            }
            tabControl1.Height = tabControlHeigth;
        }

        private void setFormSize()
        {
            this.Width = this.tabControl1.Width + formHorizontalPadding;
            this.Height = this.tabControl1.Height + formVerticalPadding;
        }

        private void generateTabs()
        {
            List<DesktopUser> usersList = changedDataBundle.getUsers();
            foreach(DesktopUser user in usersList)
            {
                generateOneTab(user);
            }
        }

        private void generateOneTab(DesktopUser user)
        {
            TabPage tabPage = new System.Windows.Forms.TabPage();
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Name = user.id;
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Size = new System.Drawing.Size(tabPageWidth, tabPageHeigth);
            tabPage.TabIndex = 0;
            tabPage.Text = user.name + " " + user.surname;
            tabPage.UseVisualStyleBackColor = true;

            int tabPageHeaderLength = TextRenderer.MeasureText(tabPage.Text, tabPage.Font).Width + 10;      //dodaję poprawkę na marginesy
            cumulativeTabPageHeadersLength += tabPageHeaderLength;

            addAppDataDisplay(tabPage, user);
            tabPage.Height = appCount * appDataDisplayHeigth;
            tabControl1.Controls.Add(tabPage);
        }

        private void addAppDataDisplay(TabPage tabPage, DesktopUser user)
        {
            List<App> chamgedAppList = changedDataBundle.getChangedUserApps(user);

            appCount = 0;

            foreach (App app in chamgedAppList)
            {
                AppDataDisplay appDataDisplay = new AppDataDisplay();
                appDataDisplay.Name = "appDataDisplay1";
                appDataDisplay.Size = new System.Drawing.Size(appDataDisplayWidth, appDataDisplayHeigth);
                appDataDisplay.TabIndex = 0;

                //liczę położenie kolejnego panelu
                appDataDisplay.Location = new System.Drawing.Point(7, appDataDisplayHeigth*appCount + 7);

                populateAppDataDisplay(appDataDisplay, user, app);
                tabPage.Controls.Add(appDataDisplay);

                appCount++;
            }
            if (appCount > maxAppCount)
            {
                maxAppCount = appCount;
            }            
        }


        private void populateAppDataDisplay(AppDataDisplay appDataDisplay, DesktopUser user, App app)
        {
            AppDataItem newAppData = changedDataBundle.getNewAppData(user, app);
            AppDataItem oldAppData = changedDataBundle.getOldAppData(user, app);

            appDataDisplay.setAppName(newAppData.appName);

            if (newAppData.isEnabled)
            {
                appDataDisplay.setNewRolaName(newAppData.rolaName);
                appDataDisplay.setNewRolaDesc(newAppData.rolaDesc);
            }
            else
            {
                appDataDisplay.setNewRolaName("");
                appDataDisplay.setNewRolaDesc("");
            }

            if (oldAppData != null)
            {
                appDataDisplay.setOldRolaDesc(oldAppData.rolaDesc);
                appDataDisplay.setOldRolaName(oldAppData.rolaName);
            }
            else
            {
                appDataDisplay.setOldRolaDesc("");
                appDataDisplay.setOldRolaName("");
            }

            string status = changedDataBundle.getAppDataStatus(user, app);
            switch(status)
            {
                case "delete":
                    appDataDisplay.setStatus("do usunięcia");
                    break;
                case "update":
                    appDataDisplay.setStatus("do aktualizacji");
                    break;
                case "insert":
                    appDataDisplay.setStatus("do dodania");
                    break;
                default:
                    appDataDisplay.setStatus("");
                    break;
            }
        }
    }
}
