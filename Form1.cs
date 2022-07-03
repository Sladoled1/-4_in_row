using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Rectangle[] boardColumns;// змінна для обмеження поля гри і задання їй чітких розмірів
        private int[,] board;// створює масив який заповнено 0
        private int turn;// змінна ходу

        public Form1()
        {
            InitializeComponent(); //ініціадізуємо змінні
            this.boardColumns = new Rectangle[7];//змінна для обмеження ігрового поля
            this.board = new int[6, 7];// створення масиву
            this.turn = 1;//задання змінни ходу
        }
        private void Form1_Paint(object sender, PaintEventArgs e) // створення поля 
        {
            e.Graphics.FillRectangle(Brushes.Blue, 0, 100, 700, 600);
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, 700, 100);
            for (int i = 0; i < 6; i++) //цикл який малює на формі чорні кола , які слугують вільними комірками
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0)// перевірка щоб круги малювались тільки у обмеженій області
                    {
                        this.boardColumns[j] = new Rectangle(5 + 100 * j, 0, 100, 700);//  задаємо параментри змінній для обмеження поля гри

                    }
                    e.Graphics.FillEllipse(Brushes.Black, 5 + 100 * j, 105 + 100 * i, 90, 90);//малювання кіл
                }
            }
        }
        private bool AllNumCheck(int tocheck, params int[] numbers)
        {
            foreach (int num in numbers)
            {
                if (num != tocheck)
                    return false;
            }
            return true;
        }
        private void nichiya()
        {
            //Перевірка чи є хоть одне можливе місце для ходу  
            for (int row = 0; row < this.board.GetLength(0); row++)
            {
                for (int col = 0; col < this.board.GetLength(1); col++)
                {
                    if (this.board[row, col] == 0)
                    {
                        return;
                    }
                }
            }
            //якщо нема то видаємо повідомлення про нічию
            MessageBox.Show("Tie");
            Application.Restart();
        }

        private int Win(int playertocheck) // фкція яка перевіряє виграш
        {
            //Вертикальний
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < this.board.GetLength(1); col++)
                {
                    if (this.AllNumCheck(playertocheck, this.board[row, col], this.board[row + 1, col], this.board[row + 2, col], this.board[row + 3, col]))
                        return playertocheck;
                }
            }
            //Горизонтальний
            for (int row = 0; row < this.board.GetLength(0); row++)
            {
                for (int col = 0; col < this.board.GetLength(1) - 3; col++)
                {
                    if (this.AllNumCheck(playertocheck, this.board[row, col], this.board[row, col + 1], this.board[row, col + 2], this.board[row, col + 3]))
                        return playertocheck;
                }
            }
            //Діагональний \
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 0; col < this.board.GetLength(1) - 3; col++)
                {
                    if (this.AllNumCheck(playertocheck, this.board[row, col], this.board[row + 1, col + 1], this.board[row + 2, col + 2], this.board[row + 3, col + 3]))
                        return playertocheck;
                }
            }
            //Діагональний /
            for (int row = 0; row < this.board.GetLength(0) - 3; row++)
            {
                for (int col = 3; col < this.board.GetLength(1); col++)
                {
                    if (this.AllNumCheck(playertocheck, this.board[row, col], this.board[row + 1, col - 1], this.board[row + 2, col - 2], this.board[row + 3, col - 3]))
                        return playertocheck;
                }
            }
            return -1;
        }
        private void anim(int turn,int RowIndex, int ColumIndex)//фкція, яка відображає анімацію падіння фішки
        {
            Graphics g = this.CreateGraphics();
            if (turn==1)
            {
                for (int k = 0; k != RowIndex; k++)// цикл відображення анімації падіння фішки для 2 гравця
                {
                    var t = Task.Delay(150);
                    g.FillEllipse(Brushes.Red, 5 + 100 * ColumIndex, 105 + 100 * k, 90, 90);
                    t.Wait();
                    g.FillEllipse(Brushes.Black, 5 + 100 * ColumIndex, 105 + 100 * k, 90, 90);
                    t.Wait();
                }
            }
            if (turn == 2)
            {
                for (int k = 0; k != RowIndex; k++)// цикл відображення анімації падіння фішки для 2 гравця
                {
                    var t = Task.Delay(150);
                    g.FillEllipse(Brushes.Green, 5 + 100 * ColumIndex, 105 + 100 * k, 90, 90);
                    t.Wait();
                    g.FillEllipse(Brushes.Black, 5 + 100 * ColumIndex, 105 + 100 * k, 90, 90);
                    t.Wait();
                }
            }
            
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e) // обробник події натискання клавіші на мишці
        {
           
            //для того щоб визначити де відобразитбся наш хід потрібно визначити індекси вільних комірок масиву у місці де ми натиснули клавішу
            int ColumIndex = this.VibirCol(e.Location);// знаходження індексу колонки
            if (ColumIndex !=-1)
            {
                int RowIndex = this.VilyniyRadok(ColumIndex);// знаходження індексу рядка
                if (RowIndex !=-1)
                {
                    this.board[RowIndex, ColumIndex] = this.turn;
                    if (this.turn==1)
                    {
                        Graphics g = this.CreateGraphics();                       
                         //відображення анімації падіння фішки для 1 гравця
                       this.anim(turn, RowIndex, ColumIndex);
                        // відображення ходу
                        g.FillEllipse(Brushes.Red, 5 + 100 * ColumIndex, 105 + 100 * RowIndex, 90, 90); 
                    }
                    else if(this.turn==2)
                    {
                        Graphics g = this.CreateGraphics();
                        this.anim(turn, RowIndex, ColumIndex);
                        // відображення ходу
                        g.FillEllipse(Brushes.Green, 5 + 100 * ColumIndex, 105 + 100 * RowIndex, 90, 90);                        
                    }
                    this.nichiya();
                    int winner = this.Win(this.turn);//виклик перевірки виграшу
                    if(winner!=-1)
                    {
                        //
                        if (winner == 1) 
                        {
                            MessageBox.Show("Congratulation player 1 win");
                            Application.Restart();
                        }
                        else if (winner == 2)
                        {
                            MessageBox.Show("Congratulation player 2 win");
                            Application.Restart();
                        }

                    }
                    if (this.turn == 1)
                        this.turn = 2;
                    else
                        this.turn = 1;
                }

            }
        }
        private int VibirCol(Point mouse) // фкція яка визначає вибір гравця
        {
            for(int i=0;i<this.boardColumns.Length;i++)
            {
                if((mouse.X>=this.boardColumns[i].X)&&(mouse.Y >= this.boardColumns[i].Y))
                {
                    if ((mouse.X <= this.boardColumns[i].X + this.boardColumns[i].Width) && (mouse.Y <= this.boardColumns[i].Y+this.boardColumns[i].Height))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private int VilyniyRadok(int col) // функція яка визначає вільну комірку у вибраному рядку
        {
            for(int i=5;i>=0;i--)
            {
                if(this.board[i,col]==0)
                {
                    return i;
                }
            }
            return -1;
        }
       

        private int Anymation(Point mouse)// фкція анімації
        {
            if ((mouse.X > this.boardColumns[0].X) && (mouse.X <= this.boardColumns[1].X))
            {
                return 0;
            }
            else if((mouse.X > this.boardColumns[1].X) && (mouse.X <= this.boardColumns[2].X))
            {
                return 1;
            }
            else if((mouse.X > this.boardColumns[2].X) && (mouse.X <= this.boardColumns[3].X))
            {
                return 2;
            }
            else if((mouse.X > this.boardColumns[3].X) && (mouse.X <= this.boardColumns[4].X))
            {
                return 3;
            }
            else if((mouse.X > this.boardColumns[4].X) && (mouse.X <= this.boardColumns[5].X))
            {
                return 4;
            }
            else if((mouse.X > this.boardColumns[5].X) && (mouse.X <= this.boardColumns[6].X))
            {
                return 5;
            }
            else if((mouse.X > this.boardColumns[6].X) && (mouse.X <= this.boardColumns[6].X+100))
            {
                return 6;
            }        
            else
            {
                return -1;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int Pos=-1;//оголошення локальної змінної для реалізації руху 
            Graphics r = this.CreateGraphics();
            r.FillRectangle(Brushes.Black, 0, 0, 700, 100);
            int CursorX = Cursor.Position.X;     
            Pos = this.Anymation(e.Location);//виклик функції для визначення позиції 
            //відображення анімації вибору ходу залежно від того чий хід
            if (this.turn == 1)
            {              
                Graphics g = this.CreateGraphics();
                r.FillEllipse(Brushes.Red, 5+ 100 * Pos, 5, 90, 90);
            }
            else if (this.turn == 2)
            {
                Graphics g = this.CreateGraphics();
                r.FillEllipse(Brushes.Green, 5 + 100 * Pos, 5, 90, 90);
            }
        }
        
    }
}
