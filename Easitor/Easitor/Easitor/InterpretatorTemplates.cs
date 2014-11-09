using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    interface IFunction 
    {
        void Init();
    }
    interface IState
    {
        void DealWithModel();
    }

    abstract public class AbstractFunction
    {
        public string FunctionName;
        public EditorModel Model;
        public string[] Args;
        public void AccessModel() 
        { 
            Model = EditorModel.StaticModel;
        }
        public void SendArgs(string[] _a)
        {
            Args = _a;
        }
        public abstract void Overload();
        public abstract void DealWithModel();
        public void Reset() { }
    }

    //public class Move : AbstractFunction, IFunction
    //{
    //    public void Init()
    //    {
    //        FunctionName = "move";
    //    }
    //}

    //public class Blur : AbstractFunction, IFunction
    //{
    //    public void Init()
    //    {
    //        FunctionName = "blur";
    //    }
    //}
}
