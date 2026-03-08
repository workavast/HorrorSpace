namespace App.InteractiveObjects
{
	public static class InteractiveObjExtension
	{
		public static bool ShouldDraw(this IInteractiveObj obj) 
			=> obj.IsActive;
	}
}