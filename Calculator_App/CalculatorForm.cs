using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator_App
{
    public partial class CalculatorForm : Form
    {
        //variables
        private string userInput = ""; //used to track the user's inputs
        private bool isResultDisplayed = false; //used to determine if the result is currently displayed 
        private double prevAns = 0; //used to track the previous answers
        DataTable dataTable = new DataTable();

        //methods
        private void addSpecialChar(string toBeInputted)
        {
            userInput = displayBox.Text;

            if (!userInput.EndsWith(toBeInputted) && !userInput.EndsWith(" "))
            {
                addToDisplay(toBeInputted);
            }
        }


        private void addToDisplay(string toBeInputted)
        {
            //check if the display needs to be cleared first
            if (shouldClear())
            {
                displayBox.Text = "";
                isResultDisplayed = false;
            }

            //check if toBeInputted is a mathematical symbol
            if (Regex.IsMatch(toBeInputted, @"^[-+×÷]$"))
            {
                toBeInputted = $" {toBeInputted} ";
            }
            else
            {
                userInput = displayBox.Text;

                if (userInput.EndsWith("²"))
                {
                    toBeInputted = "";
                }
            }

            displayBox.Text += toBeInputted;            
            centerTextBox();
        }

        private void centerTextBox()
        {
            displayBox.SelectAll();
            displayBox.SelectionAlignment = HorizontalAlignment.Right;
        }

        private bool shouldClear()
        {
            if (displayBox.Text == "0") //the calculator is displaying the default 0
            {
                return true;
            }
            else if (isResultDisplayed == true)
            {
                isResultDisplayed = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public CalculatorForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        //on load set the following settings
        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            centerTextBox();
            prevAns = 0.0;
        }

        private void zeroBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("0");
        }

        private void oneBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("1");
        }

        private void twoBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("2");
        }

        private void threeBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("3");
        }

        private void fourBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("4");
        }

        private void fiveBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("5");
        }

        private void sixBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("6");
        }

        private void sevenBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("7");
        }

        private void eightBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("8");
        }

        private void nineBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("9");
        }

        private void decimalBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar(".");
        }

        private void sqrtBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("√");
        }

        private void squareBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar("²");
        }

        private void prevAnsBttn_Click(object sender, EventArgs e)
        {
            addToDisplay(prevAns.ToString());
        }

        private void leftBraceBttn_Click(object sender, EventArgs e)
        {
            addToDisplay("(");
        }

        private void rightBraceBttn_Click(object sender, EventArgs e)
        {
            addToDisplay(")");
        }

        private void clearAllBttn_Click(object sender, EventArgs e)
        {
            displayBox.Text = "0";
            centerTextBox();
        }

        private void deleteBttn_Click(object sender, EventArgs e)
        {
            //get the user input
            userInput = displayBox.Text;

            //check if the input is not on the default
            if (userInput!="0" && userInput.Length>0)
            {
                //if the last character is a space then we need to remove an operand which is 3 chars (two spaces and an operand char)
                if (userInput.EndsWith(" "))
                {
                    displayBox.Text = userInput.Substring(0, userInput.Length - 3);
                }
                else
                {
                    //remove the last character in the string
                    displayBox.Text = userInput.Substring(0, userInput.Length - 1);
                }

                if (displayBox.Text.Length == 0)
                {
                    displayBox.Text = "0"; //make the text the default
                }
            }
            else
            {
                displayBox.Text = "0"; //make the text the default
            }

            centerTextBox();
        }

        private void divideBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar("÷");
        }

        private void multiplyBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar("×");
        }

        private void addBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar("+");
        }

        private void subtractBttn_Click(object sender, EventArgs e)
        {
            addSpecialChar("-");
        }

        private String replaceRoots(String userInput)
        {
            userInput = Regex.Replace(userInput, @"(√+)(\d+)", match =>
            {
                int sqrtCount = match.Groups[1].Value.Length;
                double number = double.Parse(match.Groups[2].Value);

                // Check if sqrtCount is greater than 0 to avoid Math.Pow with negative exponent
                return sqrtCount > 0 ? Math.Pow(number, 1.0 / Math.Pow(2, sqrtCount)).ToString() : number.ToString();
            });

            return userInput;
        }

        private String replaceAdjacentBraces(String subExpression)
        {
            //replace nrs in the format (x)(b) and x(b)
            subExpression = Regex.Replace(subExpression, @"\)\(", ")*(");
            subExpression = Regex.Replace(subExpression, @"\d\(", match => match.Value[0] + "*(");
            
            return subExpression;
        }

        private void equalsBttn_Click(object sender, EventArgs e)
        {
            userInput = displayBox.Text;

            try
            {
                // Replace unsupported symbols
                userInput = userInput.Replace(" ", "");
                userInput = userInput.Replace("÷", "/");
                userInput = userInput.Replace("×", "*");

                userInput = replaceAdjacentBraces(userInput);

                // Evaluate expressions within parentheses first
                userInput = EvaluateParentheses(userInput);

                // Replace square root (√) with Math.Sqrt
                userInput = replaceRoots(userInput);

                // Replace squared (²) with Math.Pow(x, 2)
                userInput = Regex.Replace(userInput, @"(\d+)²", match => Math.Pow(double.Parse(match.Groups[1].Value), 2).ToString());

                // Use DataTable.Compute to evaluate the modified expression
                var result = dataTable.Compute(userInput, "");

                // Set the result to the display box
                displayBox.Text = result.ToString();
                isResultDisplayed = true;
                centerTextBox();

                // Store the result in the answers array for future reference
                prevAns = Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during the computation
                displayBox.Text = $"Error: {userInput}";
            }
        }

        private string EvaluateParentheses(string expression)
        {
            while (expression.Contains("(") && expression.Contains(")"))
            {
                int openIndex = expression.LastIndexOf('(');
                int closeIndex = expression.IndexOf(')', openIndex);

                if (openIndex == -1 || closeIndex == -1 || closeIndex <= openIndex)
                {
                    throw new InvalidOperationException("Mismatched parentheses.");
                }

                string subExpression = expression.Substring(openIndex + 1, closeIndex - openIndex - 1);

                // Replace square root (√) with Math.Sqrt
                subExpression = replaceRoots(subExpression);

                // Replace squared (²) with Math.Pow(x, 2)
                subExpression = Regex.Replace(subExpression, @"(\d+)²", match => Math.Pow(double.Parse(match.Groups[1].Value), 2).ToString());

                double result = Convert.ToDouble(dataTable.Compute(subExpression, ""));

                expression = expression.Remove(openIndex, closeIndex - openIndex + 1).Insert(openIndex, result.ToString());
            }

            return expression;
        }

    }
}
