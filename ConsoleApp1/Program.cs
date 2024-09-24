using PeNet;

//Task1_Run();
//Task2_Run();
Task3_Run();

void Task1_Run()
{
    var hashFileName = "hash1.dat";
    var directoryPath = "E:\\LEVAN\\HOBBIES";
    string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    string hashFilePath = Path.Combine(Path.GetDirectoryName(executablePath), hashFileName);

    if (!Directory.Exists(directoryPath))
    {
        Console.WriteLine("The specified directory does not exist.\r\n");
        return;
    }

    if (!File.Exists(hashFilePath))
    {
        // Первый запуск: вычисляем хэш-суммы всех файлов и сохраняем их
        Dictionary<string, string> fileHashes = CalculateHashes(directoryPath, hashFileName);
        SaveHashes(hashFilePath, fileHashes);
        Console.WriteLine("Hash amounts are calculated and stored.\r\n");
    }
    else
    {
        // Последующие запуски: проверяем хэш-суммы
        Dictionary<string, string> storedHashes = LoadHashes(hashFilePath);
        Dictionary<string, string> currentHashes = CalculateHashes(directoryPath, hashFileName);

        CheckIntegrity(storedHashes, currentHashes);
    }
}

static Dictionary<string, string> CalculateHashes(string directoryPath, string HashFileName)
{
    Dictionary<string, string> fileHashes = new Dictionary<string, string>();

    foreach (string filePath in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
    {
        if (Path.GetFileName(filePath) == HashFileName)
            continue; // Пропускаем файл с хэш-суммами

        string hash = CalculateHash(filePath);
        fileHashes[filePath] = hash;
    }

    return fileHashes;
}


static void SaveHashes(string hashFilePath, Dictionary<string, string> fileHashes)
{
    using (StreamWriter writer = new StreamWriter(hashFilePath))
    {
        foreach (var entry in fileHashes)
        {
            writer.WriteLine($"{entry.Key};{entry.Value}");
        }
    }
}

static Dictionary<string, string> LoadHashes(string hashFilePath)
{
    Dictionary<string, string> fileHashes = new();

    using (StreamReader reader = new(hashFilePath))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(new[] { ';' }, 2);
            if (parts.Length == 2)
            {
                fileHashes[parts[0]] = parts[1];
            }
        }
    }

    return fileHashes;
}

static void CheckIntegrity(Dictionary<string, string> storedHashes, Dictionary<string, string> currentHashes)
{
    bool integrityOk = true;

    foreach (var entry in storedHashes)
    {
        string filePath = entry.Key;
        string storedHash = entry.Value;

        if (currentHashes.TryGetValue(filePath, out string currentHash))
        {
            if (storedHash != currentHash)
            {
                Console.WriteLine($"File was edited: {filePath}");
                integrityOk = false;
            }
        }
        else
        {
            Console.WriteLine($"File was deleted: {filePath}");
            integrityOk = false;
        }
    }

    foreach (var entry in currentHashes)
    {
        string filePath = entry.Key;
        string currentHash = entry.Value;

        if (!storedHashes.ContainsKey(filePath))
        {
            Console.WriteLine($"New File: {filePath}");
            integrityOk = false;
        }
    }

    if (integrityOk)
    {
        Console.WriteLine("The integrity of the catalog has been confirmed.");
    }
}

void Task2_Run()
{
    const string MagicWord = "MAGICWORD";
    const string HashFileName = "hash2.dat";

    string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
    string hashFilePath = Path.Combine(Path.GetDirectoryName(executablePath), HashFileName);

    if (!File.Exists(hashFilePath))
    {
        // Первый запуск: вычисляем хэш и сохраняем его в файл
        string hash = CalculateHash(executablePath);
        File.WriteAllText(hashFilePath, $"{MagicWord}\n{hash}");
        Console.WriteLine("The hash amount is calculated and stored.");
    }
    else
    {
        // Последующие запуски: проверяем хэш
        string[] lines = File.ReadAllLines(hashFilePath);
        if (lines.Length != 2 || lines[0] != MagicWord)
        {
            Console.WriteLine("The hash sum file is corrupted or does not contain the magic word.");
            return;
        }

        string storedHash = lines[1];
        string currentHash = CalculateHash(executablePath);

        if (storedHash == currentHash)
        {
            Console.WriteLine("The integrity of the executable file has been confirmed.");
        }
        else
        {
            Console.WriteLine("A change has been detected in the executable file!");
        }
    }
}

