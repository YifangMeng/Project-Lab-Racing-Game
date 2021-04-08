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
using System.Runtime.ExceptionServices;

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
        
        //Describe the width, height and location of a rectangle
        Rect hitLocation;

        int gameSpeed = 15;
        int playerSpeed = 10;
        int numOfCar;
        int numOfStar=30;
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

            hitLocation = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            if (numOfStar < 1)
            {
                starGenerator();
                numOfStar = rand.Next(600, 900);
            }

            foreach(var x in gameBoard.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "sign")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + gameSpeed);
                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }
                }
                if ((string)x.Tag == "Car")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + gameSpeed);
                    if (Canvas.GetTop(x) > 500)
                    {
                        SwitchCars(x);
                    }

                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (hitLocation.IntersectsWith(carHitBox) && isReward == true)
                    {
                        SwitchCars(x);
                    }
                    else if (hitLocation.IntersectsWith(carHitBox) && isReward == false)
                    {
                        timer.Stop();
                        scoreText.Content += "Press Enter to Restart !";
                        gameOver = true;
                    }
                }
                if ((string)x.Tag == "star")
                {
                    //The unit of 5 is pixel
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (hitLocation.IntersectsWith(starHitBox))
                    {
                        rectangles.Add(x);
                        isReward = true;
                        rewardTime = 200;
                    }
                    if (Canvas.GetTop(x) > 400)
                    {
                        rectangles.Add(x);
                    }
                }
            } //end of foreach loop

            if (isReward == true)
            {
                rewardTime -= 1;
                RewardMode();

                if (rewardTime < 1)
                {
                    isReward = false;
                }
                
            }
            else
            {
                playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\playerImage.png"));
                gameBoard.Background = Brushes.Gray;
            }

            foreach(Rectangle y in rectangles)
            {
                gameBoard.Children.Remove(y);
            }

            if (score >= 10 && score < 20)
            {
                gameSpeed = 12;
            }
            if (score >= 20 && score < 30)
            {
                gameSpeed = 14;
            }
            if (score >= 30 && score < 40)
            {
                gameSpeed = 16;
            }
            if (score >= 40 && score < 50)
            {
                gameSpeed = 18;
            }
            if (score >= 50 && score < 80)
            {
                gameSpeed = 22;
            }

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
            playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\playerImage.png"));
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
        
        //when player collects stars, the function runs
        private void RewardMode()
        {
            rewardLevel += 0.5;
            if (rewardLevel > 4)
            {
                rewardLevel = 1;
            }

            switch (rewardLevel)
            {
                case 1:
                    playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\powermode1.png"));
                    break;
                case 2:
                    playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\powermode2.png"));
                    break;
                case 3:
                    playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\powermode3.png"));
                    break;
                case 4:
                    playerCar.ImageSource = new BitmapImage(new Uri(@"C:\Users\mac\Desktop\image\powermode4.png"));
                    break;
            }
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
