using System;
using System.Collections.Generic;
using System.Text;

namespace BranchAndBound
{
    class Program
    {
        static TwoDMatrix matrix;
        static List<int> pcc;
        static int pccCost;

        static void Main(string[] args)
        {
            Console.WriteLine("Travelling Salesman - Branch and bound");
            Console.WriteLine("1 - Meilleur d'abord");
            Console.WriteLine("2 - Profondeur");
            Console.WriteLine("Glutto ?");
            bool choice = bool.Parse(Console.ReadLine());

            int nbExec = 10;

            Console.WriteLine("Taille de la matrice : ");
            int matrixSize = int.Parse(Console.ReadLine());

            Console.WriteLine("----------------------------");
            Console.WriteLine("Choice is " + choice);
            Console.WriteLine("Nb Exec is " + nbExec);
            Console.WriteLine("Size is " + matrixSize);

            for (int i = 0; i < nbExec; i++)
            {
                Console.WriteLine($"Computing pass {i}");

                matrix = new TwoDMatrix(matrixSize);

                Console.WriteLine($"Meilleur d abord");
                DateTime start = DateTime.Now;
                TravellingSalesmanMeilleur(choice);
                DateTime end = DateTime.Now;
                Console.WriteLine($"Ended with a  time of {end - start}");

                Console.WriteLine($"Profondeur d abord");
                start = DateTime.Now;
                TravellingSalesmanProfondeur(choice);
                end = DateTime.Now;
                Console.WriteLine($"Ended with a  time of {end - start}");
            }

            Console.WriteLine($"\n-----------------------------\nDONE\n-----------------------------");

            Console.ReadLine();
        }

        static string PathToString(List<int> toPrint)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int i in toPrint)
            {

                sb.Append($"{i} ");
            }

