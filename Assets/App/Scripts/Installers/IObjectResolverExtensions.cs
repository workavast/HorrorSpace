using VContainer;

namespace App.Installers
{
	public static class ObjectResolverExtensions
	{
		public static RegistrationBuilder NonLazy(this RegistrationBuilder builder)
		{
			return builder.As<INonLazy>();
		}
		
		public static RegistrationBuilder NonLazySelf(this RegistrationBuilder builder)
		{
			return builder.AsSelf().As<INonLazy>();
		}
		
		public static RegistrationBuilder NonLazyInterfaces(this RegistrationBuilder builder)
		{
			return builder.AsImplementedInterfaces().As<INonLazy>();
		}
		
		public static RegistrationBuilder NonLazyAll(this RegistrationBuilder builder)
		{
			return builder.AsSelf().AsImplementedInterfaces().As<INonLazy>();
		}
	}
}