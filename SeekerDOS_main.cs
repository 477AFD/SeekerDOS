/*

========================================================

SeekerDOS Petuni 
Version 2.1 ALPHA
Made by 477AFD for CodeChum

========================================================

- This source code uses the MIT License. Feel free to study this.

- NOTE: this code is ALPHA and bugs can occur. Executing .sh files require /bin/sh and not from SeekerDOS. 
        (shell file execution will be implemented in SeekerDOS Puniper)
- Additionally, this software is designed for use with CodeChum Playground with its programming language set to C#.

This software is designed to emulate a basic DOS architecture inside CodeChum's playground.
With this, one can look around the UNIX filesystem using its shell, or access advanced tools with /bin/sh.
Also, it can run software designed for both Windows and Linux, inside CodeChum.

FAQ:
Q: Why named "SeekerDOS"?
A: It was named after the seekers in Hide and Seek minigame in The Hive server in Minecraft.

Q: Wait! I have a rather silly plan on defeating CodeChum servers! HAHAHA! Is that right?
A: That is a BIG NO! This software is made to tinker around with the operating system, not use it to drop malware and infect CodeChum servers.
   CodeChum alone has some stiff security measures to prevent using the playground as a target for "criminal seekers".
   And here is a disclaimer: I AM NOT RESPONSIBLE FOR ANY DAMAGES OR ISSUES IN CODECHUM CAUSED BY THE USE OR MISUSE OF THIS SOFTWARE.

Q: It is a DOS. So it can manage disks?
A: Technically yes, as we are accessing the file system directly, run software written for Windows and Linux, and able to
   use disk management tools. But the fact that a DOS is an operating system, and SeekerDOS is just a shell and does not 
   have any OS routine commands.
   
Q: List of codenames?
A: Here it is (tentative):
    Code Name   Version
    Punio       1 (alpha)
    Petuni      2 (alpha)
    Puniper     3 (alpha)
    Pungent     4 (alpha)
    Peach       1
    Kersti      2
    Merlon      3
    Merlee      4
    Merluvlee   5
*/

// // // // // // // BEGIN OF PROGRAM // // // // // // //

using System;                           // Console tools (e.g. Console.Write(string))
using System.IO;                        // File management (e.g. File.WriteAllBytes(string, byte[]))
using System.Diagnostics;               // Executive tools

public static class GbVars
{
    public static bool exit = false; // Exit variable
    public static string currentDir = "/home/guest"; // Sets the directory to /home/guest
    public static bool hiddenFiles = true;
    public static char del = ' ';
    public static string shellDir = "/bin/sh";
}

