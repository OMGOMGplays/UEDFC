using Sandbox;

namespace UEDFC
{
	public partial class TestWeapon : Weapon
	{
		public override string ModelPath => "models/weapons/testweapon.vmdl";

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Game.AssertClient();

			Particles.Create( "null", EffectEntity, "muzzle" );

			Pawn.SetAnimParameter( "b_attack", true );
		}

		public override void PrimaryAttack()
		{
			ShootEffects();
			Pawn.PlaySound( "rust_pistol.shoot" );
			ShootBullet( 0.1f, 100, 20, 1 );
		}

		protected override void Animate()
		{
			Pawn.SetAnimParameter( "holdtype", (int)CitizenAnimationHelper.HoldTypes.Pistol );
		}
	}
}
