    namespace Calculadora;

public class Program {
    static void Main(string[] args)
    {
        
        Menu();
        
        static void Menu (){
            
            Console.WriteLine("-----------");
            Console.WriteLine("O que deseja fazer?");
            Console.WriteLine("1 - Soma");
            Console.WriteLine("2 - Subtração");
            Console.WriteLine("3 - Divisão");
            Console.WriteLine("4 - Multiplicação");
            Console.WriteLine("5 - Sair");

            Console.WriteLine("-----------");
            Console.WriteLine("Selecione uma opção");

            short resultado = short.Parse(Console.ReadLine()); // O método short.Parse tenta converter a string capturada para um valor do tipo short (menor número possível).

            switch (resultado) {
                case 1: Soma(); break; // Se a opção for igual (==) caso 1, irá Somar (e assim adiante) - Break é para nao ficar em um looping e sair do switch.
                case 2: Subtracao(); break;
                case 3: Divisao(); break;
                case 4: Multiplicacao(); break;
                case 5: System.Environment.Exit(0); break; // Irá voltar ao 0 e sair da aplicação com o parametro de saída Exit
                default : Menu(); break;
            }
        }

        // Método Soma
        static void Soma (){
            Console.WriteLine("Primeiro valor: ");
            decimal v1 = decimal.Parse(Console.ReadLine()); // feito uma conversão de uma string para decimal com o parse e o ReadLine é para ler na mesma linha.

            Console.WriteLine("Segundo valor é: ");
            decimal v2 = decimal.Parse(Console.ReadLine());

            decimal resultado = v1 + v2;
            // Console.WriteLine($"O resultado da soma é: " + resultado); // Outros metodos de como somar no console
            // Console.WriteLine($"O resultado da soma é: {resultado}");
            Console.WriteLine($"O resultado da soma é {v1 + v2}"); // Interpolação $ = Tem que ser colocado entre chaves {}, senao ele vai ler como se fosse uma add de uma string

            Menu();
        }

        // Metodo Subtração
        static void Subtracao (){
            Console.WriteLine("Primeiro valor: ");
            decimal v1 = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Segundo valor é: ");
            decimal v2 = decimal.Parse(Console.ReadLine());

            decimal resultado = v1 - v2;
            Console.WriteLine($"O resultado da subtração é {v1 - v2}");

            Menu();
        }

        // Metodo Divisão
        static void Divisao() {
            Console.WriteLine("Primeiro valor é: ");
            decimal v1 = decimal.Parse(Console.ReadLine());
            
            Console.WriteLine("Segundo valor é: ");
            decimal v2 = decimal.Parse(Console.ReadLine());

            decimal resultado = v1 / v2;
            Console.WriteLine($"O resultado da divisão é {resultado}");

            Menu();
        }
        
        // Metodo Multiplicação
        static void Multiplicacao() {
            Console.WriteLine("Primeiro valor é: ");
            decimal v1 = decimal.Parse(Console.ReadLine());
            
            Console.WriteLine("Segundo valor é: ");
            decimal v2 = decimal.Parse(Console.ReadLine());

            decimal resultado = v1 * v2;
            Console.WriteLine($"O resultado da multiplicação é {resultado}");

            Menu();
        }
    }
}   