# SharpTUI
A Text User Interface for c#.

If you want to start using the framework right now then follow these steps:

1. open Visual studio.
 
2. Create a new console application

2. File -> add -> exsisting project -> add the STUI project(download it if you havn't already)

3. Add a reference to it. Open solution explorer right click references on your console project -> add referance find  SharpTUI under solution.



Example:
'''c#
        ScreenDriver.BorderPreset bp = new ScreenDriver.BorderPreset();
        
    public void method(){

        ScreenDriver.newScreen("my screen");
        ScreenDriver.setScreen("my screen");
        ScreenDriver.addComponent("my screen", new CTextBox(10, 5, 5, bp));
        ScreenDriver.render(); //Should be called from a loop running about 20 - 50 times a second
        
        //Define the border preset
        public void borderPreset() {
            bp.horizontal.Char.UnicodeChar = Convert.ToChar(196);
            bp.horizontal.Attributes = 1;
            bp.vertical.Char.UnicodeChar = Convert.ToChar(179);
            bp.vertical.Attributes = 1;
            bp.topLeft.Char.UnicodeChar = Convert.ToChar(218);
            bp.topLeft.Attributes = 1;
            bp.topRight.Char.UnicodeChar = Convert.ToChar(191);
            bp.topRight.Attributes = 1;
            bp.bottomLeft.Char.UnicodeChar = Convert.ToChar(192);
            bp.bottomLeft.Attributes = 1;
            bp.bottomRight.Char.UnicodeChar = Convert.ToChar(217);
            bp.bottomRight.Attributes = 1;
        }
    }
''''
