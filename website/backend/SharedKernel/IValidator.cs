﻿using System.Collections.Generic;

namespace SharedKernel
{
	public interface IValidator<T>
	{
		(bool IsValid, string Error) IsValid(T item);
	}
}
