using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpTUI.dlls;

namespace SharpTUI
{
    public class Component
    {
        public int left, right, top, bottom;
        protected const int height = 25, width = 80;
        public string type;
        public string borderMode;
        public Screen parent;
        public bool hasFocus;
        public bool hasLastClick;

        public Kernal32.CharInfo[] screenBuffer = new Kernal32.CharInfo[width * height]; 
        public bool[] screenBufferSet = new bool[width * height];

        public delegate void delMouseClick();
        public delMouseClick onLeftMouseClick;
        public delMouseClick onRightMouseClick;

        public Component(){

        }
        
        public struct Coord{

            public int X;
            public int Y;

            public Coord(int X, int Y){
                this.X = X;
                this.Y = Y;
            }
        };

        public virtual void gotFocus(int x, int y, char keyPressed){
            hasFocus = true;
        }


        //I'm also fucking broke, ha jokes on you!
        public virtual void lostFocus(){
            hasFocus = false;
        }

        public void onMouseClick(char keyPressed) {
            hasLastClick = true;
            if (keyPressed == Convert.ToChar("L")) onLeftMouseClick.Invoke();
            else if (keyPressed == Convert.ToChar("R")) onRightMouseClick.Invoke();
        }


        public virtual void render(){
                for (int i = 0; i < (width * height - 1); i++) {
                    if (screenBufferSet[i] == true) {
                        ScreenDriver.screenBuffer[i] = screenBuffer[i];
                    }
                }

            screenBufferSet = new bool[screenBuffer.Length];
            screenBuffer = new Kernal32.CharInfo[screenBuffer.Length];

        }

        public void drawHorizontalLine(int startX, int startY, int finish, Kernal32.CharInfo ci){
            for (int i = 0; i < (finish - startX); i++){
                screenBuffer[(i + startX) + (startY * width)] = ci;
                screenBufferSet[(i + startX) + (startY * width)] = true;
            }
        }

        public void drawVerticalLine(int startX, int startY, int finish, Kernal32.CharInfo ci){
            for (int i = 0; i < (finish - startY); i++){
                screenBuffer[(width * i + width * startY) + (startX)] = ci;
                screenBufferSet[(width * i + width * startY) + (startX)] = true;
            }
        }

        public void drawString(int x, int y, string text, short attributes) {
            for(int i = 0; i < text.Length; i++) {
                Kernal32.CharInfo ci = new Kernal32.CharInfo();
                ci.Char.UnicodeChar = text[i];
                ci.Attributes = attributes;
                screenBuffer[(i + x) + (y * width)] = ci;
                screenBufferSet[(i + x) + (y * width)] = true;
            }
        }

        public virtual void setText(string text){}
        public virtual string getText() { return null; }

        public void destroy(){
        }

    }
}
