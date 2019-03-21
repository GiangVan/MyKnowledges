using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace YourExperience
{
    class Tag_of_Node
    {
        internal string date = "???";
        internal bool is_lockNode = false;
        internal bool unlocked = false;
        internal float zoomFactor = 1f;
        internal int SelecionStart = -1, SelectionLength = 0;
        internal string nodes = null;
        internal byte[] password = null;

        public Tag_of_Node(int SelecionStart, int SelectionLength, float zoomFactor)
        {
            this.SelecionStart = SelecionStart;
            this.SelectionLength = SelectionLength;
            this.zoomFactor = zoomFactor;
        }
        public Tag_of_Node(string date)
        {
            this.date = date;
        }
        public Tag_of_Node() { }

        public void Clear()
        {
            password = null;
            nodes = null;
            is_lockNode = false;
            unlocked = false;
        }
    }
}
