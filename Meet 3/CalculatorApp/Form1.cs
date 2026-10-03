using System.Globalization;
using System.Media;

namespace CalculatorApp;

public partial class Form1 : Form
{
    private decimal firstNumber;
    private string operation = string.Empty;
    private bool isNewInput = true;
    private bool hasResult;
    private readonly List<string> history = [];

    private static readonly Color NumberColor = Color.FromArgb(31, 45, 55);
    private static readonly Color UtilityColor = Color.FromArgb(52, 67, 77);
    private static readonly Color OperatorColor = Color.FromArgb(37, 125, 113);
    private static readonly Color EqualsColor = Color.FromArgb(82, 226, 190);

    public Form1()
    {
        InitializeComponent();
        BuildButtons();
    }

    private void BuildButtons()
    {
        string[,] keys =
        {
            { "C", "Back", "%", "/" },
            { "7", "8", "9", "x" },
            { "4", "5", "6", "-" },
            { "1", "2", "3", "+" },
            { "0", ".", "=", "=" },
            { "sin", "cos", "tan", "sqrt" },
            { "x²", "%", "±", "C" }
        };

        for (int row = 0; row < keys.GetLength(0); row++)
        {
            for (int column = 0; column < keys.GetLength(1); column++)
            {
                string key = keys[row, column];
                Button button = new()
                {
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                    Margin = new Padding(5),
                    Text = key,
                    UseVisualStyleBackColor = false,
                    Tag = key
                };
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 88, 98);
                button.BackColor = NumberColor;
                button.ForeColor = Color.White;

                if (key is "C" or "Back" or "%")
                {
                    button.BackColor = UtilityColor;
                    button.ForeColor = Color.FromArgb(210, 224, 228);
                }
                else if (key is "/" or "x" or "-" or "+")
                {
                    button.BackColor = OperatorColor;
                }
                else if (key is "sin" or "cos" or "tan" or "sqrt" or "x²" or "±")
                {
                    button.BackColor = Color.FromArgb(232, 225, 216);
                    button.ForeColor = Color.FromArgb(74, 66, 61);
                    button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else if (key == "=")
                {
                    button.BackColor = EqualsColor;
                    button.ForeColor = Color.FromArgb(13, 44, 40);
                    button.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                }

                button.Click += CalculatorButton_Click;
                buttonGrid.Controls.Add(button, column, row);
                if (row == 4 && column == 3)
                {
                    button.Visible = false;
                }
            }
        }
    }

    private void CalculatorButton_Click(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.Tag is not string key)
        {
            return;
        }

