using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchAndBound
{
    class TwoDMatrix
    {
        int[,] matrix;
        int size;

        public TwoDMatrix(int size)
        {
            this.size = size;
            matrix = new int[size, size];

            Random r = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    Set(i, j, r.Next(1, 101));
                }

                matrix[i, i] = -1;
            }
        }

        public int GetSize()
        {
            return matrix.GetLength(0);
        }

        public override string ToString()
        {
            string res = "";

            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    int currentValue = matrix[i, j];
                    res += currentValue;

                    if (currentValue < 10 && currentValue >= 0)
                        res += "   ";
                    else if (currentValue >= 100)
                        res += " ";
                    else
                        res += "  ";
                }
                res += "\n";
            }

            return res;
        }

        public int Get(int x, int y)
        {
            return matrix[x, y];
        }

        public void Set(int x, int y, int value)
        {
            matrix[x, y] = value;
            matrix[y, x] = value;
        }
    }
}
