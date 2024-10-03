using System.Threading.Tasks;

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

        public Form1()
        {
            InitializeComponent();
            initialDataArray = new int[N, N];
            PI1 = new double[N];
            PI2 = new double[N];
            LI = new double[N];
            Tpr = new double[N];
            Toj = new double[N];
            string filePath = Path.Combine("..", "..", "..", "InitialData.txt"); // ����������� �� ������� ���� � ���� ����
            ArrayLoader.LoadArrayFromFile(filePath, initialDataArray);
            FillTable(initialDataArray);
        }

        private void FillTable(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    // ������� ����� Label ��� ������ ������ � ������������� �����
                    Label label = new Label
                    {
                        Text = array[i, j].ToString(),
                        AutoSize = true,
                        TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill // ��������� ������
                    };

                    // ��������� Label � ��������������� ������ �������
                    tableInitialData.Controls.Add(label, j + 1, i + 1); // j+1 � i+1 ��� ����� ����������
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
                    return -x[2].CompareTo(y[2]); // �������� �� LI
                }
                return result; // ��������� �� PI �� �����������
            });

            D2.Sort((x, y) =>
            {
                int result = x[1].CompareTo(y[1]);
                if (result == 0)
                {
                    return -x[2].CompareTo(y[2]); // ����� �� ���������� ���������
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

            // ���������� copyLI
            for (int i = 0; i < N; i++)
            {
                copyLI.Add(new List<double> { i, LI[i], PI1[i] });
            }

            // ���������� copyLI �� LI � PI1 � ������� ��������
            copyLI = copyLI.OrderByDescending(x => x[1]).ThenByDescending(x => x[2]).ToList();

            // ���������� ������ posled
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

            // ���������� ������� D0, D1 � D2
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

            // ���������� �������
            D0 = D0.OrderBy(x => x[1]).ToList();
            D1 = D1.OrderBy(x => x[1]).ToList();
            D2 = D2.OrderByDescending(x => x[1]).ToList();

            // ���������� ������ posled
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

        private void buttonPetrovsRule1_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule1();
            labelPetrovsRule.Text = "�� ������� ������� � 1 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
        }

        private void buttonPetrovsRule2_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule2();
            labelPetrovsRule.Text = "�� ������� ������� � 2 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
        }

        private void buttonPetrovsRule3_Click(object sender, EventArgs e)
        {
            CalcPI();
            CalcLI();
            List<int> posled = Rule3();
            labelPetrovsRule.Text = "�� ������� ������� � 3 [";
            labelPetrovsRule.Text += string.Join(", ", posled);
            labelPetrovsRule.Text += "]";
            double[,] T = CalcT(posled);
            CalcTpr(T);
            CalcToj(T, posled);
            UpdateTableWithTojAndTpr();
        }

        // ������ �������
        private double[,] CalcT(List<int> posled)
        {
            // ���������, ��� ������ ������ posled ������������� N
            if (posled.Count != N)
            {
                throw new ArgumentException("���������� ��������� � 'posled' ������ ���� ����� N.");
            }

            double[,] T = new double[N, N];
            T[0, 0] = initialDataArray[posled[0] - 1, 0]; // ��������� �� 1, ��� ��� ���������� � 0

            for (int i = 1; i < N; i++)
            {
                T[0, i] = T[0, i - 1] + initialDataArray[posled[0] - 1, i]; // ��������� �� 1
                T[i, 0] = T[i - 1, 0] + initialDataArray[posled[i] - 1, 0]; // ��������� �� 1
            }

            for (int i = 1; i < N; i++)
            {
                for (int j = 1; j < N; j++)
                {
                    T[i, j] = Math.Max(T[i - 1, j], T[i, j - 1]) + initialDataArray[posled[i] - 1, j]; // ��������� �� 1
                }
            }

            return T;
        }


        private void CalcTpr(double[,] T)
        {
            for (int i = 0; i < N; i++)
            {
                double sumSt = 0;

                for (int j = 0; j < N; j++) // j �� 1 �� N, ������� N
                {
                    sumSt += initialDataArray[j, i]; // M[j, i] ������ M[j + 1][i], ����� �� �������� �� �������
                }

                Tpr[i] = T[N - 1, i] - sumSt; // ���������� ������� �������
            }
        }

        // ����� �������� ������
        private void CalcToj(double[,] T, List<int> posled)
        {
            for (int i = 0; i < N; i++)
            {
                double sum = 0;

                // ��������� ������ �� 1, ��� ��� ������� ������������� � 0
                for (int j = 0; j < N; j++)
                {
                    sum += initialDataArray[posled[i] - 1, j]; // ������� ����� �� �������
                }

                Toj[i] = T[i, N - 1] - sum;
            }
        }

        private void UpdateTableWithTojAndTpr()
        {
            // ����� Tpr � ������ ������ �������
            for (int j = 0; j < N; j++)
            {
                Label label = new Label
                {
                    Text = Tpr[j].ToString(),
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(label, j + 1, N + 1); // j + 1 � ������ ����������
            }

            // ����� Toj � ������ ������� �������
            for (int i = 0; i < N; i++)
            {
                Label label = new Label
                {
                    Text = Toj[i].ToString(),
                    AutoSize = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(label, N + 1, i + 1); // i + 1 � ������ ����������
            }
        }


    }
}
