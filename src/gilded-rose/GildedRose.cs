using System.Collections.Generic;

namespace gilded_rose
{
	public class GildedRose
    {
        // ReSharper disable once InconsistentNaming
        private readonly IList<Item> Items;        

        public GildedRose(IList<Item> items)
        {
            this.Items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                ItemContext.Update(item);
            }
        }
    }
}