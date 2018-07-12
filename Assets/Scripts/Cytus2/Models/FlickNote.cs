﻿using System;
using Cytus2.Views;
using UnityEngine;

namespace Cytus2.Models
{

    public class FlickNote : GameNote
    {

        public bool IsFlicking;
        public float FlickingStartTime;
        public Vector2 FlickingStartPosition;

        private float age;

        protected override void Awake()
        {
            base.Awake();
            View = new FlickNoteView(this);
            MaxMissThreshold = 0.300f;
        }

        public override void Touch(Vector2 screenPos)
        {
            // This method should never be invoked
            throw new InvalidOperationException();
        }

        public void StartFlicking(Vector2 screenPos)
        {
            if (IsFlicking) return;
            IsFlicking = true;
            FlickingStartTime = Game.Time;
            FlickingStartPosition = screenPos;
        }

        public bool UpdateFingerPosition(Vector2 screenPos)
        {
            if (IsCleared) return true;
            var swipeVector = screenPos - FlickingStartPosition;
            if (Math.Abs(swipeVector.x) > Camera.main.orthographicSize * 0.05f)
            {
                Clear(CalculateGrading());
                return true;
            }
            return false;
        }

        public override NoteGrading CalculateGrading()
        {
            var grading = NoteGrading.Miss;
            var timeUntil = TimeUntilStart;

            if (Game.Play.IsRanked)
            {
                if (timeUntil >= 0)
                {
                    if (timeUntil < 0.800f)
                    {
                        grading = NoteGrading.Great;
                    }
                    if (timeUntil <= 0.200f)
                    {
                        grading = NoteGrading.Perfect;
                    }
                } 
                else
                {
                    var timePassed = -timeUntil;
                    if (timePassed < 0.300f)
                    {
                        grading = NoteGrading.Great;
                    }
                    if (timePassed <= 0.100f)
                    {
                        grading = NoteGrading.Perfect;
                    }
                }
            }
            else
            {
                if (timeUntil >= 0)
                {
                    if (timeUntil < 0.400f)
                    {
                        grading = NoteGrading.Great;
                    }
                    if (timeUntil <= 0.120f)
                    {
                        grading = NoteGrading.Perfect;
                    }
                } 
                else
                {
                    var timePassed = -timeUntil;
                    if (timePassed < 0.150f)
                    {
                        grading = NoteGrading.Great;
                    }
                    if (timePassed <= 0.060f)
                    {
                        grading = NoteGrading.Perfect;
                    }
                }
            }
            return grading;
        }

    }

}