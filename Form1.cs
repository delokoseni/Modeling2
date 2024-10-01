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
            string filePath = Path.Combine("..", "..", "..", "InitialData.txt"); // Поднимаемся на уровень выше и ищем файл
            ArrayLoader.LoadArrayFromFile(filePath, initialDataArray);
            FillTable(initialDataArray);
        }

        private void FillTable(int[,] array)
        {
            // Предполагается, что tableInitialData уже инициализирован и имеет 8 строк и 8 столбцов
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
