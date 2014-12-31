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
    /// <summary>
    /// Функция интерпретатора
    /// </summary>
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

    public class Move : AbstractFunction, IFunction, IState
    {
        public void Init()
        {
            FunctionName = "move";
        }
        double XMove;
        double YMove;
        public override void Overload()
        {
            if (Args.Count(n => n != null) == 1)
            {
                XMove = Convert.ToInt16(Args[0]);
            }
            else
            {
                XMove = Convert.ToInt16(Args[0]);
                YMove = Convert.ToInt16(Args[1]);
            }
        }
        public override void DealWithModel()
        {
            foreach (Layer L in InterpretatorModel.SelectedLayers)
            {
                L.X += XMove;
                L.Y += YMove;
            }
        }
    }

    public class Visibilty : AbstractFunction, IFunction, IState
    {
        public void Init()
        {
            FunctionName = "visible";
        }
        public override void Overload(){}
        public override void DealWithModel()
        {
            if (Args[0]=="1")
            foreach (Layer L in InterpretatorModel.SelectedLayers)
            {
                L.Unhide();
            }
            else if (Args[0] == "0")
            foreach (Layer L in InterpretatorModel.SelectedLayers)
            {
                L.Hide();
            }
        }
    }

    public class Blur : AbstractFunction, IFunction, IState
    {
        public void Init()
        {
            FunctionName = "blur";
        }
        public override void Overload() { }
        public override void DealWithModel()
        {
            foreach (Layer L in InterpretatorModel.SelectedLayers)
            {
                L.BlurRadius = Convert.ToInt16(Args[0]);
            }
        }
    }
}
