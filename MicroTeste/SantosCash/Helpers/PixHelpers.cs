using System.Text.RegularExpressions;

namespace MicroTeste.Helpers
{
    public static class PixHelpers
    {
        public static string GerarEndToEndId(DateTime hora) // Método para gerar o EndToEndId
        {
            // Gera 11 caracteres aleatórios a partir de um GUID
            var randomChars = new Regex("[^a-zA-Z0-9 -]")
                .Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "")
                .Substring(0, 11);

            return $"E{Defaults.MicrocashIspb}{hora:yyyyMMddHHmm}{randomChars}"; // Gera o ID com o ISPB e a data
        }

        // Método para gerar o EndToEndId para devolução
        public static string GerarEndToEndIdDevolucao(string ispb, DateTime hora)
        {
            var randomChars = new Regex("[^a-zA-Z0-9 -]")
                .Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "")
                .Substring(0, 11);

            return $"R{ispb}{hora:yyyyMMddHHmm}{randomChars}";
        }
    }
}
