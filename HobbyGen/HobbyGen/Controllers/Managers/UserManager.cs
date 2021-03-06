﻿namespace HobbyGen.Controllers.Managers
{
    using HobbyGen.Models;
    using HobbyGen.Persistance;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages specific user calls
    /// </summary>
    public class UserManager
    {
        private GeneralContext context;
        private HobbyManager hManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class
        /// </summary>
        /// <param name="context"></param>
        public UserManager(GeneralContext context)
        {
            this.context = context;
            this.hManager = new HobbyManager(context, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hManager">The hobby manager to user</param>
        public UserManager(GeneralContext context, HobbyManager hManager)
        {
            this.context = context;
            this.hManager = hManager;
        }

        /// <summary>
        /// Retrieves a list of users based on the name
        /// </summary>
        /// <param name="name">The name of the user to find</param>
        /// <returns>A list of users</returns>
        public IEnumerable<User> SearchByName(string name)
        {
            var all = this.context.UserItems;

            // Get results where name is similar (contains or contains)
            var users = all.Where(

                u => u.Name.ToLower().Contains(name.ToLower())
                || name.ToLower().Contains(u.Name.ToLower()));

            // Order list (alt. Comparer to name)
            var ulist = users.OrderBy(u => u.Name);

            return ulist;
        }

        /// <summary>
        /// Retrieves a list of users based on the hobby
        /// </summary>
        /// <param name="hobbies">The hobby to search for</param>
        /// <returns>A list of users</returns>
        public IEnumerable<User> SearchByHobby(IEnumerable<string> hobbies)
        {
            if (hobbies == null)
            {
                return new User[0];
            }

            var all = this.context.UserItems;

            // order by matches (max offset of 1)
            // Get results where name is similar (contains or contains)
            var users = all.Where(delegate (User u)
            {
                var matches = hobbies.Intersect(u.Hobbies).Count();
                return matches >= hobbies.Count() - 1 && matches > 0;
            });

            // Order list by match amount
            var ulist = users.OrderByDescending(u => hobbies.Intersect(u.Hobbies).Count());

            return ulist;
        }

        /// <summary>
        /// Saves all changes made
        /// </summary>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>A list with all the users</returns>
        public IEnumerable<User> GetAll()
        {
            return this.context.UserItems;
        }

        /// <summary>
        /// Gets a user by its id
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>A user or null</returns>
        public User GetById(uint id)
        {
            var u2 = this.context.UserItems
                .FirstOrDefault(u => u.Id == id);
            return u2;
        }

        /// <summary>
        /// Creates a user with the data given
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="hobbies">The hobbies of the user</param>
        /// <returns>The user created</returns>
        public User CreateUser(string name, IEnumerable<string> hobbies)
        {
            return this.CreateUser(name, hobbies, true);
        }

        /// <summary>
        /// See <see cref="CreateUser(string, IEnumerable{string})"/>. Allows to prevent saving (for mass insertion)
        /// </summary>
        /// <param name="name">THe name of the user</param>
        /// <param name="hobbies">The user's hobbies</param>
        /// <param name="save">Whether it saves its changes immediately after</param>
        public User CreateUser(string name, IEnumerable<string> hobbies, bool save)
        {
            // Create the user
            User user = new User(name);

            if (hobbies != null)
            {
                // Duplicates not allowed
                hobbies = hobbies.Distinct();

                foreach (string hobby in hobbies)
                {
                    var h = this.hManager.FindOrCreateHobby(hobby, save);
                    user.Hobbies.Add(hobby);
                }
            }

            this.context.UserItems.Add(user);

            if (save)
            {
                this.context.SaveChanges();
            }

            return user;
        }

        /// <summary>
        /// Creates a user from the user object
        /// </summary>
        /// <param name="user">The user to create</param>
        /// <returns>The accepted user</returns>
        public User CreateUser(User user)
        {
            return this.CreateUser(user.Name, user.Hobbies);
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="hobbiesAdded">The hobbies added</param>
        /// <param name="hobbiesRemoved">The hobbies removed</param>
        public User UpdateUser(uint id, IEnumerable<string> hobbiesAdded, IEnumerable<string> hobbiesRemoved)
        {
            var user = this.GetById(id);

            // Add hobbies
            // Duplicates not allowed
            hobbiesAdded = hobbiesAdded.Distinct();
            hobbiesRemoved = hobbiesRemoved.Distinct();

            foreach (string hobby in hobbiesAdded)
            {
                if (!string.IsNullOrWhiteSpace(hobby))
                {
                    var h = this.hManager.FindOrCreateHobby(hobby);
                    if (!user.Hobbies.Contains(h))
                    {
                        user.Hobbies.Add(h);
                    }
                }
            }

            // Remove hobbies
            foreach (string hobby in hobbiesRemoved)
            {
                user.Hobbies.Remove(this.hManager.FindOrCreateHobby(hobby));
            }

            // TODO: use batches
            //var batches = CardRepository.splitList(xx, 100);
            //batches.ForEach(x =>
            //{
            //    context.xx.AddRange(x);
            //    context.SaveChanges();
            //});

            this.context.Update(user);
            this.context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Deletes a user by its id
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        public void DeleteUser(uint id)
        {
            var user = this.GetById(id);
            this.context.UserItems.Remove(user);
            this.context.SaveChanges();
        }
    }
}
