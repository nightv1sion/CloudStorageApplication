﻿using Middlewares.ExceptionHandling.Exceptions;

namespace AuthenticationMicroservice.Exceptions.BadRequest;

public class InvalidRefreshTokenBadRequestException : BadRequestException
{
    public InvalidRefreshTokenBadRequestException(string username) : base(
        $"User '{username}: 'Invalid refresh token")
    {
        
    }
}