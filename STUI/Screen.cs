using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpTUI
{
    public class Screen
    {
        public List<Component> componentList = new List<Component>();

        public Screen(){
        }

        public void addComponent(Component newComponent){
            componentList.Add(newComponent);
        }

    }
}
