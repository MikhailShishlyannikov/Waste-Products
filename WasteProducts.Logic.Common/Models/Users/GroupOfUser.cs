﻿namespace WasteProducts.Logic.Common.Models.Users
{
    /// <summary>
    /// DAL level model of the group of a user.
    /// </summary>
    public class GroupOfUser
    {
        /// <summary>
        /// ID of the group.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ID of the group admin.
        /// </summary>
        public string AdminId { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Information about group
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// True if user can create boards;
        /// false - user can't create boards.
        /// </summary>
        public bool RightToCreateBoards { get; set; }
    }
}
