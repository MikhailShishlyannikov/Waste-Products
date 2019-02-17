﻿using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using WasteProducts.Logic.Common.Models.Diagnostic;
using WasteProducts.Logic.Common.Services.Diagnostic;

namespace WasteProducts.Web.Controllers.Api
{
    /// <summary>
    /// Api controller for database management
    /// </summary>
    [RoutePrefix("api/administration/database")]
    [SwaggerResponse(HttpStatusCode.Unauthorized, "Unauthorized request.")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "Exceptions during the process.")]
    public class DatabaseController : BaseApiController
    {
        private readonly IDbService _dbService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbService">Database management service</param>
        /// <param name="logger">NLog logger</param>
        public DatabaseController(IDbService dbService, ILogger logger) : base(logger)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Returns current database state 
        /// </summary>
        /// <returns>DatabaseState</returns>
        [HttpGet, Route("state")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Returns current database state", typeof(DatabaseState), nameof(DatabaseState), "application/json")]
        public async Task<IHttpActionResult> GetDatabaseStateAsync()
        {
            using (_dbService)
            {
                var status = await _dbService.GetStateAsync().ConfigureAwait(true);
                return Ok(status);
            }
        }

        /// <summary>
        /// Recreates database and seeds it with default test data if seed == true.
        /// </summary>
        /// <param name="seed">Seeds database if "true", does not seed if "false".</param>
        /// <returns>Task</returns>
        [HttpPost, Route("recreate")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Database was successfully recreated.")]
        public async Task<IHttpActionResult> ReCreateDatabaseAsync(bool seed)
        {
            using (_dbService)
            {
                await _dbService.RecreateAsync().ConfigureAwait(true);
                if (seed)
                {
                    await _dbService.SeedAsync().ConfigureAwait(true);
                }
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        /// <summary>
        /// Seeds database with default test data.
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("seed")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Database was successfully seeded.")]
        public async Task<IHttpActionResult> ReCreateAndSeed()
        {
            using (_dbService)
            {
                await _dbService.SeedAsync().ConfigureAwait(true);
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        /// <summary>
        /// Deletes database if database exist, otherwise does nothing
        /// </summary>
        /// <returns>Task</returns>
        [HttpDelete, Route("delete")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Database was deleted.")]
        public async Task<IHttpActionResult> DeleteDatabaseAsync()
        {
            using (_dbService)
            {
                await _dbService.DeleteAsync().ConfigureAwait(true);
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
