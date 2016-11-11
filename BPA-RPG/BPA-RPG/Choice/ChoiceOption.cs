using BPA_RPG.GameItems;
using BPA_RPG.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.Choice
{
    /// <summary>
    /// Represents an option in a MenuChoice
    /// </summary>
    public class ChoiceOption
    {
        public string synopsis { get; private set; }

        private List<Action> actions;

        /// <summary>
        /// Creates a new ChoiceOption ojbect
        /// </summary>
        /// <param name="synopsis">Option synopsis</param>
        /// <param name="actions">Actions that execution on selection of this option</param>
        public ChoiceOption(string synopsis, List<Action> actions, ScreenManager manager)
        {
            this.synopsis = synopsis;
            this.actions = actions;
        }

        /// <summary>
        /// Runs all actions
        /// </summary>
        public void Activate()
        {
            foreach (Action action in actions)
                action();
        }


        /// <summary>
        /// Creates a ChoiceOption object from text
        /// </summary>
        /// <param name="screen">Screen that holds this option</param>
        /// <param name="lines">Lines of text that represent an option</param>
        /// <returns></returns>
        public static ChoiceOption OptionFromText(MenuChoiceScreen screen, List<string> lines, ScreenManager manager)
        {
            int lineNum = 0;
            bool endOfLine = false;
            string synopsis;
            List<Action> actions = new List<Action>();

            synopsis = lines[lineNum];
            lineNum++;

            if (lineNum >= lines.Count)
                endOfLine = true;

            //Loop through action lines
            while (!endOfLine)
            {
                string[] lineParts = lines[lineNum].Split(new string[] { " " }, StringSplitOptions.None);

                switch (lineParts[0].ToLower())
                {
                    case "credits":
                        {
                            int amount;
                            if (!int.TryParse(lineParts[1], out amount))
                                break;

                            actions.Add(() => PlayerData.AddMoney(Currency.credits, amount));
                        }
                        break;

                    case "jex":
                        {
                            int amount;
                            if (!int.TryParse(lineParts[1], out amount))
                                break;

                            actions.Add(() => PlayerData.AddMoney(Currency.jex, amount));
                        }
                        break;

                    case "get":
                        actions.Add(() => GetItem(lineParts[1]));
                        break;

                    case "remove":
                        actions.Add(() => RemoveItem(lineParts[1]));
                        break;
                    case "enemy":
                        int level;
                        if (!int.TryParse(lineParts[1], out level))
                            break;

                        actions.Add(() => manager.Push(new BattleScreen(PlayerData.ship, level)));
                        break;
                    case "return":
                        actions.Add(() => screen.currentChoice = screen.currentChoice.baseChoice);
                        break;
                    case "choice":
                        lineNum += 2;

                        if (lineNum >= lines.Count)
                            endOfLine = true;

                        List<string> choiceLines = new List<string>();

                        while (!endOfLine && !lines[lineNum].StartsWith("]"))
                        {
                            choiceLines.Add(lines[lineNum]);
                            lineNum++;

                            if (lineNum >= lines.Count)
                                endOfLine = true;
                        }
                        lineNum++;

                        actions.Add(() => screen.currentChoice = MenuChoice.ChoiceFromText(screen, choiceLines, manager, screen.currentChoice.baseChoice));

                        break;
                    case "exit":
                        actions.Add(() => manager.Pop());
                        break;
                }

                lineNum++;
                if (lineNum >= lines.Count)
                    endOfLine = true;
            }

            return new ChoiceOption(synopsis, actions, manager);
        }

        /// <summary>
        /// Gives the player the item specified
        /// </summary>
        /// <param name="item">Item to give to player, in string form</param>
        private static void GetItem(string itemString)
        {

        }

        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="item">Item to remove, in string form</param>
        private static void RemoveItem(string itemString)
        {
            
        }
    }
}
