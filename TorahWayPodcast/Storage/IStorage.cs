namespace TorahWayPodcast.Storage
{
    interface IStorage<T>
    {
        T Read();
        void Write(T shiurim);
    }
}
