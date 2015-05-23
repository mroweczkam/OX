using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OX.Model;
using System.Windows.Threading;
using System.Windows.Media;

namespace OX.VM
{
    public class GameVM : INotifyPropertyChanged
    {

        #region Variables

        private Game game;
        private Grid gridMain;
        private DispatcherTimer _timer;
        private TimeSpan _time;
        private Player player1, player2, currentPlayer;
        private bool isEnd = false;
        private Position lastMove;
        private SolidColorBrush color = new SolidColorBrush(Colors.Bisque);
        private int startMinutes = 1;
        private List<History> history;

        #endregion

        #region Commands

        public ICommand Player1UpCmd { get; set; }
        public ICommand Player1DownCmd { get; set; }
        public ICommand Player1RightCmd { get; set; }
        public ICommand Player1LeftCmd { get; set; }

        public ICommand Player2UpCmd { get; set; }
        public ICommand Player2DownCmd { get; set; }
        public ICommand Player2RightCmd { get; set; }
        public ICommand Player2LeftCmd { get; set; }

        public ICommand Player1MarkCmd { get; set; }
        public ICommand Player2MarkCmd { get; set; }

        public ICommand NewGameCmd { get; set; }

        public ICommand PlayHistoryCmd { get; set; }

        #endregion

        #region Properties
        private string[] sizes = new string[] { "Small", "Medium", "Large" };

        public string[] boardSizes
        {
            get { return sizes; }
            set { }
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

        private Brush _player1Color;
        public Brush Player1Color
        {
            get { return _player1Color; }
            set
            {
                _player1Color = value;
                OnPropertyChanged("Player1Color");
            }
        }

        private Brush _player2Color;
        public Brush Player2Color
        {
            get { return _player2Color; }
            set
            {
                _player2Color = value;
                OnPropertyChanged("Player2Color");
            }
        }

        #endregion


        public GameVM(Grid GridMain)
        {
            gridMain = GridMain;

            Player1UpCmd = new RelayCommand(pars => up(player1));
            Player1DownCmd = new RelayCommand(pars => down(player1));
            Player1LeftCmd = new RelayCommand(pars => left(player1));
            Player1RightCmd = new RelayCommand(pars => right(player1));

            Player2UpCmd = new RelayCommand(pars => up(player2));
            Player2DownCmd = new RelayCommand(pars => down(player2));
            Player2LeftCmd = new RelayCommand(pars => left(player2));
            Player2RightCmd = new RelayCommand(pars => right(player2));

            Player1MarkCmd = new RelayCommand(pars => mark(player1));
            Player2MarkCmd = new RelayCommand(pars => mark(player2));

            NewGameCmd = new RelayCommand(pars => NewGame());
            PlayHistoryCmd = new RelayCommand(pars => PlayFromHistory(), pars => CanPlayHistory());

            history = new List<History>();
            NewGame();


        }





        #region Moves & mark methods

        private void up(Player player)
        {
            player.position.x = player.position.x - 1 < 0 ? game.size - 1 : player.position.x - 1;
            drawBoard();
        }
        private void down(Player player)
        {
            player.position.x = player.position.x + 1 >= game.size ? 0 : player.position.x + 1;
            drawBoard();
        }
        private void left(Player player)
        {
            player.position.y = player.position.y - 1 < 0 ? game.size - 1 : player.position.y - 1;
            drawBoard();
        }
        private void right(Player player)
        {
            player.position.y = player.position.y + 1 >= game.size ? 0 : player.position.y + 1;
            drawBoard();
        }

        private void mark(Player player)
        {
            if (currentPlayer == player && !isEnd)
            {
                if (game.mark(currentPlayer.position, currentPlayer.sign))
                {
                    lastMove = new Position(currentPlayer.position);
                    history.Add(new History(lastMove, currentPlayer.sign));
                    drawBoard();
                    if (game.isEnd())
                        winner();
                    else
                        countTime();

                }
            }
        }

        private void ChnageCurrentUser()
        {

            if (currentPlayer == player1)
            {
                currentPlayer = player2;
                Player1Color = null;
                Player2Color = color;
            }
            else
            {
                currentPlayer = player1;
                Player1Color = color;
                Player2Color = null;
            }
        }

        private void countTime()
        {
            currentPlayer.time = _time;
            ChnageCurrentUser();
            _time = currentPlayer.time;
            if (_timer != null)
                _timer.Stop();
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {

                if (currentPlayer == player1)
                    TimeLeftPlayer1 = getUserString(player1, _time);
                else
                    TimeLeftPlayer2 = getUserString(player2, _time);

                if (_time <= TimeSpan.Zero)
                {
                    MessageBox.Show("Koniec czasu gracza: " + currentPlayer.Name);
                    _timer.Stop();
                    isEnd = true;
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        #endregion

        #region Methods

        private void NewGame()
        {
            game = new Game(12);
            player1 = new Player("X", startMinutes);
            player2 = new Player("O", startMinutes);
            currentPlayer = player1;
            _time = new TimeSpan(0, startMinutes, 0);

            int pos = (int)Math.Ceiling(game.size / 2.0);

            player1.position.x = pos - 1;
            player1.position.y = pos - 1;

            player2.position.x = pos + 1;
            player2.position.y = pos + 1;

            isEnd = false;

            TimeLeftPlayer1 = getUserString(player1, player1.time);
            TimeLeftPlayer2 = getUserString(player2, player2.time);

            Player1Color = color;
            Player2Color = null;

            lastMove = null;
            drawBoard();

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
            Grid GridTest = createGrid.drawGrid(game.getBoardSize(), game.getBoard(), player1.position, player2.position, lastMove);

            gridMain.Children.Add(GridTest);
            Grid.SetColumn(GridTest, 1);
            Grid.SetRow(GridTest, 1);
        }

        private string getUserString(Player player, TimeSpan time)
        {
            return player.Name + " " + time.ToString("c");
        }

        private void winner()
        {
            isEnd = true;
            if (_timer != null)
                _timer.Stop();
            MessageBox.Show("Wygrał gracz " + currentPlayer.Name);
        }

        private void PlayFromHistory()
        {
            MessageBox.Show(history.Count.ToString());
            game = new Game(12);
            lastMove = null;
            int i = 0;
            drawBoard();
            History[] _tmp = history.ToArray();
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (i == _tmp.Length)
                    _timer.Stop();
                else
                {
                    History h = _tmp[i];
                    game.mark(h.position, h.sign);
                    drawBoard();
                    i++;
                }
                
            }, Application.Current.Dispatcher);
            _timer.Start();
        }

        private bool CanPlayHistory()
        {
            return game.isEnd();
        }

        #endregion

        #region INotifyPropertyChanged methods

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged = null;

        #endregion
    }
}


