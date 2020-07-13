using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public class Tuokamake
    {
        public static void GetPositionS(double startAngle)
        {
            List<double> result = new List<double>();
            int firstStep = 60;
            int secentStep = 60;
            double R = 10;
            for (var i = 0; i < firstStep; i++)
            {
                var angle1 = startAngle + i * Math.PI * 2 / firstStep;
                //var centerX = Math.Sin(angle);
                //var centerY = Math.Cos(angle);

                for (var j = 0; j < secentStep; j++)
                {
                    var angle2 = angle1 + Math.PI * 2 / firstStep / secentStep * j;
                    var position1 = Math.Sin(angle2) * R;
                    var position2 = Math.Cos(angle2) * R;

                    var vectorX = new System.Numerics.Vector3(Convert.ToSingle(Math.Cos(angle2) * R * 0.9), Convert.ToSingle(Math.Sin(angle2) * R * 0.9), 0);
                    var vectorY = new System.Numerics.Vector3(0, 0, Convert.ToSingle(R * 0.9));

                    var angleOfVec = Math.PI * 2 / secentStep * j;

                    var vecResult = Convert.ToSingle(Math.Cos(angleOfVec)) * vectorX + Convert.ToSingle(Math.Sin(angleOfVec)) * vectorY;




                }
            }
        }
    }
}
