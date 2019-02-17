﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WasteProducts.DataAccess.Common.Models.Products
{
    /// <summary>
    /// Model for entity Category used in database.
    /// </summary>
    public class CategoryDB
    {
        /// <summary>
        /// Unique identifier of concrete Category in database.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Unique name of concrete Category in database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contains description of a specific category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of products that belong to a specific Category in database
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<ProductDB> Products { get; set; }

        /// <summary>
        /// Defines whether the category is marked for deletion
        /// </summary>
        public bool Marked { get; set; }
    }
}
