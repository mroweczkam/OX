using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OX.Model;
using System.Windows.Threading;

namespace OX.VM
{
    public class GameVM : INotifyPropertyChanged
    {
        private Game game;
        private Grid gridMain;
        private DispatcherTimer _timer;
        private TimeSpan _time;

        private Player player1, player2, currentPlayer;

        public GameVM(Grid GridMain)
        {
            NewGame();
            gridMain = GridMain;

            MarkCmd = new RelayCommand(pars => Mark());

            MoveLeftCmdPlayer1 = new RelayCommand(pars => MoveLeft(player1));
            MoveRightCmdPlayer1 = new RelayCommand(pars => MoveRight(player1));
            MoveUpCmdPlayer1 = new RelayCommand(pars => MoveUp(player1));
            MoveDownCmdPlayer1 = new RelayCommand(pars => MoveDown(player1));
            
            MoveLeftCmdPlayer2 = new RelayCommand(pars => MoveLeft(player2));
            MoveRightCmdPlayer2 = new RelayCommand(pars => MoveRight(player2));
            MoveUpCmdPlayer2 = new RelayCommand(pars => MoveUp(player2));
            MoveDownCmdPlayer2 = new RelayCommand(pars => MoveDown(player2));

            drawBoard();
            Mark();
        }

        public void NewGame() {
            game = new Game();
            player1 = new Player();
            player2 = new Player();
            currentPlayer = player1;

            player1.position.x = (int)(Math.Ceiling(game.size/2.0)-1.0);
            player1.position.y = (int)(Math.Ceiling(game.size / 2.0) - 1.0);

            player2.position.x = (int)(Math.Ceiling(game.size / 2.0) + 1.0);
            player2.position.y = (int)(Math.Ceiling(game.size / 2.0) + 1.0);
        }


        private string _boardSize;
        public string boardSize
        {
            get { return _boardSize; }
            set
            {

                if (_boardSize != value)
                {
                    _boardSize = value;

                    game.size = Int32.Parse(value);
                    Reset();
                    OnPropertyChanged("boardSize");
                }

            }
        }

        private string[] sizes = new string[] { "Small", "Medium", "Large" };

        public string[] boardSizes
        {
            get
            {
                return sizes;
            }
            set { }
        }

        public ICommand MarkCmd { get; set; }
        public ICommand MoveLeftCmdPlayer1 { get; set; }
        public ICommand MoveRightCmdPlayer1 { get; set; }
        public ICommand MoveUpCmdPlayer1 { get; set; }
        public ICommand MoveDownCmdPlayer1 { get; set; }
        public ICommand MoveLeftCmdPlayer2 { get; set; }
        public ICommand MoveRightCmdPlayer2 { get; set; }
        public ICommand MoveUpCmdPlayer2 { get; set; }
        public ICommand MoveDownCmdPlayer2 { get; set; }


        private void Reset()
        {
            game.newGame();
            drawBoard();
        }


        private void MoveLeft(Player player)
        {
            if (player.position.y - 1 > -1)
                player.position.y = player.position.y - 1;
            drawBoard();

        }

        private void MoveRight(Player player)
        {
            if (player.position.y + 1 < game.size)
                player.position.y = player.position.y + 1;
            drawBoard();

        }

        private void MoveUp(Player player)
        {
            if (player.position.x - 1 > -1 )
                player.position.x = player.position.x - 1;
            drawBoard();
        }

        private void MoveDown(Player player)
        {
            if (player.position.x + 1 < game.size)
                player.position.x = player.position.x + 1;
            drawBoard();
        }


        private string _timeLeftPlayer1;
        public string TimeLeftPlayer1
        {
            get { return _timeLeftPlayer1; }
            set
            {
                _timeLeftPlayer1 = value;
                OnPropertyChanged("TimeLeftPlayer1");
            }
        }

        private string _timeLeftPlayer2;
        public string TimeLeftPlayer2
        {
            get { return _timeLeftPlayer2; }
            set
            {
                _timeLeftPlayer2 = value;
                OnPropertyChanged("TimeLeftPlayer2");
            }
        }

        private void Mark()
        {
            game.mark(1, 1, currentPlayer.sign);
            countTime();
        }

        private void ChnageCurrentUser()
        {

            if (currentPlayer == player1)
                currentPlayer = player2;
            else
                currentPlayer = player1;
        }

        private void countTime()
        {
            currentPlayer.time = _time;
            ChnageCurrentUser();
            _time = currentPlayer.time;

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
           {

               if (currentPlayer == player1)
                   TimeLeftPlayer1 = _time.ToString("c");
               else
                   TimeLeftPlayer2 = _time.ToString("c");

               if (_time == TimeSpan.Zero)
               {
                   MessageBox.Show("Koniec czasu gracza: " + currentPlayer.Name);
                   _timer.Stop();
               }

               _time = _time.Add(TimeSpan.FromSeconds(-1));
           }, Application.Current.Dispatcher);

            _timer.Start();
        }

        private void drawBoard()
        {
            int intTotalChildren = gridMain.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter > 0; intCounter--)
            {
                if (gridMain.Children[intCounter].GetType() == typeof(Grid))
                {
                    Grid ucCurrentChild = (Grid)gridMain.Children[intCounter];
                    gridMain.Children.Remove(ucCurrentChild);
                }
            }
            Grid GridTest = createGrid.drawGrid(game.getBoardSize(), game.getBoard(),player1.position,player2.position);

            gridMain.Children.Add(GridTest);
            Grid.SetColumn(GridTest, 1);
            Grid.SetRow(GridTest, 1);
        }
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged = null;
    }
}


