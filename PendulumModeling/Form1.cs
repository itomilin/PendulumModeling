using System;
using System.Drawing;
using System.Windows.Forms;

namespace PendulumModeling
{
    public partial class Form1 : Form
    {
        // Переменные для визуализации.
        private Graphics rg;
        private RectangleF rectangle;
        private PointF point;

        private const double G = 9.81;    // Ускорение свободного падения.
        private double k1, k2, k3, k4;    // Переменные для уравнения.
        private const double STEP = 0.1;  // Шаг для уравнения.
        private const double _length = 10; // Длина нити.

        private double friction = 0.0;    // Коэффициент трения
        private double w = 0.0;           // Коэффициент диф. уравнения.

        // Углы для моделирования движения
        private double omega,
                       theta;


        private double Alpha(double omega, double theta)
        {
            //return -(G / _length) * Math.Sin(theta);
            return -2 * friction * omega - (Math.Pow(w, 2) * Math.Sin(theta));
        }


        private double Theta(double omega)
        {
            return omega;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // Решаем диф. уравнение методом Рунге-Кутта 4 порядка
            k1 = Alpha(omega, theta);
            k2 = Alpha(omega, theta + (STEP * k1 * 1 / 2));
            k3 = Alpha(omega, theta + (STEP * k2 * 1 / 2));
            k4 = Alpha(omega, theta + (STEP * k3));
            omega += (STEP / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4); // Угловая Скорость

            k1 = Theta(omega);
            k2 = Theta(omega + (STEP * k1 * 1 / 2));
            k3 = Theta(omega + (STEP * k2 * 1 / 2));
            k4 = Theta(omega + (STEP * k3));
            theta += (STEP / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4); // Угол 

            // Рисуем маятник, для визуализации добавляем координаты формы и множитель.
            rectangle.X = (float)(Math.Sin(theta) * _length * 20) + pictureBox1.Width / 2;
            rectangle.Y = (float)(Math.Cos(theta) * _length * 20) + pictureBox1.Height / 2;

            // Рисуем конец линии. Добавляем радиус окружности, чтобы линия крепилась по центру.
            point.X = rectangle.X + 20;
            point.Y = rectangle.Y;

            // Перерисовываем контрол с шаром.
            pictureBox1.Invalidate();
        }


        public Form1()
        {
            InitializeComponent();
            textBox3.Text = _length.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Включаем таймер
            timer1.Start();

            // Задаем начальные значения переменных при новом старте.
            // Блокируем кнопки.
            button1.Enabled = false;
            rectangle.Y = pictureBox1.Height / 2;
            rectangle.X = pictureBox1.Width / 2;
            theta = 5.5;
            omega = (double)numericUpDown1.Value;//Math.Sqrt(G / _length);
            friction = (double)numericUpDown2.Value;//Math.Sqrt(G / _length);
            w = (double)numericUpDown3.Value;//Math.Sqrt(G / _length);
            button2.Enabled = true;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Останавливаем таймер, разблокируем кнопки.
            timer1.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 3); // Цвет и толщина линии.
            rg = e.Graphics; // Инициализация графики.
            rg.FillEllipse(Brushes.DeepSkyBlue, rectangle); // Определяем маятник.
            rg.DrawLine(pen, pictureBox1.Width / 2, 0, point.X, point.Y);        // Рисуем линию.
            rg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Добавим сглаживание.

            point = new PointF(pictureBox1.Width / 2, pictureBox1.Height / 2);
            rectangle = new RectangleF(pictureBox1.Width / 2, pictureBox1.Height / 2, 40, 40);
        }
    }
}
