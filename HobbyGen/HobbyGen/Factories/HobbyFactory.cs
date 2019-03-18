namespace HobbyGen.Factories
{
    using System;
    using HobbyGen.Models;

    /// <summary>
    /// Factory to find and get hobbies
    /// </summary>
    public static class HobbyFactory
    {
        /// <summary>
        /// Finds a hobby by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static Hobby FindHobby(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a hobby by its id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static Hobby GetHobby(uint id)
        {
            throw new NotImplementedException();
        }

        static bool HobbyExists(uint id)
        {
            throw new NotImplementedException();
        }

        static bool HobbyExists(string name)
        {
            throw new NotImplementedException();
        }
    }
}
