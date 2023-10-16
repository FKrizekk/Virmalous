using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveGame(GameManager game)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/gameData.sus";
		FileStream stream = new FileStream(path, FileMode.Create);
		
		GameData data = new GameData(game);
		
		formatter.Serialize(stream, data);
		stream.Close();
	}
	
	public static GameData LoadGame()
	{
		string path = Application.persistentDataPath + "/gameData.sus";
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			
			GameData data = formatter.Deserialize(stream) as GameData;
			stream.Close();
			
			return data;
		}else
		{
			Debug.LogWarning("Save file not found in " + path + ", loading default values.");
			return null;
		}
	}
}
