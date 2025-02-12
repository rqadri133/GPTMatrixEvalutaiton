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
using System.Linq;

using System;

using Microsoft.ML; // Import ML.NET namespace
using Microsoft.ML.Data; 

// Define a class to represent your data

var mlContext = new MLContext(); 
    
    // Load data from a CSV file
    var dataPath = "Housing_Data.csv"; 
    var dataView = mlContext.Data.LoadFromTextFile<HousingData>(dataPath, hasHeader: true, separatorChar: ','); 
    
    // Split data into training and testing sets
    var split = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
    var trainData = split.TrainSet;
    var testData = split.TestSet; 
    
    // Define a data processing pipeline
    var pipeline = mlContext.Transforms.Categorical.OneHotEncoding("Location") // One-hot encode categorical features
        .Append(mlContext.Transforms.Concatenate("Features", "Size", "Bedrooms", "Location")) // Combine features
        .Append(mlContext.Transforms.NormalizeMinMax("Features")) // Normalize features 
        .Append(mlContext.Regression.Trainers.FastTree()); // Use a FastTree regression model 

    // Train the model
    var model = pipeline.Fit(trainData); 
    
    // Make predictions on new data
    var predictionEngine = mlContext.Model.CreatePredictionEngine<HousingData,PredictionOutput>(model);
    
    // Example prediction
    var newHouse = new HousingData { Size = 1500, Bedrooms = 3, Location = "Urban" };
    var prediction = predictionEngine.Predict(newHouse); 
    Console.WriteLine($"Predicted price: {prediction.Score}"); 

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

       int[,] inputMatrix = {
            { 4, 9, 2 },
            { 3, 5, 7 },
            { 8, 1, 5 }
        };

        var magicSquares = Result.FindMagicSquares(inputMatrix);
        Result.PrintMatrices(magicSquares);
        public class PredictionOutput
{
    public float Score { get; set; }
}

        public class HousingData 
{
    [LoadColumn(0)] // Specify which column in the data source maps to this property
    public float Size { get; set; } 
    
    [LoadColumn(1)]
    public Single Bedrooms { get; set; } 
    
    [LoadColumn(2)] 
    public string Location { get; set; } 
    
    [LoadColumn(3)] // Label to predict
    public float Price { get; set; } 
}



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
     public static List<int[,]> FindMagicSquares(int[,] inputMatrix)
    {
        var permutations = GetPermutations(inputMatrix);
        var magicSquares = new List<int[,]>();

        foreach (var perm in permutations)
        {
            if (IsMagicSquare(perm))
            {
                magicSquares.Add(perm);
            }
        }

        return magicSquares;
    }

    private static List<int[,]> GetPermutations(int[,] inputMatrix)
    {
        var numbers = inputMatrix.Cast<int>().ToArray();
        var permutations = Permute(numbers);
        var matrices = new List<int[,]>();

        foreach (var perm in permutations)
        {
            int[,] matrix = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrix[i, j] = perm[i * 3 + j];
                }
            }
            matrices.Add(matrix);
        }

        return matrices;
    }

    private static IEnumerable<int[]> Permute(int[] nums)
    {
        if (nums.Length == 1)
            yield return nums;
        else
        {
            for (int i = 0; i < nums.Length; i++)
            {
                var rest = nums.Take(i).Concat(nums.Skip(i + 1)).ToArray();
                foreach (var perm in Permute(rest))
                {
                    yield return new[] { nums[i] }.Concat(perm).ToArray();
                }
            }
        }
    }

    private static bool IsMagicSquare(int[,] matrix)
    {
        int sum = 15;
        for (int i = 0; i < 3; i++)
        {
            if (matrix[i, 0] + matrix[i, 1] + matrix[i, 2] != sum) return false;
            if (matrix[0, i] + matrix[1, i] + matrix[2, i] != sum) return false;
        }
        if (matrix[0, 0] + matrix[1, 1] + matrix[2, 2] != sum) return false;
        if (matrix[0, 2] + matrix[1, 1] + matrix[2, 0] != sum) return false;

        return true;
    }
  static List<int[,]> GenerateMagicSquares(int[,] baseSquare)
    {
         List<int[,]> magicSquares = new List<int[,]>();
        magicSquares.Add((int[,])baseSquare.Clone());
        
        for (int i = 0; i < 3; i++) // Rotate 3 times
        {
            baseSquare = Rotate90(baseSquare);
            magicSquares.Add((int[,])baseSquare.Clone());
        }
        
        baseSquare = Reflect(baseSquare); // Reflect
        magicSquares.Add((int[,])baseSquare.Clone());
        
        for (int i = 0; i < 3; i++) // Rotate reflected versions
        {
            baseSquare = Rotate90(baseSquare);
            magicSquares.Add((int[,])baseSquare.Clone());
        }
        
        return magicSquares;
    }

    static int[,] Rotate90(int[,] square)
    {
        int[,] rotated = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                rotated[j, 2 - i] = square[i, j];
        return rotated;
    }

    static int[,] Reflect(int[,] square)
    {
        int[,] reflected = new int[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                reflected[i, 2 - j] = square[i, j];
        return reflected;
    }
  public static void PrintMatrices(List<int[,]> matrices)
    {
        foreach (var matrix in matrices)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------");
        }
    }
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
        

      // List<int[,]> magicSquares  =   FindMagicSquares(ConvertTo2DArray(s));



        int minCost = int.MaxValue;

        foreach (var magicSquare in magicSquares)
        {
            minCost = Math.Min(minCost, matrix.TransformationCost(magicSquare));
        }
        
        return minCost;

    }



}



