using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XO_Game_Final.Properties;

namespace XO_Game_Final
{
    public partial class Form1 : Form
    {
       
        stGameStatus GameStatus;
        enPlayer PlayerTurn = enPlayer.Player1;

        enum enPlayer { Player1, Player2 }
        enum enWinner { Player1, Player2, Draw, GameInProgress }

        struct stGameStatus
        {
            public enWinner Winner;
            public bool GameOver;
            public short PlayCount;
        }

        public Form1()
        {
            InitializeComponent();
        }
    
        private bool IsWinningLine(Button btn1, Button btn2, Button btn3)
        {
            return btn1.Tag.ToString() != "?" &&
                   btn1.Tag.ToString() == btn2.Tag.ToString() &&
                   btn1.Tag.ToString() == btn3.Tag.ToString();
        }

        private void SetWinner(Button btn1, Button btn2, Button btn3)
        {
            btn1.BackColor = Color.GreenYellow;
            btn2.BackColor = Color.GreenYellow;
            btn3.BackColor = Color.GreenYellow;

            if (btn1.Tag.ToString() == "X")
                GameStatus.Winner = enWinner.Player1;
            else
                GameStatus.Winner = enWinner.Player2;

            GameStatus.GameOver = true;
            EndGame();
        }

        public bool CheckValues(Button btn1, Button btn2, Button btn3)
        {
            if (IsWinningLine(btn1, btn2, btn3))
            {
                SetWinner(btn1, btn2, btn3);
                return true;
            }

            GameStatus.GameOver = false;
            return false;
        }

        public void CheckWinner()
        {
            if (CheckValues(button1, button2, button3)) return;
            if (CheckValues(button4, button5, button6)) return;
            if (CheckValues(button7, button8, button9)) return;

            if (CheckValues(button1, button4, button7)) return;
            if (CheckValues(button2, button5, button8)) return;
            if (CheckValues(button3, button6, button9)) return;

            if (CheckValues(button1, button5, button9)) return;
            if (CheckValues(button3, button5, button7)) return;
        }


        private void UpdateButtonStatus(Button btn, Image img, string tag)
        {
            btn.Image = img;
            btn.Tag = tag;
            GameStatus.PlayCount++;
        }

        private void SwitchPlayerTurn()
        {
            if (PlayerTurn == enPlayer.Player1)
            {
                PlayerTurn = enPlayer.Player2;
                lblTurn.Text = "Player 2";
            }
            else
            {
                PlayerTurn = enPlayer.Player1;
                lblTurn.Text = "Player 1";
            }
        }

        public void ChangeImage(Button btn)
        {
            if (btn.Tag.ToString() == "?")
            {
                if (PlayerTurn == enPlayer.Player1)
                    UpdateButtonStatus(btn, Resources.X, "X");
                else
                    UpdateButtonStatus(btn, Resources.O, "O");

                CheckWinner();

                if (!GameStatus.GameOver && GameStatus.PlayCount == 9)
                {
                    GameStatus.GameOver = true;
                    GameStatus.Winner = enWinner.Draw;
                    EndGame();
                }
                else if (!GameStatus.GameOver)
                {
                    SwitchPlayerTurn();
                }
            }
            else
            {
                MessageBox.Show("Wrong Choice", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void EndGame()
        {
            lblTurn.Text = "Game Over";
            switch (GameStatus.Winner)
            {
                case enWinner.Player1: lblWinner.Text = "Player1"; break;
                case enWinner.Player2: lblWinner.Text = "Player2"; break;
                default: lblWinner.Text = "Draw"; break;
            }
            MessageBox.Show("GameOver", "GameOver", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RestButton(Button btn)
        {
            btn.Image = Resources.question_mark_96;
            btn.Tag = "?";
            btn.BackColor = Color.Transparent;
        }

        private void ResetGameStatus()
        {
            PlayerTurn = enPlayer.Player1;
            lblTurn.Text = "Player 1";
            GameStatus.PlayCount = 0;
            GameStatus.GameOver = false;
            GameStatus.Winner = enWinner.GameInProgress;
            lblWinner.Text = "In Progress";
        }

        private void RestartGame()
        {
            RestButton(button1); RestButton(button2); RestButton(button3);
            RestButton(button4); RestButton(button5); RestButton(button6);
            RestButton(button7); RestButton(button8); RestButton(button9);

            ResetGameStatus();
        }


        private void button_Click(object sender, EventArgs e)
        {
            ChangeImage((Button)sender);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen whitePen = new Pen(Color.White, 15);
            whitePen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            whitePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            e.Graphics.DrawLine(whitePen, 400, 300, 1050, 300);
            e.Graphics.DrawLine(whitePen, 400, 460, 1050, 460);
            e.Graphics.DrawLine(whitePen, 610, 140, 610, 620);
            e.Graphics.DrawLine(whitePen, 840, 140, 840, 620);
        }
    }
}