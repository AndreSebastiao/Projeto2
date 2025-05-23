using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CraftingSim.Model
{
    public class Material : IMaterial
    {
        private readonly string name;
        private readonly int id;

        public Material(string name)
        {
            this.name = name;
            id = 0;
        }

        public string Name => name;

        public int Id => id;

        public bool Equals(IMaterial other)
        {
            throw new NotImplementedException();
        }
    }
}