static string CalculateHash(string filePath)
{
    using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read))
    {
        ushort hash = 0;
        byte[] buffer = new byte[2];

        while (fs.Read(buffer, 0, buffer.Length) > 0)
        {
            ushort segment = BitConverter.ToUInt16(buffer, 0);
            hash ^= segment;
        }

        // Если файл нечетный, дополняем нулями
        if (fs.Position % 2 != 0)
        {
            byte lastByte = (byte)fs.ReadByte();
            hash ^= lastByte;
        }

        return hash.ToString("X4"); // Возвращаем хэш в шестнадцатеричном формате
    }
}

void Task3_Run()
{
    string directoryPath = @"C:\Users\hirutsu\Desktop\dmde-free-2.6.0.522-win32-gui";

    List<string> networkFunctions =[ "DeleteIPAddress", "FreeMibTable", "GetAdaptersAddresses", "GetAnycastIpAddressEntry",
            "GetAnycastIpAddressTable", "GetBestRoute2", "GetHostNameW", "GetIpAddrTable",
            "GetIpStatisticsEx", "GetUnicastIpAddressTable", "IcmpCloseHandle", "IcmpCreateFile",
            "IcmpSendEcho", "MultinetGetConnectionPerformance", "MultinetGetConnectionPerformanceW",
            "NetAlertRaise", "NetAlertRaiseEx", "NetApiBufferAllocate", "NetApiBufferFree",
            "NetApiBufferReallocate", "NetApiBufferSize", "NetFreeAadJoinInformation",
            "NetGetAadJoinInformation", "NetAddAlternateComputerName", "NetCreateProvisioningPackage",
            "NetEnumerateComputerNames", "NetGetJoinableOUs", "NetGetJoinInformation",
            "NetJoinDomain", "NetProvisionComputerAccount", "NetRemoveAlternateComputerName",
            "NetRenameMachineInDomain", "NetRequestOfflineDomainJoin", "NetRequestProvisioningPackageInstall",
            "NetSetPrimaryComputerName", "NetUnjoinDomain", "NetValidateName", "NetGetAnyDCName",
            "NetGetDCName", "NetGetDisplayInformationIndex", "NetQueryDisplayInformation",
            "NetGroupAdd", "NetGroupAddUser", "NetGroupDel", "NetGroupDelUser", "NetGroupEnum",
            "NetGroupGetInfo", "NetGroupGetUsers", "NetGroupSetInfo", "NetGroupSetUsers",
            "NetLocalGroupAdd", "NetLocalGroupAddMembers", "NetLocalGroupDel", "NetLocalGroupDelMembers",
            "NetLocalGroupEnum", "NetLocalGroupGetInfo", "NetLocalGroupGetMembers", "NetLocalGroupSetInfo",
            "NetLocalGroupSetMembers", "NetMessageBufferSend", "NetMessageNameAdd", "NetMessageNameDel",
            "NetMessageNameEnum", "NetMessageNameGetInfo", "NetFileClose", "NetFileEnum", "NetFileGetInfo",
            "NetRemoteComputerSupports", "NetRemoteTOD", "NetScheduleJobAdd", "NetScheduleJobDel",
            "NetScheduleJobEnum", "NetScheduleJobGetInfo", "GetNetScheduleAccountInformation",
            "SetNetScheduleAccountInformation", "NetServerDiskEnum", "NetServerEnum", "NetServerGetInfo",
            "NetServerSetInfo", "NetServerComputerNameAdd", "NetServerComputerNameDel",
            "NetServerTransportAdd", "NetServerTransportAddEx", "NetServerTransportDel",
            "NetServerTransportEnum", "NetWkstaTransportEnum", "NetUseAdd", "NetUseDel", "NetUseEnum",
            "NetUseGetInfo", "NetUserAdd", "NetUserChangePassword", "NetUserDel", "NetUserEnum",
            "NetUserGetGroups", "NetUserGetInfo", "NetUserGetLocalGroups", "NetUserSetGroups",
            "NetUserSetInfo", "NetUserModalsGet", "NetUserModalsSet", "NetValidatePasswordPolicyFree",
            "NetValidatePasswordPolicy", "NetWkstaGetInfo", "NetWkstaSetInfo", "NetWkstaUserEnum",
            "NetWkstaUserGetInfo", "NetWkstaUserSetInfo", "NetAccessAdd", "NetAccessCheck",
            "NetAccessDel", "NetAccessEnum", "NetAccessGetInfo", "NetAccessGetUserPerms",
            "NetAccessSetInfo", "NetAuditClear", "NetAuditRead", "NetAuditWrite", "NetConfigGet",
            "NetConfigGetAll", "NetConfigSet", "NetErrorLogClear", "NetErrorLogRead", "NetErrorLogWrite",
            "NetLocalGroupAddMember", "NetLocalGroupDelMember", "NetServiceControl", "NetServiceEnum",
            "NetServiceGetInfo", "NetServiceInstall", "NetWkstaTransportAdd", "NetWkstaTransportDel",
            "NetpwNameValidate", "NetapipBufferAllocate", "NetpwPathType", "NetApiBufferFree",
            "NetApiBufferAllocate", "NetApiBufferReallocate", "WNetAddConnection2", "WNetAddConnection2W",
            "WNetAddConnection3", "WNetAddConnection3W", "WNetCancelConnection", "WNetCancelConnectionW",
            "WNetCancelConnection2", "WNetCancelConnection2W", "WNetCloseEnum", "WNetCloseEnumW",
            "WNetConnectionDialog", "WNetConnectionDialogW", "WNetConnectionDialog1", "WNetConnectionDialog1W",
            "WNetDisconnectDialog", "WNetDisconnectDialogW", "WNetDisconnectDialog1", "WNetDisconnectDialog1W",
            "WNetEnumResource", "WNetEnumResourceW", "WNetGetConnection", "WNetGetConnectionW",
            "WNetGetLastError", "WNetGetLastErrorW", "WNetGetNetworkInformation", "WNetGetNetworkInformationW",
            "WNetGetProviderName", "WNetGetProviderNameW", "WNetGetResourceInformation", "WNetGetResourceInformationW",
            "WNetGetResourceParent", "WNetGetResourceParentW", "WNetGetUniversalName", "WNetGetUniversalNameW",
            "WNetGetUser", "WNetGetUserW", "WNetOpenEnum", "WNetOpenEnumW", "WNetRestoreConnectionW",
            "WNetUseConnection", "WNetUseConnectionW", "RegQueryValueExA"];

    List<string> exts = ["*.acm", "*.ax", "*.cpl", "*.dll", "*.drv", "*.efi", "*.exe", "*.mui", "*.ocx", "*.scr", "*.sys", "*.tsp", 
        "*.mun", "*.ACM", "*.AX", "*.CPL", "*.DLL", "*.DRV", "*.EFI", "*.EXE", "*.MUI", "*.OCX", "*.SCR", "*.SYS", "*.TSP", "*.MUN"];

    foreach (var ext in exts)
    {
        foreach (var filePath in Directory.GetFiles(directoryPath, ext))
        {
            Console.WriteLine($"Analyzing file: {filePath}");
            AnalyzePEFile(filePath, networkFunctions);
        }
    }
}

static void AnalyzePEFile(string filePath, List<string> networkFunctions)
{
    try
    {
        var peFile = new PeFile(filePath);
        var importFunctions = peFile.ImportedFunctions;

        foreach (var importFunction in importFunctions)
        {
            if (networkFunctions.Contains(importFunction.Name))
            {
                Console.WriteLine($"Found network function: {importFunction.Name} in file: {filePath}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error analyzing file {filePath}: {ex.Message}");
    }
}

