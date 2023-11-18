﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Text.RegularExpressions;

namespace RecipeManager
{
    public partial class frmRecipeManager : Form
    {
        /* 
           Idea - Sara Walker
           Design - Sara Walker 
           Implementation - Sara Walker, Aaron White
           Testing - Sara Walker, Aaron White 
        */

        // Global string of where the user's files are kept, this is usually in the %APPDATA% (C:/Users/<user>/AppData/Roaming) - Sara Walker
        public string UserRecipesPath = Application.UserAppDataPath;

        // Global RecipeFilePath string to be used when needed to access the current recipe file - Sara Walker
        public string RecipeFilePath;

        // Global CurrentRecipe string to be used when constructing the RecipeFilePath - Sara Walker
        public string CurrentRecipe = "";

        public frmRecipeManager()
        {
            InitializeComponent();

            // Set the first line of both the ingredients and the directions to bulleted - Sara Walker
            rtxtIngredients.SelectionBullet = true;
            rtxtDirections.SelectionBullet = true;
        }

        private void Load_Files()
        {
            // Iterate through each file in the directory - Sara Walker
            foreach (string recipeFile in Directory.GetFiles(UserRecipesPath))
            {
                // Construct an array of the file path split at each `\`, use escape character to reach - Sara Walker
                string[] recipeFileArray = recipeFile.Split('\\');

                // Construct a string using the last index in the array and replacing dashes with spaces and removing the `.recipe` - Sara Walker
                string recipeFileString = recipeFileArray[recipeFileArray.Length - 1].Replace("-", " ").Replace(".recipe", "");

                // Add recipeFileString to the listbox - Sara Walker
                lbRecipeList.Items.Add(recipeFileString);
            }
        }

        // Method to generate a recipe path when needed - Sara Walker
        // Assigning RecipeFilePath to hold CurrentRecipe file name, and files end with `.recipe` - Sara Walker
        private void GenerateRecipePath() => RecipeFilePath = $"{UserRecipesPath}\\{CurrentRecipe.Replace(" ", "-")}.recipe";

        private void ClearContentBoxes()
        {
            // Clear text in the name and decription boxes and reset count on the ingredients and directions boxes - Sara Walker
            txtRecipeName.Text = "";
            rtxtDescription.Text = "";
            rtxtIngredients.Text = "";
            rtxtDirections.Text = "";
        }

        private string IsPresent(TextBox textBox, string name)
        {
            return "";
        }

        private bool IsValidData()
        {
            return true;
        }

        private void frmRecipeManager_Load(object sender, EventArgs e)
        {
            // This is where we load any existing files on startup - Sara Walker
            Load_Files();
        }

        // Call ClearContentBoxes when creating a new recipe - Sara Walker
        private void tsmiCreateNew_Click(object sender, EventArgs e) => ClearContentBoxes();

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            // Clear CurrentRecipe variable (if needed) - Sara Walker
            CurrentRecipe = "";

            // Strings to display in the message box - Sara Walker
            string msg = "Please enter a file name";
            string caption = "Missing File Name";

            // If there is text in the text box - Sara Walker
            if (txtRecipeName.Text != "")
            {
                // Set CurrentRecipe variable to text in recipe name textbox - Sara Walker
                CurrentRecipe = txtRecipeName.Text;

                // Call GenerateRecipePath to have RecipeFilePath hold CurrentRecipe - Sara Walker
                GenerateRecipePath();
                // Check if the file exists - Sara Walker
                if (File.Exists(RecipeFilePath))
                {   // Strings to display in the message box - Sara Walker
                    string msg2 = "Would you like to rewrite file";
                    string caption2 = "File exists";

                    // Yes or no buttons - Sara Walker
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                    // Get result from the message box - Sara Walker
                    DialogResult result = MessageBox.Show(msg2, caption2, buttons);

                    // If the result equals yes, clear the contents, delete the file, and remove the recipe name from listbox - Sara Walker
                    if (result == DialogResult.Yes)
                    {

                    }
                }
                // Add recipe name to the listbox - Sara Walker
                lbRecipeList.Items.Add(txtRecipeName.Text);

                // Construct an array that is the size of the current number of lines in the richtextbox - Sara Walker
                string[] ingredientArray = new string[rtxtIngredients.Lines.Length];

                // For loop that iterates through each line in the richtextbox - Sara Walker
                for (int i = 0; i < rtxtIngredients.Lines.Length; i++)
                {
                    // Get the first character of the current line iteration - Sara Walker
                    int currentLineStart = rtxtIngredients.GetFirstCharIndexFromLine(i);

                    // Set the cursor to the current line iteration - Sara Walker
                    rtxtIngredients.SelectionStart = currentLineStart;

                    // If the line does not have a bullet point, place a bullet point - Sara Walker
                    if (!rtxtIngredients.SelectionBullet)
                    {
                        rtxtIngredients.SelectionBullet = true;
                    }

                    // Place a "~" in front of the current line iteration - Sara Walker
                    ingredientArray[i] = "~" + rtxtIngredients.Lines[i];
                }

                // Join the ingredient array with "\n" between each count - Sara Walker
                string formattedIngredients = string.Join("\n", ingredientArray);

                // Write to the file, the new formatted ingredient list - Sara Walker
                File.WriteAllText(RecipeFilePath, formattedIngredients);
            }
            else MessageBox.Show(msg, caption);




