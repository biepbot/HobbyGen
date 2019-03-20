namespace HobbyGen.Controllers.Managers
{
    using HobbyGen.Controllers.Extensions;
    using HobbyGen.Persistance;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Text;

    /// <summary>
    /// Mocking class for resources
    /// </summary>
    public static class Mocker
    {
        private static string[] FNamePart1 = { "Hen", "Bob", "Aus", "Sam", "Gen", "Gel", "Sa", "Jan", "Rik", "Anne", "Ben", "Bar", "Dirk", "El", "Jaap", "Kar" };
        private static string[] FNamePart2 = { "drik", "sen", "sem", "ka", "ta", "ki", "end", "bella", "bel", "el" };

        private static string[] SName = { "Slager", "Bobby", "Jansen", "Janssen", "van Goes", "van Weert" };

        /// <summary>
        /// Fills the database with hobbies and users
        /// </summary>
        /// <param name="useramount">the amount of users to generate</param>
        /// <param name="minhobbies">the minimum hobbies a user has</param>
        /// <param name="maxhobbies">the maximium hobbies a user has</param>
        public static void FillDatabase(int useramount, int minhobbies, int maxhobbies, UserManager uManager)
        {
            // Setup random bool
            Random r = new Random();

            while (useramount-- > 0)
            {
                var myname = string.Empty;

                // Select a first name
                myname += FNamePart1.Random();

                // maybe select a second part
                if (r.NextDouble() >= 0.10)
                {
                    myname += FNamePart2.Random();
                }

                // maybe slap on a last name
                if (r.NextDouble() >= 0.75)
                {
                    myname += " " + SName.Random();
                }

                // Create his hobbies
                var hobbiesamnt = r.Next(minhobbies, maxhobbies + 1);

                string[] hobbies = new string[hobbiesamnt];

                // GENERATE ALL THE HOBBIES
                for (int i = 0; i < hobbiesamnt; i++)
                {
                    hobbies[i] = GenerateHobby(3, r);
                }

                // Create the users
                uManager.CreateUser(myname, hobbies, false);
            }

            uManager.SaveChanges();
        }

        /// <summary>
        /// Generates a random hobby
        /// </summary>
        /// <param name="length">the length of the hobby</param>
        /// <param name="random">the randomiser</param>
        /// <returns>A hobby!!!</returns>
        private static string GenerateHobby(int length, Random random)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
