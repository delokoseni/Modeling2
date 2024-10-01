using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Modeling2
{

    internal class ArrayLoader
    {
        // Считывает данные из текстового файла и заполняет переданный многомерный массив
        public static void LoadArrayFromFile(string filePath, int[,] array)
        {
            // Проверка на существование файла
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден.", filePath);
            }

            // Чтение всех строк из файла
            string[] lines = File.ReadAllLines(filePath);

            // Проверка на соответствие размера массива
            if (lines.Length != array.GetLength(0))
            {
                throw new ArgumentException("Количество строк в файле не соответствует размеру массива.");
            }

            for (int i = 0; i < lines.Length; i++)
            {
                // Разделение строки на элементы
                string[] values = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Проверка на соответствие размера строки массиву
                if (values.Length != array.GetLength(1))
                {
                    throw new ArgumentException($"Количество элементов в строке {i + 1} не соответствует размеру массива.");
                }

                for (int j = 0; j < values.Length; j++)
                {
                    // Преобразование строки в целое число и заполнение массива
                    if (int.TryParse(values[j], out int number))
                    {
                        array[i, j] = number;
                    }
                    else
                    {
                        throw new FormatException($"Неверный формат числа в строке {i + 1}, элемент {j + 1}.");
                    }
                }
            }
        }
    }
}