using System.Windows.Forms;

namespace Admin
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            buttonGetProducts = new Button();
            buttonClose = new Button();
            buttonRefreshCache = new Button();
            richTextBoxData = new RichTextBox();
            listViewProducts = new ListView();
            columnHeaderProductsBank = new ColumnHeader();
            columnHeaderProductsProduct = new ColumnHeader();
            columnHeaderProductsCategory = new ColumnHeader();
            columnHeaderProductsRateType = new ColumnHeader();
            columnHeaderProductsID = new ColumnHeader();
            columnHeaderLastProductsRefreshed = new ColumnHeader();
            tabControlMain = new TabControl();
            tabPageCache = new TabPage();
            linkLabelReloadCandidateBanks = new LinkLabel();
            buttonDeleteCache = new Button();
            linkLabelSelectNone = new LinkLabel();
            linkLabelSelectAll = new LinkLabel();
            splitContainerCache = new SplitContainer();
            listViewBanks = new ListView();
            columnHeaderCacheBankName = new ColumnHeader();
            columnHeaderCacheLastRefrehed = new ColumnHeader();
            columnHeaderCacheLastRefrehedDays = new ColumnHeader();
            columnHeaderCacheLastBankUpdate = new ColumnHeader();
            columnHeaderCacheLastBankUpdateDays = new ColumnHeader();
            columnHeaderCacheDisplayOrder = new ColumnHeader();
            columnHeaderCacheActive = new ColumnHeader();
            columnHeaderCacheOpenBankingURL = new ColumnHeader();
            columnHeaderCacheVersion = new ColumnHeader();
            columnHeaderCacheVersionMin = new ColumnHeader();
            columnHeaderCacheLogoURL = new ColumnHeader();
            progressBarCache = new ProgressBar();
            richTextBoxCachResults = new RichTextBox();
            linkLabelOpenCacheFolder = new LinkLabel();
            tabPageProducts = new TabPage();
            buttonTestAddUser = new Button();
            buttonTestGetUsers = new Button();
            splitContainerProducts = new SplitContainer();
            splitContainerProductCounts = new SplitContainer();
            listViewProductCounts = new ListView();
            columnHeaderPropertyCount = new ColumnHeader();
            columnHeaderValueCount = new ColumnHeader();
            splitContainerProductDetails = new SplitContainer();
            panelTermDeposits = new Panel();
            textBoxDepositDepositAmount = new TextBox();
            labelDepositDepositAmount = new Label();
            linkLabelTermNone = new LinkLabel();
            linkLabelTermsAll = new LinkLabel();
            checkedDepositListBoxTerm = new CheckedListBox();
            labelDepositDepositTerm = new Label();
            comboBoxProductCategory = new ComboBox();
            labelProductCategory = new Label();
            splitContainerProductProperties = new SplitContainer();
            splitContainerProductComparison = new SplitContainer();
            listViewFilteredProducts = new ListView();
            columnFilterBankName = new ColumnHeader();
            columnHeaderFilterProduct = new ColumnHeader();
            columnHeaderFilterProductID = new ColumnHeader();
            labelProductFilterCount = new Label();
            labelBankFilterCount = new Label();
            checkBoxShowCompareResults = new CheckBox();
            linkLabelOpenFolder = new LinkLabel();
            labelResult = new Label();
            labelPrompt = new Label();
            richTextBoxPrompt = new RichTextBox();
            buttonCompare = new Button();
            richTextBoxPromptResults = new RichTextBox();
            listViewProductValues = new ListView();
            columnHeaderProperty = new ColumnHeader();
            columnHeaderValue = new ColumnHeader();
            buttonFilterProducts = new Button();
            labelSearchProducts = new Label();
            linkLabelResetProducts = new LinkLabel();
            buttonSearchProducts = new Button();
            textBoxSearchProducts = new TextBox();
            richTextBoxError = new RichTextBox();
            folderBrowserDialogCache = new FolderBrowserDialog();
            statusStripMain = new StatusStrip();
            toolStripStatusLabelBankCount = new ToolStripStatusLabel();
            toolStripStatusLabelSpacer1 = new ToolStripStatusLabel();
            toolStripStatusLabelProductCount = new ToolStripStatusLabel();
            toolStripStatusLabelSpacer2 = new ToolStripStatusLabel();
            toolStripStatusLabelMessage = new ToolStripStatusLabel();
            toolStripMain = new ToolStrip();
            toolStripLabelSpacer1 = new ToolStripLabel();
            toolStripButtonSettings = new ToolStripButton();
            menuStripMain = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            timerMain = new System.Windows.Forms.Timer(components);
            tabControlMain.SuspendLayout();
            tabPageCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCache).BeginInit();
            splitContainerCache.Panel1.SuspendLayout();
            splitContainerCache.Panel2.SuspendLayout();
            splitContainerCache.SuspendLayout();
            tabPageProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProducts).BeginInit();
            splitContainerProducts.Panel1.SuspendLayout();
            splitContainerProducts.Panel2.SuspendLayout();
            splitContainerProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductCounts).BeginInit();
            splitContainerProductCounts.Panel1.SuspendLayout();
            splitContainerProductCounts.Panel2.SuspendLayout();
            splitContainerProductCounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductDetails).BeginInit();
            splitContainerProductDetails.Panel1.SuspendLayout();
            splitContainerProductDetails.Panel2.SuspendLayout();
            splitContainerProductDetails.SuspendLayout();
            panelTermDeposits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductProperties).BeginInit();
            splitContainerProductProperties.Panel1.SuspendLayout();
            splitContainerProductProperties.Panel2.SuspendLayout();
            splitContainerProductProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductComparison).BeginInit();
            splitContainerProductComparison.Panel1.SuspendLayout();
            splitContainerProductComparison.Panel2.SuspendLayout();
            splitContainerProductComparison.SuspendLayout();
            statusStripMain.SuspendLayout();
            toolStripMain.SuspendLayout();
            menuStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // buttonGetProducts
            // 
            buttonGetProducts.Location = new Point(15, 11);
            buttonGetProducts.Name = "buttonGetProducts";
            buttonGetProducts.Size = new Size(116, 23);
            buttonGetProducts.TabIndex = 2;
            buttonGetProducts.Text = "Get Products";
            buttonGetProducts.UseVisualStyleBackColor = true;
            buttonGetProducts.Click += buttonGetProducts_Click;
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.Location = new Point(1736, 989);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(106, 23);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonRefreshCache
            // 
            buttonRefreshCache.Location = new Point(15, 11);
            buttonRefreshCache.Name = "buttonRefreshCache";
            buttonRefreshCache.Size = new Size(116, 23);
            buttonRefreshCache.TabIndex = 4;
            buttonRefreshCache.Text = "Refresh Cache";
            buttonRefreshCache.UseVisualStyleBackColor = true;
            buttonRefreshCache.Click += buttonRefreshCache_Click;
            // 
            // richTextBoxData
            // 
            richTextBoxData.Dock = DockStyle.Fill;
            richTextBoxData.Location = new Point(0, 0);
            richTextBoxData.Name = "richTextBoxData";
            richTextBoxData.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            richTextBoxData.Size = new Size(370, 831);
            richTextBoxData.TabIndex = 1;
            richTextBoxData.Text = "";
            richTextBoxData.WordWrap = false;
            richTextBoxData.TextChanged += richTextBoxData_TextChanged;
            // 
            // listViewProducts
            // 
            listViewProducts.Columns.AddRange(new ColumnHeader[] { columnHeaderProductsBank, columnHeaderProductsProduct, columnHeaderProductsCategory, columnHeaderProductsRateType, columnHeaderProductsID, columnHeaderLastProductsRefreshed });
            listViewProducts.Dock = DockStyle.Fill;
            listViewProducts.FullRowSelect = true;
            listViewProducts.Location = new Point(0, 0);
            listViewProducts.Name = "listViewProducts";
            listViewProducts.Size = new Size(475, 408);
            listViewProducts.TabIndex = 0;
            listViewProducts.UseCompatibleStateImageBehavior = false;
            listViewProducts.View = View.Details;
            listViewProducts.SelectedIndexChanged += listViewMain_SelectedIndexChanged;
            // 
            // columnHeaderProductsBank
            // 
            columnHeaderProductsBank.Text = "Bank";
            columnHeaderProductsBank.Width = 110;
            // 
            // columnHeaderProductsProduct
            // 
            columnHeaderProductsProduct.Text = "Product";
            columnHeaderProductsProduct.Width = 300;
            // 
            // columnHeaderProductsCategory
            // 
            columnHeaderProductsCategory.Text = "Category";
            columnHeaderProductsCategory.Width = 200;
            // 
            // columnHeaderProductsRateType
            // 
            columnHeaderProductsRateType.Text = "Rate Type";
            columnHeaderProductsRateType.Width = 100;
            // 
            // columnHeaderProductsID
            // 
            columnHeaderProductsID.Text = "ID";
            columnHeaderProductsID.Width = 230;
            // 
            // columnHeaderLastProductsRefreshed
            // 
            columnHeaderLastProductsRefreshed.Text = "Last Refreshed";
            columnHeaderLastProductsRefreshed.Width = 150;
            // 
            // tabControlMain
            // 
            tabControlMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlMain.Controls.Add(tabPageCache);
            tabControlMain.Controls.Add(tabPageProducts);
            tabControlMain.Location = new Point(12, 66);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1834, 909);
            tabControlMain.TabIndex = 8;
            tabControlMain.SelectedIndexChanged += tabControlMain_SelectedIndexChanged;
            // 
            // tabPageCache
            // 
            tabPageCache.Controls.Add(linkLabelReloadCandidateBanks);
            tabPageCache.Controls.Add(buttonDeleteCache);
            tabPageCache.Controls.Add(linkLabelSelectNone);
            tabPageCache.Controls.Add(linkLabelSelectAll);
            tabPageCache.Controls.Add(splitContainerCache);
            tabPageCache.Controls.Add(linkLabelOpenCacheFolder);
            tabPageCache.Controls.Add(buttonRefreshCache);
            tabPageCache.Location = new Point(4, 24);
            tabPageCache.Name = "tabPageCache";
            tabPageCache.Padding = new Padding(3);
            tabPageCache.Size = new Size(1826, 881);
            tabPageCache.TabIndex = 1;
            tabPageCache.Text = "Cache";
            tabPageCache.UseVisualStyleBackColor = true;
            // 
            // linkLabelReloadCandidateBanks
            // 
            linkLabelReloadCandidateBanks.Location = new Point(142, 16);
            linkLabelReloadCandidateBanks.Name = "linkLabelReloadCandidateBanks";
            linkLabelReloadCandidateBanks.Size = new Size(55, 15);
            linkLabelReloadCandidateBanks.TabIndex = 17;
            linkLabelReloadCandidateBanks.TabStop = true;
            linkLabelReloadCandidateBanks.Text = "Reload";
            linkLabelReloadCandidateBanks.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelReloadCandidateBanks.LinkClicked += linkLabelReloadCandidateBanks_LinkClicked;
            // 
            // buttonDeleteCache
            // 
            buttonDeleteCache.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonDeleteCache.Location = new Point(1690, 12);
            buttonDeleteCache.Name = "buttonDeleteCache";
            buttonDeleteCache.Size = new Size(116, 23);
            buttonDeleteCache.TabIndex = 16;
            buttonDeleteCache.Text = "Delete Cache";
            buttonDeleteCache.UseVisualStyleBackColor = true;
            buttonDeleteCache.Click += buttonDeleteCache_Click;
            // 
            // linkLabelSelectNone
            // 
            linkLabelSelectNone.Location = new Point(274, 15);
            linkLabelSelectNone.Name = "linkLabelSelectNone";
            linkLabelSelectNone.Size = new Size(70, 15);
            linkLabelSelectNone.TabIndex = 15;
            linkLabelSelectNone.TabStop = true;
            linkLabelSelectNone.Text = "Select None";
            linkLabelSelectNone.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelSelectNone.LinkClicked += linkLabelSelectNone_LinkClicked;
            // 
            // linkLabelSelectAll
            // 
            linkLabelSelectAll.Location = new Point(205, 15);
            linkLabelSelectAll.Name = "linkLabelSelectAll";
            linkLabelSelectAll.Size = new Size(55, 15);
            linkLabelSelectAll.TabIndex = 14;
            linkLabelSelectAll.TabStop = true;
            linkLabelSelectAll.Text = "Select All";
            linkLabelSelectAll.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelSelectAll.LinkClicked += linkLabelSelectAll_LinkClicked;
            // 
            // splitContainerCache
            // 
            splitContainerCache.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerCache.Location = new Point(15, 43);
            splitContainerCache.Name = "splitContainerCache";
            splitContainerCache.Orientation = Orientation.Horizontal;
            // 
            // splitContainerCache.Panel1
            // 
            splitContainerCache.Panel1.Controls.Add(listViewBanks);
            // 
            // splitContainerCache.Panel2
            // 
            splitContainerCache.Panel2.Controls.Add(progressBarCache);
            splitContainerCache.Panel2.Controls.Add(richTextBoxCachResults);
            splitContainerCache.Size = new Size(1794, 822);
            splitContainerCache.SplitterDistance = 341;
            splitContainerCache.TabIndex = 13;
            splitContainerCache.SplitterMoved += splitContainerCache_SplitterMoved;
            // 
            // listViewBanks
            // 
            listViewBanks.CheckBoxes = true;
            listViewBanks.Columns.AddRange(new ColumnHeader[] { columnHeaderCacheBankName, columnHeaderCacheLastRefrehed, columnHeaderCacheLastRefrehedDays, columnHeaderCacheLastBankUpdate, columnHeaderCacheLastBankUpdateDays, columnHeaderCacheDisplayOrder, columnHeaderCacheActive, columnHeaderCacheOpenBankingURL, columnHeaderCacheVersion, columnHeaderCacheVersionMin, columnHeaderCacheLogoURL });
            listViewBanks.Dock = DockStyle.Fill;
            listViewBanks.FullRowSelect = true;
            listViewBanks.Location = new Point(0, 0);
            listViewBanks.MultiSelect = false;
            listViewBanks.Name = "listViewBanks";
            listViewBanks.Size = new Size(1794, 341);
            listViewBanks.TabIndex = 5;
            listViewBanks.UseCompatibleStateImageBehavior = false;
            listViewBanks.View = View.Details;
            listViewBanks.ColumnClick += listViewBanks_ColumnClick;
            // 
            // columnHeaderCacheBankName
            // 
            columnHeaderCacheBankName.Text = "Bank";
            columnHeaderCacheBankName.Width = 200;
            // 
            // columnHeaderCacheLastRefrehed
            // 
            columnHeaderCacheLastRefrehed.Text = "Last Refreshed";
            columnHeaderCacheLastRefrehed.Width = 140;
            // 
            // columnHeaderCacheLastRefrehedDays
            // 
            columnHeaderCacheLastRefrehedDays.Text = "Last Refreshed Days";
            columnHeaderCacheLastRefrehedDays.Width = 130;
            // 
            // columnHeaderCacheLastBankUpdate
            // 
            columnHeaderCacheLastBankUpdate.Text = "Last Bank Update";
            columnHeaderCacheLastBankUpdate.Width = 140;
            // 
            // columnHeaderCacheLastBankUpdateDays
            // 
            columnHeaderCacheLastBankUpdateDays.Text = "Last Bank Update Days";
            columnHeaderCacheLastBankUpdateDays.Width = 140;
            // 
            // columnHeaderCacheDisplayOrder
            // 
            columnHeaderCacheDisplayOrder.Text = "Order";
            // 
            // columnHeaderCacheActive
            // 
            columnHeaderCacheActive.Text = "Active";
            // 
            // columnHeaderCacheOpenBankingURL
            // 
            columnHeaderCacheOpenBankingURL.Text = "Open Banking URL";
            columnHeaderCacheOpenBankingURL.Width = 420;
            // 
            // columnHeaderCacheVersion
            // 
            columnHeaderCacheVersion.Text = "Version";
            columnHeaderCacheVersion.Width = 90;
            // 
            // columnHeaderCacheVersionMin
            // 
            columnHeaderCacheVersionMin.Text = "Min Version";
            columnHeaderCacheVersionMin.Width = 100;
            // 
            // columnHeaderCacheLogoURL
            // 
            columnHeaderCacheLogoURL.Text = "Logo URL";
            columnHeaderCacheLogoURL.Width = 500;
            // 
            // progressBarCache
            // 
            progressBarCache.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBarCache.Location = new Point(0, 464);
            progressBarCache.Name = "progressBarCache";
            progressBarCache.Size = new Size(1790, 10);
            progressBarCache.TabIndex = 12;
            // 
            // richTextBoxCachResults
            // 
            richTextBoxCachResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBoxCachResults.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBoxCachResults.Location = new Point(0, 2);
            richTextBoxCachResults.Name = "richTextBoxCachResults";
            richTextBoxCachResults.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            richTextBoxCachResults.Size = new Size(1794, 458);
            richTextBoxCachResults.TabIndex = 11;
            richTextBoxCachResults.Text = "";
            richTextBoxCachResults.WordWrap = false;
            // 
            // linkLabelOpenCacheFolder
            // 
            linkLabelOpenCacheFolder.Location = new Point(360, 15);
            linkLabelOpenCacheFolder.Name = "linkLabelOpenCacheFolder";
            linkLabelOpenCacheFolder.Size = new Size(72, 15);
            linkLabelOpenCacheFolder.TabIndex = 12;
            linkLabelOpenCacheFolder.TabStop = true;
            linkLabelOpenCacheFolder.Text = "Open Folder";
            linkLabelOpenCacheFolder.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelOpenCacheFolder.LinkClicked += linkLabelOpenCacheFolder_LinkClicked;
            // 
            // tabPageProducts
            // 
            tabPageProducts.Controls.Add(buttonTestAddUser);
            tabPageProducts.Controls.Add(buttonTestGetUsers);
            tabPageProducts.Controls.Add(splitContainerProducts);
            tabPageProducts.Controls.Add(labelSearchProducts);
            tabPageProducts.Controls.Add(linkLabelResetProducts);
            tabPageProducts.Controls.Add(buttonSearchProducts);
            tabPageProducts.Controls.Add(textBoxSearchProducts);
            tabPageProducts.Controls.Add(buttonGetProducts);
            tabPageProducts.Location = new Point(4, 24);
            tabPageProducts.Name = "tabPageProducts";
            tabPageProducts.Padding = new Padding(3);
            tabPageProducts.Size = new Size(1826, 881);
            tabPageProducts.TabIndex = 0;
            tabPageProducts.Text = "Products";
            tabPageProducts.UseVisualStyleBackColor = true;
            // 
            // buttonTestAddUser
            // 
            buttonTestAddUser.Location = new Point(612, 11);
            buttonTestAddUser.Name = "buttonTestAddUser";
            buttonTestAddUser.Size = new Size(100, 23);
            buttonTestAddUser.TabIndex = 9;
            buttonTestAddUser.Text = "Test Add User";
            buttonTestAddUser.UseVisualStyleBackColor = true;
            buttonTestAddUser.Click += buttonTestAddUser_Click;
            // 
            // buttonTestGetUsers
            // 
            buttonTestGetUsers.Location = new Point(718, 11);
            buttonTestGetUsers.Name = "buttonTestGetUsers";
            buttonTestGetUsers.Size = new Size(103, 23);
            buttonTestGetUsers.TabIndex = 8;
            buttonTestGetUsers.Text = "Test Get Users";
            buttonTestGetUsers.UseVisualStyleBackColor = true;
            buttonTestGetUsers.Click += buttonTestGetUsers_Click;
            // 
            // splitContainerProducts
            // 
            splitContainerProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerProducts.Location = new Point(15, 43);
            splitContainerProducts.Name = "splitContainerProducts";
            // 
            // splitContainerProducts.Panel1
            // 
            splitContainerProducts.Panel1.Controls.Add(splitContainerProductCounts);
            // 
            // splitContainerProducts.Panel2
            // 
            splitContainerProducts.Panel2.Controls.Add(splitContainerProductDetails);
            splitContainerProducts.Size = new Size(1790, 831);
            splitContainerProducts.SplitterDistance = 475;
            splitContainerProducts.TabIndex = 7;
            splitContainerProducts.SplitterMoved += splitContainerProducts_SplitterMoved;
            // 
            // splitContainerProductCounts
            // 
            splitContainerProductCounts.Dock = DockStyle.Fill;
            splitContainerProductCounts.Location = new Point(0, 0);
            splitContainerProductCounts.Name = "splitContainerProductCounts";
            splitContainerProductCounts.Orientation = Orientation.Horizontal;
            // 
            // splitContainerProductCounts.Panel1
            // 
            splitContainerProductCounts.Panel1.Controls.Add(listViewProducts);
            // 
            // splitContainerProductCounts.Panel2
            // 
            splitContainerProductCounts.Panel2.Controls.Add(listViewProductCounts);
            splitContainerProductCounts.Size = new Size(475, 831);
            splitContainerProductCounts.SplitterDistance = 408;
            splitContainerProductCounts.TabIndex = 4;
            splitContainerProductCounts.SplitterMoved += splitContainerProductCounts_SplitterMoved;
            // 
            // listViewProductCounts
            // 
            listViewProductCounts.Columns.AddRange(new ColumnHeader[] { columnHeaderPropertyCount, columnHeaderValueCount });
            listViewProductCounts.Dock = DockStyle.Fill;
            listViewProductCounts.FullRowSelect = true;
            listViewProductCounts.Location = new Point(0, 0);
            listViewProductCounts.Name = "listViewProductCounts";
            listViewProductCounts.Size = new Size(475, 419);
            listViewProductCounts.TabIndex = 3;
            listViewProductCounts.UseCompatibleStateImageBehavior = false;
            listViewProductCounts.View = View.Details;
            // 
            // columnHeaderPropertyCount
            // 
            columnHeaderPropertyCount.Text = "Property";
            columnHeaderPropertyCount.Width = 290;
            // 
            // columnHeaderValueCount
            // 
            columnHeaderValueCount.Text = "Count";
            columnHeaderValueCount.Width = 100;
            // 
            // splitContainerProductDetails
            // 
            splitContainerProductDetails.Dock = DockStyle.Fill;
            splitContainerProductDetails.Location = new Point(0, 0);
            splitContainerProductDetails.Name = "splitContainerProductDetails";
            // 
            // splitContainerProductDetails.Panel1
            // 
            splitContainerProductDetails.Panel1.Controls.Add(richTextBoxData);
            // 
            // splitContainerProductDetails.Panel2
            // 
            splitContainerProductDetails.Panel2.Controls.Add(panelTermDeposits);
            splitContainerProductDetails.Panel2.Controls.Add(comboBoxProductCategory);
            splitContainerProductDetails.Panel2.Controls.Add(labelProductCategory);
            splitContainerProductDetails.Panel2.Controls.Add(splitContainerProductProperties);
            splitContainerProductDetails.Panel2.Controls.Add(buttonFilterProducts);
            splitContainerProductDetails.Size = new Size(1311, 831);
            splitContainerProductDetails.SplitterDistance = 370;
            splitContainerProductDetails.TabIndex = 3;
            splitContainerProductDetails.SplitterMoved += splitContainerProductValues_SplitterMoved;
            // 
            // panelTermDeposits
            // 
            panelTermDeposits.Controls.Add(textBoxDepositDepositAmount);
            panelTermDeposits.Controls.Add(labelDepositDepositAmount);
            panelTermDeposits.Controls.Add(linkLabelTermNone);
            panelTermDeposits.Controls.Add(linkLabelTermsAll);
            panelTermDeposits.Controls.Add(checkedDepositListBoxTerm);
            panelTermDeposits.Controls.Add(labelDepositDepositTerm);
            panelTermDeposits.Location = new Point(23, 37);
            panelTermDeposits.Name = "panelTermDeposits";
            panelTermDeposits.Size = new Size(322, 174);
            panelTermDeposits.TabIndex = 28;
            // 
            // textBoxDepositDepositAmount
            // 
            textBoxDepositDepositAmount.Location = new Point(120, 8);
            textBoxDepositDepositAmount.Name = "textBoxDepositDepositAmount";
            textBoxDepositDepositAmount.Size = new Size(100, 23);
            textBoxDepositDepositAmount.TabIndex = 35;
            textBoxDepositDepositAmount.Text = "10,000";
            // 
            // labelDepositDepositAmount
            // 
            labelDepositDepositAmount.AutoSize = true;
            labelDepositDepositAmount.Location = new Point(2, 11);
            labelDepositDepositAmount.Name = "labelDepositDepositAmount";
            labelDepositDepositAmount.Size = new Size(97, 15);
            labelDepositDepositAmount.TabIndex = 39;
            labelDepositDepositAmount.Text = "Deposit Amount:";
            // 
            // linkLabelTermNone
            // 
            linkLabelTermNone.Location = new Point(243, 64);
            linkLabelTermNone.Name = "linkLabelTermNone";
            linkLabelTermNone.Size = new Size(76, 15);
            linkLabelTermNone.TabIndex = 38;
            linkLabelTermNone.TabStop = true;
            linkLabelTermNone.Text = "Select None";
            linkLabelTermNone.TextAlign = ContentAlignment.MiddleLeft;
            linkLabelTermNone.LinkClicked += linkLabelTermNone_LinkClicked;
            // 
            // linkLabelTermsAll
            // 
            linkLabelTermsAll.Location = new Point(243, 41);
            linkLabelTermsAll.Name = "linkLabelTermsAll";
            linkLabelTermsAll.Size = new Size(76, 15);
            linkLabelTermsAll.TabIndex = 37;
            linkLabelTermsAll.TabStop = true;
            linkLabelTermsAll.Text = "Select All";
            linkLabelTermsAll.TextAlign = ContentAlignment.MiddleLeft;
            linkLabelTermsAll.LinkClicked += linkLabelTermsAll_LinkClicked;
            // 
            // checkedDepositListBoxTerm
            // 
            checkedDepositListBoxTerm.FormattingEnabled = true;
            checkedDepositListBoxTerm.Location = new Point(119, 39);
            checkedDepositListBoxTerm.Name = "checkedDepositListBoxTerm";
            checkedDepositListBoxTerm.Size = new Size(118, 130);
            checkedDepositListBoxTerm.TabIndex = 36;
            // 
            // labelDepositDepositTerm
            // 
            labelDepositDepositTerm.AutoSize = true;
            labelDepositDepositTerm.Location = new Point(2, 39);
            labelDepositDepositTerm.Name = "labelDepositDepositTerm";
            labelDepositDepositTerm.Size = new Size(88, 15);
            labelDepositDepositTerm.TabIndex = 35;
            labelDepositDepositTerm.Text = "Term (months):";
            // 
            // comboBoxProductCategory
            // 
            comboBoxProductCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProductCategory.FormattingEnabled = true;
            comboBoxProductCategory.Location = new Point(141, 12);
            comboBoxProductCategory.Name = "comboBoxProductCategory";
            comboBoxProductCategory.Size = new Size(341, 23);
            comboBoxProductCategory.TabIndex = 19;
            comboBoxProductCategory.SelectedIndexChanged += comboBoxProductCategory_SelectedIndexChanged;
            // 
            // labelProductCategory
            // 
            labelProductCategory.AutoSize = true;
            labelProductCategory.Location = new Point(23, 16);
            labelProductCategory.Name = "labelProductCategory";
            labelProductCategory.Size = new Size(103, 15);
            labelProductCategory.TabIndex = 25;
            labelProductCategory.Text = "Product Category:";
            // 
            // splitContainerProductProperties
            // 
            splitContainerProductProperties.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainerProductProperties.Location = new Point(3, 248);
            splitContainerProductProperties.Name = "splitContainerProductProperties";
            // 
            // splitContainerProductProperties.Panel1
            // 
            splitContainerProductProperties.Panel1.Controls.Add(splitContainerProductComparison);
            // 
            // splitContainerProductProperties.Panel2
            // 
            splitContainerProductProperties.Panel2.Controls.Add(listViewProductValues);
            splitContainerProductProperties.Size = new Size(926, 583);
            splitContainerProductProperties.SplitterDistance = 475;
            splitContainerProductProperties.TabIndex = 0;
            splitContainerProductProperties.SplitterMoved += splitContainerProductProperties_SplitterMoved;
            // 
            // splitContainerProductComparison
            // 
            splitContainerProductComparison.Dock = DockStyle.Fill;
            splitContainerProductComparison.Location = new Point(0, 0);
            splitContainerProductComparison.Name = "splitContainerProductComparison";
            splitContainerProductComparison.Orientation = Orientation.Horizontal;
            // 
            // splitContainerProductComparison.Panel1
            // 
            splitContainerProductComparison.Panel1.Controls.Add(listViewFilteredProducts);
            splitContainerProductComparison.Panel1.Controls.Add(labelProductFilterCount);
            splitContainerProductComparison.Panel1.Controls.Add(labelBankFilterCount);
            // 
            // splitContainerProductComparison.Panel2
            // 
            splitContainerProductComparison.Panel2.Controls.Add(checkBoxShowCompareResults);
            splitContainerProductComparison.Panel2.Controls.Add(linkLabelOpenFolder);
            splitContainerProductComparison.Panel2.Controls.Add(labelResult);
            splitContainerProductComparison.Panel2.Controls.Add(labelPrompt);
            splitContainerProductComparison.Panel2.Controls.Add(richTextBoxPrompt);
            splitContainerProductComparison.Panel2.Controls.Add(buttonCompare);
            splitContainerProductComparison.Panel2.Controls.Add(richTextBoxPromptResults);
            splitContainerProductComparison.Size = new Size(475, 583);
            splitContainerProductComparison.SplitterDistance = 195;
            splitContainerProductComparison.TabIndex = 13;
            splitContainerProductComparison.SplitterMoved += splitContainerProductComparison_SplitterMoved;
            // 
            // listViewFilteredProducts
            // 
            listViewFilteredProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listViewFilteredProducts.CheckBoxes = true;
            listViewFilteredProducts.Columns.AddRange(new ColumnHeader[] { columnFilterBankName, columnHeaderFilterProduct, columnHeaderFilterProductID });
            listViewFilteredProducts.FullRowSelect = true;
            listViewFilteredProducts.Location = new Point(0, 0);
            listViewFilteredProducts.Name = "listViewFilteredProducts";
            listViewFilteredProducts.Size = new Size(469, 166);
            listViewFilteredProducts.TabIndex = 5;
            listViewFilteredProducts.UseCompatibleStateImageBehavior = false;
            listViewFilteredProducts.View = View.Details;
            listViewFilteredProducts.ItemChecked += listViewFilteredProducts_ItemChecked;
            listViewFilteredProducts.SelectedIndexChanged += listViewFilteredProducts_SelectedIndexChanged;
            // 
            // columnFilterBankName
            // 
            columnFilterBankName.Text = "Bank";
            columnFilterBankName.Width = 110;
            // 
            // columnHeaderFilterProduct
            // 
            columnHeaderFilterProduct.Text = "Product";
            columnHeaderFilterProduct.Width = 300;
            // 
            // columnHeaderFilterProductID
            // 
            columnHeaderFilterProductID.Text = "ID";
            columnHeaderFilterProductID.Width = 230;
            // 
            // labelProductFilterCount
            // 
            labelProductFilterCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelProductFilterCount.AutoSize = true;
            labelProductFilterCount.Location = new Point(140, 172);
            labelProductFilterCount.Name = "labelProductFilterCount";
            labelProductFilterCount.Size = new Size(88, 15);
            labelProductFilterCount.TabIndex = 12;
            labelProductFilterCount.Text = "Product Count:";
            // 
            // labelBankFilterCount
            // 
            labelBankFilterCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelBankFilterCount.AutoSize = true;
            labelBankFilterCount.Location = new Point(3, 172);
            labelBankFilterCount.Name = "labelBankFilterCount";
            labelBankFilterCount.Size = new Size(75, 15);
            labelBankFilterCount.TabIndex = 11;
            labelBankFilterCount.Text = "Bank Count: ";
            // 
            // checkBoxShowCompareResults
            // 
            checkBoxShowCompareResults.AutoSize = true;
            checkBoxShowCompareResults.Location = new Point(231, 7);
            checkBoxShowCompareResults.Name = "checkBoxShowCompareResults";
            checkBoxShowCompareResults.Size = new Size(112, 19);
            checkBoxShowCompareResults.TabIndex = 24;
            checkBoxShowCompareResults.Text = "Show in VSCode";
            checkBoxShowCompareResults.UseVisualStyleBackColor = true;
            checkBoxShowCompareResults.CheckedChanged += checkBoxShowCompareResults_CheckedChanged;
            // 
            // linkLabelOpenFolder
            // 
            linkLabelOpenFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            linkLabelOpenFolder.Location = new Point(390, 125);
            linkLabelOpenFolder.Name = "linkLabelOpenFolder";
            linkLabelOpenFolder.Size = new Size(83, 15);
            linkLabelOpenFolder.TabIndex = 14;
            linkLabelOpenFolder.TabStop = true;
            linkLabelOpenFolder.Text = "Open Folder";
            linkLabelOpenFolder.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelOpenFolder.LinkClicked += linkLabelOpenFolder_LinkClicked;
            // 
            // labelResult
            // 
            labelResult.AutoSize = true;
            labelResult.Location = new Point(3, 125);
            labelResult.Name = "labelResult";
            labelResult.Size = new Size(101, 15);
            labelResult.TabIndex = 13;
            labelResult.Text = "Gemini API Result";
            // 
            // labelPrompt
            // 
            labelPrompt.AutoSize = true;
            labelPrompt.Location = new Point(3, 40);
            labelPrompt.Name = "labelPrompt";
            labelPrompt.Size = new Size(350, 15);
            labelPrompt.TabIndex = 12;
            labelPrompt.Text = "Prompt - {prompt below} {product 1 JSON} and {product 2 JSON}";
            // 
            // richTextBoxPrompt
            // 
            richTextBoxPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            richTextBoxPrompt.Location = new Point(3, 58);
            richTextBoxPrompt.Name = "richTextBoxPrompt";
            richTextBoxPrompt.Size = new Size(469, 58);
            richTextBoxPrompt.TabIndex = 8;
            richTextBoxPrompt.Text = "";
            // 
            // buttonCompare
            // 
            buttonCompare.Enabled = false;
            buttonCompare.Location = new Point(138, 4);
            buttonCompare.Name = "buttonCompare";
            buttonCompare.Size = new Size(84, 23);
            buttonCompare.TabIndex = 7;
            buttonCompare.Text = "Compare";
            buttonCompare.UseVisualStyleBackColor = true;
            buttonCompare.Click += buttonCompare_Click;
            // 
            // richTextBoxPromptResults
            // 
            richTextBoxPromptResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBoxPromptResults.Location = new Point(3, 143);
            richTextBoxPromptResults.Name = "richTextBoxPromptResults";
            richTextBoxPromptResults.Size = new Size(469, 238);
            richTextBoxPromptResults.TabIndex = 0;
            richTextBoxPromptResults.Text = "";
            // 
            // listViewProductValues
            // 
            listViewProductValues.Columns.AddRange(new ColumnHeader[] { columnHeaderProperty, columnHeaderValue });
            listViewProductValues.Dock = DockStyle.Fill;
            listViewProductValues.FullRowSelect = true;
            listViewProductValues.Location = new Point(0, 0);
            listViewProductValues.Name = "listViewProductValues";
            listViewProductValues.Size = new Size(447, 583);
            listViewProductValues.TabIndex = 2;
            listViewProductValues.UseCompatibleStateImageBehavior = false;
            listViewProductValues.View = View.Details;
            // 
            // columnHeaderProperty
            // 
            columnHeaderProperty.Text = "Property";
            columnHeaderProperty.Width = 220;
            // 
            // columnHeaderValue
            // 
            columnHeaderValue.Text = "Value";
            columnHeaderValue.Width = 1000;
            // 
            // buttonFilterProducts
            // 
            buttonFilterProducts.Location = new Point(141, 217);
            buttonFilterProducts.Name = "buttonFilterProducts";
            buttonFilterProducts.Size = new Size(84, 23);
            buttonFilterProducts.TabIndex = 6;
            buttonFilterProducts.Text = "Filter";
            buttonFilterProducts.UseVisualStyleBackColor = true;
            buttonFilterProducts.Click += buttonFilterProducts_Click;
            // 
            // labelSearchProducts
            // 
            labelSearchProducts.AutoSize = true;
            labelSearchProducts.Location = new Point(167, 16);
            labelSearchProducts.Name = "labelSearchProducts";
            labelSearchProducts.Size = new Size(45, 15);
            labelSearchProducts.TabIndex = 6;
            labelSearchProducts.Text = "Search:";
            // 
            // linkLabelResetProducts
            // 
            linkLabelResetProducts.Location = new Point(529, 19);
            linkLabelResetProducts.Name = "linkLabelResetProducts";
            linkLabelResetProducts.Size = new Size(35, 15);
            linkLabelResetProducts.TabIndex = 5;
            linkLabelResetProducts.TabStop = true;
            linkLabelResetProducts.Text = "Reset";
            linkLabelResetProducts.TextAlign = ContentAlignment.MiddleCenter;
            linkLabelResetProducts.LinkClicked += linkLabelResetProducts_LinkClicked;
            // 
            // buttonSearchProducts
            // 
            buttonSearchProducts.Location = new Point(469, 12);
            buttonSearchProducts.Name = "buttonSearchProducts";
            buttonSearchProducts.Size = new Size(54, 23);
            buttonSearchProducts.TabIndex = 4;
            buttonSearchProducts.Text = "Go";
            buttonSearchProducts.UseVisualStyleBackColor = true;
            buttonSearchProducts.Click += buttonSearchProducts_Click;
            // 
            // textBoxSearchProducts
            // 
            textBoxSearchProducts.Location = new Point(218, 12);
            textBoxSearchProducts.Name = "textBoxSearchProducts";
            textBoxSearchProducts.Size = new Size(245, 23);
            textBoxSearchProducts.TabIndex = 3;
            textBoxSearchProducts.Text = ".0569";
            textBoxSearchProducts.KeyDown += textBoxSearchProducts_KeyDown;
            // 
            // richTextBoxError
            // 
            richTextBoxError.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBoxError.BackColor = Color.Red;
            richTextBoxError.ForeColor = Color.White;
            richTextBoxError.Location = new Point(12, 978);
            richTextBoxError.Name = "richTextBoxError";
            richTextBoxError.Size = new Size(1718, 36);
            richTextBoxError.TabIndex = 22;
            richTextBoxError.Text = "Text";
            // 
            // statusStripMain
            // 
            statusStripMain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelBankCount, toolStripStatusLabelSpacer1, toolStripStatusLabelProductCount, toolStripStatusLabelSpacer2, toolStripStatusLabelMessage });
            statusStripMain.Location = new Point(0, 1020);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(1858, 22);
            statusStripMain.TabIndex = 9;
            statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelBankCount
            // 
            toolStripStatusLabelBankCount.Name = "toolStripStatusLabelBankCount";
            toolStripStatusLabelBankCount.Size = new Size(81, 17);
            toolStripStatusLabelBankCount.Text = "Bank Count: 0";
            // 
            // toolStripStatusLabelSpacer1
            // 
            toolStripStatusLabelSpacer1.Name = "toolStripStatusLabelSpacer1";
            toolStripStatusLabelSpacer1.Size = new Size(10, 17);
            toolStripStatusLabelSpacer1.Text = " ";
            // 
            // toolStripStatusLabelProductCount
            // 
            toolStripStatusLabelProductCount.Name = "toolStripStatusLabelProductCount";
            toolStripStatusLabelProductCount.Size = new Size(97, 17);
            toolStripStatusLabelProductCount.Text = "Product Count: 0";
            // 
            // toolStripStatusLabelSpacer2
            // 
            toolStripStatusLabelSpacer2.Name = "toolStripStatusLabelSpacer2";
            toolStripStatusLabelSpacer2.Size = new Size(10, 17);
            toolStripStatusLabelSpacer2.Text = " ";
            // 
            // toolStripStatusLabelMessage
            // 
            toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            toolStripStatusLabelMessage.Size = new Size(1645, 17);
            toolStripStatusLabelMessage.Spring = true;
            toolStripStatusLabelMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripMain
            // 
            toolStripMain.ImageScalingSize = new Size(24, 24);
            toolStripMain.Items.AddRange(new ToolStripItem[] { toolStripLabelSpacer1, toolStripButtonSettings });
            toolStripMain.Location = new Point(0, 24);
            toolStripMain.Name = "toolStripMain";
            toolStripMain.Size = new Size(1858, 31);
            toolStripMain.TabIndex = 10;
            // 
            // toolStripLabelSpacer1
            // 
            toolStripLabelSpacer1.Name = "toolStripLabelSpacer1";
            toolStripLabelSpacer1.Size = new Size(10, 28);
            toolStripLabelSpacer1.Text = " ";
            // 
            // toolStripButtonSettings
            // 
            toolStripButtonSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButtonSettings.Image = (Image)resources.GetObject("toolStripButtonSettings.Image");
            toolStripButtonSettings.ImageTransparentColor = Color.Magenta;
            toolStripButtonSettings.Name = "toolStripButtonSettings";
            toolStripButtonSettings.Size = new Size(28, 28);
            toolStripButtonSettings.Text = "Settings";
            toolStripButtonSettings.Click += toolStripButtonSettings_Click;
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, optionsToolStripMenuItem, helpToolStripMenuItem });
            menuStripMain.Location = new Point(0, 0);
            menuStripMain.Name = "menuStripMain";
            menuStripMain.Size = new Size(1858, 24);
            menuStripMain.TabIndex = 11;
            menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(116, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // timerMain
            // 
            timerMain.Interval = 5000;
            timerMain.Tick += timerMain_Tick;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1858, 1042);
            Controls.Add(toolStripMain);
            Controls.Add(richTextBoxError);
            Controls.Add(statusStripMain);
            Controls.Add(menuStripMain);
            Controls.Add(tabControlMain);
            Controls.Add(buttonClose);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripMain;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Open Banking Products";
            FormClosing += FormMain_FormClosing;
            Load += FormMain_Load;
            tabControlMain.ResumeLayout(false);
            tabPageCache.ResumeLayout(false);
            splitContainerCache.Panel1.ResumeLayout(false);
            splitContainerCache.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCache).EndInit();
            splitContainerCache.ResumeLayout(false);
            tabPageProducts.ResumeLayout(false);
            tabPageProducts.PerformLayout();
            splitContainerProducts.Panel1.ResumeLayout(false);
            splitContainerProducts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerProducts).EndInit();
            splitContainerProducts.ResumeLayout(false);
            splitContainerProductCounts.Panel1.ResumeLayout(false);
            splitContainerProductCounts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerProductCounts).EndInit();
            splitContainerProductCounts.ResumeLayout(false);
            splitContainerProductDetails.Panel1.ResumeLayout(false);
            splitContainerProductDetails.Panel2.ResumeLayout(false);
            splitContainerProductDetails.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductDetails).EndInit();
            splitContainerProductDetails.ResumeLayout(false);
            panelTermDeposits.ResumeLayout(false);
            panelTermDeposits.PerformLayout();
            splitContainerProductProperties.Panel1.ResumeLayout(false);
            splitContainerProductProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerProductProperties).EndInit();
            splitContainerProductProperties.ResumeLayout(false);
            splitContainerProductComparison.Panel1.ResumeLayout(false);
            splitContainerProductComparison.Panel1.PerformLayout();
            splitContainerProductComparison.Panel2.ResumeLayout(false);
            splitContainerProductComparison.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerProductComparison).EndInit();
            splitContainerProductComparison.ResumeLayout(false);
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            toolStripMain.ResumeLayout(false);
            toolStripMain.PerformLayout();
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonGetProducts;
        private Button buttonClose;
        private Button buttonRefreshCache;
        private RichTextBox richTextBoxData;
        private ListView listViewProducts;
        private ColumnHeader columnHeaderProductsID;
        private ColumnHeader columnHeaderProductsBank;
        private ColumnHeader columnHeaderProductsProduct;
        private ColumnHeader columnHeaderFeeCount;
        private TabControl tabControlMain;
        private TabPage tabPageProducts;
        private TabPage tabPageCache;
        private ListView listViewBanks;
        private ColumnHeader columnHeaderCacheBankName;
        private ColumnHeader columnHeaderCacheOpenBankingURL;
        private ColumnHeader columnHeaderListVersion;
        private ColumnHeader columnHeaderDetailVersion;
        private ColumnHeader columnHeaderCacheLogoURL;
        private FolderBrowserDialog folderBrowserDialogCache;
        private RichTextBox richTextBoxCachResults;
        private LinkLabel linkLabelResetProducts;
        private Button buttonSearchProducts;
        private TextBox textBoxSearchProducts;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel toolStripStatusLabelBankCount;
        private ToolStripStatusLabel toolStripStatusLabelSpacer1;
        private ToolStripStatusLabel toolStripStatusLabelProductCount;
        private ToolStripStatusLabel toolStripStatusLabelSpacer2;
        private ToolStrip toolStripMain;
        private ToolStripLabel toolStripLabelSpacer1;
        private ToolStripButton toolStripButtonSettings;
        private MenuStrip menuStripMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Label labelSearchProducts;
        private ToolStripStatusLabel toolStripStatusLabelMessage;
        private LinkLabel linkLabelOpenCacheFolder;
        private SplitContainer splitContainerProducts;
        private SplitContainer splitContainerCache;
        private ListView listViewProductValues;
        private ColumnHeader columnHeaderProperty;
        private ColumnHeader columnHeaderValue;
        private SplitContainer splitContainerProductDetails;
        private ListView listViewFilteredProducts;
        private ColumnHeader columnFilterBankName;
        private ColumnHeader columnHeaderFilterProduct;
        private ColumnHeader columnHeaderFilterProductID;
        private Button buttonFilterProducts;
        private System.Windows.Forms.Timer timerMain;
        private Label labelProductFilterCount;
        private Label labelBankFilterCount;
        private SplitContainer splitContainerProductProperties;
        private SplitContainer splitContainerProductCounts;
        private ListView listViewProductCounts;
        private ColumnHeader columnHeaderPropertyCount;
        private ColumnHeader columnHeaderValueCount;
        private RichTextBox richTextBoxError;
        private SplitContainer splitContainerProductComparison;
        private RichTextBox richTextBoxPromptResults;
        private Button buttonCompare;
        private RichTextBox richTextBoxPrompt;
        private Label labelPrompt;
        private Label labelResult;
        private LinkLabel linkLabelOpenFolder;
        private LinkLabel linkLabelSelectNone;
        private LinkLabel linkLabelSelectAll;
        private CheckBox checkBoxShowCompareResults;
        private ComboBox comboBoxProductCategory;
        private Label labelProductCategory;
        private ColumnHeader columnHeaderProductsCategory;
        private ColumnHeader columnHeaderProductsRateType;
        private Panel panelTermDeposits;
        private Label labelDepositDepositTerm;
        private CheckedListBox checkedDepositListBoxTerm;
        private LinkLabel linkLabelTermNone;
        private LinkLabel linkLabelTermsAll;
        private ColumnHeader columnHeaderCacheVersion;
        private ColumnHeader columnHeaderCacheVersionMin;
        private Button buttonDeleteCache;
        private LinkLabel linkLabelReloadCandidateBanks;
        private TextBox textBoxDepositDepositAmount;
        private Label labelDepositDepositAmount;
        private Button buttonTestAddUser;
        private Button buttonTestGetUsers;
        private ColumnHeader columnHeaderCacheDisplayOrder;
        private ColumnHeader columnHeaderCacheActive;
        private ColumnHeader columnHeaderLastProductsRefreshed;
        private ColumnHeader columnHeaderCacheLastRefrehed;
        private ColumnHeader columnHeaderCacheLastBankUpdate;
        private ColumnHeader columnHeaderCacheLastRefrehedDays;
        private ColumnHeader columnHeaderCacheLastBankUpdateDays;
        private ProgressBar progressBarCache;
    }
}