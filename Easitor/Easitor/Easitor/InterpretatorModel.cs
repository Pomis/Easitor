using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Easitor
{
    public class InterpretatorModel:INPC
    {
        public bool HasErrors
        {
            get { return (((InterpretatorViewModel)this).ErrorList.Count!=0); }
        }

        string ScriptText;
        List<AbstractFunction> Functions = new List<AbstractFunction>();
        static public List<Layer> SelectedLayers = new List<Layer>();
        public string ErrorMessage { get; set; }

        // Структура команды:
        // Function Arg1 Arg2 Arg3
        public void RunScript(string Script)
        {
            string[] Steps = Script.Split(';');
            foreach (string Step in Steps)
            {
                if (IsValide(Step))
                {
                    string[] Words = Step.Split(' ');
                    //
                }
            }
        }

        public void CheckSyntax(string Script)
        {
            Script = Regex.Replace(Script, @"\s+", " ");
            ScriptText = Script.Trim();
            string[] Steps = Script.Split(';');
            ((InterpretatorViewModel)this).ErrorList.Clear();
            foreach (string Step in Steps)
            {
                if (IsValide(Step))
                {
                    string[] Words = Step.Split(' ');
                }
            }
            
         }

        public void Start()
        {
            ScriptText = ScriptText.Trim();
            if (!HasErrors)
            {
                string[] Steps = ScriptText.Split(';').Where(n=>n.Length>=3).ToArray();
                ((InterpretatorViewModel)this).ErrorList.Clear();
                for (int i = 0; i < Steps.Count(); i++)
                {
                    Steps[i] = Steps[i].Trim();
                    if (IsValide(Steps[i]))
                    {
                        string[] Words = Steps[i].Split(' ');
                        string[] args = Words.Where(n => n!=Words[0]).ToArray();
                        RunFunction(Words[0], args);
                    }
                }
            }
        }

        void RunFunction(string FunctionName, string[] args) 
        {
            foreach (AbstractFunction Func in Functions)
            {
                if (Func.FunctionName == FunctionName)
                {
                    // Собственно, реализация паттерна Шаблонный метод
                    Func.AccessModel();     // Получить доступ к модели графиечского редактора
                    Func.SendArgs(args);    // Передаём агрументы в функцию скриптового языка
                    Func.Overload();        // Выбор реализации при перегрузке
                    Func.DealWithModel();   // Работаем с моделью
                    Func.Reset();           // Очищаем память от лишних значений
                }
            }
        }
        bool IsValide(string Step) 
        {
            bool Valide = true;
            if (Step.Length >3)
            {
                string[] Words = Step.Split(' ');
                Words=Words.Where(n => !string.IsNullOrEmpty(n)).ToArray();
                bool FunctionExists = false;
                foreach (AbstractFunction Function in Functions)
                {
                    if (Function.FunctionName == Words[0])
                        FunctionExists = true;
                }
                if (!FunctionExists)
                {
                    ErrorViewModel Err = new ErrorViewModel();
                    Err.ErrorView= "Ошибка в команде: " + Step + " (Не существует функции " + Words[0] + ")\n";
                    Err.Background = "#f7dcdc";
                    ((InterpretatorViewModel)this).ErrorList.Add(Err);
                }
            }
            else
            {
                Valide = false;
            }
            return Valide;

        }

        public void LoadFunctions()
        {
            var t = typeof(IFunction);
            var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(typ => typ.GetTypes())
                .Where(x => t.IsAssignableFrom(x));
            //на всякий случай избавляемся от интерфейсов
            List<Type> functionTypes = new List<Type>();
            foreach (Type type in assemblyTypes)
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    continue;
                }
                else
                {
                    functionTypes.Add(type);
                }
            }
            foreach (Type type in functionTypes)
            {
                IFunction func = (IFunction)Activator.CreateInstance(type);
                func.Init();
                Functions.Add((AbstractFunction)func);
            }
        }
    }
}
