using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public class CustomFileDialogModel:INPC
    {
        ObservableCollection<EadFile> _FileList = new ObservableCollection<EadFile>();
        public ObservableCollection<EadFile> FileList { get { return _FileList; } set { _FileList = value; OnPropertyChanged("FileList"); } }
        public DialogMode SelectedMode;
        EadFile SelectedFile;
        string SelectedPath = "";
        string _SelectedFileName = "*";
        public string SelectedFileName
        {
            get { return _SelectedFileName; }
            set { _SelectedFileName = value; OnPropertyChanged("SelectedFileName"); }
        }
        string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }
        string _ButtonText;

        public string ButtonText
        {
            get { return _ButtonText; }
            set { _ButtonText = value; OnPropertyChanged("ButtonText"); }
        }
        public void SelectMode(DialogMode Mode)
        {
            SelectedMode = Mode;
            if (Mode == DialogMode.Open)
            {
                Title = "Открыть проект";
                ButtonText = "Открыть";
            }
            else
            {
                Title = "Сохранить проект как";
                ButtonText = "Сохранить";
                SelectedFileName = "Новый проект";
            }
        }
        public void OpenFolder()
        {
            string[] Files = Directory.GetFiles("Save/", "*.ead");
            foreach (string S in Files)
                FileList.Add(new EadFile() { FileName = S.Split('/')[1] });
        }
        public void ButtonClick()
        {
                if (SelectedMode == DialogMode.Save)
                {
                    Save(SelectedFileName);
                }
                else if (SelectedMode == DialogMode.Open)
                {
                    Open(SelectedPath);
                }
        }
        public void Click(string FileName)
        {
            foreach (EadFile F in FileList)
            {
                if (F == SelectedFile && FileName==F.FileName)
                {
                    if (SelectedMode == DialogMode.Save)
                    {
                        Save(SelectedFileName);
                    }
                    else if (SelectedMode == DialogMode.Open)
                    {
                        Open(SelectedPath);
                    }
                }
                F.Background = "#00000000";
                F.Foreground = "#C5C9CD";
                if (F.FileName == FileName)
                {
                    SelectedFile = F;
                    F.Background = "#C5C9CD";
                    F.Foreground = "#656870";
                    SelectedPath = "Save/" + F.FileName;
                    SelectedFileName = F.FileName.Split('.')[0];
                }
            }
        }
        public void Save(string FileName)
        {
            EditorModel.StaticModel.DialogWindow.Close();
            ProjectEncoderModel Saver = new ProjectEncoderModel();
            Saver.Save("Save/"+FileName+".ead");
        }
        public void Open(string FileName)
        {
            EditorModel.StaticModel.DialogWindow.Close();
            ProjectEncoderModel Saver = new ProjectEncoderModel();
            Saver.DeserializeObject(SelectedPath);
            EditorModel.StaticModel.ChooseLayer(EditorModel.StaticModel.LayerList[0]);
        }
    }
    public class EadFile:INPC
    {
        public bool IsSelected = false;
        string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; OnPropertyChanged("FileName"); }
        }
        string _Background;

        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged("Background"); }
        }
        string _Foreground = "#C5C9CD";

        public string Foreground
        {
            get { return _Foreground; }
            set { _Foreground = value; OnPropertyChanged("Foreground"); }
        }
    }
    public enum DialogMode
    {
        Open, Save
    }
}
