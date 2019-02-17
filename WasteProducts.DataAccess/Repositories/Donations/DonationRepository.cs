﻿using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Comparers.Donations;
using WasteProducts.DataAccess.Common.Models.Donations;
using WasteProducts.DataAccess.Common.Repositories.Donations;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Donations
{
    /// <inheritdoc />
    public class DonationRepository : IDonationRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of DonationRepository.
        /// </summary>
        /// <param name="context">The specific context of WasteContext.</param>
        public DonationRepository(WasteContext context) => _context = context;

        /// <inheritdoc />
        public async Task AddAsync(DonationDB donation)
        {
            DonorDB donorFromDB = await _context.Donors.FirstOrDefaultAsync(d => d.Id == donation.Donor.Id).ConfigureAwait(false);
            if (new DonorDBComparer().Equals(donorFromDB, donation.Donor))
                donation.Donor = donorFromDB; // The donor is in the database and has not changed.
            else if (donorFromDB != null) // The donor is in the database and has changed.
            {
                if (new AddressDBComparer() // But the address has not changed.
                    .Equals(donation.Donor.Address, donorFromDB.Address))
                {
                    donation.Donor.AddressId = donorFromDB.AddressId;
                    donation.Donor.Address = donorFromDB.Address;
                }
                else  // And the address has changed too.
                {
                    donation.Donor = await SetAddressFromDBIfExistsAsync(donation.Donor).ConfigureAwait(false);
                    if (donation.Donor.Address.Id == default(Guid))
                    { // The changed address is not in the database.
                        AddressDB newAddress = _context.Addresses.Add(donation.Donor.Address);
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                        donation.Donor.AddressId = newAddress.Id;
                        donation.Donor.Address = newAddress;
                    }
                    if (donorFromDB.Address.Donors.Count == 1) // Is the old address not used?
                        _context.Entry(donorFromDB.Address).State = EntityState.Deleted;
                }
                donation.Donor.Modified = DateTime.Now;
                donation.Donor.Created = donorFromDB.Created;
                _context.Entry(donorFromDB).State = EntityState.Detached;
                _context.Entry(donation.Donor).State = EntityState.Modified;
            }
            else // The donator is not in the database.
                donation.Donor = await SetAddressFromDBIfExistsAsync(donation.Donor).ConfigureAwait(false);
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> ContainsAsync(string id)
        {
            return await _context.Donations.AnyAsync(d => d.TransactionId == id).ConfigureAwait(false);
        }

        /// <summary>
        /// Use ONLY with TestDB!
        /// </summary>
        public void RecreateTestDatabase()
        {
            _context.Database.Delete();
            _context.Database.Create();
        }

        /// <summary>
        /// Releases all resources used by the DonationRepository.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        private async Task<DonorDB> SetAddressFromDBIfExistsAsync(DonorDB donor)
        {
            AddressDB addressFromDB =
                await _context.Addresses.FirstOrDefaultAsync(
                    a => a.City == donor.Address.City
                    && a.Country == donor.Address.Country
                    && a.IsConfirmed == donor.Address.IsConfirmed
                    && a.Name == donor.Address.Name
                    && a.State == donor.Address.State
                    && a.Street == donor.Address.Street
                    && a.Zip == donor.Address.Zip)
                    .ConfigureAwait(false);
            if (addressFromDB != null) // The address is in the database.
            {
                donor.AddressId = addressFromDB.Id;
                donor.Address = addressFromDB;
                _context.Entry(donor.Address).State = EntityState.Unchanged;
            }
            return donor;
        }
    }
}