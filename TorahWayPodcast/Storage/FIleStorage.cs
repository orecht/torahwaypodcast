using System;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using TorahWayPodcast.Models;

namespace TorahWayPodcast.Storage
{
    public class FileStorage : IStorage<Shiurim>
    {
        private string fileName = "Shiurim.dat";

        public Shiurim Read()
        {
            if (File.Exists(fileName))
            {
                // Open the file containing the data that you want to deserialize.
                FileStream fs = new FileStream(fileName, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and 
                    // assign the reference to the local variable.
                    return formatter.Deserialize(fs) as Shiurim;
                }
                catch (SerializationException e)
                {
                    throw new Exception("Failed to deserialize. Reason: " + e.Message);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
                return new Shiurim();
        }

        public void Write(Shiurim shiurim)
        {

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, shiurim);
            }
            catch (SerializationException e)
            {
                throw new Exception(("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
    }
}