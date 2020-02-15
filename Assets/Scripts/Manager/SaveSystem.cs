using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem {
	private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
	private static readonly string EXTENSION = ".dat";
	public static void SaveData(object t) { // save data as given object
		BinaryFormatter bf = new BinaryFormatter(); // needed for serialize/deserialize
		using (FileStream file = File.Create(SAVE_FOLDER + t.GetType().ToString() + EXTENSION))  //create the file with the name based on type (ex. SaveData creates SaveData.dat)
			bf.Serialize(file, JsonUtility.ToJson(t));//Change object to JSON serialize the object and put it in the file we just created
	}
	public static T LoadData<T>() { // Returns object from save file based on type
		Validate<T>();
		BinaryFormatter bf = new BinaryFormatter(); // needed for serialize/deserialize
		using (FileStream file = File.Open(SAVE_FOLDER + typeof(T).ToString() + EXTENSION, FileMode.Open)) { // load found file from the path into the file stream
			T t = JsonUtility.FromJson<T>((string)bf.Deserialize(file));//Take the object (which is JSON) as string and cast whatever you found in the file as the Type given
			return t;
		}
	}
	static void Validate<T>() {
		if (!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER); //check if save folder exists
		if (!File.Exists(SAVE_FOLDER + typeof(T).ToString() + EXTENSION))  // if file doesn't exists
			GetNew<T>();//errase is new vars and save over file or create file
	}
	static void GetNew<T>() {// overwrite the file based on object
		T t = (T)Activator.CreateInstance(typeof(T));// create new instance of object based on Type
		SaveData(t); // save new instacnce of object
	}
}
