using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if(ValidateForm())
            {
                PrizeModel model = new PrizeModel(
                    placeNumberValue.Text,
                    placeNameValue.Text,
                    prizeAmountValue.Text,
                    prizePercentageValue.Text
                    );

                GlobalConfig.Connection.CreatePrize(model);

                placeNumberValue.Text = String.Empty;
                placeNameValue.Text = String.Empty;
                prizeAmountValue.Text = "0";
                prizePercentageValue.Text = "0";


            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again");
            }
        }

        private bool ValidateForm()
        {
            int placeNumber = 0;
            decimal prizeAmount = 0;
            double prizePercentage = 0;

            if (!int.TryParse(placeNumberValue.Text, out placeNumber))
            {
                return false;
            }

            if (placeNumber < 1)
            {
                return false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                return false;
            }

            if (placeNameValue.Text.Length == 0)
            {
                return false;
            }

            if (!decimal.TryParse(prizeAmountValue.Text, out prizeAmount) || !double.TryParse(prizePercentageValue.Text, out prizePercentage))
            {
                return false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                return false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                return false;
            }

            return true;
        }

    }
}
