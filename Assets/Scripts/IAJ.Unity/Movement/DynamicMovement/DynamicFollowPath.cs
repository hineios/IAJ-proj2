using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using System;
using Assets.Scripts.IAJ.Unity.Utils;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicFollowPath : DynamicArrive
    {
		public override string Name
		{
			get { return "FollowPath"; }
		}

        public Path Path { get; set; }
		// Used to get info on the total path offset.
		// since the beginning of the global path to the end.
        public float PathOffset { get; set; }

		// The current param for the current local path
		// this one is reseted after we finish a local path
        public float CurrentParam { get; set; }

        private MovementOutput EmptyMovementOutput { get; set; }


		public DynamicFollowPath(KinematicData character, Path path)
        {
            this.Target = new KinematicData();
            this.Character = character;
            this.Path = path;
            this.EmptyMovementOutput = new MovementOutput();
            //don't forget to set all properties
            //arrive properties
			this.CurrentParam = this.Path.GetParam(this.Character.position, 0.0f);
			this.PathOffset = 0.0f;
			this.MaxAcceleration = 15.0f;
        }

        public override MovementOutput GetMovement()
        {
			if (this.Path.PathEnd (PathOffset)) {
				this.MovingTarget.velocity = MathHelper.ConvertOrientationToVector(0.0f);
				return this.EmptyMovementOutput;
			}
			// Nao sei se a ordem esta correcta da chamada das funçoes.

			var newPosition = this.Path.GetPosition (this.CurrentParam);

			this.Target.position = newPosition;
			this.CurrentParam = this.Path.GetParam (newPosition, this.CurrentParam);
			this.PathOffset = this.CurrentParam;

			return base.GetMovement ();
        }
    }
}
