using System.Threading.Tasks;

namespace Modeling2
{
    public partial class Form1 : Form
    {
        private int[,] initialDataArray;

        public Form1()
        {
            InitializeComponent();
            initialDataArray = new int[7, 7];
            string filePath = Path.Combine("..", "..", "..", "InitialData.txt"); // ����������� �� ������� ���� � ���� ����
            ArrayLoader.LoadArrayFromFile(filePath, initialDataArray);
            FillTable(initialDataArray);
        }

        private void FillTable(int[,] array)
        {
            // ��������������, ��� tableInitialData ��� ��������������� � ����� 8 ����� � 8 ��������
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
