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

		public GlobalPath globalPath { get; set; }

        private MovementOutput EmptyMovementOutput { get; set; }

		private LineSegmentPath currentPath { get; set; }


		public DynamicFollowPath(KinematicData character, Path path)
        {
            this.Target = new KinematicData();
            this.Character = character;
            this.Path = path;
			this.globalPath = (GlobalPath) path;
            this.EmptyMovementOutput = new MovementOutput();
            //don't forget to set all properties
            //arrive properties
			this.CurrentParam = 0.0f;
			this.PathOffset = 0.0f;
			this.MaxAcceleration = 15.0f;
			this.currentPath = null;
        }

        public override MovementOutput GetMovement()
        {
			if (this.Path.PathEnd (PathOffset)) {
				this.MaxAcceleration = 0.0f;
				return this.EmptyMovementOutput;
			}
			// Nao sei se a ordem esta correcta da chamada das funçoes.

			if (object.ReferenceEquals (null, this.currentPath)) {
				int position = (int)Math.Truncate (this.CurrentParam);
				// o line segment que ele vai percorrer
				this.currentPath = (LineSegmentPath) this.globalPath.LocalPaths [position];
				this.Target.position = this.currentPath.EndPosition;
			}

			var newPosition = this.Path.GetPosition (this.CurrentParam);

			float previous = this.CurrentParam;
			this.CurrentParam = this.Path.GetParam (this.Character.position, this.CurrentParam);
			this.PathOffset = this.CurrentParam;

			if (checkIfParamChanged (previous, this.CurrentParam)) {
				int position = (int)Math.Truncate (this.CurrentParam);
				// o line segment que ele vai percorrer
				this.currentPath = (LineSegmentPath) this.globalPath.LocalPaths [position];
				this.Target.position = this.currentPath.EndPosition;
			}

			return base.GetMovement ();
        }


		private bool checkIfParamChanged(float lastParam, float currentParam)
		{
			bool result = false;
			int lastParamInt = (int)Math.Truncate (lastParam);
			int currentParamInt = (int)Math.Truncate (currentParam);
			if (lastParamInt != currentParamInt)
				result = true;
			return result;
		}
    }
}
