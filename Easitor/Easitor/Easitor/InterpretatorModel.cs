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
                string ErrorMessage = "";
                if (IsValide(Step, ref ErrorMessage))
                {
                    string[] Words = Step.Split(' ');
                    //
                }
            }
        }

        public void CheckSyntax(string Script)
        {
            Script = Regex.Replace(Script, @"\s+", " ");
            ((InterpretatorViewModel)this).ErrorView = "";
            string[] Steps = Script.Split(';');
            string ErrorMessage = "";
            foreach (string Step in Steps)
            {
                if (IsValide(Step, ref ErrorMessage))
                {
                    string[] Words = Step.Split(' ');
                    ((InterpretatorViewModel)this).ErrorView += ErrorMessage;
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
        bool IsValide(string Step, ref string ErrorMessage) 
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
                    ErrorMessage = "Ошибка в команде: " + Step + " (Не существует функции " + Words[0] + ")\n";
                }
            }
            else
            {
                Valide = false;
                ErrorMessage = "";
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