        if (char.IsDigit(key[0]))
        {
            EnterNumber(key);
        }
        else if (key == ".")
        {
            EnterDecimal();
        }
        else if (key is "+" or "-" or "x" or "/")
        {
            SetOperation(key);
        }
        else if (key == "=")
        {
            CalculateResult();
        }
        else if (key == "C")
        {
            ClearCalculator();
        }
        else if (key == "Back")
        {
            DeleteLastCharacter();
        }
        else if (key == "%")
        {
            ApplyPercentage();
        }
        else if (key is "sin" or "cos" or "tan" or "sqrt" or "x²")
        {
            ApplyScientificOperation(key);
        }
        else if (key == "±")
        {
            ToggleSign();
        }
    }

    private void EnterNumber(string number)
    {
        if (isNewInput || hasResult)
        {
            txtDisplay.Text = number;
            isNewInput = false;
            hasResult = false;
        }
        else if (txtDisplay.Text != "0")
        {
            txtDisplay.Text += number;
        }
        else
        {
            txtDisplay.Text = number;
        }

        SetStatus("Typing number");
    }

    private void EnterDecimal()
    {
        if (isNewInput || hasResult)
        {
            txtDisplay.Text = "0.";
            isNewInput = false;
            hasResult = false;
        }
        else if (!txtDisplay.Text.Contains('.'))
        {
            txtDisplay.Text += ".";
        }
    }

    private void SetOperation(string selectedOperation)
    {
        if (!TryReadDisplay(out decimal currentNumber))
        {
            ShowError("Input angka belum valid.");
            return;
        }

        if (!string.IsNullOrEmpty(operation) && !isNewInput)
        {
            firstNumber = Calculate(firstNumber, currentNumber, operation);
            txtDisplay.Text = FormatNumber(firstNumber);
        }
        else
        {
            firstNumber = currentNumber;
        }

        operation = selectedOperation;
        isNewInput = true;
        hasResult = false;
        lblExpression.Text = $"{FormatNumber(firstNumber)} {selectedOperation}";
        SetStatus("Choose the next number");
    }

    private void CalculateResult()
    {
        if (string.IsNullOrEmpty(operation))
        {
            SetStatus("Nothing to calculate");
            return;
        }

        if (!TryReadDisplay(out decimal secondNumber))
        {
            ShowError("Masukkan angka kedua terlebih dahulu.");
            return;
        }

        try
        {
            decimal result = Calculate(firstNumber, secondNumber, operation);
            lblExpression.Text = $"{FormatNumber(firstNumber)} {operation} {FormatNumber(secondNumber)} =";
            txtDisplay.Text = FormatNumber(result);
            AddHistory(lblExpression.Text + " " + FormatNumber(result));
            firstNumber = result;
            operation = string.Empty;
            isNewInput = true;
            hasResult = true;
            SetStatus("Calculation complete");
        }
        catch (DivideByZeroException)
        {
            ShowError("Tidak dapat membagi dengan nol.");
        }
    }

    private static decimal Calculate(decimal left, decimal right, string selectedOperation)
    {
        return selectedOperation switch
        {
            "+" => left + right,
            "-" => left - right,
            "x" => left * right,
            "/" when right != 0 => left / right,
            "/" => throw new DivideByZeroException(),
            _ => right
        };
    }

    private void ApplyPercentage()
    {
        if (TryReadDisplay(out decimal currentNumber))
        {
            txtDisplay.Text = FormatNumber(currentNumber / 100);
            isNewInput = false;
            SetStatus("Percentage applied");
        }
    }

    private void ApplyScientificOperation(string selectedOperation)
    {
        if (!TryReadDisplay(out decimal currentNumber))
        {
            ShowError("Input angka belum valid.");
            return;
        }

        try
        {
            double value = (double)currentNumber;
            double result = selectedOperation switch
            {
                "sin" => Math.Sin(value * Math.PI / 180),
                "cos" => Math.Cos(value * Math.PI / 180),
                "tan" => Math.Tan(value * Math.PI / 180),
                "sqrt" when value >= 0 => Math.Sqrt(value),
                "sqrt" => throw new ArgumentException("Akar kuadrat membutuhkan angka positif."),
                "x²" => value * value,
                _ => value
            };

            txtDisplay.Text = FormatNumber((decimal)result);
            lblExpression.Text = $"{selectedOperation}({FormatNumber(currentNumber)})";
            AddHistory(lblExpression.Text + " = " + txtDisplay.Text);
            isNewInput = true;
            hasResult = true;
            SetStatus("Scientific function applied");
        }
        catch (ArgumentException exception)
        {
            ShowError(exception.Message);
        }
    }

    private void ToggleSign()
    {
        if (TryReadDisplay(out decimal currentNumber) && currentNumber != 0)
        {
            txtDisplay.Text = FormatNumber(-currentNumber);
        }
    }

    private void AddHistory(string entry)
    {
        history.Insert(0, entry);
        if (history.Count > 8)
        {
            history.RemoveAt(history.Count - 1);
        }

        RefreshHistory();
    }

    private void RefreshHistory()
    {
        lstHistory.Items.Clear();
        foreach (string entry in history)
        {
            lstHistory.Items.Add(entry);
        }
    }

    private void btnHistory_Click(object? sender, EventArgs e)
    {
        ShowHistory();
    }

    private void btnBackToCalculator_Click(object? sender, EventArgs e)
    {
        ShowCalculator();
    }

    private void ShowHistory()
    {
        buttonGrid.Visible = false;
        btnHistory.Visible = false;
        lstHistory.Visible = true;
        btnBackToCalculator.Visible = true;
        SetStatus("Calculation history");
    }

    private void ShowCalculator()
    {
        lstHistory.Visible = false;
        btnBackToCalculator.Visible = false;
        btnHistory.Visible = true;
        buttonGrid.Visible = true;
        SetStatus("Ready · keyboard enabled");
    }

    private void DeleteLastCharacter()
    {
        if (isNewInput || hasResult)
        {
            return;
        }

        txtDisplay.Text = txtDisplay.Text.Length > 1
            ? txtDisplay.Text[..^1]
            : "0";
        SetStatus("Last digit deleted");
    }

    private void ClearCalculator()
    {
        firstNumber = 0;
        operation = string.Empty;
        isNewInput = true;
        hasResult = false;
        txtDisplay.Text = "0";
        lblExpression.Text = string.Empty;
        SetStatus("Ready · keyboard enabled");
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is >= Keys.D0 and <= Keys.D9 || e.KeyCode is >= Keys.NumPad0 and <= Keys.NumPad9)
        {
            string digit = e.KeyCode.ToString().Replace("D", string.Empty).Replace("NumPad", string.Empty);
            EnterNumber(digit);
        }
        else if (e.KeyCode is Keys.Add or Keys.Oemplus && !e.Shift)
        {
            SetOperation("+");
        }
        else if (e.KeyCode is Keys.Subtract or Keys.OemMinus)
        {
            SetOperation("-");
        }
        else if (e.KeyCode is Keys.Multiply || e.KeyCode == Keys.D8 && e.Shift)
        {
            SetOperation("x");
        }
        else if (e.KeyCode is Keys.Divide or Keys.OemQuestion)
        {
            SetOperation("/");
        }
        else if (e.KeyCode is Keys.Enter or Keys.Return)
        {
            CalculateResult();
        }
        else if (e.KeyCode is Keys.Decimal or Keys.OemPeriod or Keys.Oemcomma)
        {
            EnterDecimal();
        }
        else if (e.KeyCode == Keys.Back)
        {
            DeleteLastCharacter();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            ClearCalculator();
        }
        else
        {
            return;
        }

        e.SuppressKeyPress = true;
        e.Handled = true;
    }

    private bool TryReadDisplay(out decimal number)
    {
        return decimal.TryParse(txtDisplay.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out number);
    }

    private static string FormatNumber(decimal number)
    {
        return number.ToString("G29", CultureInfo.InvariantCulture);
    }

    private void ShowError(string message)
    {
        lblStatus.ForeColor = Color.FromArgb(255, 135, 135);
        lblStatus.Text = message;
        SystemSounds.Exclamation.Play();
    }

    private void SetStatus(string message)
    {
        lblStatus.ForeColor = Color.FromArgb(148, 165, 175);
        lblStatus.Text = message;
    }
}
