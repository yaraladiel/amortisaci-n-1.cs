using Spectre.Console;
using System;

namespace TablaDeAmortizacion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MI TABLA DE AMORTIZACIÓN DE DEUDAS");
            Console.WriteLine();

            // Solicitar datos al usuario
            Console.Write("Ingrese el monto del prestamo: ");
            decimal montoPrestamo = decimal.Parse(Console.ReadLine());

            Console.Write("Ingrese la tasa de interes anual (%): ");
            decimal interesAnual = decimal.Parse(Console.ReadLine());

            Console.Write("Ingrese el plazo del prestamo (en meses): ");
            int plazoMeses = int.Parse(Console.ReadLine());

            Console.WriteLine();

            // Calcula tasa de interés mensual
            decimal interesMensual = (interesAnual / 100) / 12;

            // Calcular la cuota fija mensual 
            double unoMasInteresMensual = 1 + (double)interesMensual;
            double potencia = Math.Pow(unoMasInteresMensual, plazoMeses);

            decimal cuotaFija = montoPrestamo * (interesMensual * (decimal)potencia) / ((decimal)potencia - 1);
            cuotaFija = Math.Round(cuotaFija, 2);

            // Crear la tabla de amortización
            var table = new Table();

            // para definir y agregar columnas
            table.AddColumn("No. de cuota");
            table.AddColumn("Pago de cuota");
            table.AddColumn("Interés a pagar");
            table.AddColumn("Abono a capital");
            table.AddColumn("Saldo pendiente");

            // Variables para el cálculo 
            decimal saldoPendiente = montoPrestamo;
            decimal totalIntereses = 0;

            // Bucle for para calcular cada período
            for (int i = 1; i <= plazoMeses; i++)
            {
                /* Calculan:
                 * [58]INTERES
                 * [59]ABONO A CAPITAL
                 * [61]ACTUALIZA SALDO PENDIENTE
                 * [62]ACUMULA INTERESES
                 */
                decimal interesPeriodo = Math.Round(saldoPendiente * interesMensual, 2);
                decimal abonoCapital = Math.Round(cuotaFija - interesPeriodo, 2);
                
                saldoPendiente = Math.Round(saldoPendiente - abonoCapital, 2);
                totalIntereses += interesPeriodo;

                // Para que se ajuste el último período y el saldo sea  0
                if (i == plazoMeses && saldoPendiente != 0)
                {
                    abonoCapital += saldoPendiente;
                    cuotaFija = interesPeriodo + abonoCapital;
                    saldoPendiente = 0;
                }

                // Agregar fila a la tabla

                /* N2 significa numero con 2 plasas decimales */

                table.AddRow(
                    i.ToString(),
                    $"{cuotaFija:N2}",
                    $"{interesPeriodo:N2}",
                    $"{abonoCapital:N2}",
                    $"{saldoPendiente:N2}"
                );
            }

            // Mostrar la tabla
            AnsiConsole.Write(table);

            // Mostrar totales finales
            Console.WriteLine();
            Console.WriteLine($"Total a pagar: {(cuotaFija * plazoMeses):N2}");
            Console.WriteLine($"Total intereses: {totalIntereses:N2}");
        }
    }
}