using System.Text.RegularExpressions;

namespace Helpers
{
    public static class PixHelpers
    {
        public static string GerarEndToEndId(DateTime hora) // Método para gerar o EndToEndId
        {
            // Gera 11 caracteres aleatórios a partir de um GUID
            var randomChars = new Regex("[^a-zA-Z0-9 -]")
                .Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "")
                .Substring(0, 11);

            return $"E{Defaults.SantosCashIspb}{hora:yyyyMMddHHmm}{randomChars}"; // Gera o ID com o ISPB e a data
        }

        // Método para gerar o EndToEndId para devolução
        public static string GerarEndToEndIdDevolucao(string ispb, DateTime hora)
        {
            var randomChars = new Regex("[^a-zA-Z0-9 -]")
                .Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "")
                .Substring(0, 11);

            return $"R{ispb}{hora:yyyyMMddHHmm}{randomChars}";
        }

        public static string GerarTxid()
        {
            var hora = DateTime.UtcNow;

            // Generate random alphanumeric string
            var randomChars = new Regex("[^a-zA-Z0-9]")
                .Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "")
                .Substring(0, 11);

            // Build the txid
            var txid = $"T{Defaults.SantosCashIspb}{hora:yyyyMMddHHmm}{randomChars}";

            // Ensure txid length is between 26 and 35 characters
            if (txid.Length > 35)
            {
                txid = txid.Substring(0, 35);
            }

            return txid;
        }
    }
}
