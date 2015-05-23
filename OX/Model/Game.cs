using System.Security.Cryptography;
using System.Windows.Navigation;
using OX.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX.Model
{
    public class Game
    {

        public const string EMPTY = "";
        public int size { get; set; }
        
        public string[][] board;



        public Game(int size)
        {
            this.size = size;
            newGame();
        }


        public void newGame()
        {
            board = new string[size][];

            for (int i = 0; i < size; i++)
            {
                board[i] = new string[size];

                for (int j = 0; j < size; j++)
                {
                    board[i][j] = EMPTY;
                }
            }
        }


        public bool mark(Position position, string sign)
        {
            if (board[position.x][position.y] == EMPTY)
            {
                board[position.x][position.y] = sign;
                return true;
            }

            return false;
        }

        public int getBoardSize()
        {
            return this.size;
        }

        public string[][] getBoard()
        {
            return this.board;
        }

        public bool isEnd()
        {
            int a = 5;
            for (int i = 0; i < size - 4; i++)
            {
                for (int j = 0; j < size - 4; j++)
                {
                    if (board[i][j] == EMPTY)
                        continue;
                    if (checkHorizontal(i, j))
                        return true;
                    if (checkVertical(i, j))
                        return true;
                    if (checkSlantRightDown(i, j))
                        return true;
                    if (j >= 4 && checkSlantLeftDown(i, j))
                        return true;
                }
            }

            for (int i = 0; i < size - 4; i++)
            {
                for (int j = size - 4; j < size; j++)
                {
                    if (board[i][j] == EMPTY)
                        continue;
                    if (checkSlantLeftDown(i, j))
                        return true;
                }
            }
            return false;
        }

        private bool checkHorizontal(int x, int y)
        {
            string sign = board[x][y];
            for (int i = 1; i < 5; i++)
            {
                if (board[x][y + i] != sign)
                    return false;
            }
            return true;
        }

        private bool checkVertical(int x, int y)
        {
            string sign = board[x][y];
            for (int i = 1; i < 5; i++)
            {
                if (board[x + i][y] != sign)
                    return false;
            }
            return true;
        }

        private bool checkSlantRightDown(int x, int y)
        {
            string sign = board[x][y];
            for (int i = 1; i < 5; i++)
            {
                if (board[x + i][y + i] != sign)
                    return false;
            }
            return true;
        }

        private bool checkSlantLeftDown(int x, int y)
        {
            string sign = board[x][y];
            for (int i = 1; i < 5; i++)
            {
                if (board[x + i][y - i] != sign)
                    return false;
            }
            return true;
        }
    }
}
