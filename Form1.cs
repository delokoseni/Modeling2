using System.Threading.Tasks;
using System.Linq;

namespace Modeling2
{
    public partial class Form1 : Form
    {
        private int[,] initialDataArray;
        private int N = 7;
        private double[] PI1;
        private double[] PI2;
        private double[] LI;
        double[] Tpr;
        double[] Toj;
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            initialDataArray = new int[N, N];
            PI1 = new double[N];
            PI2 = new double[N];
            LI = new double[N];
            Tpr = new double[N];
            Toj = new double[N];
            g = CreateGraphics();
            string filePath = Path.Combine("..", "..", "..", "InitialData.txt"); // Поднимаемся на уровень выше и ищем файл
            ArrayLoader.LoadArrayFromFile(filePath, initialDataArray);
            FillTable(initialDataArray);
        }

        private void FillTable(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    // Создаем новый Label для каждой ячейки и устанавливаем текст
                    Label label = new Label
                    {
                        Text = array[i, j].ToString(),
                        AutoSize = true,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill // Заполняем ячейку
                    };

                    // Добавляем Label в соответствующую ячейку таблицы
                    tableInitialData.Controls.Add(label, j + 1, i + 1); // j+1 и i+1 для учета заголовков
                }
            }
        }

        public void CalcLI()
        {
            for (int i = 0; i < N; i++)
            {
                LI[i] = PI2[i] - PI1[i];
            }
        }

        public void CalcPI()
        {
            double ot, to;
            if (N % 2 == 0)
            {
                ot = N / 2;
                to = N / 2;
            }
            else
            {
                ot = (N + 1) / 2 - 1;
                to = (N + 1) / 2;
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < to; j++)
                {
                    PI1[i] += initialDataArray[i, j];
                }
                for (int j = (int)ot; j < N; j++)
                {
                    PI2[i] += initialDataArray[i, j];
                }
            }
        }

        public List<int> Rule1()
        {
            List<int[]> D01 = new List<int[]>();
            List<int[]> D2 = new List<int[]>();
            List<int> posled = new List<int>();

            for (int i = 0; i < N; i++)
            {
                if (LI[i] >= 0)
                {
                    D01.Add(new int[] { i, (int)PI1[i], (int)LI[i] });
                }
                else
                {
                    D2.Add(new int[] { i, (int)PI2[i], (int)LI[i] });
                }
            }

            D01.Sort((x, y) =>
            {
                int result = x[1].CompareTo(y[1]);
                if (result == 0)
                {
                    return -x[2].CompareTo(y[2]); // Убывание по LI
                }
                return result; // Сравнение по PI по возрастанию
            });

            D2.Sort((x, y) =>
            {
                int result = x[1].CompareTo(y[1]);
                if (result == 0)
                {
                    return -x[2].CompareTo(y[2]); // здесь не используем отрицание
                }
                return result;
            });
            D2.Reverse();

            foreach (var item in D01)
            {
                posled.Add(item[0] + 1);
            }

            foreach (var item in D2)
            {
                posled.Add(item[0] + 1);
            }

            return posled;
        }


        private List<int> Rule2()
        {
            List<List<double>> copyLI = new List<List<double>>();
            List<int> posled = new List<int>();

            // Заполнение copyLI
            for (int i = 0; i < N; i++)
            {
                copyLI.Add(new List<double> { i, LI[i], PI1[i] });
            }

            // Сортировка copyLI по LI и PI1 в порядке убывания
            copyLI = copyLI.OrderByDescending(x => x[1]).ThenByDescending(x => x[2]).ToList();

            // Заполнение списка posled
            foreach (var item in copyLI)
            {
                posled.Add((int)item[0] + 1);
            }

            return posled;
        }

        private List<int> Rule3()
        {
            List<List<double>> D0 = new List<List<double>>();
            List<List<double>> D1 = new List<List<double>>();
            List<List<double>> D2 = new List<List<double>>();
            List<int> posled = new List<int>();

            // Заполнение списков D0, D1 и D2
            for (int i = 0; i < N; i++)
            {
                if (LI[i] == 0)
                {
                    D0.Add(new List<double> { i, PI1[i] });
                }
                else if (LI[i] > 0)
                {
                    D1.Add(new List<double> { i, PI1[i] });
                }
                else
                {
                    D2.Add(new List<double> { i, PI2[i] });
                }
            }

            // Сортировка списков
            D0 = D0.OrderBy(x => x[1]).ToList();
            D1 = D1.OrderBy(x => x[1]).ToList();
            D2 = D2.OrderByDescending(x => x[1]).ToList();

            // Заполнение списка posled
            foreach (var item in D1)
            {
                posled.Add((int)item[0] + 1);
            }

            foreach (var item in D0)
            {
                posled.Add((int)item[0] + 1);
            }

            foreach (var item in D2)
            {
                posled.Add((int)item[0] + 1);
            }

            return posled;
        }

        public List<int> Rule4()
        {
            List<List<double>> D0 = new List<List<double>>();
            List<List<double>> D1 = new List<List<double>>();
            List<List<double>> D2 = new List<List<double>>();
            List<int> posled = new List<int>();

            // Заполнение списков D0, D1 и D2
            for (int i = 0; i < N; i++)
            {
                if (LI[i] == 0)
                {
                    D0.Add(new List<double> { i, PI1[i], PI2[i] });
                }
                else if (LI[i] > 0)
                {
                    D1.Add(new List<double> { i, PI1[i], PI2[i] });
                }
                else
                {
                    D2.Add(new List<double> { i, PI1[i], PI2[i] });
                }
            }

            // Основная логика обработки
            while (D1.Count > 0)
            {
                if (D1.Count == 1) break;

                var maxElement = D1.OrderByDescending(x => x[2]).First();
                D1.Remove(maxElement);

                var minElement = D1.OrderBy(x => x[1]).First();
                D1.Remove(minElement);

                var computationResult = new List<double> // Переименовали переменную
        {
            maxElement[0], minElement[0],
            maxElement[2] - minElement[1],
            Math.Max(maxElement[2] - maxElement[1], minElement[2] - minElement[1]),
            Math.Min(maxElement[2] - maxElement[1], minElement[2] - minElement[1])
        };
                // Добавляем результат в posled
                posled.Add((int)maxElement[0] + 1);
                posled.Add((int)minElement[0] + 1);
            }

            while (D0.Count > 0)
            {
                if (D1.Count == 1 && D0.Count == 1)
                {
                    var pair0 = D1[0];
                    D1.RemoveAt(0);
                    var pair1 = D0.OrderBy(x => x[1]).First();
                    D0.Remove(pair1);

                    var computationResult = new List<double> // Переименовали переменную
            {
                pair0[0], pair1[0],
                pair0[2] - pair1[1],
                Math.Max(pair0[2] - pair0[1], pair1[2] - pair1[1]),
                Math.Min(pair0[2] - pair0[1], pair1[2] - pair1[1])
            };
                    // Добавляем результат в posled
                    posled.Add((int)pair0[0] + 1);
                    posled.Add((int)pair1[0] + 1);
                    break;
                }

                if (D0.Count == 1) break;

                var selected = D0.OrderByDescending(x => x[2]).First();
                D0.Remove(selected);

                var minElement = D0.OrderBy(x => x[1]).First();
                D0.Remove(minElement);

                var computationResultList = new List<double> // Переименовали переменную
        {
            selected[0], minElement[0],
            selected[2] - minElement[1],
            Math.Max(selected[2] - selected[1], minElement[2] - minElement[1]),
            Math.Min(selected[2] - selected[1], minElement[2] - minElement[1])
        };
                // Добавляем результат в posled
                posled.Add((int)selected[0] + 1);
                posled.Add((int)minElement[0] + 1);
            }

            while (D2.Count > 0)
            {
                if ((D1.Count == 1 || D0.Count == 1) && D2.Count == 1)
                {
                    var pair0 = D1.Count == 1 ? D1[0] : D0[0];
                    if (D1.Count == 1) D1.RemoveAt(0);
                    else D0.RemoveAt(0);

                    var pair1 = D2.OrderBy(x => x[1]).First();
                    D2.Remove(pair1);

                    var computationResultD2 = new List<double> // Переименовали переменную
            {
                pair0[0], pair1[0],
                pair0[2] - pair1[1],
                Math.Max(pair0[2] - pair0[1], pair1[2] - pair1[1]),
                Math.Min(pair0[2] - pair0[1], pair1[2] - pair1[1])
            };
                    // Добавляем результат в posled
                    posled.Add((int)pair0[0] + 1);
                    posled.Add((int)pair1[0] + 1);
                    break;
                }

                if (D2.Count == 1) break;

                var maxInD2 = D2.OrderByDescending(x => x[2]).First();
                D2.Remove(maxInD2);

                var minInD2 = D2.OrderBy(x => x[1]).First();
                D2.Remove(minInD2);

                var computationResultD2List = new List<double> // Переименовали переменную
        {
            maxInD2[0], minInD2[0],
            maxInD2[2] - minInD2[1],
            Math.Max(maxInD2[2] - maxInD2[1], minInD2[2] - minInD2[1]),
            Math.Min(maxInD2[2] - maxInD2[1], minInD2[2] - minInD2[1])
        };
                // Добавляем результат в posled
                posled.Add((int)maxInD2[0] + 1);
                posled.Add((int)minInD2[0] + 1);
            }

            // Обработка последнего элемента
            if (N % 2 != 0)
            {
                var lastIndex = D0.Count > 0 ? D0[0][0] :
                              D1.Count > 0 ? D1[0][0] :
                              D2.Count > 0 ? D2[0][0] : -1;

                if (lastIndex != -1)
                {
                    posled.Add((int)lastIndex + 1);
                }
            }

            return posled;
        }

        private void buttonPetrovsRule1_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule1();
            labelPetrovsRule.Text = "По правилу Петрова № 1 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(posled);
            PrintTable2(posled, T);
            DrawProcessingTimes(posled);
        }

        private void buttonPetrovsRule2_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule2();
            labelPetrovsRule.Text = "По правилу Петрова № 2 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(posled);
            PrintTable2(posled, T);
            DrawProcessingTimes(posled);
        }

        private void buttonPetrovsRule3_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule3();
            labelPetrovsRule.Text = "По правилу Петрова № 3 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(posled);
            PrintTable2(posled, T);
            DrawProcessingTimes(posled);
        }

        // Массив времени
        private double[,] CalcT(List<int> posled)
        {
            // Убедитесь, что размер списка posled соответствует N
            if (posled.Count != N)
            {
                throw new ArgumentException("Количество элементов в 'posled' должно быть равно N.");
            }

            double[,] T = new double[N, N];
            T[0, 0] = initialDataArray[posled[0] - 1, 0]; // Уменьшаем на 1, так как индексация с 0

            for (int i = 1; i < N; i++)
            {
                T[0, i] = T[0, i - 1] + initialDataArray[posled[0] - 1, i]; // Уменьшаем на 1
                T[i, 0] = T[i - 1, 0] + initialDataArray[posled[i] - 1, 0]; // Уменьшаем на 1
            }

            for (int i = 1; i < N; i++)
            {
                for (int j = 1; j < N; j++)
                {
                    T[i, j] = Math.Max(T[i - 1, j], T[i, j - 1]) + initialDataArray[posled[i] - 1, j]; // Уменьшаем на 1
                }
            }

            return T;
        }

        private void CalcTpr(double[,] T)
        {
            for (int i = 0; i < N; i++)
            {
                double sumSt = 0;

                for (int j = 0; j < N; j++) // j от 1 до N, включая N
                {
                    sumSt += initialDataArray[j, i]; // M[j, i] вместо M[j + 1][i], чтобы не выходить за пределы
                }

                Tpr[i] = T[N - 1, i] - sumSt; // Вычисление времени простоя
            }
        }

        // Время ожидания детали
        private void CalcToj(double[,] T, List<int> posled)
        {
            for (int i = 0; i < N; i++)
            {
                double sum = 0;

                // Уменьшаем индекс на 1, так как массивы индексируются с 0
                for (int j = 0; j < N; j++)
                {
                    sum += initialDataArray[posled[i] - 1, j]; // Удалили выход за границы
                }

                Toj[i] = T[i, N - 1] - sum;
            }
        }

        private void UpdateTableWithTojAndTpr()
        {
            // Вывод Tpr в нижнюю строку таблицы
            for (int j = 0; j < N; j++)
            {
                Label label = new Label
                {
                    Text = Tpr[j].ToString(),
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(label, j + 1, N + 1); // j + 1 с учетом заголовков
            }

            // Вывод Toj в правый столбец таблицы
            for (int i = 0; i < N; i++)
            {
                Label label = new Label
                {
                    Text = Toj[i].ToString(),
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(label, N + 1, i + 1); // i + 1 с учетом заголовков
            }
        }

        private void UpdateTableWithPosled(List<int> posled)
        {
            // Заполнение первого столбца таблицы значениями из List<int> posled
            for (int i = 0; i < posled.Count; i++)
            {
                Label label = new Label
                {
                    Text = posled[i].ToString(),
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(label, 0, i + 1); // 0 для первого столбца, i + 1 учитывает заголовки
            }
        }

        private void PrintTable2(List<int> posled, double[,] T)
        {
            if (posled.Count == 0 || N < 7) return; // Проверка на наличие данных и достаточное количество столбцов

            // Заполнение таблицы значениями
            for (int i = 0; i < N; i++) // Проходим по индексам 'posled'
            {
                int posledIndex = posled[i] - 1; // Координаты в массиве 'initialDataArray'

                if (posledIndex < initialDataArray.GetLength(0)) // Проверка на валидность индекса
                {
                    for (int j = 0; j < N; j++) // Проходим по столбцам 1-N
                    {
                        // Проверка на выход за границы массива T
                        if (j < T.GetLength(1))
                        {
                            // Заполнение ячеек значениями из initialDataArray и T
                            Label valueLabel = new Label
                            {
                                Text = $"{initialDataArray[posledIndex, j]} / {T[i, j]}",
                                AutoSize = true,
                                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Fill
                            };
                            tableLayoutPanel1.Controls.Add(valueLabel, j + 1, i + 1); // Заполнение ячеек (индексы с 1 по 7)
                        }
                    }
                }
            }

            // Подсчет и заполнение итоговой ячейки
            double tprSum = Tpr.Sum();
            double tojSum = Toj.Sum();

            // Заполнение итоговой ячейки
            Label sumLabel = new Label
            {
                Text = $"{tprSum} / {tojSum}",
                AutoSize = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel1.Controls.Add(sumLabel, N + 1, N + 1); // Итоговая ячейка
        }

        private void buttonPetrovsRule4_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule4();
            labelPetrovsRule.Text = "По правилу Петрова № 4 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(posled);
            PrintTable2(posled, T);
            DrawProcessingTimes(posled);
        }

        private void buttonEnumerate_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Task();
            labelPetrovsRule.Text = "Результат методом перебора [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(posled);
            PrintTable2(posled, T);
            DrawProcessingTimes(posled);
        }

        //рандом
        private int[] Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
            return array;
        }

        private bool NextSet(int[] a, int n)
        {
            int j = n - 2;
            while (j != -1 && a[j] >= a[j + 1])
            {
                j--;
            }

            if (j == -1)
            {
                return false; // Нет следующей перестановки
            }

            int k = n - 1;
            while (a[j] >= a[k])
            {
                k--;
            }

            Swap(a, j, k);

            int l = j + 1;
            int r = n - 1;

            while (l < r)
            {
                Swap(a, l, r);
                l++;
                r--;
            }

            return true; // Перестановка была изменена
        }

        private List<List<int>> AllPosled(int N)
        {
            List<List<int>> permutations = new List<List<int>>();
            int[] Nt = new int[N];
            for (int i = 0; i < N; i++)
            {
                Nt[i] = i + 1;
            }
            permutations.Add(Nt.ToList());

            while (NextSet(Nt, N))
            {
                permutations.Add(Nt.ToList());
            }

            return permutations;
        }

        private List<int> Task()
        {
            List<List<int>> permutations = AllPosled(N);
            double minT = double.MaxValue;
            double minToj = double.MaxValue;
            List<int> minPerm = null;

            foreach (var perm in permutations)
            {
                double[,] T = CalcT(perm); // Используем метод CalcT
                CalcToj(T, perm); // Метод обновляет массив Toj

                double currentTValue = T[N - 1, N - 1];

                if (currentTValue < minT ||
                    (currentTValue == minT && Toj.Sum() < minToj))
                {
                    minT = currentTValue;
                    minPerm = new List<int>(perm); // Клонируем список
                    minToj = Toj.Sum();
                }
            }

            return minPerm;
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();

            // Генерируем случайную последовательность
            List<int> randomSequence = GetRandomSequence(N);

            // Обновляем текст метки с результатом
            labelPetrovsRule.Text = "Случайная последовательность [";
            labelPetrovsRule.Text += string.Join(", ", randomSequence);
            labelPetrovsRule.Text += "]";

            double[,] T = CalcT(randomSequence);
            CalcTpr(T);
            CalcToj(T, randomSequence);
            UpdateTableWithTojAndTpr();
            UpdateTableWithPosled(randomSequence);
            PrintTable2(randomSequence, T);
        }

        private List<int> GetRandomSequence(int size)
        {
            Random random = new Random();
            List<int> sequence = Enumerable.Range(1, size).ToList(); // Создаем список от 1 до size

            // Перемешиваем последовательность
            for (int i = 0; i < sequence.Count; i++)
            {
                int j = random.Next(i, sequence.Count); // Получаем случайный индекс
                                                        // Меняем элементы местами
                int temp = sequence[i];
                sequence[i] = sequence[j];
                sequence[j] = temp;
            }

            return sequence;
        }

        private void DrawProcessingTimes(List<int> posled)
        {
            int detailCount = N; // Количество деталей
            int spacingBetweenLines = 20; // Промежуток между строками
            int heightOfEachLine = 10; // Высота каждой линии
            int baseY = 400; // Начальная координата Y для рисования
            Color[] colors = new Color[]
            {
                Color.Red, Color.Green, Color.Blue,
                Color.Yellow, Color.Cyan, Color.Magenta, Color.Orange
            };

            // Двумерный массив для хранения времён завершения обработки деталей на каждом станке
            int[,] completionTimes = new int[detailCount, detailCount];

            for (int machine = 0; machine < detailCount; machine++)
            {
                int yOffset = baseY + machine * (heightOfEachLine + spacingBetweenLines);
                int currentX = 0; // Начальная координата X для рисования

                for (int count = 0; count < detailCount; count++)
                {
                    int detailIndex = posled[count] - 1; // Индекс детали (с 0)
                    int processingTime = initialDataArray[detailIndex, machine] * 10; // Время * 10 (в пикселях)

                    // Определяем время простоя
                    if (machine > 0 && processingTime > 0) // Проверяем, что время обработки больше 0
                    {
                        int previousCompletionTime = completionTimes[detailIndex, machine - 1];
                        if (currentX < previousCompletionTime)
                        {
                            int idleTime = previousCompletionTime - currentX;

                            if (count < detailCount - 1 || idleTime > 0)
                            {
                                using (Brush grayBrush = new SolidBrush(Color.Gray))
                                {
                                    g.FillRectangle(grayBrush, currentX, yOffset, idleTime, heightOfEachLine);
                                }

                                if (idleTime > 0)
                                {
                                    string idleText = (idleTime / 10).ToString(); // Делим на 10 для отображения длины
                                    using (Font font = new Font("Arial", 8))
                                    {
                                        SizeF textSize = g.MeasureString(idleText, font);
                                        float textX = currentX + (idleTime - textSize.Width) / 2;
                                        float textY = yOffset + (heightOfEachLine - textSize.Height) / 2;
                                        g.DrawString(idleText, font, Brushes.Black, textX, textY);
                                    }
                                }

                                currentX += idleTime; // Обновляем текущую координату X после простоя
                            }
                        }
                    }

                    // Определяем цвет для каждой детали и рисуем прямоугольник для времени обработки
                    using (Brush brush = new SolidBrush(colors[detailIndex % colors.Length]))
                    {
                        if (processingTime > 0) // Проверяем, что время обработки больше 0
                        {
                            g.FillRectangle(brush, currentX, yOffset, processingTime, heightOfEachLine);

                            string text = initialDataArray[detailIndex, machine].ToString();
                            using (Font font = new Font("Arial", 8))
                            {
                                SizeF textSize = g.MeasureString(text, font);
                                float textX = currentX + (processingTime - textSize.Width) / 2;
                                float textY = yOffset + (heightOfEachLine - textSize.Height) / 2;
                                g.DrawString(text, font, Brushes.Black, textX, textY);
                            }
                        }
                    }

                    // Обновляем время завершения обработки детали на текущем станке
                    completionTimes[detailIndex, machine] = currentX + processingTime;

                    // Обновляем текущую координату X
                    currentX += processingTime;
                }

                // Проверяем наличие времени простоя после обработки всех деталей
                if (currentX < completionTimes[detailCount - 1, machine])
                {
                    int idleTimeAfterProcessing = completionTimes[detailCount - 1, machine] - currentX;

                    if (idleTimeAfterProcessing > 0)
                    {
                        using (Brush grayBrush = new SolidBrush(Color.Gray))
                        {
                            g.FillRectangle(grayBrush, currentX, yOffset, idleTimeAfterProcessing, heightOfEachLine);
                        }

                        string idleTextAfterProcessing = (idleTimeAfterProcessing / 10).ToString();
                        using (Font font = new Font("Arial", 8))
                        {
                            SizeF textSize = g.MeasureString(idleTextAfterProcessing, font);
                            float textX = currentX + (idleTimeAfterProcessing - textSize.Width) / 2;
                            float textY = yOffset + (heightOfEachLine - textSize.Height) / 2;
                            g.DrawString(idleTextAfterProcessing, font, Brushes.Black, textX, textY);
                        }
                    }
                }

                // Добавляем текст "Станок №" над каждой строкой
                using (Font font = new Font("Arial", 8))
                {
                    string machineLabel = $"Станок №{machine + 1}";
                    SizeF labelSize = g.MeasureString(machineLabel, font);
                    float labelX = 0; // Позиция X для метки
                    float labelY = baseY + machine * (heightOfEachLine + spacingBetweenLines) - labelSize.Height - 3;
                    g.DrawString(machineLabel, font, Brushes.Black, labelX, labelY);
                }
            }
        }

    }
}
