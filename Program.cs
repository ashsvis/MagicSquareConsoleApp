using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MagicSquareConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] magicSquare = GetMagicSquare(5);
            for (int i = 0; i < magicSquare.GetLength(0); i++)
            {
                for (int j = 0; j < magicSquare.GetLength(1); j++)
                {
                    Console.Write($"{magicSquare[i, j],4}");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Магический квадрат для нечетной стороны
        /// </summary>
        /// <param name="side">Размер стороны квадрата</param>
        /// <returns>Матрица магического квадрата</returns>
        private static int[,] GetMagicSquare(int side)
        {
            // смещение от середины стороны
            var offset = (side - 1) / 2;
            // список точек, содержащий индексы квадратной матрицы
            var list = new List<PointF>();
            // заполняем список, где центральный элемент квадрата будет иметь индекс 0,0
            for (var i = -offset; i <= offset; i++)
                for (var j = -offset; j <= offset; j++)
                    list.Add(new PointF(j, i));
            // получаем массив точкек с индексами
            var pts = list.ToArray();
            // заводим матрицу для вычисления поворота индексов 
            var m = new Matrix();
            // матрица смещается для придания верхнему левому элементу индекса 0,0
            m.Translate(offset, offset);
            // при повороте на 45° смещение учитывается смещение на корень из двух
            var sc = (float)Math.Sqrt(2f);
            // увеличиваем смещение
            m.Scale(sc, sc);
            // поворачиваем на 45°
            m.Rotate(45f);
            // выполняем трансформацию индексов
            m.TransformPoints(pts);
            // при трансформации возникают погрешности, которые устраняем округлением до целого
            for (var i = 0; i < pts.Length; i++)
            {
                pts[i].X = (float)Math.Round(pts[i].X, 0);
                pts[i].Y = (float)Math.Round(pts[i].Y, 0);
            }
            // создаем результирующу матрицу
            int[,] ms = new int[side, side];
            var n = 1; // начальный индекс
            foreach (var pt in pts)
            {
                if (pt.X >= 0 && pt.X < side && pt.Y >= 0 && pt.Y < side)
                    ms[(int)pt.X, (int)pt.Y] = n;           // индексы в пределах матрицы
                else if (pt.X < 0)
                    ms[(int)pt.X + side, (int)pt.Y] = n;    // индекс за левой гранью
                else if (pt.Y < 0)
                    ms[(int)pt.X, (int)pt.Y + side] = n;    // индекс за верхней гранью
                else if (pt.X >= side)
                    ms[(int)pt.X - side, (int)pt.Y] = n;    // индекс за правой гранью
                else if (pt.Y >= side)
                    ms[(int)pt.X, (int)pt.Y - side] = n;    // индекс за нижней гранью
                n = n + 1; // приращение индекса 
            }
            return ms;
        }

        private static int[,] GetNotMagicSquare(int side)
        {
            Random random = new Random();
            var range = Enumerable.Range(1, side * side).Select(n => n).ToList();
            int[,] ms = new int[side, side];
            for (int i = 0; i < ms.GetLength(0); i++)
            {
                for (int j = 0; j < ms.GetLength(1); j++)
                {
                    ms[i, j] = range[random.Next(0, range.Count())];
                    range.Remove(ms[i, j]);
                }
            }
            return ms;
        }
    }
}
