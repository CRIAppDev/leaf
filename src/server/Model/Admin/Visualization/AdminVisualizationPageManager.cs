﻿// Copyright (c) 2021, UW Medicine Research IT, University of Washington
// Developed by Nic Dobbins and Cliff Spital, CRIO Sean Mooney
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using Model.Validation;
using Model.Error;
using System.Runtime.CompilerServices;

namespace Model.Admin.Visualization
{
    public class AdminVisualizationPageManager
    {
        public interface IAdminVisualizationPageService
        {
            Task<AdminVisualizationPage> CreateVisualizationPageAsync(AdminVisualizationPage query);
            Task<AdminVisualizationPage> UpdateVisualizationPageAsync(AdminVisualizationPage query);
            Task<Guid?> DeleteVisualizationPageAsync(Guid id);
        }

        readonly IAdminVisualizationPageService svc;
        readonly ILogger<AdminVisualizationPageManager> log;

        public AdminVisualizationPageManager(
            IAdminVisualizationPageService service,
            ILogger<AdminVisualizationPageManager> log)
        {
            svc = service;
            this.log = log;
        }

        public async Task<AdminVisualizationPage> CreateVisualizationPageAsync(AdminVisualizationPage page)
        {
            ThrowIfInvalid(page);

            try
            {
                var created = await svc.CreateVisualizationPageAsync(page);
                log.LogInformation("Created VisualizationPage. VisualizationPage:{@VisualizationPage}", created);
                return created;
            }
            catch (DbException db)
            {
                log.LogError("Failed to create VisualizationPage. VisualizationPage:{@VisualizationPage} Code:{Code} Error:{Error}", page, db.ErrorCode, db.Message);
                db.MapThrow();
                throw;
            }
        }

        public async Task<AdminVisualizationPage> UpdateDatasetQueryAsync(AdminVisualizationPage page)
        {
            ThrowIfInvalid(page);

            try
            {
                var updated = await svc.UpdateVisualizationPageAsync(page);
                log.LogInformation("Updated VisualizationPage. VisualizationPage:{@VisualizationPage}", updated);
                return updated;
            }
            catch (DbException db)
            {
                log.LogError("Failed to update VisualizationPage. VisualizationPage:{@VisualizationPage} Code:{Code} Error:{Error}", page, db.ErrorCode, db.Message);
                db.MapThrow();
                throw;
            }
        }

        public async Task<Guid?> DeleteVisualizationPageAsync(Guid id)
        {
            Ensure.NotDefault(id, nameof(id));

            var deleted = await svc.DeleteVisualizationPageAsync(id);
            if (deleted.HasValue)
            {
                log.LogInformation("Deleted VisualizationPage. Id:{Id}", id);
            }
            else
            {
                log.LogInformation("VisualizationPage not found. Id:{Id}", id);
            }
            return deleted;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ThrowIfInvalid(AdminVisualizationPage page)
        {
            Ensure.NotNull(page, nameof(page));
            Ensure.NotNullOrWhitespace(page.PageName, nameof(page.PageName));
        }
    }
}
