using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DatabaseInterface;

namespace UniwersalnyDesktop
{
    public partial class ProfileItemSelector : Form
    {

        private DesktopProfile editedProfile;
        private Dictionary<string, IProfileItem> allItems = new Dictionary<string, IProfileItem>();
        private Dictionary<string, IProfileItem> unusedItems = new Dictionary<string, IProfileItem>();
        List<IProfileItem> addedItems = new List<IProfileItem>();

        /// <summary>
        /// należy przekazać słownik wszystkich elementów; w tym oknie z tego słownika wybierane są, przez walidację, tylko te elementy, które nadają się do dodania 
        /// tzn są ważne (właściwość isValid, oraz edytowany profil jeszcze ich nie zawiera
        /// </summary>
        public ProfileItemSelector(DesktopProfile editedProfile, Dictionary<string, IProfileItem> allItems)
        {
            InitializeComponent();
            this.editedProfile = editedProfile;
            this.allItems = allItems;
            labelProfileName.Text = editedProfile.name;
        }

        private void ProfileItemSelector_Load(object sender, EventArgs e)
        {
            getUnusedItems();
            fillDgv();
        }

        private void getUnusedItems()
        {
            IProfileItem item = allItems.Values.ElementAt(0);
            if (item is App)
            {
                dgvItems.Columns["colNazwa"].HeaderText = "nazwa aplikacji";
                this.unusedItems = getUnusedItems(editedProfile.applications);
            }
            else if (item is DesktopUser)
            {
                dgvItems.Columns["colNazwa"].HeaderText = "użytkownik";
                this.unusedItems = getUnusedItems(editedProfile.users);
            }
        }

        private Dictionary<string, IProfileItem> getUnusedItems(Dictionary<string, IProfileItem> profileItems)
        {
            Dictionary<string, IProfileItem> items = new Dictionary<string, IProfileItem>();
            foreach(string id in this.allItems.Keys)
            {
                IProfileItem item = allItems[id];
                bool v = allItems[id].isValid;
                if (!profileItems.ContainsKey(id) && allItems[id].isValid)
                    items.Add(allItems[id].id, allItems[id]);
            }
            return items;
        }

        private void fillDgv()
        {
            dgvItems.ClearSelection();

            foreach (string id in unusedItems.Keys)
            {
                dgvItems.Rows.Add(
                   allItems[id].id,
                    allItems[id].displayName
                    );
            }
            dgvItems.Sort(dgvItems.Columns["colNazwa"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            bool success = false;
            dgvItems.EndEdit();
            IProfileItem item = allItems.Values.ElementAt(0);
            if (item is App)
            {
                addItemsToProfile(addApp);
                success = saveToDB("INSERT INTO profile_app (ID_profile,ID_app) VALUES (");
            }
            else if (item is DesktopUser)
            {
                addItemsToProfile(addUser);
                success = saveToDB("INSERT INTO profile_users (ID_profile,ID_user) VALUES (");
            }
            if (success)
            {
                MessageBox.Show("Dodano do profilu.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private delegate void itemAddDelegate(string itemId);
        private void addApp(string appId)
        {
            editedProfile.addAppToProfile(allItems[appId] as App);
        }

        private void addUser(string userId)
        {
            editedProfile.addUserToProfile(allItems[userId] as DesktopUser);
        }

        private void addItemsToProfile(itemAddDelegate itemAdd)
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell chkchecking = dgvItems.Rows[i].Cells["colDodaj"] as DataGridViewCheckBoxCell;

                bool isChecked = Convert.ToBoolean(chkchecking.Value);
                if (isChecked)
                {
                    string itemId = dgvItems.Rows[i].Cells["colId"].Value.ToString();
                    if (allItems[itemId].isValid)
                    {
                        itemAdd(itemId);
                        addedItems.Add(allItems[itemId]);
                    }
                }
            }
        }

        private bool saveToDB(string queryBase)
        {
            string[] queries = new string[addedItems.Count];
            for (int i = 0; i < addedItems.Count; i++)
            {
                queries[i] = queryBase + editedProfile.id + "," + addedItems[i].id + "); ";
            }
            return new DBWriter(LoginForm.dbConnection).executeQuery(queries);
        }

    }
}
