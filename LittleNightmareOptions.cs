using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleNightmare;

public class LittleNightmareOptions
{
    public static LittleNightmareOptions Instance = new();

    public void Reset()
    {
        Instance = new LittleNightmareOptions();
    }
}
