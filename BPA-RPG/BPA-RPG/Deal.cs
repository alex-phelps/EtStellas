using BPA_RPG.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG
{
    public class Deal
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

            item = GameItem.Parse(lineParts[0]);

            switch (lineParts[1].ToLower())
            {
                case "credits":
                    currency = Currency.credits;
                    break;
                case "jex":
                    currency = Currency.jex;
                    break;
                default:
                    currency = Currency.credits;
                    break;
            }

            int intPrice;

            if (!int.TryParse(lineParts[2].ToLower(), out intPrice))
                buyPrice = null;
            else buyPrice = intPrice;
            
            if (!int.TryParse(lineParts[3].ToLower(), out intPrice))
                sellPrice = null;
            else sellPrice = intPrice;

            return new Deal(item, buyPrice, sellPrice, currency);
        }
    }
}
