using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX
{
    class Game
    {

        public int size { get
            
        {
        return 20;
        }
             set{}}
        public int[][] board;

     

        public Game()
        {
            newGame();
        }


        public void newGame()
        {
            board = new int[size][];

            for (int i = 0; i < size; i++)
            {
                 board[i] = new int[size];

                for (int j = 0; j < size; j++)
                {
                    board[i][j] = 0;
                }
            }
        }


        public void mark(int i, int j, int player)
        {
            board[i][j] = player;
        }

        public int getBoardSize(){
            return this.size;
        }

        public int[][] getBoard() {
            return this.board;
        }


          

    }
}
