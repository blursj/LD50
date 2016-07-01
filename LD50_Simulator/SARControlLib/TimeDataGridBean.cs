using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SARControlLib
{
    public class TimeDataGridBean
    {
        public string Header0;
        public string Head0
        {
            get { return Header0; }
            set { Header0 = value; }
        }
        public string Header1;
        public string Head1
        {
            get { return Header1; }
            set { Header1 = value; }
        }

        public TimeDataGridBean(string s0, string s1)
        {
            Header0 = s0;
            Header1 = s1;
        }
    }
}
