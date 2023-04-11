using System.Collections;
using System.Collections.Generic;


public class CreateGuestUserASCommand : CreateUserASCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public CreateGuestUserASCommand()
		: base("CreateGuestUser")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new CreateGuestUserASResponse();
	}
}

public class CreateGuestUserASResponse : CreateUserASResponse
{
}
