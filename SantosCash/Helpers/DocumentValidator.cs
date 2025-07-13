using System.Text.RegularExpressions;

namespace Helpers;

public static class DocumentValidator
{
    // Valida CPF
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

    // Valida CNPJ
    public static bool IsValidCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        // Remove todos os caracteres não numéricos
        cnpj = Regex.Replace(cnpj, @"[^\d]", "");

        // Verifica se o CNPJ possui exatamente 14 dígitos
        if (cnpj.Length != 14)
            return false;

        // Verifica se o CNPJ não é uma sequência repetida (ex: "11111111111111")
        if (new string(cnpj[0], 14) == cnpj)
            return false;

        // Calcula o primeiro e segundo dígito verificadores
        var tempCnpj = cnpj.Substring(0, 12);
        var firstDigit = CalculateDigitCnpj(tempCnpj);
        var secondDigit = CalculateDigitCnpj(tempCnpj + firstDigit);

        // Verifica se os dígitos verificadores calculados coincidem com os dígitos do CNPJ
        return cnpj.EndsWith(firstDigit.ToString() + secondDigit.ToString());
    }

    // Método para calcular o dígito verificador de CPF
    private static int CalculateDigit(string document)
    {
        var sum = 0;
        int weight = document.Length + 1;

        // Calcula a soma dos produtos dos números pelas posições de peso
        for (int i = 0; i < document.Length; i++)
        {
            sum += int.Parse(document[i].ToString()) * weight--;
        }

        // Calcula o dígito verificador
        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    // Método para calcular o dígito verificador de CNPJ
    private static int CalculateDigitCnpj(string cnpj)
    {
        var sum = 0;
        int[] weights = cnpj.Length == 12
            ? new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }
            : new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        // Calcula a soma dos produtos dos números pelos pesos
        for (int i = 0; i < cnpj.Length; i++)
        {
            sum += int.Parse(cnpj[i].ToString()) * weights[i];
        }

        // Calcula o dígito verificador
        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
