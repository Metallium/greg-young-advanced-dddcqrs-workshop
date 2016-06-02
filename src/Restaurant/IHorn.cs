namespace Restaurant
{
    public interface IHorn
    {
        void Whisper(string message);
        void Say(string message);
        void Note(string message);
    }
}
