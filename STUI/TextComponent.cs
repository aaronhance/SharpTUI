using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpTUI {
    public class TextComponent : Component{

        protected ScreenDriver.BorderPreset borderPreset;

        protected string text = "";
        protected short attributes = 01;

        public override void setText(string text) {
            if (text.Length <= width - 2) {
                this.text = text;
            }
            else throw new ArgumentException("(string text): \"" + text + "\" length is to long for the textbox! Max string length: " + (width - 2));
        }

        public override string getText() {
            return text;
        }

        public void border() {

            drawHorizontalLine(left, top, right, borderPreset.horizontal);
            drawHorizontalLine(left, bottom, right, borderPreset.horizontal);
            drawVerticalLine(left, top, bottom, borderPreset.vertical);
            drawVerticalLine(right, top, bottom, borderPreset.vertical);

            drawHorizontalLine(left, top, left + 1, borderPreset.topLeft);
            drawHorizontalLine(right, top, right + 1, borderPreset.topRight);
            drawHorizontalLine(left, bottom, left + 1, borderPreset.bottomLeft);
            drawHorizontalLine(right, bottom, right + 1, borderPreset.bottomRight);

        }

    }
}
