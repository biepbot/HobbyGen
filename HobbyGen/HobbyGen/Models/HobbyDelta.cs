namespace HobbyGen.Models
{
    /// <summary>
    /// Class to monitor deltas of hobbies added and removed
    /// </summary>
    public class HobbyDelta
    {
        public string[] HobbiesAdded { get; set; }
        public string[] HobbiesRemoved { get; set; }

        public HobbyDelta()
        {
        }
    }
}
