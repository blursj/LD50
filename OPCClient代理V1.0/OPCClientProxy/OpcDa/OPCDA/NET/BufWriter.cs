namespace OPCDA.NET
{
    using System;
    using System.IO;
    using System.Text;

    internal class BufWriter : TextWriter
    {
        private string buf = "";

        internal string GetString()
        {
            return this.buf;
        }

        public override void Write(char ch)
        {
            this.buf = this.buf + Convert.ToString(ch);
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return System.Text.Encoding.Unicode;
            }
        }
    }
}