            /* Code Features */

            //string example = $"Name:{txtRecipeName.Text}\n" +
            //                 $"Description:{rtxtDescription.Text}\n" +
            //                 $"Ingredients:1/4_tsp-Black_Pepper|1_tbsp-Olive_Oil" + //This is an example of how ingredients will be displayed in the file
            //                 $"Directions: {rtxtDirections.Text}";

            // Construct a string to be saved to the file
            // Save contents of text boxes to recipe file
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            // Call GenerateRecipePath to have RecipeFilePath hold CurrentRecipe (from SelectedValueChange) - Sara Walker
            GenerateRecipePath();

            // Check if the file exists - Sara Walker
            if (File.Exists(RecipeFilePath))
            {
                // Strings to display in the message box - Sara Walker
                string msg = "Are you sure you would like to delete this recipe?";
                string caption = "Delete Recipe";

                // Yes or no buttons - Sara Walker
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                // Get result from the message box - Sara Walker
                DialogResult result = MessageBox.Show(msg, caption, buttons);

                // If the result equals yes, clear the contents, delete the file, and remove the recipe name from listbox - Sara Walker
                if (result == DialogResult.Yes)
                {
                    ClearContentBoxes();

                    // Delete file - Sara Walker
                    File.Delete(RecipeFilePath);

                    // Remove CurrentRecipe from the listbox - Sara Walker
                    lbRecipeList.Items.Remove(CurrentRecipe);

                    // Set selected item to null - Sara Walker
                    lbRecipeList.SelectedItem = null;
                }
            }
            // If the file does not exist, display an error message - Sara Walker
            else
            {
                MessageBox.Show("File not found", "Error");
            }
        }

        private void lbRecipeList_SelectedValueChanged(object sender, EventArgs e)
        {
            // Gaurd clause for if the selected item in the list box is null, do nothing - Sara Walker
            if (lbRecipeList.SelectedItem == null) return;

            // Clear CurrentRecipe variable - Sara Walker
            CurrentRecipe = "";

            // Set CurrentRecipe variable to the selected item in the listbox as a string - Sara Walker
            CurrentRecipe = lbRecipeList.SelectedItem.ToString();


            /* Code Features */

            // Load respective recipe file contents into text boxes

        }

        // This event handler is attached to the ingredients and direction richtextboxes - Sara Walker
        private void rtxt_KeyDown(object sender, KeyEventArgs e)
        {
            // Set the sender to a RichTextBox - Sara Walker
            RichTextBox richTextBox = (RichTextBox)sender;

            // If the user presses "control b" - Sara Walker
            if (e.Control && e.KeyCode == Keys.B)
            {
                // Bullet tne selected line - Sara Walker
                richTextBox.SelectionBullet = true;
            }
        }

        // Close form when pressing escape - Sara Walker
        private void btnExit_Click(object sender, EventArgs e) => Close();

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show frmHelp when clicked apply frmRecipeManager to the parameter - Sara Walker
            frmHelp frmHelp = new frmHelp(this);
            frmHelp.Show();
        }
    }
}
