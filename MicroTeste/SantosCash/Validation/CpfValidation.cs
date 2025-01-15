
namespace MicroTeste.Validations;

public static class CpfValidation
{
    public static bool IsCpfValid(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return false;

        // Remove caracteres especiais como "." e "-"
        cpf = cpf.Replace(".", "").Replace("-", "");

        // Verifica se o CPF possui 11 dígitos e não é uma sequência repetida
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        // Calcula o primeiro dígito verificador
        var tempCpf = cpf.Substring(0, 9);
        var digito1 = CalculateCpfDigit(tempCpf);

        // Calcula o segundo dígito verificador
        var tempCpfWithFirstDigit = tempCpf + digito1.ToString();
        var digito2 = CalculateCpfDigit(tempCpfWithFirstDigit);

        // Compara os dígitos calculados com os dígitos do CPF fornecido
        return cpf.EndsWith(digito1.ToString() + digito2.ToString());
    }

    internal static bool IsCpfValid(object pagador_Cpf)
    {
        throw new NotImplementedException();
    }

    // Método para calcular um dígito verificador de CPF
    private static int CalculateCpfDigit(string cpfBase)
    {
        int soma = 0;
        int multiplicador = cpfBase.Length + 1;

        // Soma os dígitos do CPF multiplicados por uma sequência decrescente
        foreach (var caracter in cpfBase)
        {
            soma += int.Parse(caracter.ToString()) * multiplicador;
            multiplicador--;
        }

        // Calcula o dígito com base na soma
        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
