using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie8_Plarium
{
    // Производный класс от EventArgs
    class MyEventArgs : EventArgs
    {
        public char ch;
    }

    class KeyEvent
    {
        // Создадим событие, используя обобщенный делегат
        public event EventHandler<MyEventArgs> KeyDown;

        public void OnKeyDown(char ch)
        {
            MyEventArgs c = new MyEventArgs();

            if (KeyDown != null)
            {
                c.ch = ch;
                KeyDown(this, c);
            }
        }
    }
}
