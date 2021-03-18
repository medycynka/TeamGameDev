using UnityEngine;


namespace SzymonPeszek.Misc
{
    /// <summary>
    /// Class storing extension methods for mesh, terrain, floats, etc 
    /// </summary>
    public static class ExtensionMethods
    {
        #region Custom float extensions
        /// <summary>
        /// Remapping given value from value between inputMin and inputMax to value between outputMin to outputMax
        /// </summary>
        /// <param name="value"></param>
        /// <param name="inputMin"></param>
        /// <param name="inputMax"></param>
        /// <param name="outputMin"></param>
        /// <param name="outputMax"></param>
        /// <returns>Remapped value</returns>
        public static float Remap(this float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            return (value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin;
        }
        #endregion
        
        #region Custom Maths extensions
        //Ease in out
        public static float Hermite(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
        }

        public static Vector2 Hermite(Vector2 start, Vector2 end, float value)
        {
            return new Vector2(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value));
        }

        public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
        {
            return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
        }

        //Ease out
        public static float Sinerp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
        }

        public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
        {
            return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));
        }

        public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
        {
            return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));
        }
        //Ease in
        public static float Coserp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
        }

        public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
        {
            return new Vector2(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value));
        }

        public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
        {
            return new Vector3(Coserp(start.x, end.x, value), Coserp(start.y, end.y, value), Coserp(start.z, end.z, value));
        }

        //Boing
        public static float Berp(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static Vector2 Berp(Vector2 start, Vector2 end, float value)
        {
            return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
        }

        public static Vector3 Berp(Vector3 start, Vector3 end, float value)
        {
            return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
        }

        //Like lerp with ease in ease out
        public static float SmoothStep(float x, float min, float max)
        {
            x = Mathf.Clamp(x, min, max);
            float v1 = (x - min) / (max - min);
            float v2 = (x - min) / (max - min);
            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }

        public static Vector2 SmoothStep(Vector2 vec, float min, float max)
        {
            return new Vector2(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max));
        }

        public static Vector3 SmoothStep(Vector3 vec, float min, float max)
        {
            return new Vector3(SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max));
        }

        public static float Lerp(float start, float end, float value)
        {
            return ((1.0f - value) * start) + (value * end);
        }

        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);
            return lineStart + (closestPoint * lineDirection);
        }

        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = Vector3.Normalize(fullDirection);
            float closestPoint = Vector3.Dot((point - lineStart), lineDirection);
            return lineStart + (Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
        }

        //Bounce
        public static float Bounce(float x)
        {
            return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
        }

        public static Vector2 Bounce(Vector2 vec)
        {
            return new Vector2(Bounce(vec.x), Bounce(vec.y));
        }

        public static Vector3 Bounce(Vector3 vec)
        {
            return new Vector3(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));
        }

        // test for value that is near specified float (due to floating point inprecision)
        // all thanks to Opless for this!
        public static bool Approx(float val, float about, float range)
        {
            return ((Mathf.Abs(val - about) < range));
        }

        // test if a Vector3 is close to another Vector3 (due to floating point inprecision)
        // compares the square of the distance to the square of the range as this 
        // avoids calculating a square root which is much slower than squaring the range
        public static bool Approx(Vector3 val, Vector3 about, float range)
        {
            return ((val - about).sqrMagnitude < range * range);
        }

        /*
          * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
          * This is useful when interpolating eulerAngles and the object
          * crosses the 0/360 boundary.  The standard Lerp function causes the object
          * to rotate in the wrong direction and looks stupid. Clerp fixes that.
          */
        public static float Clerp(float start, float end, float value)
        {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);//half the distance between min and max
            float retval = 0.0f;
            float diff = 0.0f;

            if ((end - start) < -half)
            {
                diff = ((max - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

            // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
            return retval;
        }
        #endregion
        
        #region Custom mesh extensions
        /// <summary>
        /// Picks a random point inside a CONVEX mesh.
        /// Taking advantage of Convexity, we can produce more evenly distributed points
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <returns>Random position inside convex mesh</returns>
        public static Vector3 GetRandomPointInsideConvex(this Mesh m)
        {
            // Grab two points on the surface
            Vector3 randomPointOnSurfaceA = m.GetRandomPointOnSurface();
            Vector3 randomPointOnSurfaceB = m.GetRandomPointOnSurface();

            // Interpolate between them
            return Vector3.Lerp(randomPointOnSurfaceA, randomPointOnSurfaceB, Random.Range(0f, 1f));
        }

        /// <summary>
        /// Picks a random point inside a NON-CONVEX mesh.
        /// The only way to get good approximations is by providing a point (if there is one)
        /// that has line of sight to most other points in the non-convex shape.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <param name="pointWhichSeesAll">A point (if there is one) that has line of sight to most other points in the non-convex shape</param>
        /// <returns>Random position inside non convex mesh</returns>
        public static Vector3 GetRandomPointInsideNonConvex(this Mesh m, Vector3 pointWhichSeesAll)
        {
            // Grab one point (and the center which we assume has line of sight with this point)
            Vector3 randomPointOnSurface = m.GetRandomPointOnSurface();

            // Interpolate between them
            return Vector3.Lerp(pointWhichSeesAll, randomPointOnSurface, Random.Range(0f, 1f));
        }
        
        /// <summary>
        /// Picks a random point on the mesh's surface.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <param name="debugTransform">Transform for debug purpose - drawing rays to the surface from given point</param>
        /// <returns>Random point on mesh</returns>
        public static Vector3 GetRandomPointOnSurface(this Mesh m, Transform debugTransform = null)
        {
            // Pick a random triangle (each triangle is 3 integers in a row in m.triangles)
            // So Pick a random origin (0, 3, 6, .. m.triangles.Length - 3)
            // -> Random (0.. m.triangles.Length / 3) * 3
            int triangleOrigin = Mathf.FloorToInt(Random.Range(0f, m.triangles.Length) / 3f) * 3;

            // Grab the 3 points that consist of the triangle
            Vector3 vertexA = m.vertices[m.triangles[triangleOrigin]];
            Vector3 vertexB = m.vertices[m.triangles[triangleOrigin + 1]];
            Vector3 vertexC = m.vertices[m.triangles[triangleOrigin + 2]];

            // Pick a random point on the triangle
            // For a uniform distribution, we pick randomly according to this:
            // http://mathworld.wolfram.com/TrianglePointPicking.html
            // From the point of origin (vertexA) move a random distance towards vertexB and from there a random distance in the direction of (vertexC - vertexB)
            // The only (temporary) downside is that we might end up with points outside our triangle as well, which have to be mapped back
            // The good thing is that these points can only end up in the triangle's "reflection" across the AC side (forming a quad AB, BC, CD, DA)
            Vector3 dAb = vertexB - vertexA;
            Vector3 dBC = vertexC - vertexB;
            float rAb = Random.Range(0f, 1f);
            float rBC = Random.Range(0f, 1f);
            Vector3 randPoint = vertexA + rAb * dAb + rBC * dBC;

            // We have produces random points on a quad (the extension of our triangle)
            // To map back to the triangle, first we check if we are on the extension of the triangle
            // Since we can be on one of two triangles this is equivalent with checking if we are on the correct side of the AC line
            // If we are on the correct side (towards B) we are on the triangle - else we are not.

            // To check that we can compare the direction of our point towards any point on that line (say, C)
            // with the direction of the height of side AC (Cross (triangleNormal, dirBC)))
            Vector3 dirPC = (vertexC - randPoint).normalized;
            Vector3 dirAb = (vertexB - vertexA).normalized;
            Vector3 dirAc = (vertexC - vertexA).normalized;
            Vector3 triangleNormal = Vector3.Cross(dirAc, dirAb).normalized;
            Vector3 dirHAc = Vector3.Cross(triangleNormal, dirAc).normalized;

            // If the two are alligned, we're in the wrong side
            float dot = Vector3.Dot(dirPC, dirHAc);

            // We are on the right side, we're done
            if (dot >= 0)
            {
                // Otherwise, we need to find the symmetric to the center of the "quad" which is on the intersection of side AC with the bisecting line of angle (BA, BC)
                // Given by
                Vector3 centralPoint = (vertexA + vertexC) / 2;

                // And the symmetric point is given by the equation c - p = p_Sym - c => p_Sym = 2c - p
                Vector3 symmetricRandPoint = 2 * centralPoint - randPoint;

                if (debugTransform)
                {
                    Debug.DrawLine(debugTransform.TransformPoint(randPoint),
                        debugTransform.TransformPoint(symmetricRandPoint), Color.red, 10);
                }

                randPoint = symmetricRandPoint;
            }

            // For debugging purposes
            if (debugTransform)
            {
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexA),
                    Color.cyan, 10);
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexB),
                    Color.green, 10);
                Debug.DrawLine(debugTransform.TransformPoint(randPoint), debugTransform.TransformPoint(vertexC),
                    Color.blue, 10);
                // Debug.DrawRay(debugTransform.TransformPoint(randPoint), triangleNormal, Color.cyan, 10); 
            }

            return randPoint;
        }

        /// <summary>
        /// Picks a random point on the mesh's surface within given bounds.
        /// Can be slow on large meshes.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <param name="bounds">Bounds in which the random point will be get</param>
        /// <returns>Random point on mesh within the bounds</returns>
        public static Vector3 GetRandomPointOnSurfaceWithinBound(this Mesh m, Bounds bounds)
        {
            Vector3 testPoint = m.GetRandomPointOnSurface();

            while (!bounds.Contains(testPoint))
            {
                testPoint = m.GetRandomPointOnSurface();
            }
            
            return testPoint;
        }

        /// <summary>
        /// Get array of random points on mesh's surface.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <param name="numberOfPoints">How many points should be created</param>
        /// <returns>Array of random points</returns>
        public static Vector3[] GetRandomPointsOnSurface(this Mesh m, int numberOfPoints)
        {
            Vector3[] randomPoints = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                randomPoints[i] = m.GetRandomPointOnSurface();
            }

            return randomPoints;
        }

        /// <summary>
        /// Get array of random points on mesh's surface within given bounds.
        /// Can be slow on large meshes.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <param name="numberOfPoints">How many points should be created</param>
        /// <param name="bounds">Bounds in which the random points will be get</param>
        /// <returns>Array of random points</returns>
        public static Vector3[] GetRandomPointsOnSurfaceWithinBound(this Mesh m, int numberOfPoints, Bounds bounds)
        {
            Vector3[] randomPoints = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                Vector3 testPoint = m.GetRandomPointOnSurface();
                
                if (bounds.Contains(testPoint))
                {
                    randomPoints[i] = testPoint;
                }
                else
                {
                    i--;
                }
            }

            return randomPoints;
        }
        
        /// <summary>
        /// Returns the mesh's center.
        /// </summary>
        /// <param name="m">Surface's mesh</param>
        /// <returns>Center point of the mesh</returns>
        public static Vector3 GetCenterPoint(this Mesh m)
        {
            Vector3 center = Vector3.zero;

            foreach (Vector3 v in m.vertices)
            {
                center += v;
            }

            return center / m.vertexCount;
        }
        #endregion

        #region Custom terrain extension
        /// <summary>
        /// Generate random point on surface (terrain).
        /// </summary>
        /// <param name="t">Terrain</param>
        /// <param name="terrainLayer">Terrain's layer mask</param>
        /// <returns>Random point on surface</returns>
        public static Vector3 GetRandomPointOnSurface(this Terrain t, LayerMask terrainLayer)
        {
            float terrainLeft = t.transform.position.x;
            float terrainRight = terrainLeft + t.terrainData.size.x;
            float terrainBottom = t.transform.position.z;
            float terrainTop = terrainBottom + t.terrainData.size.z;
            float terrainHeight = 0.0f;
            float randomX = Random.Range(terrainLeft, terrainRight);
            float randomZ = Random.Range(terrainBottom, terrainTop);

            if (Physics.Raycast(new Vector3(randomX, 9999f, randomZ), Vector3.down, out RaycastHit hit, 15000f, terrainLayer))
            {
                terrainHeight = hit.point.y;
            }

            float randomY = terrainHeight;
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);
            
            return randomPosition;
        }

        /// <summary>
        /// Generate random point on given surface (terrain) without unnecessary reference to surface properties.
        /// </summary>
        /// <param name="terrainPosition">Surface's position</param>
        /// <param name="terrainSize">Surface's size</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <returns>Random point on surface</returns>
        public static Vector3 GetRandomPointOnSurfaceNonRef(Vector3 terrainPosition, Vector3 terrainSize, LayerMask terrainLayer)
        {
            float terrainLeft = terrainPosition.x;
            float terrainRight = terrainLeft + terrainSize.x;
            float terrainBottom = terrainPosition.z;
            float terrainTop = terrainBottom + terrainSize.z;
            float terrainHeight = 0.0f;
            float maxHeight = terrainSize.y;
            bool heightCheck = true;
            float randomX = Random.Range(terrainLeft, terrainRight);
            float randomZ = Random.Range(terrainBottom, terrainTop);

            while (heightCheck)
            {
                if (Physics.Raycast(new Vector3(randomX, 9999f, randomZ), Vector3.down, out RaycastHit hit, 15000f,
                    terrainLayer))
                {
                    terrainHeight = hit.point.y;
                    
                    if (terrainHeight <= maxHeight)
                    {
                        heightCheck = false;
                    }
                    else
                    {
                        randomX = Random.Range(terrainLeft, terrainRight);
                        randomZ = Random.Range(terrainBottom, terrainTop);
                    }
                }
            }

            Vector3 randomPosition = new Vector3(randomX, terrainHeight, randomZ);
            
            return randomPosition;
        }

        /// <summary>
        /// Generate array of random points on surface (terrain).
        /// </summary>
        /// <param name="t">Terrain</param>
        /// <param name="terrainLayer">Terrain's layer mask</param>
        /// <param name="amount">Amount of points to generate</param>
        /// <returns>Array of random points on surface</returns>
        public static Vector3[] GetRandomPointsOnSurface(this Terrain t, LayerMask terrainLayer, int amount)
        {
            Vector3[] randomPoints = new Vector3[amount];

            for (int i = 0; i < amount; i++)
            {
                randomPoints[i] = t.GetRandomPointOnSurface(terrainLayer);
            }
            
            return randomPoints;
        }

        /// <summary>
        /// Generate array of random points on given surface (terrain) without unnecessary reference to surface properties.
        /// </summary>
        /// <param name="terrainPosition">Surface's position</param>
        /// <param name="terrainSize">Surface's size</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <param name="amount">Amount of points to generate</param>
        /// <returns>Array of random points on surface</returns>
        public static Vector3[] GetRandomPointsOnSurfaceNonRef(Vector3 terrainPosition, Vector3 terrainSize,
            LayerMask terrainLayer, int amount)
        {
            Vector3[] randomPoints = new Vector3[amount];

            for (int i = 0; i < amount; i++)
            {
                randomPoints[i] = GetRandomPointOnSurfaceNonRef(terrainPosition, terrainSize, terrainLayer);
            }
            
            return randomPoints;
        }

        /// <summary>
        /// Generate random point on surface (terrain) in given bounds.
        /// </summary>
        /// <param name="t">Terrain</param>
        /// <param name="terrainLayer">Terrain's layer mask</param>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <returns>Random point on surface in given bounds</returns>
        public static Vector3 GetRandomPointOnSurfaceInBounds(this Terrain t, LayerMask terrainLayer, Bounds bounds)
        {
            Vector3 randomPoint = t.GetRandomPointOnSurface(terrainLayer);

            while (!bounds.Contains(randomPoint))
            {
                randomPoint = t.GetRandomPointOnSurface(terrainLayer);
            }
            
            return randomPoint;
        }

        /// <summary>
        /// Generate random point on given surface (terrain) without unnecessary reference to surface properties
        /// in given bounds.
        /// </summary>
        /// <param name="terrainPosition">Surface's position</param>
        /// <param name="terrainSize">Surface's size</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <returns>Random point on surface in given bounds</returns>
        public static Vector3 GetRandomPointOnSurfaceInBoundsNonRef(Vector3 terrainPosition, Vector3 terrainSize,
            LayerMask terrainLayer, Bounds bounds)
        {
            Vector3 randomPoint = GetRandomPointOnSurfaceNonRef(terrainPosition, terrainSize, terrainLayer);

            while (!bounds.Contains(randomPoint))
            {
                randomPoint = GetRandomPointOnSurfaceNonRef(terrainPosition, terrainSize, terrainLayer);
            }
            
            return randomPoint;
        }
        
        /// <summary>
        /// Generate random point on given surface (terrain) without unnecessary reference to surface properties
        /// in given bounds. Faster than GetRandomPointOnSurfaceInBoundsNonRef.
        /// </summary>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <returns>Random point on surface in given bounds</returns>
        public static Vector3 GetRandomPointOnSurfaceInBoundsNonRefFast(Bounds bounds,
            LayerMask terrainLayer)
        {
            Vector3 boundPoint = new Vector3(bounds.center.x - bounds.extents.x, 0f, bounds.center.z - bounds.extents.z);
            Vector3 randomPoint = GetRandomPointOnSurfaceNonRef(boundPoint, bounds.size, terrainLayer);

            return randomPoint;
        }
        
        /// <summary>
        /// Generate array of random points on surface (terrain) in given bounds.
        /// </summary>
        /// <param name="t">Terrain</param>
        /// <param name="terrainLayer">Terrain's layer mask</param>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <param name="amount">Amount of points to generate</param>
        /// <returns>Array of random points on surface in given bounds</returns>
        public static Vector3[] GetRandomPointsOnSurfaceInBounds(this Terrain t, LayerMask terrainLayer, Bounds bounds, int amount)
        {
            Vector3[] randomPoints = new Vector3[amount];

            for (int i = 0; i < amount; i++)
            {
                randomPoints[i] = t.GetRandomPointOnSurfaceInBounds(terrainLayer, bounds);
            }
            
            return randomPoints;
        }

        /// <summary>
        /// Generate array of random points on given surface (terrain) without unnecessary reference to surface properties
        /// in given bounds.
        /// </summary>
        /// <param name="terrainPosition">Surface's position</param>
        /// <param name="terrainSize">Surface's size</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <param name="amount">Amount of points to generate</param>
        /// <returns>Array of random points on surface in given bounds</returns>
        public static Vector3[] GetRandomPointsOnSurfaceInBoundsNonRef(Vector3 terrainPosition, Vector3 terrainSize,
            LayerMask terrainLayer, Bounds bounds, int amount)
        {
            Vector3[] randomPoints = new Vector3[amount];

            for (int i = 0; i < amount; i++)
            {
                randomPoints[i] =
                    GetRandomPointOnSurfaceInBoundsNonRef(terrainPosition, terrainSize, terrainLayer, bounds);
            }
            
            return randomPoints;
        }

        /// <summary>
        /// Generate random point on given surface (terrain) without unnecessary reference to surface properties
        /// in given bounds. Faster than GetRandomPointOnSurfaceInBoundsNonRef.
        /// </summary>
        /// <param name="bounds">Bounds in which point will be generated</param>
        /// <param name="terrainLayer">Surface's layer mask</param>
        /// <param name="amount">Amount of points to generate</param>
        /// <returns>Array of random points on surface in given bounds</returns>
        public static Vector3[] GetRandomPointsOnSurfaceInBoundsNonRefFast(Bounds bounds, LayerMask terrainLayer,
            int amount)
        {
            Vector3[] randomPoints = new Vector3[amount];

            for (int i = 0; i < amount; i++)
            {
                randomPoints[i] = GetRandomPointOnSurfaceInBoundsNonRefFast(bounds, terrainLayer);
            }
            
            return randomPoints;
        }
        #endregion
    }
}