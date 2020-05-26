using System;
using System.Collections.Generic;

namespace ArticuloAG {
    class Program {
        static void Main() {
            Random azar = new Random(); //Usado para la semilla del generador de aleatorios de la clase población

            //Cadena que será buscada por los algoritmos genéticos
            Cadena objCad = new Cadena();
            string cadOriginal;

            int Pruebas = 50; //Total de veces que se cambiará la cadena y se repetirá todo el proceso
            int TotalIndividuos = 2000; //Total de individuos de la población
            int TotalCiclos = 100000; //Total de ciclos que hará por cada operador
            int Promedio = 1000; //Cada cuanto calcula el promedio de la población
            int TamCadena = 90; //Tamaño de la cadena a buscar

            Poblacion objPoblacion = new Poblacion();

            List<double> Muta = new List<double>(); //Lista de promedios de adaptación del operador de mutación
            List<double> Cruce = new List<double>(); //Lista de promedios de adaptación del operador de cruce
            List<double> MutaCruce = new List<double>(); //Lista de promedios de adaptación del operador de mutación+cruce

            for (int num=0; num < TotalCiclos/Promedio; num++) {
                Muta.Add(0);
                Cruce.Add(0);
                MutaCruce.Add(0);
            }

            //En cada prueba se cambia la cadena a buscar y se reinicia las poblaciones con nuevos individuos
            for (int num = 0; num < Pruebas; num++) {
                int semilla = azar.Next();
                cadOriginal = objCad.CadenaAzar(azar, TamCadena);

                //Misma semilla para que los tres operadores trabajen con los mismos individuos al inicio
                objPoblacion.Configura(semilla, cadOriginal, TotalIndividuos, TotalCiclos, Promedio);
                objPoblacion.Mutacion();
                objPoblacion.Configura(semilla, cadOriginal, TotalIndividuos, TotalCiclos, Promedio);
                objPoblacion.Cruce();
                objPoblacion.Configura(semilla, cadOriginal, TotalIndividuos, TotalCiclos, Promedio);
                objPoblacion.MutacionCruce();

                //Para el promedio de evolución de las métricas de acercamiento a la cadena original
                //de los tres operadores
                for (int cont = 0; cont < objPoblacion.PromedioMuta.Count; cont++) {
                    Muta[cont] += objPoblacion.PromedioMuta[cont];
                    Cruce[cont] += objPoblacion.PromedioCruce[cont];
                    MutaCruce[cont] += objPoblacion.PromedioMutaCruce[cont];
                }
            }

            //Calcula el promedio y lo muestra en consola.
            for (int num = 0; num < TotalCiclos / Promedio; num++) {
                Muta[num] /= Pruebas;
                Console.Write(Muta[num].ToString() + ";");
                Cruce[num] /= Pruebas;
                Console.Write(Cruce[num].ToString() + ";");
                MutaCruce[num] /= Pruebas;
                Console.WriteLine(MutaCruce[num].ToString());
            }

            Console.ReadKey();
        }
    }
}
