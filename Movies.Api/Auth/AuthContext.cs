using System;

namespace Movies.Api.Auth;

public class AuthContext
{
  public Guid? UserId { get; private set; }

  public bool IsAuthenticated => UserId.HasValue;

  public void SetUserId(Guid userId) => UserId = userId;
}
