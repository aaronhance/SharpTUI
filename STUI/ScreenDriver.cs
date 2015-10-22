using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using SharpTUI.dlls;

namespace SharpTUI
{
    public static class ScreenDriver
    {
        private static Dictionary<string, Screen> screens = new Dictionary<string, Screen>();
        private static Screen currentScreen = null;
        private static Component componentFocused;

        private static User32.RECT windowRect;
        private static IntPtr handle;

        static SafeFileHandle consoleHandle;
        public static Kernal32.CharInfo[] screenBuffer = new Kernal32.CharInfo[80*25];
        public static Kernal32.CONSOLE_SCREEN_BUFFER_INFO consoleInfo = new Kernal32.CONSOLE_SCREEN_BUFFER_INFO();

        private static Keys lastKey;

        public delegate void delMouseButtonPress(int x, int y, char keyPressed);
        public delegate void delKeyboardButtonPress(Keys key);

        public static delMouseButtonPress onMouseButtonPressed;
        public static delKeyboardButtonPress onKeyboardButtonPressed;

        public static void init(){
            //callBacks
            onMouseButtonPressed = mouseButtonPressed;
            onKeyboardButtonPressed = keyboardButtonPressed;
            //other shit
            consoleHandle = Kernal32.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            Thread mouseHandler = new Thread(new ThreadStart(() => new IO.Mouse()));
            Thread keyboardHandler = new Thread(new ThreadStart(() => new IO.Keyboard()));
            mouseHandler.Start();
            keyboardHandler.Start();

            handle = Process.GetCurrentProcess().MainWindowHandle;
            User32.GetWindowRect(handle, ref windowRect);

            Console.CursorVisible = false;
         }


        public static void render() { 
            Kernal32.SmallRect sr = new Kernal32.SmallRect();
            sr.Bottom = 25;
            sr.Top = 0;
            sr.Left = 0;
            sr.Right = 80;
            int x = 0;
            int y = 0;
            int xx = 80;
            int yy = 25;

            screenBuffer = new Kernal32.CharInfo[screenBuffer.Length];

            foreach (Component component in currentScreen.componentList) {
                if (component != null) {
                    component.render();
                }
            }

            Kernal32.WriteConsoleOutput(consoleHandle, screenBuffer, new Kernal32.Coord((short)xx, (short)yy), new Kernal32.Coord((short)x, (short)y), ref sr);
        }

        public static void newScreen(string name){
            screens.Add(name, new Screen());
        }

        public static Screen findScreen(string name) {
            return screens[name];
        }


        public static void setScreen(string name){
            currentScreen = findScreen(name);
        }

        public static void destroyScreen(string name) {
            screens[name] = null;
        }

        public static void addComponent(string screenName, Component newComponent){
            findScreen(screenName).addComponent(newComponent);
        }

        public static Component findFromPosition(int x, int y) {
            foreach (Component c in currentScreen.componentList) {
                if (c != null) {
                    if (x > c.left && x < c.right && y > c.top - 1 && y < c.bottom + 1) {
                        return c;
                    }
                }
            }

            return null;
        }

        public static cood getConsolePosition(int x, int y) {
            cood position = new cood();
            User32.GetWindowRect(handle, ref windowRect);
            position.x = (x - windowRect.left) / (( windowRect.right - windowRect.left - 30) / 80) + 1; 
            position.y = (y - windowRect.top) / ((windowRect.bottom - windowRect.top - 43) / 25) - 3;
            //position / (width - offset) / console units +/- console offset

            return position;
        }

        public static void mouseButtonPressed(int x, int y, char keyPressed) {
            cood mousePos = getConsolePosition(x, y);

            Component comp = findFromPosition(mousePos.x, mousePos.y);
            if (comp != null && comp != componentFocused) {
                //componentFocused.lostFocus(); 
                componentFocused = comp;
                componentFocused.gotFocus(mousePos.x, mousePos.y, keyPressed);
                componentFocused.onMouseClick(keyPressed);
            }
            else if(comp == componentFocused){
                componentFocused.onMouseClick(keyPressed);
            }
        }

        //Needs to be fixed :/

        public static void keyboardButtonPressed(Keys keys) {
            if(componentFocused.type == "TextBox"){
                if (keys.ToString() != "Back" && keys.ToString() != "LShiftKey") {
                    componentFocused.setText(componentFocused.getText() + keys.ToString().ToLower());
                }
                else if (keys.ToString() != "Back" && lastKey.ToString() == "LShiftKey") {
                    componentFocused.setText(componentFocused.getText() + keys.ToString());
                }
                else if (keys.ToString() == "Back" ) {
                    componentFocused.setText(componentFocused.getText().Substring(0, componentFocused.getText().Length - 1));
                }
            }
            lastKey = keys;
        }

        public struct cood{
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BorderPreset
        {
            public Kernal32.CharInfo horizontal;
            public Kernal32.CharInfo vertical;
            public Kernal32.CharInfo topLeft;
            public Kernal32.CharInfo topRight;
            public Kernal32.CharInfo bottomLeft;
            public Kernal32.CharInfo bottomRight;
        }

        public static void test(){
        }

    }
}

