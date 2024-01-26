using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ValueObject;

namespace UnityObject
{
    [CreateAssetMenu(fileName = "CD_ColorData", menuName = "Picker3D/CD_Color", order = 0)]
    public class CD_ColorData : ScriptableObject
    {
        public List<ColorData> Colors;
    }
}