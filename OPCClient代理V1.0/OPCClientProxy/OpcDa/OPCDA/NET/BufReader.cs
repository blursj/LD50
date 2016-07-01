namespace OPCDA.NET
{
    using System;
    using System.IO;

    internal class BufReader : TextReader
    {
        private string buf;
        private int pos = 0;

        internal BufReader(string text)
        {
            this.buf = text;
        }

        public override int Read()
        {
            if (this.pos >= this.buf.Length)
            {
                return -1;
            }
            int num = Convert.ToInt32(this.buf[this.pos]);
            this.pos++;
            return num;
        }
    }
}

