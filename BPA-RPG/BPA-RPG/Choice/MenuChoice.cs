using BPA_RPG.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.Choice
{
    /// <summary>
    /// Represents a choice in a menu
    /// </summary>
    public class MenuChoice
    {
        public MenuChoice baseChoice;

        public List<string> synopsis { get; private set; }
        public List<ChoiceOption> options { get; private set; }
        
        /// <summary>
        /// Creates a new MenuChoice object
        /// </summary>
        /// <param name="synopsis">Choice synopsis</param>
        /// <param name="options">Possible options to choose from</param>
        public MenuChoice(List<string> synopsis, List<ChoiceOption> options)
        {
            this.synopsis = synopsis;
            this.options = options;

            baseChoice = this;
        }

        /// <summary>
        /// Creates a new MenuChoice object
        /// </summary>
        /// <param name="synopsis">Choice synopsis</param>
        /// <param name="options">Possible options to choose from</param>
        /// <param name="baseChoice">Base choice in a choice tree</param>
        public MenuChoice(List<string> synopsis, List<ChoiceOption> options, MenuChoice baseChoice)
        {
            this.synopsis = synopsis;
            this.options = options;

            this.baseChoice = baseChoice;
        }

        /// <summary>
        /// Creates a MenuChoice object from text.
        /// </summary>
        /// <param name="screen">Screen that holds this choice</param>
        /// <param name="lines">Lines of text that represent a choice</param>
        /// <param name="baseChoice">The original choice in a choice tree</param>
        /// <returns></returns>
        public static MenuChoice ChoiceFromText(MenuChoiceScreen screen, List<string> lines, ScreenManager manager, MenuChoice baseChoice = null)
        {
            int lineNum = 0;
            bool endOfLine = false;
            List<string> synopsis = new List<string>();
            List<ChoiceOption> options = new List<ChoiceOption>();

            //Loop through lines that make up the Choice synopsis
            while (!endOfLine && !lines[lineNum].ToLower().StartsWith(">"))
            {
                synopsis.Add(lines[lineNum]);
                lineNum++;

                if (lineNum >= lines.Count)
                    endOfLine = true;
            }

            //Loop through the options for this choice
            while (!endOfLine)
            {
                List<string> optionLines = new List<string>();
                optionLines.Add(lines[lineNum]);
                lineNum++;

                int choiceLevels = 1;

                while (!endOfLine && (!lines[lineNum].StartsWith(">") || choiceLevels > 1))
                {
                    if (lines[lineNum].StartsWith("choice"))
                        choiceLevels++;
                    else if (lines[lineNum].StartsWith("}"))
                        choiceLevels--;

                    optionLines.Add(lines[lineNum]);
                    lineNum++;

                    if (lineNum >= lines.Count)
                        endOfLine = true;
                }
                
                options.Add(ChoiceOption.OptionFromText(screen, optionLines, manager));

                if (lineNum >= lines.Count)
                    endOfLine = true;
            }

            return new MenuChoice(synopsis, options) { baseChoice = baseChoice };
        }
    }
}
