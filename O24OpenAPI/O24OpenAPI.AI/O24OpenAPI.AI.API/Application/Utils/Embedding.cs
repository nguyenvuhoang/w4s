using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.AI.API.Application.Utils
{
    public class Embedding
    {
        public static float[] BuildFakeEmbedding(string input, int dimension)
        {
            var vector = new float[dimension];
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));

            for (int i = 0; i < vector.Length; i++)
                vector[i] = hash[i % hash.Length] / 255f;

            return vector;
        }
    }
}
