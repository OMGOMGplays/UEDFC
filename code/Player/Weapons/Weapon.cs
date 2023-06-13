using Sandbox;
using System.Collections.Generic;

namespace UEDFC
{
	public partial class Weapon : AnimatedEntity
	{

		public Player Pawn => Owner as Player;

		public AnimatedEntity EffectEntity => Pawn;

		public virtual string ModelPath => null;

		public virtual float PrimaryRate => 5.0f;

		[Net, Predicted] public TimeSince TimeSincePrimaryAttack { get; set; }

		public override void Spawn()
		{
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			EnableDrawing = true;

			if ( ModelPath != null )
			{
				SetModel( ModelPath );
			}
		}

		public void OnEquip(Player pawn)
		{
			Owner = pawn;
			SetParent( pawn, true );
			EnableDrawing = true;
		}

		public void OnHolster()
		{
			EnableDrawing = false;
		}

		public override void Simulate( IClient cl )
		{
			Animate();

			if (CanPrimaryAttack())
			{
				using (LagCompensation())
				{
					TimeSincePrimaryAttack = 0;
					PrimaryAttack();
				}
			}
		}

		public virtual bool CanPrimaryAttack()
		{
			if ( !Owner.IsValid() || !Input.Pressed( "attack1" ) ) return false;

			var rate = PrimaryRate;
			if ( rate <= 0 ) return true;

			return TimeSincePrimaryAttack > (1 / rate);
		}

		public virtual void PrimaryAttack()
		{
		}

		protected virtual void Animate()
		{
		}

		public virtual IEnumerable<TraceResult> TraceBullet( Vector3 start, Vector3 end, float radius = 2.0f )
		{
			bool underWater = Trace.TestPoint( start, "water" );

			var trace = Trace.Ray( start, end )
					.UseHitboxes()
					.WithAnyTags( "solid", "enemy" )
					.Ignore( this )
					.Size( radius );

			if ( !underWater )
				trace = trace.WithAnyTags( "water" );

			var tr = trace.Run();

			if ( tr.Hit )
				yield return tr;
		}

		public virtual void ShootBullet( Vector3 pos, Vector3 dir, float spread, float force, float damage, float bulletSize)
		{
			var forward = dir;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
			forward = forward.Normal;

			foreach (var tr  in TraceBullet( pos, pos + forward * 5000, bulletSize))
			{
				tr.Surface.DoBulletImpact( tr );

				if ( !Game.IsServer ) continue;
				if ( !tr.Entity.IsValid() ) continue;

				using (Prediction.Off())
				{
					var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100 * force, damage )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}
		}

		public virtual void ShootBullet(float spread, float force, float damage, float bulletSize)
		{
			Game.SetRandomSeed( Time.Tick );

			var ray = Owner.AimRay;
			ShootBullet(ray.Position, ray.Forward, spread, force, damage, bulletSize );
		}
	}
}
