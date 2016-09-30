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
        public List<string> synopsis { get; private set; }
        public List<ChoiceOption> options { get; private set; }

        public MenuChoice(List<string> synopsis, List<ChoiceOption> options)
        {
            this.synopsis = synopsis;
            this.options = options;
        }

        /// <summary>
        /// Creates a MenuChoice object from text.
        /// </summary>
        /// <param name="lines">Lines of text that represent a choice</param>
        /// <returns></returns>
        public static MenuChoice CreateChoice(List<string> lines)
        {
            int lineNum = 0;
            bool endOfLine = false;
            List<string> synopsis = new List<string>();
            List<ChoiceOption> options = new List<ChoiceOption>();

            //Loop through lines that make up the Choice synopsis
            while (!lines[lineNum].ToLower().StartsWith("choice") && !endOfLine)
            {
                synopsis.Add(lines[lineNum]);
                lineNum++;

                if (lineNum > lines.Count)
                    endOfLine = true;
            }
            lineNum += 2;

            //Loop through the choices for this option
            while (!endOfLine)
            {
                List<string> optionLines = new List<string>();
                while (!lines[lineNum].StartsWith("}") && !endOfLine)
                {
                    optionLines.Add(lines[lineNum]);
                    lineNum++;

                    if (lineNum > lines.Count)
                        endOfLine = true;
                }
                lineNum++;

                //Creates a choice unless there was some error and syntax was not fully completed
                if (!endOfLine)
                    options.Add(ChoiceOption.OptionFromText(optionLines));

                if (lineNum > lines.Count)
                    endOfLine = true;
            }

            return new MenuChoice(synopsis, options);
        }
    }
}