class Dos
{
    private string[] separateByDelimeter(string w, char s)
    {
        return w.Split(s);
    }
    private string getFileExtension(string w)
    {
        string[] c = w.Split('.')
        return c[c.Length - 1];
    }
    public bool ParseCmd(string w, string cdir)
    {
        string data = ""; bool ww = false;
        string[] cmdstring = this.separateByDelimeter(w, GbVars.del);
        if (cmdstring.Length != 0)
        {
            string arg0 = "";
            string arg1 = "";
            string arg2 = "";
            if (cmdstring.Length >= 1)
            {
                arg0 = cmdstring[0]; 
                if (cmdstring.Length >= 2) arg1 = cmdstring[1]; 
                if (cmdstring.Length >= 3) arg2 = cmdstring[2];
            }
        switch (cmdstring[0])
        {
            case "dir":
            case "ls":
                try
                {
                    bool dh = true;
                    if (arg1 == "--hidden" || arg1 == "-h" || arg1 == "-hidden")
                        dh = false;
                    else
                        dh = GbVars.hiddenFiles;
                    string[] directories = Directory.GetDirectories(cdir);
                    Console.WriteLine("Contents:\n");
                    foreach (var d in System.IO.Directory.GetDirectories(cdir))
                    {
                        var dirName = new DirectoryInfo(d).Name;
                        if (dh && dirName[0] == '.')
                            continue;
                        else
                            Console.WriteLine("[" + dirName + "]");
                    }
                    string[] fs = Directory.GetFiles(cdir);
                    foreach (var f in System.IO.Directory.GetFiles(cdir))
                    {
                        var fileName = new FileInfo(f).Name;
                        if (dh && fileName[0] == '.')
                            continue;
                        else
                            Console.WriteLine(fileName);
                    }
                    Console.WriteLine("");
                } 
                catch (Exception e)
                {
                    Console.WriteLine("Access is denied.");
                }
                break;
            case "read":
            case "rd":
                if (arg1 == "")
                    Console.WriteLine($"Incorrect input.\n\nSyntax: read{GbVars.del}<filename>");
                else {
                try
                {
                    if (arg1[0] == '/')
                        data = File.ReadAllText(arg1);
                    else
                        data = File.ReadAllText($"{cdir}/{arg1}");
                } catch (Exception e) {
                    if (cmdstring[0] != "exit")
                        Console.WriteLine("Incorrect input."); 
                    ww = true;
                }
                if (!ww)
                    if (arg1[0] == '/')
                        Console.WriteLine($"File content of {arg1}:\n\n--- Start of File ---\n{data}\n--- End of File ---");
                    else
                        Console.WriteLine($"File content of {cdir}/{arg1}:\n\n--- Start of File ---\n{data}\n--- End of File ---");
                }
                break;
            case "cp":
            case "copy":
                if (arg1 == "" || arg2 == "")
                    Console.WriteLine($"Incorrect input.\n\nSyntax: copy{GbVars.del}<filename>{GbVars.del}<absolute_filepath>");
                else {
                    byte[] s = new byte[16777216]; // File size limit: 16 MB
                    try
                    {
                        if (arg1[0] == '/')
                            s = File.ReadAllBytes(arg1);
                        else
                        s = File.ReadAllBytes($"{cdir}/{arg1}");
                    } catch (Exception e) {
                        if (cmdstring[0] != "exit")
                            Console.WriteLine($"ERROR: {e.Message}"); 
                        ww = true;
                    }
                    if (!ww)
                    {
                        if (arg2[0] != '/')
                            Console.WriteLine("The second argument must be an absolute file path!");
                        else {
                            try
                            {
                                Console.WriteLine($"Write content of {arg1} to {arg2}");
                                File.WriteAllBytes(arg2, s);
                            } 
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR: file cannot be written.");
                            }
                        }
                    }
                }
                break;
            case "cd":
            case "chdir":
                if (arg1 == "")
                    Console.WriteLine($"Incorrect input.\n\nSyntax: cd{GbVars.del}<folder name/path>");
                else {
                    if (arg1[0] == '/')
                        if (Directory.Exists(arg1))
                            GbVars.currentDir = arg1;
                        else 
                            Console.WriteLine("Invalid directory or access forbidden.");
                    else
                        if (Directory.Exists($"{cdir}/{arg1}"))
                            GbVars.currentDir = Path.GetFullPath($"{cdir}/{arg1}");
                        else 
                            Console.WriteLine("Invalid directory or access forbidden.");
                }
                break;
            case "help":
            case "?":
                Console.WriteLine("dir - List files and folders inside a current folder. Use \"--hidden\" to show hidden files and folders.");
                Console.WriteLine("cd - Change the current directory.");
                Console.WriteLine("read - Read the contents of the file and display it.");
                Console.WriteLine("copy - Copies a file to its absolute destination. Currently unsupported by CodeChum.");
                Console.WriteLine("help - Displays this help.\n");
                Console.WriteLine("To run Windows or UNIX software, just type its name or file path.");
                break;
            case "exit":
            case "quit":
                return true;
            default:
                if (arg0[0] == '/')
                    if (File.Exists(arg0))
                        {
                            Application app = new Application();
                            Console.WriteLine("\n=BEGIN========================================\n\n");
                            string apparg = "";
                            for (int rr = 1; rr < cmdstring.Length; rr++)
                            {
                                apparg += $"{cmdstring[rr]} ";
                            }
                            Console.WriteLine($"\n\n=END==========================================\n\nExit code: {app.Run(arg0, apparg, cdir)}");
                        }
                    else
                        Console.WriteLine("Invalid command or file name.");
                else
                    if (File.Exists($"{cdir}/{arg0}"))
                        {
                            Application app = new Application();
                            Console.WriteLine("\n=BEGIN========================================\n\n");
                            string apparg = "";
                            for (int rr = 1; rr < cmdstring.Length; rr++)
                            {
                                apparg += $"{cmdstring[rr]} ";
                            }
                            Console.WriteLine($"\n\n=END==========================================\n\nExit code: {app.Run($"{cdir}/{arg0}", apparg, cdir)}");
                        }
                    else if File.Exists($"/bin/{arg0}"))
                        {
                            Application app = new Application();
                            Console.WriteLine("\n=BEGIN========================================\n\n");
                            string apparg = "";
                            for (int rr = 1; rr < cmdstring.Length; rr++)
                            {
                                apparg += $"{cmdstring[rr]} ";
                            }
                            Console.WriteLine($"\n\n=END==========================================\n\nExit code: {app.Run($"/bin/{arg0}", apparg, cdir)}");
                        }
                    else
                        Console.WriteLine("Invalid command or file name.");
                break;
            }
        }
        return false;
    }
}

