using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace EpubSpellChecker
{
    class BufferedListBox : ListBox
    {
        public BufferedListBox()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
        }


    }
}
