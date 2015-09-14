using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using SharpTUI.dlls;

namespace SharpTUI
{
    public static class ScreenDriver
    {
        private static Screen[] screens;
        private static int currentScreens, maxScreens = 0;
        private static Screen currentScreen = null;
        private static Component componentFocused;

        private static User32.RECT windowRect;
        private static IntPtr handle;

        static SafeFileHandle consoleHandle;
        public static Kernal32.CharInfo[] screenBuffer = new Kernal32.CharInfo[80*25];
        static Kernal32.CharInfo ci;
        public static Kernal32.CONSOLE_SCREEN_BUFFER_INFO consoleInfo = new Kernal32.CONSOLE_SCREEN_BUFFER_INFO();

        public delegate void delButtonPress(int x, int y, char keyPressed);

        public static delButtonPress onMouseButtonPressed;

        public static void init(int maxScreensIn){
            //callBacks
            onMouseButtonPressed = mouseButtonPressed;
            //other shit
            maxScreens = maxScreensIn;
            screens = new Screen[maxScreens];        
            consoleHandle = Kernal32.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

           // Kernal32.GetConsoleScreenBufferInfo(consoleHandle.DangerousGetHandle(), out consoleInfo);

            handle = Process.GetCurrentProcess().MainWindowHandle;
            User32.GetWindowRect(handle, ref windowRect);
         }


        public static void render() { 
            //To be improved - add render order(priority value?)!
            foreach (Component component in currentScreen.components) {
                component.render();
                //get small array for each component and it's origin and compile into a buffer!
            }

            Kernal32.SmallRect sr = new Kernal32.SmallRect();
            sr.Bottom = 25;
            sr.Top = 0;
            sr.Left = 0;
            sr.Right = 80;
            int x = 0;
            int y = 0;
            int xx = 80;
            int yy = 25;
            Kernal32.WriteConsoleOutput(consoleHandle, screenBuffer, new Kernal32.Coord((short)xx, (short)yy), new Kernal32.Coord((short)x, (short)y), ref sr);
        }

        public static bool newScreen(string name){
            if (currentScreens < maxScreens) {
                foreach (Screen screen in screens) {
                    if (screen == null) {
                        screens[currentScreens] = new Screen(name);

                        return true;
                    }
                }
                screens[currentScreens] = new Screen(name);
                currentScreens++;
                return true;
            } else return false;
        }

        public static Screen findScreen(string name) {
            Screen scrn = null;
            foreach(Screen screen in screens){
                if (screen != null) { 
                   if (screen.name == name) {
                       return screen;
                       Console.WriteLine("FINDING" + screen.name + "  " + name);
                    }
                }
            }
            return scrn; 
        }

        public static int findScreenPosition(string name){
            int i;
            for(i = 0; i <= maxScreens; i++){
                if(screens[i].name == name){
                    return i;
                }
            }
            return -1;
        }

        public static void setScreen(string name){
            currentScreen = findScreen(name);
        }

        public static void destroyScreen(string name) {
            int position = findScreenPosition(name);
            if (position != -1) {
                screens[position] = null;
            }
            //Where's the dam garbage truck?
        }

        public static void addComponent(string screenName, Component newComponent){
            findScreen(screenName).addComponent(newComponent);
        }

        public static Component findFromPosition(int x, int y) {
            for (int i = currentScreen.components.Length; i >= 0; i--){
                if (x > currentScreen.components[i].left && x < currentScreen.components[i].right && y > currentScreen.components[i].top && y < currentScreen.components[i].bottom){
                    return currentScreen.components[i];
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

            //Window rect needs re-updating ever time this is called!
        }

        public static void mouseButtonPressed(int x, int y, char keyPressed) {
            //Console.WriteLine("Callback to mouse Button pressed. Data:" + x.ToString() + y.ToString() + keyPressed);
            cood mousePos = getConsolePosition(x, y);
            //Console.WriteLine(((x - windowRect.left) / ((windowRect.right - windowRect.left - 30) / 80) - 1).ToString() + ", " + ((y - windowRect.top) / ((windowRect.bottom - windowRect.top - 43) / 25) - 3).ToString());

            Component comp = findFromPosition(x, y);
            if (comp != null) {
                componentFocused.lostFocus();
                componentFocused = comp;
                componentFocused.gotFocus(mousePos.x, mousePos.y, keyPressed);
            }
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

    }
}

