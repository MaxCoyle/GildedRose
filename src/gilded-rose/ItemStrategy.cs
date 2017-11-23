using System;
using System.Collections.Generic;

namespace gilded_rose
{
    public interface IQualityManager
    {
        void Validate(Item item);
    }

    public class StandardQualityManager : IQualityManager
    {
        private const int MinimumQuality = 0;
        private const int MaximumQuality = 50;

        public void Validate(Item item)
        {
            item.Quality = Math.Min(MaximumQuality, item.Quality);
            item.Quality = Math.Max(MinimumQuality, item.Quality);
        }
    }

    public abstract class ItemStrategy
    {
        public abstract void Update(Item item, IQualityManager qualityManager);
    }

    public class DefaultItemStrategy : ItemStrategy
    {
        public override void Update(Item item, IQualityManager qualityManager)
        {
            item.SellIn--;

            var increment = item.SellIn <= 0 ? 2 : 1;

            item.Quality -= increment;
            
            qualityManager.Validate(item);
        }
    }

    public class EnhancingItemStrategy : ItemStrategy
    {
        public override void Update(Item item, IQualityManager qualityManager)
        {
            item.Quality++;
            item.SellIn--;

            qualityManager.Validate(item);
        }
    }

    public class BackStageItemStrategy : ItemStrategy
    {
        public override void Update(Item item, IQualityManager qualityManager)
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

            qualityManager.Validate(item);
        }
    }

    public class SulfurusItemStrategy : ItemStrategy
    {
        public override void Update(Item item, IQualityManager qualityManager)
        {
        }
    }

    public class ConjuredItemStrategy : ItemStrategy
    {
        public override void Update(Item item, IQualityManager qualityManager)
        {
            item.Quality-=2;
            item.SellIn--;

            qualityManager.Validate(item);
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

            matchingItemStrategy.Update(item, new StandardQualityManager());
        }
    }
}