using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTUI.dlls;

namespace SharpTUI
{
    public class Component
    {
        public int left, right, top, bottom;
        private const int height = 25, width = 80;
        public string type;
        public string borderMode;
        public Screen parent;
        public bool hasFocus;

        public Kernal32.CharInfo[] screenBuffer = new Kernal32.CharInfo[width * height]; // when merging arrays if != null for charInfo
        public bool[] screenBufferSet = new bool[width * height];

        //public delegate void del_MouseButtonClick(int x, int y);

        //  public del_MouseButtonClick onLeftMouseClick;
        //  public del_MouseButtonClick onRightMouseClick;

        public Component()
        {
            // onLeftMouseClick = leftClick;
            // onRightMouseClick = rightClick;
        }

        public struct Coord
        {
            public int X;
            public int Y;

            public Coord(int X, int Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        public void gotFocus(int x, int y, char keyPressed)
        {
            hasFocus = true;
        }

        public void lostFocus()
        {
            hasFocus = false;
        }


        public void render()
        {
            for (int i = 0; i < (width * height - 1); i++)
            {
                if (screenBufferSet[i] == true)
                {
                    ScreenDriver.screenBuffer[i] = screenBuffer[i];
                }
            }
        }

        // public void doSomeFuckingShitAndLearnToProgram()
        // {
        // onLeftMouseClick(10, 20); //Deleigates are in opisit places
        // }

        public void drawHorizontalLine(int startX, int startY, int finish, Kernal32.CharInfo ci)
        {
            for (int i = 0; i < (finish - startX); i++)
            {
                screenBuffer[(i + startX) + (startY * width)] = ci;
                screenBufferSet[(i + startX) + (startY * width)] = true;
            }
        }

        public void drawVerticalLine(int startX, int startY, int finish, Kernal32.CharInfo ci)
        {
            for (int i = 0; i < (finish - startY); i++)
            {
                screenBuffer[(width * i + width * startY) + (startX)] = ci;
                screenBufferSet[(width * i + width * startY) + (startX)] = true;
            }
        }


        public void destroy()
        {
        }

    }
}
