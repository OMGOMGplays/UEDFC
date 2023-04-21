using Sandbox;

namespace UEDFC 
{
	public class UEDFCGame : GameManager 
	{
		public override void ClientJoined( IClient cl )
		{
			base.ClientJoined( cl );

			var pawn = new UEDFCPlayer();
			pawn.Respawn();

			cl.Pawn = pawn;
		}
	}
}