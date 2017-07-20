using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Items;
using BeeGame.Exceptipns;

namespace BeeGame.Core.Dictionarys
{
    public static class CraftingRecipies
    {
        /// <summary>
        /// Contains all crafting recipies that require a certian layout in the crafting grid (<see cref="Blocks.CraftingTable"/>
        /// </summary>
        private static Dictionary<string, Item> shapedCraftingRecipies = new Dictionary<string, Item>();

        /// <summary>
        /// Will add a shaped crafting recipie to the game
        /// </summary>
        /// <param name="recipie">The desired recipie.  Layout is {"XXX", "XXX", "XXX", "X", ItemID} where each X is a slot in the crafting grid, Each group of 3 is a row, and a "X", ItemID is the <see cref="Item"/> ID X represents (for each new item a new symbol is required), a Sapce is no item required in that slot</param>
        /// <param name="result">The <see cref="Item"/> that the recipie will produce</param>
        /// <example>
        /// This example shows how to call <see cref="AddShapedRecipie(object[], Item)"/>
        /// <code>
        /// void Main()
        /// {
        ///     CraftingRecipies.AddShapedRecipie(new object[] { " X ", "X@X", " X ", "X", Wood.GetItemID(), "@", Stone.GetItemID() }, new Chest());
        /// }
        /// </code>
        /// </example>
        public static void AddShapedRecipie(object[] recipie, Item result)
        {
            //* converts the given blocks of 3 haracters to a 9 character string
            var stringRecipie = "";

            for (int i = 0; i < 3; i++)
            {
                stringRecipie += recipie[i] as string;
            }

            //* gets what character represents which item
            for (int i = 3; i < recipie.Length; i += 2)
            {
                var character = (string)recipie[i];
                var itemID = (int)recipie[i + 1];

                //* replaces the character with the items id
                stringRecipie = stringRecipie.Replace(character, itemID.ToString());
            }

            //* converts empty sots ' ' into '0'
            stringRecipie = stringRecipie.Replace(' ', '0');

            //* if the recipe exists an exception is thrown as two recipies cannot be the same
            if (shapedCraftingRecipies.ContainsKey(stringRecipie))
                throw new CraftingRecipieAdditionException("Failed to add crafting recipie as it already exists");

            //* adds the recipie to the dictionary
            shapedCraftingRecipies.Add(stringRecipie, result);
        }

        /// <summary>
        /// Returns an <see cref="Item"/> from the <see cref="shapedCraftingRecipies"/> dictionary
        /// </summary>
        /// <param name="recipie">Recipie for <see cref="Item"/></param>
        /// <returns>An <see cref="Item"/> or <see cref="null"/> is recipie was not found</returns>
        public static Item GetShapedRecipeItem(string recipie)
        {
            shapedCraftingRecipies.TryGetValue(recipie, out var item);

            return item;
        }
    }
}
