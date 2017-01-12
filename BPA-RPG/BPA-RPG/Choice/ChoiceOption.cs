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
        public readonly Dictionary<GameItem, int> requirements;
        public readonly bool invisible;

        private readonly Action action;

        /// <summary>
        /// Creates a new ChoiceOption ojbect
        /// </summary>
        /// <param name="synopsis">Option synopsis</param>
        /// <param name="actions">Actions that execution on selection of this option</param>
        public ChoiceOption(string synopsis, Action action, Dictionary<GameItem, int> requirements, bool invisible, ScreenManager manager)
        {
            this.synopsis = synopsis;
            this.action = action;
            this.requirements = requirements;
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
            foreach (KeyValuePair<GameItem, int> item in requirements)
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
            Dictionary<GameItem, int> requirements = new Dictionary<GameItem, int>();
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
                    case "require":
                        {
                            int count;
                            if (!int.TryParse(lineParts[2], out count))
                                break;

                            requirements.Add(GameItem.Parse(lineParts[1]), count);
                        }
                        break;

                    case "invisible":
                        invisible = true;
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
                        if (!int.TryParse(lineParts[1], out level))
                            break;

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

            return new ChoiceOption(synopsis, action, requirements, invisible, manager);
        }
    }
}
