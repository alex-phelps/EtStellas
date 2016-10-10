using BPA_RPG.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG
{
    class Deal
    {
        public readonly GameItem item;
        public readonly bool canBuy;
        public readonly int buyPrice;
        public readonly bool canSell;
        public readonly int sellPrice;
        public readonly Currency currency;

        public Deal(GameItem item, int? buyPrice, int? sellPrice, Currency currency = Currency.credits)
        {
            this.item = item;
            this.currency = currency;

            canBuy = true;
            canSell = true;

            if (buyPrice == null)
                canBuy = false;
            else this.buyPrice = (int)buyPrice;
            if (sellPrice == null)
                canSell = false;
            else this.sellPrice = (int)sellPrice;
        }

        public static Deal DealFromText(string line)
        {
            GameItem item;
            Currency currency;
            int? buyPrice;
            int? sellPrice;

            string[] lineParts = line.Split(new string[] { " " }, StringSplitOptions.None);

            item = GameItem.ItemFromText(lineParts[0]);

            switch (lineParts[1].ToLower())
            {
                case "jex":
                    currency = Currency.jex;
                    break;
                default:
                    currency = Currency.credits;
                    break;
            }

            if (lineParts[2].ToLower() == "null")
                buyPrice = null;
            else buyPrice = Convert.ToInt32(lineParts[2]);

            if (lineParts[3].ToLower() == "null")
                sellPrice = null;
            else sellPrice = Convert.ToInt32(lineParts[3]);

            return new Deal(item, buyPrice, sellPrice, currency);
        }
    }
}
