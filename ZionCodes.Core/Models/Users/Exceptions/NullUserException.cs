// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
namespace ZionCodes.Web.Api.Models.Users.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException() : base("The user is null.") { }
    }
}
