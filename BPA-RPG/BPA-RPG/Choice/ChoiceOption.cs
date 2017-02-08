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
        public readonly string synopsis;
        public readonly Dictionary<GameItem, int> itemRequirements;
        public readonly Dictionary<Currency, int> moneyRequirements;
        public readonly Dictionary<string, int> varRequirements;
        public readonly bool invisible;

        private readonly Action action;

        /// <summary>
        /// Creates a new ChoiceOption ojbect
        /// </summary>
        /// <param name="synopsis">Option synopsis</param>
        /// <param name="actions">Actions that execution on selection of this option</param>
        public ChoiceOption(string synopsis, Action action, Dictionary<GameItem, int> itemRequirements, Dictionary<Currency, int> moneyRequirements, Dictionary<string, int> varRequirements, bool invisible, ScreenManager manager)
        {
            this.synopsis = synopsis;
            this.action = action;
            this.itemRequirements = itemRequirements;
            this.moneyRequirements = moneyRequirements;
            this.varRequirements = varRequirements;
            this.invisible = invisible;
        }

        /// <summary>
        /// Runs all actions
        /// </summary>
        public void Activate()
        {
            //Make sure requirements are met, just in case
            if (MeetsRequirements())
                action?.Invoke();
        }

        /// <summary>
        /// Checks if the player meets this options requirements
        /// </summary>
        /// <returns></returns>
        public bool MeetsRequirements()
        {
            foreach (KeyValuePair<GameItem, int> item in itemRequirements)
            {
                //If positive, check if player has at least that amount
                if (item.Value > 0)
                {
                    if (PlayerData.inventory.Count(i => i == item.Key) < item.Value)
                        return false;
                }
                //If number is negative or 0, check if player has at MOST that amount
                else if (PlayerData.inventory.Count(i => i == item.Key) > Math.Abs(item.Value))
                    return false;
            }

            foreach (KeyValuePair<Currency, int> money in moneyRequirements)
            {
                //If positive, check if player has at least that amount
                if (money.Value > 0)
                {
                    if (PlayerData.GetMoney(money.Key) < money.Value)
                        return false;
                }
                //If number is negative or 0, check if player has at MOST that amount
                else if (PlayerData.GetMoney(money.Key) > Math.Abs(money.Value))
                        return false;
            }

            foreach (KeyValuePair<string, int> variable in varRequirements)
            {
                //If positive, check if player has at least that var value
                if (variable.Value > 0)
                {
                    if (PlayerData.GetVariable(variable.Key) < variable.Value)
                        return false;
                }
                //If number is negative or 0, check if player has at MOST that var value
                else if (PlayerData.GetVariable(variable.Key) > Math.Abs(variable.Value))
                    return false;
            }

            return true;
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
            Action action = null;
            Dictionary<GameItem, int> itemRequirements = new Dictionary<GameItem, int>();
            Dictionary<Currency, int> moneyRequirements = new Dictionary<Currency, int>();
            Dictionary<string, int> varRequirements = new Dictionary<string, int>();
            bool invisible = false;

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
                    case "invisible":
                        invisible = true;
                        break;

                    case "require":
                        {
                            lineParts[1] = lineParts[1].ToLower();

                            if (lineParts[1] == "var")
                            {
                                int value;
                                if (!int.TryParse(lineParts[3], out value))
                                    break;

                                varRequirements.Add(lineParts[2], value);
                                break;
                            }

                            int count;
                            if (!int.TryParse(lineParts[2], out count))
                                break;

                            switch (lineParts[1])
                            {
                                case "credits":
                                    moneyRequirements.Add(Currency.credits, count);
                                    break;

                                case "jex":
                                    moneyRequirements.Add(Currency.jex, count);
                                    break;

                                default:
                                    itemRequirements.Add(GameItem.Parse(lineParts[1]), count);
                                    break;
                            }
                        }
                        break;
                        
                    case "var":
                        {
                            int count;
                            if (!int.TryParse(lineParts[3], out count))
                                break;

                            switch (lineParts[1].ToLower())
                            {
                                case "set":
                                    action += () => PlayerData.SetVariable(lineParts[2], count);
                                    break;

                                case "add":
                                    action += () => PlayerData.SetVariable(lineParts[2], PlayerData.GetVariable(lineParts[2]) + count);
                                    break;
                            }
                        }
                        break;

                    case "credits":
                        {
                            int amount;
                            if (!int.TryParse(lineParts[1], out amount))
                                break;

                            action += () => PlayerData.AddMoney(Currency.credits, amount);
                        }
                        break;

                    case "jex":
                        {
                            int amount;
                            if (!int.TryParse(lineParts[1], out amount))
                                break;

                            action += () => PlayerData.AddMoney(Currency.jex, amount);
                        }
                        break;

                    case "get":
                        {
                            int count;
                            if (!int.TryParse(lineParts[2], out count))
                                break;

                            action += () =>
                            {
                                for (int i = 0; i < count; i++)
                                    PlayerData.inventory.Add(GameItem.Parse(lineParts[1]));
                            };
                        }
                        break;

                    case "remove":
                        {
                            int count;
                            if (!int.TryParse(lineParts[2], out count))
                                break;

                            action += () =>
                            {
                                for (int i = 0; i < count; i++)
                                    PlayerData.inventory.Remove(GameItem.Parse(lineParts[1]));
                            };
                        }
                        break;
                    case "enemy":
                        int level;
                        float pos = Math.Abs(PlayerData.ship.position.X) + Math.Abs(PlayerData.ship.position.Y);

                        if (pos < 10000)
                            level = 1;
                        else if (pos < 30000)
                            level = 2;
                        else level = 3;

                        action += () => manager.Push(new BattleScreen(PlayerData.ship, level));
                        break;
                    case "return":
                        action += () => screen.currentChoice = screen.currentChoice.baseChoice;
                        break;
                    case "choice":
                        lineNum += 2;

                        if (lineNum >= lines.Count)
                            endOfLine = true;

                        List<string> choiceLines = new List<string>();

                        while (!endOfLine)
                        {
                            choiceLines.Add(lines[lineNum]);
                            lineNum++;

                            if (lineNum >= lines.Count)
                                endOfLine = true;
                        }
                        lineNum++;

                        action += () => screen.currentChoice = MenuChoice.ChoiceFromText(screen, choiceLines, manager, screen.currentChoice.baseChoice);

                        break;
                    case "exit":
                        action += () => manager.Pop();
                        break;
                }

                lineNum++;
                if (lineNum >= lines.Count)
                    endOfLine = true;
            }

            return new ChoiceOption(synopsis, action, itemRequirements, moneyRequirements, varRequirements, invisible, manager);
        }
    }
}
