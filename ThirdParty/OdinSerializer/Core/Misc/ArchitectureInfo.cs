//-----------------------------------------------------------------------
// <copyright file="ArchitectureInfo.cs" company="Sirenix IVS">
// Copyright (c) 2018 Sirenix IVS
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace OdinSerializer
{
    using System;
    using UnityEngine;

    /// <summary>
    /// This class gathers info about the current architecture for the purpose of determinining
    /// the unaligned read/write capabilities that we have to work with.
    /// </summary>
    public unsafe static class ArchitectureInfo
    {
        public static bool Architecture_Supports_Unaligned_Float32_Reads;

        /// <summary>
        /// This will be false on some ARM architectures, such as ARMv7.
        /// In these cases, we will have to perform slower but safer int-by-int read/writes of data.
        /// <para />
        /// Since this value will never change at runtime, performance hits from checking this 
        /// everywhere should hopefully be negligible, since branch prediction from speculative
        /// execution will always predict it correctly.
        /// </summary>
        public static bool Architecture_Supports_All_Unaligned_ReadWrites;

        static ArchitectureInfo()
        {
#if UNITY_EDITOR
            Architecture_Supports_Unaligned_Float32_Reads = true;
            Architecture_Supports_All_Unaligned_ReadWrites = true;
#else
            // At runtime, we are going to be very pessimistic and assume the
            // worst until we get more info about the platform we are on.
            Architecture_Supports_Unaligned_Float32_Reads = true;
            Architecture_Supports_All_Unaligned_ReadWrites = true;
            Console.WriteLine("Odin Serializer ArchitectureInfo initialization with defaults (all unaligned read/writes disabled).");
#endif
        }
  public enum RuntimePlatform
  {
    /// <summary>
    ///   <para>In the Unity editor on macOS.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.OSXEditor.html">External documentation for `RuntimePlatform.OSXEditor`</a></footer>
    OSXEditor = 0,
    /// <summary>
    ///   <para>In the player on macOS.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.OSXPlayer.html">External documentation for `RuntimePlatform.OSXPlayer`</a></footer>
    OSXPlayer = 1,
    /// <summary>
    ///   <para>In the player on Windows.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WindowsPlayer.html">External documentation for `RuntimePlatform.WindowsPlayer`</a></footer>
    WindowsPlayer = 2,
    /// <summary>
    ///   <para>In the web player on macOS.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.OSXWebPlayer.html">External documentation for `RuntimePlatform.OSXWebPlayer`</a></footer>
    [Obsolete("WebPlayer export is no longer supported in Unity 5.4+.", true)] OSXWebPlayer = 3,
    /// <summary>
    ///   <para>In the Dashboard widget on macOS.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.OSXDashboardPlayer.html">External documentation for `RuntimePlatform.OSXDashboardPlayer`</a></footer>
    [Obsolete("Dashboard widget on Mac OS X export is no longer supported in Unity 5.4+.", true)] OSXDashboardPlayer = 4,
    /// <summary>
    ///   <para>In the web player on Windows.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WindowsWebPlayer.html">External documentation for `RuntimePlatform.WindowsWebPlayer`</a></footer>
    [Obsolete("WebPlayer export is no longer supported in Unity 5.4+.", true)] WindowsWebPlayer = 5,
    /// <summary>
    ///   <para>In the Unity editor on Windows.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WindowsEditor.html">External documentation for `RuntimePlatform.WindowsEditor`</a></footer>
    WindowsEditor = 7,
    /// <summary>
    ///   <para>In the player on the iPhone.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.IPhonePlayer.html">External documentation for `RuntimePlatform.IPhonePlayer`</a></footer>
    IPhonePlayer = 8,
    [Obsolete("PS3 export is no longer supported in Unity >=5.5.")] PS3 = 9,
    [Obsolete("Xbox360 export is no longer supported in Unity 5.5+.")] XBOX360 = 10, // 0x0000000A
    /// <summary>
    ///   <para>In the player on Android devices.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.Android.html">External documentation for `RuntimePlatform.Android`</a></footer>
    Android = 11, // 0x0000000B
    [Obsolete("NaCl export is no longer supported in Unity 5.0+.")] NaCl = 12, // 0x0000000C
    /// <summary>
    ///   <para>In the player on Linux.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.LinuxPlayer.html">External documentation for `RuntimePlatform.LinuxPlayer`</a></footer>
    LinuxPlayer = 13, // 0x0000000D
    [Obsolete("FlashPlayer export is no longer supported in Unity 5.0+.")] FlashPlayer = 15, // 0x0000000F
    /// <summary>
    ///   <para>In the Unity editor on Linux.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.LinuxEditor.html">External documentation for `RuntimePlatform.LinuxEditor`</a></footer>
    LinuxEditor = 16, // 0x00000010
    /// <summary>
    ///   <para>In the player on WebGL</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WebGLPlayer.html">External documentation for `RuntimePlatform.WebGLPlayer`</a></footer>
    WebGLPlayer = 17, // 0x00000011
    [Obsolete("Use WSAPlayerX86 instead")] MetroPlayerX86 = 18, // 0x00000012
    /// <summary>
    ///   <para>In the player on Windows Store Apps when CPU architecture is X86.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WSAPlayerX86.html">External documentation for `RuntimePlatform.WSAPlayerX86`</a></footer>
    WSAPlayerX86 = 18, // 0x00000012
    [Obsolete("Use WSAPlayerX64 instead")] MetroPlayerX64 = 19, // 0x00000013
    /// <summary>
    ///   <para>In the player on Windows Store Apps when CPU architecture is X64.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WSAPlayerX64.html">External documentation for `RuntimePlatform.WSAPlayerX64`</a></footer>
    WSAPlayerX64 = 19, // 0x00000013
    [Obsolete("Use WSAPlayerARM instead")] MetroPlayerARM = 20, // 0x00000014
    /// <summary>
    ///   <para>In the player on Windows Store Apps when CPU architecture is ARM.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.WSAPlayerARM.html">External documentation for `RuntimePlatform.WSAPlayerARM`</a></footer>
    WSAPlayerARM = 20, // 0x00000014
    [Obsolete("Windows Phone 8 was removed in 5.3")] WP8Player = 21, // 0x00000015
    [Obsolete("BlackBerryPlayer export is no longer supported in Unity 5.4+.")] BlackBerryPlayer = 22, // 0x00000016
    [Obsolete("TizenPlayer export is no longer supported in Unity 2017.3+.")] TizenPlayer = 23, // 0x00000017
    [Obsolete("PSP2 is no longer supported as of Unity 2018.3")] PSP2 = 24, // 0x00000018
    /// <summary>
    ///   <para>In the player on the Playstation 4.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.PS4.html">External documentation for `RuntimePlatform.PS4`</a></footer>
    PS4 = 25, // 0x00000019
    [Obsolete("PSM export is no longer supported in Unity >= 5.3")] PSM = 26, // 0x0000001A
    /// <summary>
    ///   <para>In the player on Xbox One.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.XboxOne.html">External documentation for `RuntimePlatform.XboxOne`</a></footer>
    XboxOne = 27, // 0x0000001B
    [Obsolete("SamsungTVPlayer export is no longer supported in Unity 2017.3+.")] SamsungTVPlayer = 28, // 0x0000001C
    [Obsolete("Wii U is no longer supported in Unity 2018.1+.")] WiiU = 30, // 0x0000001E
    /// <summary>
    ///   <para>In the player on the Apple's tvOS.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.tvOS.html">External documentation for `RuntimePlatform.tvOS`</a></footer>
    tvOS = 31, // 0x0000001F
    /// <summary>
    ///   <para>In the player on Nintendo Switch.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.Switch.html">External documentation for `RuntimePlatform.Switch`</a></footer>
    Switch = 32, // 0x00000020
    Lumin = 33, // 0x00000021
    /// <summary>
    ///   <para>In the player on Stadia.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.Stadia.html">External documentation for `RuntimePlatform.Stadia`</a></footer>
    Stadia = 34, // 0x00000022
    /// <summary>
    ///   <para>In the player on CloudRendering.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.CloudRendering.html">External documentation for `RuntimePlatform.CloudRendering`</a></footer>
    CloudRendering = 35, // 0x00000023
    [Obsolete("GameCoreScarlett is deprecated, please use GameCoreXboxSeries (UnityUpgradable) -> GameCoreXboxSeries", false)] GameCoreScarlett = 36, // 0x00000024
    GameCoreXboxSeries = 36, // 0x00000024
    GameCoreXboxOne = 37, // 0x00000025
    /// <summary>
    ///   <para>In the player on the Playstation 5.</para>
    /// </summary>
    /// <footer><a href="file:///D:/Work/Unity/2020.3.17f1/Editor/Data/Documentation/en/ScriptReference/RuntimePlatform.PS5.html">External documentation for `RuntimePlatform.PS5`</a></footer>
    PS5 = 38, // 0x00000026
  }
        internal static void SetRuntimePlatform(RuntimePlatform platform)
        {
            // Experience indicates that unaligned read/write support is pretty spotty and sometimes causes subtle bugs even when it appears to work,
            // so to be safe, we only enable it for platforms where we are certain that it will work.

            switch (platform)
            {
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.PS3:
                case RuntimePlatform.PS4:
                case RuntimePlatform.XBOX360:
                case RuntimePlatform.XboxOne:
                case RuntimePlatform.WebGLPlayer:
                case RuntimePlatform.WSAPlayerX64:
                case RuntimePlatform.WSAPlayerX86:
                case RuntimePlatform.WiiU:
                    
                    try
                    {
                        // Try to perform some unaligned float reads.
                        // If this throws an exception, the current
                        // architecture does not support doing this.

                        // Note that there are cases where this is supported
                        // but other unaligned read/writes are not, usually 
                        // 64-bit read/writes. However, testing indicates 
                        // that these read/writes cause hard crashes and not
                        // NullReferenceExceptions, and so we cannot test for
                        // them but must instead look at the architecture.

                        byte[] testArray = new byte[8];

                        fixed (byte* test = testArray)
                        {
                            // Even if test is weirdly aligned in the stack, trying four differently aligned 
                            // reads will definitely have an unaligned read or two in there.

                            // If all of these reads work, we are safe. We do it this way instead of just having one read,
                            // because as far as I have been able to determine, there are no guarantees about the alignment 
                            // of local stack memory.

                            for (int i = 0; i < 4; i++)
                            {
                                float value = *(float*)(test + i);
                            }

                            Architecture_Supports_Unaligned_Float32_Reads = true;
                        }
                    }
                    catch (NullReferenceException)
                    {
                        Architecture_Supports_Unaligned_Float32_Reads = false;
                    }

                    if (Architecture_Supports_Unaligned_Float32_Reads)
                    {
                        Console.WriteLine("Odin Serializer detected whitelisted runtime platform " + platform + " and memory read test succeeded; enabling all unaligned memory read/writes.");
                        Architecture_Supports_All_Unaligned_ReadWrites = true;
                    }
                    else
                    {
                        Console.WriteLine("Odin Serializer detected whitelisted runtime platform " + platform + " and memory read test failed; disabling all unaligned memory read/writes.");
                    }
                    break;
                default:
                    Architecture_Supports_Unaligned_Float32_Reads = false;
                    Architecture_Supports_All_Unaligned_ReadWrites = false;
                    Console.WriteLine("Odin Serializer detected non-white-listed runtime platform " + platform + "; disabling all unaligned memory read/writes.");
                    break;
            }
        }
    }
}