//-----------------------------------------------------------------------
// <copyright file="YearGroupCreatorScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can create new year groups.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can create new year groups.
    /// </summary>
    public class YearGroupCreatorScreen
    {
        /// <summary>
        /// The last index of the input fields.
        /// </summary>
        private const int MaxSelection = 1;

        /// <summary>
        /// The position of the first selected input field.
        /// </summary>
        private int[] firstSelectionPosition;

        /// <summary>
        /// An array of error messages for each input field.
        /// </summary>
        private string[] errorMessages;

        /// <summary>
        /// An array of entered values for each input field.
        /// </summary>
        private string[] inputValues;

        /// <summary>
        /// The current index of the input fields.
        /// </summary>
        private int currentSelection;

        /// <summary>
        /// Used for handling user input.
        /// </summary>
        private InputHandler inputHandler;

        /// <summary>
        /// New year group, which will be returned after successful user input.
        /// </summary>
        private YearGroup newYearGroup;

        /// <summary>
        /// Referent, who is the creator of the year group.
        /// </summary>
        private Referent creator;

        /// <summary>
        /// If this boolean is true, the user created a year group which already exists.
        /// </summary>
        private bool sameYearGroup;

        /// <summary>
        /// As long as this boolean is false, the screen waits for user input.
        /// </summary>
        private bool savePressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="YearGroupCreatorScreen"/> class.
        /// </summary>
        public YearGroupCreatorScreen()
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.inputValues = new string[YearGroupCreatorScreen.MaxSelection + 1];
            this.errorMessages = new string[YearGroupCreatorScreen.MaxSelection + 1];

            this.Reset();

            this.inputHandler = new InputHandler();

            this.inputHandler.SubscribeForKey(ConsoleKey.UpArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.DownArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.Enter);
            this.inputHandler.SubscribeForKey(ConsoleKey.Tab);
            this.inputHandler.SubscribeForKey(ConsoleKey.F1);
            this.inputHandler.SubscribeForKey(ConsoleKey.F2);
            this.inputHandler.SubscribeForKey(ConsoleKey.Escape);

            this.inputHandler.OnSubscribedKeyCalled += this.InputHandler_OnSubscribedKeyCalled;
        }

        /// <summary>
        /// Draws the screen on the console.
        /// </summary>
        public void Draw()
        {
            Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [Up / Down] Navigate");
            Console.WriteLine("\n - Create new year group\n");
            Console.WriteLine("    [ ] Year:        {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Description: {0}\n", this.inputValues[1]);

            for (int i = 0; i < this.errorMessages.Length; i++)
            {
                if (!this.errorMessages[i].Equals(string.Empty))
                {
                    Console.WriteLine("    ERROR: {0}", this.errorMessages[i]);
                }
            }

            if (this.sameYearGroup)
            {
                Console.WriteLine("    ERROR: This referent already created a year group with the same values!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and returns a new year group.
        /// </summary>
        /// <param name="creator">A referent, who is logged in.</param>
        /// <returns>A new year group created by the referent.</returns>
        public YearGroup GetNewAgeGroupFromInput(Referent creator)
        {
            this.newYearGroup = new YearGroup();

            this.creator = creator;

            while (!this.savePressed)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            if (this.newYearGroup != null)
            {
                this.creator.AddYearGroup(this.newYearGroup);
            }

            this.Reset();

            return this.newYearGroup;
        }

        /// <summary>
        /// Resets the selection, all input fields and error messages.
        /// </summary>
        public void Reset()
        {
            this.currentSelection = 0;

            for (int i = 0; i < this.inputValues.Length; i++)
            {
                this.inputValues[i] = this.errorMessages[i] = string.Empty;
            }

            this.savePressed = false;
            this.sameYearGroup = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            // Move the cursor to the input field.
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 16, this.firstSelectionPosition[1] + (this.currentSelection * 2));
            
            this.errorMessages[this.currentSelection] = string.Empty;

            switch (this.currentSelection)
            {
                case 0:
                    this.inputHandler.ReadChars(ref this.inputValues[0], 4);

                    break;
                case 1:
                    this.inputHandler.ReadChars(ref this.inputValues[1], 20);

                    break;
            }

            this.ApplySettings();
        }

        /// <summary>
        /// Is called when the user presses one of the subscribed keys.
        /// </summary>
        /// <param name="key">Subscribed key.</param>
        private void InputHandler_OnSubscribedKeyCalled(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.DownArrow:
                case ConsoleKey.Enter:
                case ConsoleKey.Tab:
                    if (this.currentSelection < YearGroupCreatorScreen.MaxSelection)
                    {
                        this.currentSelection++;
                    }
                    else
                    {
                        this.currentSelection = 0;
                    }

                    break;
                case ConsoleKey.UpArrow:
                    if (this.currentSelection > 0)
                    {
                        this.currentSelection--;
                    }
                    else
                    {
                        this.currentSelection = YearGroupCreatorScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    if (this.ApplySettings())
                    {
                        if (!this.creator.YearGroups.Contains(this.newYearGroup))
                        {
                            this.savePressed = true;
                            this.sameYearGroup = false;
                        }
                        else
                        {
                            this.sameYearGroup = true;
                        }
                    }

                    break;
                case ConsoleKey.Escape:
                    this.newYearGroup = null;
                    this.savePressed = true;
                    break;
            }
        }

        /// <summary>
        /// Shows the help to this screen.
        /// </summary>
        private void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine(" - Help\n");
            Console.WriteLine("     To create a new year group, fill in all input fields.");
            Console.WriteLine("     Make sure the entered year group doesn't exist yet!");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - Year: Number, which must contain 4 digits.\n");
            Console.WriteLine("       - Description (UNIQUE!): Must contain at least 2 characters.\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Applies the filled input fields to the new year group.
        /// </summary>
        /// <returns>A boolean, indicating whether the input values are valid or not.</returns>
        private bool ApplySettings()
        {
            if (this.newYearGroup != null)
            {
                int index = 0;

                try
                {
                    this.newYearGroup.SetYear(this.inputValues[0]);
                    this.newYearGroup.SetDescription(this.inputValues[index = 1]);

                    return true;
                }
                catch (ArgumentException ex)
                {
                    this.errorMessages[index] = ex.Message;

                    return false;
                }
            }

            return false;
        }
    }
}
