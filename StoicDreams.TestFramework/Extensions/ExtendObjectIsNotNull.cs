using System.Diagnostics.CodeAnalysis;

namespace StoicDreams;

public static class ExtendObjectIsNotNull
{
	
	public static T IsNotNull<T>([NotNull] this T? instance)
	{
		if (instance == null) { throw new NullReferenceException($"Expected instance of {typeof(T).FullName} is null."); }
		return instance;
	}
}
