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
        string borderMode;
        int width;
        const int height = 3;

        ScreenDriver.BorderPreset borderPreset;
        Kernal32.CharInfo blankPreset = new Kernal32.CharInfo();

        SafeFileHandle consoleHandle;

        public CTextBox(int width, int positionX, int positionY, ScreenDriver.BorderPreset borderPreset)
        {
            this.parent = parent;
            this.left = positionX;
            this.right = positionX + width;
            this.top = positionY;
            this.bottom = positionY + 2;
            this.width = width;

            this.borderPreset = borderPreset;
            consoleHandle = Kernal32.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            border();
        }

        public void border()
        {

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

            //  for (int i = 175; i < 220; i++) {
            //      Kernal32.CharInfo x = new Kernal32.CharInfo();
            //      x.Char.UnicodeChar = Convert.ToChar(i);
            //      x.Attributes = 01;
            //      screenBuffer[i] = x;
            //      screenBufferSet[i] = true;
            //  }
        }

        //public void textField;

        public void gotFocus(int x, int y, char keyPressed)
        {
            hasFocus = true;
            Console.WriteLine("I'm being called!");
            Console.SetCursorPosition(left + 1, top - 1);
        }

        public void lostFocus()
        {
            hasFocus = false;
        }

    }
}
