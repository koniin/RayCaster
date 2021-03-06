﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting
{
    public static class FovTestRunner
    {
        private static Fov fov = new Fov(new SomeOtherBresenHam());
        //private static Fov fov = new Fov(new SecondBresenHam());

        public static void Run(int startX, int startY, int[,] grid, string name) {    
            fov.SetFov(grid, startX, startY, 5, 0);
            grid[startY, startX] = 3;
            Console.WriteLine(name);
            GridPrinter.Print(grid);
        }
    }
}
