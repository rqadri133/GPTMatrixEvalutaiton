using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;


List<List<int>> s = new List<List<int>>();

        using (StreamReader sr = new StreamReader("//Users/syedqadri/Documents/Dev/FormMatrixCost/input/input00.txt"))
            {
                while (sr.Peek() >= 0)
                {
                    s.Add(sr.ReadLine().TrimEnd().Split(' ').ToList().Select(sTemp => Convert.ToInt32(sTemp)).ToList());

                }
            }

        int result = Result.formingMagicSquare(s);
       Console.WriteLine("" + result);


class Box
{
    public int X { get; set; }
    public int Y { get; set; }
}

class Matrix
{
    public int[,] Grid { get; set; }
    public int Rows { get; } = 3;
    public int Cols { get; } = 3;

    public Matrix(int[,] grid)
    {
        Grid = grid;
    }

    // Calculate cost to transform this matrix into a given magic square
    public int TransformationCost(int[,] magicSquare)
    {
        int cost = 0;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                cost += Math.Abs(Grid[i, j] - magicSquare[i, j]);
            }
        }
        return cost;
    }
}

class Result
{

    /*
     * Complete the 'formingMagicSquare' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts 2D_INTEGER_ARRAY s as parameter.
     */
 static int[,] ConvertTo2DArray(List<List<int>> listMatrix)
    {
        int rows = listMatrix.Count;
        int cols = listMatrix[0].Count;
        int[,] arrayMatrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arrayMatrix[i, j] = listMatrix[i][j];
            }
        }

        return arrayMatrix;
    }

    public static int formingMagicSquare(List<List<int>> s)
    {
        
         Matrix matrix = new Matrix(ConvertTo2DArray(s));

        int[][,] magicSquares = new int[8][,]
        {
            new int[,] {{8, 1, 6}, {3, 5, 7}, {4, 9, 2}},
            new int[,] {{6, 1, 8}, {7, 5, 3}, {2, 9, 4}},
            new int[,] {{4, 9, 2}, {3, 5, 7}, {8, 1, 6}},
            new int[,] {{2, 9, 4}, {7, 5, 3}, {6, 1, 8}},
            new int[,] {{8, 3, 4}, {1, 5, 9}, {6, 7, 2}},
            new int[,] {{4, 3, 8}, {9, 5, 1}, {2, 7, 6}},
            new int[,] {{6, 7, 2}, {1, 5, 9}, {8, 3, 4}},
            new int[,] {{2, 7, 6}, {9, 5, 1}, {4, 3, 8}}
        };

        int minCost = int.MaxValue;

        foreach (var magicSquare in magicSquares)
        {
            minCost = Math.Min(minCost, matrix.TransformationCost(magicSquare));
        }
        
        return minCost;

    }



}