            return sb.ToString();

        }

        static void TravellingSalesmanMeilleur(bool initWithGluttony)
        {
            pcc = null;
            pccCost = int.MaxValue;

            if (initWithGluttony)
                GluttonyShortestPath();

            int numberOfNodes = matrix.GetSize();
            List<int> toVisit = new List<int>();
            for (int i = 1; i < numberOfNodes; i++)
                toVisit.Add(i);
            List<int> visited = new List<int>() { 0 };

            MeilleurDAbord(visited, toVisit);
        }

        static void TravellingSalesmanProfondeur(bool initWithGluttony)
        {
            pcc = null;
            pccCost = int.MaxValue;

            if (initWithGluttony)
                GluttonyShortestPath();

            int numberOfNodes = matrix.GetSize();
            List<int> toVisit = new List<int>();
            for (int i = 1; i < numberOfNodes; i++)
                toVisit.Add(i);
            List<int> visited = new List<int>() { 0 };

            ParcoursProfondeur(visited, toVisit, 0);
        }

        static void MeilleurDAbord(List<int> visited, List<int> toVisit)
        {
            List<Etat> etats = new List<Etat>();

            etats.Add(new Etat(0, visited, toVisit));

            while (etats.Count > 0)
            {
                Etat current = etats[0];

                //Console.WriteLine($"\nStudying path {PathToString(current.alreadyVisited)}");

                etats.RemoveAt(0);

                if (current.borneInf > pccCost)
                {
                    //Console.WriteLine($"Exiting as path {PathToString(current.alreadyVisited)} has a cost of {current.borneInf}");
                    break;
                }

                int previous = current.alreadyVisited[current.alreadyVisited.Count - 1];
                foreach (int i in current.notVisited)
                {
                    List<int> alreadyVisited = new List<int>(current.alreadyVisited);
                    alreadyVisited.Add(i);
                    List<int> notVisited = new List<int>(current.notVisited);
                    notVisited.Remove(i);

                    //Console.WriteLine($"Studying {PathToString(alreadyVisited)} - {PathToString(notVisited)}");

                    int borneInf = GetPathLength(alreadyVisited);
                    borneInf += Bound(alreadyVisited[alreadyVisited.Count - 1], notVisited);

                    if (notVisited.Count == 0)
                    {
                        //Console.WriteLine($"\nEnded path found : {PathToString(alreadyVisited)} with a cost of {borneInf} - pccCost is {pccCost}");

                        if (borneInf < pccCost)
                        {
                            pcc = alreadyVisited;
                            pccCost = borneInf;

                            //Console.WriteLine($"\nBetter path found : {PathToString(pcc)}");
                        }
                    }
                    else
                    {
                        etats.Add(new Etat(borneInf, alreadyVisited, notVisited));
                    }
                }

                etats.Sort();
            }
        }

        static void ParcoursProfondeur(List<int> alreadyVisited, List<int> notVisited, int currentDistance)
        {
            if (notVisited.Count == 0)
            {
                int pathDistance = currentDistance + matrix.Get(alreadyVisited[alreadyVisited.Count - 1], 0);
                //Console.WriteLine($"Path <{PathToString(alreadyVisited)}> has a distance of {pathDistance}");
                if (pathDistance < pccCost)
                {
                    pcc = alreadyVisited;
                    pccCost = pathDistance;
                }
            }

            int currentMinimalDistance = Bound(alreadyVisited[alreadyVisited.Count - 1], notVisited) + currentDistance;

            if (currentMinimalDistance > pccCost)
                return;

            int previouslyVisited = alreadyVisited[alreadyVisited.Count - 1];

            foreach (int i in notVisited)
            {
                List<int> visited = new List<int>(alreadyVisited);
                visited.Add(i);

                List<int> toVisit = new List<int>(notVisited);
                toVisit.Remove(i);

                ParcoursProfondeur(visited, toVisit, currentDistance + matrix.Get(previouslyVisited, i));
            }
        }

        static int Bound(int previouslyVisited, List<int> toVisit)
        {
            if (toVisit.Count == 0)
            {
                return matrix.Get(previouslyVisited, 0);
            }

            if (toVisit.Count == 1)
            {
                int remaning = toVisit[0];
                return matrix.Get(previouslyVisited, remaning) + matrix.Get(remaning, 0);
            }

            int minimalCost = int.MaxValue;

            foreach (int i in toVisit)
            {
                int current = matrix.Get(previouslyVisited, i);
                if (current < minimalCost)
                    minimalCost = current;
            }

            foreach (int i in toVisit)
            {
                int min = int.MaxValue;
                int current;
                foreach (int j in toVisit)
                {
                    if (j == i)
                        continue;

                    current = matrix.Get(i, j);
                    if (current < min)
                        min = current;
                }

                current = matrix.Get(i, 0);
                if (current < min)
                    min = current;

                minimalCost += min;
            }

            return minimalCost;
        }

        static void GluttonyShortestPath()
        {
            int numberOfNodes = matrix.GetSize();
            List<int> toVisit = new List<int>();
            for (int i = 1; i < numberOfNodes; i++)
                toVisit.Add(i);
            List<int> visited = new List<int>() { 0 };

            pccCost = 0;

            while (toVisit.Count > 0)
            {
                int previousVisit = visited[visited.Count - 1];

                int nextVisit = toVisit[0];

                foreach (int i in toVisit)
                {
                    if (matrix.Get(previousVisit, i) < matrix.Get(previousVisit, nextVisit))
                        nextVisit = i;
                }

                visited.Add(nextVisit);
                toVisit.Remove(nextVisit);

                pccCost += matrix.Get(previousVisit, nextVisit);
            }

            pccCost += matrix.Get(visited[visited.Count - 1], 0);

            pcc = visited;
        }

        static int GetPathLength(List<int> path)
        {
            int res = 0;

            int previous = -1;

            foreach (int i in path)
            {
                if (previous >= 0)
                {
                    res += matrix.Get(previous, i);
                }
                previous = i;
            }

            return res;
        }
    }
}