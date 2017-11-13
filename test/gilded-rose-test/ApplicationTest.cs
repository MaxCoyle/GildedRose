using System.Collections.Generic;
using gilded_rose;
using Xunit;
using System.IO;
using System.Linq;

namespace gilded_rose_test
{
	public class ApplicationTest
    {
        // ReSharper disable once InconsistentNaming
		private readonly string expectedOutput = File.ReadAllText("ExpectedOutput.txt");

		[Fact]
		public void RunsWithSpecifiedInputFile()
		{
			var output = string.Empty;
			Application.SendOutput(x => output = x);
			Application.Main(new[] { "" });

			Assert.Equal(expectedOutput, output);
		}

        [Theory,
         InlineData("+5 Dexterity Vest", 10, 20, 9),
         InlineData("Aged Brie", 2, 0, 1),
         InlineData("Elixir of the Mongoose", 5, 7, 4),
         InlineData("Sulfuras, Hand of Ragnaros", 0, 80, 0),
         InlineData("Backstage passes to a TAFKAL80ETC concert", 15, 20, 14),
         InlineData("Conjured Mana Cake", 3, 6, 2)
        ]
        public void ItemSellInCorrect(string name, int sellIn, int quality, int expected)
        {
            var items = new List<Item>
            {
                new Item {Name = name, SellIn = sellIn, Quality = quality }
            };
            new GildedRose(items).UpdateQuality();
            Assert.Equal(expected, items.First().SellIn);
        }

        // Input : new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
        // Expected : Conjured Mana Cake, 2, 5

        [Theory,
            InlineData("+5 Dexterity Vest", 10, 20, 19),
            InlineData("Aged Brie", 2, 0, 1),
            InlineData("Elixir of the Mongoose", 5, 7, 6),
            InlineData("Sulfuras, Hand of Ragnaros", 0, 80, 80),
            InlineData("Sulfuras, Hand of Ragnaros", -1, 80, 80),
            InlineData("Backstage passes to a TAFKAL80ETC concert", 15, 20, 21),
            InlineData("Backstage passes to a TAFKAL80ETC concert", 10, 30, 32),
            InlineData("Backstage passes to a TAFKAL80ETC concert", 5, 40, 43),
            InlineData("Backstage passes to a TAFKAL80ETC concert", 1, 50, 0),
            InlineData("Conjured Mana Cake", 3, 6, 4)
        ]
        public void ItemQualityCorrect(string name, int sellIn, int quality, int expected)
        {
            var items = new List<Item>
            {
                new Item {Name = name, SellIn = sellIn, Quality = quality }
            };
            new GildedRose(items).UpdateQuality();
            Assert.Equal(expected, items.First().Quality);
        }

        [Fact]
        public void QualityIsNeverNegative()
        {
            var items = new List<Item> {
                new Item {Name = "Aged Brie", SellIn = 0, Quality = 0}
            };
            new GildedRose(items).UpdateQuality();
            Assert.True(items.First().Quality >= 0);
        }

        [Fact]
        public void QualityIsNeverMoreThanFifty()
        {
            var items = new List<Item> {
                new Item {Name = "Aged Brie", SellIn = 0, Quality = 50}
            };
            new GildedRose(items).UpdateQuality();
            Assert.True(items.First().Quality <= 50);
        }
    }
}