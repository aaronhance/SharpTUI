using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using SharpTUI.dlls;

namespace SharpTUI
{
    public class CTextBox : Component
    {
        int width;
        const int height = 3;

        ScreenDriver.BorderPreset borderPreset;
        Kernal32.CharInfo blankPreset = new Kernal32.CharInfo();


        private string text = "";


        public CTextBox(int width, int positionX, int positionY, ScreenDriver.BorderPreset borderPreset)
{
            this.parent = parent;
            this.left = positionX;
            this.right = positionX + width;
            this.top = positionY;
            this.bottom = positionY + 2;
            this.width = width;

            this.borderPreset = borderPreset;
            type = "TextBox";
        }

        public void border(){

            blankPreset.Char.UnicodeChar = Convert.ToChar("B");
            blankPreset.Attributes = 01;

            drawHorizontalLine(left, top, right, borderPreset.horizontal);
            drawHorizontalLine(left, bottom, right, borderPreset.horizontal);
            drawVerticalLine(left, top, bottom, borderPreset.vertical);
            drawVerticalLine(right, top, bottom, borderPreset.vertical);

            drawHorizontalLine(left, top, left + 1, borderPreset.topLeft);
            drawHorizontalLine(right, top, right + 1, borderPreset.topRight);
            drawHorizontalLine(left, bottom, left + 1, borderPreset.bottomLeft);
            drawHorizontalLine(right, bottom, right + 1, borderPreset.bottomRight);

            drawString(left + 1, top + 1, text, 01);

        }

        public override void render() {
            border();
            base.render();
        }


        public override void gotFocus(int x, int y, char keyPressed){
            hasFocus = true;
        }

        public override void lostFocus(){
            hasFocus = false;
        }

        public override void setText(string text) {
            if (text.Length <= width - 2){
                this.text = text;
            }
            else throw new ArgumentException("(string text): \"" + text + "\" length is to long for the textbox! Max string length: " + (width - 2));
        }

        public override string getText() {
            return text;
        }

    }
}
