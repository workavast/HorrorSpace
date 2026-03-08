namespace App.InteractiveObjects
{
	public static class InteractiveObjExtension
	{
		public static bool IsInteractive(this IInteractiveObj obj) 
			=> obj.IsActive && obj.IsInteractable;
	}
}