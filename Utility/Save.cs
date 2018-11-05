using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class Save
{

    public Save CreateNewSave(string name)
    {
        Save save = new Save();
        var folder = Directory.CreateDirectory(Application.persistentDataPath + name);
        return save;
    }



    public void SaveGame(string name)
    {
        Save save = CreateNewSave(name);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }
}
