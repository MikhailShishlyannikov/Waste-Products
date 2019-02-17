﻿using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Users;

namespace WasteProducts.DataAccess.Common.Models.Groups
{
    public class GroupBoardDB
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Board name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// This board
        /// </summary>
        public virtual GroupDB Group { get; set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual string CreatorId { get; set; }

        /// <summary>
        /// User which created board
        /// </summary>
        public virtual UserDB Creator { get; set; }

        /// <summary>
        /// Products which add at board
        /// </summary>
        public virtual IList<GroupProductDB> GroupProducts { get; set; }

        /// <summary>
        /// Messages sent by users
        /// </summary>
        public virtual IList<GroupCommentDB> GroupComments { get; set; }

        /// <summary>
        /// true - group created;
        /// false - group deleted
        /// </summary>
        public virtual bool IsNotDeleted { get; set; }

        /// <summary>
        /// Group creation time
        /// </summary>
        public virtual DateTime Created { get; set; }

        /// <summary>
        /// Group delete time
        /// </summary>
        public virtual DateTime? Deleted { get; set; }

        /// <summary>
        /// Model modification time
        /// </summary>
        public virtual DateTime? Modified { get; set; }
    }
}
