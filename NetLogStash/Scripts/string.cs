using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Scripts
{
    public static class stringex
    {
        private static T Construct<T>(string name, params string[] args)
           where T : class
        {
            string typeDesciption = "";

            string fileName = string.Format(@".\Scripts\{0}s\{1}.csx", typeDesciption, name);
            string typeName = name + typeDesciption;

            T result = ConstructFromScript<T>(fileName, typeName);

            //result.Initialize(args);

            return result;
        }

        private static T ConstructFromScript<T>(string fileName, string typeName)
            where T : class
        {
            string code = File.ReadAllText(fileName);

            T result = CSScript.LoadCode(code).CreateObject(typeName).AlignToInterface<T>();

            return result;
        }
    }
}
