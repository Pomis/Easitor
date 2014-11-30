using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Easitor
{
    [Serializable]
    class FileOperationsModel :ISerializable
    {
        #region Сериализация
        public FileOperationsModel()
        {

        }
        public FileOperationsModel(SerializationInfo sInfo, StreamingContext contextArg)
        {
            EditorModel.StaticModel.NewDocument();
            EditorModel.StaticModel.LayerList = (ObservableCollection<Layer>)sInfo.GetValue("LayerList", typeof(ObservableCollection<Layer>));
            
        }
        public void GetObjectData(SerializationInfo sInfo, StreamingContext contextArg)
        {
            sInfo.AddValue("LayerList", EditorModel.StaticModel.LayerList);
        }
        public FileOperationsModel DeserializeObject(string fileName)
        {
            FileOperationsModel objToSerialize = null;
            System.IO.FileStream fstream = System.IO.File.Open(fileName, System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            objToSerialize = (FileOperationsModel)binaryFormatter.Deserialize(fstream);
            // Тут происходит некоторое чудо со ссылкой на объект. Напрямую загруженное из потока не хочет отображаться в датаконтексте
            // и приходится обновлять ссылку на коллекцию. Бред немного, но зато работает
            EditorModel.StaticModel.RevercedLayerList = new ObservableCollection<Layer>(EditorModel.StaticModel.LayerList.Reverse().ToList());
            EditorModel.StaticModel.LayerList = new ObservableCollection<Layer>(EditorModel.StaticModel.RevercedLayerList.Reverse().ToList());
            fstream.Close();
            return objToSerialize;
        }

        public void Save(string fileName)
        {
            System.IO.FileStream fstream = System.IO.File.Open(fileName, System.IO.FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fstream, this);
        }
        #endregion
    }
}
