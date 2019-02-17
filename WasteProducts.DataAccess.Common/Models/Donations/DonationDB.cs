﻿using System;

namespace WasteProducts.DataAccess.Common.Models.Donations
{
    /// <summary>
    /// DAL level model of donation.
    /// </summary>
    public class DonationDB
    {
        /// <summary>
        /// Unique identifier for a specific donation.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Specifies the date of donation.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Specifies the gross of donation.
        /// </summary>
        public decimal Gross { get; set; }

        /// <summary>
        /// Specifies the currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Transaction fee associated with the donation.
        /// Gross minus Fee equals the amount deposited into the receiver E-Mail account.
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Specifies the foreign key.
        /// </summary>
        public string DonorId { get; set; }

        /// <summary>
        /// Specifies the donor.
        /// </summary>
        public virtual DonorDB Donor { get; set; }

        /// <summary>
        /// Specifies the timestamp for creating of a specific donation in the database.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}