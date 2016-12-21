namespace TorahWayPodcast.Storage
{
    interface IStorage<T>
    {
        /// <summary>
        /// Read the storage.
        /// </summary>
        /// <returns>Content of cache formatted as T. new emty instance of T if nothing in storage</returns>
        T Read();

        /// <summary>
        /// Write to storage
        /// </summary>
        /// <param name="shiurim"></param>
        void Write(T shiurim);
    }
}
