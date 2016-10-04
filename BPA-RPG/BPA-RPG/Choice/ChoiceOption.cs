using BPA_RPG.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.Choice
{
    /// <summary>
    /// Represents an option for a choice
    /// </summary>
    public class ChoiceOption
    {
        public List<string> synopsis { get; private set; }
        private List<Action> actions;

        public ChoiceOption(List<string> synopsis, List<Action> actions)
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
        /// <param name="lines">Lines of text that represent an option</param>
        /// <returns></returns>
        public static ChoiceOption OptionFromText(List<string> lines)
        {
            int lineNum = 0;
            bool endOfLine = false;
            List<string> synopsis = new List<string>();
            List<Action> actions = new List<Action>();

            //Loop through synopsis lines
            while (lines[lineNum].StartsWith(">") && !endOfLine)
            {
                synopsis.Add(lines[lineNum]);
                lineNum++;

                if (lineNum > lines.Count)
                    endOfLine = true;
            }

            //Loop through action lines
            while (!endOfLine)
            {
                string[] lineParts = lines[lineNum].Split(new string[] { " " }, StringSplitOptions.None);

                switch (lineParts[0].ToLower())
                {
                    case "credits":
                        int amount;
                        if (!int.TryParse(lineParts[1], out amount))
                            break;

                        actions.Add(() => PlayerData.credits += amount);
                        break;

                    case "get":
                        actions.Add(() => GetItem(lineParts[1]));
                        break;

                    case "remove":
                        actions.Add(() => RemoveItem(lineParts[1]));
                        break;
                }

                lineNum++;
                if (lineNum >= lines.Count)
                    endOfLine = true;
            }

            return new ChoiceOption(synopsis, actions);
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
