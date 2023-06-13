using Sandbox;
using System;
using System.Linq;

namespace UEDFC 
{
	public class UEDFCGame : GameManager 
	{
		public override void ClientJoined( IClient cl )
		{
			base.ClientJoined( cl );

			var pawn = new Player();
			cl.Pawn = pawn;
			pawn.Respawn();
			pawn.DressFromClient( cl );

			var spawnpoints = All.OfType<SpawnPoint>();

			var randomSpawnpoint = spawnpoints.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

			if ( randomSpawnpoint != null )
			{
				var tx = randomSpawnpoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f;
				pawn.Transform = tx;
			}
		}
	}
}
