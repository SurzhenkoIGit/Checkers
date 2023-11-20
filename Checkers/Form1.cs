using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{

    public partial class Form1 : Form
    {
        const int fieldSize = 8; //размер поля
        const int cell = 50; //размер клетки

        int curPlayer;

        List<Button> simpleSteps = new List<Button>();

        int countSteps = 0;
        bool isContinue = false;
        Button prevButton;
        Button pressButton;

        bool isMove;

        int[,] field = new int[fieldSize, fieldSize];
        Button[,] buttons = new Button[fieldSize, fieldSize];

        Image white;
        Image black;
        public Form1()
        {
            InitializeComponent();

            lbl_playerName.Text = label5.Text;
            white = new Bitmap(new Bitmap("w.png"), new Size(cell - 1, cell - 1));
            black = new Bitmap(new Bitmap("b.png"), new Size(cell - 1, cell -1));
            InitMap();
        }

        public void InitMap() //инициализация поля
        {
            curPlayer = 1;
            isMove = false;
            prevButton = null;

            field = new int[fieldSize, fieldSize]
            {
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };
            FillMap();
        }

        public void ResetGame() //рестарт игры
        {
            bool player1 = false;
            bool player2 = false;

            for(int i =0; i < fieldSize; i++)
            {
                for(int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 1)
                        player1 = true;
                    if (field[i, j] == 2)
                        player2 = true;
                }
            }
            if(!player1)
            {
                lbl_player2Score.Text += 1;
                this.Controls.Clear();
                InitMap();
            }
            if(!player2)
            {
				lbl_player1Score.Text += 1;
				this.Controls.Clear();
				InitMap();
			}
        }

        public void FillMap() //заполнение поля
        {
            this.Width = ((fieldSize + 1) * cell) + 200;
            this.Height = (fieldSize + 1) * cell;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    
                    Button button = new Button();
                    //button.BackColor= Color.White;
                    button.Location = new Point(j * cell, i * cell);
                    button.Size = new Size(cell, cell);
                    button.Click += new EventHandler(Button_Click);
                    if (field[i,j] == 1)
                    {
                        button.Image = white;
                    }
                    else if (field[i,j] == 2)
                    {
                        button.Image = black;
                    }

                    button.BackColor = GetButtonColor(button);
                    button.ForeColor = Color.Red;
                    

                    buttons[i, j] = button;

                    this.Controls.Add(button);
                }
            }
        }

        public void Switch() //смена хода игрока
        {
            //curPlayer = curPlayer == 1 ? 2 : 1;
            if(curPlayer == 1)
            {
                curPlayer = 2;
                lbl_playerName.Text = label6.Text;
            }
            else
            {
                curPlayer = 1;
                lbl_playerName.Text = label5.Text;
            }
            ResetGame();
        }

        public Color GetButtonColor(Button prevButton)
        {
            if ((prevButton.Location.Y / cell) % 2 != 0)
            {
                if ((prevButton.Location.X / cell) % 2 == 0)
                {
                    return Color.Gray;
                }
            }
            if ((prevButton.Location.Y / cell) % 2 == 0)
            {
                if ((prevButton.Location.X / cell) % 2 != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }
        public void Button_Click(object sender, EventArgs e) //алогритм отработки события при нажатии на шашку
        {
            if(prevButton != null)
            {
                prevButton.BackColor = GetButtonColor(prevButton);
            }

            pressButton = sender as Button;

            if (field[pressButton.Location.Y/cell, pressButton.Location.X/cell] != 0 && field[pressButton.Location.Y / cell, pressButton.Location.X / cell] == curPlayer)
            {
                CloseSteps();
                pressButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressButton.Enabled = true;
                countSteps = 0;
                if (pressButton.Text == "D")
                    ShowSteps(pressButton.Location.Y / cell, pressButton.Location.X / cell, false);
                else
                    ShowSteps(pressButton.Location.Y / cell, pressButton.Location.X / cell);
                if (isMove)
                {
                    CloseSteps();
                    pressButton.BackColor = GetButtonColor(pressButton);
                    PossibleSteps();
                    isMove = false;
                }
                else
                    isMove = true;
            }
            else
            {
                if(isMove)
                {
                    isContinue = false;
                    if(Math.Abs(pressButton.Location.X / cell - prevButton.Location.X / cell) > 1)
                    {
                        isContinue = true;
                        DeleteDestroy(pressButton, prevButton);
                    }

                    int temp = field[pressButton.Location.Y / cell, pressButton.Location.X / cell];
                    field[pressButton.Location.Y / cell, pressButton.Location.X / cell] = field[prevButton.Location.Y/cell, prevButton.Location.X / cell];
                    field[prevButton.Location.Y / cell, prevButton.Location.X / cell] = temp;
                    pressButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    SwitchButton(pressButton);
                    countSteps = 0;
                    isMove = false;
                    CloseSteps();
                    DeactivateAllButtons();
                    if(pressButton.Text == "D")
                        ShowSteps(pressButton.Location.Y / cell, pressButton.Location.X / cell, false);
                    else
                        ShowSteps(pressButton.Location.Y / cell, pressButton.Location.X / cell);
                    if(countSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        Switch();
                        PossibleSteps();
                        isContinue = false;
                    }
                    else if(isContinue)
                    {
                        pressButton.BackColor = Color.Red;
                        pressButton.Enabled = true;
                        isMove = true;
                    }
                }
            }
            prevButton = pressButton;
        }

        public void PossibleSteps() //алгоритм для отображения возможных ходов шашки
        {
            bool isOneStep = true;
            bool isDestrStep = false;
            DeactivateAllButtons();
            for(int i = 0; i < fieldSize; i++)
            {
                for(int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == curPlayer)
                    {
                        if (buttons[i, j].Text == "D")
                        {
                            isOneStep = false;
                        }
                        else
                            isOneStep = true;
                        if (DestroyCheckStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isDestrStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if(!isDestrStep)
            {
                ActivateAllButtons();
            }
        }

        public void SwitchButton(Button button) //поменять шашку на "дамку"
        {
            if (field[button.Location.Y / cell, button.Location.X / cell] == 1 && button.Location.Y / cell == fieldSize - 1)
            {
                button.Text = "D";
                button.Font = new Font("Arial", 10, FontStyle.Italic);
            }
            if(field[button.Location.X / cell, button.Location.Y / cell] == 2 && button.Location.Y == 0)
            {
                button.Text = "D";
                button.Font = new Font("Arial", 10, FontStyle.Italic);
            }
        }

        public void DeleteDestroy(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.Location.Y / cell - startButton.Location.Y / cell);
            int startIndexX = endButton.Location.Y / cell - startButton.Location.Y / cell;
            int startIndexY = endButton.Location.X / cell - startButton.Location.X / cell;

            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;

            int curCount = 0;
            int i = startButton.Location.Y / cell + startIndexX;
            int j = startButton.Location.X / cell + startIndexY;
            while(curCount < count - 1)
            {
                field[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                curCount++;
            }
        }

        public void ShowSteps(int iCur, int jCur, bool isOneStep = true) 
        {
            simpleSteps.Clear();
            DiagonalDir(iCur, jCur, isOneStep);
            if (countSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        public void DiagonalDir(int iCur, int jCur, bool isOneStep = false)
        {
            int j = jCur + 1;
            for (int i = iCur - 1; i >= 0; i--)
            {
                if (curPlayer == 1 && isOneStep && !isContinue)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (!isPath(i, j))
                        break;
                }
                if (j < 7)
                {
                    j++;
                }
                else
                    break;
                
                if (isOneStep)
                {
                    break;
                }
            }

            j = jCur - 1;
            for (int i = iCur - 1; i >= 0; i--)
            {
                if (curPlayer == 1 && isOneStep && !isContinue)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (!isPath(i, j))
                        break;
                }
                if (j > 0)
                {
                    j--;
                }
                else
                    break;

                if (isOneStep)
                    break;
            }

            j = jCur - 1;
            for (int i = iCur + 1; i < 8; i++)
            {
                if (curPlayer == 2 && isOneStep && !isContinue)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (!isPath(i, j))
                        break;
                }
                if (j > 0)
                {
                    j--;
                }
                else
                    break;

                if (isOneStep)
                    break;
            }

            j = jCur + 1;
            for (int i = iCur + 1; i < 8; i++)
            {
                if (curPlayer == 2 && isOneStep && !isContinue)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (!isPath(i, j))
                        break;
                }
                if (j < 7)
                {
                    j++;
                }
                else
                    break;

                if (isOneStep)
                    break;
            }
        }


        public bool isPath(int i, int j)
        {
            if (field[i, j] == 0 && !isContinue)
            {
                buttons[i, j].BackColor = Color.Yellow;
                buttons[i, j].Enabled = true;
                simpleSteps.Add(buttons[i, j]);
            }
            else
            {
                if (field[i, j] != curPlayer)
                {
                    if (pressButton.Text == "D")
                    {
                        ProcedureDestroy(i, j, false);
                    }
                    else
                        ProcedureDestroy(i, j);
                }
                return false;
            }
            return true;
        }

        public void CloseSimpleSteps(List<Button> Steps)
        {
            if(Steps.Count > 0)
            {
                for(int i = 0; i < Steps.Count; i++)
                {
                    Steps[i].BackColor = GetButtonColor(Steps[i]);
                    Steps[i].Enabled = false;
                }
            }
        }

        public void ProcedureDestroy(int i, int j, bool isOneStep = true)
        {
            int dirX = i - pressButton.Location.Y / cell;
            int dirY = j - pressButton.Location.X / cell;

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            int indexI = i;
            int indexJ = j;

            bool isEmpty = true;
            while(isBorders(indexI, indexJ))
            {
                if (field[indexI, indexJ] != 0 && field[indexI, indexJ] != curPlayer)
                {
                    isEmpty = false;
                    break;
                }
                indexI += dirX;
                indexJ += dirY;

                if (isOneStep)
                    break;
            }
            if (isEmpty)
                return;
            List<Button> toClose = new List<Button>();
            bool closeStep = false;
            int ik = indexI + dirX;
            int jk = indexJ + dirY;
            while(isBorders(ik, jk))
            {
                if (field[ik, jk] == 0)
                {
                    if (DestroyCheckStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        closeStep = true;
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]);
                    }
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countSteps++;
                }
                else
                    break;
                if (isOneStep)
                    break;
                jk += dirY;
                ik += dirX;
            }
            if(closeStep && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }
        }

        public bool DestroyCheckStep(int iCur, int jCur, bool isStep, int[] dir)
        {
            bool destrStep = false;
            int j = jCur + 1;
            for(int i = iCur - 1; i >= 0; i--)
            {
                if(curPlayer == 1 && isStep && !isContinue)
                {
                    break;
                }
                if (dir[0] == 1 && dir[1] == -1 && !isStep)
                {
                    break;
                }
                if(isBorders(i, j))
                {
                    if (field[i,j] != 0 && field[i,j] != curPlayer)
                    {
                        destrStep = true;
                        if(!isBorders(i-1, j+1))
                        {
                            destrStep = false;
                        }
                        else if (field[i-1,j+1] != 0)
                        {
                            destrStep = false;
                        }
                        else
                        {
                            return destrStep;
                        }
                    }
                }
                if(j < 7)
                {
                    j++;
                }
                else
                {
                    break;
                }
                if(isStep)
                {
                    break;
                }
            }
            j = jCur - 1;
            for (int i = iCur - 1; i >= 0; i--)
            {
                if (curPlayer == 1 && isStep && !isContinue)
                {
                    break;
                }
                if (dir[0] == 1 && dir[1] == 1 && !isStep)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (field[i, j] != 0 && field[i, j] != curPlayer)
                    {
                        destrStep = true;
                        if (!isBorders(i - 1, j - 1))
                        {
                            destrStep = false;
                        }
                        else if (field[i - 1, j - 1] != 0)
                        {
                            destrStep = false;
                        }
                        else
                        {
                            return destrStep;
                        }
                    }
                }
                if (j > 0)
                {
                    j--;
                }
                else
                {
                    break;
                }
                if (isStep)
                {
                    break;
                }
            }
            j = jCur - 1;
            for (int i = iCur + 1; i < 8; i++)
            {
                if (curPlayer == 2 && isStep && !isContinue)
                {
                    break;
                }
                if (dir[0] == -1 && dir[1] == 1 && !isStep)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (field[i, j] != 0 && field[i, j] != curPlayer)
                    {
                        destrStep = true;
                        if (!isBorders(i + 1, j - 1))
                        {
                            destrStep = false;
                        }
                        else if (field[i + 1, j - 1] != 0)
                        {
                            destrStep = false;
                        }
                        else
                        {
                            return destrStep;
                        }
                    }
                }
                if (j > 0)
                {
                    j--;
                }
                else
                {
                    break;
                }
                if (isStep)
                {
                    break;
                }
            }
            j = jCur + 1;
            for (int i = iCur + 1; i < 8; i++)
            {
                if (curPlayer == 2 && isStep && !isContinue)
                {
                    break;
                }
                if (dir[0] == -1 && dir[1] == -1 && !isStep)
                {
                    break;
                }
                if (isBorders(i, j))
                {
                    if (field[i, j] != 0 && field[i, j] != curPlayer)
                    {
                        destrStep = true;
                        if (!isBorders(i + 1, j + 1))
                        {
                            destrStep = false;
                        }
                        else if (field[i + 1, j + 1] != 0)
                        {
                            destrStep = false;
                        }
                        else
                        {
                            return destrStep;
                        }
                    }
                }
                if (j < 7)
                {
                    j++;
                }
                else
                {
                    break;
                }
                if (isStep)
                {
                    break;
                }
            }
            return destrStep;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    buttons[i, j].BackColor = GetButtonColor(buttons[i,j]);
                }
            }
        }

        private bool isBorders(int i, int j)
        {
            if(i >= fieldSize || j >= fieldSize || i < 0 || j < 0)
            {
                return false;
            }
            return true;
        }

        public void ActivateAllButtons()
        {
            for(int i = 0; i < fieldSize; i++)
            {
                for(int j = 0; j < fieldSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }
    }
}
