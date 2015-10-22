using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTUI;

namespace SharpTUI {
    public class CLabel : TextComponent{

        //Where's my border?

        public CLabel(String text, int positionX, int positionY, short attributes) {
            this.parent = parent;
            this.left = positionX;
            this.right = positionX + width;
            this.top = positionY;
            this.bottom = positionY + 2;
            this.borderPreset = borderPreset;
            this.text = text;
            this.attributes = attributes;
        }

        public override void render() {
            drawString(left + 1, top + 1, text, attributes);
            border();
            base.render();
        }


    }
}
