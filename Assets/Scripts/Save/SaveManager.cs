using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    public SaveState SaveState;
  


    public Action<SaveState> OnLoad; 
    public Action<SaveState> OnSave;

    private BinaryFormatter _formatter;
    private const string SAVE_FILE_NAME = "data.ss";

    private void Awake()
    {
        instance = this;
        _formatter = new BinaryFormatter();

        Load();
    }

    public void Load()
    {
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + SAVE_FILE_NAME, FileMode.Open, FileAccess.Read);
            SaveState = (SaveState)_formatter.Deserialize(file);
            file.Close();
            OnLoad?.Invoke(SaveState);
        }
        catch
        {
            Debug.Log("Save file not found! Creating new file");
            Save();
        }
    }

    public void Save()
    {
        // If theres no previous state found, create a new one!
        if (SaveState == null)
            SaveState = new SaveState();

        // Set the time at which we've tried saving
        SaveState.LastSaveTime = DateTime.Now;

        // Open a file on our system, and write to it
        FileStream file = new FileStream(Application.persistentDataPath + SAVE_FILE_NAME, FileMode.OpenOrCreate, FileAccess.Write);
        _formatter.Serialize(file, SaveState);
        file.Close();

        OnSave?.Invoke(SaveState);
    }
}
