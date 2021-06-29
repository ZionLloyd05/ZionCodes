﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZionCodes.Core.Models.Tags;
using ZionCodes.Core.Models.Tags.Exceptions;

namespace ZionCodes.Core.Services.Tags
{
    public partial class TagService
    {
        private delegate ValueTask<Tag> ReturningTagFunction();

        private async ValueTask<Tag> TryCatch(
            ReturningTagFunction returningTagFunction)
        {
            try
            {
                return await returningTagFunction();
            }
            catch (NullTagException nullTagException)
            {
                throw CreateAndLogValidationException(nullTagException);
            }
            catch (InvalidTagException invalidTagException)
            {
                throw CreateAndLogValidationException(invalidTagException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTagException =
                    new AlreadyExistsTagException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsTagException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
        }


        private TagDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var categoryDependencyException = new TagDependencyException(exception);
            this.loggingBroker.LogCritical(categoryDependencyException);

            return categoryDependencyException;
        }

        private TagValidationException CreateAndLogValidationException(Exception exception)
        {
            var TagValidationException = new TagValidationException(exception);
            this.loggingBroker.LogError((Exception)TagValidationException);

            return TagValidationException;
        }
        private TagDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var tagDependencyException = new TagDependencyException(exception);
            this.loggingBroker.LogError(tagDependencyException);

            return tagDependencyException;
        }
    }
}
