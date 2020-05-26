using System;
using System.Collections.Generic;

namespace ArticuloAG {
    class Poblacion {
        private List<string> Individuos; //Lista de individuos
        private Random azar;
        private int numCiclos; //Cuantos ciclos hará por cada operador
        private int Promedio; //Cada cuanto generará promedio de adaptación de la población
        private string cadOriginal; //Cadena a la que debe aproximarse
        public List<double> PromedioMuta; //Lista de promedios de adaptación del operador de mutación
        public List<double> PromedioCruce; //Lista de promedios de adaptación del operador de cruce
        public List<double> PromedioMutaCruce; //Lista de promedios de adaptación del operador de mutación+cruce

        public Poblacion() {
            Individuos = new List<string>();
            PromedioMuta = new List<double>();
            PromedioCruce = new List<double>();
            PromedioMutaCruce = new List<double>();
        }

        public void Configura(int semilla, string cadOriginal, int numIndividuos, int numCiclos, int Promedio) {
            azar = new Random(semilla);

            //Crea la población con individuos generados al azar (dependiendo de la semilla)
            Individuos.Clear();
            Cadena objCad = new Cadena();
            for (int cont = 1; cont <= numIndividuos; cont++) {
                Individuos.Add(objCad.CadenaAzar(azar, cadOriginal.Length));
            }

            this.numCiclos = numCiclos;
            this.Promedio = Promedio;
            this.cadOriginal = cadOriginal;
        }

        //Operador mutación
        public void Mutacion() {
            PromedioMuta.Clear();
            Cadena objCad = new Cadena();

            //Proceso de algoritmo genético
            for (int ciclo = 1; ciclo <= numCiclos; ciclo++) {

                //Toma dos individuos al azar
                int indivA = azar.Next(Individuos.Count);
                int indivB;
                do {
                    indivB = azar.Next(Individuos.Count);
                } while (indivA == indivB); //Asegura que sean dos individuos distintos

                //Evalúa la adaptación de los dos individuos
                int valorIndivA = objCad.EvaluaCadena(cadOriginal, Individuos[indivA]);
                int valorIndivB = objCad.EvaluaCadena(cadOriginal, Individuos[indivB]);

                //Si individuo A está mejor adaptado que B entonces: Elimina B + Duplica A + Modifica duplicado
                if (valorIndivA > valorIndivB) {
                    Individuos[indivB] = objCad.MutaCadena(azar, Individuos[indivA]);
                }
                else if (valorIndivA < valorIndivB) { //Caso A es menor que B: Elimina A + Duplica B + Modifica duplicado
                    Individuos[indivA] = objCad.MutaCadena(azar, Individuos[indivB]);
                }

                //Calcula promedio de adaptación de toda la población
                if (ciclo % Promedio == 0) {
                    double acumula = 0;
                    for (int indiv = 0; indiv < Individuos.Count; indiv++)
                        acumula += objCad.EvaluaCadena(cadOriginal, Individuos[indiv]);
                    PromedioMuta.Add((double)acumula / Individuos.Count);
                }
            }
        }

        public void Cruce() {
            PromedioCruce.Clear();
            Cadena objCad = new Cadena();

            //Proceso de algoritmo genético
            for (int ciclo = 1; ciclo <= numCiclos; ciclo++) {

                //Toma dos individuos al azar
                int indivA = azar.Next(Individuos.Count);
                int indivB;
                do {
                    indivB = azar.Next(Individuos.Count);
                } while (indivA == indivB); //Asegura que sean dos individuos distintos

                //Usa el operador cruce
                int posAzar = azar.Next(cadOriginal.Length);
                string parteA = Individuos[indivA].Substring(0, posAzar);
                string parteB = Individuos[indivB].Substring(posAzar);
                string HijoA = parteA + parteB;

                //Evalúa la adaptación de los individuos padres e hijos
                int valorIndivA = objCad.EvaluaCadena(cadOriginal, Individuos[indivA]);
                int valorIndivB = objCad.EvaluaCadena(cadOriginal, Individuos[indivB]);
                int valorHijoA = objCad.EvaluaCadena(cadOriginal, HijoA);

                //Si los hijos son mejores que los padres, entonces los reemplaza
                if (valorHijoA > valorIndivA) Individuos[indivA] = HijoA;
                if (valorHijoA > valorIndivB) Individuos[indivB] = HijoA;

                //Calcula promedio de adaptación de toda la población
                if (ciclo % Promedio == 0) {
                    double acumula = 0;
                    for (int indiv = 0; indiv < Individuos.Count; indiv++)
                        acumula += objCad.EvaluaCadena(cadOriginal, Individuos[indiv]);
                    PromedioCruce.Add((double)acumula / Individuos.Count);
                }
            }
        }

        public void MutacionCruce() {
            PromedioMutaCruce.Clear();
            Cadena objCad = new Cadena();

            //Proceso de algoritmo genético
            for (int ciclo = 1; ciclo <= numCiclos; ciclo++) {

                //Toma dos individuos al azar
                int indivA = azar.Next(Individuos.Count);
                int indivB;
                do {
                    indivB = azar.Next(Individuos.Count);
                } while (indivA == indivB); //Asegura que sean dos individuos distintos

                //Usa el operador cruce
                int posAzar = azar.Next(cadOriginal.Length);
                string parteA = Individuos[indivA].Substring(0, posAzar);
                string parteB = Individuos[indivB].Substring(posAzar);
                string HijoA = parteA + parteB;

                //Además muta el hijo
                HijoA = objCad.MutaCadena(azar, HijoA);

                //Evalúa la adaptación de los individuos padres e hijos
                int valorIndivA = objCad.EvaluaCadena(cadOriginal, Individuos[indivA]);
                int valorIndivB = objCad.EvaluaCadena(cadOriginal, Individuos[indivB]);
                int valorHijoA = objCad.EvaluaCadena(cadOriginal, HijoA);

                //Si los hijos son mejores que los padres, entonces los reemplaza
                if (valorHijoA > valorIndivA) Individuos[indivA] = HijoA;
                if (valorHijoA > valorIndivB) Individuos[indivB] = HijoA;

                //Calcula promedio de adaptación de toda la población
                if (ciclo % Promedio == 0) {
                    double acumula = 0;
                    for (int indiv = 0; indiv < Individuos.Count; indiv++)
                        acumula += objCad.EvaluaCadena(cadOriginal, Individuos[indiv]);
                    PromedioMutaCruce.Add((double)acumula / Individuos.Count);
                }
            }
        }
    }
}
