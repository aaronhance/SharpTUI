using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTUI.dlls;

namespace SharpTUI {
   public class CButton : TextComponent
    {

        Kernal32.CharInfo buttonPreset = new Kernal32.CharInfo();
        Kernal32.CharInfo buttonPressedPreset = new Kernal32.CharInfo(); 

        public CButton(string text, int width, int positionX, int positionY, Kernal32.CharInfo buttonPreset, Kernal32.CharInfo buttonPressedPreset) {
            this.parent = parent;
            this.left = positionX;
            this.right = positionX + width + 4;
            this.top = positionY;
            this.bottom = positionY;
            this.buttonPreset = buttonPreset;
            this.buttonPressedPreset = buttonPressedPreset;
            this.text = text;
            this.attributes = attributes;
        }

        public void buttonDraw(){
            drawHorizontalLine(left - 1, top, right - 2, buttonPreset);
            drawString(left, top, text, buttonPreset.Attributes);
        }

        public void buttonDrawPressed(){
            drawHorizontalLine(left - 1, top, right -2, buttonPressedPreset);
            drawString(left, top, text, buttonPressedPreset.Attributes);
        }

        public override void render(){
            if (hasLastClick) { buttonDrawPressed(); hasLastClick = false; }
            else buttonDraw();


 	        base.render();
        }

    }
}
