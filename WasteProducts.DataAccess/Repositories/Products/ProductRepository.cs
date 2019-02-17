﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Products;
using WasteProducts.DataAccess.Common.Repositories.Products;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories.Products
{   /// <summary>
    ///This class is a context class. A binder for the 'ProductDB' class with a data access.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly WasteContext _context;
        private bool _disposed;

        /// <summary>
        /// Using the context of the WasteContext class through the private field.
        /// </summary>
        /// <param name="context">The specific context of WasteContext</param>
        public ProductRepository(WasteContext context) => _context = context;

        /// <inheritdoc/>
        public async Task<string> AddAsync(ProductDB product)
        {
            product.Barcode.Id = Guid.NewGuid().ToString();
            product.Barcode.Created = DateTime.UtcNow;
            product.Barcode.Product = product;

            product.Id = Guid.NewGuid().ToString();
            product.Created = DateTime.UtcNow;

            _context.Barcodes.Add(product.Barcode);
            _context.Products.Add(product);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return product.Id;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(ProductDB product)
        {
            if (! (await _context.Products.ContainsAsync(product))) return;
            product.Marked = true;

            await UpdateAsync(product).ConfigureAwait(false);
            
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return;

            product.Marked = true;

            await UpdateAsync(product).ConfigureAwait(false);
            
        }

        /// <inheritdoc/>
        public async Task<ProductDB> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name && !p.Marked).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductDB>> SelectAllAsync()
        {
            return await _context.Products.Where(p => !p.Marked).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task <IEnumerable<ProductDB>> SelectWhereAsync(Predicate<ProductDB> predicate)
        {
            var condition = new Func<ProductDB, bool>(predicate);

            return await Task.Run(() =>
            {
                return _context.Products.Include(p => p.Category).Include(p => p.Barcode).Where(condition).ToList();
            });
                
        }

        /// <inheritdoc />
        public async Task <IEnumerable<ProductDB>> SelectByCategoryAsync(CategoryDB category)
        {
            return await _context.Products.Where(p => p.Category.Id == category.Id && p.Marked == false).ToListAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ProductDB> GetByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.Marked).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task AddToCategoryAsync(string productId, string categoryId)
        {
            var productDB = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(false);
            var categoryDB = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId).ConfigureAwait(false);

            productDB.Category = categoryDB;
            productDB.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(ProductDB product)
        {
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id).ConfigureAwait(false);

            var entry = _context.Entry(productInDb);
            entry.CurrentValues.SetValues(product);

            entry.Property(p => p.Modified).CurrentValue = DateTime.UtcNow;

            entry.Property(p => p.Id).IsModified = false;

            await _context.SaveChangesAsync().ConfigureAwait(false) ;
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
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
