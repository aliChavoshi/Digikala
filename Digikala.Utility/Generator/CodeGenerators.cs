using System;

namespace Digikala.Utility.Generator
{
    public class CodeGenerators
    {
        public static string ActiveCodeFiveNumbers()
        {
            var random = new Random();
            return random.Next(10000, 99999).ToString();
        }
        public static string MergeIntArray(int[] numbers)
        {
            var code = "";
            foreach (var number in numbers)
            {
                code += number;
            }
            return code;
        }
        public static string FactorCodeEightNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        public static string GuidId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}