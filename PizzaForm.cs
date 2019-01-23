using System;
using System.Windows.Forms;

namespace PizzaDelivery
{

    /// *********************************************************
    /// <summary>
    /// Project: Pizza Delivery
    /// Task: 1) Split Class PizzaForm initializes app 
    ///       2) Create event handlers for form and button
    ///       3) Create blocks of logic and methods to obtain 
    ///         and print user input
    /// </summary>
    /// *********************************************************
    public partial class PizzaForm : Form
    {
        private bool drinkValid = true;
        private bool infoValid = true;
        private string order;
        private double totalCost;

        public PizzaForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set initial values on load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PizzaForm_Load(object sender, EventArgs e)
        {
            this.cmboSize.SelectedIndex = 0;
            this.cmboCrust.SelectedIndex = 0;
            this.cmboSpecialty.SelectedIndex = 0;
            this.txtBxSodaQty.Text = "0";
            this.txtBxWaterQty.Text = "0";
            order = "";
            totalCost = 0;
        }

        /// <summary>
        /// Button event handler gathers strings and calls 
        /// methods to validate and calculate totals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            double drinkCost,
            pizzaCost,
            specialtyCost;
            order = "";

            infoValid = GetCustomerInfo();

            try
            {
                order += cmboSize.SelectedItem.ToString() + "\n";
            }
            catch (NullReferenceException)
            {
                order += "No Pizza Ordered";
            }

            try
            {
                order += cmboCrust.SelectedItem.ToString() + " - ";
            }
            catch (NullReferenceException)
            {
                order += "";
            }
            
            if (ckBxItems.SelectedItems.Count > 0)
            {
                order += "Toppings: ";
                foreach (string s in ckBxItems.CheckedItems)
                {
                    order += "\n" + s;
                }
            }

            pizzaCost = DeterminePizzaCost();
            if (pizzaCost != 0)
            {
                order += "\nPizza Total: " + pizzaCost.ToString("C");
            }

            drinkCost = DetermineDrinkCost();
            if (drinkCost != 0)
            {
                order += "\nDrink Total: " + drinkCost.ToString("C");
            }

            specialtyCost = DeterminSpecialtyCost();
            if (specialtyCost != 0)
            {
                order += "\n\nSpecialty Items:\n" + cmboSpecialty.SelectedItem.ToString();
            }

            order += "\nAmount Due: " + (pizzaCost + drinkCost + specialtyCost).ToString("C");

            if (drinkValid && infoValid)
            {
                lblMsgName.Text = "";
                lblMsgAddress.Text = "";
                lblMsgPhone.Text = "";
                lblErrorMsg.Text = "";
                MessageBox.Show(order, "Cheech's Pizza Express Order");
            }
            else
            {
                if (!drinkValid)
                {
                    tabControl1.SelectTab(1);
                }
                if (!infoValid)
                {
                    Focus();
                }          
            }
        }

        /// <summary>
        /// Method enforces required field input for user info, 
        ///     builds string with input
        /// </summary>
        /// <returns> infoValid </returns>
        private bool GetCustomerInfo()
        {
            bool nameFilled = true;
            bool addressFilled = true;
            bool phoneFilled = true;

            if(txtBxName.Text == "")
            {              
                lblMsgName.Text = "Required Field";
                nameFilled = false;               
            }
            order += "Order for:\n\n" + "\t" + txtBxName.Text + "\n";

            if(txtBxAddress.Text == "")
            {              
                lblMsgAddress.Text = "Required Field";
                addressFilled = false;   
            }
            order += "\t" + txtBxAddress.Text + "\n";

            if(txtBxPhone.Text == "")
            {              
                lblMsgPhone.Text = "Required Field";
                phoneFilled = false;               
            }
            order += "\t" + txtBxPhone.Text + "\n\n";

            return infoValid = nameFilled & addressFilled & phoneFilled;
        }

        /// <summary>
        /// Method validates numeric input and calculates totals
        /// </summary>
        /// <returns> totalCost </returns>
        private double DetermineDrinkCost()
        {
            double sodaCnt,
                waterCnt;
            bool waterValid = true;
            bool sodaValid = true;
            totalCost = 0;
            if (txtBxSodaQty.Text != "")
            {
                while (double.TryParse(txtBxSodaQty.Text, out sodaCnt) == false)
                {
                    lblErrorMsg.Text = "Quantity must be numeric";
                    txtBxSodaQty.Text = "0";
                    sodaValid = false;
                }
                
                order += "\nDrink Selections: \n" + sodaCnt + " soda(s)";
                totalCost = sodaCnt * 1.75;
            }
            if (txtBxWaterQty.Text != "")
            {
                while (double.TryParse(txtBxWaterQty.Text, out waterCnt) == false)
                {
                    lblErrorMsg.Text = "Quantity must be numeric";
                    txtBxWaterQty.Text = "0";
                    waterValid = false;
                }
                
                order += "\n" + waterCnt + " water(s)";  
            }
            drinkValid = sodaValid && waterValid;
            return totalCost;
        }

        /// <summary>
        /// Method calculates totals with user selection
        /// </summary>
        /// <returns> cost + checkbox item totals </returns>
        public double DeterminePizzaCost()
        {
            double cost = 0;
            if (cmboSize.SelectedIndex == 0)
            {
                cost = 9.95;
            }
            else
            {
                if (cmboSize.SelectedIndex == 1)
                {
                    cost = 12.95;
                }
                else
                {
                    if (cmboSize.SelectedIndex == 2)
                    {
                        cost = 15.95;
                    }
                    else
                    {
                        cost = 0;
                    }
                }
            }


            //if (cmboSize.SelectedIndex == 0)
            //{
            //    //cost = 0; 
            //    cost = 9.95;
            //}

            //else 
            //if (cmboSize.SelectedIndex == 1)
            //{
            //    //cost = 9.95;
            //    cost = 12.95;
            //}
            //if (cmboSize.SelectedIndex == 2)
            //{
            //    //cost = 12.95;
            //    cost = 15.95;
            //}

            return cost + (ckBxItems.CheckedItems.Count * 1.50);
        }

        /// <summary>
        /// Method calculates totals with user selection
        /// </summary>
        /// <returns> cost </returns>
        public double DeterminSpecialtyCost()
        {
            double cost = 0;
            if(cmboSpecialty.SelectedIndex == 1)
            {
                cost = 5.00;
            }
            else
            {
                if(cmboSpecialty.SelectedIndex == 2)
                {
                    cost = 4.00;
                }
                else
                {
                    if(cmboSpecialty.SelectedIndex == 3)
                    {
                        cost = 3.00;
                    }
                    else
                    {
                        cost = 0;
                    }
                }
            }
            return cost;
        }
    }
}
