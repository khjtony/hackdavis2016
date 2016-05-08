using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class calPosition : MonoBehaviour {

    private Vector2[] tags_pos = new Vector2[4];
    private int[] tags_dis = new int[4];
    private Vector2 current_pos = new Vector2();
    private float tolerance = 200.0f;
    private double _x = 0.0f;
    private double _y = 0.0f;
    serialParser myParser;


    static void CircleApprox(double x0, double y0, double r0,
            double x1, double y1, double r1,
            double x2, double y2, double r2,
            ref double sol_x, ref double sol_y)
    {
        // Get ratios for circle A
        double dx_a = x1 - x0;
        double dy_a = y1 - y0;
        //double ratio_a = r1 / r0;
        double denom_a = r1 + r0;

        dx_a = dx_a *(r0)/(denom_a);
        dy_a = dy_a *(r0)/(denom_a);

        // Compute new approximate circle A
        double x_a = x0 + dx_a;
        double y_a = y0 + dy_a;
        double r_a = (r1 + r2) / 2;

        // Get ratios for circle 
        double dx_b = x2 - x_a;
        double dy_b = y2 - y_a;
        // double ratio_b = r2 / r_a;
        double denom_b = r_a + r2;

        dx_b = dx_b * (r_a) / (denom_b);
        dy_b = dy_b * (r_a) / (denom_b);

       // dx_b = dx_b / ratio_b;
        //dy_b = dy_b / ratio_b;


        // Get approximate point
        double x_b = x_a + dx_b;
        double y_b = y_a + dy_b;

        sol_x = x_b;
        sol_y = y_b;
    } // CircleApprox()


    static bool CircleIntersect(double x0, double y0, double r0,
        double x1, double y1, double r1,
        double x2, double y2, double r2,
        ref double sol_x, ref double sol_y, double tolerance)
    {
        double a, dx, dy, d, h, rx, ry;
        double point2_x, point2_y;

        /* dx and dy are the vertical and horizontal distances between
        * the circle centers.
        */
        dx = x1 - x0;
        dy = y1 - y0;

        /* Determine the straight-line distance between the centers. */
        d = Math.Sqrt((dy * dy) + (dx * dx));

        /* Check for solvability. */
        if (d > (r0 + r1))
        {
            /* no solution. circles do not intersect. */
            return false;
        }
        if (d < Math.Abs(r0 - r1))
        {
            /* no solution. one circle is contained in the other */
            return false;
        }

        /* 'point 2' is the point where the line through the circle
        * intersection points crosses the line between the circle
        * centers.
        */

        /* Determine the distance from point 0 to point 2. */
        a = ((r0 * r0) - (r1 * r1) + (d * d)) / (2.0 * d);

        /* Determine the coordinates of point 2. */
        point2_x = x0 + (dx * a / d);
        point2_y = y0 + (dy * a / d);

        /* Determine the distance from point 2 to either of the
        * intersection points.
        */
        h = Math.Sqrt((r0 * r0) - (a * a));

        /* Now determine the offsets of the intersection points from
        * point 2.
        */
        rx = -dy * (h / d);
        ry = dx * (h / d);

        /* Determine the absolute intersection points. */
        double intersectionPoint1_x = point2_x + rx;
        double intersectionPoint2_x = point2_x - rx;
        double intersectionPoint1_y = point2_y + ry;
        double intersectionPoint2_y = point2_y - ry;

        //Console.WriteLine("INTERSECTION Circle1 AND Circle2:" +
        //    "(" + intersectionPoint1_x + "," + intersectionPoint1_y + ")" +
          //  " AND (" + intersectionPoint2_x + "," + intersectionPoint2_y + ")");

        /* Lets determine if circle 3 intersects at either of the above intersection points. */
        dx = intersectionPoint1_x - x2;
        dy = intersectionPoint1_y - y2;
        double d1 = Math.Sqrt((dy * dy) + (dx * dx));

        dx = intersectionPoint2_x - x2;
        dy = intersectionPoint2_y - y2;
        double d2 = Math.Sqrt((dy * dy) + (dx * dx));

        if (Math.Abs(d1 - r2) < tolerance)
        {
            sol_x = intersectionPoint1_x;
            sol_y = intersectionPoint1_y;

            print("INTERSECTION Circle1 AND Circle2 AND Circle3:" +
                "(" + intersectionPoint1_x + "," + intersectionPoint1_y + ")");
        }
        else if (Math.Abs(d2 - r2) < tolerance)
        {
            sol_x = intersectionPoint2_x;
            sol_y = intersectionPoint2_y;

            print("INTERSECTION Circle1 AND Circle2 AND Circle3:" +
                "(" + intersectionPoint2_x + "," + intersectionPoint2_y + ")");
        }
        else
        {
           print("INTERSECTION Circle1 AND Circle2 AND Circle3: NONE");
            return false;
        }

        return true;
    }

    static bool get_circle_sol2(double x1, double y1, double r1,
        double x2, double y2, double r2,
        double x3, double y3, double r3,
        ref double sol_x, ref double sol_y, double tolerance, double step_size)
    {
        bool converges = false;
        int max_steps = 100;
        int steps = 0;

        while (!converges)
        {
            converges = CircleIntersect(
                x1, y1, r1,
                x2, y2, r2,
                x3, y3, r3,
                ref sol_x, ref sol_y, tolerance);

            r1 += step_size;
            r2 += step_size;
            r3 += step_size;

            steps++;
            if(steps > max_steps)
            {
                return false;
            }

        }

        return true;

    }


    static void get_circle_sol(double x1, double y1, double r1,
        double x2, double y2, double r2,
        double x3, double y3, double r3,
        ref double sol_x, ref double sol_y, double tolerance)
    {
       // print("First Try");
        bool converges = false;
        converges = CircleIntersect(
            x1, y1, r1,
            x2, y2, r2,
            x3, y3, r3,
            ref sol_x, ref sol_y, tolerance);

        if (!converges)
        {
            // Console.WriteLine("Second Try;");
            //print("Second Try");
            // Second Try
            converges = CircleIntersect(
                x1, y1, r1,
                x3, y3, r3,
                x2, y2, r2,
                ref sol_x, ref sol_y, tolerance);

            if (!converges)
            {
                //   Console.WriteLine("Third Try;");
                //print("Third Try");
                // Third Try
                converges = CircleIntersect(
                    x2, y2, r2,
                    x3, y3, r3,
                    x1, y1, r1,
                    ref sol_x, ref sol_y, tolerance);

            } // third try
        } // second try

        if (!converges)
        {
            // Give up with approximation
            CircleApprox(
                x1, y1, r1,
                x2, y2, r2,
                x3, y3, r3,
                ref sol_x, ref sol_y);
           // Console.WriteLine("Failed To converge: (" + sol_x + "," + sol_y + ")");
        } // give up
        //else
         //   Console.WriteLine("Converge: (" + sol_x + "," + sol_y + ")");
    } // get_circle_sol()

    /*
    public void dirtyPos(float x1, float y1, float r1,
        )

    */

    public void updatePos()
    {
        tags_dis[1] = myParser.getValue(1);
        tags_dis[2] = myParser.getValue(2);
        tags_dis[3] = myParser.getValue(3);
        double temp_x = 0;
        double temp_y = 0;


        /*
        bool flag = get_circle_sol2(
                tags_pos[1].x, tags_pos[1].y, tags_dis[1],
                tags_pos[2].x, tags_pos[2].y, tags_dis[2],
                tags_pos[3].x, tags_pos[3].y, tags_dis[3],
                ref temp_x, ref temp_y, tolerance, 100.0f);
        if (flag)
        {
            _x = temp_x;
            _y = temp_y;
        }

        */

        /*
        print("dis 1: " + tags_dis[1] +
            "dis 2: " + tags_dis[2] +
            "dis 3: " + tags_dis[3] );
        
        print("_x is: " + _x + " _y is: " + _y);
        */
        _y = tags_dis[3];
    }

    public Vector3 getPosition()
    {
        //return new Vector3((float)_x, 1, (float)_y);
        print("Y is: " + tags_dis[3]);
        return new Vector3(0, 0, tags_dis[3]);
    }


    // Use this for initialization
    void Start () {
        tags_pos[1] = new Vector2(0, 0);
        tags_pos[2] = new Vector2((500*2.54f), 0);
        tags_pos[3] = new Vector2(0, (624*2.54f));
        tags_dis[1] = 0;
        tags_dis[2] = 0;
        tags_dis[3] = 0;
        current_pos.x = 0.0f;
        current_pos.y = 0.0f;
        myParser = GetComponent<serialParser>();

    }
	
    void Update()
    {
        updatePos();
    }

}
