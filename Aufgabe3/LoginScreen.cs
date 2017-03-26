//-----------------------------------------------------------------------
// <copyright file="LoginScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can login with some credentials.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can login with some credentials.
    /// </summary>
    public class LoginScreen
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
        /// As long as this boolean is false, the screen will be displayed.
        /// </summary>
        private bool loginPressed;

        /// <summary>
        /// If this boolean is true, the authentication failed.
        /// </summary>
        private bool loginFailed;

        /// <summary>
        /// New referent, which will be returned after successful login.
        /// </summary>
        private Referent loggedInReferent;

        /// <summary>
        /// List of existing referents to check username and password.
        /// </summary>
        private List<Referent> referents;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginScreen"/> class.
        /// </summary>
        /// <param name="referents">List of referents, which already exist.</param>
        public LoginScreen(List<Referent> referents)
        {
            this.referents = referents;

            this.firstSelectionPosition = new int[] { 5, 5 };

            this.inputValues = new string[LoginScreen.MaxSelection + 1];

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
            Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Login  [Up / Down] Navigate");
            Console.WriteLine("\n - Login\n");
            Console.WriteLine("    [ ] ID - Number: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Password:    {0}\n", this.inputValues[1]);

            if (this.loginFailed)
            {
                Console.WriteLine("    ERROR: Wrong combination of ID - number and password!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and returns the logged in referent.
        /// </summary>
        /// <returns>A referent, who is logged in to the system.</returns>
        public Referent GetLoggedInReferent()
        {
            this.loggedInReferent = null;

            while (!this.loginPressed)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            this.Reset();

            return this.loggedInReferent;
        }

        /// <summary>
        /// Resets the selection and all input fields.
        /// </summary>
        public void Reset()
        {
            this.currentSelection = 0;

            for (int i = 0; i < this.inputValues.Length; i++)
            {
                this.inputValues[i] = string.Empty;
            }

            this.loginPressed = false;
            this.loginFailed = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            // Move the cursor to the input field.
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 16, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            // Depending on the input field, the handler reads a certain amount of letters.
            switch (this.currentSelection)
            {
                case 0:
                    this.inputHandler.ReadChars(ref this.inputValues[0], 5);

                    break;
                case 1:
                    this.inputHandler.ReadChars(ref this.inputValues[1], 20);

                    break;
            }
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
                    // The user wants to move the selection cursor to the bottom.
                    if (this.currentSelection < LoginScreen.MaxSelection)
                    {
                        this.currentSelection++;
                    }
                    else
                    {
                        this.currentSelection = 0;
                    }

                    break;
                case ConsoleKey.UpArrow:
                    // The user wants to move the selection cursor to the top.
                    if (this.currentSelection > 0)
                    {
                        this.currentSelection--;
                    }
                    else
                    {
                        this.currentSelection = LoginScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    // The user wants to login with a combination of username and password.
                    Referent foundReferent = null;

                    for (int i = 0; i < this.referents.Count; i++)
                    {
                        if (this.referents[i].ID.Equals(this.inputValues[0]) && this.referents[i].IsMatchingPassword(this.inputValues[1]))
                        {
                            foundReferent = this.referents[i];
                        }
                    }

                    if (foundReferent != null)
                    {
                        this.loggedInReferent = foundReferent;
                        this.loginPressed = true;
                        this.loginFailed = false;
                    }
                    else
                    {
                        this.loginFailed = true;
                    }

                    break;
                case ConsoleKey.Escape:
                    // The user wants to leave the screen without logging in.
                    this.loginPressed = true;
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
            Console.WriteLine("     To login, fill in all input fields. Make sure the referent exists!");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - ID: Number, which must be between 10000 and 99999.\n");
            Console.WriteLine("       - Password: Must contain at least 4 characters.\n");
            Console.ReadLine();
        }
    }
}
