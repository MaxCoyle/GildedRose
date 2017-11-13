using System;
using System.Collections.Generic;

namespace gilded_rose
{
    public abstract class ItemStrategy
    {
        public abstract void Update(Item item);

        public void ValidateQuality(Item item)
        {
            // Item is never more than 50
            item.Quality = Math.Min(50, item.Quality);
            // Item is never less than 0
            item.Quality = Math.Max(0, item.Quality);
        }
    }

    public class DefaultItemStrategy : ItemStrategy
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            var increment = item.SellIn <= 0 ? 2 : 1;

            item.Quality -= increment;
            
            ValidateQuality(item);
        }
    }

    public class EnhancingItemStrategy : ItemStrategy
    {
        public override void Update(Item item)
        {
            item.Quality++;
            item.SellIn--;

            ValidateQuality(item);
        }
    }

    public class BackStageItemStrategy : ItemStrategy
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            if (item.SellIn <= 0)
            {
                item.Quality = 0;
            }
            else if (item.SellIn <= 5)
            {
                item.Quality += 3;
            }
            else if (item.SellIn <= 10)
            {
                item.Quality += 2;
            }
            else
            {
                item.Quality++;
            }

            ValidateQuality(item);
        }
    }

    public class SulfurusItemStrategy : ItemStrategy
    {
        public override void Update(Item item)
        {
        }
    }

    public class ConjuredItemStrategy : ItemStrategy
    {
        public override void Update(Item item)
        {
            item.Quality-=2;
            item.SellIn--;

            ValidateQuality(item);
        }
    }

    public class ItemContext
    {
        private static readonly Dictionary<string, ItemStrategy> ItemStrategies = 
            new Dictionary<string, ItemStrategy>();

        static ItemContext()
        {
            ItemStrategies.Add("Aged Brie", new EnhancingItemStrategy());
            ItemStrategies.Add("BackStage Passes", new BackStageItemStrategy());
            ItemStrategies.Add("Sulfuras", new SulfurusItemStrategy());
            ItemStrategies.Add("Conjured", new ConjuredItemStrategy());
            ItemStrategies.Add("Default Item", new DefaultItemStrategy());
        }

        public static void Update(Item item)
        {
            ItemStrategy matchingItemStrategy = null;

            foreach (var itemStrategy in ItemStrategies)
            {
                if (item.Name.ToLower().Contains(itemStrategy.Key.ToLower()))
                {
                    matchingItemStrategy = itemStrategy.Value;
                }
            }

            if (matchingItemStrategy == null)
            {
                matchingItemStrategy = ItemStrategies["Default Item"];
            }

            matchingItemStrategy.Update(item);
        }
    }
}