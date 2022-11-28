using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static readonly string path = Path.Combine(Application.persistentDataPath, "schism.dat");
    
    public static void SavePlayer(Player player)
{
    FileStream stream = new FileStream(path, FileMode.Create);
    BinaryWriter writer = new BinaryWriter(stream);
    
    PlayerData data = PlayerData.FromPlayer(player);
    
    writer.Write(data.Position[0]);
    writer.Write(data.Position[1]);
 
    stream.Close();
}
    public static PlayerData LoadPlayer()
{
    if (File.Exists(path))
    {
        FileStream stream = new FileStream(path, FileMode.Open);
        BinaryReader reader = new BinaryReader(stream);
            
        PlayerData data = new PlayerData();
 
        data.Position = new float[2];
        data.Position[0] = reader.ReadSingle();
        data.Position[1] = reader.ReadSingle();

        stream.Close();
 
        return data;
    }
    else
    {
        Debug.LogError("Save file not found in " + path);
        return null;
    }
}
}