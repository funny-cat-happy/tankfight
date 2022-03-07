using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class BagManager : MonoBehaviour
{
   public static BagManager Instance;
   public bool isHeavyArm=false;
   public int[] ArmId=new int[2]{0,0};
   private void Awake()
   {
      Instance = this;
   }
}
