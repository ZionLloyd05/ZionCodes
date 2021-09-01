﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using ZionCodes.Core.Models.Users;
using ZionCodes.Web.Api.Models.Users.Exceptions;

namespace ZionCodes.Core.Services.Users
{
    public partial class UserService
    {
        private void ValidateUserOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
            ValidateUserIdIsNull(user.Id);
            ValidateUserFields(user);
            ValidateInvalidAuditFields(user);
            ValidateAuditFieldsDataOnCreate(user);
            ValidateCreatedDateIsRecent(user);
        }

        private void ValidateUserOnModify(User user)
        {
            ValidateUserIsNull(user);
            ValidateUserIdIsNull(user.Id);
            ValidateUserFields(user);
            ValidateInvalidAuditFields(user);
            ValidateAuditFieldsOnModify(user);
        }

        private static void ValidateStorageUser(User storageUser, Guid userId)
        {
            if (storageUser == null)
            {
                throw new NotFoundUserException(userId);
            }
        }

        private void ValidateCreatedDateIsRecent(User user)
        {
            if (IsDateNotRecent(user.CreatedDate))
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.CreatedDate),
                    parameterValue: user.CreatedDate);
            }
        }

        private static void ValidateAuditFieldsDataOnCreate(User user)
        {
            switch (user)
            {
                case { } when user.UpdatedDate != user.CreatedDate:
                    throw new InvalidUserException(
                    parameterName: nameof(User.UpdatedDate),
                    parameterValue: user.UpdatedDate);
            }
        }

        private static void ValidateInvalidAuditFields(User user)
        {
            switch (user)
            {
                case { } when IsInvalid(user.CreatedDate):
                    throw new InvalidUserException(
                    parameterName: nameof(User.CreatedDate),
                    parameterValue: user.CreatedDate);
                case { } when IsInvalid(user.UpdatedDate):
                    throw new InvalidUserException(
                    parameterName: nameof(User.UpdatedDate),
                    parameterValue: user.UpdatedDate);
            }
        }

        private void ValidateAuditFieldsOnModify(User user)
        {
            switch (user)
            {
                case { } when user.UpdatedDate == user.CreatedDate:
                    throw new InvalidUserException(
                        parameterName: nameof(User.UpdatedDate),
                        parameterValue: user.UpdatedDate);

                case { } when IsDateNotRecent(user.UpdatedDate):
                    throw new InvalidUserException(
                        parameterName: nameof(User.UpdatedDate),
                        parameterValue: user.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageUserOnModify(User inputUser, User storageUser)
        {
            switch (inputUser)
            {
                case { } when inputUser.CreatedDate != storageUser.CreatedDate:
                    throw new InvalidUserException(
                        parameterName: nameof(User.CreatedDate),
                        parameterValue: inputUser.CreatedDate);

                case { } when inputUser.UpdatedDate == storageUser.UpdatedDate:
                    throw new InvalidUserException(
                        parameterName: nameof(User.UpdatedDate),
                        parameterValue: inputUser.UpdatedDate);
            }
        }

        private static void ValidateUserFields(User user)
        {
            if (IsInvalid(user.Email))
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.Email),
                    parameterValue: user.Email);
            }
            if (IsInvalid(user.Name))
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.Name),
                    parameterValue: user.Name);
            }
        }

        private static void ValidateUserIdIsNull(Guid userId)
        {
            if (userId == default)
            {
                throw new InvalidUserException(
                    parameterName: nameof(User.Id),
                    parameterValue: userId);
            }
        }

        private static void ValidateUserIsNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }

        private static bool IsInvalid(string input) => String.IsNullOrWhiteSpace(input);
        private static bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) >= oneMinute;
        }

        private void ValidateStorageUsers(IQueryable<User> storageUsers)
        {
            if (!storageUsers.Any())
            {
                this.loggingBroker.LogWarning("No users found in storage.");
            }
        }
    }
}
