using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public class PassObj
    {
        internal bool isEnd;

        public string ObjType { get; set; }
        public string msg { get; set; }
        public bool showContinue { get; set; }
        public bool showIsError { get; set; }
        public string ObjID { get; set; }
        public string styleStr { get; set; }
    }
    public class PassCanvas : PassObj
    {
        public string CanvasID { get; set; }
    }

    public class InputAddress : PassObj
    {
    }

    public class Close : PassObj
    {
    }
    public class MorseCodeModel : PassObj
    {
        public List<int> codeShowT { get; set; }

        public List<MorseCodeT> paramters { get; set; }
    }
    public class MorseCodeT
    {
        public int mType { get; set; }
        public int startT { get; set; }
        public int endT { get; set; }

        public static MorseCodeT Di(ref int start)
        {
            var result = new MorseCodeT()
            {
                startT = start,
                endT = start + 1,
                mType = 1
            };
            start += 2;
            return result;
        }
        public static MorseCodeT Da(ref int start)
        {
            var result = new MorseCodeT()
            {
                startT = start,
                endT = start + 3,
                mType = 0
            };
            start += 4;
            return result;
        }
    }

    public class ChangeObj
    {
        public string ObjType { get; set; }
        public string[] ids { get; set; }
        public string[] msgs { get; set; }
    }
}
