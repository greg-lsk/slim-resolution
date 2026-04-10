namespace SlimResolution.Core;

public delegate T Resolution<T>(IResolutionContext context) where T : notnull;