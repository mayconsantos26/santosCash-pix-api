using System.Text.RegularExpressions;

namespace Helpers;

public static class CpfValidator
    {
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove todos os caracteres não numéricos
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            // Verifica se o CPF possui exatamente 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Verifica se o CPF não é uma sequência repetida (ex: "11111111111")
            if (new string(cpf[0], 11) == cpf)
                return false;

            // Calcula o primeiro e segundo dígito verificadores
            var tempCpf = cpf.Substring(0, 9);
            var firstDigit = CalculateDigit(tempCpf);
            var secondDigit = CalculateDigit(tempCpf + firstDigit);

            // Verifica se os dígitos verificadores calculados coincidem com os dígitos do CPF
            return cpf.EndsWith(firstDigit.ToString() + secondDigit.ToString());
        }

        // Método para calcular o dígito verificador
        private static int CalculateDigit(string cpf)
        {
            var sum = 0;
            int weight = cpf.Length + 1;

            // Calcula a soma dos produtos dos números pelas posições de peso
            for (int i = 0; i < cpf.Length; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * weight--;
            }

            // Calcula o dígito verificador
            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }
    }