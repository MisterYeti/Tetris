﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vectorf {

    public static Vector2 Round(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x),Mathf.Round(vector2.y));
    }



}
