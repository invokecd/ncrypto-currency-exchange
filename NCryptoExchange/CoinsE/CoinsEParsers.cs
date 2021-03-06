﻿using Lostics.NCryptoExchange.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lostics.NCryptoExchange.CoinsE
{
    public static class CoinsEParsers
    {
        public static Book ParseMarketDepth(JObject bookJson)
        {
            JArray bidsArray = bookJson.Value<JArray>("bids");
            JArray asksArray = bookJson.Value<JArray>("asks");

            List<MarketDepth> bids = bidsArray.Select(
                depth => (MarketDepth)CoinsEMarketDepth.ParseMarketDepth(depth as JObject)
            ).ToList();
            List<MarketDepth> asks = asksArray.Select(
                depth => (MarketDepth)CoinsEMarketDepth.ParseMarketDepth(depth as JObject)
            ).ToList();

            return new Book(asks, bids);
        }

        public static MyTrade ParseMyTrade(JObject jObject, CoinsEMarketId marketId)
        {
            throw new NotImplementedException();
        }

        public static DateTime ParseTime(int secondsSinceEpoch)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(secondsSinceEpoch);
        }

        /// <summary>
        /// Parse an order type as returned from Coins-E.
        /// </summary>
        /// <param name="val">An order type, for example "buy" or "sell"</param>
        /// <returns></returns>
        public static OrderType ParseOrderType(string val)
        {
            if (val.Length == 0)
            {
                throw new ArgumentException("Order type cannot be an empty string.");
            }

            string firstLetter = val.Substring(0, 1);
            string remainder = val.Substring(1);
            string correctedVal = firstLetter.ToUpper() + remainder;

            return (OrderType)Enum.Parse(typeof(OrderType), correctedVal);
        }

        /// <summary>
        /// Parse an order status as returned from Coins-E. See the order workflow
        /// documentation for more details (https://www.coins-e.com/exchange/api/order-workflow/).
        /// Note that whitespace in the order status is replaced with an underscore
        /// to allow parsing as an enum.
        /// </summary>
        /// <param name="val">An order status, for example "queued" or "cancelled"</param>
        /// <returns></returns>
        public static CoinsEOrderStatus ParseOrderStatus(string val)
        {
            string correctedVal = val.Replace(' ', '_');

            return (CoinsEOrderStatus)Enum.Parse(typeof(CoinsEOrderStatus), correctedVal);
        }
    }
}
