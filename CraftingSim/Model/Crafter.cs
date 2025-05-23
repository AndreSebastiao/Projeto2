using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace CraftingSim.Model
{
    /// <summary>
    /// Implementation of ICrafter. 
    /// </summary>
    public class Crafter : ICrafter
    {
        private readonly Inventory inventory;
        private readonly List<IRecipe> recipeList;

        public Crafter(Inventory inventory)
        {
            this.inventory = inventory;
            recipeList = new List<IRecipe>();
        }

        /// <summary>
        /// returns a read only list of loaded recipes.
        /// </summary>
        public IEnumerable<IRecipe> RecipeList => recipeList;

        /// <summary>
        /// Loads recipes from the files.
        /// Must parse the name, success rate, required materials and
        /// necessary quantities.
        /// </summary>
        /// <param name="recipeFiles">Array of file paths</param>
        public void LoadRecipesFromFile(string[] recipeFiles)
        {
            string s;

            StreamReader sword = new StreamReader("IronSword.txt");
            while ((s = sword.ReadLine()) != null)
            {
                string[] parts = s.Split(',');
                string name = parts[0];
                double successRate = double.Parse(parts[1]);
                Dictionary<IMaterial, int> requiredMaterials =
                    new Dictionary<IMaterial, int>();

                for (int i = 2; i < parts.Length; i += 2)
                {
                    IMaterial material = new Material(parts[i]);
                    int quantity = int.Parse(parts[i + 1]);
                    requiredMaterials.Add(material, quantity);
                }

                IRecipe recipe = new Recipe(name, successRate, requiredMaterials);
                recipeList.Add(recipe);
            }
            sword.Close();

            StreamReader leather = new StreamReader("LeatherBoots.txt");
            while ((s = leather.ReadLine()) != null)
            {
                string[] parts = s.Split(',');
                string name = parts[0];
                double successRate = double.Parse(parts[1]);
                Dictionary<IMaterial, int> requiredMaterials =
                    new Dictionary<IMaterial, int>();

                for (int i = 2; i < parts.Length; i += 2)
                {
                    IMaterial material = new Material(parts[i]);
                    int quantity = int.Parse(parts[i + 1]);
                    requiredMaterials.Add(material, quantity);
                }

                IRecipe recipe = new Recipe(name, successRate, requiredMaterials);
                recipeList.Add(recipe);
            }
            leather.Close();
        }

        /// <summary>
        /// Attempts to craft an item from a given recipe. Consumes inventory 
        /// materials and returns the result message.
        /// </summary>
        /// <param name="recipeName">Name of the recipe to craft</param>
        /// <returns>A message indicating success, failure, or error</returns>
        public string CraftItem(string recipeName)
        {
            IRecipe selected = null;

            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i].Name.Equals(recipeName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    selected = recipeList[i];
                    break;
                }
            }
            
            if (selected == null)
                return "Recipe not found.";

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
            {
                IMaterial material = required.Key;
                int need = required.Value;
                int have = inventory.GetQuantity(material);

                if (have < need)
                {
                    if (have == 0)
                    {
                        return "Missing material: " + material.Name;
                    }
                    return "Not enough " + material.Name +
                           " (need " + need +
                           ", have " + have + ")";
                }
            }

            foreach (KeyValuePair<IMaterial, int> required in selected.RequiredMaterials)
                if (!inventory.RemoveMaterial(required.Key, required.Value))
                    return "Not enough materials";

            Random rng = new Random();
            if (rng.NextDouble() < selected.SuccessRate)
                return "Crafting '" + selected.Name + "' succeeded!";
            else
                return "Crafting '" + selected.Name + "' failed. Materials lost.";

        }
    }
}