using Sandbox;

namespace UEDFC
{
	public partial class GameCommands : Game
	{
		[ConCmd.Client("kill")]
		public static void KillCommand()
		{
			var caller = ConsoleSystem.Caller as Player;
			caller.TakeDamage( new DamageInfo { Damage = caller.Health * 99f } );
		}
	}
}
