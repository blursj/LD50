namespace OPCDA.NET
{
    using System;

    public class DataBindFormat
    {
        public double add;
        public string format;
        public double multiply;
        public Type reqType;
        public bool showNonGoodQuality;
        public bool showNoValueOnBadQuality;

        public DataBindFormat()
        {
            this.format = null;
            this.reqType = typeof(void);
            this.add = 0.0;
            this.multiply = 1.0;
        }

        public DataBindFormat(string fmt)
        {
            this.format = fmt;
            this.reqType = typeof(void);
            this.add = 0.0;
            this.multiply = 1.0;
        }
    }
}

