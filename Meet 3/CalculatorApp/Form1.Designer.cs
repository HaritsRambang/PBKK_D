#nullable enable

namespace CalculatorApp;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private Label lblBrand = null!;
    private Label lblExpression = null!;
    private Label lblStatus = null!;
    private TextBox txtDisplay = null!;
    private TableLayoutPanel buttonGrid = null!;
    private Button btnHistory = null!;
    private Button btnBackToCalculator = null!;
    private ListBox lstHistory = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblBrand = new Label();
        lblExpression = new Label();
        lblStatus = new Label();
        txtDisplay = new TextBox();
        buttonGrid = new TableLayoutPanel();
        btnHistory = new Button();
        btnBackToCalculator = new Button();
        lstHistory = new ListBox();
        SuspendLayout();

        BackColor = Color.FromArgb(18, 25, 32);
        ClientSize = new Size(420, 760);
        Font = new Font("Segoe UI", 10F);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(420, 760);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Calculator Sakti";
        KeyPreview = true;
        KeyDown += Form1_KeyDown;

        lblBrand.AutoSize = true;
        lblBrand.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        lblBrand.ForeColor = Color.FromArgb(82, 226, 190);
        lblBrand.Location = new Point(28, 24);
        lblBrand.Text = "CALCULATOR SAKTI";

        lblExpression.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblExpression.Font = new Font("Segoe UI", 10F);
        lblExpression.ForeColor = Color.FromArgb(148, 165, 175);
        lblExpression.Location = new Point(28, 78);
        lblExpression.Size = new Size(360, 24);
        lblExpression.TextAlign = ContentAlignment.MiddleRight;

        txtDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtDisplay.BackColor = Color.FromArgb(25, 36, 45);
        txtDisplay.BorderStyle = BorderStyle.None;
        txtDisplay.Font = new Font("Segoe UI", 30F, FontStyle.Bold);
        txtDisplay.ForeColor = Color.White;
        txtDisplay.Location = new Point(24, 105);
        txtDisplay.ReadOnly = true;
        txtDisplay.Size = new Size(372, 54);
        txtDisplay.TabStop = false;
        txtDisplay.Text = "0";
        txtDisplay.TextAlign = HorizontalAlignment.Right;

        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Segoe UI", 9F);
        lblStatus.ForeColor = Color.FromArgb(148, 165, 175);
        lblStatus.Location = new Point(28, 174);
        lblStatus.Text = "Ready · keyboard enabled";

        buttonGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        buttonGrid.BackColor = Color.FromArgb(18, 25, 32);
        buttonGrid.ColumnCount = 4;
        buttonGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        buttonGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        buttonGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        buttonGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        buttonGrid.Location = new Point(24, 210);
        buttonGrid.Padding = new Padding(0);
        buttonGrid.RowCount = 7;
        for (int row = 0; row < 7; row++)
        {
            buttonGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857F));
        }
        buttonGrid.Size = new Size(372, 470);

        btnHistory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnHistory.BackColor = Color.FromArgb(55, 113, 211);
        btnHistory.FlatStyle = FlatStyle.Flat;
        btnHistory.FlatAppearance.BorderSize = 0;
        btnHistory.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnHistory.ForeColor = Color.White;
        btnHistory.Location = new Point(24, 690);
        btnHistory.Size = new Size(372, 42);
        btnHistory.Text = "HISTORY";
        btnHistory.UseVisualStyleBackColor = false;
        btnHistory.Click += btnHistory_Click;

        btnBackToCalculator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        btnBackToCalculator.BackColor = Color.FromArgb(55, 113, 211);
        btnBackToCalculator.FlatStyle = FlatStyle.Flat;
        btnBackToCalculator.FlatAppearance.BorderSize = 0;
        btnBackToCalculator.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        btnBackToCalculator.ForeColor = Color.White;
        btnBackToCalculator.Location = new Point(24, 690);
        btnBackToCalculator.Size = new Size(372, 42);
        btnBackToCalculator.Text = "BACK TO CALCULATOR";
        btnBackToCalculator.UseVisualStyleBackColor = false;
        btnBackToCalculator.Visible = false;
        btnBackToCalculator.Click += btnBackToCalculator_Click;

        lstHistory.BackColor = Color.FromArgb(25, 36, 45);
        lstHistory.BorderStyle = BorderStyle.FixedSingle;
        lstHistory.Font = new Font("Segoe UI", 9F);
        lstHistory.ForeColor = Color.White;
        lstHistory.FormattingEnabled = true;
        lstHistory.ItemHeight = 17;
        lstHistory.Location = new Point(24, 520);
        lstHistory.Size = new Size(372, 150);
        lstHistory.Visible = false;

        Controls.Add(buttonGrid);
        Controls.Add(lstHistory);
        Controls.Add(btnHistory);
        Controls.Add(btnBackToCalculator);
        Controls.Add(lblStatus);
        Controls.Add(txtDisplay);
        Controls.Add(lblExpression);
        Controls.Add(lblBrand);
        ResumeLayout(false);
        PerformLayout();
    }
}
