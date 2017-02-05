namespace YouTown
{
    public interface IIdentifier
    {
        int NewId();
    }

    public class Identifier : IIdentifier
    {
        public const int DontCare = -1;
        private int _id = -1;
        public int NewId()
        {
            _id++;
            return _id;
        }
    }

}