using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using System;
using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicFollowPath : DynamicArrive
    {
		public override string Name
		{
			get { return "FollowPath"; }
		}

        public Path Path { get; set; }
		// Used to do a foward check for a new position.
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
			this.PathOffset = 0.3f;
			this.MaxAcceleration = 15.0f;
			this.currentPath = null;
        }

        public override MovementOutput GetMovement()
        {
			// We check if we reached the end of the destiny here
			// if we do then put all velocities at 0 and return base method.
			if (this.Path.PathEnd (PathOffset)) {
				this.MaxAcceleration = 0.0f;
				this.MovingTarget.velocity = Vector3.zero;
				this.Character.velocity = Vector3.zero;
				return base.GetMovement ();
			}

		/*	// First time only.
			if (object.ReferenceEquals (null, this.currentPath)) {
				int position = (int)Math.Truncate (this.CurrentParam);
				// o line segment que ele vai percorrer
				this.currentPath = (LineSegmentPath) this.globalPath.LocalPaths [position];
				this.Target.position = this.currentPath.EndPosition;
			} */

			// Before we get a new param we need to save the old one to compare if nodes change.
			float previous = this.CurrentParam;
			this.CurrentParam = this.Path.GetParam (this.Character.position, this.CurrentParam);

			this.Target.position = this.Path.GetPosition (this.CurrentParam + this.PathOffset);
			/*
			if (checkIfParamChanged (previous, this.CurrentParam)) {
				int position = (int)Math.Truncate (this.CurrentParam);
				// o line segment que ele vai percorrer
				this.currentPath = (LineSegmentPath) this.globalPath.LocalPaths [position];
				this.Target.position = this.currentPath.EndPosition;
			} */

			return base.GetMovement ();
        }

		/**
		 * Since we need to know when the currentParam changes its localNode
		 * we check if the previous param and the new param match.
		 * We only compare the integer part of the param. 
		 **/
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
