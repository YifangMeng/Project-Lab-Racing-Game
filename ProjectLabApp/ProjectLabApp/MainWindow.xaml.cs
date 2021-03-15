using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjectLabApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        List<Rectangle> rectangles = new List<Rectangle>();
        Random rand = new Random();
        ImageBrush playerCar = new ImageBrush();
        ImageBrush star = new ImageBrush();  //image of star
        Rect hitLocation;

        int gameSpeed = 15;
        int playerSpeed = 10;
        int numOfCar;
        int numOfStar;
        int rewardTime=200;
        double score;

        bool moveLeft;
        bool moveRight;
        bool gameOver;
        bool isReward;

        double rewardLevel;

        public MainWindow()
        {
            InitializeComponent();
            gameBoard.Focus();
            timer.Tick += GameLoop;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            GameStart();

        }

        private void GameLoop(object sender, EventArgs e)
        {
            score = score + 0.05;
            numOfStar = numOfStar - 1;
            scoreText.Content = "Survived " + score.ToString("#.#") + " Seconds";

            //throw new NotImplementedException();
        }

        public void GameStart()
        {
            gameSpeed = 8;
            timer.Start();

            isReward = false;
            moveLeft = false;
            moveRight = false;
            gameOver = false;
            score = 0;

            scoreText.Content = "Current Score: 00";
            playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\weak.png"));
            player.Fill = playerCar;

            star.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\star.png"));
            gameBoard.Background = Brushes.LightGreen;
            foreach(var item in gameBoard.Children.OfType<Rectangle>())
            {
                if ((string)item.Tag == "Car")
                {
                    Canvas.SetTop(item, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(item, (rand.Next(0, 430)));
                    SwitchCars(item);
                }

                if ((string)item.Tag == "star")
                {
                    rectangles.Add(item);
                }
            }
            rectangles.Clear();
        }

        private void starGenerator()
        {
            Rectangle newstar = new Rectangle
            {
                Height = 50,
                Width = 50,
                Tag = "star",
                Fill = star
            };
            Canvas.SetTop(newstar, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(newstar, rand.Next(0, 430));
            gameBoard.Children.Add(newstar);
        }
        private void SwitchCars(Rectangle car)
        {
            numOfCar = rand.Next(1, 6);
            ImageBrush othercar = new ImageBrush();
            switch (numOfCar)
            {
                case 1:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car1.png"));
                    break;
                case 2:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car2.png"));
                    break;
                case 3:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car3.png"));
                    break;
                case 4:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car4.png"));
                    break;
                case 5:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car5.png")); ;
                    break;
                case 6:
                    othercar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\car6.png"));
                    break;
            }
            car.Fill = othercar;
            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(car, rand.Next(0, 430));

        }

        private void RewardMode()
        {
            gameBoard.Background = Brushes.LightCoral;
        }

        private void gameBoard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void gameBoard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                GameStart();
            }
        }
    }
}
