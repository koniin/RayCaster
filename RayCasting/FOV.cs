﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RayCasting {
    public class Fov {
        private readonly BresenHamAlgorithm bresenHam;

        public Fov() {
            bresenHam = new BresenHamAlgorithm();
        }

        public void SetFov2(int[,] grid, int startX, int startY, int blocked, int fovMarker) {
            Parallel.Invoke(
                () => CheckAgainstBorder(grid, 5, 0, 5, (i) => bresenHam.Line(startY, startX, 0, i)),
                () => CheckAgainstBorder(grid, 5, 0, 5, (i) => bresenHam.Line(startY, startX, grid.GetUpperBound(0), i)),
                () => CheckAgainstBorder(grid, 5, 0, 5, (i) => bresenHam.Line(startY, startX, i, 0)),
                () => CheckAgainstBorder(grid, 5, 0, 5, (i) => bresenHam.Line(startY, startX, i, grid.GetUpperBound(1))));

        }

        public void SetFov(int[,] grid, int startX, int startY, int blocked, int fovMarker) {
            // X == 4
            // Y == 0

            List<Point> points = bresenHam.Line(startY, startX, 0, 4);
            points.Reverse();
            SetMarker(points, grid, blocked, fovMarker);

            for (int i = 0; i < 5; i++) { // all vertical at bottom
                points = bresenHam.Line(startY, startX, 5, i); 
                if(i < startX)
                    points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }

            for (int i = 0; i < 5; i++) { // all vertical at top
                points = bresenHam.Line(startY, startX, 0, i);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }

            for (int i = 0; i < 6; i++) { // all horizontal left
                points = bresenHam.Line(startY, startX, i, 0);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
            
            for (int i = 0; i < 6; i++) { // all horizontal right
                points = bresenHam.Line(startY, startX, i, 4);
                if (i < startX)
                    points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
        }

/*
        public void SetFov(int[,] grid, int startX, int startY, int blocked, int fovMarker) {
            for (int i = 0; i < 5; i++) {
                List<Point> points = bresenHam.Line(startY, startX, 0, i);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
            for (int i = 0; i < 5; i++) {
                List<Point> points = bresenHam.Line(startY, startX, 5, i);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
            for (int i = 0; i < 6; i++) {
                List<Point> points = bresenHam.Line(startY, startX, i, 0);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
            for (int i = 0; i < 6; i++) {
                List<Point> points = bresenHam.Line(startY, startX, i, 4);
                SetMarker(points, grid, blocked, fovMarker);
            }
        }
        */
        private void CheckAgainstBorder(int[,] grid, int blocked, int fovMarker, int borderSize, Func<int, List<Point>> getPoints) {
            for (int i = 0; i < borderSize; i++) {
                List<Point> points = getPoints(i);
                points.Reverse();
                SetMarker(points, grid, blocked, fovMarker);
            }
        }

        private void SetMarker(IEnumerable<Point> points, int[,] grid, int blocked, int fovMarker) {
            foreach (Point p in points) {
                if (grid[p.X, p.Y] == blocked)
                    break;
                grid[p.X, p.Y] = fovMarker;
            }
            GridPrinter.Print(grid, true);
        }
    }
}