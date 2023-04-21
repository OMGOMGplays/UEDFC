using Sandbox;

namespace UEDFC 
{
	public partial class UEDFCPlayer : Player 
	{
		public override void Respawn()
		{
			base.Respawn();

			SetModel("models/citizen/citizen.vmdl");
		}
	}
}