﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace ModernWpf.Controls
{
	public interface IKeyIndexMapping
	{
		string KeyFromIndex(int index);
		int IndexFromKey(string key);
	}
}
