﻿using System;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Barcodes;
using WasteProducts.DataAccess.Common.Repositories.Barcodes;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Barcodes
{
    public class BarcodeRepository : IBarcodeRepository
    {
        private WasteContext _wasteContext;
        private bool _disposed;

        public BarcodeRepository(WasteContext wasteContext)
        {
            _wasteContext = wasteContext;
        }
        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="id">Id of the barcode.</param>
        /// <returns>Barcode with the specific ID.</returns>
        public BarcodeDB GetById(string id)
        {
            var barcode = _wasteContext.Barcodes.Find(id);
            return barcode;
        }

        /// <summary>
        /// Return the barcode by its numerical barcode.
        /// </summary>
        /// <param name="code">Code of the barcode.</param>
        /// <returns>Barcode with the specific code.</returns>
        public BarcodeDB GetByCode(string code)
        {
            var barcode = _wasteContext.Barcodes.SingleOrDefault(c => c.Code == code);
            return barcode;
        }

        /// <summary>
        /// Add new barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to add.</param>
        public void Add(BarcodeDB barcode)
        {
            barcode.Created = DateTime.UtcNow;
            _wasteContext.Barcodes.Add(barcode);
            _wasteContext.SaveChanges();
        }

        /// <summary>
        /// Update record of the barcode in the repository.
        /// </summary>
        /// <param name="barcode">New barcode to Update.</param>
        public void Update(BarcodeDB barcode)
        {
            _wasteContext.Entry(barcode).State = EntityState.Modified;
            _wasteContext.Barcodes.First(i => i.Id == barcode.Id).Modified = DateTime.UtcNow; ;
            _wasteContext.SaveChanges();
        }
    
        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="id">ID of the barcode.</param>
        public void DeleteById(string id)
        {
            var barcode = _wasteContext.Barcodes.Find(id);
            if (barcode != null) _wasteContext.Barcodes.Remove(barcode);
            _wasteContext.SaveChanges();
        }

        /// <summary>
        /// Delete record of the barcode in the repository.
        /// </summary>
        /// <param name="code">ID of the barcode.</param>
        public void DeleteByCode(string code)
        {
            var barcode = _wasteContext.Barcodes.SingleOrDefault(c => c.Code == code);
            if (barcode != null) _wasteContext.Barcodes.Remove(barcode);
            _wasteContext.SaveChanges();
        }

        /// <summary>
        /// This method calls if the data context means release or closing of connections.
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
                    _wasteContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
