using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CraftingSim.Model
{
    public class Recipe : IRecipe
    {
        private readonly string name;
        private readonly double successRate;
        private readonly Dictionary<IMaterial, int> requiredMaterials;

        public Recipe(string name, double successRate, Dictionary<IMaterial, int> requiredMaterials)
        {
            this.name = name;
            this.successRate = successRate;
            this.requiredMaterials = requiredMaterials;
        }

        public string Name => name;

        public double SuccessRate => successRate;

        public Dictionary<IMaterial, int> RequiredMaterials => requiredMaterials;

        IReadOnlyDictionary<IMaterial, int> IRecipe.RequiredMaterials => RequiredMaterials;

        public bool CanCraft(Inventory inventory)
        {
            foreach (var material in requiredMaterials)
            {
                if (inventory.GetQuantity(material.Key) < material.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(IRecipe other)
        {
            throw new NotImplementedException();
        }
    }
}