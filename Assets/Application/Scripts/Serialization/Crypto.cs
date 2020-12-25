using System;

namespace LabyrinthGame.SerializebleData
{
    public static class Crypto
    {
        public static string CryptoXOR(string text, int key = 42)
        {
            var result = String.Empty;
            foreach (var simbol in text)
            {
                result += (char)(simbol ^ key);
            }
            return result;
        }
    }
}