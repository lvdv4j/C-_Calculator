using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
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
        private string result = ""; //used to display the calculated result
        private bool isResultDisplayed = false; //used to determine if the result is currently displayed
        private double[] answers = new double[1]; //used to track the latest and previous answers
        private double prevAns = 0;

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
                //isResultDisplayed = false;
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
        }

        //on load set the following settings
        private void CalculatorForm_Load(object sender, EventArgs e)
        {
            centerTextBox();
            answers[0] = 0.0;
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
            addToDisplay(answers[0].ToString());
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

        private void equalsBttn_Click(object sender, EventArgs e)
        {

        }
    }
}
