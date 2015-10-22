using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTUI.dlls;

namespace SharpTUI {
    public class CLoadingBar : Component
    {

        private int width;
        private int widthLoaded; 

        Kernal32.CharInfo loadingPreset;

        public CLoadingBar(int width, int positionX, int positionY, Kernal32.CharInfo loadingPreset) {
            this.parent = parent;
            this.left = positionX;
            this.right = positionX + width;
            this.top = positionY;
            this.bottom = positionY;
            this.width = width;
            this.loadingPreset = loadingPreset;
        }

        public void setLoaded(int loaded) {
            if (loaded <= width) widthLoaded = loaded;
            else widthLoaded = width;
        }

        public void drawLoadingBar() {
            drawHorizontalLine(left, top, right + widthLoaded - width, loadingPreset);
        }

        public override void render() {
            drawLoadingBar();
            base.render();
        }

    }
}
