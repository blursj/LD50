using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SARControlLib
{
	/// <summary>
	/// SARBoard.xaml 的交互逻辑
	/// </summary>
	public partial class SARBoard : UserControl
	{
		public SARBoard()
		{
            this.InitializeComponent();
            this.Board.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board1.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board1.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board2.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board2.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board3.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board3.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board4.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board4.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board5.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board5.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board6.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board6.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board7.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board7.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board8.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board8.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board9.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board9.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board10.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board10.MouseLeave += new MouseEventHandler(Board_MouseLeave);
            this.Board11.MouseEnter += new MouseEventHandler(Board_MouseEnter);
            this.Board11.MouseLeave += new MouseEventHandler(Board_MouseLeave);

            this.Board.MouseUp +=new MouseButtonEventHandler(Board_MouseUp);
            this.Board1.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board2.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board3.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board4.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board5.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board6.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board7.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board8.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board9.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board10.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
            this.Board11.MouseUp += new MouseButtonEventHandler(Board_MouseUp);
		}

        private void Board_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Children[2].Visibility = Visibility.Hidden;

        }

        private void Board_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Grid)sender).Children[2].Visibility = Visibility.Visible;

        }

        private void Board_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ( selectedBoardIndex < 12 && selectedBoardIndex >= 0 )
            {
                ((Grid)this.Boards.Children[selectedBoardIndex]).Children[0].Visibility = ((Grid)this.Boards.Children[selectedBoardIndex]).Children[0].Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible; 
            }
            ((Grid)sender).Children[0].Visibility = ((Grid)sender).Children[0].Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            if (sender == this.Board)
            {
                selectedBoardIndex = 0;
            }
            if (sender == this.Board1)
            {
                selectedBoardIndex = 1;
            }
            if (sender == this.Board2)
            {
                selectedBoardIndex = 2;
            }
            if (sender == this.Board3)
            {
                selectedBoardIndex = 3;
            }
            if (sender == this.Board4)
            {
                selectedBoardIndex = 4;
            }
            if (sender == this.Board5)
            {
                selectedBoardIndex = 5;
            }
            if (sender == this.Board6)
            {
                selectedBoardIndex = 6;
            }
            if (sender == this.Board7)
            {
                selectedBoardIndex = 7;
            }
            if (sender == this.Board8)
            {
                selectedBoardIndex = 8;
            }
            if (sender == this.Board9)
            {
                selectedBoardIndex = 9;
            }
            if (sender == this.Board10)
            {
                selectedBoardIndex = 10;
            }
            if (sender == this.Board11)
            {
                selectedBoardIndex = 11;
            }

            if (SARBoardClick != null)
            {
                SARBoardClick.Invoke(sender, selectedBoardIndex);
            }

        }

        //private int boardNums;
        //public int BoardNums
        //{
        //    get
        //    {
        //        return boardNums;
        //    }
        //    set
        //    {
        //        boardNums = value;
        //        for (int i = 0; i<boardNums && i < Boards.Children.Count; i++)
        //        {
        //            Boards.Children[i].Visibility = Visibility.Visible;
        //        }
        //        for (int i = boardNums; i < Boards.Children.Count; i++)
        //        {
        //            Boards.Children[i].Visibility = Visibility.Hidden;
        //        }
        //    }
        //}
        private int selectedBoardIndex=-1;
        public int SelectedBoardIndex
        {
            get
            {
                return selectedBoardIndex;
            }
            set
            {
                selectedBoardIndex = value;
                for (int i = 0; i < Boards.Children.Count; i++)
                {
                    if ( i == selectedBoardIndex )
                    {
                        Boards.Children[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Boards.Children[i].Visibility = Visibility.Hidden;
                    }
                }
            }
        }
        public event SARBoardClickHandler SARBoardClick;
	}
    
    public delegate void SARBoardClickHandler(object sender,int index);
}