using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpTUI
{
    public class Screen
    {
        public Component[] components = new Component[100];
        public string name;

        public Screen(string name)
        {
            this.name = name;
        }

        public void addComponent(Component newComponent)
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    components[i] = newComponent;
                    break;
                }
            }
        }

    }
}
