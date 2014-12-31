using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    // Является Widget-классом паттерна "Состояние"
    public class Select : AbstractFunction, IFunction, IState
    {
        IState State;
        public void Init(){
            FunctionName = "select";
        }
        public override void Overload(){
            if (Args.Count() == 1 && Args[0].Split('-').Count() == 2) // Если функция имеет вид   select a-z;
                State = new IntervalState(this);
            else                                                      // Если функция имеет вид   select a b c d;
                State = new EnumedState(this);
        }
        public override void DealWithModel()
        {
            State.DealWithModel();
        }
    } 

    public class EnumedState : IState
    {
        Select Selection;
        public EnumedState(Select Selection)
        {
            this.Selection = Selection;
        }
        public void DealWithModel()
        {
            InterpretatorModel.SelectedLayers.Clear();
            foreach (string arg in Selection.Args)
            {
                InterpretatorModel.SelectedLayers.Add(Selection.Model.RevercedLayerList[Convert.ToInt16(arg)]);
            }
        }
    }
    public class IntervalState : IState
    {
        Select Selection;
        public IntervalState(Select Selection)
        {
            this.Selection = Selection;
        }
        public void DealWithModel()
        {
            InterpretatorModel.SelectedLayers.Clear();
            for (int i = Convert.ToInt16(Selection.Args[0].Split('-')[0]); i <= Convert.ToInt16(Selection.Args[0].Split('-')[1]); i++)
            {
                InterpretatorModel.SelectedLayers.Add(Selection.Model.RevercedLayerList[i]);
            }
        }
    } 
}