class Application
{
    public int Run(string fileDir, string fileargs, string workingDir)
    {
        Process exe_seeker = new Process(); // Create a process tray for the application
        try {
            exe_seeker.StartInfo.FileName = fileDir; // File name and path of executable (must be an EXE)
            exe_seeker.StartInfo.Arguments = fileargs; // Application arguments
            exe_seeker.StartInfo.WorkingDirectory = workingDir; // Working directory
            exe_seeker.StartInfo.UseShellExecute = true; // Use the current console window (currently required)
            exe_seeker.Start(); // Execute the specified application
            exe_seeker.WaitForExit(); // Wait for the process to stop
            return exe_seeker.ExitCode; // Return an exit code
        } catch (Exception w)
        {
            Console.WriteLine($"The file {fileDir} is not an operable program.");
            return -1; // Return exit code -1
        }
    }
    public string[] GetOSInfo()
    {
        Process OSI_seeker = new Process(); // Create a process tray for the application
        int cmdseq = 2;
        string[] cmdlist = new string[cmdseq];
        string[] arglist = new string[cmdseq];
        string[] outlist = new string[cmdseq];
        cmdlist[0] = @"/bin/uname"; arglist[0] = @"-r";
        cmdlist[1] = @"/bin/grep"; arglist[1] = @"-E 'PRETTY_NAME=' /etc/os-release";
        for (int i = 0; i < cmdseq; i++)
        {
            try {
                OSI_seeker.StartInfo.FileName = cmdlist[i]; // File name and path of executable (must be an EXE)
                OSI_seeker.StartInfo.Arguments = arglist[i]; // Application arguments
                OSI_seeker.StartInfo.WorkingDirectory = "/bin"; // Working directory
                OSI_seeker.StartInfo.UseShellExecute = false; // Petuni wants it, not the console.
                OSI_seeker.StartInfo.RedirectStandardOutput = true; // Use redirection so Petuni can see it.
                OSI_seeker.Start(); // Execute the specified application
                outlist[i] = OSI_seeker.StandardOutput.ReadToEnd(); // Capture information needed for Petuni
                OSI_seeker.WaitForExit(); // Wait for the process to stop
            } catch (Exception w)
            {
                Console.WriteLine("\"A seeker has been detected. Try debugging the source code to eliminate it.\" - Petuni");
                outlist[i] = "Not available";
            }
        }
        return outlist;
    }
}

class TermProc
{
    bool ex = false;
    string input = "";
    public void setup()
    {
        // This is executed once
        Application OSI = new Application();
        string[] p = OSI.GetOSInfo();
        string[] q = p[1].Split('"');
        string PetuniOS = q[1];
        Console.WriteLine($"SeekerDOS Petuni\nRunning on {PetuniOS}, version {p[0]}\nVersion a2.1\n\nType \"help\" for instructions.");
    }
    
    public void loop()
    {
        // This is executed FOREVER until Petuni sets "ex" to true.
        Dos dos = new Dos(); // Creates new class for DOS commands
        do
        {
            input = "";
            // Command:
            Console.Write($"{GbVars.currentDir}> ");
            input = Console.ReadLine();
            if (input != "")
                ex = dos.ParseCmd(input, GbVars.currentDir);
            else
                Console.WriteLine("No command provided. Type \"help\" for instructions.");
        } while (!ex);
    }
}

class Program
{
    // Set variables here!
    [STAThread]
    static void Main(string[] args)
    {
        TermProc Petuni = new TermProc();
        Petuni.setup();
        Petuni.loop();
        Console.WriteLine("Exit program");
    }
}
